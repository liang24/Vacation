using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vacation.Domain.Entities;

namespace Vacation.Web.Models
{
    public class VacationAuditFlowTypeModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary> 
        public string Name { get; set; }

        /// <summary>
        /// 适用角色
        /// </summary> 
        public string UsedRoles { get; set; }

        /// <summary>
        /// 流程
        /// </summary>
        public IEnumerable<VacationAuditFlowModel> Flows { get; set; }
    }

    public class VacationAuditFlowModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string AuditRole { get; set; }

        public int Sort { get; set; }

        public string DeptLevel { get; set; }
    }
}