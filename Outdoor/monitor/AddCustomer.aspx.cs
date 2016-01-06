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
    public partial class AddCustomer : BasePage
    {
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                base.CheckLoginOn();
                jianbo_UserMenu.MenuType = 4;
                bindProvince();
                bindCity("0");
                bindArea("0");
                if (CusId > 0)
                {
                    bindData();
                }

            }
        }

        void bindProvince()
        {
            DataTable dt = monitor.GetRegionList("0", 1).Tables[0];
            ddlProvince.DataSource = dt;
            ddlProvince.DataTextField = "RegionName";
            ddlProvince.DataValueField = "RegionId";
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("全部", "0"));
            ddlProvince.SelectedValue = "0";
        }

        void bindCity(string regionId)
        {
            DataTable dt = monitor.GetRegionList(regionId, 2).Tables[0];
            ddlCity.DataSource = dt;
            ddlCity.DataTextField = "RegionName";
            ddlCity.DataValueField = "RegionId";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, new ListItem("全部", "0"));
            ddlCity.SelectedValue = "0";

        }

        void bindArea(string regionId)
        {
            DataTable dt = monitor.GetRegionList(regionId, 3).Tables[0];
            ddlArea.DataSource = dt;
            ddlArea.DataTextField = "RegionName";
            ddlArea.DataValueField = "RegionId";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("全部", "0"));
            ddlArea.SelectedValue = "0";
        }

        void bindData()
        {
            string regionId = "0";
            DataTable dt = monitor.GetCustomerById(CusId);
            if (dt.Rows.Count > 0)
            {
                txtCusName.Value = dt.Rows[0]["CusName"].ToString();
                txtPwd.Value = dt.Rows[0]["cPassWord"].ToString();
                txtEmail.Value = dt.Rows[0]["Email"].ToString();
                txtPhone.Value = dt.Rows[0]["Phone"].ToString();
                regionId = dt.Rows[0]["RegionId"].ToString();
                dt = monitor.GetRegionCodeLevelList(regionId);
                if (dt.Rows.Count>0)
                {
                    if (ConvertHelper.GetInteger(dt.Rows[0]["CodeLevel"]) == 1)
                    {
                        ddlProvince.SelectedValue = regionId;
                    }
                    else if (ConvertHelper.GetInteger(dt.Rows[0]["CodeLevel"]) == 2)
                    {
                        ddlCity.SelectedValue = regionId;
                        ddlProvince.SelectedValue = dt.Rows[0]["ParentId"].ToString();
                    }
                    else if (ConvertHelper.GetInteger(dt.Rows[0]["CodeLevel"]) == 3)
                    {
                        ddlArea.SelectedValue = regionId;
                        ddlCity.SelectedValue = dt.Rows[0]["ParentId"].ToString();
                        dt = monitor.GetRegionCodeLevelList(dt.Rows[0]["ParentId"].ToString());
                        if (dt.Rows.Count>0)
                        {
                            ddlProvince.SelectedValue = dt.Rows[0]["ParentId"].ToString();
                        }
                    }
                }
            }
        }

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindCity(this.ddlProvince.SelectedValue);
        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindArea(this.ddlCity.SelectedValue);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (monitor.GetCusIDByUserName(txtCusName.Value, CusId) > 0)
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('当前账号名称已经存在！')");
            }
            else
            {
                try
                {
                    string regionId = "0";
                    if (ddlArea.SelectedValue != "0")
                    {
                        regionId = ddlArea.SelectedValue;
                    }
                    else if (ddlCity.SelectedValue != "0")
                    {
                        regionId = ddlCity.SelectedValue;
                    }
                    else
                    {
                        regionId = ddlProvince.SelectedValue;
                    }
                    if (CusId == 0)
                    {
                        monitor.AddCustomer(txtCusName.Value, txtPwd.Value, 2, txtEmail.Value, txtPhone.Value, regionId);
                    }
                    else
                    {
                        monitor.UpdateCustomer(txtCusName.Value, txtPwd.Value, 2, txtEmail.Value, txtPhone.Value, regionId, CusId);
                    }
                    Response.Redirect("CustomerList.aspx");
                }
                catch (Exception ex)
                {
                    ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('操作出错！')");
                    Loger.Current.Write("AddCustomer error=" + ex.Message);
                }

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomerList.aspx");
        }

        int CusId { get { return ConvertHelper.GetInteger(Request["cusid"]); } }
    }
}