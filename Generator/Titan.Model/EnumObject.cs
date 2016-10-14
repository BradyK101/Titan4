using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model
{
    public class EnumObject
    {
        public string Name { get; set; }
        public string Comment { get; set; }

        public List<EnumMember> EnumMembers { get; set; }

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
                    return this.Name;
                }
            }
            set { _DisplayName = value; }
        }
    }
}
