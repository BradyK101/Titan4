using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Model;
using Titan.SchemaModel;

namespace Titan.NodeJsGenerator
{
    public class ORM_ModelGenerator_MySql
    {
        public void Create(Project project, string outputFileName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("module.exports = function (orm,Sequelize) {\r\n");

            foreach (Table table in project.Tables)
            {
                string description = table.Comment;
                if (!String.IsNullOrEmpty(description))
                {
                    description = description.Replace("\r\n", "\r\n    // ");
                }
                sb.AppendFormat("    //#region {0} \r\n", table.TableName);
                sb.AppendFormat("    var {0}=orm.define(\"{0}\"", table.TableName);
                sb.Append(",{ \r\n");

                #region colun
                var i = 0;
                foreach (Column column in table.Columns)
                {
                    if (i > 0)
                    {
                        sb.Append(",\r\n");
                    }
                    sb.AppendFormat("        {0}:", Space(column.ColumnName, 30));
                    sb.Append("{");
                    GenertateType(column, sb);
                    if (!column.AllowNull)
                    {
                        sb.Append(",allowNull: false ");
                    }
                    if (column.IsKey)
                    {
                        sb.Append(",primaryKey: true ");
                    }
                    if (column.Identity)
                    {
                        sb.Append(",autoIncrement: true ");
                    }
                    if (!string.IsNullOrEmpty(column.DisplayName))
                    {
                        sb.AppendFormat(",comment: \"{0}\"", column.DisplayName);
                    }

                    #region validate
                    if (!column.IsKey)
                    {
                        sb.Append(",validate:{");
                        if (!column.AllowNull)
                        {
                            sb.Append("notEmpty:{");
                            sb.AppendFormat("msg: \"{0}必填\"", column.DisplayName);
                            sb.Append("}");
                        }

                        if (column.ColumnTypeLength > 0)
                        {
                            if (!column.AllowNull)
                            {
                                sb.Append(",");
                            }
                            sb.Append("len:{");
                            sb.AppendFormat("args:[{0},{1}]", 0, column.ColumnTypeLength);
                            sb.AppendFormat(",msg:\"{0}最大长度为{1}\"", column.DisplayName, column.ColumnTypeLength);
                            sb.Append("}");
                        }
                        sb.Append("}");
                    }
                    #endregion

                    sb.Append("}");
                    i++;
                }
                sb.Append("\r\n    }");
                #endregion

                #region 验证
                sb.Append(",{ \r\n");
                sb.AppendFormat("        tableName:\"{0}\"\r\n", table.TableName);
                sb.Append("    }");
                #endregion

                sb.Append(");\r\n");

                

                sb.Append("    //#endregion \r\n\r\n\r\n");

            }

            foreach (Table table in project.Tables)
            {
                #region 外键关联
                if (table.Relations.Count > 0)
                {
                    foreach (Relation relation in table.Relations)
                    {
                        foreach (RelationColumn relationColumn in relation.RelationColumns)
                        {
                            sb.AppendFormat("    {0}.belongsTo({1},", table.TableName, relation.TableName);
                            sb.Append("{ ");
                            sb.AppendFormat("foreignKey: '{1}'", relation.PropertyName, relationColumn.LocalColumnName);
                            sb.Append("}");
                            sb.Append(");\r\n");
                        }
                    }
                }
                #endregion
            }

            sb.Append("};\r\n");

            TextFile.Write(outputFileName, sb.ToString(), Encoding.UTF8);
        }

        private string Space(string str, long length) {
            if (length > str.Length)
            {
                string s = "";
                for (var i = 0; i < length - str.Length; i++)
                {
                    s += " ";
                }
                return str + s;
            }
            return str;
        }

        private void GenertateType(Column column,StringBuilder sb) {
            string columnType = column.ColumnType;
            int pos = column.ColumnType.IndexOf('(');
            if (pos > 0)
            {
                columnType = columnType.Substring(0, pos);
            }
            switch (columnType)
            {
                case "bit":
                case "bool":
                case "boolean":
                    sb.AppendFormat("type:{0}", "Sequelize.BOOLEAN");
                    break;
                case "int":
                    sb.AppendFormat("type:{0}", "Sequelize.INTEGER ");
                    break;
                case "bigint":
                    sb.AppendFormat("type:{0}", "Sequelize.BIGINT");
                    break;
                case "decimal":
                    sb.AppendFormat("type:{0}", "Sequelize.DECIMAL");
                    break;
                case "float":
                    sb.AppendFormat("type:{0}", "Sequelize.FLOAT");
                    break;
                case "char":
                case "nvarchar":
                case "nvarchar2":
                case "varchar":
                case "varchar2":
                case "text":
                case "ntext":
                    sb.AppendFormat("type:{0}", "Sequelize.STRING");
                    if (column.ColumnTypeLength > 0) {
                        sb.AppendFormat("()",column.ColumnTypeLength);
                    }
                    break;
                case "datetime":
                    sb.AppendFormat("type:{0},defaultValue: Sequelize.NOW", "Sequelize.DATE");
                    break;
                case "timestamp":
                    sb.AppendFormat("type:{0}", "Sequelize.DATE");
                    break;
                case "image":
                    sb.AppendFormat("type:{0}", "Sequelize.STRING.BINARY");
                    break;
                default:
                    throw new Exception(string.Format("未识别的字段类型:{0}", columnType));
            }
        }
    }
}
