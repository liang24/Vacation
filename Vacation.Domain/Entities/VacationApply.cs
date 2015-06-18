using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("vacation_applies")]
    [PrimaryKey("id")]
    public class VacationApply : DB.Record<VacationApply>
    {
        public int ID { get; set; }

        public int VacationTypeID { get; set; }

        public int UserID { get; set; }

        public DateTime ApplyTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public EnumVacationApplyStatus Status { get; set; }

        public int? FlowID { get; set; }
    }

    /// <summary>
    /// 休假申请状态
    /// </summary>
    public enum EnumVacationApplyStatus
    {
        Apply = 1,
        Audited,
        Pass,
        Nopass
    }
}
