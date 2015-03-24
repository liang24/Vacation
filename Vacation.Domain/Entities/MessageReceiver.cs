/********************************************************************
* File Name: M_Sending
* Copyright (C) 2013 本程序由 实体类代码生成器 生成  所有权 HDZ(侯东照)
* Creater Date: 2013/2/21
* Modify Explain: 
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("message_receivers")]
    [PrimaryKey("ID")]
    public class MessageReceiver : DB.Record<MessageReceiver>
    {
        /// <summary>
        /// 可达消息接受者列表ID
        /// </summary> 
        [Column]
        public int ID { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        [Column("message_id")]
        public int MessageID { get; set; }

        /// <summary>
        /// 消息接受者ID
        /// </summary>
        [Column("receiver_id")]
        public int ReceiverID { get; set; }

        /// <summary>
        /// 消息发送时间
        /// </summary>
        [Column("create_time")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        [Column]
        public MessageStatus Status { get; set; }
    }

    /// <summary>
    /// 系统信息状态
    /// </summary> 
    public enum MessageStatus
    {
        /// <summary>
        /// 未读
        /// </summary>
        Unread,
        /// <summary>
        /// 已读
        /// </summary>
        Readeded
    }
}
