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
    public partial class CustomerList : BasePage
    {
        Monitor monitor = new Monitor();
        protected string pageStr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            if (!IsPostBack)
            {
                jianbo_UserMenu.MenuType = 4;
                BindPageList();
            }
        }

        #region 绑定列表
        /// <summary>
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            string txt_key = Request["cusname"];
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            string strCondition = " c.CustomerType=2 ";
            if (!string.IsNullOrEmpty(txt_key) && txt_key != "请输入账号")
            {
                strCondition += string.Format(" AND c.CusName like '%{0}%' ", StringHelper.SqlFilter(txt_key));
            }
            recordCount = monitor.GetCustomerListCount(strCondition);
            ds = monitor.GetCustomerList(strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("CustomerList.aspx?cusname={0}", txt_key));
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int cusId = ConvertHelper.GetInteger(drv["CusId"]);
                if (drv["RegionId"].ToString() != "0")
                {
                    Literal litRegionName = (Literal)e.Item.FindControl("litRegionName");
                    DataTable dt = monitor.GetRegionCodeLevelList(drv["RegionId"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        litRegionName.Text = dt.Rows[0]["FullName"].ToString();
                    }
                }
                Literal litAction = (Literal)e.Item.FindControl("litAction");
                if (ConvertHelper.GetInteger(drv["IsDisabled"]) == 0)
                {
                    litAction.Text = "<a href=\"AddCustomer.aspx?CusId=" + cusId + "\" class=\"tab_a\">编辑</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"IsDisabled(" + cusId + ",1)\">停用</a>";
                }
                else
                {
                    litAction.Text = "<a href=\"AddCustomer.aspx?CusId=" + cusId + "\" class=\"tab_a\">编辑</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"IsDisabled(" + cusId + ",0)\">启用</a>";
                }
                if (ConvertHelper.GetInteger(drv["UserId"]) > 0)
                {
                    litAction.Text += "<a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"UnBind(" + ConvertHelper.GetInteger(drv["UserId"]) + ")\">解绑</a>";
                }
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            string txt_key = this.txt_key.Text.ToString();
            Response.Redirect(string.Format("CustomerList.aspx?cusname={0}", txt_key));
        }
        #endregion

        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddCustomer.aspx");
        }
    }
}