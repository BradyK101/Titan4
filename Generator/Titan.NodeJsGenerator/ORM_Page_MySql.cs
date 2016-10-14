using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Model;
using System.IO;
using Titan.SchemaModel;

namespace Titan.NodeJsGenerator
{
    public class ORM_Page_MySql
    {
        public string CreateController(Table table)
        {
            string tableLName = table.TableName.ToLower();
            Column keyColumn = table.Columns.Find(p => p.IsKey);
            if (keyColumn == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("var common = require('../../lib/common');\r\n");
            sb.Append("var validate = require('../../lib/validate');\r\n");

            sb.Append("exports.before=function(req,res,next){\r\n");
            sb.AppendFormat("    common.setModel(req,'{0}');\r\n", table.TableName);
            sb.AppendFormat("    var {0}Id = req.params.{0}id;\r\n", tableLName);
            sb.AppendFormat("    if (!{0}Id) ", tableLName);
            sb.Append("{\r\n");
            sb.Append("        return next();\r\n");
            sb.Append("    }\r\n");
            sb.Append("    process.nextTick(function () {\r\n");
            sb.AppendFormat("        req.models.{0}.findById({1}Id).then(function ({1}) ",table.TableName,tableLName);
            sb.Append("{\r\n");
            //sb.AppendFormat("            if (!{0}) return next('route');\r\n", tableLName);
            sb.AppendFormat("            req.{0} = {0};\r\n", tableLName);
            sb.Append("            next();\r\n");
            sb.Append("        }).catch(function (error) {\r\n");
            sb.Append("            return res.render(errorPath, {\r\n");
            sb.Append("                message: error.message,\r\n");
            sb.Append("                error: error\r\n");
            sb.Append("            });\r\n");
            sb.Append("        });\r\n");
            sb.Append("    });\r\n");
            sb.Append("}\r\n\r\n\r\n");

            #region 列表
            sb.Append("//#region 列表\r\n");
            sb.Append("exports.index = function (req, res, next) {\r\n");
            sb.Append("    var pageIndex= 0;\r\n");
            sb.Append("    if (req.query.p) {\r\n");
            sb.Append("        pageIndex = parseInt(req.query.p);\r\n");
            sb.Append("    }\r\n");
            sb.Append("    if (pageIndex < 0) { \r\n");
            sb.Append("        pageIndex = 0;\r\n");
            sb.Append("    }\r\n");
            sb.Append("    var query = {};\r\n");
            sb.Append("    query.where = {};\r\n");
            sb.Append("    query.attributes = [");
            int i = 0;
            foreach (Column column in table.Columns)
            {
                if (i > 0)
                {
                    sb.Append(",\r\n                        ");

                }
                sb.AppendFormat("'{0}'", column.ColumnName);
                i++;
            }
            sb.Append("];\r\n");
            sb.Append("    query.offset = pageIndex * common.pageSize;\r\n");
            sb.Append("    query.limit = common.pageSize;\r\n");

            sb.AppendFormat("    req.models.{0}.findAndCountAll(query).then(function ({1}s) {2}", table.TableName, tableLName, "{\r\n");
            sb.Append("        res.render('index', { \r\n");
            sb.AppendFormat("            {0}s: {0}s.rows,\r\n", tableLName);
            sb.Append("            pageSize: common.pageSize,\r\n");
            sb.Append("            pageIndex: pageIndex,\r\n");
            sb.AppendFormat("            totalCount: {0}s.count\r\n",tableLName);
            sb.Append("        });\r\n");
            sb.Append("    }).catch(function (error) {\r\n");
            sb.Append("            return res.render(errorPath, {\r\n");
            sb.Append("            message: error.message,\r\n");
            sb.Append("            error: error\r\n");
            sb.Append("        });\r\n");
            sb.Append("    });\r\n");
            sb.Append("}\r\n");
            sb.Append("//#endregion\r\n\r\n\r\n");
            #endregion

            #region 编辑
            sb.Append("//#region 编辑\r\n");
            sb.Append("exports.edit = function (req, res, next) {\r\n");
            sb.Append("    res.render('edit', {\r\n");
            sb.AppendFormat("        title:'编辑{0}',\r\n",table.DisplayName);
            sb.AppendFormat("        {0}:req.{0}\r\n", tableLName);
            sb.Append("    });\r\n");
            sb.Append("");
            sb.Append("}\r\n");
            sb.Append("//#endregion\r\n\r\n\r\n");
            #endregion

            #region 提交编辑
            sb.Append("//#region 提交修改\r\n");
            sb.Append("exports.update = function (req, res, next) {\r\n");
            sb.AppendFormat("    var {0}Id = req.params.{0}id;\r\n", tableLName);
            sb.AppendFormat("    var model = req.model;\r\n", tableLName);
            foreach (Column column in table.Columns)
            {
                if (!column.AllowNull)
                {
                    sb.AppendFormat("    validate(req, req.modelType, '{0}');\r\n", column.ColumnName);
                }
            }
            sb.Append("    if (!req.IsValid)\r\n");
            sb.Append("    {\r\n");
            sb.Append("        return res.render('edit', {\r\n");
            sb.AppendFormat("            {0}: model,\r\n", tableLName);
            sb.Append("            formError:req.formError\r\n");
            sb.Append("        });\r\n");
            sb.Append("    }\r\n");

            sb.AppendFormat("    if (!{0}Id || {0}Id <= 0) {1}\r\n", tableLName, "{");
            #region 添加
            sb.Append("        //#region 添加\r\n");
            sb.AppendFormat("        req.models.{2}.findOne({0} where: {0} {3}: model.{3} {1},attributes: ['{3}'] {1}).then(function (result) {0}//update code\r\n", "{", "}", table.TableName, keyColumn.ColumnName);
            sb.Append("            return result;\r\n");
            sb.Append("        }).then(function (result) {\r\n");
            sb.Append("            if (result) {\r\n");
            sb.AppendFormat("                req.formError[\"{0}\"] = \"{1}不允许重复\";\r\n", keyColumn.ColumnName, keyColumn.DisplayName);
            sb.Append("                return res.render('edit', {\r\n");
            sb.AppendFormat("                    {0}: model,\r\n", tableLName);
            sb.AppendFormat("                    formError: req.formError//update code\r\n", keyColumn.ColumnName, keyColumn.DisplayName);
            sb.Append("                });\r\n");
            sb.Append("            }\r\n\r\n");
            sb.Append("            //#region 赋值\r\n");
            sb.Append("            //add code\r\n");
            sb.Append("            //#endregion\r\n");
            sb.AppendFormat("            req.models.{1}.create(model).then(function () {0}\r\n", "{", table.TableName);
            sb.AppendFormat("                    common.iframeRender(res, '/{0}/edit/', '/{0}');\r\n",tableLName);
            sb.Append("            }).catch(function (error) {\r\n");
            sb.Append("                return res.render('edit', {\r\n");
            sb.AppendFormat("                    {0}: model,\r\n", tableLName);
            sb.Append("                    pageError: error.message\r\n");
            sb.Append("                });\r\n");
            sb.Append("            });\r\n");
            sb.Append("        }).catch(function (error) {\r\n");
            sb.Append("            return res.render('edit', {\r\n");
            sb.AppendFormat("                {0}: model,\r\n", tableLName);
            sb.Append("                pageError: error.message\r\n");
            sb.Append("            });\r\n");
            sb.Append("        });\r\n");
            sb.Append("        //#endregion\r\n");
            #endregion
            sb.Append("    } else {\r\n");
            #region 修改
            sb.Append("        //#region 修改\r\n");
            sb.AppendFormat("        req.models.{2}.findOne({0} where: {0} {3}: model.{3}, {3}: {0} $ne: {4}Id {1} {1},attributes: ['{3}']  {1}).then(function (result) {0}//update code\r\n", "{","}",table.TableName,keyColumn.ColumnName,tableLName);
            sb.Append("            return result;\r\n");
            sb.Append("        }).then(function (result) {\r\n");
            sb.Append("            if (result) {\r\n");
            sb.AppendFormat("                req.formError[\"{0}\"] = \"{1}不允许重复\";\r\n", keyColumn.ColumnName, keyColumn.DisplayName);
            sb.Append("                return res.render('edit', {\r\n");
            sb.AppendFormat("                    {0}: model,\r\n", tableLName);
            sb.Append("                    formError: req.formError\r\n");
            sb.Append("                });\r\n");
            sb.Append("            }\r\n");
            sb.AppendFormat("            req.models.{1}.update(model, {0}\r\n", "{",table.TableName);
            sb.AppendFormat("                where: {2} {0}: {1}Id {3},\r\n", keyColumn.ColumnName,tableLName,"{","}");
            sb.Append("                fields: [");
            int j = 0;
            foreach (Column column in table.Columns)
            {
                if (column.IsKey) continue;
                if (j > 0)
                {
                    sb.Append(",");
                }
                sb.AppendFormat("\"{0}\"", column.ColumnName);
                j++;
            }
            sb.Append("]\r\n");
            sb.Append("            }).then(function () {\r\n");
            sb.AppendFormat("                    common.iframeRender(res, '/{0}/edit/'+ {0}Id, '/{0}');\r\n", tableLName);
            sb.Append("            }).catch(function (error) {\r\n");
            sb.Append("                return res.render('edit', {\r\n");
            sb.AppendFormat("                    {0}: model,\r\n", tableLName);
            sb.Append("                    pageError: error.message\r\n");
            sb.Append("                });\r\n");
            sb.Append("            });\r\n");
            sb.Append("        }).catch(function (error) {\r\n");
            sb.Append("            return res.render('edit', {\r\n");
            sb.AppendFormat("                {0}: model,\r\n", tableLName);
            sb.Append("                pageError: error.message\r\n");
            sb.Append("            });\r\n");
            sb.Append("        });\r\n");
            sb.Append("        //#endregion\r\n");
            #endregion
            sb.Append("    }\r\n");

            sb.Append("}\r\n");
            sb.Append("//#endregion\r\n\r\n\r\n");
            #endregion

            #region 详情
            sb.Append("//#region 详情\r\n");
            sb.Append("exports.detail = function (req, res, next) {\r\n");
            sb.Append("    res.render('detail', {\r\n");
            sb.AppendFormat("        title: '{0}详情-'+req.{1}.{1}Id,\r\n", table.DisplayName,tableLName);
            sb.AppendFormat("        {0}: req.{0}\r\n", tableLName);
            sb.Append("    });\r\n");
            sb.Append("");
            sb.Append("}\r\n");
            sb.Append("//#endregion\r\n\r\n\r\n");
            #endregion
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string CreateList(Table table) {
            string tableLName = table.TableName.ToLower();
            Column keyColumn = table.Columns.Find(p => p.IsKey);
            if (keyColumn == null) {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{{#section 'tool'}}\r\n");
            sb.Append("<div class=\"r-c-list\">\r\n");
            sb.Append("    {{#linkAction '新建" + table.DisplayName+"' 0 '{\"class\":\"w-btn bc2\"}'}}/"+tableLName+"/edit{{/linkAction}}\r\n");
            sb.Append("    {{#linkAction '全部" + table.DisplayName+"' 1 '{\"class\":\"w-btn bc2 cur\"}'}}/"+tableLName+"{{/linkAction}}\r\n");
            sb.Append("</div>\r\n");
            sb.Append("{{/section}}\r\n");

            sb.Append("<div class=\"w-m-table\">\r\n");
            sb.AppendFormat("    <tablefor for=\"{0}List\">\r\n", table.TableName);
            sb.Append("        <FixedHead></FixedHead>\r\n");
            sb.Append("    </tablefor>\r\n");
            sb.AppendFormat("    <table mark=\"LedinghuoTable\" id=\"{0}List\">\r\n", table.TableName);
            sb.Append("        <thead>\r\n");
            sb.Append("            <tr class=\"bgc3\">\r\n");
            foreach (Column column in table.Columns)
            {
                if (column.IsKey)
                {
                    sb.Append("                <th>{{{check '" + column.ColumnName + "' }}}" + column.DisplayName + "</th>\r\n");
                }
                else
                {
                    sb.AppendFormat("                <th>{0}</th>\r\n", column.DisplayName);
                }
            }
            sb.Append("                <th class=\"t-c\">操作</th>\r\n");
            sb.Append("            </tr>\r\n");
            sb.Append("        </thead>\r\n");
            sb.AppendFormat("        {1}{1}#each {0}s {2}{2}\r\n", tableLName,"{","}");
            sb.Append("        <tr>\r\n");
            foreach (Column column in table.Columns)
            {
                if (column.IsKey)
                {
                    sb.Append("            <td>{{{check '" + column.ColumnName + "' "+ column.ColumnName+ " }}}{{#linkAction " + keyColumn.ColumnName+" 1}}/"+tableLName+"/detail/{{"+keyColumn.ColumnName+"}}{{/linkAction}}</td>\r\n");
                }
                else
                {
                    sb.Append("            <td>{{");
                    sb.Append(column.ColumnName);
                    sb.Append("}}</td>\r\n");
                }
            }
            sb.Append("            <td>\r\n");
            sb.Append("                {{#linkAction '修改' 1}}/"+tableLName+"/edit/{{");
            sb.AppendFormat("{0}",keyColumn.ColumnName);
            sb.Append("}}{{/linkAction}}\r\n");
            sb.Append("                {{{ajaxPost '删除' '{");
            sb.AppendFormat("\"{0}id\":$0,\"url\":\"/{0}/delete/$0\",\"callback\":\"callbackRemoveTr\"",tableLName);
            sb.Append("}' ");
            sb.Append(keyColumn.ColumnName);
            sb.Append("}}}\r\n");
            sb.Append("            </td>\r\n");
            sb.Append("        </tr>\r\n");
            sb.Append("        {{/each}}\r\n");
            sb.Append("    </table>\r\n");
            sb.Append("</div>");
            
            sb.Append("{{#section 'pager'}}\r\n");
            sb.Append("   {{#pager totalCount pageSize pageIndex}}");
            sb.AppendFormat("/{0}/index?p=",tableLName);
            sb.Append("{{pageIndex}}{{/pager}}\r\n");
            sb.Append("{{/section}}\r\n");
            return sb.ToString();
        }

        public string CreateEdit(Table table)
        {
            string tableLName = table.TableName.ToLower();
            Column keyColumn = table.Columns.Find(p => p.IsKey);
            if (keyColumn == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"w-form\">\r\n");
            sb.AppendFormat("    <form mark=\"LedinghuoForm\" action=\"/{2}/update/{0}{0}{2}.{3}Id{1}{1}\" method=\"post\">\r\n","{","}",tableLName,table.TableName);
            foreach (Column column in table.Columns) {
                sb.Append("        {{{inputboxfor ");
                sb.AppendFormat("'{0}'",column.ColumnName);
                sb.Append("}}}\r\n");
            }
            sb.Append("        {{{button '保存'}}}\r\n");
            sb.Append("        {{{pageError}}}\r\n");
            sb.Append("    </form>\r\n");
            sb.AppendFormat("    <input type=\"hidden\" name=\"{3}\" id=\"{3}\" value=\"{0}{0}{2}.{3}{1}{1}\" />\r\n", "{", "}", tableLName, keyColumn.ColumnName);
            sb.Append("</div>");
            return sb.ToString();
        }

        public string CreateDetail(Table table)
        {
            string tableLName = table.TableName.ToLower();
            Column keyColumn = table.Columns.Find(p => p.IsKey);
            if (keyColumn == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"w-m-detail\">\r\n");
            foreach (Column column in table.Columns)
            {
                sb.Append("    {{{");
                sb.AppendFormat("detail '{0}'",column.ColumnName);
                sb.Append("}}}\r\n");
            }
            sb.Append("</div>");
            return sb.ToString();
        }

        public void Create(Table table, string outputFileName) {
            if (!Directory.Exists(outputFileName)) {
                return;
            }
            string path = Path.Combine(outputFileName, table.TableName.ToLower());
            if (Directory.Exists(path)) {
                return;
            }
            Directory.CreateDirectory(path);

            string viewPath = Path.Combine(path, "views");
            Directory.CreateDirectory(viewPath);

            string codePath = Path.Combine(path, "index.js");
            string listPath = Path.Combine(viewPath, "index.jshtml");
            string editPath = Path.Combine(viewPath, "edit.jshtml");
            string detailPath = Path.Combine(viewPath, "detail.jshtml");

            write(codePath, CreateController(table));
            write(listPath,CreateList(table));
            write(editPath, CreateEdit(table));
            write(detailPath, CreateDetail(table));
        }

        private void write(string path,string str)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(str);
                sw.Close();
            }
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
