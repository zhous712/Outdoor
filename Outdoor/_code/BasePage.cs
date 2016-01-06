using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Outdoor._code
{
    public class BasePage : System.Web.UI.Page
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

        protected void CheckLoginOn()
        {
            if (!this.IsLogin)
            {
                Response.Redirect(this.Request.Url.ToString().Contains("ref") ? this.Request.Url.ToString() : "../Login.aspx?ref=" + Server.UrlEncode(this.Request.Url.ToString()));
            }
        }

        /// <summary>
        /// 生成页码列表
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="pageSize">显示多少条</param>
        /// <param name="totalPage">总记录数</param>
        protected string BindPageList(int page, int pageSize, int totalRecord, string Url)
        {
            int totalPage = (totalRecord % pageSize == 0) ? (totalRecord / pageSize) : (totalRecord / pageSize + 1);
            int PrevPageCount = 1;
            int NextPageCount = 1;
            if (page <= PrevPageCount) PrevPageCount = page - 1;
            if (page + NextPageCount > totalPage) NextPageCount = totalPage - page;
            int StartPage = page - PrevPageCount;
            int EndPage = page + NextPageCount;
            string actionUrl = Url;

            StringBuilder sb = new StringBuilder();
            //if (totalRecord > pageSize)
            //{
            sb.Append("<div class=\"manu\">");
            sb.Append(CreatePageUrl("首页", actionUrl, 1, page, false));
            sb.Append(CreatePageUrl("上一页", actionUrl, (page > 1) ? page - 1 : 1, page, false));
            sb.Append(CreatePageUrl("下一页", actionUrl, (page < totalPage) ? (page + 1) : totalPage, page, false));
            sb.Append(CreatePageUrl("末页", actionUrl, totalPage, page, false));
            sb.Append("&nbsp;第&nbsp;");
            string onchangeUrl = string.IsNullOrEmpty(actionUrl) ? "?page=" : actionUrl + "&page=";
            sb.AppendFormat("<input type=\"text\" class=\"page_txt\"  value=\"{0}\" id=\"page_text\" onkeyup=\"dokeyup(event)\" rel=\"" + onchangeUrl + "\" pCount=\"" + totalPage + "\" onfocus=\"this.select();\"/><input type=\"hidden\" id=\"hdpage\" value=\"{0}\" />", page);
            sb.AppendFormat("&nbsp;/{0}页&nbsp;&nbsp;&nbsp;&nbsp;共{1}条记录", totalPage, totalRecord);
            sb.Append("</div>");
            //}
            return sb.ToString();
        }

        private string CreatePageUrl(string text, string baseurl, int page, int curPage, bool highlight)
        {
            return String.Format("<a href='{1}'>{0}</a> ", text, AddUrlParam(baseurl, "page", page.ToString()));
        }

        private string AddUrlParam(string url, string paramname, string param)
        {
            if (url == "")
                return "?" + paramname + "=" + param;
            else
                return url + "&" + paramname + "=" + param;
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

        /// <summary>
        /// 度分秒经纬度(必须含有'°')和数字经纬度转换
        /// </summary>
        /// <param name="digitalDegree">度分秒经纬度</param>
        /// <return>数字经纬度</return>
        public static double ConvertDegreesToDigital(string degrees)
        {
            double num = 60;
            double digitalDegree = 0.0;
            int d = degrees.IndexOf('°');           //度的符号对应的 Unicode 代码为：00B0[1]（六十进制），显示为°。
            if (d < 0)
            {
                return digitalDegree;
            }
            string degree = degrees.Substring(0, d);
            digitalDegree += Convert.ToDouble(degree);

            int m = degrees.IndexOf('′');           //分的符号对应的 Unicode 代码为：2032[1]（六十进制），显示为′。
            if (m < 0)
            {
                return digitalDegree;
            }
            string minute = degrees.Substring(d + 1, m - d - 1);
            digitalDegree += ((Convert.ToDouble(minute)) / num);

            int s = degrees.IndexOf('″');           //秒的符号对应的 Unicode 代码为：2033[1]（六十进制），显示为″。
            if (s < 0)
            {
                return digitalDegree;
            }
            string second = degrees.Substring(m + 1, s - m - 1);
            digitalDegree += (Convert.ToDouble(second) / (num * num));

            return digitalDegree;
        }
    }
}