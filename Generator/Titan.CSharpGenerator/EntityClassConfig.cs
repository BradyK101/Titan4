using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.CSharpGenerator
{
    public class EntityClassConfig
    {
        public string OutputFileName { get; set; }
        public bool WcfEnabled { get; set; }
        public string NameSpace { get; set; }
        public bool UseAttribute { get; set; }
        public bool UseValidate { get; set; }

        //public bool SupportMySql { get; set; }
        //public bool SupportSQLite { get; set; }
        //public bool SupportSqlServer { get; set; }
        //public bool SupportOracle { get; set; }
    }
}
