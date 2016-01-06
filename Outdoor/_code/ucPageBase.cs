using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Outdoor._code
{
    public class ucPageBase : UserControl
    {
        /// <summary>
        /// 用户管理类
        /// </summary>
        protected static UserManager userManager = new UserManager();
        /// <summary>
        /// 是否登录
        /// </summary>
        protected bool IsLogin
        {
            get
            {
                return userManager.CheckLogin();
            }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public Model.Customer Profile
        {
            get
            {
                return userManager.Profile;
            }
        }
    }
}