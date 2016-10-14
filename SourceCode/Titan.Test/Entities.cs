using System;
using System.Collections.Generic;
using Titan.Oracle;
using Titan.SqlServer;
using System.Runtime.Serialization;
using Titan.SQLite;
using Titan.MySql;


namespace Titan.Test.Entity
{
   
    #region enums
 

    #region AccountTypeId
    /// <summary>
    /// 账户类型,AccountTypeId comment
    /// </summary>
    [DataContract]
    public enum AccountTypeId 
    {
           
    /// <summary>
    /// 未开通
    /// </summary>
    [EnumMember] 
    NotOpened=1,
           
    /// <summary>
    /// 开通中
    /// </summary>
    [EnumMember] 
    Opening=2,
           
    /// <summary>
    /// 已开通
    /// </summary>
    [EnumMember] 
    Opened=3,
            }
    #endregion

    #endregion




    #region Customer
    /// <summary>
    /// 客户,customer comment
    /// </summary>
    [DataContract]
    [Table(TableName="test_Customer")]
    public partial class Customer 
    {
        
        public Customer()
        {
        }
        #region propertys
        
        /// <summary>
        /// CustomerId,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column(IsPrimaryKey = true)] 
        [OracleColumn(Sequence="Test_CustomerId")]
        [SqlServerColumn(IsIdentity = true)]
        [SQLiteColumn(IsIdentity = true)]
        [MySqlColumn(IsIdentity = true)]
        public int CustomerId {  get; set; }



        [GroupColumn(OriginalPropertyName="CustomerId",GroupFunction=GroupFunction.Sum)]
        public int Sum_CustomerId { get; set; }

        [GroupColumn(OriginalPropertyName = "***", GroupFunction = GroupFunction.Count)]
        public int Count_ { get; set; }

        /// <summary>
        /// 账户开通类型,column comment
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public AccountTypeId AccountTypeId {  get; set; }


        /// <summary>
        /// 客户名称,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()]
        [OracleColumn(FullTextSearch="")]
        //[StringLengthValidator(MaxLength = 50)]
        //[NotNullableValidator]
        public string CustomerName {  get; set; }


        /// <summary>
        /// 所属地区,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public int AreaId {  get; set; }


        /// <summary>
        /// 客户类型,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public int CustomerTypeId {  get; set; }


        #endregion

        #region link objects
        
        /// <summary>
        /// Customer.CustomerType,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Relation("this.CustomerTypeId=out.CustomerTypeId")]
        public CustomerType CustomerType { get;  set;  } 



        /// <summary>
        /// Customer.Area,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Relation("this.AreaId=out.AreaId")]
        public Area Area { get;  set;  } 



        #endregion
    }
    #endregion
    #region CustomerProperties
    public static partial class CustomerProp
    {
    
        private static CustomerDescriptor instance = new CustomerDescriptor(""); 
        
        /// <summary>
        /// 全部字段
        /// </summary>
        public static PropertyExpression[] ALL { get{return instance.ALL;}}

 
        /// <summary>
        /// CustomerId,
        /// </summary>
        public static PropertyExpression CustomerId { get { return instance.CustomerId; } } 
        /// <summary>
        /// 账户开通类型,column comment
        /// </summary>
        public static PropertyExpression AccountTypeId { get { return instance.AccountTypeId; } } 
        /// <summary>
        /// 客户名称,
        /// </summary>
        public static PropertyExpression CustomerName { get { return instance.CustomerName; } } 
        /// <summary>
        /// 所属地区,
        /// </summary>
        public static PropertyExpression AreaId { get { return instance.AreaId; } } 
        /// <summary>
        /// 客户类型,
        /// </summary>
        public static PropertyExpression CustomerTypeId { get { return instance.CustomerTypeId; } }


 
        /// <summary>
        /// Customer.CustomerType,
        /// </summary>
        public static CustomerTypeDescriptor CustomerType { get{return instance.CustomerType;}} 
        /// <summary>
        /// Customer.Area,
        /// </summary>
        public static AreaDescriptor Area { get{return instance.Area;}}

        public static IEnumerable<PropertyExpression> Exclude(params PropertyExpression[] properties)
        {
            return instance.Exclude(properties);
        }

    }
     #endregion
    #region CustomerDescriptor
    public partial class CustomerDescriptor:ObjectDescriptorBase
    {

        public CustomerDescriptor(string prefix)
            : base(prefix)
        {

            _CustomerId = new PropertyExpression(prefix + "CustomerId");
            _AccountTypeId = new PropertyExpression(prefix + "AccountTypeId");
            _CustomerName = new PropertyExpression(prefix + "CustomerName");
            _AreaId = new PropertyExpression(prefix + "AreaId");
            _CustomerTypeId = new PropertyExpression(prefix + "CustomerTypeId");
            ALL = new PropertyExpression[] { _CustomerId, _AccountTypeId, _CustomerName, _AreaId, _CustomerTypeId };
        }
         

        private PropertyExpression _CustomerId;
        /// <summary>
        /// CustomerId,
        /// </summary>
        public PropertyExpression CustomerId { get { return _CustomerId; } }
        private PropertyExpression _AccountTypeId;
        /// <summary>
        /// 账户开通类型,column comment
        /// </summary>
        public PropertyExpression AccountTypeId { get { return _AccountTypeId; } }
        private PropertyExpression _CustomerName;
        /// <summary>
        /// 客户名称,
        /// </summary>
        public PropertyExpression CustomerName { get { return _CustomerName; } }
        private PropertyExpression _AreaId;
        /// <summary>
        /// 所属地区,
        /// </summary>
        public PropertyExpression AreaId { get { return _AreaId; } }
        private PropertyExpression _CustomerTypeId;
        /// <summary>
        /// 客户类型,
        /// </summary>
        public PropertyExpression CustomerTypeId { get { return _CustomerTypeId; } }



        private CustomerTypeDescriptor _CustomerType;
        public CustomerTypeDescriptor CustomerType 
        { 
            get
            {
                if(_CustomerType==null) _CustomerType=new CustomerTypeDescriptor(base.Prefix+"CustomerType.");
                return _CustomerType;
            }
        }
        private AreaDescriptor _Area;
        public AreaDescriptor Area 
        { 
            get
            {
                if(_Area==null) _Area=new AreaDescriptor(base.Prefix+"Area.");
                return _Area;
            }
        }
    }
     #endregion


    #region CustomerType
    /// <summary>
    /// 客户类型,
    /// </summary>
    [DataContract]
    [Table(TableName = "test_CustomerType")]
    public partial class CustomerType 
    {
        
        public CustomerType()
        {
        }
        #region propertys
        
        /// <summary>
        /// 客户类型Id,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column(IsPrimaryKey = true)]
        [OracleColumn(Sequence = "Test_CustomerTypeId")]
        [SqlServerColumn(IsIdentity = true)]
        [SQLiteColumn(IsIdentity = true)]
        [MySqlColumn(IsIdentity = true)]
        public int CustomerTypeId {  get; set; }


        /// <summary>
        /// 客户类型名称,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()]
        public string CustomerTypeName {  get; set; }


        #endregion

    }
    #endregion
    #region CustomerTypeProperties
    public static partial class CustomerType_
    {
    
        private static CustomerTypeDescriptor instance = new CustomerTypeDescriptor(""); 
        
        /// <summary>
        /// 全部字段
        /// </summary>
        public static PropertyExpression[] ALL { get { return instance.ALL; } }

 
        /// <summary>
        /// 客户类型Id,
        /// </summary>
        public static PropertyExpression CustomerTypeId { get { return instance.CustomerTypeId; } } 
        /// <summary>
        /// 客户类型名称,
        /// </summary>
        public static PropertyExpression CustomerTypeName { get { return instance.CustomerTypeName; } }




        public static IEnumerable<PropertyExpression> Exclude(params PropertyExpression[] properties)
        {
            return instance.Exclude(properties);
        }

    }
     #endregion
    #region CustomerTypeDescriptor
    public partial class CustomerTypeDescriptor:ObjectDescriptorBase
    {
     
        public CustomerTypeDescriptor(string prefix):base(prefix)
        {  
    
            _CustomerTypeId =new PropertyExpression( prefix + "CustomerTypeId");
            _CustomerTypeName = new PropertyExpression(prefix + "CustomerTypeName");
            ALL = new PropertyExpression[] { _CustomerTypeId, _CustomerTypeName };
        }


        private PropertyExpression _CustomerTypeId;
        /// <summary>
        /// 客户类型Id,
        /// </summary>
        public PropertyExpression CustomerTypeId { get { return _CustomerTypeId; } }
        private PropertyExpression _CustomerTypeName;
        /// <summary>
        /// 客户类型名称,
        /// </summary>
        public PropertyExpression CustomerTypeName { get { return _CustomerTypeName; } }



    }
     #endregion


    #region Area
    /// <summary>
    /// 地区,
    /// </summary>
    [DataContract]
    [Table(TableName = "test_Area")]
    public partial class Area 
    {
        
        public Area()
        {
        }
        #region propertys
        
        /// <summary>
        /// AreaId,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column(IsPrimaryKey = true)]
        [OracleColumn(Sequence = "Test_AreaId")]
        [SqlServerColumn(IsIdentity = true)]
        [SQLiteColumn(IsIdentity = true)]
        [MySqlColumn(IsIdentity = true)]
        public int AreaId {  get; set; }


        /// <summary>
        /// AreaName,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public string AreaName {  get; set; }


        /// <summary>
        /// ParentAreaId,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public int ParentAreaId {  get; set; }


        #endregion

        #region link objects
        
        /// <summary>
        /// Area.ParentArea,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Relation("this.ParentAreaId=out.AreaId")]
        public Area ParentArea { get;  set;  } 



        #endregion
    }
    #endregion
    #region AreaProperties
    public static partial class AreaProp
    {
    
        private static AreaDescriptor instance = new AreaDescriptor(""); 
        
        /// <summary>
        /// 全部字段
        /// </summary>
        public static PropertyExpression[] ALL { get { return instance.ALL; } }

 
        /// <summary>
        /// AreaId,
        /// </summary>
        public static PropertyExpression AreaId { get { return instance.AreaId; } } 
        /// <summary>
        /// AreaName,
        /// </summary>
        public static PropertyExpression AreaName { get { return instance.AreaName; } } 
        /// <summary>
        /// ParentAreaId,
        /// </summary>
        public static PropertyExpression ParentAreaId { get { return instance.ParentAreaId; } }


 
        /// <summary>
        /// Area.ParentArea,
        /// </summary>
        public static AreaDescriptor ParentArea { get{return instance.ParentArea;}}

        public static IEnumerable<PropertyExpression> Exclude(params PropertyExpression[] properties)
        {
            return instance.Exclude(properties);
        }

    }
     #endregion
    #region AreaDescriptor
    public partial class AreaDescriptor:ObjectDescriptorBase
    {
     
        public AreaDescriptor(string prefix):base(prefix)
        {  
    
            _AreaId = new PropertyExpression(prefix + "AreaId");
            _AreaName =new PropertyExpression( prefix + "AreaName");
            _ParentAreaId = new PropertyExpression(prefix + "ParentAreaId");
            ALL = new PropertyExpression[] { _AreaId, _AreaName, _ParentAreaId };
        }


        private PropertyExpression _AreaId;
        /// <summary>
        /// AreaId,
        /// </summary>
        public PropertyExpression AreaId { get { return _AreaId; } }
        private PropertyExpression _AreaName;
        /// <summary>
        /// AreaName,
        /// </summary>
        public PropertyExpression AreaName { get { return _AreaName; } }
        private PropertyExpression _ParentAreaId;
        /// <summary>
        /// ParentAreaId,
        /// </summary>
        public PropertyExpression ParentAreaId { get { return _ParentAreaId; } }



        private AreaDescriptor _ParentArea;
        public AreaDescriptor ParentArea 
        { 
            get
            {
                if(_ParentArea==null) _ParentArea=new AreaDescriptor(base.Prefix+"ParentArea.");
                return _ParentArea;
            }
        }
    }
     #endregion


    #region ColumnType
    /// <summary>
    /// ColumnType,
    /// </summary>
    [DataContract]
    [Table(TableName="test_columnType")]
    public partial class ColumnType 
    {
        
        public ColumnType()
        {
        }
        #region propertys
        
        /// <summary>
        /// Column_1,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column(IsPrimaryKey = true)] 
        public int Id {  get; set; }


        /// <summary>
        /// FBoolean,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public bool? FBoolean {  get; set; }


        /// <summary>
        /// FByte,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public byte? FByte {  get; set; }


        /// <summary>
        /// FShort,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public short? FShort {  get; set; }


        /// <summary>
        /// FInteger,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public int? FInteger {  get; set; }


        /// <summary>
        /// FLong,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public long? FLong {  get; set; }


        /// <summary>
        /// FFloat,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public float? FFloat {  get; set; }


        /// <summary>
        /// FDouble,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public double? FDouble {  get; set; }


        /// <summary>
        /// FNVarChar,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column(Size=50)] 
        public string FNVarChar {  get; set; }


        /// <summary>
        /// FVarChar,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public string FVarChar {  get; set; }


        /// <summary>
        /// FNClob,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()]
        [SqlServerColumn(IsNText=true)] 
        public string FNClob {  get; set; }


        /// <summary>
        /// FClob,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public string FClob {  get; set; }


        /// <summary>
        /// FBlob,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public byte[] FBlob {  get; set; }


        /// <summary>
        /// TTime,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public DateTime? FTime {  get; set; }


        /// <summary>
        /// FTimestamp,
        /// </summary>
        [DataMember(EmitDefaultValue=false)]
        [Column()] 
        public DateTime? FTimestamp {  get; set; }


        #endregion

    }
    #endregion
    #region ColumnTypeProperties
    public static partial class ColumnTypeProp
    {
    
        private static ColumnTypeDescriptor instance = new ColumnTypeDescriptor(""); 
        
        /// <summary>
        /// 全部字段
        /// </summary>
        public static PropertyExpression[] ALL { get { return instance.ALL; } }
        public static string[] ALLNONEKEY { get { return instance.ALLNONEKEY; } }

 
        /// <summary>
        /// Column_1,
        /// </summary>
        public static PropertyExpression Id { get { return instance.Id; } } 
        /// <summary>
        /// FBoolean,
        /// </summary>
        public static PropertyExpression FBoolean { get { return instance.FBoolean; } } 
        /// <summary>
        /// FByte,
        /// </summary>
        public static PropertyExpression FByte { get { return instance.FByte; } } 
        /// <summary>
        /// FShort,
        /// </summary>
        public static PropertyExpression FShort { get { return instance.FShort; } } 
        /// <summary>
        /// FInteger,
        /// </summary>
        public static PropertyExpression FInteger { get { return instance.FInteger; } } 
        /// <summary>
        /// FLong,
        /// </summary>
        public static PropertyExpression FLong { get { return instance.FLong; } } 
        /// <summary>
        /// FFloat,
        /// </summary>
        public static PropertyExpression FFloat { get { return instance.FFloat; } } 
        /// <summary>
        /// FDouble,
        /// </summary>
        public static PropertyExpression FDouble { get { return instance.FDouble; } } 
        /// <summary>
        /// FNVarChar,
        /// </summary>
        public static PropertyExpression FNVarChar { get { return instance.FNVarChar; } } 
        /// <summary>
        /// FVarChar,
        /// </summary>
        public static PropertyExpression FVarChar { get { return instance.FVarChar; } } 
        /// <summary>
        /// FNClob,
        /// </summary>
        public static PropertyExpression FNClob { get { return instance.FNClob; } } 
        /// <summary>
        /// FClob,
        /// </summary>
        public static PropertyExpression FClob { get { return instance.FClob; } } 
        /// <summary>
        /// FBlob,
        /// </summary>
        public static PropertyExpression FBlob { get { return instance.FBlob; } } 
        /// <summary>
        /// TTime,
        /// </summary>
        public static PropertyExpression FTime { get { return instance.FTime; } } 
        /// <summary>
        /// FTimestamp,
        /// </summary>
        public static PropertyExpression FTimestamp { get { return instance.FTimestamp; } }




        public static IEnumerable<PropertyExpression> Exclude(params PropertyExpression[] properties)
        {
            return instance.Exclude(properties);
        }

    }
     #endregion
    #region ColumnTypeDescriptor
    public partial class ColumnTypeDescriptor:ObjectDescriptorBase
    {
        public string[] ALLNONEKEY { get; set; }
        public ColumnTypeDescriptor(string prefix)
            : base(prefix)
        {

            _Id = new PropertyExpression(prefix + "Id");
            _FBoolean = new PropertyExpression(prefix + "FBoolean");
            _FByte = new PropertyExpression(prefix + "FByte");
            _FShort = new PropertyExpression(prefix + "FShort");
            _FInteger = new PropertyExpression(prefix + "FInteger");
            _FLong = new PropertyExpression(prefix + "FLong");
            _FFloat = new PropertyExpression(prefix + "FFloat");
            _FDouble = new PropertyExpression(prefix + "FDouble");
            _FNVarChar = new PropertyExpression(prefix + "FNVarChar");
            _FVarChar = new PropertyExpression(prefix + "FVarChar");
            _FNClob = new PropertyExpression(prefix + "FNClob");
            _FClob = new PropertyExpression(prefix + "FClob");
            _FBlob = new PropertyExpression(prefix + "FBlob");
            _FTime = new PropertyExpression(prefix + "FTime");
            _FTimestamp = new PropertyExpression(prefix + "FTimestamp");
            ALL = new PropertyExpression[] { _Id, _FBoolean, _FByte, _FShort, _FInteger, _FLong, _FFloat, _FDouble, _FNVarChar, _FVarChar, _FNClob, _FClob, _FBlob, _FTime, _FTimestamp };
            ALLNONEKEY = new string[] { _FBoolean.PropertyName, _FByte.PropertyName, _FShort.PropertyName, _FInteger.PropertyName, _FLong.PropertyName, _FFloat.PropertyName, _FDouble.PropertyName, _FNVarChar.PropertyName, _FVarChar.PropertyName, _FNClob.PropertyName, _FClob.PropertyName, _FBlob.PropertyName, _FTime.PropertyName, _FTimestamp.PropertyName };
        
        }


        private PropertyExpression _Id;
        /// <summary>
        /// Column_1,
        /// </summary>
        public PropertyExpression Id { get { return _Id; } }
        private PropertyExpression _FBoolean;
        /// <summary>
        /// FBoolean,
        /// </summary>
        public PropertyExpression FBoolean { get { return _FBoolean; } }
        private PropertyExpression _FByte;
        /// <summary>
        /// FByte,
        /// </summary>
        public PropertyExpression FByte { get { return _FByte; } }
        private PropertyExpression _FShort;
        /// <summary>
        /// FShort,
        /// </summary>
        public PropertyExpression FShort { get { return _FShort; } }
        private PropertyExpression _FInteger;
        /// <summary>
        /// FInteger,
        /// </summary>
        public PropertyExpression FInteger { get { return _FInteger; } }
        private PropertyExpression _FLong;
        /// <summary>
        /// FLong,
        /// </summary>
        public PropertyExpression FLong { get { return _FLong; } }
        private PropertyExpression _FFloat;
        /// <summary>
        /// FFloat,
        /// </summary>
        public PropertyExpression FFloat { get { return _FFloat; } }
        private PropertyExpression _FDouble;
        /// <summary>
        /// FDouble,
        /// </summary>
        public PropertyExpression FDouble { get { return _FDouble; } }
        private PropertyExpression _FNVarChar;
        /// <summary>
        /// FNVarChar,
        /// </summary>
        public PropertyExpression FNVarChar { get { return _FNVarChar; } }
        private PropertyExpression _FVarChar;
        /// <summary>
        /// FVarChar,
        /// </summary>
        public PropertyExpression FVarChar { get { return _FVarChar; } }
        private PropertyExpression _FNClob;
        /// <summary>
        /// FNClob,
        /// </summary>
        public PropertyExpression FNClob { get { return _FNClob; } }
        private PropertyExpression _FClob;
        /// <summary>
        /// FClob,
        /// </summary>
        public PropertyExpression FClob { get { return _FClob; } }
        private PropertyExpression _FBlob;
        /// <summary>
        /// FBlob,
        /// </summary>
        public PropertyExpression FBlob { get { return _FBlob; } }
        private PropertyExpression _FTime;
        /// <summary>
        /// TTime,
        /// </summary>
        public PropertyExpression FTime { get { return _FTime; } }
        private PropertyExpression _FTimestamp;
        /// <summary>
        /// FTimestamp,
        /// </summary>
        public PropertyExpression FTimestamp { get { return _FTimestamp; } }



    }
     #endregion
}
