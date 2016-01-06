using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;

namespace Outdoor.uc
{
    public partial class Header : ucPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetReferer(true).IndexOf("lexus") > 0)
            {
                divLexus.Visible = true;
                divTel.Visible = false;
            }
            else
            {
                divLexus.Visible = false;
                divTel.Visible = true;
            }
        }

        /// <summary>
        /// 获取HTTP请求的Referer
        /// </summary>
        /// <param name="ishost">Referer为空时是否返回Host（网站首页地址）</param>
        /// <returns>string</returns>
        public string GetReferer(bool ishost)
        {
            if (Request.UrlReferrer != null)
            {
                return Request.UrlReferrer.ToString();
            }
            else
            {
                if (ishost)
                {
                    return Request.Url.Scheme + "://" + Request.Url.Authority;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}