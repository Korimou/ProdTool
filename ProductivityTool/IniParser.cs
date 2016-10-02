using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTool
{
    class IniParser
    {
        List<IniEntry> m_IniEntries;
        IniEntry m_CurrentEntry;

        public void Parse(string[] fileContents)
        {
            m_IniEntries = new List<IniEntry>();

            foreach(string line in fileContents)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                IniSection currentSection = DetermineSectionType(line);
                HandleSection(line, currentSection);
            }

            if (m_CurrentEntry != null) m_IniEntries.Add(m_CurrentEntry); // one left over...
        }

        public IEnumerable<IniEntry> Sections
        {
            get
            {
                return m_IniEntries;
            }
        }

        private void HandleSection(string line, IniSection currentSection)
        {
            if (currentSection == IniSection.HEADER)
            {
                string headerName = line.TrimStart('[').TrimEnd(']');
                CreateNewHeader(headerName);
            }
            else if(currentSection == IniSection.VALUE_ENTRY)
            {
                string[] entryParts = line.Split(new []{ "=" }, 2, StringSplitOptions.None);
                if (entryParts.Length == 2)
                    AddNewValueEntry(entryParts[0], entryParts[1]);
                else
                    AddNewValueEntry(line, String.Empty);
            }
        }

        private void AddNewValueEntry(string entryName, string entryValue)
        {
            if (m_CurrentEntry == null) return;

            m_CurrentEntry.Values.Add(new Tuple<string, string>(entryName.Trim(), entryValue.Trim()));
        }

        private void CreateNewHeader(string headerName)
        {
            if(m_CurrentEntry != null)
            {
                m_IniEntries.Add(m_CurrentEntry);
            }

            m_CurrentEntry = new IniEntry();
            m_CurrentEntry.Header = headerName;
        }

        private IniSection DetermineSectionType(string line)
        {
            if (IsHeader(line))
                return IniSection.HEADER;
            if (IsValueEntry(line))
                return IniSection.VALUE_ENTRY;
            if (IsComment(line))
                return IniSection.COMMENT;

            return IniSection.NOT_SET;
        }

        private bool IsComment(string line)
        {
            return (line.Trim().StartsWith("#"));
        }

        private bool IsValueEntry(string line)
        {
            return line.Contains('=');
        }

        private bool IsHeader(string str)
        {
            string compareStr = str.Trim();
            return (compareStr.StartsWith("[") && compareStr.EndsWith("]"));
        }

        enum IniSection
        {
            NOT_SET,
            HEADER,
            VALUE_ENTRY,
            COMMENT
        }
    }   
}
