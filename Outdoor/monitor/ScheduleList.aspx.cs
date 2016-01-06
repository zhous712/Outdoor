using AutoRadio.RadioSmart.Common;
using Dal;
using Outdoor._code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outdoor.monitor
{
    public partial class ScheduleList : BasePage
    {
        Monitor monitor = new Monitor();
        protected string pageStr = string.Empty;
        protected string showList = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            showList = Request["type"];
            if (!IsPostBack)
            {
                jianbo_UserMenu.MenuType = 2;
                Big();
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
        void Big()
        {
            string OpId = base.Profile.CusId.ToString();
            string sqlWhere = " t.CreateDate>'2015-09-25' AND tp.MediaType IS NOT NULL AND tp.MediaType!=''";
            if (base.Profile.CustomerType == 0)
            {
                sqlWhere += " AND t.CusId=" + OpId;
            }
            else if (base.Profile.CustomerType == 2)
            {
                sqlWhere += " AND tp.TPId IN(SELECT TPId FROM dbo.TaskProjectUserRelation WHERE CusId=" + OpId + ")";
            }
            DataTable dt = monitor.GetMediaTypeList(sqlWhere);
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"fx\">");
            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(showList))
                {
                    showList = dr["MediaType"].ToString();
                }
                sb.Append("<li " + (showList == dr["MediaType"].ToString() ? "class=\"on\"" : "") + "><a href=\"?type=" + dr["MediaType"].ToString() + "\">" + dr["MediaType"].ToString() + "</a></li>");
            }
            sb.Append("</ul>");
            divMediaType.InnerHtml = sb.ToString();
        }

        /// <summary>
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            txt_key.Text = Request["name"];
            inputDate20.Value = Request["date"];
            string OpId = base.Profile.CusId.ToString();
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            string strCondition = " SId IN (SELECT DISTINCT SId FROM dbo.Task WHERE TId IN (SELECT DISTINCT TId FROM dbo.TaskProject WHERE MediaType='" + showList + "')) ";
            if (Profile.CustomerType == 0)
            {
                strCondition = " SId IN (SELECT DISTINCT SId FROM dbo.Task WHERE TId IN (SELECT DISTINCT TId FROM dbo.TaskProject WHERE MediaType='" + showList + "') AND CusId=" + OpId + ") ";
            }
            else if (Profile.CustomerType == 2)
            {
                DataTable dt = monitor.GetTids(Profile.CusId, showList);
                string tids = "";
                foreach (DataRow dr in dt.Rows)
                {
                    tids += dr["TId"].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(tids))
                {
                    strCondition = " SId IN (SELECT DISTINCT SId FROM dbo.Task WHERE TId IN (" + tids.Trim(',') + ")) ";
                }
            }
            if (!string.IsNullOrEmpty(txt_key.Text) && txt_key.Text != "请输入排期编号或名称")
            {
                int sid = ConvertHelper.GetInteger(txt_key.Text);
                if (sid != 0)
                {
                    strCondition += string.Format(" AND SId = {0} ", sid);
                }
                else
                {
                    strCondition += string.Format(" AND ScheduleName like '%{0}%' ", StringHelper.SqlFilter(txt_key.Text));
                }
            }
            if (!string.IsNullOrEmpty(inputDate20.Value))
            {
                string[] dateArr = inputDate20.Value.Split('-');
                if (dateArr.Length > 1)
                {
                    strCondition += string.Format(" AND BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    strCondition += string.Format(" AND EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[1]).ToString("yyyy-MM-dd"));
                }
                else
                {
                    strCondition += string.Format(" AND BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    strCondition += string.Format(" AND EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                }
            }
            recordCount = monitor.GetScheduleListCount(strCondition);
            ds = monitor.GetScheduleList(strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("ScheduleList.aspx?type={0}&date={1}&name={2}", showList, inputDate20.Value, txt_key.Text));
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int sid = ConvertHelper.GetInteger(drv["SId"]);
                Literal litScheduleName = (Literal)e.Item.FindControl("litScheduleName");
                litScheduleName.Text = Profile.CustomerType == 1 ? "<a href=\"TaskList.aspx?sid=" + sid + "\" title=\"" + drv["ScheduleName"].ToString() + "\" class=\"tab_a\">" + drv["ScheduleName"].ToString() + "</a>" : drv["ScheduleName"].ToString();
                Literal litPlanCount = (Literal)e.Item.FindControl("litPlanCount");
                Literal litAuditCount = (Literal)e.Item.FindControl("litAuditCount");
                Literal litSamplingRate = (Literal)e.Item.FindControl("litSamplingRate");
                Literal litQualifiedCount = (Literal)e.Item.FindControl("litQualifiedCount");
                Literal litUnqualifiedCount = (Literal)e.Item.FindControl("litUnqualifiedCount");
                Literal litQualifiedRate = (Literal)e.Item.FindControl("litQualifiedRate");
                int planCount = 0, auditCount = 0, qualifiedCount = 0, unQualifiedCount = 0;
                planCount = monitor.GetTaskPlanListCount(" TId IN(SELECT TId FROM dbo.Task WHERE SId=" + sid + ")");
                auditCount = monitor.GetTaskPlanListCount(" TId IN(SELECT TId FROM dbo.Task WHERE SId=" + sid + ") AND Status=3");
                qualifiedCount = monitor.GetTaskPlanListCount(" TId IN(SELECT TId FROM dbo.Task WHERE SId=" + sid + ") AND Status=3 AND AbnormalType in (0,5)");
                unQualifiedCount = monitor.GetTaskPlanListCount(" TId IN(SELECT TId FROM dbo.Task WHERE SId=" + sid + ") AND Status=3 AND AbnormalType in (1,2,3,4,6,7)");
                litPlanCount.Text = planCount.ToString();
                litAuditCount.Text = auditCount.ToString();
                litSamplingRate.Text = planCount == 0 ? "0%" : Math.Round((Convert.ToDouble(auditCount) / Convert.ToDouble(planCount) * 100), 2) + "%";
                litQualifiedCount.Text = qualifiedCount.ToString();
                litUnqualifiedCount.Text = unQualifiedCount.ToString();
                litQualifiedRate.Text = auditCount == 0 ? "0%" : Math.Round((Convert.ToDouble(qualifiedCount) / Convert.ToDouble(auditCount) * 100), 2) + "%";
                Literal litPlanCycle = (Literal)e.Item.FindControl("litPlanCycle");
                litPlanCycle.Text = ConvertHelper.GetDateTimeString(drv["BeginDate"], "yyyy.MM.dd") + "-" + ConvertHelper.GetDateTimeString(drv["EndDate"], "yyyy.MM.dd");
                Literal litAction = (Literal)e.Item.FindControl("litAction");
                if (Profile.CustomerType == 2)
                {
                    litAction.Text = "<a href=\"TaskList.aspx?sid=" + sid + "\" class=\"tab_a\">查看任务</a>";
                }
                else
                {
                    litAction.Text = "<a href=\"ViewReport.aspx?sid=" + sid + "\" class=\"tab_a\">查看详情</a><a href=\"AddTask.aspx?sid=" + sid + "&name=" + drv["ScheduleName"].ToString() + "\" class=\"tab_a\">添加任务</a>";
                }
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ScheduleList.aspx?type={0}&date={1}&name={2}", showList, inputDate20.Value, txt_key.Text));
        }
        #endregion
    }
}