using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("vacation_audit_flows")]
    [PrimaryKey("id")]
    public class VacationAuditFlow : DB.Record<VacationAuditFlow>
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? PrevFlowID { get; set; }

        public int? NextFlowID { get; set; }

        public int AuditRoleID { get; set; }


    }
}
