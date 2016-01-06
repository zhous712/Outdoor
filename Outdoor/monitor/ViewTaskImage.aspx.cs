using AutoRadio.RadioSmart.Common;
using Dal;
using Outdoor._code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outdoor.monitor
{
    public partial class ViewTaskImage : BasePage
    {
        protected int TId;
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            TId = ConvertHelper.GetInteger(Request["tid"]);
            if (!IsPostBack)
            {
                string[] picturePathArr = monitor.GetTaskPicturePath(TId).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (string item in picturePathArr)
                {
                    sb.Append(item + "|");
                }
                hfImagePath.Value = sb.ToString();
                hfDirectory.Value = ConfigurationManager.AppSettings["TaskImage"] + "/";
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
    }
}