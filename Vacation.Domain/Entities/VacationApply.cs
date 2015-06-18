﻿using System;
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
        [Column("id")]
        public int ID { get; set; }

        [Column("vacation_type_id")]
        public int VacationTypeID { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }

        [Column("apply_time")]
        public DateTime ApplyTime { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        public EnumVacationApplyStatus Status { get; set; }

        [Column("flow_id")]
        public int? FlowID { get; set; }

        [Column("reason")]
        public string Reason { get; set; }
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
