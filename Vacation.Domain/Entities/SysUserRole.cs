using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace Vacation.Domain.Entities
{
    [TableName("sys_user_roles")]
    public class SysUserRole
    {
        [Column("role_id")]
        public int RoleID { get; set; }

        [Column("user_id")]
        public int UserID { get; set; }
    }
}
