using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("sys_roles")]
    [PrimaryKey("id")]
    public class SysRole : DB.Record<SysRole>
    {
        [Column("description")]
        public string Description { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("is_system")]
        public bool IsSystem { get; set; }

        [Column("role_name")]
        public string Name { get; set; }
    }
}
