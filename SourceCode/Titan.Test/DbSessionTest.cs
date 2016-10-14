using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Titan.Test.Entity;
using System.Data;
using Titan.SQLite;
//using MySql.Data.MySqlClient;
//using System.Transactions;

namespace Titan.Test
{
    
    
    /// <summary>
    ///这是 DbSessionTest 的测试类，旨在
    ///包含所有 DbSessionTest 单元测试
    ///</summary>
    [TestClass]
    public class DbSessionTest
    {
        private readonly IDbSession _session; 

        private static string[] ToStringArray(PropertyExpression[] ps)
        {
            List<string> list = new List<string>();
            foreach (PropertyExpression p in ps)
            {
                list.Add(p.PropertyName);
            }
            return list.ToArray();
        }
        public DbSessionTest()
        {
            //string desc = "客户类型CustomerType}";
            //string pattern = @"(\{[^\}]*}?)";
            //MatchCollection ms = Regex.Matches(desc, pattern);
            //GroupCollection gs = ms[0].Groups;
            //Group g = gs[1];


            _session = Environment.OpenSession(); 
        }

 


        /// <summary>
        ///Insert 的测试
        ///</summary>
        [TestMethod]
        public void InsertTest()
        { 
            CustomerType customerType = new CustomerType();
            customerType.CustomerTypeName = "Testa中文 " + Guid.NewGuid().ToString();


            //string sql = String.Format(
            //    "SELECT count(9) FROM dual WHERE EXISTS (" +
            //    "SELECT * " +
            //    "FROM all_objects " +
            //    "WHERE object_type IN ('TABLE','VIEW') " +
            //    "AND object_name = 'CODESMITH_EXTENDED_PROPERTIES' " +
            //    "AND owner = {0} )", "user");
            //object bbb = _session.ExecuteScalar(sql);


            _session.Insert(customerType);


            //取回当前id符合名称的id
            object value = _session.ExecuteScalar("select CustomerTypeId from Test_CustomerType where CustomerTypeName='" + customerType.CustomerTypeName + "'");
            int expected = ConvertDatabaseValue<int>(value);
            Assert.AreEqual(expected, customerType.CustomerTypeId);

            //取回最大id 
            value = _session.ExecuteScalar("select max(CustomerTypeId) from Test_CustomerType");
            expected = ConvertDatabaseValue<int>(value);
            Assert.AreEqual(expected, customerType.CustomerTypeId);
            
        }
        /// <summary>
        ///测试insert时排除某些字段
        ///</summary>
        [TestMethod]
        public void InsertTest_Exclude()
        {
            //customertype表的CustomerTypeName字段有默认值:'dft'
            Area area = new Area();
            area.AreaName = "Testa中文 " + Guid.NewGuid().ToString();

            _session.InsertExclude( area, new string[]{"AreaName"});
             
            //取回当前id符合名称的id
            object value = _session.ExecuteScalar("select AreaName from Test_Area where areaId=" + area.AreaId + "");
            string actual = ConvertDatabaseValue<string>(value);
            string expected = "dft";
            Assert.AreEqual(expected, actual);
             

        }


        /// <summary>
        /// 测试更新所有字段
        /// </summary>
        [TestMethod]
        public void UpdateTest()
        {
            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = 9;
            _session.Insert( obj);

            Area obj2=new Area();
            obj2.AreaId = obj.AreaId;
            obj2.AreaName = Guid.NewGuid().ToString();
            obj2.ParentAreaId = 3456;
            _session.Update( obj2,AreaProp.ALL);

            

            //取回
            string expectedName = obj2.AreaName;
            object valueName = _session.ExecuteScalar("select AreaName from Test_Area where AreaId=" + obj2.AreaId + "");
            string actualName = ConvertDatabaseValue<string>(valueName); 
            Assert.AreEqual(expectedName, actualName);

            //取回最大id 
            int expectedParentAreaId = obj2.ParentAreaId;
            object valueParentAreaId = _session.ExecuteScalar("select ParentAreaId from Test_Area where AreaId=" + obj2.AreaId + "");
            int actualParentAreaId = ConvertDatabaseValue<int>(valueParentAreaId);
            Assert.AreEqual(expectedParentAreaId, actualParentAreaId);

        }

        /// <summary>
        /// 测试更新所有字段
        /// </summary>
        [TestMethod]
        public void UpdateTest_SpecifiedProperties()
        {
            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = 9;
            _session.Insert( obj);

            Area obj2 = new Area();
            obj2.AreaId = obj.AreaId;
            obj2.AreaName = Guid.NewGuid().ToString();
            obj2.ParentAreaId = 3456;
            _session.Update( obj2, new string[]{"areaName"});



            //取回
            string expectedName = obj2.AreaName;
            object valueName = _session.ExecuteScalar("select AreaName from Test_Area where AreaId=" + obj2.AreaId + "");
            string actualName = ConvertDatabaseValue<string>(valueName);
            Assert.AreEqual(expectedName, actualName);

            //取回最大id 
            int expectedParentAreaId = obj.ParentAreaId;
            object valueParentAreaId = _session.ExecuteScalar("select ParentAreaId from Test_Area where AreaId=" + obj2.AreaId + "");
            int actualParentAreaId = ConvertDatabaseValue<int>(valueParentAreaId);
            Assert.AreEqual(expectedParentAreaId, actualParentAreaId);

        }
        [TestMethod]
        public void DeleteTest()
        {
            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = 9;
            _session.Insert( obj);

            //执行删除
            Area obj2 = new Area();
            obj2.AreaId = obj.AreaId; 
            _session.Delete( obj2);
             
             

            //是否还存在这个id  
            bool actual = _session.RecordExists("select * from Test_Area where AreaId=" + obj.AreaId + ""); 
            Assert.AreEqual(false, actual);

        }
        [TestMethod]
        public void BatchUpdateTest_SingleTable()
        {
            //先添加几条记录，然后批量修改
            int minId = 0;
            List<Area> list = new List<Area>();
            for (int i = 0; i < 5; i++)
            {
                Area obj = new Area();
                obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj.ParentAreaId = 9;
                _session.Insert( obj);
                list.Add(obj);
                if (i == 0) minId = obj.AreaId;
            }

            //执行修改
            ConditionExpressionCollection cs = new ConditionExpressionCollection();
            cs.Add("areaId", ConditionOperator.GreaterThanOrEqual, minId); 


            Area obj2 = new Area();
            obj2.AreaName = "update" + Guid.NewGuid().ToString();
            _session.BatchUpdate( obj2, cs, "areaname");



            //是否还存在这个id  
            foreach (Area item in list)
            {
                string expected = obj2.AreaName;
                object value = _session.ExecuteScalar("select AreaName from Test_Area where AreaId=" + item.AreaId + "");
                string actual = ConvertDatabaseValue<string>(value);
                Assert.AreEqual(expected, actual);
            }

        }

        [TestMethod]
        public void BatchUpdateTest_LinkTable()
        {
            //先添加几条记录，然后批量修改

            Area parentArea = new Area();
            parentArea.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            _session.Insert( parentArea);


            int minId = 0; 
            List<Area> list = new List<Area>();
            for (int i = 0; i < 5; i++)
            {
                Area obj = new Area();
                obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj.ParentAreaId = parentArea.AreaId;//将它设置为父对象
                _session.Insert( obj);
                list.Add(obj);
                if (i == 0) minId = obj.AreaId;
            }

            //执行修改
            ConditionExpressionCollection cs = new ConditionExpressionCollection();
            cs.Add("ParentArea.areaName", ConditionOperator.Equal, parentArea.AreaName);


            Area obj2 = new Area();
            obj2.AreaName = "update" + Guid.NewGuid().ToString();

            try
            {
                _session.BatchUpdate( obj2, cs, "areaname");
            }
            catch(Exception ex)
            {
                //sqlite不支持关联更新
                if (Environment.SqlProvider.GetType() == typeof(SQLiteSqlProvider))
                {
                    Assert.AreEqual(1, 1);
                    return;
                }
                else
                {
                    throw ex;
                }
            }



            //是否还存在这个id  
            foreach (Area item in list)
            {
                string expected = obj2.AreaName;
                object value = _session.ExecuteScalar("select AreaName from Test_Area where AreaId=" + item.AreaId + "");
                string actual = ConvertDatabaseValue<string>(value);
                Assert.AreEqual(expected, actual);
            }

        }

        [TestMethod]
        public void BatchDeleteTest_SingleTable()
        {
            //先添加几条记录，然后批量修改
            int minId = 0;
            List<Area> list = new List<Area>();
            for (int i = 0; i < 5; i++)
            {
                Area obj = new Area();
                obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj.ParentAreaId = 9;
                _session.Insert( obj);
                list.Add(obj);
                if (i == 0) minId = obj.AreaId;
            }

            //执行删除
            ConditionExpressionCollection cs = new ConditionExpressionCollection();
            cs.Add("areaId",ConditionOperator.GreaterThanOrEqual, minId); 
            _session.BatchDelete<Area>( cs );



            //是否还存在  
            bool actual = _session.RecordExists("select AreaName from Test_Area where AreaId>=" + minId + "");
            bool expected = false;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void BatchDeleteTest_LinkTable()
        {
           

            //先添加几条记录，然后批量删除

            Area parentArea = new Area();
            parentArea.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            _session.Insert( parentArea);


            int minId = 0;
            List<Area> list = new List<Area>();
            for (int i = 0; i < 5; i++)
            {
                Area obj = new Area();
                obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj.ParentAreaId = parentArea.AreaId;//将它设置为父对象
                _session.Insert( obj);
                list.Add(obj);
                if (i == 0) minId = obj.AreaId;
            }

            //执行删除
            ConditionExpressionCollection cs = new ConditionExpressionCollection();
            cs.Add("ParentArea.areaName", ConditionOperator.Equal, parentArea.AreaName);
            try
            {
                _session.BatchDelete<Area>( cs);
            }
            catch (Exception ex)
            {
                //sqlite不支持关联更新
                if (Environment.SqlProvider.GetType() == typeof(SQLiteSqlProvider))
                {
                    Assert.AreEqual(1, 1);
                    return;
                }
                else
                {
                    throw ex;
                }
            }



            //是否还存在  
            bool actual = _session.RecordExists("select AreaName from Test_Area where AreaId>=" + minId + "");
            bool expected = false;
            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void SelectTest()
        {

            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = 9;
            _session.Insert( obj);


            Area obj2 = new Area();
            obj2.AreaId = obj.AreaId;
            bool actual= _session.Select( obj2,AreaProp.ALL);

            Assert.AreEqual(obj.AreaName, obj2.AreaName);
            Assert.AreEqual(obj.ParentAreaId, obj2.ParentAreaId);
            //测试返回
            Assert.AreEqual(true, actual);
        }

        

        [TestMethod]
        public void SelectTest_SpecifiedProperties()
        {

            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = 9;
            _session.Insert( obj);

            Area obj2 = new Area();
            obj2.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj2.ParentAreaId = obj.AreaId;
            _session.Insert( obj2);


            Area obj9 = new Area();
            obj9.AreaId = obj2.AreaId;
            obj9.AreaName = Guid.NewGuid().ToString();
            obj9.ParentAreaId = int.MinValue;
            obj9.ParentArea = new Area();
            obj9.ParentArea.ParentAreaId = int.MinValue;//判断ParentArea是否有被重新实例化
            obj9.ParentArea.AreaName = Guid.NewGuid().ToString();
            _session.Select( obj9,new string[]{ "areaname","parentarea.areaname"});

            Assert.AreEqual(obj2.AreaName, obj9.AreaName);
            Assert.AreEqual(obj.AreaName, obj9.ParentArea.AreaName);
            Assert.AreEqual(int.MinValue, obj9.ParentArea.ParentAreaId);

        }


        [TestMethod]
        public void SelectCountAndExistsTest_LinkTable()
        {
            int expected = 5;

            //第一次添加
            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = int.MinValue;
            _session.Insert( obj);

            for(int i=0;i<expected;i++){
                Area obj2 = new Area();
                obj2.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj2.ParentAreaId = obj.AreaId;
                _session.Insert( obj2);
            }

            //第二次添加
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            _session.Insert( obj); 
            for (int i = 0; i < expected; i++)
            {
                Area obj2 = new Area();
                obj2.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj2.ParentAreaId = obj.AreaId;
                _session.Insert( obj2);
            }

            ConditionExpressionCollection cs=new ConditionExpressionCollection();
            cs.Add("parentarea.areaname",ConditionOperator.Equal, obj.AreaName);

            int actual = _session.SelectCount<Area>(cs);

            Assert.AreEqual(expected, actual);
             
            bool actualExists = _session.Exists<Area>( cs);
            Assert.AreEqual(true, actualExists);
        }


        [TestMethod]
        public void SelectCountAndExistsTest_SingleTable()
        {
            int expected = 5;

            //第一次添加
            Area obj = new Area();
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            obj.ParentAreaId = int.MinValue;
            _session.Insert( obj);

            for (int i = 0; i < expected; i++)
            {
                Area obj2 = new Area();
                obj2.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj2.ParentAreaId = obj.AreaId;
                _session.Insert( obj2);
            }

            //第二次添加
            obj.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
            _session.Insert( obj);
            for (int i = 0; i < expected; i++)
            {
                Area obj2 = new Area();
                obj2.AreaName = "Testa中文 " + Guid.NewGuid().ToString();
                obj2.ParentAreaId = obj.AreaId;
                _session.Insert( obj2);
            }

            ConditionExpressionCollection cs = new ConditionExpressionCollection();
            cs.Add("parentareaid", ConditionOperator.Equal, obj.AreaId);

            int actual = _session.SelectCount<Area>( cs);

            Assert.AreEqual(expected, actual);


            bool actualExists = _session.Exists<Area>( cs);
            Assert.AreEqual(true, actualExists);
        }

        [TestMethod]
        public void SelectCollectionTest()
        {
            Random r=new Random();

            #region 初始化一些数据
            #region 添加2个CustomerType
            List<CustomerType> customerTypes=new List<CustomerType>();

            CustomerType customerType1 = new CustomerType();
            customerType1.CustomerTypeName=Guid.NewGuid().ToString();
            _session.Insert( customerType1);
            customerTypes.Add(customerType1);

            CustomerType customerType2 = new CustomerType();
            customerType2.CustomerTypeName=Guid.NewGuid().ToString();
            _session.Insert( customerType2);
            customerTypes.Add(customerType2);
            #endregion
            #region 添加2个Area
            List<Area> areas=new List<Area>();

            Area area1 = new Area();
            area1.AreaName=Guid.NewGuid().ToString();
            _session.Insert( area1);
            customerTypes.Add(customerType1);
            areas.Add(area1);

            Area area2 = new Area();
            area2.ParentAreaId = area1.AreaId;
            area2.AreaName = Guid.NewGuid().ToString();
            _session.Insert( area2);
            areas.Add(area2);
            #endregion
            #region 添加200个Customer
            for(int i=0;i<200;i++)
            { 
                Customer customer=new Customer();
                customer.AccountTypeId = (AccountTypeId)r.Next(1, 4);
                customer.AreaId = areas[r.Next(0, 2)].AreaId;
                customer.CustomerName = Guid.NewGuid().ToString();
                customer.CustomerTypeId = customerTypes[r.Next(0, 2)].CustomerTypeId;
                _session.Insert(customer);
            }
            #endregion
            #endregion

            QueryExpression q;
            List<Customer> result;
            int actualCount = 0;
            int expectedCount = 0;
            #region 不分页，返回count，是否正确
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Selects.Add("CustomerId", "AccountTypeId", "CustomerName", "AreaId", "CustomerTypeId");
            q.Selects.Add("Area.AreaId", "Area.AreaName", "Area.ParentAreaId");
            q.Wheres.Add("area.areaname", ConditionOperator.Equal, areas[0].AreaName);
            q.Wheres.Add("customertypeid", ConditionOperator.Equal, customerTypes[0].CustomerTypeId);
            q.ReturnMatchedCount = true;
            result = new List<Customer>();
            result.Clear();
            actualCount = _session.SelectCollection( result, q);
            expectedCount = ConvertDatabaseValue<int>(_session.ExecuteScalar("select count(*) from test_customer a left outer join test_area b on a.areaid=b.areaid where a.customertypeid=" + customerTypes[0].CustomerTypeId + " and b.areaname='" + areas[0].AreaName + "'"));
            Assert.AreEqual(expectedCount, actualCount);
            #endregion
            #region 分页，返回count，是否正确 
            q.PageIndex = 2;
            q.PageSize = 3;
            q.ReturnMatchedCount = true;
            result.Clear();
            actualCount = _session.SelectCollection( result, q);
            expectedCount = ConvertDatabaseValue<int>(_session.ExecuteScalar("select count(*) from test_customer a left outer join test_area b on a.areaid=b.areaid where a.customertypeid=" + customerTypes[0].CustomerTypeId + " and b.areaname='" + areas[0].AreaName + "'"));
            Assert.AreEqual(expectedCount, actualCount);
            #endregion
            #region 分页，不返回count，是否正确
            q.PageIndex = 2;
            q.PageSize = 3;
            q.ReturnMatchedCount = false;
            result.Clear();
            actualCount = _session.SelectCollection ( result, q);
            expectedCount = result.Count;
            Assert.AreEqual(expectedCount, actualCount);
            #endregion 

            
            #region 分页
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Selects.Add("area.areaname");
            q.Selects.Add("customername");
            q.Wheres.Add("area.areaname", ConditionOperator.Equal, areas[0].AreaName);
            q.Wheres.Add("customertypeid", ConditionOperator.Equal, customerTypes[0].CustomerTypeId);
            q.OrderBys.Add("customername",OrderType.Desc);
            q.PageIndex = 2;
            q.PageSize = 3;
            q.ReturnMatchedCount = false;
            result = new List<Customer>();
            actualCount = _session.SelectCollection ( result, q);

            string actualKey = result[0].CustomerName + "_" + result[1].CustomerName +"_"+ result[2].CustomerName;
            DataTable table = _session.ExecuteDataTable("select a.customerName,b.areaname from test_customer a left outer join test_area b on a.areaid=b.areaid where a.customertypeid=" + customerTypes[0].CustomerTypeId + " and b.areaname='" + areas[0].AreaName + "' order by a.customername desc");
            string expectedKey = table.Rows[3]["customerName"] + "_" + table.Rows[4]["customerName"] + "_" + table.Rows[5]["customerName"];
            Assert.AreEqual(expectedKey, actualKey);

            //父对象
            actualKey = result[0].Area.AreaName + "_" + result[1].Area.AreaName + "_" + result[2].Area.AreaName;
            expectedKey = table.Rows[3]["areaname"] + "_" + table.Rows[4]["areaname"] + "_" + table.Rows[5]["areaname"];
            Assert.AreEqual(expectedKey, actualKey);
            #endregion

            #region 分页_缓存测试
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Selects.Add("customername");//输出顺序换一下
            q.Selects.Add("area.areaname");//输出顺序换一下
            q.Wheres.Add("area.areaname", ConditionOperator.Equal, areas[0].AreaName);
            q.Wheres.Add("customertypeid", ConditionOperator.Equal, customerTypes[0].CustomerTypeId);
            q.OrderBys.Add("customername", OrderType.Desc);
            q.PageIndex = 2;
            q.PageSize = 4;
            q.ReturnMatchedCount = false;
            result = new List<Customer>();
            actualCount = _session.SelectCollection ( result, q);
            //看日志中的sql语句

            //actualKey = result[0].CustomerName + "_" + result[1].CustomerName + "_" + result[2].CustomerName;
            //table = _session.ExecuteDataTable("select a.customerName,b.areaname from test_customer a left outer join test_area b on a.areaid=b.areaid where a.customertypeid=" + customerTypes[0].CustomerTypeId + " and b.areaname='" + areas[0].AreaName + "' order by a.customername desc");
            //expectedKey = table.Rows[3]["customerName"] + "_" + table.Rows[4]["customerName"] + "_" + table.Rows[5]["customerName"];
            //Assert.AreEqual(expectedKey, actualKey);

            ////父对象
            //actualKey = result[0].Area.AreaName + "_" + result[1].Area.AreaName + "_" + result[2].Area.AreaName;
            //expectedKey = table.Rows[3]["areaname"] + "_" + table.Rows[4]["areaname"] + "_" + table.Rows[5]["areaname"];
            //Assert.AreEqual(expectedKey, actualKey);
            #endregion
        }

        [TestMethod]
        public void SelectCollectionGroupTest()
        {
            Random r = new Random();

            #region 初始化一些数据
            #region 添加2个CustomerType
            List<CustomerType> customerTypes = new List<CustomerType>();

            CustomerType customerType1 = new CustomerType();
            customerType1.CustomerTypeName = Guid.NewGuid().ToString();
            _session.Insert( customerType1);
            customerTypes.Add(customerType1);

            CustomerType customerType2 = new CustomerType();
            customerType2.CustomerTypeName = Guid.NewGuid().ToString();
            _session.Insert( customerType2);
            customerTypes.Add(customerType2);
            #endregion
            #region 添加51个Area，第一个固定
            List<Area> areas = new List<Area>();

            Area area1 = new Area();
            area1.AreaName = Guid.NewGuid().ToString();
            _session.Insert( area1);
            customerTypes.Add(customerType1);
            areas.Add(area1);

            for (int i = 0; i < 50; i++)
            {
                Area area2 = new Area();
                area2.ParentAreaId = area1.AreaId;
                area2.AreaName = Guid.NewGuid().ToString();
                _session.Insert( area2);
                areas.Add(area2);
            }
            #endregion
            #region 添加200个Customer
            for (int i = 0; i < 200; i++)
            {
                Customer customer = new Customer();
                customer.AccountTypeId = (AccountTypeId)r.Next(1, 4);
                customer.AreaId = areas[r.Next(0, 51)].AreaId;
                customer.CustomerName = Guid.NewGuid().ToString();
                customer.CustomerTypeId = customerTypes[r.Next(0, 2)].CustomerTypeId;
                _session.Insert( customer);
            }
            #endregion
            #endregion

            QueryExpression q;
            List<Customer> result;


            #region 不分页，不返回count，验证sum是否正确
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Select(CustomerProp.AreaId);
            q.Select(PropertyExpression.Count);
            q.Select(CustomerProp.CustomerId.Sum);
            q.Where(CustomerProp.CustomerType.CustomerTypeName == customerTypes[0].CustomerTypeName);
            q.GroupBy(CustomerProp.AreaId);
            q.OrderBy(PropertyExpression.Count.Desc);
            q.Having(CustomerProp.CustomerId.Sum > 0);
            result = new List<Customer>();
            int actualTotalCount1 = _session.SelectCollection ( result, q);
            int actualSum1 = 0;
            foreach (Customer obj in result)
            {
                actualSum1 += obj.Sum_CustomerId;
            }
            int expectedSum1 = ConvertDatabaseValue<int>(_session.ExecuteScalar("select sum(customerId) from test_customer  where CustomerTypeId=" + customerTypes[0].CustomerTypeId + " "));
            Assert.IsTrue(actualSum1 == expectedSum1);
            #endregion

            #region 分页，不返回count，验证分页是否正确
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Select(CustomerProp.AreaId);
            q.Select(PropertyExpression.Count);
            q.Select(CustomerProp.CustomerId.Sum);
            q.Where(CustomerProp.CustomerType.CustomerTypeName == customerTypes[0].CustomerTypeName);
            q.GroupBy(CustomerProp.AreaId);
            q.OrderBy(CustomerProp.CustomerId.Sum.Desc);
            q.Having(CustomerProp.CustomerId.Sum > 0);
            q.PageSize = 5;
            q.PageIndex = 1;
            result = new List<Customer>();
            int actualTotalCount2 = _session.SelectCollection ( result, q);
            int actualSum2 = 0;
            foreach (Customer obj in result)
            {
                actualSum2 += obj.Sum_CustomerId;
            }
            IDataReader reader = _session.ExecuteReader("select sum(customerId) from test_customer  where CustomerTypeId=" + customerTypes[0].CustomerTypeId + " group by areaid order by sum(customerId) desc");
            int expectedSum2 = 0;
            int index = 0;//只读取x个
            while (reader.Read())
            {
                expectedSum2 += reader.GetInt32(0);
                index++;
                if (index >= q.PageSize) break;
            }
            reader.Close();
            Assert.IsTrue(actualSum2 == expectedSum2);
            #endregion 


            #region 验证count
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Select(CustomerProp.AreaId);
            q.Select(PropertyExpression.Count);
            q.Select(CustomerProp.CustomerId.Sum);
            q.Where(CustomerProp.CustomerType.CustomerTypeName == customerTypes[0].CustomerTypeName);
            q.GroupBy(CustomerProp.AreaId);
            q.OrderBy(CustomerProp.CustomerId.Sum.Desc);
            q.Having(PropertyExpression.Count > 4);
            q.ReturnMatchedCount = true;
            result = new List<Customer>();
            int actualTotalCount3 = _session.SelectCollection ( result, q);
            int expectedTotalCount3 = ConvertDatabaseValue<int>(_session.ExecuteScalar("select count(*) from (select areaid from test_customer  where CustomerTypeId=" + customerTypes[0].CustomerTypeId + " group by areaid having count(*)>4) a"));
            Assert.IsTrue(actualTotalCount3 == expectedTotalCount3);
            #endregion

        }
        /// <summary>
        /// 测试like是否会自动增加%
        /// 需要查看sql 跟踪
        /// </summary>
        [TestMethod]
        public void WhereLikeTest()
        {
            Random r = new Random();

            
            QueryExpression q; 
            int actualCount = 0;
            //int expectedCount = 0;
            #region 不分页，返回count是否正确
            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Selects.Add("customerId");
            q.Wheres.Add("area.areaname", ConditionOperator.Like, "A"); 
            q.ReturnMatchedCount = true;
            List<Customer> result = new List<Customer>();
            actualCount = _session.SelectCollection ( result, q);
            Assert.AreEqual(0, 0);
            #endregion
             
        }
        [TestMethod]
        public void EnumTest()
        {
            Customer obj = new Customer();
            obj.CustomerName = Guid.NewGuid().ToString();
            obj.AccountTypeId = AccountTypeId.Opened;
            _session.Insert( obj);

            Customer obj2 = new Customer();
            obj2.CustomerId = obj.CustomerId;
            _session.Select( obj2,ToStringArray(CustomerProp.ALL));
            AccountTypeId expected = AccountTypeId.Opened;
            AccountTypeId actual = obj2.AccountTypeId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// 递归QueryExpression
        /// </summary>
        [TestMethod]
        public void SelectCollectionTest_Recursion()
        {
           
            QueryExpression q;
            List<Customer> result;
            int actualCount = 0;
            int expectedCount = 0;
            #region 不分页，返回count是否正确

            QueryExpression qinner=new QueryExpression();
            qinner.EntityType = typeof(CustomerType);
            qinner.Selects.Add("CustomerTypeId");
            qinner.Wheres.Add("CustomerTypeId", ConditionOperator.GreaterThan, 5);
            qinner.Wheres.Add("CustomerTypeId", ConditionOperator.GreaterThan, 6);


            q = new QueryExpression();
            q.EntityType = typeof(Customer);
            q.Selects.Add("CustomerId", "AccountTypeId", "CustomerName", "AreaId", "CustomerTypeId");
            q.Selects.Add("Area.AreaId", "Area.AreaName", "Area.ParentAreaId");
            q.Wheres.Add("area.areaname", ConditionOperator.Equal, "area123");
            q.Wheres.Add("customertypeid", ConditionOperator.In, qinner);
            q.ReturnMatchedCount = true;
            result = new List<Customer>();
            actualCount = _session.SelectCollection ( result, q);
            Assert.AreEqual(expectedCount, actualCount);
            #endregion  
        }
        //[TestMethod]
        //public void TestValidate()
        //{

        //    bool errorMatched = false;


        //    //测试长度验证
        //    Customer obj = new Customer();
        //    obj.CustomerName = new string('a', 51);
        //    obj.AccountTypeId = AccountTypeId.Opened; 
        //    try
        //    {
        //        _session.Insert( obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMatched = ex.Message.IndexOf("超出最大长度") >= 0;
        //    }
        //    Assert.AreEqual(errorMatched, true);


        //    //测试null验证
        //    errorMatched = false;

        //    obj = new Customer();
        //    obj.CustomerName = null;
        //    obj.AccountTypeId = AccountTypeId.Opened;
        //    try
        //    {
        //        _session.Insert( obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMatched = ex.Message.IndexOf("不允许空") >= 0;
        //    }
        //    Assert.AreEqual(errorMatched, true);
        //}


        private T ConvertDatabaseValue<T>(object databaseValue)
        {
            return (T)_session.SqlProvider.ConvertDbValue(databaseValue, typeof(T));
        }
    }
}
