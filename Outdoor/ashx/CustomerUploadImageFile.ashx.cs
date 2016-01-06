using AutoRadio.RadioSmart.Common;
using Dal;
using Outdoor._code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Outdoor.ashx
{
    /// <summary>
    /// CustomerUploadImageFile 的摘要说明
    /// </summary>
    public class CustomerUploadImageFile : IHttpHandler, IRequiresSessionState
    {
        protected static UserManager userManager = new UserManager();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int cusid = ConvertHelper.GetInteger(context.Request.Form["cusid"]);
            string uploadType = ConvertHelper.GetString(context.Request.Form["uploadtype"]);
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
            string rootImage = string.IsNullOrEmpty(uploadType) ? ConfigurationManager.AppSettings["RootRadioSmartImage"] : ConfigurationManager.AppSettings["RootRadioSmartTaskImage"];//上传目录  
            if (string.IsNullOrEmpty(rootImage))
            {
                context.Response.Write(GetJson("0", "目录配置错误", "", ""));
                return;
            }
            if (context.Request.Files.Count == 0)
            {
                context.Response.Write(GetJson("0", "请选择图片文件", "", ""));
                return;
            }
            HttpPostedFile myAdFile;
            string fileName, fileType, contentType;
            string parcialFile, targetFile, targetpath, newParcialFile, newTargetFile;
            string parcialFiles = string.Empty, fileNames = string.Empty;
            for (int i = 0; i < context.Request.Files.Count; i++)
            {
                myAdFile = context.Request.Files[i];
                if (myAdFile.ContentLength > 10 * 1024 * 1024)
                {
                    context.Response.Write(GetJson("0", "请选择小于10M的图片文件", "", ""));
                    return;
                }
                fileName = Path.GetFileName(myAdFile.FileName);
                fileType = Path.GetExtension(fileName).ToLower().Trim('.');
                contentType = myAdFile.ContentType.ToLower();
                Loger.Current.Write(fileName + "的contentType=" + contentType + "；fileType=" + fileType);
                if (!(fileType == "jpg" || fileType == "jpeg" || fileType == "png"))//判断是否可上传的文件类型
                {
                    context.Response.Write(GetJson("0", "只能上传图片文件", "", ""));
                    return;
                }
                parcialFile = FileHelper.GetADFileSubPath(ConvertHelper.GetLong(cusid + DateTime.Now.Ticks), fileType);//按规则生成目录格式
                targetFile = rootImage + System.IO.Path.DirectorySeparatorChar + parcialFile;//生成上传文件的全路径
                targetpath = System.IO.Path.GetDirectoryName(targetFile);
                newParcialFile = parcialFile.Substring(0, parcialFile.LastIndexOf('.')) + "s." + fileType;//按规则生成缩略图目录格式
                newTargetFile = rootImage + System.IO.Path.DirectorySeparatorChar + newParcialFile;//生成上传文件缩略图的全路径
                if (!System.IO.Directory.Exists(targetpath))
                    System.IO.Directory.CreateDirectory(targetpath);
                try
                {
                    myAdFile.SaveAs(targetFile);
                    MakeThumbnail(targetFile, newTargetFile, 240, 240);//生成缩略图
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