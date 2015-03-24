using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("sys_powers")]
    [PrimaryKey("id")]
    public class SysPower : DB.Record<SysPower>
    {
        [Column("function_id")]
        public int FunctionID { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("master_id")]
        public int MasterID { get; set; }

        [Column("master_type")]
        public string MasterType { get; set; }
    }
}
