using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
[TableName("textnotices")]
    [PrimaryKey("id")]

    public class Notice : DB.Record<Notice>
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }


        [Column("create_time")]
        public string CreateTime { get; set; }

     public Notice()
        {
            CreateTime = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            
                }
    }

}
