using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Titan.Model
{
    public class Column
    {
        public string ColumnName { get; set; }
        public bool IsKey { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 为空表示无原文索引
        /// </summary>
        public string FullTextSearch { get; set; }


        /// <summary>
        /// 对于SqlServer,MySql,SQLite等，是否自动增长
        /// </summary>
        public bool Identity { get; set; }

        /// <summary>
        /// 对于Oracle的序列名称
        /// </summary>
        public string SequenceName { get; set; }


       

        private bool _AllowNull = true;
        /// <summary>
        /// 
        /// </summary>
        public bool AllowNull
        {
            get { return IsKey ? false : _AllowNull; }
            set { _AllowNull = value; }
        }





        private int _ColumnTypeLength;
        /// <summary>
        /// 字段长度
        /// </summary>
        public int ColumnTypeLength
        {
            get { return _ColumnTypeLength; }
            set { _ColumnTypeLength = value; }
        }


        public int Precision { get; set; } 

        //private int _Length;
        ///// <summary>
        ///// 一般对字符串有效，有时候2字节存放的时候，要除以2才是Length
        ///// </summary>
        //public int Length
        //{
        //    get { return _Length; }
        //    set { _Length = value; }
        //}

 







        
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
                    return this.ColumnName;
                }
            }
            set { _DisplayName = value; }
        }


        public string ControlType { get; set; }


        /// <summary>
        /// 输入提示
        /// </summary>
        public string InputPrompt { get; set; }


        public string Comment { get; set; }
        public string EnumType { get; set; }


        public string CSharpValidate { get; set; }
    }
}
