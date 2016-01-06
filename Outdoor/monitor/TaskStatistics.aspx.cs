using AutoRadio.RadioSmart.Common;
using Dal;
using Outdoor._code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outdoor.monitor
{
    public partial class TaskStatistics : BasePage
    {
        protected string pageStr = string.Empty;
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            if (!IsPostBack)
            {
                jianbo_UserMenu.MenuType = 6;
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
        /// 绑定页面数据
        /// </summary>
        private void BindPageList()
        {
            string cusName = Request["name"];
            string mediaType = Request["type"];
            string regionName = Request["city"];
            string date = Request["date"] == null ? inputDate20.Value : ConvertHelper.GetString(Request["date"]);
            txtCusName.Text = cusName;
            txtMediaType.Text = mediaType;
            txtCity.Text = regionName;
            string OpId = base.Profile.CusId.ToString();
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            string strCondition = "";
            if (!string.IsNullOrEmpty(cusName) && cusName != "请输入客户")
            {
                strCondition += string.Format(" AND t.CusName LIKE '%{0}%'", StringHelper.SqlFilter(cusName));
            }
            if (!string.IsNullOrEmpty(mediaType) && mediaType != "请输入媒体类型")
            {
                strCondition += string.Format(" AND tp.MediaType LIKE '%{0}%'", StringHelper.SqlFilter(mediaType));
            }
            if (!string.IsNullOrEmpty(regionName) && regionName != "请输入城市")
            {
                string regionId = monitor.GetReturnsRegionId(regionName);
                strCondition += string.Format(" AND tp.RegionId = '{0}'", regionId);
            }
            if (date != null && date != "")
            {
                inputDate20.Value = date;
                string[] dateArr = inputDate20.Value.Split('-');
                if (dateArr.Length > 1)
                {
                    strCondition += string.Format(" AND tp.BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    strCondition += string.Format(" AND tp.EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[1]).ToString("yyyy-MM-dd"));
                }
                else
                {
                    strCondition += string.Format(" AND tp.BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    strCondition += string.Format(" AND tp.EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                }
            }
            recordCount = monitor.GetTaskStatisticsCount(strCondition);
            ds = monitor.GetTaskStatisticsList(strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("TaskStatistics.aspx?name={0}&type={1}&city={2}&date={3}", cusName, mediaType, regionName, date));
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Literal litPlanCount = (Literal)e.Item.FindControl("litPlanCount");
                int planCount, auditCount;
                string where = "TId IN(SELECT TId FROM dbo.Task WHERE CusId=" + drv["CusId"].ToString() + " AND Status>0) AND RegionId='" + drv["RegionId"].ToString() + "' AND MediaType='" + drv["MediaType"].ToString() + "'";
                if (inputDate20.Value != null && inputDate20.Value != "")
                {
                    inputDate20.Value = inputDate20.Value;
                    string[] dateArr = inputDate20.Value.Split('-');
                    if (dateArr.Length > 1)
                    {
                        where += string.Format(" AND tp.BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                        where += string.Format(" AND tp.EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[1]).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        where += string.Format(" AND tp.BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                        where += string.Format(" AND tp.EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    }
                }
                planCount = monitor.GetTaskPlanListCount(where);
                where += " AND Status=3";
                auditCount = monitor.GetTaskPlanListCount(where);
                litPlanCount.Text = planCount + "/" + auditCount;
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("TaskStatistics.aspx?name={0}&type={1}&city={2}&date={3}", txtCusName.Text, txtMediaType.Text, txtCity.Text, inputDate20.Value));
        }
        #endregion
    }
}