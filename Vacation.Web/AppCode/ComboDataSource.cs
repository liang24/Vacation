using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Models;
using Vacation.Domain.Entities;

namespace Vacation.Web.AppCode
{
    public class ComboDataSource
    {
        public static List<ComboBox> DeptTree(int parentId = 0)
        {
            List<ComboBox> result = new List<ComboBox>();

            var depts = BasicDataCache.listDepts;
            if (parentId > 0)
            {
                depts = depts.Where(d => d.ID == parentId);
            }
            else
            {
                depts = depts.Where(d => d.ParentID == parentId);
            }

            string prefix = string.Empty;

            foreach (var dept in depts)
            {
                ComboBox combo = new ComboBox();
                combo.ID = dept.ID;
                combo.Text = string.Format("{0}{1}", prefix, dept.Name);
                combo.Value = dept.ID.ToString();

                result.Add(combo);
                result.AddRange(DeptRecursion(dept, prefix));
            }

            return result;
        }

        /// <summary>
        /// 部门递归
        /// </summary>
        /// <param name="parent">上级部门</param>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        private static List<ComboBox> DeptRecursion(SysDept parent, string prefix)
        {
            List<ComboBox> result = new List<ComboBox>();

            prefix = string.IsNullOrEmpty(prefix) ? "┝" : prefix + "─";

            var depts = BasicDataCache.listDepts.Where(d => d.ParentID == parent.ID);

            foreach (var dept in depts)
            {
                ComboBox combo = new ComboBox();
                combo.ID = dept.ID;
                combo.Text = string.Format("{0}{1}", prefix, dept.Name);
                combo.Value = dept.ID.ToString();

                result.Add(combo);
                result.AddRange(DeptRecursion(dept, prefix));
            }

            return result;
        }

        public static List<ComboBox> VacationType()
        {
            return BasicDataCache.listVacationTypes.Select(type => new ComboBox
            {
                ID = type.ID,
                Text = type.Name,
                Value = type.ID.ToString()
            }).ToList();
        }

        public static List<ComboBox> Role()
        {
            return BasicDataCache.listRoles.Select(type => new ComboBox
            {
                ID = type.ID,
                Text = type.Name,
                Value = type.ID.ToString()
            }).ToList();
        }
    }
}