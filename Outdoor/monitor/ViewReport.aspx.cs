using AutoRadio.RadioSmart.Common;
using Dal;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using Outdoor._code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outdoor.monitor
{
    public partial class ViewReport : BasePage
    {
        Monitor monitor = new Monitor();
        protected string pageStr = string.Empty;
        protected string scheduleName = string.Empty;
        protected int SId;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckLoginOn();
            SId = ConvertHelper.GetInteger(Request["sid"]);
            hidCity.Value = ConvertHelper.GetString(Request["regionids"]);
            hidBlock.Value = ConvertHelper.GetString(Request["blocknames"]);
            if (!IsPostBack)
            {
                jianbo_UserMenu.MenuType = 2;
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

        /// <summary>
        /// 绑定页面
        /// </summary>
        private void BindPageList()
        {
            int notPaintTotal = 0, hiddenTotal = 0, normalTotal = 0, breakTotal = 0;
            DataTable dt = monitor.GetScheduleList(" SId=" + SId, 1, 20).Tables[0];
            if (dt.Rows.Count > 0)
            {
                scheduleName = dt.Rows[0]["ScheduleName"].ToString();
                fontScheduleTitle.InnerText = dt.Rows[0]["ScheduleName"] + "（" + ConvertHelper.GetShortDateString(dt.Rows[0]["BeginDate"]) + "至" + ConvertHelper.GetShortDateString(dt.Rows[0]["EndDate"]) + "）";
            }
            int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            int pageSize = 9999;
            int recordCount = 0;
            DataSet ds = new DataSet();
            recordCount = monitor.GetReportDetailCount(GetCondition());
            ds = monitor.GetReportDetailList(GetCondition(), pageIndex, pageSize);
            dt = ds.Tables[0];
            DataTable dtCity = monitor.GetCityReportDetailList(GetCondition()).Tables[0];
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add(new DataColumn("RegionId", typeof(string)));
            dtNew.Columns.Add(new DataColumn("City", typeof(string)));
            dtNew.Columns.Add(new DataColumn("RegionName", typeof(string)));
            dtNew.Columns.Add(new DataColumn("BlockName", typeof(string)));
            dtNew.Columns.Add(new DataColumn("NotPaintCount", typeof(int)));
            dtNew.Columns.Add(new DataColumn("HiddenCount", typeof(int)));
            dtNew.Columns.Add(new DataColumn("BreakCount", typeof(int)));
            dtNew.Columns.Add(new DataColumn("NormalCount", typeof(int)));
            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                DataRow dr = dtNew.NewRow();
                dr["RegionId"] = dtCity.Rows[i]["RegionId"];
                dr["City"] = dtCity.Rows[i]["RegionName"];
                dr["RegionName"] = "<span style=\"margin-right:20px;color: black;cursor:pointer\" onclick=\"OpenAndCloseBlock('" + dtCity.Rows[i]["RegionId"] + "',this)\">━</span>" + dtCity.Rows[i]["RegionName"] + " Total";
                dr["BlockName"] = "";
                dr["NotPaintCount"] = dtCity.Rows[i]["NotPaintCount"];
                dr["HiddenCount"] = dtCity.Rows[i]["HiddenCount"];
                dr["BreakCount"] = dtCity.Rows[i]["BreakCount"];
                dr["NormalCount"] = dtCity.Rows[i]["NormalCount"];
                dtNew.Rows.Add(dr);
                DataRow[] drc = dt.Select(" RegionId='" + dtCity.Rows[i]["RegionId"] + "'");
                for (int j = 0; j < drc.Length; j++)
                {
                    dr = dtNew.NewRow();
                    dr["RegionId"] = drc[j]["RegionId"];
                    dr["City"] = drc[j]["RegionName"];
                    dr["RegionName"] = j == 0 ? ("<span style=\"font-size: larger; font-weight: 700;color:black\">" + drc[j]["RegionName"] + "</span>") : "";
                    dr["BlockName"] = drc[j]["BlockName"];
                    dr["NotPaintCount"] = drc[j]["NotPaintCount"];
                    dr["HiddenCount"] = drc[j]["HiddenCount"];
                    dr["BreakCount"] = drc[j]["BreakCount"];
                    dr["NormalCount"] = drc[j]["NormalCount"];
                    dtNew.Rows.Add(dr);
                }
                notPaintTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["NotPaintCount"]);
                hiddenTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["HiddenCount"]);
                normalTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["NormalCount"]);
                breakTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["BreakCount"]);
            }
            this.rpt.DataSource = dtNew;
            this.rpt.DataBind();
            NotPaintTotal.Text = notPaintTotal == 0 ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=NotPaint\"><div class=\"abnormal\">" + notPaintTotal.ToString() + "</div></a>";
            HiddenTotal.Text = hiddenTotal == 0 ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=Hidden\"><div class=\"abnormal\">" + hiddenTotal.ToString() + "</div></a>";
            BreakTotal.Text = breakTotal == 0 ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=Break\"><div class=\"abnormal\">" + breakTotal.ToString() + "</div></a>";
            NormalTotal.Text = normalTotal == 0 ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=Normal\"><div class=\"normal\">" + normalTotal.ToString() + "</div></a>";
            AllTotal.Text = (notPaintTotal + hiddenTotal + breakTotal + normalTotal) == 0 ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=All\"><div class=\"normal\">" + (notPaintTotal + hiddenTotal + breakTotal + normalTotal).ToString() + "</div></a>";
            // pageStr = base.BindPageList(pageIndex, pageSize, recordCount, string.Format("ViewReport.aspx?sid={0}", SId));
        }

        private string GetCondition(string type = "select")
        {
            string strCondition = " tp.TId IN(SELECT TId FROM dbo.Task WHERE SId=" + SId + ")";
            if (type == "select")
            {
                strCondition += " AND tp.Status=3";
            }
            if (!string.IsNullOrEmpty(hidCity.Value))
            {
                strCondition += " AND tp.RegionId IN (" + hidCity.Value.Trim(',') + ")";
            }
            if (!string.IsNullOrEmpty(hidBlock.Value))
            {
                string[] blockArr = hidBlock.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string blockNames = string.Empty;
                foreach (string item in blockArr)
                {
                    blockNames += "'" + item + "'" + ",";
                }
                strCondition += " AND tp.BlockName IN (" + blockNames.Trim(',') + ")";
            }
            return strCondition;
        }

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                Literal litNotPaintCount = (Literal)e.Item.FindControl("litNotPaintCount");
                Literal litHiddenCount = (Literal)e.Item.FindControl("litHiddenCount");
                Literal litBreakCount = (Literal)e.Item.FindControl("litBreakCount");
                Literal litNormalCount = (Literal)e.Item.FindControl("litNormalCount");
                Literal litTotalCount = (Literal)e.Item.FindControl("litTotalCount");
                litNotPaintCount.Text = drv["NotPaintCount"].ToString() == "0" ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=NotPaint" + (drv["RegionName"].ToString().Contains("Total") ? "&city=" + drv["City"].ToString() : "&city=" + drv["City"].ToString() + "&brand=" + drv["BlockName"].ToString()) + "\"><div class=\"abnormal\">" + drv["NotPaintCount"].ToString() + "</div></a>";
                litHiddenCount.Text = drv["HiddenCount"].ToString() == "0" ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=Hidden" + (drv["RegionName"].ToString().Contains("Total") ? "&city=" + drv["City"].ToString() : "&city=" + drv["City"].ToString() + "&brand=" + drv["BlockName"].ToString()) + "\"><div class=\"abnormal\">" + drv["HiddenCount"].ToString() + "</div></a>";
                litBreakCount.Text = drv["BreakCount"].ToString() == "0" ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=Break" + (drv["RegionName"].ToString().Contains("Total") ? "&city=" + drv["City"].ToString() : "&city=" + drv["City"].ToString() + "&brand=" + drv["BlockName"].ToString()) + "\"><div class=\"abnormal\">" + drv["BreakCount"].ToString() + "</div></a>";
                litNormalCount.Text = drv["NormalCount"].ToString() == "0" ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=Normal" + (drv["RegionName"].ToString().Contains("Total") ? "&city=" + drv["City"].ToString() : "&city=" + drv["City"].ToString() + "&brand=" + drv["BlockName"].ToString()) + "\"><div class=\"normal\">" + drv["NormalCount"].ToString() + "</div></a>";
                litTotalCount.Text = (ConvertHelper.GetInteger(drv["NotPaintCount"]) + ConvertHelper.GetInteger(drv["HiddenCount"]) + ConvertHelper.GetInteger(drv["BreakCount"]) + ConvertHelper.GetInteger(drv["NormalCount"])) == 0 ? "" : "<a href=\"ViewPlan.aspx?sid=" + SId + "&type=All" + (drv["RegionName"].ToString().Contains("Total") ? "&city=" + drv["City"].ToString() : "&city=" + drv["City"].ToString() + "&brand=" + drv["BlockName"].ToString()) + "\"><div class=\"normal\">" + (ConvertHelper.GetInteger(drv["NotPaintCount"]) + ConvertHelper.GetInteger(drv["HiddenCount"]) + ConvertHelper.GetInteger(drv["BreakCount"]) + ConvertHelper.GetInteger(drv["NormalCount"])).ToString() + "</div></a>";
            }
        }

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
            var filename = string.IsNullOrEmpty(scheduleName) ? (adProductName + mediaType + "监测" + DateTime.Now.ToString("yyyyMMddHHmmss")) : scheduleName;
            sheet = hssfworkbook.CreateSheet(adProductName + mediaType);
            row = sheet.CreateRow(0);
            row.Height = 30 * 20;
            if (mediaType.Contains("框架") || mediaType.Contains("楼宇"))
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
                ICellStyle cityStyle = CityStyle;
                ICellStyle blockStyle = BlockStyle;
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

                row = sheet.CreateRow(10);
                cell = row.CreateCell(0);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("城市");
                cell = row.CreateCell(1);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("楼宇名称");
                cell.CellStyle = TitleStyle;
                cell = row.CreateCell(2);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("未上画");
                cell = row.CreateCell(3);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("遮挡");
                cell = row.CreateCell(4);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("破损");
                cell = row.CreateCell(5);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("正常");
                cell = row.CreateCell(6);
                cell.CellStyle = TitleStyle;
                cell.SetCellValue("合计");
                int notPaintTotal = 0, hiddenTotal = 0, normalTotal = 0, breakTotal = 0;
                int tempNum = 11;
                DataTable dt = monitor.GetReportDetailList(GetCondition(), 1, 9999).Tables[0];
                DataTable dtCity = monitor.GetCityReportDetailList(GetCondition()).Tables[0];
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add(new DataColumn("RegionId", typeof(string)));
                dtNew.Columns.Add(new DataColumn("City", typeof(string)));
                dtNew.Columns.Add(new DataColumn("RegionName", typeof(string)));
                dtNew.Columns.Add(new DataColumn("BlockName", typeof(string)));
                dtNew.Columns.Add(new DataColumn("NotPaintCount", typeof(int)));
                dtNew.Columns.Add(new DataColumn("HiddenCount", typeof(int)));
                dtNew.Columns.Add(new DataColumn("BreakCount", typeof(int)));
                dtNew.Columns.Add(new DataColumn("NormalCount", typeof(int)));
                for (int i = 0; i < dtCity.Rows.Count; i++)
                {
                    DataRow dr = dtNew.NewRow();
                    dr["RegionId"] = dtCity.Rows[i]["RegionId"];
                    dr["City"] = dtCity.Rows[i]["RegionName"];
                    dr["RegionName"] = dtCity.Rows[i]["RegionName"] + " Total";
                    dr["BlockName"] = "";
                    dr["NotPaintCount"] = dtCity.Rows[i]["NotPaintCount"];
                    dr["HiddenCount"] = dtCity.Rows[i]["HiddenCount"];
                    dr["BreakCount"] = dtCity.Rows[i]["BreakCount"];
                    dr["NormalCount"] = dtCity.Rows[i]["NormalCount"];
                    dtNew.Rows.Add(dr);
                    DataRow[] drc = dt.Select(" RegionId='" + dtCity.Rows[i]["RegionId"] + "'");
                    for (int j = 0; j < drc.Length; j++)
                    {
                        dr = dtNew.NewRow();
                        dr["RegionId"] = drc[j]["RegionId"];
                        dr["City"] = drc[j]["RegionName"];
                        dr["RegionName"] = j == 0 ? drc[j]["RegionName"] : "";
                        dr["BlockName"] = drc[j]["BlockName"];
                        dr["NotPaintCount"] = drc[j]["NotPaintCount"];
                        dr["HiddenCount"] = drc[j]["HiddenCount"];
                        dr["BreakCount"] = drc[j]["BreakCount"];
                        dr["NormalCount"] = drc[j]["NormalCount"];
                        dtNew.Rows.Add(dr);
                    }
                    notPaintTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["NotPaintCount"]);
                    hiddenTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["HiddenCount"]);
                    normalTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["NormalCount"]);
                    breakTotal += ConvertHelper.GetInteger(dtCity.Rows[i]["BreakCount"]);
                }
                for (int j = 0; j < dtNew.Rows.Count; j++)
                {
                    bool IsCityTr = dtNew.Rows[j]["RegionName"].ToString().Contains("Total");
                    row = sheet.CreateRow(tempNum);
                    cell = row.CreateCell(0);
                    cell.CellStyle = IsCityTr ? cityStyle : blockStyle;
                    cell.SetCellValue(dtNew.Rows[j]["RegionName"].ToString());
                    cell = row.CreateCell(1);
                    cell.CellStyle = IsCityTr ? cityStyle : blockStyle;
                    cell.CellStyle.ShrinkToFit = true;
                    cell.SetCellValue(dtNew.Rows[j]["BlockName"].ToString());
                    cell = row.CreateCell(2);
                    cell.CellStyle = dtNew.Rows[j]["NotPaintCount"].ToString() == "0" ? (IsCityTr ? cityStyle : blockStyle) : (IsCityTr ? cityStyle : AbnormalStyle);
                    cell.SetCellValue(dtNew.Rows[j]["NotPaintCount"].ToString() == "0" ? "" : dtNew.Rows[j]["NotPaintCount"].ToString());
                    cell = row.CreateCell(3);
                    cell.CellStyle = dtNew.Rows[j]["HiddenCount"].ToString() == "0" ? (IsCityTr ? cityStyle : blockStyle) : (IsCityTr ? cityStyle : AbnormalStyle);
                    cell.SetCellValue(dtNew.Rows[j]["HiddenCount"].ToString() == "0" ? "" : dtNew.Rows[j]["HiddenCount"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = dtNew.Rows[j]["BreakCount"].ToString() == "0" ? (IsCityTr ? cityStyle : blockStyle) : (IsCityTr ? cityStyle : AbnormalStyle);
                    cell.SetCellValue(dtNew.Rows[j]["BreakCount"].ToString() == "0" ? "" : dtNew.Rows[j]["BreakCount"].ToString());
                    cell = row.CreateCell(5);
                    cell.CellStyle = IsCityTr ? cityStyle : blockStyle;
                    cell.SetCellValue(dtNew.Rows[j]["NormalCount"].ToString() == "0" ? "" : dtNew.Rows[j]["NormalCount"].ToString());
                    cell = row.CreateCell(6);
                    cell.CellStyle = IsCityTr ? cityStyle : blockStyle;
                    cell.SetCellValue((ConvertHelper.GetInteger(dtNew.Rows[j]["NotPaintCount"]) + ConvertHelper.GetInteger(dtNew.Rows[j]["HiddenCount"]) + ConvertHelper.GetInteger(dtNew.Rows[j]["BreakCount"]) + ConvertHelper.GetInteger(dtNew.Rows[j]["NormalCount"])).ToString() == "0" ? "" : (ConvertHelper.GetInteger(dtNew.Rows[j]["NotPaintCount"]) + ConvertHelper.GetInteger(dtNew.Rows[j]["HiddenCount"]) + ConvertHelper.GetInteger(dtNew.Rows[j]["BreakCount"]) + ConvertHelper.GetInteger(dtNew.Rows[j]["NormalCount"])).ToString());
                    tempNum++;
                }
                row = sheet.CreateRow(tempNum);
                cell = row.CreateCell(0);
                cell.CellStyle = blockStyle;
                cell.SetCellValue("合计");
                cell = row.CreateCell(1);
                cell.CellStyle = blockStyle;
                cell.SetCellValue("");
                cell = row.CreateCell(2);
                cell.CellStyle = blockStyle;
                cell.SetCellValue(notPaintTotal);
                cell = row.CreateCell(3);
                cell.CellStyle = blockStyle;
                cell.SetCellValue(hiddenTotal);
                cell = row.CreateCell(4);
                cell.CellStyle = blockStyle;
                cell.SetCellValue(breakTotal);
                cell = row.CreateCell(5);
                cell.CellStyle = blockStyle;
                cell.SetCellValue(normalTotal);
                cell = row.CreateCell(6);
                cell.CellStyle = blockStyle;
                cell.SetCellValue(notPaintTotal + hiddenTotal + breakTotal + normalTotal);


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
                style.VerticalAlignment = VerticalAlignment.Bottom;
                style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.CornflowerBlue.Index;
                style.FillPattern = FillPattern.SolidForeground;
                style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.CornflowerBlue.Index;
                font = hssfworkbook.CreateFont();
                font.FontName = "微软雅黑";
                font.FontHeightInPoints = 10;//字号
                font.Color = HSSFColor.Black.Index;
                style.SetFont(font);
                return style;
            }
        }

        private ICellStyle CityStyle
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
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
                style.FillPattern = FillPattern.SolidForeground;
                style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
                font = hssfworkbook.CreateFont();
                font.FontName = "微软雅黑";
                font.FontHeightInPoints = 10;//字号
                font.Color = HSSFColor.Black.Index;
                style.SetFont(font);
                return style;
            }
        }

        private ICellStyle BlockStyle
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
                font.FontName = "微软雅黑";
                font.FontHeightInPoints = 10;//字号
                font.Color = HSSFColor.Black.Index;
                style.SetFont(font);
                return style;
            }
        }

        private ICellStyle AbnormalStyle
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
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                style.FillPattern = FillPattern.SolidForeground;
                style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                font = hssfworkbook.CreateFont();
                font.FontName = "微软雅黑";
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