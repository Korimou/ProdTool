using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTool
{
    class IniEntry
    {
        public IniEntry()
        {
            Values = new List<Tuple<string, string>>();
        }

        public string Header { get; set; }
        public List<Tuple<string, string>> Values { get; set; }
        public Tuple<string, string>[] this[string key]
        {
            get
            {
                return Values.Where(t => t.Item1.ToLower().Equals(key.ToLower())).ToArray();
            }
        }
    }
}
