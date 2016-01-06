using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AutoRadio.RadioSmart.Common;
using System.IO;
using System.Configuration;
using Dal;
using Outdoor._code;

namespace Outdoor.ashx
{
    /// <summary>
    /// UploadExcelFile 的摘要说明
    /// </summary>
    public class UploadExcelFile : IHttpHandler, IRequiresSessionState
    {

        protected static UserManager userManager = new UserManager();
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

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int cusid = ConvertHelper.GetInteger(context.Request.Form["cusid"]);
            if (!userManager.CheckLogin())
            {
                context.Response.Write(GetJson("0", "登录信息丢失", "", ""));
                return;
            }
            Model.Customer customer = new Monitor().GetCustomerInfoByUserId(cusid);
            if (customer == null)
            {
                context.Response.Write(GetJson("0", "客户不存在,操作失败", "", ""));
                return;
            }
            string rootExcel = ConfigurationManager.AppSettings["RootRadioSmartXLS"];//上传目录  
            if (string.IsNullOrEmpty(rootExcel))
            {
                context.Response.Write(GetJson("0", "目录配置错误", "", ""));
                return;
            }
            if (context.Request.Files.Count == 0)
            {
                context.Response.Write(GetJson("0", "请选择排期文件", "", ""));
                return;
            }
            HttpPostedFile myAdFile;
            string fileName, fileType, contentType;
            string parcialFile, targetFile, targetpath;
            string parcialFiles = string.Empty, fileNames = string.Empty;
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                myAdFile = context.Request.Files[i];
                if (myAdFile.ContentLength > 10 * 1024 * 1024)
                {
                    context.Response.Write(GetJson("0", "请选择小于10M的排期文件", "", ""));
                    return;
                }
                fileName = Path.GetFileName(myAdFile.FileName);
                fileType = Path.GetExtension(fileName).ToLower().Trim('.');
                contentType = myAdFile.ContentType.ToLower();
                Loger.Current.Write(fileName + "的contentType=" + contentType + "；fileType=" + fileType);
                if (!((contentType == "application/kset" || contentType == "application/octet-stream" || contentType == "application/vnd.ms-excel" || contentType == "application/ms-excel" || contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") && (fileType == "xls" || fileType == "xlsx")))//判断是否可上传的文件类型
                {
                    context.Response.Write(GetJson("0", "只能上传Excel文件", "", ""));
                    return;
                }
                parcialFile = FileHelper.GetADFileSubPath(ConvertHelper.GetLong(cusid + DateTime.Now.Ticks), fileType);//按规则生成目录格式
                targetFile = rootExcel + System.IO.Path.DirectorySeparatorChar + parcialFile;//生成上传文件的全路径
                targetpath = System.IO.Path.GetDirectoryName(targetFile);
                if (!System.IO.Directory.Exists(targetpath))
                    System.IO.Directory.CreateDirectory(targetpath);
                try
                {
                    myAdFile.SaveAs(targetFile);
                }
                catch
                {
                    context.Response.Write(GetJson("0", "上传出错，请重新上传", "", ""));
                    return;
                }
                parcialFiles += parcialFile + "|";
                fileNames += fileName + "|";
            }
            context.Response.Write(GetJson("1", cusid.ToString(), parcialFiles, fileNames));
        }

        public string GetJson(string status, string note, string filePath, string fileName)
        {
            string json = "status:'{0}',note:'{1}',filePath:'{2}',fileName:'{3}'";
            json = string.Format(json, status, note, filePath, fileName);
            json = "{" + json + "}";
            return json;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}