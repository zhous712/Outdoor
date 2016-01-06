using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;
using AutoRadio.RadioSmart.Common;
using Dal;
using System.Configuration;
using System.Data;

namespace Outdoor.monitor
{
    public partial class ViewImage : BasePage
    {
        protected int TPUId;
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            TPUId = ConvertHelper.GetInteger(Request["tpuid"]);
            if (!IsPostBack)
            {
                DataTable dt = monitor.GetImageDetailList(TPUId);
                int abnormalType = monitor.GetTaskProjectAbnormalType(TPUId);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append(dr["ImgPath"].ToString() + "," + dr["Sort"].ToString() + "|");
                    if (Profile.CustomerType == 0 && abnormalType == 0)
                    {
                        break;
                    }
                }
                hfImagePath.Value = sb.ToString();
                hfDirectory.Value = ConfigurationManager.AppSettings["WeiXinUploadImage"] + "/";
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
            if (HiddenField1.Value == "," || string.IsNullOrEmpty(HiddenField1.Value))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>effect.Dialog.alert('错误操作！');</script>");
                return;
            }
            else
            {
                string[] arrImgPath = HiddenField1.Value.Split('|');
                for (int i = 0; i < arrImgPath.Length; i++)
                {
                    var arrPath = arrImgPath[i].Split(',');
                    if (arrPath.Length > 1)
                    {
                        monitor.UpdateImageDetailSort(arrPath[0], ConvertHelper.GetInteger(arrPath[1]));
                    }
                }
                Response.Redirect("ViewImage.aspx?tpuid=" + TPUId);
            }
        }
    }
}