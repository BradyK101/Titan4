using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titan.SqlTracer;
using Titan.Oracle;
using Titan.SqlServer;
using Titan.SQLite;
using Titan.MySql;

namespace Titan.Test
{
    [TestClass()]
    public class Environment
    {

        public static IDbSession OpenSession()
        {
            IDbSession session = new DbSession(SqlProvider, ConnectionString, SqlTracers);
            session.Open();
            return session;
            //return DbSessionFactory.CreateAndOpenSession(SqlProviderType, ConnectionString, SqlTracers);
        }

        public static string ConnectionString;
        public static ISqlProvider SqlProvider;
        public static ISqlTracer[] SqlTracers; 
        /// <summary>
        /// 初始化环境
        /// </summary>
        /// <param name="testContext"></param>
        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            FileSqlTracer tracer = new FileSqlTracer();
            tracer.FileName = "d:\\sqllog\\{yyyyMMdd_HH}.txt";
            SqlTracers = new ISqlTracer[] { tracer };

            InitializeOracle();
            //InitializeSqlServer();
            //InitializeSQLite();
            //InitializeMySql();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            CleanupOracle();
            //CleanupSqlServer();
            //CleanupSQLite();
            //CleanupMySql();
        }

         
        /// <summary>
        /// 初始化SqlServer
        /// </summary>
        private static void InitializeSqlServer()
        {
            SqlProvider = new SqlServerSqlProvider();
            ConnectionString = @"Data Source=192.168.139.129\SQLEXPRESS;Initial Catalog=titan;User Id=titan;Password=123456;";


            using (IDbSession session = OpenSession())
            {

                string sql = "";

                #region 创建test_dbtype表
                sql = @"


IF OBJECT_ID('TEST_COLUMNTYPE', 'U') IS NOT NULL DROP TABLE TEST_COLUMNTYPE;
create table TEST_COLUMNTYPE
(
  ID         [int] NOT NULL PRIMARY KEY,
  FBOOLEAN   bit,
  FBYTE      tinyint,
  FSHORT     smallint,
  FINTEGER   int,
  FLONG      bigint,
  FFLOAT     real,
  FDOUBLE    float,
  FNVARCHAR  NVARCHAR(50),
  FVARCHAR   VARCHAR(50),
  FNCLOB     ntext,
  FCLOB      text,
  FBLOB      image,
  FTIME      datetime,
  FTIMESTAMP datetime
);


IF OBJECT_ID('TEST_CUSTOMER', 'U') IS NOT NULL DROP TABLE TEST_CUSTOMER;
create table TEST_CUSTOMER
(
  CUSTOMERID     [int] Identity(1,1) NOT NULL PRIMARY KEY,
  ACCOUNTTYPEID  int not null,
  CUSTOMERNAME   nVARCHAR(50) not null,
  AREAID         int not null,
  CUSTOMERTYPEID int not null 
);


IF OBJECT_ID('TEST_CUSTOMERTYPE', 'U') IS NOT NULL DROP TABLE TEST_CUSTOMERTYPE;
create table TEST_CUSTOMERTYPE
(
  CUSTOMERTYPEID   [int] Identity(1,1) NOT NULL PRIMARY KEY,
  CUSTOMERTYPENAME NVARCHAR(50) not null 
);


IF OBJECT_ID('TEST_AREA', 'U') IS NOT NULL DROP TABLE TEST_AREA;
create table TEST_AREA
(
  AREAID       [int] Identity(1,1) NOT NULL PRIMARY KEY,
  AREANAME     NVARCHAR(50) not null default ('dft'),
  PARENTAREAID int not null 
);

if Exists(select name from sysobjects where name='test_sp' and type='P') drop procedure test_sp;
";
                session.ExecuteNonQuery(sql);

                sql = @"CREATE PROCEDURE test_sp
            @I1 int,
			@I2 int,
			@I3 int output
       AS
        begin
        set @I3=@i1+@i2;
		insert into test_area( areaname,parentAreaId) values('a1',555);
		insert into test_area( areaname,parentAreaId) values('a2',555);
		insert into test_area( areaname,parentAreaId) values('a3',555);

		insert into test_customerType( customerTypeName) values('t1');
		insert into test_customerType( customerTypeName) values('t2');

        select * from test_area;
        select * from test_customerType;

       end;

";
                session.ExecuteNonQuery(sql);
                session.ExecuteNonQuery("insert into test_columntype (id) values (1)");
                #endregion


            }
        }
        /// <summary>
        /// 清除SqlServer
        /// </summary>
        private static void CleanupSqlServer()
        {
            using (IDbSession session = OpenSession())
            {
                string sql = "";

                #region 清除test_dbtype表

                sql = @"
IF OBJECT_ID('TEST_COLUMNTYPE', 'U') IS NOT NULL DROP TABLE TEST_COLUMNTYPE;  
IF OBJECT_ID('TEST_CUSTOMER', 'U') IS NOT NULL DROP TABLE TEST_CUSTOMER; 
IF OBJECT_ID('TEST_CUSTOMERTYPE', 'U') IS NOT NULL DROP TABLE TEST_CUSTOMERTYPE; 
IF OBJECT_ID('TEST_AREA', 'U') IS NOT NULL DROP TABLE TEST_AREA;

if Exists(select name from sysobjects where name='test_sp' and type='P') drop procedure test_sp;

";
                session.ExecuteNonQuery(sql);

                #endregion
            }
        }


        /// <summary>
        /// 初始化SQLite
        /// </summary>
        private static void InitializeSQLite()
        {
            SqlProvider  = new  SQLiteSqlProvider();
            ConnectionString = @"Data Source=d:\titan_sqlite.db;Version=3;";


            using (IDbSession session = OpenSession())
            {

                string sql = "";

                #region 创建test_dbtype表
                sql = @"
DROP TABLE IF EXISTS TEST_COLUMNTYPE;  
DROP TABLE IF EXISTS TEST_CUSTOMER; 
DROP TABLE IF EXISTS TEST_CUSTOMERTYPE; 
DROP TABLE IF EXISTS TEST_AREA; 
 
CREATE TABLE IF NOT EXISTS TEST_COLUMNTYPE
(
  ID         integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  FBOOLEAN   bit,
  FBYTE      tinyint,
  FSHORT     smallint,
  FINTEGER   int,
  FLONG      bigint,
  FFLOAT     real,
  FDOUBLE    float,
  FNVARCHAR  NVARCHAR(50),
  FVARCHAR   VARCHAR(50),
  FNCLOB     ntext,
  FCLOB      text,
  FBLOB      blob,
  FTIME      datetime,
  FTIMESTAMP timestamp
);

 
create table IF NOT EXISTS TEST_CUSTOMER
(
  CUSTOMERID     integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  ACCOUNTTYPEID  int not null,
  CUSTOMERNAME   nVARCHAR(50) not null,
  AREAID         int not null,
  CUSTOMERTYPEID int not null 
);
 
create table IF NOT EXISTS TEST_CUSTOMERTYPE
(
  CUSTOMERTYPEID   integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  CUSTOMERTYPENAME NVARCHAR(50) not null 
);

 
create table IF NOT EXISTS TEST_AREA
(
  AREAID       integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  AREANAME     NVARCHAR(50) not null default 'dft',
  PARENTAREAID int not null 
);
";
                session.ExecuteNonQuery(sql);
                session.ExecuteNonQuery("insert into test_columntype (id) values (1)");
                #endregion


            }
        }
        /// <summary>
        /// 清除SQLite
        /// </summary>
        private static void CleanupSQLite()
        {
            using (IDbSession session = OpenSession())
            {
                string sql = "";

                #region 清除test_dbtype表

                sql = @" 
DROP TABLE IF EXISTS TEST_COLUMNTYPE;  
DROP TABLE IF EXISTS TEST_CUSTOMER; 
DROP TABLE IF EXISTS TEST_CUSTOMERTYPE; 
DROP TABLE IF EXISTS TEST_AREA; 

";
                session.ExecuteNonQuery(sql);

                #endregion
            }
        }


        /// <summary>
        /// 初始化MySql
        /// </summary>
        private static void InitializeMySql()
        {
            SqlProvider = new MySqlSqlProvider();
            ConnectionString = @"Data Source=192.168.139.129; Database=titan; User ID=root; Password=123456;";


            using (IDbSession session = OpenSession())
            {

                string sql = "";

                #region 创建test_dbtype表
                sql = @"
DROP TABLE IF EXISTS `TEST_COLUMNTYPE`;  
DROP TABLE IF EXISTS `TEST_CUSTOMER`; 
DROP TABLE IF EXISTS `TEST_CUSTOMERTYPE`; 
DROP TABLE IF EXISTS `TEST_AREA`; 
 
CREATE TABLE IF NOT EXISTS `TEST_COLUMNTYPE`
(
  `ID`        integer NOT NULL AUTO_INCREMENT,
  `FBOOLEAN`   bit,
  `FBYTE`      tinyint,
  `FSHORT`     smallint,
  `FINTEGER`   int,
  `FLONG`      bigint,
  `FFLOAT`     real,
  `FDOUBLE`    double,
  `FNVARCHAR`  NVARCHAR(50),
  `FVARCHAR`   VARCHAR(50),
  `FNCLOB`     text,
  `FCLOB`      text,
  `FBLOB`      blob,
  `FTIME`      datetime,
  `FTIMESTAMP` timestamp,
    PRIMARY KEY (`ID`) 
);

 
create table IF NOT EXISTS `TEST_CUSTOMER`
(
  `CUSTOMERID`     integer NOT NULL AUTO_INCREMENT,
  `ACCOUNTTYPEID`  int not null,
  `CUSTOMERNAME`   nVARCHAR(50) not null,
  `AREAID`         int not null,
  `CUSTOMERTYPEID` int not null ,
    PRIMARY KEY (`CUSTOMERID`) 
);
 
create table IF NOT EXISTS `TEST_CUSTOMERTYPE`
(
  `CUSTOMERTYPEID`   integer NOT NULL AUTO_INCREMENT,
  `CUSTOMERTYPENAME` NVARCHAR(50) not null ,
    PRIMARY KEY (`CUSTOMERTYPEID`) 
);

 
create table IF NOT EXISTS `TEST_AREA`
(
  `AREAID`       integer NOT NULL AUTO_INCREMENT,
  `AREANAME`     NVARCHAR(50) not null default 'dft',
  `PARENTAREAID` int not null ,
    PRIMARY KEY (`AREAID`) 
);
";
                session.ExecuteNonQuery(sql);
                session.ExecuteNonQuery("insert into test_columntype (id) values (1)");
                #endregion

 
            }
        }
        /// <summary>
        /// 清除MySql
        /// </summary>
        private static void CleanupMySql()
        {
            using (IDbSession session = OpenSession())
            {
                string sql = "";

                #region 清除test_dbtype表

                sql = @" 
DROP TABLE IF EXISTS `TEST_COLUMNTYPE`;  
DROP TABLE IF EXISTS `TEST_CUSTOMER`; 
DROP TABLE IF EXISTS `TEST_CUSTOMERTYPE`; 
DROP TABLE IF EXISTS `TEST_AREA`; 

";
                session.ExecuteNonQuery(sql);

                #endregion
            }
        }



        /// <summary>
        /// 初始化oracle环境
        /// </summary>
        private static void InitializeOracle()
        {
            SqlProvider = new OracleSqlProvider();
            ConnectionString = "User ID=testtitan;Password=123456;Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST=192.168.139.131)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)))";


            using (IDbSession session = OpenSession())
            {

                string sql = "";

                #region 创建test_dbtype表
                sql = "select table_name from all_tables where table_name = 'TEST_COLUMNTYPE'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_COLUMNTYPE";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create table TEST_COLUMNTYPE
(
  ID         NUMBER(10) not null,
  FBOOLEAN   NUMBER(1),
  FBYTE      NUMBER(3),
  FSHORT     NUMBER(5),
  FINTEGER   NUMBER(10),
  FLONG      NUMBER(20),
  FFLOAT     NUMBER(20,6),
  FDOUBLE    NUMBER(38,10),
  FNVARCHAR  NVARCHAR2(50),
  FVARCHAR   VARCHAR2(50),
  FNCLOB     NCLOB,
  FCLOB      CLOB,
  FBLOB      BLOB,
  FTIME      DATE,
  FTIMESTAMP TIMESTAMP(6),
constraint pk_TEST_COLUMNTYPE primary key (ID)
)";
                session.ExecuteNonQuery(sql);
                #endregion

                #region 创建test_customer表
                sql = "select table_name from all_tables where table_name = 'TEST_CUSTOMER'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_CUSTOMER";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create table TEST_CUSTOMER
(
  CUSTOMERID     NUMBER(10) not null,
  ACCOUNTTYPEID  NUMBER(10) not null,
  CUSTOMERNAME   VARCHAR2(50) not null,
  AREAID         NUMBER(10) not null,
  CUSTOMERTYPEID NUMBER(10) not null, 
  constraint pk_TEST_CUSTOMER primary key (CUSTOMERID)
)";
                session.ExecuteNonQuery(sql);
                #endregion

                #region 创建test_customerId序列
                sql = "select sequence_name from all_sequences where sequence_name= 'TEST_CUSTOMERID'";
                if (session.RecordExists(sql))
                {
                    sql = "drop sequence TEST_CUSTOMERID";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create sequence  TEST_CUSTOMERID start with 1";
                session.ExecuteNonQuery(sql);
                #endregion

                #region 创建test_customertype表
                sql = "select table_name from all_tables where table_name = 'TEST_CUSTOMERTYPE'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_CUSTOMERTYPE";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create table TEST_CUSTOMERTYPE
(
  CUSTOMERTYPEID   NUMBER(10) not null,
  CUSTOMERTYPENAME NVARCHAR2(50) not null, 
  constraint pk_TEST_CUSTOMERTYPE primary key (CUSTOMERTYPEID)
)";
                session.ExecuteNonQuery(sql);
                #endregion

                #region 创建test_customertypeId序列
                sql = "select sequence_name from all_sequences where sequence_name= 'TEST_CUSTOMERTYPEID'";
                if (session.RecordExists(sql))
                {
                    sql = "drop sequence TEST_CUSTOMERTYPEID";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create sequence TEST_CUSTOMERTYPEID start with 1";
                session.ExecuteNonQuery(sql);
                #endregion



                #region 创建test_area表
                sql = "select table_name from all_tables where table_name = 'TEST_AREA'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_AREA";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create table TEST_AREA
(
  AREAID       NUMBER(10) not null,
  AREANAME     NVARCHAR2(50) default 'dft' not null,
  PARENTAREAID NUMBER(10) not null, 
  constraint pk_TEST_AREA primary key (AREAID)
)";
                session.ExecuteNonQuery(sql);
                #endregion

                #region 创建test_areaId序列
                sql = "select sequence_name from all_sequences where sequence_name= 'TEST_AREAID'";
                if (session.RecordExists(sql))
                {
                    sql = "drop sequence TEST_AREAID";
                    session.ExecuteNonQuery(sql);
                }
                sql = @"create sequence TEST_AREAID start with 1";
                session.ExecuteNonQuery(sql);
                #endregion


                #region 创建test_sp存储过程
                sql = "select procedure_name from all_procedures where procedure_name='TEST_SP'";
                sql = "select * from dba_source where name='TEST_SP'";
                if (session.RecordExists(sql))
                {
                    sql = "drop procedure TEST_SP";
                    session.ExecuteNonQuery(sql);
                }
                sql = "create procedure test_sp\n";
                sql = sql + "(\n";
                sql = sql + "i1 number,\n";
                sql = sql + "i2 number,\n";
                sql = sql + "i3 out number,\n";
                sql = sql + "areas out sys_refcursor,\n";
                sql = sql + "companytypes out sys_refcursor\n";
                sql = sql + ")\n";
                sql = sql + "\n";
                sql = sql + "as\n";
                sql = sql + "\n";
                sql = sql + "begin\n";
                sql = sql + "    i3:=i1+i2;\n";
                sql = sql + "    insert into test_area(areaid,areaname,parentareaid) values(test_areaid.nextval,'a1',555);\n";
                sql = sql + "    insert into test_area(areaid,areaname,parentareaid) values(test_areaid.nextval,'a2',555);\n";
                sql = sql + "    insert into test_area(areaid,areaname,parentareaid) values(test_areaid.nextval,'a3',555);\n";
                sql = sql + "    insert into test_customerType(customerTypeid,customerTypename) values(test_customerTypeid.nextval,'t1');\n";
                sql = sql + "    insert into test_customerType(customerTypeid,customerTypename) values(test_customerTypeid.nextval,'t2');\n";
                sql = sql + "    open areas for select * from test_area;\n";
                sql = sql + "    open companytypes for select * from test_customertype;\n";
                sql = sql + "\n";
                sql = sql + "end;\n";
                session.ExecuteNonQuery(sql);
                //sql = @"EXECUTE IMMEDIATE ALTER PROCEDURE TEST_SP COMPILE";
                //session.ExecuteNonQuery(sql);
                #endregion


                session.ExecuteNonQuery("insert into test_columntype (id) values (1)");

 
            }
        }
        /// <summary>
        /// 清除oracle
        /// </summary>
        private static void CleanupOracle()
        {
            using (IDbSession session = OpenSession())
            {
                string sql = "";

                #region 清除test_dbtype表

                sql = "select table_name from all_tables where table_name = 'TEST_COLUMNTYPE'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_COLUMNTYPE";
                    session.ExecuteNonQuery(sql);
                }

                #endregion

                #region 清除test_customer表

                sql = "select table_name from all_tables where table_name = 'TEST_CUSTOMER'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_CUSTOMER";
                    session.ExecuteNonQuery(sql);
                }

                #endregion

                #region 清除test_customerId序列

                sql = "select sequence_name from all_sequences where sequence_name= 'TEST_CUSTOMERID'";
                if (session.RecordExists(sql))
                {
                    sql = "drop sequence TEST_CUSTOMERID";
                    session.ExecuteNonQuery(sql);
                }

                #endregion

                #region 清除test_customertype表

                sql = "select table_name from all_tables where table_name = 'TEST_CUSTOMERTYPE'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_CUSTOMERTYPE";
                    session.ExecuteNonQuery(sql);
                }

                #endregion

                #region 清除test_customertypeId序列

                sql = "select sequence_name from all_sequences where sequence_name= 'TEST_CUSTOMERTYPEID'";
                if (session.RecordExists(sql))
                {
                    sql = "drop sequence TEST_CUSTOMERTYPEID";
                    session.ExecuteNonQuery(sql);
                }

                #endregion



                #region 清除test_area表

                sql = "select table_name from all_tables where table_name = 'TEST_AREA'";
                if (session.RecordExists(sql))
                {
                    sql = "drop table TEST_AREA";
                    session.ExecuteNonQuery(sql);
                }

                #endregion

                #region 清除test_areaId序列

                sql = "select sequence_name from all_sequences where sequence_name= 'TEST_AREAID'";
                if (session.RecordExists(sql))
                {
                    sql = "drop sequence TEST_AREAID";
                    session.ExecuteNonQuery(sql);
                }

                #endregion


                #region 清除test_sp存储过程
                sql = "select * from dba_source where name='TEST_SP'";
                if (session.RecordExists(sql))
                {
                    sql = "drop procedure TEST_SP";
                    session.ExecuteNonQuery(sql);
                }
                #endregion
            }
        }
    }
}
