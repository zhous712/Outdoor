using System;
using System.Web; 
using System.Web.SessionState;  
using AutoRadio.RadioSmart.Common;
using Outdoor._code;
using Dal;
using AutoRadio.RadioSmart.Common.Security;

namespace Outdoor.ashx
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
    {
        #region 变量
        //{status:0,info:{uid:'',uname:'',taskCount:'0',taskCount2:'0',chCount:'0',shortuname:''}
        // status: -5已锁定 -4未知错误 -3验证码错误 -2用户名或密码错误 -1用户名或密码不能为空 0未登录 1登录成功
        private string responseTextModel = "({{status:'{0}',info:{{uid:'{1}',uname:'{2}'}}}})";
         
        /// <summary>
        /// 用户提交操作的类型
        /// </summary>
        private int type = 0;

        /// <summary>
        /// 用户提交操作的用户名
        /// </summary>
        private string userName = "";
        
        /// <summary>
        /// 用户提交操作的密码
        /// </summary>
        private string password = "";

        private UserManager userManager = new UserManager();
        private Monitor monitor = new Monitor();
        #endregion

        #region 方法
        public void ProcessRequest(HttpContext context)
        { 
            try
            {
                type = Int32.Parse(context.Request.Form["type"]);
                userName = context.Request.Form["username"] == null ? "" : context.Request.Form["username"];
                password = context.Request.Form["password"] == null ? "" : context.Request.Form["password"].ToString();
            }
            catch 
            {
                type = 0;
                userName = "";
                password = "";
            }
            context.Response.ContentType = "text/HTML";
            ValidateParams(context);
            switch (type)
            {
                case 1: LoginUser(); break;
                case 2: CheckLogin(); break;
                case 3: ValidateUser(); break;
                case 4:
                    LogOut();
                    break;
            }
        }

        /// <summary>
        /// 过滤参数
        /// </summary>
        private void ValidateParams(HttpContext context)
        {
            userName = StringHelper.XSSFilter(StringHelper.SqlFilter(userName));
            //password = StringHelper.XSSFilter(StringHelper.SqlFilter(password));
            if ((type==1 ||type==3 )&& (userName == null || userName.Length < 1 || userName == null || userName.Length < 1))
            {
                //用户名或密码不能为空
                context.Response.ContentType = "text/HTML";
                context.Response.Write("{status:-1,info:null}");
                context.Response.End();
            }
        }
         
        /// <summary>
        /// 验证用户
        /// </summary>
        private void ValidateUser()
        {
            string responseText = "";
            ValidationResult result = userManager.Login(userName, password);
            if (result == ValidationResult.Success)
            {
                responseText = string.Format(responseTextModel, "1", userManager.Profile.CusId.ToString(), userManager.Profile.CusName);
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            }
            else if (result == ValidationResult.InvalidUserNameOrPassword)
            {
                responseText = string.Format(responseTextModel, "-2", "", "");
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            }
            else
            {
                responseText = string.Format(responseTextModel, "-4", "", "");
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            } 
        }

        /// <summary>
        /// 验证登陆状态
        /// </summary>
        private void CheckLogin()
        {
            HttpContext context = HttpContext.Current;
            string responseText = "";
            if (userManager.CheckLogin())
            {
                //已登录，输出用户信息
                responseText = string.Format(responseTextModel, "1", userManager.Profile.CusId.ToString(), userManager.Profile.CusName);
                responseText=responseText.Replace("<","{").Replace(">","}");  
                context.Response.Write(responseText);
                context.Response.End();
            }
            else
            {
                //未登录，输出最后登录用户名，体验用户也输出密码
                responseText = string.Format(responseTextModel, "0", "", "");
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                context.Response.Write(responseText);
                context.Response.End();  
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        private void LoginUser()
        {
            //string sessionCode="";
            string responseText = ""; 
            //登录
            ValidationResult result= userManager.Login(userName, password);
            if (result == ValidationResult.Success)
            {
                responseText = string.Format(responseTextModel, "1", userManager.Profile.CusId.ToString(), userManager.Profile.CusName);              
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            }
            else if (result == ValidationResult.InvalidUserNameOrPassword)
            { 
                responseText = string.Format(responseTextModel, "-2", "", "");
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            }
            else if (result == ValidationResult.LockedOut)
            {
                responseText = string.Format(responseTextModel, "-5", "", "");
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            }
            else
            {
                responseText = string.Format(responseTextModel, "-4", "", "");
                responseText = responseText.Replace("<", "{").Replace(">", "}");
                HttpContext.Current.Response.Write(responseText);
                HttpContext.Current.Response.End();
            } 
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        private void LogOut()
        {
            userManager.Logout();
            string responseText = string.Format(responseTextModel, "0", "","");
            responseText = responseText.Replace("<", "{").Replace(">", "}");
            HttpContext.Current.Response.Write(responseText);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 登录后写cookie的damain
        /// </summary>
        private string defaultDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["logindomain"];
            }
        }
      

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }        
        #endregion
    }
}
