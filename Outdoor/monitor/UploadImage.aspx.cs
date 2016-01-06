using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;
using Dal;
using AutoRadio.RadioSmart.Common;
using System.Configuration;

namespace Outdoor.monitor
{
    public partial class UploadImage : BasePage
    {
        Monitor monitor = new Monitor();
        private int tpuId;
        private string path;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            tpuId = ConvertHelper.GetInteger(Request["tpuid"]);
            path = ConvertHelper.GetString(Request["path"]);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (tpuId > 0 && !string.IsNullOrEmpty(path))
            {
                string imagePath = monitor.GetImgPathByTPUId(tpuId);
                string[] arrImgPath = imagePath.Split('|');
                string thumbnailImgPath = string.Empty;
                for (int i = 0; i < arrImgPath.Length; i++)
                {
                    var arrPath = arrImgPath[i].Split('.');
                    if (arrPath.Length > 1)
                    {
                        thumbnailImgPath += (arrPath[0] + "s." + arrPath[1] + "|");
                    }
                }
                if (hfImagePath.Value == "," || string.IsNullOrEmpty(hfImagePath.Value))
                {
                    imagePath = imagePath.Replace(path + "|", "").Replace(path, "");
                    thumbnailImgPath = thumbnailImgPath.Replace(path.Substring(0, path.LastIndexOf('.')) + "s" + path.Substring(path.LastIndexOf('.')) + "|", "").Replace(path.Substring(0, path.LastIndexOf('.')) + "s" + path.Substring(path.LastIndexOf('.')), "");
                    monitor.DeleteImageDetail(path);
                }
                else
                {
                    imagePath = imagePath.Replace(path, path + "|" + hfImagePath.Value);
                    thumbnailImgPath = thumbnailImgPath.Replace(path.Substring(0, path.LastIndexOf('.')) + "s" + path.Substring(path.LastIndexOf('.')), path.Substring(0, path.LastIndexOf('.')) + "s" + path.Substring(path.LastIndexOf('.')) + "|" + (hfImagePath.Value.Substring(0, path.LastIndexOf('.')) + "s" + hfImagePath.Value.Substring(path.LastIndexOf('.'))));
                    string rootImage = ConfigurationManager.AppSettings["RootRadioSmartImage"] + System.IO.Path.DirectorySeparatorChar + hfImagePath.Value;//上传目录  
                    Exif exif = new Exif(rootImage);
                    string ShootPosition = ConvertDegreesToDigital(exif.GPSLongitude) + "," + ConvertDegreesToDigital(exif.GPSLatitude);
                    if (!string.IsNullOrEmpty(exif.GPSLongitude) && !string.IsNullOrEmpty(exif.GPSLatitude))
                    {
                        monitor.UpdateShootPosition(ShootPosition, tpuId);
                    }
                    string exportImgPath = monitor.GetIdByTPUId(tpuId) + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + hfImagePath.Value.Substring(hfImagePath.Value.LastIndexOf('.'));
                    monitor.AddImageDetail(tpuId, hfImagePath.Value, hfImagePath.Value.Substring(0, hfImagePath.Value.LastIndexOf('.')) + "s" + hfImagePath.Value.Substring(hfImagePath.Value.LastIndexOf('.')), arrImgPath.Length, exportImgPath);
                }
                monitor.UpdateImagePath(imagePath.Trim('|'), thumbnailImgPath.Trim('|'), tpuId);
                Response.Redirect("ViewImage.aspx?tpuid=" + tpuId);
            }
            else
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('操作出错！')");
                Response.Redirect("ViewImage.aspx?tpuid=" + tpuId);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewImage.aspx?tpuid=" + tpuId);
        }
    }
}