using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoRadio.RadioSmart.Common.Security;
using Dal;

namespace Outdoor._code
{
    /// <summary>
    ///		本枚举类型表示方法
    ///		<see cref="M:BitAuto.Services.UserManager.IUserManager.ValidateUser()">ValidateUser</see>
    ///		的返回状态
    /// </summary>
    public enum ValidationResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        InvalidUserNameOrPassword,

        /// <summary>
        /// 用户已锁定
        /// </summary>
        LockedOut,

        /// <summary>
        /// 用户未启用或用户未审核
        /// </summary>
        Invalid,

        /// <summary>
        /// 用户已过期
        /// </summary>
        Expired
    }
    public class UserManager
    {
        /// <summary>
        /// 取登录用户信息
        /// </summary>
        public Model.Customer Profile
        {
            get
            {
                if (HttpContext.Current.Session[sessionKey] != null)
                {
                    try
                    {
                        return (Model.Customer)HttpContext.Current.Session[sessionKey];
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
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

        #region 常量

        //private const string defaultDomain = "localhost";        
        private const string sessionKey = "Customer_Profile";
        private const string idCookieKey = "outdooruid";
        #endregion

        #region pulibc方法

        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="_uname"></param>
        /// <param name="_password"></param>
        /// <returns></returns>
        public ValidationResult Login(string _uname, string _password)
        {
            Model.Customer customer = FindUserByName(_uname);
            ValidationResult result = ValidateUserByUserAccount(customer, _password);
            if (result == ValidationResult.Success)
            {
                passPort(customer, true, null);
            }
            return result;
        }

        /// <summary>
        /// 根据用户名查询用户信息
        /// </summary>
        /// <param name="_username"></param>
        /// <returns></returns>
        public Model.Customer FindUserByName(string _username)
        {
            Monitor dal = new Monitor();
            return dal.GetCustomerInfoByName(_username);
        }

        /// <summary>
        /// 判断用户登录是否合法
        /// </summary>
        /// <param name="_userAccount"></param>
        /// <param name="_password"></param>
        /// <returns></returns>
        private ValidationResult ValidateUserByUserAccount(Model.Customer _userAccount, string _password)
        {
            if (_userAccount == null || !_userAccount.cPassWord.Equals(_password))
            {
                return ValidationResult.InvalidUserNameOrPassword;
            }
            else
            {
                if (_userAccount.IsDisabled == 1)
                {
                    return ValidationResult.LockedOut;
                }
            }
            return ValidationResult.Success;
        }

        /// <summary>
        /// 登出操作
        /// </summary>
        public void Logout()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            {
                context.Session[sessionKey] = null;
                context.Response.Cookies[idCookieKey].Value = "";
                context.Response.Cookies[idCookieKey].Expires = DateTime.Now.AddDays(-1);//丢弃这个现已过期的Cookie
#if !DEBUG
                  context.Response.Cookies[idCookieKey].Domain = defaultDomain;
#endif

            }
        }

        /// <summary>
        /// 检验是否登录,如果已登录，把用户信息放到session里。
        /// </summary>
        public bool CheckLogin()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            bool isLogin = true;

            if (context.Request.Cookies[idCookieKey] == null || context.Request.Cookies[idCookieKey].Value == "")
            {
                isLogin = false;
            }
            else
            {
                int uid = Int32.Parse(DESEncryptor.Decrypt(context.Request.Cookies[idCookieKey].Value));
                Model.Customer cusInfo = new Monitor().GetCustomerInfoByUserId(uid);
                context.Session[sessionKey] = cusInfo;
            }
            return isLogin;
        }
        #endregion

        #region private 方法
        /// <summary>
        /// 把用户信息存到cookie和session中
        /// </summary>
        /// <param name="_customer"></param>
        /// <param name="_isPersistent"></param>
        /// <param name="_expiresTime"></param>
        private void passPort(Model.Customer _customer, bool _isPersistent, DateTime? _expiresTime)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            context.Session[sessionKey] = _customer;
            HttpCookie newcookie = new HttpCookie(idCookieKey);
            newcookie.Value = DESEncryptor.Encrypt(_customer.CusId.ToString());
#if !DEBUG 
            newcookie.Domain = defaultDomain;
#endif
            context.Response.AppendCookie(newcookie);
        }

        #endregion

    }
}