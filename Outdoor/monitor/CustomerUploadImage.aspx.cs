using AutoRadio.RadioSmart.Common;
using Dal;
using Outdoor._code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outdoor.monitor
{
    public partial class CustomerUploadImage : BasePage
    {
        Monitor monitor = new Monitor();
        private int tpId;
        private int tid;
        private string type, name;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            tpId = ConvertHelper.GetInteger(Request["tpid"]);
            type = ConvertHelper.GetString(Request["type"]);
            name = ConvertHelper.GetString(Request["name"]);
            tid = monitor.GetTidByTpId(tpId);
            jianbo_UserMenu.MenuType = 5;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (tpId == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>effect.Dialog.alert('错误操作！');</script>");
                return;
            }
            int userId = monitor.GetUserIdByCusId(base.Profile.CusId);
            //if (base.Profile.CustomerType == 2 && userId == 0)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>effect.Dialog.alert('该账号未绑定微信！');</script>");
            //    return;
            //}
            string imagePath = hfImagePath.Value;
            if (imagePath == "," || string.IsNullOrEmpty(imagePath))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>effect.Dialog.alert('请上传图片文件！');</script>");
                return;
            }
            string rootImage = ConfigurationManager.AppSettings["RootRadioSmartImage"];//上传目录  
            string[] imagePaths = hfImagePath.Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            string thumbnailImgPaths = string.Empty, thumbnailImgPath = string.Empty, ShootPosition = string.Empty;
            DateTime ShootDate = new DateTime();
            DataTable dt;
            if (userId > 0)
            {
                dt = monitor.GetTaskProjectUserRelation(tpId, userId, Profile.CusId);
            }
            else
            {
                dt = monitor.GetTaskProjectUserRelation(tpId);
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < imagePaths.Length; i++)
                {
                    Exif exif = new Exif(rootImage + System.IO.Path.DirectorySeparatorChar + imagePaths[i]);
                    string[] dtParts = exif.DateTime.Split(new string[2] { " ", ":" },
                        StringSplitOptions.RemoveEmptyEntries);
                    if (dtParts.Length > 0)
                    {
                        if (Convert.ToInt32(dtParts[0]) > 0)
                        {
                            //有拍照日期
                            ShootDate = new DateTime(Convert.ToInt32(dtParts[0]), Convert.ToInt32(dtParts[1]), Convert.ToInt32(dtParts[2]),
                                Convert.ToInt32(dtParts[3]), Convert.ToInt32(dtParts[4]), Convert.ToInt32(dtParts[5]), DateTimeKind.Local);
                        }
                    }
                    if (!string.IsNullOrEmpty(exif.GPSLongitude) && !string.IsNullOrEmpty(exif.GPSLatitude))
                    {
                        ShootPosition = ConvertDegreesToDigital(exif.GPSLongitude) + "," + ConvertDegreesToDigital(exif.GPSLatitude);
                    }
                    var arrPath = imagePaths[i].Split('.');
                    if (arrPath.Length > 1)
                    {
                        thumbnailImgPath = arrPath[0] + "s." + arrPath[1];
                        string exportImgPath = monitor.GetIdByTPUId(ConvertHelper.GetInteger(dt.Rows[0]["TPUId"])) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + imagePaths[i].Substring(imagePaths[i].LastIndexOf('.'));
                        monitor.AddImageDetail(ConvertHelper.GetInteger(dt.Rows[0]["TPUId"]), imagePaths[i], thumbnailImgPath, i + 1, exportImgPath);
                        thumbnailImgPaths += thumbnailImgPath + "|";
                    }
                }
                imagePath = imagePath.Trim('|');
                thumbnailImgPaths = thumbnailImgPaths.Trim('|');
                if (userId == 0)
                {
                    userId = ConvertHelper.GetInteger(dt.Rows[0]["UserId"]);
                }
                imagePath += "|" + dt.Rows[0]["ImgPath"];
                thumbnailImgPaths += "|" + dt.Rows[0]["ThumbnailImgPath"];
                if (ShootDate.Year == 1)
                {
                    ShootDate = ConvertHelper.GetDateTime(dt.Rows[0]["ShootTime"]).Year == 1 ? DateTime.Now : ConvertHelper.GetDateTime(dt.Rows[0]["ShootTime"]);
                }
                if (string.IsNullOrEmpty(ShootPosition))
                {
                    ShootPosition = dt.Rows[0]["ShootPosition"].ToString();
                }
                monitor.SaveImagePath(tpId, userId,Profile.CusId, imagePath.Trim('|'), thumbnailImgPaths.Trim('|'), ShootDate, ShootPosition);
                monitor.UpdateAbnormalType(tpId, ConvertHelper.GetInteger(hfAbnormalType.Value));
            }
            if (Profile.CustomerType == 1)
            {
                Response.Redirect("ViewPlan.aspx?tid=" + tid);
            }
            Response.Redirect(string.Format("MyTaskList.aspx?tid={0}&type={1}&name={2}", tid, type, name));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Profile.CustomerType == 1)
            {
                Response.Redirect("ViewPlan.aspx?tid=" + tid);
            }
            Response.Redirect(string.Format("MyTaskList.aspx?tid={0}&type={1}&name={2}", tid, type, name));
        }
    }
}