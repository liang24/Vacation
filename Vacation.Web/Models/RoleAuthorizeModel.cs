using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vacation.Domain.Views;
using Vacation.Domain.Entities;

namespace Vacation.Web.Models
{
    public class RoleAuthorizeModel
    {
        public SysRole Role { get; set; }

        public List<MenuTree> Trees { get; set; }

        public List<SysFunction> Funcs { get; set; }

        public IEnumerable<int> SelectedFuncIds { get; set; }
    }
}