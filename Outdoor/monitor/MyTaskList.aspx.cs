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
    public partial class MyTaskList : BasePage
    {
        Monitor monitor = new Monitor();
        protected string pageStr = string.Empty;
        protected string showList = string.Empty;
        protected int Tid;
        private string commonImagePath = ConfigurationManager.AppSettings["WeiXinUploadImage"] + "/";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            showList = Request["type"];
            Tid = ConvertHelper.GetInteger(Request["tid"]);
            if (string.IsNullOrEmpty(showList))
            {
                if (monitor.GetTaskStatus(Tid) == 4)
                {
                    showList = "Overdue";
                }
                else
                {
                    showList = "In";
                }
            }
            if (!IsPostBack)
            {
                jianbo_UserMenu.MenuType = 2;
                BindPageList();
            }
        }

        #region 绑定列表
        /// <summary>
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            string txt_key = Request["name"];
            this.txt_key.Text = txt_key;
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int userId = monitor.GetUserIdByCusId(base.Profile.CusId);
            int pageSize = 20;
            int recordCount = 0;
            string relation = "0";//All
            string status = "1";
            switch (showList)
            {
                case "In":
                    relation = "0";
                    status = "1";
                    LiTypeIn.Attributes["class"] = "on";
                    break;
                case "Upload":
                    relation = "0";
                    status = "2";
                    LiTypeUpload.Attributes["class"] = "on";
                    break;
                case "AuditPass":
                    relation = "1";
                    status = "3";
                    LiTypeAuditPass.Attributes["class"] = "on";
                    break;
                case "AuditNoPass":
                    relation = "2";
                    status = "4";
                    LiTypeAuditNoPass.Attributes["class"] = "on";
                    break;
                case "Overdue":
                    relation = "3";
                    status = "5";
                    LiTypeOverdue.Attributes["class"] = "on";
                    break;
            }
            DataSet ds = new DataSet();
            string strCondition = " t.TId=" + Tid + " AND tp.TPId IN (SELECT TPId FROM dbo.TaskProjectUserRelation WHERE (UserId=" + userId + " or CusId=" + Profile.CusId + ") AND Relation = " + relation + ")";
            if (status == "1" || status == "2")
            {
                strCondition += " AND tp.Status = " + status + "";
            }
            if (!string.IsNullOrEmpty(txt_key) && txt_key != "请输入楼宇名称")
            {
                strCondition += " AND tp.BlockName LIKE '%" + txt_key + "%'";
            }
            recordCount = monitor.GetMyTaskListCount(strCondition);
            ds = monitor.GetMyTaskList(userId,Profile.CusId, strCondition, pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("MyTaskList.aspx?tid={0}&type={1}&name={2}", Tid, showList, txt_key));
        }
        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int tpid = ConvertHelper.GetInteger(drv["TpId"]);
                int tpuid = ConvertHelper.GetInteger(drv["TPUId"]);
                if (drv["RegionId"].ToString() != "0" && !string.IsNullOrEmpty(drv["RegionId"].ToString()))
                {
                    Literal litCity = (Literal)e.Item.FindControl("litCity");
                    DataTable dt = monitor.GetRegionCodeLevelList(drv["RegionId"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        litCity.Text = dt.Rows[0]["RegionName"].ToString();
                    }
                }
                Literal litStatus = (Literal)e.Item.FindControl("litStatus");
                Literal litAction = (Literal)e.Item.FindControl("litAction");
                switch (showList)
                {
                    case "In":
                        litStatus.Text = "已领取";
                        litAction.Text = "<a href=\"CustomerUploadImage.aspx?tpid=" + tpid + "&type=" + showList + "&name=" + txt_key.Text + "\" class=\"tab_a\">上传</a>";
                        break;
                    case "Upload":
                        litStatus.Text = "已上传";
                        string[] arrPath = ConvertHelper.GetString(drv["ThumbnailImgPath"]).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        litAction.Text = "<a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"ViewImage(" + tpuid + ",'" + (commonImagePath + (arrPath.Length > 0 ? arrPath[0] : drv["ThumbnailImgPath"])) + "','" + drv["ShootTime"] + "','" + drv["ShootPosition"] + "'," + drv["GpsType"] + ")\">校对</a><a href=\"CustomerUploadImage.aspx?tpid=" + tpid + "&type=" + showList + "&name=" + txt_key.Text + "\" class=\"tab_a\">上传</a><a target=\"_blank\" href=\"ViewImage.aspx?tpuid=" + tpuid + "\" class=\"tab_a\">预览</a>";
                        break;
                    case "AuditPass":
                        litStatus.Text = "已审核";
                        litAction.Text = "<a target=\"_blank\" href=\"ViewImage.aspx?tpuid=" + tpuid + "\" class=\"tab_a\">预览</a>";
                        break;
                    case "AuditNoPass":
                        litStatus.Text = drv["AuditReason"].ToString();
                        litAction.Text = "<a target=\"_blank\" href=\"ViewImage.aspx?tpuid=" + tpuid + "\" class=\"tab_a\">预览</a>";
                        break;
                    case "Overdue":
                        litStatus.Text = "已过期";
                        litAction.Text = "<a href=\"CustomerUploadImage.aspx?tpid=" + tpid + "&type=" + showList + "&name=" + txt_key.Text + "\" class=\"tab_a\">上传</a><a target=\"_blank\" href=\"ViewImage.aspx?tpuid=" + tpuid + "\" class=\"tab_a\">预览</a>";
                        break;
                }
                litAction.Text += "<a href=\"ViewTaskImage.aspx?tid=" + ConvertHelper.GetInteger(drv["TId"]) + "\" class=\"tab_a\" target=\"_blank\">样图</a>";
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            string txt_key = this.txt_key.Text.ToString();
            Response.Redirect(string.Format("MyTaskList.aspx?tid={0}&type={1}&name={2}", Tid, showList, txt_key));
        }
        #endregion
    }
}