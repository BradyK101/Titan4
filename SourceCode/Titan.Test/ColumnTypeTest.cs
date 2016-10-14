using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Titan.SqlServer;
using Titan.Test.Entity;
using System.Text;
using System.Collections.Generic;

namespace Titan.Test
{
    
    
    /// <summary>
    ///测试字段类型
    ///</summary>
    [TestClass]
    public class ColumnTypeTest
    { 
        private readonly IDbSession _session;
        //private readonly Dictionary<string, string> _tableMapping;

        public ColumnTypeTest()
        {
            _session = Environment.OpenSession();
            //_tableMapping = Environment.TableMapping;
            //_session.ExecuteNonQuery("truncate table test_columntype");
            //_session.ExecuteNonQuery("insert into test_columntype (id) values (1)");
        }

        [TestMethod]
        public void TestNTextEqual()
        {
            QueryExpression q = new QueryExpression();
            q.EntityType = typeof(ColumnType);
            q.Selects.Add(ColumnTypeProp.ALL);
            //q.Wheres.Add(ColumnType_.FNClob.TEqual("aaaa"));
            //q.Wheres.Add(ColumnType_.FNVarChar.TEqual("bbbb"));
            q.Wheres.Add("FNClob", ConditionOperator.Equal, "aaaa");
            q.Wheres.Add("FNVarChar",ConditionOperator.Equal,"bbbb" );
             
            List<ColumnType> list=new List<ColumnType>();
            if (_session.SqlProvider is SqlServerSqlProvider)
            {
                _session.SelectCollection( list, q);
            }
 
             
            Assert.AreEqual(1, 1);

        }

        [TestMethod]
        public void TestBoolean()
        { 
            ColumnType obj = new ColumnType {Id = 1, FBoolean = true}; 
            _session.Update( obj,ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);

            bool expected = obj.FBoolean.Value;
            bool actual = obj2.FBoolean.Value; 
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void TestInteger()
        { 
            ColumnType obj = new ColumnType {Id = 1, FInteger = 2};
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);

            int expected = obj.FInteger.Value;
            int actual = obj2.FInteger.Value;
            Assert.AreEqual(expected, actual);
             

        }

        [TestMethod]
        public void TestFloat()
        { 


            ColumnType obj = new ColumnType {Id = 1, FFloat = 2.12345F};

            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);


            float expected = obj.FFloat.Value;
            float actual = obj2.FFloat.Value;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestDouble()
        { 

            ColumnType obj = new ColumnType {Id = 1, FDouble = 2.987654D};
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);

            double expected = obj.FDouble.Value;
            double actual = obj2.FDouble.Value;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TestByte()
        { 

            ColumnType obj = new ColumnType {Id = 1, FByte = (byte) 3};
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);


            byte expected = obj.FByte.Value;
            byte actual = obj2.FByte.Value;
            Assert.AreEqual(expected, actual);

        }




        [TestMethod]
        public void TestDateTime()
        { 
            ColumnType obj = new ColumnType { Id = 1, FTime = DateTime.Now };
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);


            string expected = obj.FTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string actual = obj2.FTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            Assert.AreEqual(expected, actual);  

        }





        [TestMethod]
        public void TestNVarChar()
        {
             
            ColumnType obj = new ColumnType {Id = 1, FNVarChar = "n中"};
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);
            string expected = obj.FNVarChar;
            string actual = obj2.FNVarChar;
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void TestVarChar()
        { 
            ColumnType obj = new ColumnType {Id = 1, FVarChar = "v中"};
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);


            string expected = obj.FVarChar;
            string actual = obj2.FVarChar;
            Assert.AreEqual(expected, actual);

        }



        [TestMethod]
        public void TestNClob()
        { 


            ColumnType obj = new ColumnType {Id = 1}; 
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 20000; i++)
            {
                sb.Append('啊');
            }
            obj.FNClob = sb.ToString();
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);

            string expected = obj.FNClob;
            string actual = obj2.FNClob;
            Assert.AreEqual(expected, actual);  
        }



        [TestMethod]
        public void TestClob()
        { 


            ColumnType obj = new ColumnType {Id = 1};

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 20000; i++)
            {
                sb.Append('啊');
            }
            obj.FClob = sb.ToString();
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);


            string expected = obj.FClob;
            string actual = obj2.FClob;
            Assert.AreEqual(expected, actual);  

        }

        [TestMethod]
        public void TestBlob()
        {  
            ColumnType obj = new ColumnType {Id = 1};

            byte[] bytes = new byte[20000];
            for (int i = 0; i < 20000; i++)
            {
                bytes[i] = (byte)i;
            }
            obj.FBlob = bytes;
            _session.Update( obj, ColumnTypeProp.ALLNONEKEY);

            ColumnType obj2 = new ColumnType {Id = 1};
            _session.Select( obj2, ColumnTypeProp.ALLNONEKEY);

            bool hasDiffrence = false;
            for (int i = 0; i < 20000; i++)
            {
                if (obj2.FBlob[i] != bytes[i])
                {
                    hasDiffrence = true;
                    break;
                }
            }
             
            bool actual = hasDiffrence;
            Assert.AreEqual(false, actual);  

        }
    }
}
