using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vacation.Web.Models
{
    public class VacationApplyModel
    {
        public int ID { get; set; }

        public string VacationTypeName { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string DeptName { get; set; }

        public string ApplyTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Status { get; set; }

        public bool CanModify { get; set; }

        /// <summary>
        /// 原因
        /// </summary> 
        public string Reason { get; set; }

        /// <summary>
        /// 意见
        /// </summary> 
        public string Opinion { get; set; }
    }
}