using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model
{
    public class Table
    {
        public bool IsView { get; set; }

        public string TableName { get; set; }

        public List<Column> Columns { get; set; }

        public List<Relation> Relations { get; set; }
        public string Comment { get; set; }

        private string _DisplayName;
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (this._DisplayName != null)
                {
                    return this._DisplayName;
                }
                else
                {
                    return this.TableName;
                }
            }
            set { _DisplayName = value; }
        }



    }
}
