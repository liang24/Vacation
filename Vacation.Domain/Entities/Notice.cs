using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("notices")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class Notice : DB.Record<Notice>
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        public string CreateTimeString { get { return this.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        public Notice()
        {
            CreateTime = DateTime.Now;
        }
    }

}
