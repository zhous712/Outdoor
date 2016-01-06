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
    /// UploadImageFile 的摘要说明
    /// </summary>
    public class UploadImageFile : IHttpHandler, IRequiresSessionState
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
                context.Response.Write(GetJson("0", "登录信息丢失", "", "", ""));
                return;
            }
            Model.Customer customer = new Monitor().GetCustomerInfoByUserId(cusid);
            if (customer == null)
            {
                context.Response.Write(GetJson("0", "客户不存在,操作失败", "", "", ""));
                return;
            }
            string rootImage = ConfigurationManager.AppSettings["RootRadioSmartImage"];//上传目录  
            if (string.IsNullOrEmpty(rootImage))
            {
                context.Response.Write(GetJson("0", "目录配置错误", "", "", ""));
                return;
            }
            if (context.Request.Files.Count == 0)
            {
                context.Response.Write(GetJson("0", "请选择图片文件", "", "", ""));
                return;
            }
            HttpPostedFile myAdFile = context.Request.Files[0];
            if (myAdFile.ContentLength > 10 * 1024 * 1024)
            {
                context.Response.Write(GetJson("0", "请选择小于10M的图片文件", "", "", ""));
                return;
            }
            string fileName = Path.GetFileName(myAdFile.FileName);
            string fileType = Path.GetExtension(fileName).ToLower().Trim('.');
            string contentType = myAdFile.ContentType.ToLower();
            Loger.Current.Write(fileName + "的contentType=" + contentType + "；fileType=" + fileType);
            if (!(fileType == "jpg" || fileType == "jpeg" || fileType == "png"))//判断是否可上传的文件类型
            {
                context.Response.Write(GetJson("0", "只能上传图片文件", "", "", ""));
                return;
            }


            string parcialFile = FileHelper.GetADFileSubPath(ConvertHelper.GetLong(cusid + DateTime.Now.Ticks), fileType);//按规则生成目录格式
            string targetFile = rootImage + System.IO.Path.DirectorySeparatorChar + parcialFile;//生成上传文件的全路径
            string targetpath = System.IO.Path.GetDirectoryName(targetFile);
            string newParcialFile = parcialFile.Substring(0, parcialFile.LastIndexOf('.')) + "s." + fileType;//按规则生成缩略图目录格式
            string newTargetFile = rootImage + System.IO.Path.DirectorySeparatorChar + newParcialFile;//生成上传文件缩略图的全路径
            if (!System.IO.Directory.Exists(targetpath))
                System.IO.Directory.CreateDirectory(targetpath);
            try
            {
                myAdFile.SaveAs(targetFile);
                MakeThumbnail(targetFile, newTargetFile, 240, 240);//生成缩略图
            }
            catch
            {
                context.Response.Write(GetJson("0", "上传出错，请重新上传", "", "", ""));
                return;
            }
            context.Response.Write(GetJson("1", cusid.ToString(), parcialFile, StringHelper.SubString(fileName, 20, true), fileName));
        }

        private void MakeThumbnail(string sourcePath, string newPath, int width, int height)
        {
            System.Drawing.Image ig = System.Drawing.Image.FromFile(sourcePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = ig.Width;
            int oh = ig.Height;
            if ((double)ig.Width / (double)ig.Height > (double)towidth / (double)toheight)
            {
                oh = ig.Height;
                ow = ig.Height * towidth / toheight;
                y = 0;
                x = (ig.Width - ow) / 2;

            }
            else
            {
                ow = ig.Width;
                oh = ig.Width * height / towidth;
                x = 0;
                y = (ig.Height - oh) / 2;
            }
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(ig, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            try
            {
                bitmap.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ig.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

        }

        public string GetJson(string status, string note, string filePath, string shortFileName, string fileName)
        {
            string json = "status:'{0}',note:'{1}',filePath:'{2}',shortFileName:'{3}',fileName:'{4}'";
            json = string.Format(json, status, note, filePath, shortFileName, fileName);
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