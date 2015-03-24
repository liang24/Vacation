using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("messages")]
    [PrimaryKey("id")]
    public class Message : DB.Record<Message>
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [Column]
        public int ID { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        [Column]
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Column]
        public string Content { get; set; }

        /// <summary>
        /// 消息生成时间
        /// </summary>
        [Column("create_time")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 发送者账号
        /// </summary>
        [Column("sender_id")]
        public int SenderID { get; set; }
    }
}
