using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using Common.Models;
using Vacation.Domain.Views;

namespace Vacation.Domain.Entities
{
    [TableName("sys_users")]
    [PrimaryKey("id")]
    public class SysUser : DB.Record<SysUser>
    {
        [Column("email")]
        public string Email { get; set; }

        [Column("id")]
        public int ID { get; set; }

        [Column("is_super_user")]
        public bool IsSuperUser { get; set; }

        [Column("nick_name")]
        public string NickName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("real_name")]
        public string RealName { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("is_first_visit")]
        public bool IsFirstVisit { get; set; }

        [Column("head_image")]
        public string HeadImage { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Column("employed_date")]
        public DateTime? EmployedDate { get; set; }

        [Column("sex")]
        public bool Sex { get; set; }

        [Column("marry")]
        public bool Marry { get; set; }

        [Column("dept_id")]
        public int DeptID { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        public SysUser()
        {
            this.IsFirstVisit = true;
            this.EmployedDate = null;
        }

        /// <summary>
        /// 是否有权限
        /// </summary>
        /// <param name="functionTags"></param>
        /// <returns></returns>
        public bool HasPower(IEnumerable<string> functionTags)
        {
            if (this.IsSuperUser)
            {
                return true;
            }

            if (functionTags.HasElements() == false)
            {
                return false;
            }

            var power = SysPower.FirstOrDefault("where [master_id]=@0 and [master_type]=@1 and [function_id] in (select id from sys_functions where function_tag in (@2))", this.ID, MasterType.User.ToString(), functionTags);

            return power != null;
        }

        public Page<MessageView> GetMessages(int pageIndex, int pageSize, MessageStatus? status = null)
        {
            var sql = Sql.Builder.Where("receiver_id=@0", this.ID);

            if (status.HasValue)
            {
                sql.Where("status=@0", (int)status.Value);
            }

            return MessageView.Page(pageIndex, pageSize, sql);
        }
    }
}
