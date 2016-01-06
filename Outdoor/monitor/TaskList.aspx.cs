using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;
using Dal;
using AutoRadio.RadioSmart.Common;
using System.Data;
using System.Configuration;
using System.Text;

namespace Outdoor.monitor
{
    public partial class TaskList : BasePage
    {
        protected string pageStr = string.Empty;
        Monitor monitor = new Monitor();
        string TaskXLS = ConfigurationManager.AppSettings["TaskXLS"];
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            pTipContent.InnerHtml = "该排期下目前没有任务，现在立即<img src=\"../images/icon_new.png\" /><a href=\"addtask.aspx\" target=\"_parent\" class=\"green\">创建</a> 吧！";
            if (!IsPostBack)
            {
                txt_key.Text = Request["brand"];
                jianbo_UserMenu.MenuType = 2;
                BindPageList();
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

        #region 绑定列表
        /// <summary>
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            string OpId = base.Profile.CusId.ToString();
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            string strCondition = " t.TId IN (SELECT TId FROM dbo.Task WHERE SId=" + SId + ")";
            strCondition += base.Profile.CustomerType == 0 ? string.Format(" AND t.CusId={0} ", OpId) : "";
            if (Profile.CustomerType == 2)
            {
                DataTable dt = monitor.GetTids(Profile.CusId);
                string tids = "";
                foreach (DataRow dr in dt.Rows)
                {
                    tids += dr["TId"].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(tids))
                {
                    strCondition += " AND t.TId IN (" + tids.Trim(',') + ")";
                }
            }
            if (!string.IsNullOrEmpty(txt_key.Text) && txt_key.Text != "请输入任务编号或名称")
            {
                int sTid = ConvertHelper.GetInteger(txt_key.Text);
                if (sTid != 0)
                {
                    strCondition += string.Format(" AND t.TId = {0} ", sTid);
                }
                else
                {
                    strCondition += string.Format(" AND t.TaskName like '%{0}%' ", StringHelper.SqlFilter(txt_key.Text));
                }
            }
            recordCount = monitor.GetTaskListCount(strCondition);
            if (recordCount == 0)
            {
                this.NoDivList.Visible = true;
            }
            else
            {
                this.YesDivList.Visible = true;
                YesDivListTop.Visible = true;
            }
            ds = monitor.GetTaskList(strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("TaskList.aspx?sid={0}&brand={1}", SId, txt_key.Text));
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int tid = ConvertHelper.GetInteger(drv["Tid"]);
                Literal litTaskName = (Literal)e.Item.FindControl("litTaskName");
                litTaskName.Text = "<a href=\"" + (Profile.CustomerType == 2 ? "MyTaskList.aspx" : "ViewPlan.aspx") + "?tid=" + tid + "&sid=" + SId + "\" title=\"" + drv["TaskName"].ToString() + "\" class=\"tab_a\">" + StringHelper.SubString(drv["TaskName"].ToString(), 30, false) + "</a>";
                Literal litPlanCount = (Literal)e.Item.FindControl("litPlanCount");
                int planCount, auditCount;
                planCount = monitor.GetTaskPlanListCount(" TId=" + tid);
                auditCount = monitor.GetTaskPlanListCount(" TId=" + tid + " AND Status=3");
                litPlanCount.Text = planCount + "/" + auditCount;
                Literal litImage = (Literal)e.Item.FindControl("litImage");
                litImage.Text = "<a href=\"ViewTaskImage.aspx?tid=" + tid + "\" class=\"tab_a\" target=\"_blank\">查看</a>";
                Literal litAction = (Literal)e.Item.FindControl("litAction");
                if (ConvertHelper.GetInteger(drv["Status"]) == 0)
                {
                    if (Profile.CustomerType == 1)
                    {
                        litAction.Text = "<a href=\"ViewPlan.aspx?tid=" + tid + "&sid=" + SId + "\" class=\"tab_a\">查看点位</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"ReleaseTask(" + tid + ")\">发布</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"DelTask(" + tid + "," + SId + ")\">删除</a>";
                    }
                    else if (Profile.CustomerType == 0)
                    {
                        litAction.Text = "<a href=\"ViewPlan.aspx?tid=" + tid + "&sid=" + SId + "\" class=\"tab_a\">查看点位</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"DelTask(" + tid + "," + SId + ")\">删除</a>";
                    }
                    else
                    {
                        litAction.Text = "<a href=\"MyTaskList.aspx?tid=" + tid + "\" class=\"tab_a\">查看点位</a>";
                    }
                }
                else
                {
                    litAction.Text = "<a href=\"" + (Profile.CustomerType == 2 ? "MyTaskList.aspx" : "ViewPlan.aspx") + "?tid=" + tid + "&sid=" + SId + "\" class=\"tab_a\">查看点位</a>";
                }
                litAction.Text += "<a href=\"" + string.Format("http://{0}/{1}", TaskXLS, drv["ExcelPath"]) + "\" class=\"tab_a\">下载排期</a>";
            }
        }
        #endregion

        int SId { get { return ConvertHelper.GetInteger(Request["sid"]); } }

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            string txt_key = this.txt_key.Text.ToString();
            Response.Redirect(string.Format("TaskList.aspx?sid={0}&brand={1}", SId, txt_key));
        }
        #endregion

        #region 获取任务状态
        protected string GetTaskStatus(string status)
        {
            string str = "";
            switch (status)
            {
                case "0": str = "草稿"; break;
                case "1": str = "发布"; break;
                case "2": str = "进行中"; break;
                case "3": str = "已完成"; break;
                case "4": str = "已过期";
                    if (base.Profile.CusId == 3)
                    {
                        str = "已完成";
                    }
                    break;
            }
            return str;
        }
        #endregion
    }
}