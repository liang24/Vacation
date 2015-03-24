using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [PrimaryKey("id")]
    [TableName("sys_functions")]
    public class SysFunction : DB.Record<SysFunction>
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("menu_id")]
        public int MenuID { get; set; }

        [Column("function_name")]
        public string Name { get; set; }

        [Column("sort")]
        public int Sort { get; set; }

        [Column("function_tag")]
        public string Tag { get; set; }
    }
}
