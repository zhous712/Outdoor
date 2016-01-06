using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TaskProject
    {
        /// <summary>
        /// 排期ID
        /// </summary>
        public int TPId { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public int TId { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 城市/地区id
        /// </summary>
        public string RegionId { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 楼宇名称
        /// </summary>
        public string BlockName { get; set; }

        /// <summary>
        /// 点位名称
        /// </summary>
        public string PointName { get; set; }

        /// <summary>
        /// 媒体类型
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// 广告产品名
        /// </summary>
        public string AdProductName { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 拍照要求
        /// </summary>
        public string PhotoRequire { get; set; }

        /// <summary>
        /// 项目状态 0草稿 1已领取 2已上传 3审核通过 4已过期
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 任务金额
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 备用字段1
        /// </summary>
        public string SpareOne { get; set; }

        /// <summary>
        /// 备用字段2
        /// </summary>
        public string SpareTwo { get; set; }

        /// <summary>
        ///   --异常类型 0正常 1黑屏 2花屏 3未上画 4破损 5电梯维修 6遮挡 7异常
        /// </summary>
        public int AbnormalType { get; set; }
    }
}
