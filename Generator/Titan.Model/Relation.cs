using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model
{
    public class Relation
    {
        public bool ReferenceValidation { get; set; }
        public string TableName { get; set; }
        public string PropertyName { get; set; }
        public List<RelationColumn> RelationColumns { get; set; }

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
                    return this.PropertyName;
                }
            }
            set { _DisplayName = value; }
        }
    }
}
