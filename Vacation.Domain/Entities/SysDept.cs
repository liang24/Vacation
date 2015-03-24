using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    /// <summary>
    /// 部门
    /// </summary>
    [TableName("sys_depts")]
    [PrimaryKey("id")]
    public class SysDept : DB.Record<SysDept>
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("dept_code")]
        public string Code { get; set; }

        [Column("dept_name")]
        public string Name { get; set; }

        [Column("parent_id")]
        public int ParentID { get; set; }

        [Column("sort")]
        public int Sort { get; set; }
    }
}
