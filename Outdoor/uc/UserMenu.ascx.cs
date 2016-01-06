using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dal;
using Outdoor._code;

namespace Outdoor.uc
{
    public partial class UserMenu : ucPageBase
    {
        public int MenuType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindMenuType();
            if (base.Profile.CustomerType==0)
            {
                LiMoneyStatistics.Visible = false;
                LiCustomerList.Visible = false;
                LiTaskStatistics.Visible = false;
            }
            if (base.Profile.CustomerType==2)
            {
                LiTaskAdd.Visible = false;
                LiMoneyStatistics.Visible = false;
                LiCustomerList.Visible = false;
                LiTaskStatistics.Visible = false;
            }
        }
        private void BindMenuType()
        {
            switch (this.MenuType)
            {
                case 1:
                    LiTaskAdd.Attributes["class"] = "hover";
                    break;
                case 2:
                    LiScheduleList.Attributes["class"] = "hover";
                    break;
                case 4:
                    LiCustomerList.Attributes["class"] = "hover";
                    break;
                case 5:                  
                    LiMoneyStatistics.Attributes["class"] = "hover";
                    break;
                case 6:
                    LiTaskStatistics.Attributes["class"] = "hover";
                    break;
            }
        }
    }
}