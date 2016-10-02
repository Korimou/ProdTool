using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ProductivityTool
{
    internal class CursorMonitor
    {
        private List<CursorRule> m_Rules = new List<CursorRule>();
        private IntPtr m_OriginalCursor;

        private bool m_IsRunning = false;
        private bool m_ContinueRunning = true;
        Thread m_MonitorThread;

        public IEnumerable<CursorRule> Rules
        {
            get
            {
                if (m_IsRunning)
                    throw new InvalidOperationException("Can't touch rules when monitoring.");

                return m_Rules;
            }
            set
            {
                if (m_IsRunning)
                    throw new InvalidOperationException("Can't touch rules when monitoring.");

                m_Rules.Clear();
                m_Rules.AddRange(value);
            }
        }

        public void BeginMonitoring()
        {
            if (m_Rules == null || m_Rules.Count == 0) return;

            if (m_MonitorThread != null)
            {
                if (m_IsRunning || m_MonitorThread.ThreadState == System.Threading.ThreadState.Running)
                    StopMonitoring();
            }

            // cache original cursor.
            m_OriginalCursor = CopyIcon(LoadCursor(IntPtr.Zero, 32512)); // IDC_ARROW = 32512

            m_MonitorThread = new Thread(DoMonitor);
            m_MonitorThread.Start(); // poss race condition between start and actually starting, rules can be changed. take care.
        }

        public void StopMonitoring()
        {
            if (m_IsRunning)
            {
                m_ContinueRunning = false;
                m_MonitorThread.Join();
                ResetCursor(); // Just in case.
            }
        }

        private void DoMonitor()
        {
            m_IsRunning = true;

            int numRulesUsingActiveMode = m_Rules.Where(cr => cr.ActivationMethod == ActivationMethod.Enter).Count();
            int numRulesUsingEnterMode = m_Rules.Count - numRulesUsingActiveMode; // save an enumeration, do it once.

            WindowPollingMode pollMode = WindowPollingMode.None;
            if (numRulesUsingActiveMode > 0)
                pollMode |= WindowPollingMode.Activate;
            if (numRulesUsingEnterMode > 0)
                pollMode |= WindowPollingMode.MouseOver;

            CursorRule currentRule = null;
            WindowInformation previousMouseOverWinInfo = null;
            WindowInformation previousActivatedWinInfo = null;
            DateTime timeRuleWasApplied = DateTime.UtcNow; // just to keep the compiler happy.
            int lastIncrement = -1;
            while (m_ContinueRunning)
            {                 
                WindowInformation winInfoMouseOver = GetWindowInformationMouseOver();
                WindowInformation winInfoActivated = GetWindowInformationActivated();

                // If we don't have a state in either of them, we can't do a proper evaluation.
                if (previousActivatedWinInfo == null || previousMouseOverWinInfo == null)
                {
                    previousMouseOverWinInfo = winInfoMouseOver;
                    previousActivatedWinInfo = winInfoActivated;
                    continue; // no point processing yet, probably first run.
                }

                // now we have all of the information we need, we can evaluate the rules.
                // TODO consider how we handle multiple rules that might match? Currently we just select the first one.
                CursorRule matchingRule = FindMatchingRule(previousMouseOverWinInfo,
                                                           winInfoMouseOver,
                                                           previousActivatedWinInfo,
                                                           winInfoActivated);

                if(currentRule != null && currentRule.Expired)
                {
                    currentRule.Expired = false;
                    currentRule = null;
                    lastIncrement = -1;
                }

                if (matchingRule != null)
                {
                    if (currentRule == null || RulesAreDifferent(currentRule, matchingRule))
                    {
                        currentRule = matchingRule;
                        timeRuleWasApplied = DateTime.UtcNow;
                        lastIncrement = -1;
                    }
                }

                if (currentRule == null || (matchingRule == null && currentRule == null))
                    continue; // No rule fit and we didn't have an active one it seems.

                lastIncrement = UpdateCursor(timeRuleWasApplied, currentRule, lastIncrement);
            }

            m_IsRunning = false;
        }

        private int UpdateCursor(DateTime timeRuleWasApplied, CursorRule rule, int lastIncrement)
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan delta = currentTime.Subtract(timeRuleWasApplied);

            long secondsPerIncrement = rule.Duration / 4;

            if (delta.Seconds > secondsPerIncrement * lastIncrement)
                lastIncrement++;

            if (lastIncrement > 4)
                lastIncrement = 4;

            // If the rule's expired (if it's been complete for the length of it's duration) then just reset it.
            if (lastIncrement == 4 && delta.Seconds >= rule.Duration * 2)
                rule.Expired = true;

            // Update Icon.
            string iconFile = String.Format("aero_arrow{0}.cur", lastIncrement);
            IntPtr hNewCursor = LoadCursorFromFile(iconFile);

            uint defaultNormalCursor = 32512; /*OCR_NORMAL*/
            SetSystemCursor(hNewCursor, defaultNormalCursor);

            return lastIncrement;
        }

        private void ResetCursor()
        {
            uint defaultNormalCursor = 32512; /*OCR_NORMAL*/
            SetSystemCursor(m_OriginalCursor, defaultNormalCursor);
        }

        private bool RulesAreDifferent(CursorRule previousRule, CursorRule matchingRule)
        {
            // compare all the props to make sure we're the same rule, then invert it. 
            // TODO Perhaps a handle check too?
            return !(previousRule.ActivationMethod == matchingRule.ActivationMethod &&
                     previousRule.ComparisonType == matchingRule.ComparisonType &&
                     previousRule.Duration == matchingRule.Duration &&
                     previousRule.Name == matchingRule.Name &&
                     previousRule.ParentSearchMode == matchingRule.ParentSearchMode &&
                     previousRule.ProcessName == matchingRule.ProcessName &&
                     previousRule.WindowTitle == matchingRule.WindowTitle);
        }

        private CursorRule FindMatchingRule(WindowInformation previousMouseOverWinInfo, WindowInformation currentMouseOverWinInfo, WindowInformation previousActivatedWinInfo, WindowInformation currentActivatedWinInfo)
        {
            foreach (CursorRule rule in m_Rules)
            {
                WindowInformation previousState = null;
                WindowInformation currentState = null;

                if (rule.ActivationMethod == ActivationMethod.Enter)
                {
                    previousState = previousMouseOverWinInfo;
                    currentState = currentMouseOverWinInfo;
                }
                else if (rule.ActivationMethod == ActivationMethod.Activate)
                {
                    previousState = previousActivatedWinInfo;
                    currentState = currentActivatedWinInfo;
                }

                if (!WindowsAreTheSame(previousState, currentState))
                {
                    if (DoesRuleMatch(rule, currentState))
                        return rule;
                }
            }

            return null;
        }

        private bool DoesRuleMatch(CursorRule rule, WindowInformation currentState)
        {
            bool matchesRule = false;

            if (rule.ParentSearchMode == ParentSearchMode.None) // Match this window only.
            {
                matchesRule = DoesStringMatchWindow(rule.ComparisonType,
                                                    rule.IsProcessMode ? currentState.ProcessPath : currentState.WindowTitle,
                                                    rule.IsProcessMode ? rule.ProcessName : rule.WindowTitle);
            }
            else if (rule.ParentSearchMode == ParentSearchMode.Single) // Match the string in any single parent window, or the window itself.
            {
                matchesRule |= DoesStringMatchWindow(rule.ComparisonType,
                                                    rule.IsProcessMode ? currentState.ProcessPath : currentState.WindowTitle,
                                                    rule.IsProcessMode ? rule.ProcessName : rule.WindowTitle);

                // The window matched, what about it's parents.
                foreach (string parentTitle in currentState.ParentWindowTexts)
                {
                    if (rule.ComparisonType == ComparisonType.Exact)
                        matchesRule |= parentTitle == rule.WindowTitle;
                    else if (rule.ComparisonType == ComparisonType.Substring)
                        matchesRule |= parentTitle.Contains(rule.WindowTitle);
                }
            }
            else if (rule.ParentSearchMode == ParentSearchMode.Any)
            {
                // TODO figure out a way to match the rule accross all parents. Will likely need to be after the combination logic is written.
                // not implemented right now as we aren't doing the pattern matching stuff! e.g. a AND b, a OR b etc.
            }

            return matchesRule;
        }

        private static bool DoesStringMatchWindow(ComparisonType comparisonType, string windowString, string ruleString)
        {
            bool matchesRule = false;

            if (comparisonType == ComparisonType.Exact)
                matchesRule = windowString.ToLower() == ruleString.ToLower();
            else if (comparisonType == ComparisonType.Substring)
                matchesRule = windowString.ToLower().Contains(ruleString.ToLower());

            return matchesRule;
        }

        private bool WindowsAreTheSame(WindowInformation previousState, WindowInformation currentState)
        {
            if (previousState.WindowTitle != currentState.WindowTitle)
                return false;

            if (previousState.ParentWindowTexts.Count() != currentState.ParentWindowTexts.Count())
                return false;

            for (int i = 0; i < previousState.ParentWindowTexts.Count(); i++)
            {
                if (previousState.ParentWindowTexts[i] != currentState.ParentWindowTexts[i])
                    return false;
            }

            return true;
        }

        private WindowInformation GetWindowInformationActivated()
        {
            WindowInformation windowInformation = new WindowInformation();

            IntPtr hWnd = GetForegroundWindow();

            return GetWindowInformation(hWnd);
        }

        private WindowInformation GetWindowInformationMouseOver()
        {
            Point mouseLocation = new Point();
            if (!GetCursorPos(out mouseLocation)) return null;

            IntPtr hWnd = WindowFromPoint(mouseLocation);
            if (hWnd == IntPtr.Zero) return null;

            return GetWindowInformation(hWnd);
        }

        private WindowInformation GetWindowInformation(IntPtr hWnd)
        {
            WindowInformation windowInformation = new WindowInformation();

            if (hWnd == IntPtr.Zero)
                return windowInformation; // shortcut it, there's no window.

            StringBuilder windowTitle = new StringBuilder();
            windowTitle.Length = 255;
            int numCharsInTitle = GetWindowText(hWnd, windowTitle, 255); // just in case we need it, cache the value

            List<string> parentWindowTitles = new List<string>();
            IntPtr parent = GetAncestor(hWnd, GetAncestorFlags.GetParent);
            while (parent != IntPtr.Zero)
            {
                StringBuilder parentTitle = new StringBuilder();
                parentTitle.Length = 255;
                GetWindowText(parent, parentTitle, 255);
                parentWindowTitles.Add(parentTitle.ToString());

                parent = GetAncestor(parent, GetAncestorFlags.GetParent);
            }

            windowInformation.WindowTitle = windowTitle.ToString();
            windowInformation.ProcessPath = GetProcessNameFromWindow(hWnd);
            windowInformation.ParentWindowTexts = parentWindowTitles.ToArray();

            return windowInformation;
        }

        private string GetProcessNameFromWindow(IntPtr hWnd)
        {
            uint pid;
            GetWindowThreadProcessId(hWnd, out pid);
            IntPtr processHandle = OpenProcess(ProcessAccessFlags.QueryInformation, false, (int)pid);
            StringBuilder processName = new StringBuilder();
            processName.Length = 255;
            GetProcessImageFileName(processHandle, processName, 255);

            return processName.ToString();
        }

        // This is ported from C++ so some of this may be doable in C# but fuck it.
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll")]
        static extern uint GetProcessImageFileName(IntPtr hProcess, [Out] StringBuilder lpImageFileName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr LoadCursorFromFile(string lpFileName);

        [DllImport("user32.dll")]
        static extern bool SetSystemCursor(IntPtr hcur, uint id);

        [DllImport("user32.dll")]
        static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        enum GetAncestorFlags
        {
            /// <summary>
            /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function.
            /// </summary>
            GetParent = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent. 
            /// </summary>
            GetRootOwner = 3
        }

    }
}