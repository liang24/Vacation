using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    /// <summary>
    /// 休假类型
    /// </summary>
    [TableName("vacation_types")]
    [PrimaryKey("id")]
    public class VacationType : DB.Record<VacationType>
    {
        [Column("id")]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        //[Column("type")]
        //public string Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 是否系统数据
        /// </summary>
        [Column("is_system")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column("description")]
        public string Description { get; set; }
    }
}
