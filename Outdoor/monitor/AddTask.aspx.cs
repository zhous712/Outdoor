using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;
using Dal;
using AutoRadio.RadioSmart.Common;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Configuration;
using System.Data;
using Model;
using NPOI.HSSF.UserModel;
using Excel;
using NPOI.XSSF.UserModel;

namespace Outdoor.monitor
{
    public partial class AddTask : BasePage
    {
        Monitor monitor = new Monitor();
        string beginDate = string.Empty, endDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            jianbo_UserMenu.MenuType = 1;
            if (!string.IsNullOrEmpty(ScheduleName))
            {
                txtScheduleName.Value = ScheduleName;
                txtScheduleName.Disabled = true;
                txtCusName.Value = monitor.GetTaskCusName(SId);
            }
            if (Profile.CustomerType == 0)
            {
                divCustomer.Visible = false;
            }
            if (GetReferer(true).IndexOf("lexus") > 0)
            {
                jianbo_Footer.Visible = false;
            }
            else
            {
                jianbo_Footer.Visible = true;
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string commonExcelPath = ConfigurationManager.AppSettings["RootRadioSmartXLS"] + System.IO.Path.DirectorySeparatorChar;
            string[] strExcelPathArr = hfExcelPath.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string[] strExcelNameArr = txtUploadExcelFile.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string strImagePath = hfImagePath.Value.Trim('|');
            string currentURL = Request.Url.ToString();
            int cusId = Profile.CusId;
            string cusName = Profile.CusName;
            string tids = string.Empty;
            string errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(txtCusName.Value.Trim()))
            {
                Customer customer = monitor.GetCustomerInfoByName(txtCusName.Value.Trim());
                if (customer != null)
                {
                    cusId = customer.CusId;
                    cusName = customer.CusName;
                }
            }
            int sid = SId > 0 ? SId : monitor.AddSchedule(txtScheduleName.Value);
            for (int i = 0; i < strExcelPathArr.Length; i++)
            {
                int tid = monitor.AddTask(cusId, cusName, strExcelNameArr[i].Substring(0, strExcelNameArr[i].LastIndexOf('.')), sid, strExcelPathArr[i], strImagePath == "," ? "" : strImagePath);
                if (tid > 0)
                {
                    int result = Resolve(tid, commonExcelPath, strExcelPathArr[i]);
                    if (result == 0)
                    {
                        tids += tid + ",";
                        continue;
                    }
                    else if (result == -1)
                    {
                        errorMessage += (strExcelNameArr[i] + "排期格式错误,");
                        continue;
                    }
                    else
                    {
                        errorMessage += ("Excel" + strExcelNameArr[i] + "第" + result + "行解析出错,");
                        continue;
                    }
                }
                else
                {
                    ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert_error('操作出错！',function(){window.history.go(-1)})");
                    return;
                }
            }
            if (Profile.CustomerType == 0)
            {
                //给客户发邮件
                string subject = "友情提醒--户外监测上传Excel了";
                string mailInfo = "户外监测客户" + Profile.CusName + "刚刚上传Excel生成了任务，排期号：" + sid + ",任务号：" + tids.Trim(',');
                NotifyHelper.SendMailNotify(ConfigurationManager.AppSettings["SendEmailAddress"].ToString(), "", subject, mailInfo);
            }
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                monitor.UpdateScheduleDate(sid, ConvertHelper.GetDateTime(beginDate), ConvertHelper.GetDateTime(endDate));
            }
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert_error('" + errorMessage + "',function(){window.location.href='TaskList.aspx?sid=" + sid + "'})");
                if (monitor.GetTaskListCount(" SId=" + sid) == 0)
                {
                    monitor.DeleteSchedule(sid);
                }
                return;
            }
            Response.Redirect("TaskList.aspx?sid=" + sid);
        }
        public int Resolve(int tid, string path, string file)
        {
            bool IsSuccess = true;
            int num = 0;
            Loger.Current.Write("AddTask.Resolve() begin");
            try
            {
                FileStream fileStream = null;
                fileStream = new FileStream(path + file, FileMode.Open);
                if (file.Substring(file.LastIndexOf('.') + 1).ToLower() == "xls")
                {
                    HSSFWorkbook hssfWorkbook = new HSSFWorkbook(fileStream);
                    TaskProject model = null;
                    if (hssfWorkbook != null)
                    {
                        for (int sheetIndex = 0; sheetIndex < hssfWorkbook.NumberOfSheets; ++sheetIndex)
                        {
                            HSSFSheet hssfSheet = hssfWorkbook.GetSheetAt(sheetIndex) as HSSFSheet;
                            try
                            {
                                for (int i = (hssfSheet.FirstRowNum + 1); i <= hssfSheet.LastRowNum; i++)
                                {
                                    HSSFRow row = hssfSheet.GetRow(hssfSheet.FirstRowNum) as HSSFRow;
                                    if (row.Cells.Count < 10)
                                    {
                                        IsSuccess = false;
                                        num = -1;
                                        monitor.DeleteTask(tid);
                                        return num;
                                    }
                                    row = hssfSheet.GetRow(i) as HSSFRow;
                                    num = i;
                                    if (row != null && row.Cells.Count > 0)
                                    {
                                        model = new TaskProject();
                                        model.TId = tid;
                                        model.RegionId = monitor.GetReturnsRegionId(ConvertHelper.GetString(row.GetCell(1)));
                                        model.AreaName = ConvertHelper.GetString(row.GetCell(2));
                                        model.StreetAddress = ConvertHelper.GetString(row.GetCell(3));
                                        model.BlockName = ConvertHelper.GetString(row.GetCell(4));
                                        model.PointName = ConvertHelper.GetString(row.GetCell(5));
                                        model.MediaType = ConvertHelper.GetString(row.GetCell(6));
                                        model.AdProductName = ConvertHelper.GetString(row.GetCell(7));
                                        string[] dates = ConvertHelper.GetString(row.GetCell(8)).Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (dates.Length > 1)
                                        {
                                            model.BeginDate = ConvertHelper.GetDateTime(dates[0]);
                                            model.EndDate = ConvertHelper.GetDateTime(dates[1]);
                                        }
                                        else
                                        {
                                            model.BeginDate = ConvertHelper.GetDateTime(row.GetCell(8));
                                            model.EndDate = ConvertHelper.GetDateTime(row.GetCell(8));
                                        }
                                        if (string.IsNullOrEmpty(beginDate) || string.IsNullOrEmpty(endDate))
                                        {
                                            beginDate = model.BeginDate.ToString();
                                            endDate = model.EndDate.ToString();
                                        }
                                        model.Status = 0;
                                        model.PhotoRequire = "实地获取位置信息并拍照上传";
                                        model.Price = 0;
                                        model.SpareOne = ConvertHelper.GetString(row.GetCell(9));
                                        model.SpareTwo = "";
                                        monitor.AddTaskProject(model);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Loger.Current.Write("AddTask.Resolve() errornum=" + num + " err=" + e.ToString());
                                monitor.DeleteTask(tid);
                                IsSuccess = false;
                            }
                        }
                    }
                }
                else if (file.Substring(file.LastIndexOf('.') + 1).ToLower() == "xlsx")
                {
                    XSSFWorkbook xssfWorkbook = new XSSFWorkbook(fileStream);
                    TaskProject model = null;
                    if (xssfWorkbook != null)
                    {
                        for (int sheetIndex = 0; sheetIndex < xssfWorkbook.NumberOfSheets; ++sheetIndex)
                        {
                            XSSFSheet xssfSheet = xssfWorkbook.GetSheetAt(sheetIndex) as XSSFSheet;
                            try
                            {
                                for (int i = (xssfSheet.FirstRowNum + 1); i <= xssfSheet.LastRowNum; i++)
                                {
                                    XSSFRow row = xssfSheet.GetRow(xssfSheet.FirstRowNum) as XSSFRow;
                                    if (row.Cells.Count < 10)
                                    {
                                        IsSuccess = false;
                                        num = -1;
                                        monitor.DeleteTask(tid);
                                        return num;
                                    }
                                    row = xssfSheet.GetRow(i) as XSSFRow;
                                    num = i;
                                    if (row != null && row.Cells.Count > 0)
                                    {
                                        model = new TaskProject();
                                        model.TId = tid;
                                        model.RegionId = monitor.GetReturnsRegionId(ConvertHelper.GetString(row.GetCell(1)));
                                        model.AreaName = ConvertHelper.GetString(row.GetCell(2));
                                        model.StreetAddress = ConvertHelper.GetString(row.GetCell(3));
                                        model.BlockName = ConvertHelper.GetString(row.GetCell(4));
                                        model.PointName = ConvertHelper.GetString(row.GetCell(5));
                                        model.MediaType = ConvertHelper.GetString(row.GetCell(6));
                                        model.AdProductName = ConvertHelper.GetString(row.GetCell(7));
                                        string[] dates = ConvertHelper.GetString(row.GetCell(8)).Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (dates.Length > 1)
                                        {
                                            model.BeginDate = ConvertHelper.GetDateTime(dates[0]);
                                            model.EndDate = ConvertHelper.GetDateTime(dates[1]);
                                        }
                                        else
                                        {
                                            model.BeginDate = ConvertHelper.GetDateTime(row.GetCell(8));
                                            model.EndDate = ConvertHelper.GetDateTime(row.GetCell(8));
                                        }
                                        if (string.IsNullOrEmpty(beginDate) || string.IsNullOrEmpty(endDate))
                                        {
                                            beginDate = model.BeginDate.ToString();
                                            endDate = model.EndDate.ToString();
                                        }
                                        model.Status = 0;
                                        model.PhotoRequire = "实地获取位置信息并拍照上传";
                                        model.Price = 0;
                                        model.SpareOne = ConvertHelper.GetString(row.GetCell(9));
                                        model.SpareTwo = "";
                                        monitor.AddTaskProject(model);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Loger.Current.Write("AddTask.Resolve() errornum=" + num + " err=" + e.ToString());
                                monitor.DeleteTask(tid);
                                IsSuccess = false;
                            }
                        }
                    }
                }
                else
                {
                    num = -1;
                }
            }
            catch (Exception e)
            {
                Loger.Current.Write("AddTask.Resolve() err=" + e.ToString());
                monitor.DeleteTask(tid);
                IsSuccess = false;
            }
            Loger.Current.Write("AddTask.Resolve() end");
            return IsSuccess ? 0 : num;
        }

        int SId { get { return ConvertHelper.GetInteger(Request["sid"]); } }
        string ScheduleName { get { return ConvertHelper.GetString(Request["name"]); } }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScheduleList.aspx");
        }
    }
}