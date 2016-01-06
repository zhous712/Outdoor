using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;
using System.Configuration;
using Dal;
using AutoRadio.RadioSmart.Common;
using System.Data;

namespace Outdoor.monitor
{
    public partial class MoneyStatistics : BasePage
    {
        protected string pageStr = string.Empty;
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            if (!IsPostBack)
            {
                jianbo_UserMenu.MenuType = 5;
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
            string txt_key = Request["brand"];
            string OpId = base.Profile.CusId.ToString();
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            string strCondition = "";
            if (!string.IsNullOrEmpty(txt_key) && txt_key != "请输入昵称")
            {
                strCondition += string.Format(" AND tpur.UserId IN (SELECT UserId FROM dbo.WerXinUser WHERE NickName LIKE '%{0}%') ", StringHelper.SqlFilter(txt_key));
            }
            recordCount = monitor.GetMoneyStatisticsCount(strCondition);
            ds = monitor.GetMoneyStatisticsList(strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("MoneyStatistics.aspx?brand={0}", txt_key));
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Literal litAction = (Literal)e.Item.FindControl("litAction");
                litAction.Text = "<a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"PayMoney(" + ConvertHelper.GetInteger(drv["UserId"]) + ")\">支付</a>";
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            string txt_key = this.txt_key.Text.ToString();
            if (txt_key == "" || txt_key == "请输入昵称")
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "jscancelaudit", "<script>alert('请输入昵称！')</script>");
                return;
            }
            Response.Redirect(string.Format("MoneyStatistics.aspx?brand={0}", txt_key));
        }
        #endregion
    }
}