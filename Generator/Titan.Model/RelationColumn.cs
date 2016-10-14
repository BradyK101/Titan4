using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model
{
    public class RelationColumn
    {
        public RelationColumn(string localColumnName, string foreignColumnName)
        {
            this.LocalColumnName = localColumnName;
            this.ForeignColumnName = foreignColumnName;
        }

        public string LocalColumnName { get; set; }
        public string ForeignColumnName { get; set; }
    }
}
