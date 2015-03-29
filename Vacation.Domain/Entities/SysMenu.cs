using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("sys_menus")]
    [PrimaryKey("id")]
    public class SysMenu : DB.Record<SysMenu>
    {
        [Column("description")]
        public string Description { get; set; }

        [Column("icon")]
        public string Icon { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("is_visible")]
        public bool IsVisible { get; set; }

        [Column("menu_name")]
        public string Name { get; set; }

        [Column("parent_id")]
        public int ParentID { get; set; }

        [Column("sort")]
        public int Sort { get; set; }

        [Column("menu_tag")]
        public string Tag { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Ignore]
        public IEnumerable<SysFunction> Functions { get; set; }
    }
}
