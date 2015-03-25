using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Views
{
    [TableName("v_sys_menu_tree")]
    public class MenuTree : DB.View<MenuTree>
    {
        /// <summary>
        /// 描述
        /// </summary>
        [Column("description")]
        public string Description { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Column("icon")]
        public string Icon { get; set; }
        [Column("id")]
        public int ID { get; set; }
        [Column("is_parent")]
        public bool IsParent { get; set; }
        /// <summary>
        /// 是否首页显示
        /// </summary>
        [Column("is_showindex")]
        public bool IsShowIndex { get; set; }
        [Column("is_visible")]
        public bool IsVisible { get; set; }
        [Column("menu_name")]
        public string Name { get; set; }
        [Column("parent_id")]
        public int ParentID { get; set; }
        public int Sort { get; set; }
        [Column("menu_tag")]
        public string Tag { get; set; }
        [Column("url")]
        public string Url { get; set; }
    }
}
