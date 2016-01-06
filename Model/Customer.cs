using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Customer
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int CusId { get; set; }

        /// <summary>
        /// 所在公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string cPassWord { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }

        /// <summary>
        /// 是否停用 0否 1是
        /// </summary>
        public int IsDisabled { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户类型 0客户 1管理员 2账号
        /// </summary>
        public int CustomerType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
