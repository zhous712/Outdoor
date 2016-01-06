using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoRadio.RadioSmart.Common;
using System.Data;
using Model;
using System.Configuration;
using System.IO;

namespace Outdoor.ashx
{
    /// <summary>
    /// Action 的摘要说明
    /// </summary>
    public class Action : IHttpHandler
    {
        private Dal.Monitor monitor = new Dal.Monitor();
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request["option"]))
            {
                int tid = ConvertHelper.GetInteger(context.Request["tid"]);
                int status;
                switch (context.Request["option"])
                {
                    case "delTask":
                        int sid = ConvertHelper.GetInteger(context.Request["sid"]);
                        if (tid > 0 && sid > 0)
                        {
                            status = monitor.GetTaskStatus(tid);
                            if (status > 1)
                            {
                                context.Response.Write("no");
                            }
                            else
                            {
                                monitor.DeleteTask(tid);
                                if (monitor.GetTaskListCount(" SId=" + sid) == 0)
                                {
                                    monitor.DeleteSchedule(sid);
                                }
                                context.Response.Write("true");
                            }
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "releaseTask":
                        if (tid > 0)
                        {
                            int result = monitor.ReleaseTask(tid, 1);
                            if (result > 0)
                            {
                                context.Response.Write("true");
                            }
                            else
                            {
                                context.Response.Write("error");
                            }
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "auditTask":
                        int tpid = ConvertHelper.GetInteger(context.Request["tpid"]);
                        status = ConvertHelper.GetInteger(context.Request["status"]);
                        int relation = ConvertHelper.GetInteger(context.Request["relation"]);
                        int abnormalType = ConvertHelper.GetInteger(context.Request["abnormalType"]);
                        string reason = ConvertHelper.GetString(context.Request["reason"]);
                        string position = ConvertHelper.GetString(context.Request["position"]);
                        if (tpid > 0)
                        {
                            int result = monitor.UpdateTaskPlanStatus(tpid, status, abnormalType);
                            if (string.IsNullOrEmpty(position))
                            {
                                monitor.UpdateTaskProjectUserRelation(tpid, relation, reason);
                            }
                            else
                            {
                                monitor.UpdateTaskProjectUserRelation(tpid, relation, reason, position);
                            }

                            if (status == 3 && monitor.GetTaskPlanListCount(" tp.TId=(SELECT TId FROM dbo.TaskProject WHERE TPId=" + tpid + ") AND tp.Status !=3") == 0)
                            {
                                monitor.ReleaseTask(monitor.GetTidByTpId(tpid), 3);
                            }
                            if (result > 0)
                            {
                                context.Response.Write("true");
                            }
                            else
                            {
                                context.Response.Write("error");
                            }
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "payMoney":
                        int userid = ConvertHelper.GetInteger(context.Request["userid"]);
                        if (userid > 0)
                        {
                            int result = monitor.PayMoney(userid);
                            if (result > 0)
                            {
                                context.Response.Write("true");
                            }
                            else
                            {
                                context.Response.Write("error");
                            }
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "getCustomer":
                        string key = ConvertHelper.GetString(context.Request["q"]);
                        int customerType = ConvertHelper.GetInteger(context.Request["customerType"]);
                        DataTable dt = monitor.GetCustInfo(key, customerType);
                        string retuStr = "[";
                        foreach (DataRow dr in dt.Rows)
                        {
                            retuStr += "{\"CusName\":\"" + dr["CusName"].ToString() + "\"},";
                        }
                        if (retuStr != "")
                        {
                            retuStr = retuStr.TrimEnd(',');
                        }
                        retuStr += "]";
                        context.Response.Write(retuStr);
                        break;
                    case "getCity":
                        key = ConvertHelper.GetString(context.Request["key"]) == "请输入城市" ? "" : ConvertHelper.GetString(context.Request["key"]);
                        sid = ConvertHelper.GetInteger(context.Request["sid"]);
                        dt = monitor.GetRegionBySid(sid, key);
                        retuStr = "[";
                        foreach (DataRow dr in dt.Rows)
                        {
                            retuStr += "{\"RegionId\":\"" + dr["RegionId"].ToString() + "\",\"RegionName\":\"" + dr["RegionName"].ToString() + "\"},";
                        }
                        if (retuStr != "")
                        {
                            retuStr = retuStr.TrimEnd(',');
                        }
                        retuStr += "]";
                        context.Response.Write(retuStr);
                        break;
                    case "getBlock":
                        key = ConvertHelper.GetString(context.Request["key"]) == "请输入楼宇" ? "" : ConvertHelper.GetString(context.Request["key"]);
                        sid = ConvertHelper.GetInteger(context.Request["sid"]);
                        dt = monitor.GetBlockBySid(sid, key);
                        retuStr = "[";
                        foreach (DataRow dr in dt.Rows)
                        {
                            retuStr += "{\"BlockName\":\"" + dr["BlockName"].ToString() + "\"},";
                        }
                        if (retuStr != "")
                        {
                            retuStr = retuStr.TrimEnd(',');
                        }
                        retuStr += "]";
                        context.Response.Write(retuStr);
                        break;
                    case "UpdateGps":
                        tpid = ConvertHelper.GetInteger(context.Request["tpid"]);
                        int tpuid = ConvertHelper.GetInteger(context.Request["tpuid"]);
                        string gps = ConvertHelper.GetString(context.Request["gps"]);
                        if (tpid > 0 && tpuid > 0)
                        {
                            monitor.UpdateTaskPlanGpsType(tpid, 1);
                            monitor.UpdateShootPosition(gps, tpuid);
                            context.Response.Write("true");
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "isDisabled":
                        int cusId = ConvertHelper.GetInteger(context.Request["cusid"]);
                        int isDisabled = ConvertHelper.GetInteger(context.Request["isdisabled"]);
                        if (cusId > 0)
                        {
                            monitor.UpdateIsDisabled(cusId, isDisabled);
                            context.Response.Write("true");
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "BindTaskPlan":
                        string cusName = ConvertHelper.GetString(context.Request["cusname"]);
                        string tpids = ConvertHelper.GetString(context.Request["tpids"]);
                        string[] tpidArr = tpids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tid > 0 && !string.IsNullOrEmpty(cusName))
                        {
                            Customer customer = monitor.GetCustomerInfoByNameAndType(cusName, 2);
                            if (customer != null && customer.CusId > 0)
                            {
                                int userId = monitor.GetUserIdByCusId(customer.CusId);
                                if (tpidArr.Length == 0)
                                {
                                    DataTable dtTpid = monitor.GetTpidsByTid(tid);
                                    tpidArr = new string[dtTpid.Rows.Count];
                                    for (int j = 0; j < dtTpid.Rows.Count; j++)
                                    {
                                        tpidArr[j] = dtTpid.Rows[j]["TpId"].ToString();
                                    }
                                }
                                for (int i = 0; i < tpidArr.Length; i++)
                                {
                                    monitor.ReceiveTask(ConvertHelper.GetInteger(tpidArr[i]), userId, customer.CusId);
                                }
                                status = monitor.GetTaskStatus(tid);
                                if (status == 1)
                                {
                                    monitor.ReleaseTask(tid, 2);
                                }
                                context.Response.Write("true");
                            }
                            else
                            {
                                context.Response.Write("账号不存在！");
                            }
                        }
                        else
                        {
                            context.Response.Write("操作出错！");
                        }
                        break;
                    case "imageDel":
                        try
                        {
                            string rootImage = ConfigurationManager.AppSettings["RootRadioSmartImage"];
                            string imagePath = ConvertHelper.GetString(context.Request["imagepath"]);
                            string targetFile = rootImage + System.IO.Path.DirectorySeparatorChar + imagePath;
                            if (System.IO.File.Exists(Path.GetFullPath(targetFile)))
                            {
                                File.Delete(Path.GetFullPath(targetFile));
                            }
                            var arrPath = imagePath.Split('.');
                            if (arrPath.Length > 1)
                            {
                                string thumbnailImgPath = arrPath[0] + "s." + arrPath[1];
                                string thumbnailTargetFile = rootImage + System.IO.Path.DirectorySeparatorChar + thumbnailImgPath;
                                if (System.IO.File.Exists(Path.GetFullPath(thumbnailTargetFile)))
                                {
                                    File.Delete(Path.GetFullPath(thumbnailTargetFile));
                                }
                            }
                            context.Response.Write("true");
                        }
                        catch
                        {
                            context.Response.Write("error");
                            return;
                        }
                        break;
                    case "reDo":
                        tpid = ConvertHelper.GetInteger(context.Request["tpid"]);
                        if (tpid > 0)
                        {
                            dt = monitor.GetTaskPlanList(" tp.TPId=" + tpid, 1, 999).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                TaskProject taskProject = new TaskProject();
                                taskProject.TId = ConvertHelper.GetInteger(dt.Rows[0]["TId"]);
                                taskProject.RegionId = ConvertHelper.GetString(dt.Rows[0]["RegionId"]);
                                taskProject.AreaName = ConvertHelper.GetString(dt.Rows[0]["AreaName"]);
                                taskProject.StreetAddress = ConvertHelper.GetString(dt.Rows[0]["StreetAddress"]);
                                taskProject.BlockName = ConvertHelper.GetString(dt.Rows[0]["BlockName"]);
                                taskProject.PointName = ConvertHelper.GetString(dt.Rows[0]["PointName"]);
                                taskProject.MediaType = ConvertHelper.GetString(dt.Rows[0]["MediaType"]);
                                taskProject.AdProductName = ConvertHelper.GetString(dt.Rows[0]["AdProductName"]);
                                taskProject.BeginDate = ConvertHelper.GetDateTime(dt.Rows[0]["BeginDate"]);
                                taskProject.EndDate = ConvertHelper.GetDateTime(dt.Rows[0]["EndDate"]);
                                taskProject.Status = 0;
                                taskProject.PhotoRequire = ConvertHelper.GetString(dt.Rows[0]["PhotoRequire"]);
                                taskProject.Price = ConvertHelper.GetDecimal(dt.Rows[0]["Price"]);
                                monitor.AddTaskProject(taskProject);
                            }
                            context.Response.Write("true");
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "unBind":
                        userid = ConvertHelper.GetInteger(context.Request["userid"]);
                        if (userid > 0)
                        {
                            monitor.UnBind(userid);
                            context.Response.Write("true");
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                    case "delSchedule":
                        sid = ConvertHelper.GetInteger(context.Request["sid"]);
                        if (sid > 0)
                        {
                            if (monitor.GetTaskListCount(" SId=" + sid) > 0)
                            {
                                context.Response.Write("no");
                            }
                            else
                            {
                                monitor.DeleteSchedule(sid);
                                context.Response.Write("true");
                            }
                        }
                        else
                        {
                            context.Response.Write("error");
                        }
                        break;
                }
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}