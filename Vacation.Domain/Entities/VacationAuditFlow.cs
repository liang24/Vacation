using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Common.EnumLib;

namespace Vacation.Domain.Entities
{
    [TableName("vacation_audit_flows")]
    [PrimaryKey("id")]
    public class VacationAuditFlow : DB.Record<VacationAuditFlow>
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; } 

        /// <summary>
        /// 审核角色
        /// </summary>
        [Column("audit_role_ids")]
        public string AuditRoleIDs { get; set; }

        [Column("sort")]
        public int Sort { get; set; }

        [Column("dept_level")]
        public EnumDeptLevel DeptLevel { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        [Column("type_id")]
        public int TypeID { get; set; }

        public VacationAuditFlow()
        {
            Sort = 1;
        }
    }

    /// <summary>
    /// 部门层次
    /// </summary>
    public enum EnumDeptLevel
    {
        /// <summary>
        /// 本部门或子部门
        /// </summary>
        [Enum("本部门或子部门")]
        TheOrSub = 1,
        /// <summary>
        /// 本站内
        /// </summary>
        [Enum("本站内")]
        TheSite,
        /// <summary>
        /// 所有站
        /// </summary>
        [Enum("所有站")]
        All
    }
}
