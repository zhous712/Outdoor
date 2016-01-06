using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using AutoRadio.RadioSmart.Common.Data;
using AutoRadio.RadioSmart.Common;
using Model;

namespace Dal
{
    public class Monitor
    {
        private string OutdoorMonitor = "OutdoorMonitor";
        /// <summary>
        /// 根据用户名获得用户信息
        /// </summary>
        /// <param name="cusName"></param>
        /// <returns></returns>
        public Model.Customer GetCustomerInfoByName(string cusName)
        {
            Model.Customer model = new Model.Customer();
            string sql = @"SELECT * FROM dbo.Customer WHERE CusName=@CusName";
            SqlParameter[] parameters = {
                    new SqlParameter("@CusName", SqlDbType.NVarChar,100)};
            parameters[0].Value = cusName;
            DataTable dt = SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                model.CusId = Int32.Parse(dt.Rows[0]["CusId"].ToString());
                model.CusName = dt.Rows[0]["CusName"].ToString();
                model.cPassWord = dt.Rows[0]["cPassWord"].ToString();
                model.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                model.CustomerType = ConvertHelper.GetInteger(dt.Rows[0]["CustomerType"].ToString());
                model.Email = dt.Rows[0]["Email"].ToString();
                model.Phone = dt.Rows[0]["Phone"].ToString();
                model.IsDisabled = ConvertHelper.GetInteger(dt.Rows[0]["IsDisabled"].ToString());
                model.CreateTime = ConvertHelper.GetDateTime(dt.Rows[0]["CreateTime"].ToString());
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名获得用户信息
        /// </summary>
        /// <param name="cusName"></param>
        /// <returns></returns>
        public Model.Customer GetCustomerInfoByNameAndType(string cusName, int customerType)
        {
            Model.Customer model = new Model.Customer();
            string sql = @"SELECT * FROM dbo.Customer WHERE CusName=@CusName AND CustomerType=@CustomerType";
            SqlParameter[] parameters = {
                    new SqlParameter("@CusName", SqlDbType.NVarChar,100),
                    new SqlParameter("@CustomerType", SqlDbType.Int)};
            parameters[0].Value = cusName;
            parameters[1].Value = customerType;
            DataTable dt = SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                model.CusId = Int32.Parse(dt.Rows[0]["CusId"].ToString());
                model.CusName = dt.Rows[0]["CusName"].ToString();
                model.cPassWord = dt.Rows[0]["cPassWord"].ToString();
                model.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                model.CustomerType = ConvertHelper.GetInteger(dt.Rows[0]["CustomerType"].ToString());
                model.Email = dt.Rows[0]["Email"].ToString();
                model.Phone = dt.Rows[0]["Phone"].ToString();
                model.IsDisabled = ConvertHelper.GetInteger(dt.Rows[0]["IsDisabled"].ToString());
                model.CreateTime = ConvertHelper.GetDateTime(dt.Rows[0]["CreateTime"].ToString());
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户id获得用户信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public Model.Customer GetCustomerInfoByUserId(int cusId)
        {
            Model.Customer model = new Model.Customer();
            string sql = @"SELECT * FROM dbo.Customer WHERE CusId=@CusId";
            SqlParameter[] parameters = {
                    new SqlParameter("@CusId", SqlDbType.Int)};
            parameters[0].Value = cusId;
            DataTable dt = SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                model.CusId = Int32.Parse(dt.Rows[0]["CusId"].ToString());
                model.CusName = dt.Rows[0]["CusName"].ToString();
                model.cPassWord = dt.Rows[0]["cPassWord"].ToString();
                model.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                model.CustomerType = ConvertHelper.GetInteger(dt.Rows[0]["CustomerType"].ToString());
                model.Email = dt.Rows[0]["Email"].ToString();
                model.Phone = dt.Rows[0]["Phone"].ToString();
                model.IsDisabled = ConvertHelper.GetInteger(dt.Rows[0]["IsDisabled"].ToString());
                model.CreateTime = ConvertHelper.GetDateTime(dt.Rows[0]["CreateTime"].ToString());
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询企业客户信息
        /// </summary>
        /// <param name="entName"></param>
        /// <returns></returns>
        public DataTable GetCustInfo(string entName, int customerType)
        {
            string sql = @"SELECT CusId,CusName FROM dbo.Customer where CusName like '%'+@entName+'%' AND IsDisabled=0 AND CustomerType=@CustomerType";
            SqlParameter[] parameters = {
                    new SqlParameter("@entName", entName),
                    new SqlParameter("@customerType", customerType)};
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="cusName"></param>
        /// <param name="taskName"></param>
        /// <param name="sid"></param>
        /// <param name="excelPath"></param>
        /// <param name="picturePath"></param>
        /// <returns></returns>
        public int AddTask(int cusId, string cusName, string taskName, int sid, string excelPath, string picturePath)
        {
            string sql = @"INSERT INTO dbo.Task ( CusId ,CusName ,TaskName ,SId, ExcelPath ,PicturePath) VALUES  ( @CusId ,@CusName ,@TaskName ,@SId, @ExcelPath ,@PicturePath ) SELECT @@IDENTITY";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@CusId",SqlDbType.Int),
                    new SqlParameter("@CusName",SqlDbType.NVarChar,50), 
                    new SqlParameter("@TaskName",SqlDbType.NVarChar,100),
                    new SqlParameter("@SId",SqlDbType.NVarChar,100),
                    new SqlParameter("@ExcelPath",SqlDbType.NVarChar,100),
                    new SqlParameter("@PicturePath",SqlDbType.NVarChar,200)
                };
            parameters[0].Value = cusId;
            parameters[1].Value = cusName;
            parameters[2].Value = taskName;
            parameters[3].Value = sid;
            parameters[4].Value = excelPath;
            parameters[5].Value = picturePath;
            var taskId = ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
            return taskId;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteTask(int tid)
        {
            string sql = @"DELETE FROM dbo.TaskProject WHERE TId=@TId;DELETE FROM dbo.Task WHERE TId=@TId;";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TId",SqlDbType.Int)
                };
            parameters[0].Value = tid;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 修改主任务状态
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int ReleaseTask(int tid, int status)
        {
            string sql = @"UPDATE dbo.Task SET STATUS=@STATUS WHERE TId=@TId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TId",SqlDbType.Int),
                    new SqlParameter("@STATUS",SqlDbType.Int),
                };
            parameters[0].Value = tid;
            parameters[1].Value = status;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 添加任务排期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddTaskProject(TaskProject model)
        {
            string sql = @"INSERT INTO dbo.TaskProject(TId ,StreetAddress ,RegionId ,AreaName ,BlockName ,PointName ,MediaType ,AdProductName ,BeginDate ,EndDate ,PhotoRequire ,Status ,Price,SpareOne,SpareTwo)
 VALUES  (@TId ,@StreetAddress ,@RegionId ,@AreaName ,@BlockName ,@PointName ,@MediaType ,@AdProductName ,@BeginDate ,@EndDate ,@PhotoRequire ,@Status ,@Price,@SpareOne,@SpareTwo)";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TId",SqlDbType.Int),
                    new SqlParameter("@StreetAddress",SqlDbType.NVarChar,100), 
                    new SqlParameter("@RegionId",SqlDbType.VarChar,12),
                    new SqlParameter("@AreaName",SqlDbType.NVarChar,50),
                    new SqlParameter("@BlockName",SqlDbType.NVarChar,50),
                    new SqlParameter("@PointName",SqlDbType.NVarChar,100),
                    new SqlParameter("@MediaType",SqlDbType.NVarChar,50), 
                    new SqlParameter("@AdProductName",SqlDbType.NVarChar,50),
                    new SqlParameter("@BeginDate",SqlDbType.DateTime),
                    new SqlParameter("@EndDate",SqlDbType.DateTime),
                    new SqlParameter("@PhotoRequire",SqlDbType.NVarChar,100),
                    new SqlParameter("@Status",SqlDbType.Int), 
                    new SqlParameter("@Price",SqlDbType.Decimal),
                    new SqlParameter("@SpareOne",SqlDbType.NVarChar,100),
                    new SqlParameter("@SpareTwo",SqlDbType.NVarChar,100)
                };
            parameters[0].Value = model.TId;
            parameters[1].Value = model.StreetAddress;
            parameters[2].Value = model.RegionId;
            parameters[3].Value = model.AreaName;
            parameters[4].Value = model.BlockName;
            parameters[5].Value = model.PointName;
            parameters[6].Value = model.MediaType;
            parameters[7].Value = model.AdProductName;
            parameters[8].Value = model.BeginDate;
            parameters[9].Value = model.EndDate;
            parameters[10].Value = model.PhotoRequire;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.Price;
            parameters[13].Value = model.SpareOne;
            parameters[14].Value = model.SpareTwo;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 更新任务点位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateTaskProject(TaskProject model)
        {
            string sql = @"UPDATE dbo.TaskProject SET StreetAddress=@StreetAddress,RegionId=@RegionId,AreaName=@AreaName,BlockName=@BlockName,PointName=@PointName,MediaType=@MediaType,AdProductName=@AdProductName,BeginDate=@BeginDate,EndDate=@EndDate,PhotoRequire=@PhotoRequire,Price=@Price,AbnormalType=@AbnormalType WHERE TPId=@TPId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@StreetAddress",SqlDbType.NVarChar,100), 
                    new SqlParameter("@RegionId",SqlDbType.VarChar,12),
                    new SqlParameter("@AreaName",SqlDbType.NVarChar,50),
                    new SqlParameter("@BlockName",SqlDbType.NVarChar,50),
                    new SqlParameter("@PointName",SqlDbType.NVarChar,100),
                    new SqlParameter("@MediaType",SqlDbType.NVarChar,50), 
                    new SqlParameter("@AdProductName",SqlDbType.NVarChar,50),
                    new SqlParameter("@BeginDate",SqlDbType.DateTime),
                    new SqlParameter("@EndDate",SqlDbType.DateTime),
                    new SqlParameter("@PhotoRequire",SqlDbType.NVarChar,100),
                    new SqlParameter("@Price",SqlDbType.Decimal),
                    new SqlParameter("@AbnormalType",SqlDbType.Int),
                    new SqlParameter("@TPId",SqlDbType.Int)
                };
            parameters[0].Value = model.StreetAddress;
            parameters[1].Value = model.RegionId;
            parameters[2].Value = model.AreaName;
            parameters[3].Value = model.BlockName;
            parameters[4].Value = model.PointName;
            parameters[5].Value = model.MediaType;
            parameters[6].Value = model.AdProductName;
            parameters[7].Value = model.BeginDate;
            parameters[8].Value = model.EndDate;
            parameters[9].Value = model.PhotoRequire;
            parameters[10].Value = model.Price;
            parameters[11].Value = model.AbnormalType;
            parameters[12].Value = model.TPId;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 通过城市名称查询城市Id
        /// </summary>
        /// <param name="RegionName">城市名称</param>
        /// <returns></returns>
        public string GetReturnsRegionId(string RegionName)
        {
            RegionName = RegionName.Replace("省", "").Replace("市", "");
            string sql = "select top 1 RegionId from dbo.Region where RegionName=@RegionName";
            SqlParameter[] parameters = { new SqlParameter("@RegionName", SqlDbType.NVarChar, 20) };
            parameters[0].Value = RegionName;
            object str = SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
            str = str == null ? "" : str;
            return str.ToString();
        }

        /// <summary>
        /// 获取任务数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTaskListCount(string where)
        {
            string sql = @"SELECT COUNT(t.TId)
                        FROM dbo.Task t WHERE " + where;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 查询任务的状态
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int GetTaskStatus(int tid)
        {
            string sql = @"select status from dbo.Task where tid=@tid";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@tid", tid)));
        }

        /// <summary>
        /// 获取任务客户名称
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public string GetTaskCusName(int sid)
        {
            string sql = @"SELECT CusName FROM dbo.Task WHERE SId=@SId";
            return ConvertHelper.GetString(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@SId", sid)));
        }

        /// <summary>
        /// 查询点位结果
        /// </summary>
        /// <param name="tpuid"></param>
        /// <returns></returns>
        public int GetTaskProjectAbnormalType(int tpuid)
        {
            string sql = @"SELECT AbnormalType FROM dbo.TaskProject WHERE TPId =(SELECT TPId FROM dbo.TaskProjectUserRelation WHERE TPUId=@TPUId)";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@TPUId", tpuid)));
        }

        /// <summary>
        /// 查询任务的名称
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetTaskName(int tid)
        {
            string sql = @"select taskname from dbo.Task where tid=@tid";
            return ConvertHelper.GetString(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@tid", tid)));
        }

        /// <summary>
        /// 查询排期名称
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetScheduleName(int sid)
        {
            string sql = @"select ScheduleName from dbo.Schedule where SId=@SId";
            return ConvertHelper.GetString(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@SId", sid)));
        }

        /// <summary>
        /// 查询任务的图片路径
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public string GetTaskPicturePath(int tid)
        {
            string sql = @"select PicturePath from dbo.Task where tid=@tid";
            return ConvertHelper.GetString(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@tid", tid)));
        }

        /// <summary>
        /// 分页获取任务列表数据
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetTaskList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.CreateDate desc) rownum,* FROM (SELECT *,(SELECT TOP 1 (CONVERT(varchar(100), BeginDate, 102)+'-'+CONVERT(varchar(100), EndDate, 102)) FROM dbo.TaskProject WHERE TId=t.TId) DateCycle FROM dbo.Task t where {0})b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取任务排期数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTaskPlanListCount(string where)
        {
            string sql = @"SELECT COUNT(TPId) FROM dbo.TaskProject tp WHERE " + where;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 分页获取列表数据
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetTaskPlanList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.TPId) rownum,* FROM (SELECT tp.*,(SELECT RegionName FROM dbo.Region WHERE RegionId=tp.RegionId) RegionName,tpur.ImgPath,tpur.TPUId,tpur.ThumbnailImgPath,tpur.ShootTime,tpur.ShootPosition,tpur.AuditReason,(SELECT NickName FROM dbo.WerXinUser WHERE UserId=tpur.UserId) NickName,(SELECT CusName FROM dbo.Customer WHERE CusId=tpur.CusId) CusName FROM dbo.TaskProject tp LEFT JOIN (SELECT * FROM (SELECT *,row_number() over (partition by TPId order by TPUId desc) rn from dbo.TaskProjectUserRelation) t1 WHERE rn=1) tpur ON tp.TPId = tpur.TPId where {0})b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        ///  修改排期任务状态
        /// </summary>
        /// <param name="tpid"></param>
        ///  <param name="status"></param>
        /// <returns></returns>
        public int UpdateTaskPlanStatus(int tpid, int status, int abnormalType)
        {
            string sql = @"UPDATE dbo.TaskProject SET Status=@Status,AbnormalType=@AbnormalType WHERE TPId=@TPId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPId",SqlDbType.Int),
                    new SqlParameter("@Status",SqlDbType.Int),
                    new SqlParameter("@AbnormalType",SqlDbType.Int)
                };
            parameters[0].Value = tpid;
            parameters[1].Value = status;
            parameters[2].Value = abnormalType;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        ///  修改微信用户和任务关系
        /// </summary>
        /// <param name="tpid"></param>
        ///  <param name="relation"></param>
        ///   <param name="reason"></param>
        ///   <param name="position"></param>
        /// <returns></returns>
        public int UpdateTaskProjectUserRelation(int tpid, int relation, string reason, string position)
        {
            string sql = @"UPDATE dbo.TaskProjectUserRelation SET Relation=@Relation,AuditDate=GETDATE(),AuditReason=@AuditReason,ShootPosition=@ShootPosition WHERE TPId=@TPId AND Relation=0";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPId",SqlDbType.Int),
                    new SqlParameter("@relation",SqlDbType.Int),
                    new SqlParameter("@AuditReason",SqlDbType.NVarChar,50),
                    new SqlParameter("@ShootPosition",SqlDbType.NVarChar,50)
                };
            parameters[0].Value = tpid;
            parameters[1].Value = relation;
            parameters[2].Value = reason;
            parameters[3].Value = position;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        public int UpdateTaskProjectUserRelation(int tpid, int relation, string reason)
        {
            string sql = @"UPDATE dbo.TaskProjectUserRelation SET Relation=@Relation,AuditDate=GETDATE(),AuditReason=@AuditReason WHERE TPId=@TPId AND Relation=0";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPId",SqlDbType.Int),
                    new SqlParameter("@relation",SqlDbType.Int),
                    new SqlParameter("@AuditReason",SqlDbType.NVarChar,50)
                };
            parameters[0].Value = tpid;
            parameters[1].Value = relation;
            parameters[2].Value = reason;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取金额统计列表数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetMoneyStatisticsCount(string where)
        {
            string sql = @"SELECT COUNT(t.UserId) FROM (SELECT tpur.UserId FROM dbo.TaskProjectUserRelation tpur
 LEFT JOIN dbo.TaskProject tp ON tpur.TPId=tp.TPId WHERE tpur.Relation=1 AND tp.STATUS=3 AND tp.IsPay=0 " + where + " GROUP BY tpur.UserId) t ";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 分页获取金额统计列表
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetMoneyStatisticsList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.UserId) rownum,* FROM (SELECT tpur.UserId,(SELECT NickName FROM dbo.WerXinUser WHERE UserId=tpur.UserId) NickName,SUM(tp.Price) SumMoney FROM dbo.TaskProjectUserRelation tpur LEFT JOIN dbo.TaskProject tp ON tpur.TPId=tp.TPId WHERE tpur.Relation=1 AND tp.STATUS=3 AND tp.IsPay=0 {0} GROUP BY tpur.UserId)b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        ///  支付金额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int PayMoney(int userid)
        {
            string sql = @"UPDATE dbo.TaskProject SET IsPay=1 WHERE Status=3 AND IsPay=0 AND TPId IN(SELECT TPId FROM dbo.TaskProjectUserRelation WHERE UserId=@UserId AND Relation=1)";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@UserId",SqlDbType.Int)
                };
            parameters[0].Value = userid;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 根据tpid获取tid
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public int GetTidByTpId(int tpid)
        {
            string sql = @"SELECT TId FROM dbo.TaskProject WHERE TPId=@TPId";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@TPId", tpid)));
        }

        /// <summary>
        /// 根据tpuid获取所有上传图片路径
        /// </summary>
        /// <param name="tpuid"></param>
        /// <returns></returns>
        public string GetImgPathByTPUId(int tpuid)
        {
            string sql = @"SELECT ImgPath FROM dbo.TaskProjectUserRelation WHERE TPUId=@TPUId";
            return ConvertHelper.GetString(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@TPUId", tpuid)));
        }

        /// <summary>
        /// 根据tpuid获取areaid+tid+tpid
        /// </summary>
        /// <param name="tpuid"></param>
        /// <returns></returns>
        public string GetIdByTPUId(int tpuid)
        {
            string sql = @"SELECT cast(r.AreaId as varchar(5))+'_'+cast(tp.TId as varchar(12))+'_'+cast(tp.TPId as varchar(12)) AS Id FROM dbo.TaskProject tp LEFT JOIN dbo.Region r ON tp.RegionId = r.RegionId WHERE tp.TPId=(SELECT TPId FROM dbo.TaskProjectUserRelation WHERE TPUId=@TPUId)";
            return ConvertHelper.GetString(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, new SqlParameter("@TPUId", tpuid)));
        }

        /// <summary>
        /// 保存图片路径
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <param name="ThumbnailImgPath"></param>
        /// <param name="TPUId"></param>
        /// <returns></returns>
        public int UpdateImagePath(string ImagePath, string ThumbnailImgPath, int TPUId)
        {
            string sql = @"UPDATE dbo.TaskProjectUserRelation SET ImgPath=@ImgPath,ThumbnailImgPath=@ThumbnailImgPath WHERE TPUId=@TPUId";
            SqlParameter[] parameters = { 
                                new SqlParameter("@ImgPath",SqlDbType.NVarChar,400),
                                new SqlParameter("@ThumbnailImgPath",SqlDbType.NVarChar,400), 
                                new SqlParameter("@TPUId",SqlDbType.Int)
                            };
            parameters[0].Value = ImagePath;
            parameters[1].Value = ThumbnailImgPath;
            parameters[2].Value = TPUId;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 修改图片地理位置
        /// </summary>
        /// <param name="ShootPosition"></param>
        /// <param name="TPUId"></param>
        /// <returns></returns>
        public int UpdateShootPosition(string ShootPosition, int TPUId)
        {
            string sql = @"UPDATE dbo.TaskProjectUserRelation SET ShootPosition=@ShootPosition WHERE TPUId=@TPUId";
            SqlParameter[] parameters = { 
                                new SqlParameter("@ShootPosition",ShootPosition),
                                new SqlParameter("@TPUId",TPUId)
                            };
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        ///  修改排期任务GpsType
        /// </summary>
        /// <param name="tpid"></param>
        ///  <param name="gpsType"></param>
        /// <returns></returns>
        public int UpdateTaskPlanGpsType(int tpid, int gpsType)
        {
            string sql = @"UPDATE dbo.TaskProject SET GpsType=@GpsType WHERE TPId=@TPId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPId",SqlDbType.Int),
                    new SqlParameter("@GpsType",SqlDbType.Int)
                };
            parameters[0].Value = tpid;
            parameters[1].Value = gpsType;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="codeLevel"></param>
        /// <returns></returns>
        public DataSet GetRegionList(string parentId, int codeLevel)
        {
            var sql = @"SELECT RegionId,RegionName FROM dbo.Region WHERE CodeLevel=" + codeLevel;
            if (parentId != "0")
            {
                sql += " AND ParentId='" + parentId + "'";
            }
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql);
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="cusName"></param>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public int GetCusIDByUserName(string cusName, int cusId)
        {
            SqlParameter[] parameters = { new SqlParameter("@cusName", SqlDbType.NVarChar, 100), new SqlParameter("@CusId", SqlDbType.Int) };
            parameters[0].Value = cusName;
            parameters[1].Value = cusId;
            string sql = @"SELECT CusId FROM dbo.Customer WHERE IsDisabled=0 AND CusName=@cusName AND CusId<>@cusId";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
        }

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="cusName"></param>
        /// <param name="password"></param>
        /// <param name="customerType"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public int AddCustomer(string cusName, string password, int customerType, string email, string phone, string regionId)
        {
            string sql = @"INSERT INTO dbo.Customer(CusName ,cPassWord ,CustomerType ,Email ,Phone,RegionId) VALUES  (@CusName ,@cPassWord ,@CustomerType ,@Email ,@Phone,@RegionId) SELECT @@IDENTITY";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@CusName",SqlDbType.NVarChar,100),
                    new SqlParameter("@cPassWord",SqlDbType.NVarChar,50), 
                    new SqlParameter("@CustomerType",SqlDbType.Int),
                    new SqlParameter("@Email",SqlDbType.NVarChar,50),
                    new SqlParameter("@Phone",SqlDbType.NVarChar,50),
                    new SqlParameter("@RegionId",SqlDbType.VarChar,12)
                };
            parameters[0].Value = cusName;
            parameters[1].Value = password;
            parameters[2].Value = customerType;
            parameters[3].Value = email;
            parameters[4].Value = phone;
            parameters[5].Value = regionId;
            var taskId = ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
            return taskId;
        }


        /// <summary>
        /// 更新账号
        /// </summary>
        /// <param name="cusName"></param>
        /// <param name="password"></param>
        /// <param name="customerType"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="regionId"></param>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public int UpdateCustomer(string cusName, string password, int customerType, string email, string phone, string regionId, int cusId)
        {
            string sql = @"UPDATE dbo.Customer SET CusName=@CusName,cPassWord=@cPassWord,Email=@Email,Phone=@Phone,RegionId=@RegionId WHERE CusId=@CusId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@CusId",SqlDbType.Int),
                    new SqlParameter("@CusName",SqlDbType.NVarChar,100),
                    new SqlParameter("@cPassWord",SqlDbType.NVarChar,50), 
                    new SqlParameter("@CustomerType",SqlDbType.Int),
                    new SqlParameter("@Email",SqlDbType.NVarChar,50),
                    new SqlParameter("@Phone",SqlDbType.NVarChar,50),
                    new SqlParameter("@RegionId",SqlDbType.VarChar,12)
                };
            parameters[0].Value = cusId;
            parameters[1].Value = cusName;
            parameters[2].Value = password;
            parameters[3].Value = customerType;
            parameters[4].Value = email;
            parameters[5].Value = phone;
            parameters[6].Value = regionId;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
        }

        /// <summary>
        /// 根据CusId获取账号信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public DataTable GetCustomerById(int cusId)
        {
            var sql = @"SELECT * FROM dbo.Customer WHERE CusId=@CusId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@CusId",SqlDbType.Int)
                };
            parameters[0].Value = cusId;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 获取区域等级
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public DataTable GetRegionCodeLevelList(string regionId)
        {
            var sql = @"SELECT RegionName,ParentId,CodeLevel,FullName FROM dbo.Region WHERE RegionId=" + regionId;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 分页获取列表数据
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetCustomerList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.RegionId,b.CreateTime desc) rownum,* FROM (SELECT c.*,w.UserId,w.NickName FROM dbo.Customer c LEFT JOIN dbo.WerXinUser w ON c.CusId = w.CusId where {0})b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取账号数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetCustomerListCount(string where)
        {
            string sql = @"SELECT COUNT(CusId) FROM dbo.Customer c WHERE " + where;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 启用/停用账号
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="isDisabled"></param>
        /// <returns></returns>
        public int UpdateIsDisabled(int cusId, int isDisabled)
        {
            string sql = @"UPDATE dbo.Customer SET IsDisabled=@IsDisabled WHERE CusId=@CusId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@CusId",SqlDbType.Int),
                    new SqlParameter("@IsDisabled",SqlDbType.Int)
                };
            parameters[0].Value = cusId;
            parameters[1].Value = isDisabled;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 根据账号id获取微信用户id
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public int GetUserIdByCusId(int cusId)
        {
            string sql = "SELECT UserId FROM dbo.WerXinUser WHERE CusId=@CusId";
            SqlParameter[] parameters = { new SqlParameter("@CusId", SqlDbType.Int) };
            parameters[0].Value = cusId;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
        }

        /// <summary>
        /// 领取任务
        /// </summary>
        /// <param name="tpId"></param>
        /// <param name="userId"></param>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public int ReceiveTask(int tpId, int userId, int cusId)
        {
            string sql = "INSERT INTO dbo.TaskProjectUserRelation (TPId, UserId,CusId,Relation) VALUES (@TPId,@UserId,@CusId,0) SELECT @@IDENTITY;UPDATE dbo.TaskProject SET Status=1 WHERE TPId=@TPId";
            SqlParameter[] parameters = { 
                                new SqlParameter("@TPId",tpId),
                                new SqlParameter("@UserId",userId),
                                new SqlParameter("@CusId",cusId)
                            };
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
        }

        /// <summary>
        /// 我的任务列表数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetMyTaskListCount(string where)
        {
            string sql = @"SELECT COUNT(tp.TpId) FROM dbo.Task t INNER JOIN dbo.TaskProject tp ON t.TId = tp.TId where " + where;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 我的任务列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cusId"></param>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetMyTaskList(int userId, int cusId, string sqlCondition, int pageIndex, int pageSize)
        {
            string sql = string.Format("SELECT * FROM (select row_number() over ( order by tp.Status,tp.TId desc) rownum, tp.*,t.CusId,t.CusName,t.CreateDate,tpur.ImgPath,tpur.TPUId,tpur.ThumbnailImgPath,tpur.ShootTime,tpur.ShootPosition,tpur.AuditReason FROM dbo.Task t INNER JOIN dbo.TaskProject tp ON t.TId = tp.TId INNER JOIN (SELECT * FROM (SELECT *,row_number() over (partition by TPId order by TPUId desc) rn from dbo.TaskProjectUserRelation WHERE (UserId=" + userId + " or CusId=" + cusId + ")) t1 WHERE rn=1) tpur ON tp.TPId = tpur.TPId where {0}) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize order by CreateDate DESC ", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取用户领取任务信息
        /// </summary>
        /// <param name="TpId"></param>
        /// <param name="UserId"></param>
        /// <param name="CusId"></param>
        /// <returns></returns>
        public DataTable GetTaskProjectUserRelation(int TpId, int UserId, int CusId)
        {
            string sql = @"SELECT TPUId,ShootTime,ShootPosition,ImgPath,ThumbnailImgPath FROM dbo.TaskProjectUserRelation WHERE TPId=@TPId AND (UserId=@UserId OR CusId=@CusId) AND Relation in(0,3)";
            SqlParameter[] parameters = { 
                                new SqlParameter("@TPId",TpId),
                                new SqlParameter("@UserId",UserId),
                                  new SqlParameter("@CusId",CusId)
                            };
            return (SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0]);
        }

        /// <summary>
        /// 获取用户领取任务信息
        /// </summary>
        /// <param name="TpId"></param>
        /// <returns></returns>
        public DataTable GetTaskProjectUserRelation(int TpId)
        {
            string sql = @"SELECT TPUId,UserId,ShootTime,ShootPosition,ImgPath,ThumbnailImgPath FROM dbo.TaskProjectUserRelation WHERE TPId=@TPId AND Relation in(0,3) ORDER BY UserBeginWorkTime DESC";
            SqlParameter[] parameters = { 
                                new SqlParameter("@TPId",TpId)
                            };
            return (SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0]);
        }

        /// <summary>
        /// 保存图片路径
        /// </summary>
        /// <param name="TpId"></param>
        /// <param name="UserId"></param>
        /// <param name="ImagePath"></param>
        /// <returns></returns>
        public int SaveImagePath(int TpId, int UserId, int CusId, string ImagePath, string ThumbnailImgPath, DateTime ShootTime, string ShootPosition)
        {
            string sql = @"UPDATE dbo.TaskProjectUserRelation SET Relation=0,ImgPath=@ImgPath,ThumbnailImgPath=@ThumbnailImgPath,ShootTime=@ShootTime,ShootPosition=@ShootPosition WHERE TPId=@TPId AND (UserId=@UserId OR CusId=@CusId) AND Relation in(0,3);UPDATE dbo.TaskProject SET Status=2 WHERE TPId=@TPId";
            SqlParameter[] parameters = { 
                                new SqlParameter("@TPId",SqlDbType.Int),
                                new SqlParameter("@UserId",SqlDbType.Int),
                                  new SqlParameter("@CusId",SqlDbType.Int),
                                new SqlParameter("@ImgPath",SqlDbType.NVarChar,400),
                                new SqlParameter("@ThumbnailImgPath",SqlDbType.NVarChar,400),
                                new SqlParameter("@ShootTime",SqlDbType.DateTime),
                                new SqlParameter("@ShootPosition",SqlDbType.NVarChar,50)
                            };
            parameters[0].Value = TpId;
            parameters[1].Value = UserId;
            parameters[2].Value = CusId;
            parameters[3].Value = ImagePath;
            parameters[4].Value = ThumbnailImgPath;
            parameters[5].Value = ShootTime;
            parameters[6].Value = ShootPosition;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 解除微信用户和账号的绑定
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int UnBind(int userId)
        {
            string sql = @"UPDATE dbo.WerXinUser SET CusId=0 WHERE UserId=@UserId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@UserId",SqlDbType.Int)
                };
            parameters[0].Value = userId;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 添加图片详细信息
        /// </summary>
        /// <param name="TpuId"></param>
        /// <param name="ImgPath"></param>
        /// <param name="ThumbnailImgPath"></param>
        /// <param name="Sort"></param>
        /// <param name="ExportImgPath"></param>
        /// <returns></returns>
        public int AddImageDetail(int TpuId, string ImgPath, string ThumbnailImgPath, int Sort, string ExportImgPath)
        {
            string sql = @"INSERT INTO dbo.ImageDetail(TPUId, ImgPath, ThumbnailImgPath, Sort,ExportImgPath) VALUES (@TPUId, @ImgPath, @ThumbnailImgPath, @Sort,@ExportImgPath) SELECT @@IDENTITY";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPUId",SqlDbType.Int),
                    new SqlParameter("@ImgPath",SqlDbType.NVarChar,100),
                    new SqlParameter("@ThumbnailImgPath",SqlDbType.NVarChar,100),
                    new SqlParameter("@Sort",SqlDbType.Int),
                    new SqlParameter("@ExportImgPath",SqlDbType.NVarChar,100)
                };
            parameters[0].Value = TpuId;
            parameters[1].Value = ImgPath;
            parameters[2].Value = ThumbnailImgPath;
            parameters[3].Value = Sort;
            parameters[4].Value = ExportImgPath;
            var Id = ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
            return Id;
        }

        /// <summary>
        /// 删除图片详细信息
        /// </summary>
        /// <param name="ImgPath"></param>
        /// <returns></returns>
        public int DeleteImageDetail(string ImgPath)
        {
            string sql = @"DELETE FROM dbo.ImageDetail WHERE ImgPath=@ImgPath";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@ImgPath",SqlDbType.NVarChar,100)
                };
            parameters[0].Value = ImgPath;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 修改图片排序
        /// </summary>
        /// <param name="ImgPath"></param>
        /// <param name="Sort"></param>
        /// <returns></returns>
        public int UpdateImageDetailSort(string ImgPath, int Sort)
        {
            string sql = @"UPDATE dbo.ImageDetail SET Sort=@Sort WHERE ImgPath=@ImgPath";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@ImgPath",SqlDbType.NVarChar,100),
                    new SqlParameter("@Sort",SqlDbType.Int)
                };
            parameters[0].Value = ImgPath;
            parameters[1].Value = Sort;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 根据TPUId获取图片信息
        /// </summary>
        /// <param name="TPUId"></param>
        /// <returns></returns>
        public DataTable GetImageDetailList(int TPUId)
        {
            string sql = @"SELECT ImgPath,ExportImgPath,Sort FROM dbo.ImageDetail WHERE TPUId=@TPUId ORDER BY Sort";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPUId",SqlDbType.Int)
                };
            parameters[0].Value = TPUId;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 获取媒体类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetMediaTypeList(string sqlWhere)
        {
            var sql = @"with tt as
                        (SELECT DISTINCT tp.MediaType,t.CreateDate 
                        FROM dbo.Task t 
                        INNER JOIN dbo.TaskProject tp ON t.TId = tp.TId WHERE " + sqlWhere + "), temp as (select RANK() OVER (PARTITION BY tt.MediaType ORDER BY tt.CreateDate )as rnk, tt.MediaType,tt.CreateDate  FROM tt) select * from temp where rnk=1 order by createdate DESC";
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql).Tables[0];
        }

        /// <summary>
        /// 获取客户统计列表数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTaskStatisticsCount(string where)
        {
            string sql = @"SELECT COUNT(1) FROM (SELECT t.CusId,t.CusName,tp.MediaType,tp.RegionId FROM dbo.Task t INNER JOIN dbo.TaskProject tp ON t.TId = tp.TId WHERE t.Status>0 " + where + " GROUP BY t.CusId,t.CusName,tp.MediaType,tp.RegionId) a";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 分页获取客户统计列表
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetTaskStatisticsList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.CusId) rownum,* FROM (SELECT t.CusId,t.CusName,tp.MediaType,tp.RegionId,(SELECT RegionName FROM dbo.Region WHERE RegionId=tp.RegionId) RegionName FROM dbo.Task t INNER JOIN dbo.TaskProject tp ON t.TId = tp.TId WHERE t.Status>0 {0} GROUP BY t.CusId,t.CusName,tp.MediaType,tp.RegionId)b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 分页获取账号统计列表
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetCustomerStatisticsList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.CreateTime desc) rownum,* FROM (SELECT c.*,(SELECT FullName FROM dbo.Region WHERE RegionId=c.RegionId) FullName FROM dbo.Customer c WHERE {0})b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取账号绑定任务数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTaskProjectUserRelationCount(string where)
        {
            string sql = @"SELECT COUNT(TPId) FROM dbo.TaskProjectUserRelation WHERE " + where + " GROUP BY TPId";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 根据tid获取草稿状态的任务点位
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public DataTable GetTpidsByTid(int tid)
        {
            var sql = @"SELECT TPId FROM dbo.TaskProject WHERE TId=@TId AND Status=0";
            SqlParameter[] parameters = { 
                                new SqlParameter("@TId",tid)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 更新任务异常类型
        /// </summary>
        /// <param name="tpid"></param>
        /// <param name="abnormalType"></param>
        /// <returns></returns>
        public int UpdateAbnormalType(int tpid, int abnormalType)
        {
            string sql = @"UPDATE dbo.TaskProject SET AbnormalType=@AbnormalType WHERE TPId=@TPId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@TPId",SqlDbType.Int),
                    new SqlParameter("@AbnormalType",SqlDbType.Int)
                };
            parameters[0].Value = tpid;
            parameters[1].Value = abnormalType;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 添加排期
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <returns></returns>
        public int AddSchedule(string scheduleName)
        {
            string sql = @"INSERT INTO dbo.Schedule(ScheduleName) VALUES(@ScheduleName) SELECT @@IDENTITY";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@ScheduleName",SqlDbType.NVarChar,100)
                };
            parameters[0].Value = scheduleName;
            var scheduleId = ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters));
            return scheduleId;
        }

        /// <summary>
        /// 修改排期周期
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int UpdateScheduleDate(int sid, DateTime beginDate, DateTime endDate)
        {
            string sql = @"UPDATE dbo.Schedule SET BeginDate=@BeginDate,EndDate=@EndDate WHERE SId=@SId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@SId",SqlDbType.Int),
                    new SqlParameter("@BeginDate",SqlDbType.DateTime),
                    new SqlParameter("@EndDate",SqlDbType.DateTime)
                };
            parameters[0].Value = sid;
            parameters[1].Value = beginDate;
            parameters[2].Value = endDate;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取排期数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetScheduleListCount(string where)
        {
            string sql = @"SELECT COUNT(SId)
                        FROM dbo.Schedule WHERE " + where;
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 获取排期列表数据
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetScheduleList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.CreateDate desc) rownum,* FROM (SELECT * FROM dbo.Schedule where {0})b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 删除排期
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public int DeleteSchedule(int sid)
        {
            string sql = @"DELETE FROM dbo.Schedule WHERE SId=@SId";
            SqlParameter[] parameters =
                {   
                    new SqlParameter("@SId",SqlDbType.Int)
                };
            parameters[0].Value = sid;
            return SqlHelper.ExecuteNonQuery(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取账号分配的任务号
        /// </summary>
        /// <param name="cusId"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public DataTable GetTids(int cusId, string mediaType)
        {
            var sql = @"SELECT DISTINCT TId FROM dbo.TaskProject WHERE TPId IN(SELECT TPId FROM dbo.TaskProjectUserRelation WHERE CusId=@CusId) AND MediaType=@MediaType";
            SqlParameter[] parameters = { 
                                new SqlParameter("@CusId",SqlDbType.Int),
                                  new SqlParameter("@MediaType",SqlDbType.NVarChar,50)
                            };
            parameters[0].Value = cusId;
            parameters[1].Value = mediaType;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 获取账号分配的任务号
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public DataTable GetTids(int cusId)
        {
            var sql = @"SELECT DISTINCT TId FROM dbo.TaskProject WHERE TPId IN(SELECT TPId FROM dbo.TaskProjectUserRelation WHERE CusId=@CusId)";
            SqlParameter[] parameters = { 
                                new SqlParameter("@CusId",SqlDbType.Int)
                            };
            parameters[0].Value = cusId;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 根据排期id获取当前排期的城市
        /// </summary>
        /// <param name="sid"></param>
        ///  <param name="key"></param>
        /// <returns></returns>
        public DataTable GetRegionBySid(int sid, string key)
        {
            var sql = @"SELECT tp.RegionId,r.RegionName FROM dbo.TaskProject tp INNER JOIN dbo.Region r ON tp.RegionId = r.RegionId WHERE tp.TId IN(SELECT TId FROM dbo.Task WHERE SId=@SId) AND tp.Status=3" + (string.IsNullOrEmpty(key) ? "" : "AND r.RegionName LIKE '%" + key + "%'") + " GROUP BY tp.RegionId,r.RegionName";
            SqlParameter[] parameters = { 
                                new SqlParameter("@SId",SqlDbType.Int)
                            };
            parameters[0].Value = sid;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 根据排期id获取当前排期的楼宇
        /// </summary>
        /// <param name="sid"></param>
        ///  <param name="key"></param>
        /// <returns></returns>
        public DataTable GetBlockBySid(int sid, string key)
        {
            var sql = @"SELECT RegionId,BlockName FROM dbo.TaskProject WHERE TId IN(SELECT TId FROM dbo.Task WHERE SId=@SId) AND Status=3" + (string.IsNullOrEmpty(key) ? "" : "AND BlockName LIKE '%" + key + "%'") + " GROUP BY RegionId,BlockName";
            SqlParameter[] parameters = { 
                                new SqlParameter("@SId",SqlDbType.Int)
                            };
            parameters[0].Value = sid;
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters).Tables[0];
        }

        /// <summary>
        /// 获取报告详情数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetReportDetailCount(string where)
        {
            string sql = @"SELECT COUNT(t.BlockName) FROM (SELECT tp.RegionId,r.RegionName,tp.BlockName FROM dbo.TaskProject tp INNER JOIN dbo.Region r ON tp.RegionId = r.RegionId WHERE " + where + " GROUP BY tp.RegionId,r.RegionName,tp.BlockName) t";
            return ConvertHelper.GetInteger(SqlHelper.ExecuteScalar(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql));
        }

        /// <summary>
        /// 获取报告详情列表数据
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataSet GetReportDetailList(string sqlCondition, int pageIndex, int pageSize)
        {
            var sql = string.Format(@"SELECT * FROM (SELECT row_number() over (order by b.RegionId) rownum,* FROM (SELECT tp.RegionId,r.RegionName,tp.BlockName,SUM(CASE WHEN AbnormalType=3 THEN 1 ELSE 0 END) NotPaintCount,SUM(CASE WHEN AbnormalType=6 THEN 1 ELSE 0 END) HiddenCount,SUM(CASE WHEN AbnormalType=4 THEN 1 ELSE 0 END) BreakCount,SUM(CASE WHEN AbnormalType IN (0,5) THEN 1 ELSE 0 END) NormalCount FROM dbo.TaskProject tp INNER JOIN dbo.Region r ON tp.RegionId = r.RegionId WHERE {0} GROUP BY tp.RegionId,r.RegionName,tp.BlockName)b) a where rownum > (@PageIndex-1)*@PageSize and rownum <= @PageIndex*@PageSize", sqlCondition);
            SqlParameter[] parameters = { 
                                new SqlParameter("@PageIndex",pageIndex),
                                new SqlParameter("@PageSize",pageSize)
                            };
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取城市报告详情
        /// </summary>
        /// <param name="sqlCondition"></param>
        /// <returns></returns>
        public DataSet GetCityReportDetailList(string sqlCondition)
        {
            var sql = string.Format(@"SELECT tp.RegionId,r.RegionName,SUM(CASE WHEN AbnormalType=3 THEN 1 ELSE 0 END) NotPaintCount,SUM(CASE WHEN AbnormalType=6 THEN 1 ELSE 0 END) HiddenCount,SUM(CASE WHEN AbnormalType=4 THEN 1 ELSE 0 END) BreakCount,SUM(CASE WHEN AbnormalType IN (0,5) THEN 1 ELSE 0 END) NormalCount FROM dbo.TaskProject tp INNER JOIN dbo.Region r ON tp.RegionId = r.RegionId WHERE {0} GROUP BY tp.RegionId,r.RegionName order by tp.RegionId", sqlCondition);
            return SqlHelper.ExecuteDataset(DBConnectionString.Get(OutdoorMonitor), CommandType.Text, sql);
        }
    }
}
