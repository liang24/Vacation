using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Vacation.Domain.Entities;

namespace Vacation.Domain.Views
{
    [TableName("v_messages")]
    public class MessageView : DB.Record<MessageView>
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
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 发送者账号
        /// </summary>
        [Column("sender_id")]
        public int SenderID { get; set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        [Column]
        public MessageStatus Status { get; set; }

        /// <summary>
        /// 消息接受者ID
        /// </summary>
        [Column("receiver_id")]
        public int ReceiverID { get; set; }
    }
}
