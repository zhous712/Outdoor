using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outdoor._code;
using Dal;
using System.Configuration;
using AutoRadio.RadioSmart.Common;
using System.Data;
using System.Web.UI.HtmlControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.IO;
using AutoRadio.RadioSmart.Common.Pdf;

namespace Outdoor.monitor
{
    public partial class ViewPlan : BasePage
    {
        protected string showList = string.Empty;
        protected string pageStr = string.Empty;
        protected string TaskName = string.Empty;
        private string commonImagePath = ConfigurationManager.AppSettings["WeiXinUploadImage"] + "/";
        protected int Tid;
        protected int SId;
        Monitor monitor = new Monitor();
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            showList = Request["type"];
            Tid = ConvertHelper.GetInteger(Request["tid"]);
            SId = ConvertHelper.GetInteger(Request["sid"]);
            TaskName = monitor.GetTaskName(Tid);
            if (string.IsNullOrEmpty(showList))
            {
                showList = "All";
            }
            if (!IsPostBack)
            {
                Big(showList);
                BindPageList();
            }
            if (GetReferer(true).IndexOf("lexus") > 0)
            {
                jianbo_Footer.Visible = false;
            }
            else
            {
                jianbo_Footer.Visible = true;
            }
        }

        #region 基本参数
        void Big(string _showList)
        {
            switch (_showList)
            {
                case "Normal":
                    LiTypeNormal.Attributes["class"] = "on";
                    this.Page.Title = "正常的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Black":
                    LiTypeBlack.Attributes["class"] = "on";
                    this.Page.Title = "黑屏的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Blur":
                    LiTypeBlur.Attributes["class"] = "on";
                    this.Page.Title = "花屏的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "NotPaint":
                    LiTypeNotPaint.Attributes["class"] = "on";
                    this.Page.Title = "未上画的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Break":
                    LiTypeBreak.Attributes["class"] = "on";
                    this.Page.Title = "损坏排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Repair":
                    LiTypeRepair.Attributes["class"] = "on";
                    this.Page.Title = "电梯维修-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Hidden":
                    LiTypeHidden.Attributes["class"] = "on";
                    this.Page.Title = "遮挡的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Abnormal":
                    LiTypeAbnormal.Attributes["class"] = "on";
                    this.Page.Title = "异常的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Overdue":
                    LiTypeOverdue.Attributes["class"] = "on";
                    this.Page.Title = "已过期的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Audit":
                    LiTypeAudit.Attributes["class"] = "on";
                    this.Page.Title = "审核通过的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Upload":
                    LiTypeUpload.Attributes["class"] = "on";
                    this.Page.Title = "已上传的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Receive":
                    LiTypeReceive.Attributes["class"] = "on";
                    this.Page.Title = "已领取的排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "Draft":
                    LiTypeDraft.Attributes["class"] = "on";
                    this.Page.Title = "草稿排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
                case "All":
                    LiTypeAll.Attributes["class"] = "on";
                    this.Page.Title = "全部排期-我的任务- RadioBuy广告监播 广告监测 电台刊例 广告刊例";
                    break;
            }
            if (base.Profile.CustomerType == 0)
            {
                LiTypeDraft.Visible = false;
                LiTypeReceive.Visible = false;
                LiTypeUpload.Visible = false;
                LiTypeAudit.Visible = false;
                LiTypeOverdue.Visible = false;
                YesDivListTop.Visible = true;
                radioCity.Visible = false;
                radioBlock.Visible = false;
                radioPoint.Visible = false;
                radioAdProduct.Visible = false;
                chkIsImage.Visible = false;
                btnExportData.Visible = false;
            }
            if (base.Profile.CustomerType == 1)
            {
                tdPrice.Visible = true;
                tdIsPay.Visible = true;
                tdReceiptor.Visible = true;
                DivCheckAll.Visible = true;
                tdCheckBox.Visible = true;
                YesDivListTop.Visible = true;
            }
        }
        #endregion
        #region 绑定列表
        /// <summary>
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            string txt_key = Request["brand"];
            this.txt_key.Text = txt_key;
            string txtMediaType = Request["mediatype"];
            this.txtMediaType.Text = txtMediaType;
            this.txtCity.Text = Request["city"];
            this.txtArea.Text = Request["area"];
            inputDate20.Value = Request["date"];
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 20;
            int recordCount = 0;
            DataSet ds = new DataSet();
            recordCount = monitor.GetTaskPlanListCount(GetCondition());
            ds = monitor.GetTaskPlanList(GetCondition(), pageIndex, pageSize);
            this.rpt.DataSource = ds;
            this.rpt.DataBind();
            pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("ViewPlan.aspx?tid={0}&type={1}&brand={2}&mediatype={3}&city={4}&date={5}&area={6}&sid={7}", Tid, showList, txt_key, txtMediaType, txtCity.Text, inputDate20.Value, txtArea.Text, SId));
        }

        private string GetCondition(string src = "select")
        {
            string taskStatus = "0,1,2,3,4";//All
            string abnormalType = string.Empty;
            switch (showList)
            {
                case "Normal":
                    taskStatus = "3";
                    abnormalType = "0,5";
                    break;
                case "Black":
                    taskStatus = "3";
                    abnormalType = "1";
                    break;
                case "Blur":
                    taskStatus = "3";
                    abnormalType = "2";
                    break;
                case "NotPaint":
                    taskStatus = "3";
                    abnormalType = "3";
                    break;
                case "Break":
                    taskStatus = "3";
                    abnormalType = "4";
                    break;
                case "Repair":
                    taskStatus = "3";
                    abnormalType = "5";
                    break;
                case "Hidden":
                    taskStatus = "3";
                    abnormalType = "6";
                    break;
                case "Abnormal":
                    taskStatus = "3";
                    abnormalType = "7";
                    break;
                case "Overdue":
                    taskStatus = "4";
                    break;
                case "Audit":
                    taskStatus = "3";
                    break;
                case "Upload":
                    taskStatus = "2";
                    break;
                case "Receive":
                    taskStatus = "1";
                    break;
                case "Draft":
                    taskStatus = "0";
                    break;
                case "All":
                    taskStatus = "0,1,2,3,4";
                    break;
            }
            if (base.Profile.CustomerType == 0)
            {
                taskStatus = "3";
            }
            if (src == "export")
            {
                taskStatus = "0,1,2,3,4";//All;
                abnormalType = "";
            }
            string strCondition = " 1=1 AND tp.EndDate>'2015-10-07'";
            if (base.Profile.CustomerType == 1 && !string.IsNullOrEmpty(txtMediaType.Text) && txtMediaType.Text != "请输入媒体类型")
            {
                strCondition += string.Format(" AND tp.MediaType like '%{0}%' ", StringHelper.SqlFilter(txtMediaType.Text));
            }
            else if (SId > 0 && Tid == 0)
            {
                strCondition += " AND tp.TId IN (SELECT TId FROM dbo.Task WHERE SId=" + SId + ")";
            }
            else if (Tid > 0)
            {
                strCondition += " AND tp.TId=" + Tid;
            }
            if (!string.IsNullOrEmpty(txtCity.Text) && txtCity.Text != "请输入城市")
            {
                strCondition += string.Format(" AND tp.RegionId=(SELECT RegionId FROM dbo.Region WHERE RegionName='{0}') ", txtCity.Text);
            }
            if (!string.IsNullOrEmpty(txtArea.Text) && txtArea.Text != "请输入区域")
            {
                strCondition += string.Format(" AND tp.AreaName like '%{0}%' ", StringHelper.SqlFilter(txtArea.Text));
            }
            if (!string.IsNullOrEmpty(txt_key.Text) && txt_key.Text != "请输入编号或楼宇名称")
            {
                int sTpid = ConvertHelper.GetInteger(txt_key.Text);
                if (sTpid != 0)
                {
                    strCondition += string.Format(" AND tp.TPId = {0} ", sTpid);
                }
                else
                {
                    strCondition += string.Format(" AND tp.BlockName like '%{0}%' ", StringHelper.SqlFilter(txt_key.Text));
                }
            }
            if (!string.IsNullOrEmpty(taskStatus))
            {
                strCondition += string.Format(" AND tp.Status in ({0}) ", taskStatus);
            }
            if (!string.IsNullOrEmpty(abnormalType))
            {
                strCondition += string.Format(" AND tp.AbnormalType IN ({0}) ", abnormalType);
            }
            if (!string.IsNullOrEmpty(inputDate20.Value))
            {
                string[] dateArr = inputDate20.Value.Split('-');
                if (dateArr.Length > 1)
                {
                    strCondition += string.Format(" AND tp.BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    strCondition += string.Format(" AND tp.EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[1]).ToString("yyyy-MM-dd"));
                }
                else
                {
                    strCondition += string.Format(" AND tp.BeginDate>='{0} 00:00:00'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                    strCondition += string.Format(" AND tp.EndDate<='{0} 23:59:59'", ConvertHelper.GetDateTime(dateArr[0]).ToString("yyyy-MM-dd"));
                }
            }
            return strCondition;
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int tpid = ConvertHelper.GetInteger(drv["Tpid"]);
                var tdPrice = e.Item.FindControl("tdPrice") as HtmlControl;
                var tdIsPay = e.Item.FindControl("tdIsPay") as HtmlControl;
                var tdReceiptor = e.Item.FindControl("tdReceiptor") as HtmlControl;
                var tdCheckBox = e.Item.FindControl("tdCheckBox") as HtmlControl;
                Literal litCheckBox = (Literal)e.Item.FindControl("litCheckBox");
                if (ConvertHelper.GetInteger(drv["Status"]) == 0)
                {
                    litCheckBox.Text = " <input id=\"chk_" + tpid + "\" type=\"checkbox\" name=\"chkTaskPlan\" value=\"" + tpid + "\" onclick=\"changeCheckAll()\" />";
                }
                Literal litAction = (Literal)e.Item.FindControl("litAction");
                string[] arrPath = ConvertHelper.GetString(drv["ThumbnailImgPath"]).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (base.Profile.CustomerType == 1)
                {
                    tdPrice.Visible = true;
                    tdIsPay.Visible = true;
                    tdReceiptor.Visible = true;
                    tdCheckBox.Visible = true;
                    if (ConvertHelper.GetInteger(drv["Status"]) == 1)
                    {
                        litAction.Text = "<a href=\"CustomerUploadImage.aspx?tpid=" + tpid + "\" class=\"tab_a\">上传</a>";
                    }
                    else if (ConvertHelper.GetInteger(drv["Status"]) == 2)
                    {
                        litAction.Text = "<a href=\"CustomerUploadImage.aspx?tpid=" + tpid + "\" class=\"tab_a\">上传</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"AuditTask(" + tpid + "," + ConvertHelper.GetInteger(drv["TPUId"]) + ",'" + drv["BlockName"] + "','" + drv["AbnormalType"] + "','" + (commonImagePath + (arrPath.Length > 0 ? arrPath[0] : drv["ThumbnailImgPath"])) + "','" + drv["ShootTime"] + "','" + drv["ShootPosition"] + "'," + drv["GpsType"] + "," + arrPath.Length + ")\">审核</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"UpdateGps(" + tpid + "," + ConvertHelper.GetInteger(drv["TPUId"]) + ")\">坐标</a>";
                    }
                    else if (ConvertHelper.GetInteger(drv["Status"]) == 3)
                    {
                        litAction.Text = "<a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"ViewImage(" + ConvertHelper.GetInteger(drv["TPUId"]) + ",'" + (commonImagePath + (arrPath.Length > 0 ? arrPath[0] : drv["ThumbnailImgPath"])) + "','" + drv["ShootTime"] + "','" + drv["ShootPosition"] + "'," + drv["GpsType"] + "," + arrPath.Length + ")\">查看</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"UpdateGps(" + tpid + "," + ConvertHelper.GetInteger(drv["TPUId"]) + ")\">坐标</a><a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"ReDo(" + tpid + ")\">重做</a>";
                    }
                    litAction.Text += "<a href=\"UpdateTaskProject.aspx?tpid=" + tpid + "&tid=" + Tid + "&type=" + showList + "&brand=" + txt_key.Text + "&mediatype=" + txtMediaType.Text + "&city=" + txtCity.Text + "&date=" + inputDate20.Value + "\" class=\"tab_a\">编辑</a>";
                }
                else
                {
                    tdPrice.Visible = false;
                    tdIsPay.Visible = false;
                    tdReceiptor.Visible = false;
                    tdCheckBox.Visible = false;
                    if (ConvertHelper.GetInteger(drv["Status"]) == 3)
                    {
                        DataTable dt = monitor.GetImageDetailList(ConvertHelper.GetInteger(drv["TPUId"]));
                        string imgPath = string.Empty;
                        if (dt.Rows.Count > 0)
                        {
                            imgPath = dt.Rows[0]["ImgPath"].ToString();
                            imgPath = imgPath.Substring(0, imgPath.LastIndexOf('.')) + "s" + imgPath.Substring(imgPath.LastIndexOf('.'));
                        }
                        litAction.Text = "<a href=\"javascript:void(0)\" class=\"tab_a\" onclick=\"ViewImage(" + ConvertHelper.GetInteger(drv["TPUId"]) + ",'" + (commonImagePath + imgPath) + "','" + drv["ShootTime"] + "','" + drv["ShootPosition"] + "'," + drv["GpsType"] + "," + (ConvertHelper.GetInteger(drv["AbnormalType"]) == 0 ? 1 : arrPath.Length) + ")\">查看</a>";
                    }
                }
                Literal litPlanCycle = (Literal)e.Item.FindControl("litPlanCycle");
                litPlanCycle.Text = ConvertHelper.GetDateTimeString(drv["BeginDate"], "yyyy.MM.dd") + "-" + ConvertHelper.GetDateTimeString(drv["EndDate"], "yyyy.MM.dd");
            }
        }
        #endregion

        #region 搜索
        protected void bntSearch_Click(object sender, EventArgs e)
        {
            string txt_key = this.txt_key.Text.ToString();
            Response.Redirect(string.Format("ViewPlan.aspx?tid={0}&type={1}&brand={2}&mediatype={3}&city={4}&date={5}&area={6}&sid={7}", Tid, showList, txt_key, txtMediaType.Text, txtCity.Text, inputDate20.Value, txtArea.Text, SId));
        }
        #endregion

        #region 获取排期状态
        protected string GetTaskPlanStatus(string status, string abnormalType, string type = "select")
        {
            string str = "";
            switch (status)
            {
                case "0": str = (base.Profile.CustomerType == 1 && type == "select") ? "草稿" : "--"; break;
                case "1": str = (base.Profile.CustomerType == 1 && type == "select") ? "已领取" : "--"; break;
                case "2": str = (base.Profile.CustomerType == 1 && type == "select") ? "已上传(" + GetAbnormalType(abnormalType, type) + ")" : "--"; break;
                case "3":
                    str = GetAbnormalType(abnormalType, type);
                    break;
                case "4": str = (base.Profile.CustomerType == 1 && type == "select") ? "已过期" : "--"; break;
            }
            return str;
        }

        private string GetAbnormalType(string abnormalType, string type)
        {
            string str = string.Empty;
            if (abnormalType == "0")
            {
                str = "正常";
            }
            else if (abnormalType == "1")
            {
                str = "黑屏";
            }
            else if (abnormalType == "2")
            {
                str = "花屏";
            }
            else if (abnormalType == "3")
            {
                str = "未上画";
            }
            else if (abnormalType == "4")
            {
                str = "破损";
            }
            else if (abnormalType == "5")
            {
                str = (type == "export" ? "正常" : "电梯维修");
            }
            else if (abnormalType == "6")
            {
                str = "遮挡";
            }
            else if (abnormalType == "7")
            {
                str = "异常";
            }
            return str;
        }
        #endregion

        #region 导出数据
        private HSSFWorkbook hssfworkbook = null;
        private ISheet sheet = null;
        private IRow row = null;
        private ICell cell = null;
        private ICellStyle style = null;
        private IFont font = null;
        string rootImage = ConfigurationManager.AppSettings["RootRadioSmartImage"];//上传目录
        string rootPackImage = ConfigurationManager.AppSettings["RootPackImage"];//打包目录
        #region 下载报表
        protected void btnExportData_Click(object sender, EventArgs e)
        {
            string radioRule = string.Empty;
            var dt = monitor.GetTaskPlanList(GetCondition(), 1, 99999).Tables[0];
            if (dt.Rows.Count == 0)
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('没有记录！')");
                return;
            }
            if (dt.Rows.Count > 65536)
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('excel行数不能超过65536行！')");
                return;
            }
            hssfworkbook = new HSSFWorkbook();
            string mediaType = dt.Rows[0]["MediaType"].ToString();
            string adProductName = dt.Rows[0]["AdProductName"].ToString();
            string scheduleName = monitor.GetScheduleName(SId);
            DateTime planBegin = ConvertHelper.GetDateTime(dt.Rows[0]["BeginDate"]);
            DateTime planEnd = ConvertHelper.GetDateTime(dt.Rows[0]["EndDate"]);
            int rowNum;
            var filename = string.IsNullOrEmpty(scheduleName) ? (adProductName + mediaType + "监测" + DateTime.Now.ToString("yyyyMMddHHmmss")) : scheduleName;
            sheet = hssfworkbook.CreateSheet(adProductName + mediaType);
            style = TitleStyle;
            row = sheet.CreateRow(0);
            row.Height = 30 * 20;

            if (mediaType.Contains("4S") || mediaType.Contains("4s"))
            {
                #region  设置列宽度
                sheet.SetColumnWidth(0, 10 * 256);
                sheet.SetColumnWidth(1, 20 * 256);
                sheet.SetColumnWidth(2, 20 * 256);
                sheet.SetColumnWidth(3, 40 * 256);
                sheet.SetColumnWidth(4, 40 * 256);
                sheet.SetColumnWidth(5, 20 * 256);
                sheet.SetColumnWidth(6, 30 * 256);
                sheet.SetColumnWidth(7, 30 * 256);
                sheet.SetColumnWidth(8, 30 * 256);
                sheet.SetColumnWidth(9, 30 * 256);
                sheet.SetColumnWidth(10, 30 * 256);
                sheet.SetColumnWidth(11, 30 * 256);
                sheet.SetColumnWidth(12, 30 * 256);
                sheet.SetColumnWidth(13, 30 * 256);
                sheet.SetColumnWidth(14, 40 * 256);
                #endregion

                #region 4s店导出报告（带图片）
                style = RowStyle;
                row = sheet.CreateRow(0);
                row.Height = 25 * 20;
                cell = row.CreateCell(0);
                cell.CellStyle = style;
                cell.SetCellValue("序号");
                cell = row.CreateCell(1);
                cell.CellStyle = style;
                cell.SetCellValue("品牌");
                cell = row.CreateCell(2);
                cell.CellStyle = style;
                cell.SetCellValue("经销商/售后代码");
                cell = row.CreateCell(3);
                cell.CellStyle = style;
                cell.SetCellValue("经销商/售后名称");
                cell = row.CreateCell(4);
                cell.CellStyle = style;
                cell.SetCellValue("经销商/售后地址");
                cell = row.CreateCell(5);
                cell.CellStyle = style;
                cell.SetCellValue("监测结果");
                cell = row.CreateCell(6);
                cell.CellStyle = style;
                cell.SetCellValue("4S店门头照");
                cell = row.CreateCell(7);
                cell.CellStyle = style;
                cell.SetCellValue("4S店WiFi");
                cell = row.CreateCell(8);
                cell.CellStyle = style;
                cell.SetCellValue("输入手机号码");
                cell = row.CreateCell(9);
                cell.CellStyle = style;
                cell.SetCellValue("验证码短信");
                cell = row.CreateCell(10);
                cell.CellStyle = style;
                cell.SetCellValue("4S店网站首页");
                cell = row.CreateCell(11);
                cell.CellStyle = style;
                cell.SetCellValue("汽车之家网站");
                cell = row.CreateCell(12);
                cell.CellStyle = style;
                cell.SetCellValue("车享网");
                cell = row.CreateCell(13);
                cell.CellStyle = style;
                cell.SetCellValue("搜狐网");
                cell = row.CreateCell(14);
                cell.CellStyle = style;
                cell.SetCellValue("备注");
                rowNum = 1;
                foreach (DataRow item in dt.Rows)
                {
                    if (radioCity.Checked)
                    {
                        radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                    }
                    else if (radioBlock.Checked)
                    {
                        radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["BlockName"].ToString()) ? "无楼宇" : item["BlockName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                    }
                    else if (radioPoint.Checked)
                    {
                        radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["BlockName"].ToString()) ? "无楼宇" : item["BlockName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["PointName"].ToString()) ? "无点位" : item["PointName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                    }
                    else if (radioAdProduct.Checked)
                    {
                        radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["PointName"].ToString()) ? "无点位" : item["PointName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["AdProductName"].ToString()) ? "无广告" : item["AdProductName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                    }
                    if (!Directory.Exists(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + radioRule))
                    {
                        Directory.CreateDirectory(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + radioRule);
                    }
                    string photo1 = "无", photo2 = "无", photo3 = "无", photo4 = "无", photo5 = "无", photo6 = "无", photo7 = "无", photo8 = "无";
                    DataTable dtImage = monitor.GetImageDetailList(ConvertHelper.GetInteger(item["TPUId"]));
                    foreach (DataRow dr in dtImage.Rows)
                    {
                        switch (ConvertHelper.GetInteger(dr["Sort"]))
                        {
                            case 1: photo1 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 2: photo2 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 3: photo3 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 4: photo4 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 5: photo5 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 6: photo6 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 7: photo7 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            case 8: photo8 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                        }
                        if (!string.IsNullOrEmpty(dr["ImgPath"].ToString()))
                        {
                            string sourceFileName = rootImage + System.IO.Path.DirectorySeparatorChar + dr["ImgPath"].ToString();
                            string destFileName = rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + radioRule + dr["ImgPath"].ToString().Substring(dr["ImgPath"].ToString().LastIndexOf('/'));
                            if (File.Exists(sourceFileName))
                            {
                                File.Copy(sourceFileName, destFileName, true);
                            }
                        }
                    }
                    row = sheet.CreateRow(rowNum);
                    row.Height = 20 * 20;
                    cell = row.CreateCell(0);
                    cell.CellStyle = style;
                    cell.SetCellValue(rowNum);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style;
                    cell.SetCellValue(ConvertHelper.GetString(item["AdProductName"]));
                    cell = row.CreateCell(2);
                    cell.CellStyle = style;
                    cell.SetCellValue(ConvertHelper.GetString(item["SpareOne"]));
                    cell = row.CreateCell(3);
                    cell.CellStyle = style;
                    cell.CellStyle.ShrinkToFit = true;
                    cell.SetCellValue(ConvertHelper.GetString(item["SpareTwo"]));
                    cell = row.CreateCell(4);
                    cell.CellStyle = style;
                    cell.CellStyle.ShrinkToFit = true;
                    cell.SetCellValue(ConvertHelper.GetString(item["PointName"]));
                    cell = row.CreateCell(5);
                    cell.CellStyle = style;
                    cell.SetCellValue(GetTaskPlanStatus(ConvertHelper.GetString(item["Status"]), ConvertHelper.GetString(item["AbnormalType"])));
                    cell = row.CreateCell(6);
                    HSSFHyperlink hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                    {
                        Address = (radioRule + photo1)
                    };
                    if (photo1 != "无")
                    {
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo1);
                    cell = row.CreateCell(7);
                    if (photo2 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo2)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo2);
                    cell = row.CreateCell(8);
                    if (photo3 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo3)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo3);
                    cell = row.CreateCell(9);
                    if (photo4 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo4)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo4);
                    cell = row.CreateCell(10);
                    if (photo5 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo5)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo5);
                    cell = row.CreateCell(11);
                    if (photo6 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo6)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo6);
                    cell = row.CreateCell(12);
                    if (photo7 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo7)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo7);
                    cell = row.CreateCell(13);
                    if (photo8 != "无")
                    {
                        hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo8)
                        };
                        cell.Hyperlink = hssfHyperlink;
                    }
                    cell.CellStyle = style;
                    cell.SetCellValue(photo8);
                    cell = row.CreateCell(14);
                    cell.CellStyle = style;
                    cell.CellStyle.ShrinkToFit = true;
                    cell.SetCellValue(ConvertHelper.GetString(item["AuditReason"]));
                    rowNum++;
                }
                PackageDown(hssfworkbook, filename);
                #endregion
            }
            else if (mediaType.Contains("框架") || mediaType.Contains("楼宇"))
            {
                if (chkIsImage.Checked)
                {
                    #region  设置列宽度
                    sheet.SetColumnWidth(0, 10 * 256);
                    sheet.SetColumnWidth(1, 20 * 256);
                    sheet.SetColumnWidth(2, 20 * 256);
                    sheet.SetColumnWidth(3, 30 * 256);
                    sheet.SetColumnWidth(4, 30 * 256);
                    sheet.SetColumnWidth(5, 40 * 256);
                    sheet.SetColumnWidth(6, 20 * 256);
                    sheet.SetColumnWidth(7, 30 * 256);
                    sheet.SetColumnWidth(8, 30 * 256);
                    sheet.SetColumnWidth(9, 30 * 256);
                    sheet.SetColumnWidth(10, 30 * 256);
                    sheet.SetColumnWidth(11, 30 * 256);
                    sheet.SetColumnWidth(12, 30 * 256);
                    sheet.SetColumnWidth(13, 30 * 256);
                    sheet.SetColumnWidth(14, 30 * 256);
                    sheet.SetColumnWidth(15, 30 * 256);
                    sheet.SetColumnWidth(16, 30 * 256);
                    sheet.SetColumnWidth(17, 30 * 256);
                    sheet.SetColumnWidth(18, 40 * 256);
                    #endregion
                    #region 框架和楼宇导出报告（带图片）
                    style = RowStyle;
                    row = sheet.CreateRow(0);
                    row.Height = 25 * 20;
                    cell = row.CreateCell(0);
                    cell.CellStyle = style;
                    cell.SetCellValue("序号");
                    cell = row.CreateCell(1);
                    cell.CellStyle = style;
                    cell.SetCellValue("城市");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style;
                    cell.SetCellValue("区域");
                    cell = row.CreateCell(3);
                    cell.CellStyle = style;
                    cell.SetCellValue("街道地址");
                    cell = row.CreateCell(4);
                    cell.CellStyle = style;
                    cell.SetCellValue("楼宇名称");
                    cell = row.CreateCell(5);
                    cell.CellStyle = style;
                    cell.SetCellValue("点位名称");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style;
                    cell.SetCellValue("媒体类型");
                    cell = row.CreateCell(7);
                    cell.CellStyle = style;
                    cell.SetCellValue("广告产品名");
                    cell = row.CreateCell(8);
                    cell.CellStyle = style;
                    cell.SetCellValue("任务周期");
                    cell = row.CreateCell(9);
                    cell.CellStyle = style;
                    cell.SetCellValue("监测结果");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片一");
                    cell = row.CreateCell(11);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片二");
                    cell = row.CreateCell(12);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片三");
                    cell = row.CreateCell(13);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片四");
                    cell = row.CreateCell(14);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片五");
                    cell = row.CreateCell(15);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片六");
                    cell = row.CreateCell(16);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片七");
                    cell = row.CreateCell(17);
                    cell.CellStyle = style;
                    cell.SetCellValue("图片八");
                    cell = row.CreateCell(18);
                    cell.CellStyle = style;
                    cell.SetCellValue("备注");
                    rowNum = 1;
                    foreach (DataRow item in dt.Rows)
                    {
                        if (radioCity.Checked)
                        {
                            radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                        }
                        else if (radioBlock.Checked)
                        {
                            radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["BlockName"].ToString()) ? "无楼宇" : item["BlockName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                        }
                        else if (radioPoint.Checked)
                        {
                            radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["BlockName"].ToString()) ? "无楼宇" : item["BlockName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["PointName"].ToString()) ? "无点位" : item["PointName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                        }
                        else if (radioAdProduct.Checked)
                        {
                            radioRule = (string.IsNullOrEmpty(item["RegionName"].ToString()) ? "无城市" : item["RegionName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["PointName"].ToString()) ? "无点位" : item["PointName"].ToString()) + System.IO.Path.DirectorySeparatorChar + (string.IsNullOrEmpty(item["AdProductName"].ToString()) ? "无广告" : item["AdProductName"].ToString()) + System.IO.Path.DirectorySeparatorChar;
                        }
                        if (!Directory.Exists(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + radioRule))
                        {
                            Directory.CreateDirectory(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + radioRule);
                        }
                        string photo1 = "无", photo2 = "无", photo3 = "无", photo4 = "无", photo5 = "无", photo6 = "无", photo7 = "无", photo8 = "无";
                        DataTable dtImage = monitor.GetImageDetailList(ConvertHelper.GetInteger(item["TPUId"]));
                        foreach (DataRow dr in dtImage.Rows)
                        {
                            switch (ConvertHelper.GetInteger(dr["Sort"]))
                            {
                                case 1: photo1 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 2: photo2 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 3: photo3 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 4: photo4 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 5: photo5 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 6: photo6 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 7: photo7 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                                case 8: photo8 = ConvertHelper.GetString(dr["ImgPath"]).Substring(dr["ImgPath"].ToString().LastIndexOf('/') + 1); break;
                            }
                            if (!string.IsNullOrEmpty(dr["ImgPath"].ToString()))
                            {
                                string sourceFileName = rootImage + System.IO.Path.DirectorySeparatorChar + dr["ImgPath"].ToString();
                                string destFileName = rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + radioRule + dr["ImgPath"].ToString().Substring(dr["ImgPath"].ToString().LastIndexOf('/'));
                                if (File.Exists(sourceFileName))
                                {
                                    File.Copy(sourceFileName, destFileName, true);
                                }
                            }
                        }
                        row = sheet.CreateRow(rowNum);
                        row.Height = 20 * 20;
                        cell = row.CreateCell(0);
                        cell.CellStyle = style;
                        cell.SetCellValue(rowNum);
                        cell = row.CreateCell(1);
                        cell.CellStyle = style;
                        cell.SetCellValue(ConvertHelper.GetString(item["RegionName"]));
                        cell = row.CreateCell(2);
                        cell.CellStyle = style;
                        cell.SetCellValue(ConvertHelper.GetString(item["AreaName"]));
                        cell = row.CreateCell(3);
                        cell.CellStyle = style;
                        cell.CellStyle.ShrinkToFit = true;
                        cell.SetCellValue(ConvertHelper.GetString(item["StreetAddress"]));
                        cell = row.CreateCell(4);
                        cell.CellStyle = style;
                        cell.CellStyle.ShrinkToFit = true;
                        cell.SetCellValue(ConvertHelper.GetString(item["BlockName"]));
                        cell = row.CreateCell(5);
                        cell.CellStyle = style;
                        cell.SetCellValue(ConvertHelper.GetString(item["PointName"]));
                        cell = row.CreateCell(6);
                        cell.CellStyle = style;
                        cell.SetCellValue(ConvertHelper.GetString(item["MediaType"]));
                        cell = row.CreateCell(7);
                        cell.CellStyle = style;
                        cell.CellStyle.ShrinkToFit = true;
                        cell.SetCellValue(ConvertHelper.GetString(item["AdProductName"]));
                        cell = row.CreateCell(8);
                        cell.CellStyle = style;
                        cell.CellStyle.ShrinkToFit = true;
                        cell.SetCellValue(ConvertHelper.GetDateTimeString(item["BeginDate"], "yyyy.MM.dd") + "-" + ConvertHelper.GetDateTimeString(item["EndDate"], "yyyy.MM.dd"));
                        cell = row.CreateCell(9);
                        cell.CellStyle = style;
                        cell.SetCellValue(GetTaskPlanStatus(ConvertHelper.GetString(item["Status"]), ConvertHelper.GetString(item["AbnormalType"])));
                        cell = row.CreateCell(10);
                        HSSFHyperlink hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = (radioRule + photo1)
                        };
                        if (photo1 != "无")
                        {
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo1);
                        cell = row.CreateCell(11);
                        if (photo2 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo2)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo2);
                        cell = row.CreateCell(12);
                        if (photo3 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo3)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo3);
                        cell = row.CreateCell(13);
                        if (photo4 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo4)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo4);
                        cell = row.CreateCell(14);
                        if (photo5 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo5)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo5);
                        cell = row.CreateCell(15);
                        if (photo6 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo6)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo6);
                        cell = row.CreateCell(16);
                        if (photo7 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo7)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo7);
                        cell = row.CreateCell(17);
                        if (photo8 != "无")
                        {
                            hssfHyperlink = new HSSFHyperlink(HyperlinkType.Url)
                            {
                                Address = (radioRule + photo8)
                            };
                            cell.Hyperlink = hssfHyperlink;
                        }
                        cell.CellStyle = style;
                        cell.SetCellValue(photo8);
                        cell = row.CreateCell(18);
                        cell.CellStyle = style;
                        cell.CellStyle.ShrinkToFit = true;
                        cell.SetCellValue(ConvertHelper.GetString(item["AuditReason"]));
                        rowNum++;
                    }
                    PackageDown(hssfworkbook, filename);
                    #endregion
                }
                else
                {
                    #region 框架报告新模板（不带图片）
                    MemoryStream m = CreatWebReportForFrame(dt, mediaType, adProductName, scheduleName, planBegin, planEnd);
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                    HttpContext.Current.Response.BinaryWrite(m.GetBuffer());
                    HttpContext.Current.Response.OutputStream.Flush();
                    HttpContext.Current.Response.OutputStream.Close();
                    #endregion
                }
            }
            else
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('该任务媒体类型的导出模板不存在！')");
                return;
            }


        }

        /// <summary>
        /// 带封面模板导出excel报告
        /// </summary>
        /// <param name="dtTask"></param>
        /// <param name="mediaType"></param>
        /// <param name="adProductName"></param>
        /// <param name="planBegin"></param>
        /// <param name="planEnd"></param>
        /// <returns></returns>
        private MemoryStream CreatWebReportForFrame(DataTable dtTask, string mediaType, string adProductName, string scheduleName, DateTime planBegin, DateTime planEnd)
        {
            FileStream fileStream = null;
            string configPath = HttpContext.Current.Server.MapPath("~/xlsTemplate/");
            fileStream = new FileStream(configPath + "ReportFrame.xls", FileMode.Open);
            hssfworkbook = new HSSFWorkbook(fileStream);
            int total = monitor.GetTaskPlanListCount(GetCondition("export"));
            if (hssfworkbook != null)
            {
                #region 处理封面
                sheet = hssfworkbook.GetSheetAt(0);
                row = sheet.GetRow(14);
                cell = row.GetCell(0);
                cell.SetCellValue(adProductName + planBegin.ToString("yyyy年M月") + mediaType + "监测");
                row = sheet.GetRow(40);
                cell = row.GetCell(0);
                cell.SetCellValue(planBegin.ToString("yyyy年M月d日") + "至" + planEnd.ToString("yyyy年M月d日"));
                #endregion
                #region 处理内容
                ICellStyle normalStyle = NormalStyle;
                ICellStyle redStyle = RedStyle;
                sheet = hssfworkbook.GetSheetAt(1);
                hssfworkbook.SetSheetName(hssfworkbook.GetSheetIndex(sheet), (string.IsNullOrEmpty(scheduleName) ? adProductName : scheduleName));
                row = sheet.GetRow(4);
                cell = row.GetCell(0);
                cell.SetCellValue("排期名称：" + adProductName);
                cell = row.GetCell(2);
                cell.SetCellValue("投放时间：" + ConvertHelper.GetShortDateString(planBegin) + "至" + ConvertHelper.GetShortDateString(planEnd));
                row = sheet.GetRow(5);
                cell = row.GetCell(0);
                cell.SetCellValue("全部投放量：" + total);
                cell = row.GetCell(1);
                cell.SetCellValue("抽检量：" + dtTask.Select(" Status=3").Length);
                cell = row.GetCell(2);
                cell.SetCellValue("抽检率：" + Math.Round((Convert.ToDouble(dtTask.Select(" Status=3").Length) / Convert.ToDouble(total) * 100), 2) + "%");
                row = sheet.GetRow(6);
                cell = row.GetCell(0);
                cell.SetCellValue("抽检量：" + dtTask.Select(" Status=3").Length);
                cell = row.GetCell(1);
                cell.SetCellValue("合格量：" + dtTask.Select(" Status=3 AND AbnormalType in (0,5)").Length);
                cell = row.GetCell(2);
                cell.SetCellValue("不合格量：" + dtTask.Select(" Status=3 AND AbnormalType in (1,2,3,4,6,7)").Length);
                cell = row.GetCell(3);
                cell.SetCellValue("合格率：" + Math.Round((Convert.ToDouble(dtTask.Select(" Status=3 AND AbnormalType in (0,5)").Length) / Convert.ToDouble(dtTask.Select(" Status=3").Length) * 100), 2) + "%");
                row = sheet.GetRow(7);
                cell = row.GetCell(0);
                cell.SetCellValue("异常原因：");
                cell = row.GetCell(1);
                cell.SetCellValue("未上画：" + dtTask.Select(" Status=3 AND AbnormalType=3").Length);
                cell = row.GetCell(2);
                cell.SetCellValue("遮挡：" + dtTask.Select(" Status=3 AND AbnormalType=6").Length);
                cell = row.GetCell(3);
                cell.SetCellValue("破损：" + dtTask.Select(" Status=3 AND AbnormalType=4").Length);

                sheet = hssfworkbook.GetSheetAt(2);
                hssfworkbook.SetSheetName(hssfworkbook.GetSheetIndex(sheet), "监测明细");
                int rowNum = 1;
                for (int i = 0; i < dtTask.Rows.Count; i++)
                {
                    row = sheet.CreateRow(rowNum);
                    cell = row.CreateCell(0);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(i + 1);//序号
                    cell = row.CreateCell(1);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["RegionName"]));
                    cell = row.CreateCell(2);
                    cell.CellStyle = normalStyle;
                    cell.CellStyle.ShrinkToFit = true;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["StreetAddress"]));
                    cell = row.CreateCell(3);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["BlockName"]));
                    cell = row.CreateCell(4);
                    cell.CellStyle = normalStyle;
                    cell.CellStyle.ShrinkToFit = true;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["PointName"]));
                    cell = row.CreateCell(5);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["MediaType"]));
                    cell = row.CreateCell(6);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["AdProductName"]));
                    cell = row.CreateCell(7);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetDateTimeString(dtTask.Rows[i]["BeginDate"], "yyyy.MM.dd") + "-" + ConvertHelper.GetDateTimeString(dtTask.Rows[i]["EndDate"], "yyyy.MM.dd"));
                    cell = row.CreateCell(8);
                    cell.CellStyle = (ConvertHelper.GetInteger(dtTask.Rows[i]["AbnormalType"]) == 0 || ConvertHelper.GetInteger(dtTask.Rows[i]["AbnormalType"]) == 5 ? normalStyle : redStyle);
                    cell.SetCellValue(GetTaskPlanStatus(ConvertHelper.GetString(dtTask.Rows[i]["Status"]), ConvertHelper.GetString(dtTask.Rows[i]["AbnormalType"]), "export"));
                    cell = row.CreateCell(9);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["AuditReason"]));
                    cell = row.CreateCell(10);
                    cell.CellStyle = normalStyle;
                    cell.SetCellValue(ConvertHelper.GetString(dtTask.Rows[i]["SpareOne"]));
                    rowNum++;
                }
            }
                #endregion
            if (fileStream != null)
            {
                fileStream.Close();
            }
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);
            return ms;
        }

        /// <summary>
        /// 打包下载excel报告和图片
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="filename"></param>
        private void PackageDown(HSSFWorkbook hssfworkbook, string filename)
        {
            MemoryStream ms = new MemoryStream();
            hssfworkbook.Write(ms);
            using (Stream localFile = new FileStream(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package" + System.IO.Path.DirectorySeparatorChar + filename + ".xls",
           FileMode.OpenOrCreate))
            {
                //ms.ToArray()转换为字节数组就是想要的图片源字节
                localFile.Write(ms.ToArray(), 0, (int)ms.Length);
            }
            //打包文件
            RarHelper.RAR(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package", rootPackImage + System.IO.Path.DirectorySeparatorChar, filename + ".rar");
            if (Directory.Exists(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package"))
            {
                Directory.Delete(rootPackImage + System.IO.Path.DirectorySeparatorChar + "package", true);
            }
            //下载文件
            string rarFullPath = rootPackImage + System.IO.Path.DirectorySeparatorChar + filename + ".rar";
            if (File.Exists(rarFullPath))
            {
                HttpRequestHelper.WebClient client = new HttpRequestHelper.WebClient();
                client.ResponseFile(rarFullPath);
                File.Delete(rarFullPath);
            }
            else
            {
                ScriptHelper.ShowCustomScript(this, "effect.Dialog.alert('没有文件可供下载！')");
            }
        }

        protected float ContentLineHight
        {
            get
            {
                return 115f;
            }
        }

        protected ICellStyle NormalStyle
        {
            get
            {
                style = hssfworkbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Bottom;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                font = hssfworkbook.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 7;//字号
                font.Color = HSSFColor.Black.Index;
                style.SetFont(font);
                return style;
            }
        }

        protected ICellStyle RedStyle
        {
            get
            {
                style = hssfworkbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Bottom;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                font = hssfworkbook.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 7;//字号
                font.Color = HSSFColor.Red.Index;
                style.SetFont(font);
                return style;
            }
        }

        private ICellStyle TitleStyle
        {
            get
            {
                style = hssfworkbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                style.FillForegroundColor = HSSFColor.White.Index;
                style.FillPattern = FillPattern.SolidForeground;
                font = hssfworkbook.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 14;//字号
                font.Color = HSSFColor.Black.Index;
                style.SetFont(font);
                return style;
            }
        }
        private ICellStyle RowStyle
        {
            get
            {
                style = hssfworkbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                font = hssfworkbook.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 10;//字号
                font.Color = HSSFColor.Black.Index;
                style.SetFont(font);
                return style;
            }
        }
        #endregion
        #endregion
    }
}