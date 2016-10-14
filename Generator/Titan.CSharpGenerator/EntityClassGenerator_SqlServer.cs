using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Titan.Model;
using Titan.SchemaModel;
using System.IO;

namespace Titan.CSharpGenerator
{
    public class EntityClassGenerator_SqlServer : EntityClassGenerator
    {


        public override string GetPropertyType(Column column)
        {
            string propertyType = "int";
            SqlType sqlType = GetSqlType(column);

            switch (sqlType)
            {
                case SqlType.Blob:
                    propertyType = "byte[]";
                    break;
                case SqlType.Boolean:
                    propertyType = column.AllowNull ? "bool?" : "bool";
                    break;
                case SqlType.Byte:
                    propertyType = column.AllowNull ? "byte?" : "byte";
                    break;
                case SqlType.Clob:
                    propertyType = "string";
                    break;
                case SqlType.DateTime:
                    propertyType = column.AllowNull ? "DateTime?" : "DateTime";
                    break;
                case SqlType.Decimal:
                    propertyType = column.AllowNull ? "decimal?" : "decimal";
                    break;
                case SqlType.Double:
                    propertyType = column.AllowNull ? "double?" : "double";
                    break;
                case SqlType.Float:
                    propertyType = column.AllowNull ? "float?" : "float";
                    break;
                case SqlType.Integer:
                    propertyType = column.AllowNull ? "int?" : "int";
                    break;
                case SqlType.Long:
                    propertyType = column.AllowNull ? "long?" : "long";
                    break;
                case SqlType.NClob:
                case SqlType.NVarChar:
                    propertyType = "string";
                    break;
                case SqlType.Short:
                    propertyType = column.AllowNull ? "short?" : "short";
                    break;
                case SqlType.VarChar:
                    propertyType = "string";
                    break;
                case SqlType.Char:
                    propertyType = "Guid";
                    break;
                default:
                    throw new Exception(string.Format("未转化的SqlType:{0}", sqlType));
            }
            return propertyType;
        }
        public override SqlType GetSqlType(Column column)
        {
            string columnType = column.ColumnType;
            int pos = column.ColumnType.IndexOf('(');
            if (pos > 0)
            {
                columnType = columnType.Substring(0, pos);
            }


            SqlType sqlType = SqlType.Integer;
            switch (columnType)
            {
                case "bit":
                    sqlType = SqlType.Boolean;
                    break;
                case "int":
                    sqlType = SqlType.Integer;
                    break;
                case "bigint":
                    sqlType = SqlType.Long;
                    break;
                case "decimal":
                    sqlType = SqlType.Decimal;
                    break;
                //case "number":
                //    if (column.ColumnTypeLength <= 1)
                //    {
                //        sqlType = SqlType.Boolean;
                //    }
                //    else if (column.ColumnTypeLength <= 3)
                //    {
                //        sqlType = SqlType.Byte;
                //    }
                //    else if (column.ColumnTypeLength <= 5)
                //    {
                //        sqlType = SqlType.Short;
                //    }
                //    else if (column.ColumnTypeLength <= 10)
                //    {
                //        sqlType = SqlType.Integer;
                //    }
                //    else if (column.ColumnTypeLength <= 20)
                //    {
                //        if (column.Precision == 0)
                //        {
                //            sqlType = SqlType.Long;
                //        }
                //        else
                //        {
                //            sqlType = SqlType.Float;
                //        }
                //    }
                //    else if (column.ColumnTypeLength <= 38)
                //    {
                //        sqlType = SqlType.Double;
                //    }
                //    break;
                case "nvarchar":
                case "nvarchar2":
                    sqlType = SqlType.NVarChar;
                    break;
                case "varchar":
                case "varchar2":
                    sqlType = SqlType.VarChar;
                    break;
                case "date":
                case "datetime":
                    sqlType = SqlType.DateTime;
                    break;
                case "timestamp":
                    sqlType = SqlType.DateTime;
                    break;
                case "ntext":
                    sqlType = SqlType.NClob;
                    break;
                case "text":
                    sqlType = SqlType.Clob;
                    break;
                case "image":
                    sqlType = SqlType.Blob;
                    break;
                case "float":
                    sqlType = SqlType.Float;
                    break;
                case "char":
                    sqlType = SqlType.Char;
                    break;
                default:
                    throw new Exception(string.Format("未识别的字段类型:{0}", columnType));
            }
            return sqlType;
        }

        public override string SubCreateAttribute_Table(Table table)
        {
            return "";
        }

        public override string SubCreateAttribute_Column(Column column)
        {
            StringBuilder sb = new StringBuilder();
            SqlType sqlType = GetSqlType(column);
            if (column.Identity)
            {
                sb.AppendFormat(@" 
        [SqlServerColumn(IsIdentity=true)]", column.SequenceName);
            }
            return sb.ToString();
        }

        public override string SubCreateAttribute_Relation(Relation relation)
        {
            return "";
        }



    }
}
