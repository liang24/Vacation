using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    /// <summary>
    /// 休假类型审核流程类型
    /// </summary>
    [TableName("vacation_audit_flow_types")]
    [PrimaryKey("id")]
    public class VacationAuditFlowType : DB.Record<VacationAuditFlowType>
    {
        [Column("id")]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// 适用角色
        /// </summary>
        [Column("used_role_ids")]
        public string UsedRoleIDs { get; set; }
    }
}
