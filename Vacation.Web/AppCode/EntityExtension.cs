using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vacation.Domain.Entities;

namespace Vacation.Web.AppCode
{
    public static class EntityExtension
    {
        public static SysDept ToDept(this int id)
        {
            var dept = BasicDataCache.listDepts.SingleOrDefault(d => d.ID == id);

            if (dept == null)
            {
                dept = new SysDept();
                if (id == 0)
                {
                    dept.Name = "一级部门";
                }
            }

            return dept;
        }

        public static SysRole ToRole(this int id)
        {
            var role = BasicDataCache.listRoles.SingleOrDefault(r => r.ID == id);

            if (role == null)
            {
                role = new SysRole();
            }

            return role;
        }

        public static SysUser ToUser(this int id)
        {
            var user = SysUser.SingleOrDefault(id);

            if (user == null)
            {
                user = new SysUser();
            }

            return user;
        }

        public static VacationType ToVacationType(this int id)
        {
            var type = BasicDataCache.listVacationTypes.SingleOrDefault(t => t.ID == id);

            if (type == null)
            {
                type = new VacationType();
            }

            return type;
        }

        public static string GetFullDeptName(this SysDept dept)
        {
            string name = dept.Name;

            while (dept.ParentID != 0)
            {
                dept = BasicDataCache.listDepts.Single(d => d.ID == dept.ParentID);
                name = string.Format("{0} > {1}", dept.Name, name);
            }

            return name;
        }
    }
}