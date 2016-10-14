//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using Titan.CSharpGenerator.Model;
//using Titan.SchemaModel;
//using System.IO;

//namespace Titan.CSharpGenerator
//{ 
//    public class EntityClassGenerator  
//    {
//        private IEntityClassGenerator MySqlGenerator;
//        private IEntityClassGenerator OracleGenerator;
//        private IEntityClassGenerator SqlServerGenerator;
//        private IEntityClassGenerator SQLiteGenerator;
//        public EntityClassGenerator()
//        {
//            MySqlGenerator = new EntityClassGenerator_MySql();
//            OracleGenerator = new EntityClassGenerator_Oracle();
//            SqlServerGenerator = new EntityClassGenerator_SqlServer();
//            SQLiteGenerator = new EntityClassGenerator_SQLite();
//        }

//        public void Create(Project project, EntityClassConfig config)
//        { 


//            //CSharpEntityGeneratorConfig config = new CSharpEntityGeneratorConfig();
//            //config.OutputFileName = "";
//            //config.WcfEnabled = true;
//            //config.NameSpace = "Football.IService";

//            //fStatus.ShowMe("正在生成Entities");
//            //int totalProcessCount = project.Tables.Count;
//            //fStatus.SetMax(totalProcessCount);
//            //int currentProcess = 0;

//            StringBuilder sb = new StringBuilder();

//            sb.AppendFormat(@"using System;
//using System.Collections.Generic;
//using System.Data;
//using System.ComponentModel;
//using Titan;");
//            if (config.SupportMySql)
//            {
//                sb.AppendFormat(@"
//using Titan.MySql;");
//            }
//            if (config.SupportSQLite)
//            {
//                sb.AppendFormat(@"
//using Titan.SQLite;");
//            }
//            if (config.SupportOracle)
//            {
//                sb.AppendFormat(@"
//using Titan.Oracle;");
//            }
//            if (config.SupportSqlServer)
//            {
//                sb.AppendFormat(@"
//using Titan.SqlServer;");
//            }
//            if (config.WcfEnabled)
//            {
//                sb.AppendFormat(@"
//using System.Runtime.Serialization;");
//            }
//            sb.AppendFormat(@"
//
//
//namespace {0}
//{{
//    ", config.NameSpace);


             
//            sb.AppendFormat(@"
//    {0}
//    public class EntityList<T> 
//    {{ 
//        private List<T> _items =   new List<T>();
//        {1}
//        public long TotalCount{{ get; set; }}
//
//        {1}
//        public List<T> Items {{
//            get {{ return _items;}}
//            set {{ _items = value;}}
//        
//        }}
//          
//    }}", config.WcfEnabled ? "[DataContract]" : "", config.WcfEnabled ? "[DataMember]" : "");

//            #region knownType


//            sb.Append(@"
///*
//<system.runtime.serialization>
//    <dataContractSerializer>
//      <declaredTypes>
//");
//            foreach (EnumObject enumObject in project.EnumObjects)
//            {
//                sb.AppendFormat(@"        <knownType type=""{0}.{1}, {0}""/> 
//", config.NameSpace, enumObject.Name);
//            }
//            sb.Append(@" 
//      </declaredTypes>
//    </dataContractSerializer>
//  </system.runtime.serialization>
//
//*/
//");

//            #endregion

 

//            #region enums
//            sb.Append("    #region enums\r\n");
//            foreach (EnumObject enumObject in project.EnumObjects)
//            {
//                string edescription = enumObject.Comment;
//                if (!String.IsNullOrEmpty(edescription))
//                {
//                    edescription = edescription.Replace("\r\n", "\r\n    /// ");
//                }

//                sb.AppendFormat(@" 
//
//    #region {0}
//    /// <summary>
//    /// {1},{2}
//    /// </summary>{3}
//    public enum {0} 
//    {{
//        ", enumObject.Name, enumObject.DisplayName, edescription, config.WcfEnabled ? "\r\n    [DataContract]" : "");

//                foreach (EnumMember enumMember in enumObject.EnumMembers)
//                {
//                    string eedescription = enumMember.Comment;
//                    if (!String.IsNullOrEmpty(eedescription))
//                    {
//                        eedescription = eedescription.Replace("\r\n", "\r\n    /// ");
//                    }
//                    sb.AppendFormat(@"   
//    /// <summary>
//    /// {2}
//    /// </summary>{3} 
//    {0}={1},
//        ", enumMember.Name, enumMember.Value, eedescription, config.WcfEnabled ? "\r\n    [EnumMember]" : "");
//                }

//                sb.AppendFormat(@"    }}
//    #endregion
//");
//            }
//            sb.Append("\r\n    #endregion\r\n\r\n");
//            #endregion




//            foreach (Table table in project.Tables)
//            {
//                //currentProcess++;
//                //fStatus.SetCurrent(currentProcess);

//                string description = table.Comment;
//                if (!String.IsNullOrEmpty(description))
//                {
//                    description = description.Replace("\r\n", "\r\n    /// ");
//                }

//                #region class
//                sb.AppendFormat(@"
//
//
//    #region {0}
//    /// <summary>
//    /// {1},{2}
//    /// </summary>{3}{4}
//    public partial class {0} 
//    {{
//        ", table.TableName, table.DisplayName, description, config.WcfEnabled ? "\r\n    [DataContract]" : "",  CreateAttribute_Table(table)  );



//                #region 构造函数
//                sb.AppendFormat(@"
//        public {0}()
//        {{
//", table.TableName);

//                //datetime初始化值的问题
//                foreach (Column column in table.Columns)
//                {
//                    string sqlType = GetSqlType(column);
//                    if (!column.AllowNull && sqlType == "DateTime")
//                    {
//                        sb.AppendFormat(@"
//            {0}=DateTime.Now;", column.ColumnName);

//                    }
//                }

//                sb.AppendFormat(@"
//        
//        }}", table.TableName);

//                #endregion

//                #region propertys
//                sb.AppendFormat(@"
//        #region propertys
//        ");

//                foreach (Column column in table.Columns)
//                {
//                    string fdescription = column.Comment;
//                    if (!String.IsNullOrEmpty(fdescription))
//                    {
//                        fdescription = fdescription.Replace("\r\n", "\r\n        /// ");
//                    }

//                    //SqlType sqlType = GetSqlType(column);
//                    //string strDataMember = "";
//                    //if (!column.AllowNull && sqlType == SqlType.DateTime)
//                    //{
//                    //    strDataMember="\r\n        [DataMember]\r\n        [DefaultValue(typeof(DateTime), \"2001-01-01\")] ";
//                    //}
//                    //else
//                    //{
//                    //    strDataMember = "\r\n        [DataMember]";
//                    //}
//                    sb.AppendFormat(@"
//        /// <summary>
//        /// {0},{1}
//        /// </summary>{2}{3}", column.DisplayName, fdescription, config.WcfEnabled ? "\r\n        [DataMember]" : "",  CreateAttribute_Column(column) );





//                    sb.AppendFormat(@" 
//        public {0} {1} {{  get; set; }}
//
//", string.IsNullOrEmpty(column.EnumType) ? GetPropertyType(column) : column.EnumType, column.ColumnName);
//                }




//                sb.Append(@"
//        #endregion
//");
//                #endregion

//                #region link objects
//                if (table.Relations.Count > 0)
//                {
//                    sb.Append(@"
//        #region link objects
//        ");




//                    foreach (Relation relation in table.Relations)
//                    {
//                        string fdescription = relation.Comment;
//                        if (!String.IsNullOrEmpty(fdescription))
//                        {
//                            fdescription = fdescription.Replace("\r\n", "\r\n        /// ");
//                        }



//                        sb.AppendFormat(@"
//        /// <summary>
//        /// {0},{1}
//        /// </summary>{2}{3}", relation.DisplayName, fdescription, config.WcfEnabled ? "\r\n        [DataMember]" : "",  CreateAttribute_Relation(relation) );


//                        sb.AppendFormat(@"
//        public {0} {1} {{ get;  set;  }} 
//
//
//", relation.TableName, relation.PropertyName);
//                    }



//                    sb.AppendFormat(@"
//        #endregion");
//                }
//                #endregion


//                sb.Append(@"
//    }
//    #endregion");
//                #endregion


//                #region object properties
//                sb.AppendFormat(@"
//    #region {0}Properties
//    public static partial class {0}_
//    {{
//    ", table.TableName);

//                sb.AppendFormat(@"
//        private static {0}Descriptor instance = new {0}Descriptor(""""); 
//        
//        /// <summary>
//        /// 全部字段
//        /// </summary>
//        public static string[] ALL {{ get{{return instance.ALL;}}}}
//
//", table.TableName);
//                foreach (Column column in table.Columns)
//                {
//                    string fdescription = column.Comment;
//                    if (!String.IsNullOrEmpty(fdescription))
//                    {
//                        fdescription = fdescription.Replace("\r\n", "\r\n        /// ");
//                    }
//                    sb.AppendFormat(@" 
//        /// <summary>
//        /// {1},{2}
//        /// </summary>
//        public static string {0} {{ get{{return instance.{0};}}}}", column.ColumnName, column.DisplayName, fdescription);
//                }
//                sb.Append(@"
//
//
//");
//                foreach (Relation relation in table.Relations)
//                {
//                    string fdescription = relation.Comment;
//                    if (!String.IsNullOrEmpty(fdescription))
//                    {
//                        fdescription = fdescription.Replace("\r\n", "\r\n        /// ");
//                    }
//                    sb.AppendFormat(@" 
//        /// <summary>
//        /// {2},{3}
//        /// </summary>
//        public static {1}Descriptor {0} {{ get{{return instance.{0};}}}}", relation.PropertyName, relation.TableName, relation.DisplayName, fdescription);
//                }
//                sb.AppendFormat(@"
//
//        public static IEnumerable<string> Exclude(params string[] properties)
//        {{
//            return instance.Exclude(properties);
//        }}
//
//    }}
//     #endregion");
//                #endregion

//                #region objectDescriptor
//                sb.AppendFormat(@"
//    #region {0}Descriptor
//    public partial class {0}Descriptor:ObjectDescriptorBase
//    {{
//    ", table.TableName);
//                sb.AppendFormat(@" 
//        public {0}Descriptor(string prefix):base(prefix)
//        {{  
//    ", table.TableName);
//                foreach (Column column in table.Columns)
//                {
//                    sb.AppendFormat(@"
//            this._{0} = prefix + ""{0}"";", column.ColumnName);
//                }

//                sb.Append(@"
//            ALL = new string[] {");
//                int index = 0;
//                foreach (Column column in table.Columns)
//                {
//                    if (index > 0) sb.Append(",");
//                    index++;
//                    sb.AppendFormat(@"this._{0}", column.ColumnName);
//                }
//                sb.Append(@"};");

//                sb.AppendFormat(@"
//        }}
//         
//");
//                foreach (Column column in table.Columns)
//                {
//                    string fdescription = column.Comment;
//                    if (!String.IsNullOrEmpty(fdescription))
//                    {
//                        fdescription = fdescription.Replace("\r\n", "\r\n        /// ");
//                    }
//                    sb.AppendFormat(@"
//        private string _{0};
//        /// <summary>
//        /// {1},{2}
//        /// </summary>
//        public string {0} {{ get{{return _{0};}}}}", column.ColumnName, column.DisplayName, fdescription);
//                }
//                sb.Append(@"
//
//
//");
//                foreach (Relation relation in table.Relations)
//                {
//                    sb.AppendFormat(@"
//        private {1}Descriptor _{0};
//        public {1}Descriptor {0} 
//        {{ 
//            get
//            {{
//                if(_{0}==null) _{0}=new {1}Descriptor(base.Prefix+""{0}."");
//                return _{0};
//            }}
//        }}", relation.PropertyName, relation.TableName);
//                }
//                sb.AppendFormat(@"
//    }}
//     #endregion");
//                #endregion

//                #region classes
//                sb.AppendFormat(@"
//
//
//    #region {0}s
//    /// <summary>
//    /// {1},{2}
//    /// </summary>{3}{4}
//    public partial class {0}s:EntityList<{0}> 
//    {{
//        ", table.TableName, table.DisplayName, description, config.WcfEnabled ? "\r\n    [DataContract]" : "",  CreateAttribute_Table(table) );




//                sb.Append(@"
//    }
//    #endregion");
//                #endregion

//            }
//            sb.Append(@"
//}");


//            //create text file
//            TextFile.Write(config.OutputFileName, sb.ToString());
//        }


//        private string CreateAttribute_Table(Table table)
//        {
//            StringBuilder sb = new StringBuilder();
//            //            sb.AppendFormat(@"
//            //    [Table(DefaultTableName=""{0}"")]", table.TableName);
//            sb.AppendFormat(@"
//    [Table]");
//            sb.Append(SubCreateAttribute_Table(table));
//            return sb.ToString();
//        }

//        private string CreateAttribute_Column(Column column)
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.Append(@"
//        [Column(");

//            bool hasItem = false;//影响到逗号的输出
//            if (column.IsKey)
//            {
//                if (hasItem) { sb.Append(","); }
//                hasItem = true;
//                sb.Append(@"IsKey = true");
//            }
//            if (!string.IsNullOrWhiteSpace(column.SequenceName))
//            {
//                if (hasItem) { sb.Append(","); }
//                hasItem = true;
//                sb.Append(@"ReturnAfterInsert = true");
//            }
//            sb.Append(@")]");
//            sb.Append(SubCreateAttribute_Column(column));
//            return sb.ToString();
//        }

//        private string CreateAttribute_Relation(Relation relation)
//        {
//            StringBuilder sb = new StringBuilder();

//            sb.Append(@"
//        [Relation(");
//            int i = 1;
//            int j = relation.RelationColumns.Count;
//            foreach (RelationColumn relationColumn in relation.RelationColumns)
//            {


//                sb.AppendFormat(@"""this.{0}=out.{1}""", relationColumn.LocalColumnName, relationColumn.ForeignColumnName);
//                if (i < j)
//                {
//                    sb.Append(",");
//                }
//                i += 1;
//            }
//            sb.Append(@")]");
//            sb.Append(SubCreateAttribute_Relation(relation));
//            return sb.ToString();
//        }

         

//    }
//}
