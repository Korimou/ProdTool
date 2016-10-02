using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductivityTool
{
    public partial class Form1 : Form
    {
        private List<IniEntry> m_Rules = new List<IniEntry>();
        private CursorMonitor m_Monitor;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                LoadFile(ofd.FileName);
            }
        }

        private void LoadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;
            if (!File.Exists(fileName)) return;

            string[] fileContents = File.ReadAllLines(fileName);
            IniParser iniParser = new IniParser();

            iniParser.Parse(fileContents);

            lstRules.Items.Clear();
            foreach (var section in iniParser.Sections)
                lstRules.Items.Add(section.Values.FirstOrDefault(t => t.Item1.ToLower().Equals("name")).Item2);

            m_Rules.AddRange(iniParser.Sections);
        }

        private void lstRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRules.SelectedIndex == -1)
            {
                grpRules.Enabled = false;
                return;
            }

            SetActiveRule(lstRules.SelectedIndex);
            grpRules.Enabled = true;
        }

        private void SetActiveRule(int index)
        {
            txtName.Text = m_Rules[index]["Name"].FirstOrDefault()?.Item2;
            txtWindowTitle.Text = m_Rules[index]["WindowTitle"].FirstOrDefault()?.Item2;
            txtProcessName.Text = m_Rules[index]["ProcessName"].FirstOrDefault()?.Item2;
            cmbComparisonType.SelectedItem = m_Rules[index]["ComparisonType"].FirstOrDefault()?.Item2;
            cmbActivationMethod.SelectedItem = m_Rules[index]["ActivationMethod"].FirstOrDefault()?.Item2;
            cmbSearchParents.SelectedItem = m_Rules[index]["SearchParents"].FirstOrDefault()?.Item2;

            int duration = 0;
            int.TryParse(m_Rules[index]["Duration"].FirstOrDefault()?.Item2, out duration);
            nudDuration.Value = duration;

            // Change control availability for process mode or not.
            bool isProcessMode = String.IsNullOrWhiteSpace(txtWindowTitle.Text);
            
            // Not available in process mode.
            txtWindowTitle.Enabled = !isProcessMode;
            cmbSearchParents.Enabled = !isProcessMode;

            // Available in process mode.
            txtProcessName.Enabled = isProcessMode;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (lstRules.Items.Count == 0)
            {
                MessageBox.Show("You must load some rules first!");
                return;
            }

            if(m_Monitor != null)
            {
                // stop the current monitor.
            }

            m_Monitor = new CursorMonitor();
            m_Monitor.Rules = ConvertTuplesToRules();
            m_Monitor.BeginMonitoring();
        }

        private List<CursorRule> ConvertTuplesToRules()
        {
            if (m_Rules.Count == 0) return null;
            List<CursorRule> rules = new List<CursorRule>();

            foreach (var ruleTuple in m_Rules)
            {
                int duration = 1;
                int.TryParse(ruleTuple["Duration"].FirstOrDefault()?.Item2, out duration);

                CursorRule rule = new CursorRule()
                {
                    Name = ruleTuple["Name"].FirstOrDefault()?.Item2,
                    WindowTitle = ruleTuple["WindowTitle"].FirstOrDefault()?.Item2,
                    ProcessName = ruleTuple["ProcessName"].FirstOrDefault()?.Item2,
                    ComparisonType = ToEnum<ComparisonType>(ruleTuple["ComparisonType"].FirstOrDefault()?.Item2, ComparisonType.Substring),
                    ActivationMethod = ToEnum<ActivationMethod>(ruleTuple["ActivationMethod"].FirstOrDefault()?.Item2, ActivationMethod.Activate),
                    ParentSearchMode = ToEnum<ParentSearchMode>(ruleTuple["SearchParents"].FirstOrDefault()?.Item2, ParentSearchMode.Any),
                    Duration = duration
                };
                rules.Add(rule);
            }

            return rules;
        }

        private T ToEnum<T>(string value, T defaultValue) where T : struct // NonNullables must be structs!
        {
            T result;
            if (Enum.TryParse<T>(value, out result))
            {
                return result;
            }
            return defaultValue;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Monitor.StopMonitoring();
        }
    }
}