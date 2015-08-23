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

        [Column("audit_role_id")]
        public int AuditRoleID { get; set; }

        [Column("sort")]
        public int Sort { get; set; }

        [Column("dept_level")]
        public EnumDeptLevel DeptLevel { get; set; }

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
        /// 所有部门
        /// </summary>
        [Enum("所有部门")]
        All
    }
}
