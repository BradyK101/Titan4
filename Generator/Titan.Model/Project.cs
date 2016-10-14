using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Titan.SchemaModel;

namespace Titan.Model
{
    public class Project
    {
        public List<Table> Tables { get; set; }
        public List<EnumObject> EnumObjects { get; set; }

        ///<summary>
        ///从pmd文件加载模型
        ///</summary>
        ///<param name="pdmFileName"></param>
        ///<returns></returns>
        public static Project LoadProject(Entity model)
        {
            Project project = new Project();
            project.Tables = new List<Table>();






            #region enum
             
            project.EnumObjects = new List<EnumObject>();
            if (model.Properties.ContainsKey("Enums"))
            {
                Dictionary<string,Entity> enums = (Dictionary<string,Entity>)model.Properties["Enums"];
                foreach (Entity colItem in enums.Values)
                {
                    EnumObject enumObject = new EnumObject();
                    enumObject.Name = colItem.GetString("Code");
                    enumObject.DisplayName = colItem.GetString("Name");
                    enumObject.Comment = colItem.GetString("Comment");
                    enumObject.EnumMembers = new List<EnumMember>();
                    string members = colItem.GetString("EnumMember");

                    string[] ss = members.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in ss)
                    {
                        string[] ss1 = s.Split(new char[] { ',' });
                        EnumMember enumMember = new EnumMember();
                        enumMember.Value = Convert.ToInt32(ss1[0]);
                        enumMember.Name = ss1[1].Trim();
                        if (ss1.Length > 2)
                        {
                            enumMember.Comment = ss1[2];
                        }
                        enumObject.EnumMembers.Add(enumMember);
                    }

                    project.EnumObjects.Add(enumObject);
                } 
            }
            #endregion





            #region 获取引用 对象关系
            Dictionary<string, List<Relation>> references = new Dictionary<string, List<Relation>>();

            if (model.ContainsProperty("ViewReferences"))
            {
                Dictionary<string, Entity> refs = (Dictionary<string, Entity>)model.Properties["ViewReferences"];
                foreach (Entity reference in refs.Values)
                {

                    Relation relation = new Relation();
                    relation.Comment = reference.GetString("Comment");
                    //string referenceValidationStr = reference.GetString("ReferenceValidation");
                    //relation.ReferenceValidation = String.IsNullOrEmpty(referenceValidationStr) ? true : Convert.ToBoolean(referenceValidationStr);
                    string[] ss = reference.Code.Split('.');
                    if (ss.Length > 0)
                    {

                        ////ownerName 
                        //string ownerName = "";//City.ParentCity 中的City表示属于谁的
                        //Entity childTable = reference.Object1 as PdPDM.Table;
                        //if (childTable != null)
                        //{
                        //    ownerName = childTable.Code;
                        //}
                        //else
                        //{
                        //    PdPDM.View pptable = reference.Object1 as PdPDM.View;
                        //    ownerName = pptable.Code;
                        //}



                        ////PropertyName
                        //relation.PropertyName = ss[1];
                        //relation.DisplayName = reference.Name;


                        ////tableName
                        //PdPDM.View parentTable = (PdPDM.View)reference.Object2;
                        //if (parentTable != null)
                        //{
                        //    relation.TableName = parentTable.Name;
                        //}
                        //else
                        //{
                        //    PdPDM.Table parentView = (PdPDM.Table)reference.Object2;
                        //    relation.TableName = parentView.Name;
                        //}


                        //relation.RelationColumns = new List<RelationColumn>();
                        //foreach (PdPDM.ViewReferenceJoin join in reference.Joins)
                        //{
                        //    string localColumnName = "";
                        //    PdPDM.ViewColumn ccolumn = join.Column2 as PdPDM.ViewColumn;
                        //    if (ccolumn != null)
                        //    {
                        //        localColumnName = ccolumn.Code;
                        //    }
                        //    else
                        //    {
                        //        PdPDM.Column ppcolumn = join.Column2 as PdPDM.Column;
                        //        localColumnName = ppcolumn.Code;
                        //    }
                        //    string foreignColumnName = "";
                        //    PdPDM.Column pcolumn = join.Column1 as PdPDM.Column;
                        //    if (pcolumn != null)
                        //    {
                        //        foreignColumnName = pcolumn.Code;
                        //    }
                        //    else
                        //    {
                        //        PdPDM.ViewColumn pviewc = join.Column1 as PdPDM.ViewColumn;
                        //        foreignColumnName = pviewc.Code;
                        //    }
                        //    relation.RelationColumns.Add(new RelationColumn(localColumnName, foreignColumnName));
                        //}

                        //if (!references.ContainsKey(ownerName))
                        //{
                        //    references.Add(ownerName, new List<Relation>());
                        //}
                        //List<Relation> list = references[ownerName];
                        //list.Add(relation);
                    }
                }
            }


            if (model.Properties.ContainsKey("References"))
            {
                Dictionary<string, Entity> refs = (Dictionary<string, Entity>)model.Properties["References"];
                foreach (Entity reference in refs.Values)
                {
                    Relation relation = new Relation();
                    relation.Comment = reference.GetString("Comment");
                    //string referenceValidationStr = reference.GetExtendedAttributeText("ReferenceValidation");
                    //relation.ReferenceValidation = String.IsNullOrEmpty(referenceValidationStr) ? true : Convert.ToBoolean(referenceValidationStr);

                    string[] ss = reference.Code.Split('.');
                    if (ss.Length > 0)
                    {
                        string ownerName = "";
                        relation.PropertyName = ss[1];
                        Entity parentTable = (Entity)reference["ParentTable"];
                        Entity childTable = (Entity)reference["ChildTable"];
                        ownerName = childTable.Code;
                        relation.TableName = parentTable.Code;
                        relation.DisplayName = reference.Name;
                        relation.RelationColumns = new List<RelationColumn>();
                        List< Entity> joins = (List<Entity>)reference["ReferenceColumns"];
                        foreach (Entity join in joins)
                        {
                            Entity pcolumn = (Entity)join["ParentColumn"];
                            Entity ccolumn = (Entity)join["ChildColumn"];

                            relation.RelationColumns.Add(new RelationColumn(ccolumn.Code, pcolumn.Code));

                        }


                        if (!references.ContainsKey(ownerName))
                        {
                            references.Add(ownerName, new List<Relation>());
                        }
                        List<Relation> list = references[ownerName];
                        list.Add(relation);
                    }
                }
            }
            #endregion




            #region each table and view
            if (model.Properties.ContainsKey("Tables"))
            {

                Dictionary<string, Entity> tables = (Dictionary<string, Entity>)model.Properties["Tables"];
                foreach (Entity tb in tables.Values)
                {
                    Table t = new Table();
                    t.TableName = tb.Code;
                    t.IsView = false;
                    t.DisplayName = tb.Name;
                    if (t.DisplayName == t.TableName)
                    {
                        t.DisplayName = t.Comment;
                    }
                    t.Comment = tb.GetString("Comment");
                    //t.EnumData = tb.GetExtendedAttributeText("EnumData");



                    t.Columns = new List<Column>();
                    Dictionary<string, Entity> columns = (Dictionary<string, Entity>)tb.Properties["Columns"];
                    foreach (Entity f in columns.Values)
                    {
                        Column c = new Column();
                        c.AllowNull = !f.GetBoolean("Column.Mandatory");
                        c.ColumnName = f.Code;
                        c.ColumnType = f.GetString("DataType").ToLower();
                        c.Comment = f.GetString("Comment");
                        c.DisplayName = f.Name;
                        if (c.DisplayName == c.ColumnName)
                        {
                            c.DisplayName = c.Comment;
                        }
                        if (c.Comment == c.DisplayName)
                        {
                            c.Comment = "";
                        }
                        c.Identity = f.GetBoolean("Identity");
                        c.ColumnTypeLength = f.GetInt32("Length");
                        c.Precision = f.GetInt32("Precision");
                        if (f.ContainsProperty("Sequence"))
                        {
                            c.SequenceName = ((Entity)f["Sequence"]).Code;
                        }
                        if (c.SequenceName == "<None>") c.SequenceName = null;

                        if (t.TableName == "Company" && c.ColumnName == "CompanyName")
                        {
                            Console.WriteLine(c.ColumnType + "_" + c.ColumnTypeLength);
                        }
                        if (!string.IsNullOrEmpty(c.SequenceName))
                        {
                            c.Identity = true;
                        }
                        c.IsKey = f.GetBoolean("Primary");

                        c.InputPrompt = f.GetString("InputPrompt");
                        c.ControlType = f.GetString("ControlType");
                        if (f.ContainsProperty("Enum"))
                        {
                            c.EnumType = ((Entity)f["Enum"]).Code;
                        }
                        if (c.EnumType == "<None>") c.EnumType = null;
                        c.FullTextSearch = f.GetString("FullTextSearch");
                        c.CSharpValidate = f.GetString("CSharpValidate");


                        t.Columns.Add(c);
                    }

                    t.Relations = new List<Relation>();
                    if (references.ContainsKey(t.TableName))
                    {
                        t.Relations = references[t.TableName];
                    }
                    project.Tables.Add(t);
                }

            }
            if (model.Properties.ContainsKey("Views"))
            {

                Dictionary<string, Entity> views = (Dictionary<string, Entity>)model.Properties["Views"];
                foreach (Entity view in views.Values)
                {
                    Table t = new Table();
                    t.TableName = view.Code;
                    t.IsView = true;
                    t.DisplayName = view.Name;
                    t.Comment = view.GetString("Comment");

                    t.Columns = new List<Column>();
                    Dictionary<string, Entity> columns = (Dictionary<string, Entity>)view.Properties["Columns"];
                    foreach (Entity f in columns.Values)
                    {
                        Column c = new Column();
                        c.AllowNull = true;
                        c.ColumnName = f.Code;
                        c.ColumnType = f.GetString("DataType").ToLower();
                        c.Comment = f.GetString("Comment");
                        c.DisplayName = f.Name;


                        if (c.DisplayName == c.ColumnName)
                        {
                            c.DisplayName = c.Comment;
                        }
                        if (c.Comment == c.DisplayName)
                        {
                            c.Comment = "";
                        }


                        c.ColumnTypeLength = f.GetInt32("Length");
                        c.Precision = f.GetInt32("Precision");
                        if (f.ContainsProperty("Sequence"))
                        {
                            c.SequenceName = ((Entity)f["Sequence"]).Code;
                        }
                        if (c.SequenceName == "<None>") c.SequenceName = null;
                        //c.Length = getStringLength(c);
                        c.Identity = false;


                        c.IsKey = false;

                        c.InputPrompt = f.GetString("InputPrompt");
                        c.ControlType = f.GetString("ControlType");
                        if (f.ContainsProperty("Enum"))
                        {
                            c.EnumType = ((Entity)f["Enum"]).Code;
                        }
                        if (c.EnumType == "<None>") c.EnumType = null;
                        c.FullTextSearch = f.GetString("FullTextSearch");
                        c.CSharpValidate = f.GetString("CSharpValidate");

                        t.Columns.Add(c);

                    }
                    t.Relations = new List<Relation>();
                    if (references.ContainsKey(t.TableName))
                    {
                        t.Relations = references[t.TableName];
                    }
                    project.Tables.Add(t);
                }
            }


            #endregion


             
            return project;
        }
    }
}
