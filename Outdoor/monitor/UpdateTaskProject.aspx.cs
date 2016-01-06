using AutoRadio.RadioSmart.Common;
using Dal;
using Model;
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
    public partial class UpdateTaskProject : BasePage
    {
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                base.CheckLoginOn();
                if (TpId == 0)
                {
                    ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('操作出错，返回上个页面！')");
                    Response.Redirect(string.Format("ViewPlan.aspx?tid={0}&type={1}&brand={2}&mediatype={3}&city={4}&date={5}", Tid, Type, Brand, MediaType, City, Date));
                    return;
                }
                else
                {
                    bindData();
                }
            }
        }

        void bindData()
        {
            DataTable dt = monitor.GetTaskPlanList(" tp.TPId=" + TpId, 1, 20).Tables[0];
            if (dt.Rows.Count > 0)
            {
                hfTid.Value = dt.Rows[0]["TId"].ToString();
                hfStatus.Value = dt.Rows[0]["Status"].ToString();
                txtCity.Value = dt.Rows[0]["RegionName"].ToString();
                txtAreaName.Value = dt.Rows[0]["AreaName"].ToString();
                txtStreetAddress.Value = dt.Rows[0]["StreetAddress"].ToString();
                txtBlockName.Value = dt.Rows[0]["BlockName"].ToString();
                txtPointName.Value = dt.Rows[0]["PointName"].ToString();
                txtMediaType.Value = dt.Rows[0]["MediaType"].ToString();
                txtAdProductName.Value = dt.Rows[0]["AdProductName"].ToString();
                inputDate20.Value = ConvertHelper.GetDateTimeString(dt.Rows[0]["BeginDate"], "yyyy.MM.dd") + "-" + ConvertHelper.GetDateTimeString(dt.Rows[0]["EndDate"], "yyyy.MM.dd");
                txtPhotoRequire.Value = dt.Rows[0]["PhotoRequire"].ToString();
                txtPrice.Value = dt.Rows[0]["Price"].ToString();
                radioAbnormalType.SelectedValue = dt.Rows[0]["AbnormalType"].ToString();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            string[] dateArr =inputDate20.Value.Split('-');
            if (dateArr.Length < 2)
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('请选择一个周期！')");
                return;
            }
            string regionId = monitor.GetReturnsRegionId(txtCity.Value);
            if (string.IsNullOrEmpty(regionId))
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('城市输入错误！')");
                return;
            }
            try
            {
                TaskProject model = new TaskProject();
                model.TId = ConvertHelper.GetInteger(hfTid.Value);
                model.Status = ConvertHelper.GetInteger(hfStatus.Value);
                model.TPId = TpId;
                model.RegionId = regionId;
                model.AreaName = txtAreaName.Value;
                model.StreetAddress = txtStreetAddress.Value;
                model.BlockName = txtBlockName.Value;
                model.PointName = txtPointName.Value;
                model.MediaType = txtMediaType.Value;
                model.AdProductName = txtAdProductName.Value;
                model.BeginDate = ConvertHelper.GetDateTime(dateArr[0]);
                model.EndDate = ConvertHelper.GetDateTime(dateArr[1]);
                model.PhotoRequire = txtPhotoRequire.Value;
                model.Price = ConvertHelper.GetDecimal(txtPrice.Value);
                model.AbnormalType = ConvertHelper.GetInteger(radioAbnormalType.SelectedValue);
                monitor.UpdateTaskProject(model);
                Response.Redirect(string.Format("ViewPlan.aspx?tid={0}&type={1}&brand={2}&mediatype={3}&city={4}&date={5}", Tid, Type, Brand, MediaType, City, Date));
            }
            catch (Exception ex)
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('操作出错！')");
                Loger.Current.Write("UpdateTaskProject error=" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ViewPlan.aspx?tid={0}&type={1}&brand={2}&mediatype={3}&city={4}&date={5}", Tid, Type, Brand, MediaType, City, Date));
        }

        int TpId { get { return ConvertHelper.GetInteger(Request["tpid"]); } }
        int Tid { get { return ConvertHelper.GetInteger(Request["tid"]); } }
        string Type { get { return ConvertHelper.GetString(Request["type"]); } }
        string Brand { get { return ConvertHelper.GetString(Request["brand"]); } }
        string MediaType { get { return ConvertHelper.GetString(Request["mediatype"]); } }
        string City { get { return ConvertHelper.GetString(Request["city"]); } }
        string Date { get { return ConvertHelper.GetString(Request["date"]); } }
    }
}