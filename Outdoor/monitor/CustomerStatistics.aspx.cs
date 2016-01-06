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
    public partial class CustomerStatistics : BasePage
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
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            string cusName = Request["name"];
            txtCusName.Text = cusName;
            string date = Request["date"] == null ? inputDate20.Value : ConvertHelper.GetString(Request["date"]);
            inputDate20.Value = date;
            string OpId = base.Profile.CusId.ToString();
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            string strCondition = "  c.CustomerType=2 ";
            if (!string.IsNullOrEmpty(cusName) && cusName != "请输入账号")
            {
                strCondition += string.Format(" AND c.CusName LIKE '%{0}%'", StringHelper.SqlFilter(cusName));
            }
            recordCount = monitor.GetCustomerListCount(strCondition);
            ds = monitor.GetCustomerStatisticsList(strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("CustomerStatistics.aspx?name={0}&date={1}", cusName, date));
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Literal litPlanCount = (Literal)e.Item.FindControl("litPlanCount");
                int planCount, auditCount;
                string where = "CusId=" + drv["CusId"].ToString();
                if (inputDate20.Value != null && inputDate20.Value != "")
                {
                    inputDate20.Value = inputDate20.Value;
                    string[] dateArr = inputDate20.Value.Split('-');
                    if (dateArr.Length > 1)
                    {
                        where += string.Format(" AND TPId IN(SELECT TPId FROM dbo.TaskProject WHERE BeginDate>='{0} 00:00:00' AND EndDate<='{1} 23:59:59')", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"), ConvertHelper.GetDateTime(dateArr[1]).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        where += string.Format(" AND TPId IN(SELECT TPId FROM dbo.TaskProject WHERE BeginDate>='{0} 00:00:00' AND EndDate<='{0} 23:59:59')", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    }
                }
                planCount = monitor.GetTaskProjectUserRelationCount(where);
                where += " AND Relation=1";
                auditCount = monitor.GetTaskProjectUserRelationCount(where);
                litPlanCount.Text = planCount + "/" + auditCount;
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("CustomerStatistics.aspx?name={0}&date={1}", txtCusName.Text, inputDate20.Value));
        }
        #endregion
    }
}