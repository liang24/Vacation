/******************************************************************************
 *  作者：       [尹学良]
 *  创建时间：   2013-3-13 9:00:30
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using Common.WebLib;
using PetaPoco;
using Vacation.Domain.Entities;
using Vacation.Domain.Views;


namespace Vacation.Web.AppCode
{
    /// <summary>
    /// 基础数据缓存
    /// </summary>
    public class BasicDataCache
    {
        #region 缓存Key

        /// <summary>
        /// 功能缓存键
        /// </summary>
        public const string FunctionList_CacheKey = "list_function";

        /// <summary>
        /// 菜单缓存键
        /// </summary> 
        public const string MenuList_CacheKey = "list_menu";

        /// <summary>
        /// 菜单树缓存键
        /// </summary>
        public const string MenuTree_CacheKey = "list_menutree";

        /// <summary>
        /// 休假类型缓存键
        /// </summary>
        public const string VacationType_CacheKey = "list_vacationtype";

        /// <summary>
        /// 休假审核流程缓存键
        /// </summary>
        public const string VacationAuditFlow_CacheKey = "list_vacationauditflow";

        /// <summary>
        /// 休假审核流程类型缓存键
        /// </summary>
        public const string VacationAuditFlowType_CacheKey = "list_vacationauditflowtype";

        /// <summary>
        /// 角色缓存键
        /// </summary>
        public const string Role_CacheKey = "list_role";

        /// <summary>
        /// 部门缓存键
        /// </summary>
        public const string Dept_CacheKey = "list_dept";

        /// <summary>
        /// 缓存同步锁
        /// </summary>
        private static object _cacheLockObject = new object();

        #endregion

        #region 缓存集合

        /// <summary>
        /// 功能集合
        /// </summary>  
        public static IEnumerable<SysFunction> listFunctions
        {

            get
            {
                IEnumerable<SysFunction> functions = CacheHelper.Instance.Get(FunctionList_CacheKey) as List<SysFunction>;

                if (functions != null)
                {
                    return functions;
                }

                lock (_cacheLockObject)
                {
                    functions = CacheHelper.Instance.Get(FunctionList_CacheKey) as List<SysFunction>;
                    if (functions == null)
                    {
                        functions = SysFunction.Fetch(Sql.Builder).OrderBy(function => function.Sort);

                        CacheHelper.Instance.Add(FunctionList_CacheKey, functions);
                    }
                }

                return functions;
            }
        }

        /// <summary>
        /// 菜单集合
        /// </summary>  
        public static IEnumerable<SysMenu> listMenus
        {
            get
            {
                IEnumerable<SysMenu> menus = CacheHelper.Instance.Get(MenuList_CacheKey) as List<SysMenu>;

                if (menus != null)
                {
                    return menus;
                }

                lock (_cacheLockObject)
                {
                    menus = CacheHelper.Instance.Get(MenuList_CacheKey) as List<SysMenu>;
                    if (menus == null)
                    {
                        menus = SysMenu.Fetch(Sql.Builder).OrderBy(menu => menu.Sort);

                        CacheHelper.Instance.Add(MenuList_CacheKey, menus);
                    }
                }

                return menus;
            }
        }

        public static IEnumerable<MenuTree> listMenuTrees
        {
            get
            {
                IEnumerable<MenuTree> menuTrees = CacheHelper.Instance.Get(MenuTree_CacheKey) as List<MenuTree>;

                if (menuTrees != null)
                {
                    return menuTrees;
                }

                lock (_cacheLockObject)
                {
                    menuTrees = CacheHelper.Instance.Get(MenuTree_CacheKey) as List<MenuTree>;
                    if (menuTrees == null)
                    {
                        menuTrees = MenuTree.Fetch(Sql.Builder);

                        CacheHelper.Instance.Add(MenuTree_CacheKey, menuTrees, new CacheDependency(null, new string[] { MenuList_CacheKey }));
                    }
                }

                return menuTrees;
            }
        }

        public static IEnumerable<VacationType> listVacationTypes
        {
            get
            {
                IEnumerable<VacationType> vacationTypes = CacheHelper.Instance.Get(VacationType_CacheKey) as List<VacationType>;

                if (vacationTypes != null)
                {
                    return vacationTypes;
                }

                lock (_cacheLockObject)
                {
                    vacationTypes = CacheHelper.Instance.Get(VacationType_CacheKey) as List<VacationType>;
                    if (vacationTypes == null)
                    {
                        vacationTypes = VacationType.Fetch(Sql.Builder).OrderBy(type => type.Sort);

                        CacheHelper.Instance.Add(VacationType_CacheKey, vacationTypes);
                    }
                }

                return vacationTypes;
            }
        }

        public static IEnumerable<VacationAuditFlow> listVacationAuditFlows
        {
            get
            {
                IEnumerable<VacationAuditFlow> vacationAuditFlows = CacheHelper.Instance.Get(VacationAuditFlow_CacheKey) as List<VacationAuditFlow>;

                if (vacationAuditFlows != null)
                {
                    return vacationAuditFlows;
                }

                lock (_cacheLockObject)
                {
                    vacationAuditFlows = CacheHelper.Instance.Get(VacationAuditFlow_CacheKey) as List<VacationAuditFlow>;
                    if (vacationAuditFlows == null)
                    {
                        vacationAuditFlows = VacationAuditFlow.Fetch(Sql.Builder).OrderBy(type => type.Sort);

                        CacheHelper.Instance.Add(VacationAuditFlow_CacheKey, vacationAuditFlows);
                    }
                }

                return vacationAuditFlows;
            }
        }

        public static IEnumerable<VacationAuditFlowType> listVacationAuditFlowTypes
        {
            get
            {
                IEnumerable<VacationAuditFlowType> vacationAuditFlowTypes = CacheHelper.Instance.Get(VacationAuditFlowType_CacheKey) as List<VacationAuditFlowType>;

                if (vacationAuditFlowTypes != null)
                {
                    return vacationAuditFlowTypes;
                }

                lock (_cacheLockObject)
                {
                    vacationAuditFlowTypes = CacheHelper.Instance.Get(VacationAuditFlowType_CacheKey) as List<VacationAuditFlowType>;
                    if (vacationAuditFlowTypes == null)
                    {
                        vacationAuditFlowTypes = VacationAuditFlowType.Fetch(Sql.Builder);

                        CacheHelper.Instance.Add(VacationAuditFlowType_CacheKey, vacationAuditFlowTypes);
                    }
                }

                return vacationAuditFlowTypes;
            }
        }

        public static IEnumerable<SysRole> listRoles
        {
            get
            {
                IEnumerable<SysRole> roles = CacheHelper.Instance.Get(Role_CacheKey) as List<SysRole>;

                if (roles != null)
                {
                    return roles;
                }

                lock (_cacheLockObject)
                {
                    roles = CacheHelper.Instance.Get(Role_CacheKey) as List<SysRole>;
                    if (roles == null)
                    {
                        roles = SysRole.Fetch(Sql.Builder);

                        CacheHelper.Instance.Add(Role_CacheKey, roles);
                    }
                }

                return roles;
            }
        }

        public static IEnumerable<SysDept> listDepts
        {
            get
            {
                IEnumerable<SysDept> depts = CacheHelper.Instance.Get(Dept_CacheKey) as List<SysDept>;

                if (depts != null)
                {
                    return depts;
                }

                lock (_cacheLockObject)
                {
                    depts = CacheHelper.Instance.Get(Dept_CacheKey) as List<SysDept>;
                    if (depts == null)
                    {
                        depts = SysDept.Fetch(Sql.Builder).OrderBy(type => type.Sort);

                        CacheHelper.Instance.Add(Dept_CacheKey, depts);
                    }
                }

                return depts;
            }
        }
        #endregion


        #region 缓存方法

        public static IEnumerable<MenuTree> GetMenuTrees(int parentId, bool? isVisible = true)
        {
            IEnumerable<MenuTree> trees = listMenuTrees.Where(tree => tree.ParentID == parentId);

            if (isVisible.HasValue && SysHelper.GetCurrUser().IsSuperUser == false)
            {
                trees = trees.Where(tree => CanVisible(tree) == isVisible.Value);
            }

            return trees.OrderBy(tree => tree.Sort);
        }

        public static IEnumerable<MenuTree> GetCurrUserMenuTrees()
        {
            if (!SysHelper.IsLogined())
            {
                return new List<MenuTree>();
            }

            var currUser = SysHelper.GetCurrUser();

            if (currUser.IsSuperUser)
            {
                return listMenuTrees;
            }

            var funcIds = SysPower.Fetch("where master_id=@0 and master_type=@1", currUser.ID, MasterType.User.ToString()).Select(power => power.FunctionID);
            var funcs = listFunctions.Where(func => funcIds.Contains(func.ID));

            List<MenuTree> result = new List<MenuTree>();

            result.AddRange(listMenuTrees.Where(tree => funcs.Any(func => func.MenuID == tree.ID)));

            foreach (var item in listMenuTrees.Where(tree => funcs.Any(func => func.MenuID == tree.ID)))
            {
                var parent = listMenuTrees.SingleOrDefault(tree => tree.ID == item.ParentID);

                if (parent != null && !result.Any(tree => tree.ID == parent.ID))
                {
                    result.Add(parent);
                }
            }

            return result.Where(tree => tree.IsVisible).OrderBy(tree => tree.Sort);
        }

        private static bool CanVisible(MenuTree tree)
        {
            var result = false;
            if (tree == null)
            {
                result = false;
            }
            else
            {
                if (!tree.IsParent)
                {
                    result = tree.IsVisible;
                }
                else if (tree.IsParent && tree.IsVisible == false)
                {
                    result = false;
                }
                else
                {
                    foreach (var leaf in listMenuTrees.Where(l => l.ParentID == tree.ID))
                    {
                        if (CanVisible(leaf))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取本部门及子部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static List<SysDept> GetSubDepts(int deptId)
        {
            List<SysDept> result = new List<SysDept>();

            var dept = BasicDataCache.listDepts.Single(d => d.ID == deptId);

            result.Add(dept);
            foreach (var sub in BasicDataCache.listDepts.Where(d => d.ParentID == dept.ID))
            {
                result.AddRange(GetSubDepts(sub.ID));
            }

            return result;
        }

        /// <summary>
        /// 获取所属站
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public static SysDept GetSite(int deptId)
        {
            var dept = BasicDataCache.listDepts.Single(d => d.ID == deptId);

            while (dept.ParentID != 0)
            {
                dept = BasicDataCache.listDepts.Single(d => d.ID == dept.ParentID);
            }

            return dept;
        }

        #endregion

        #region 缓存字典

        /// <summary>
        /// 功能字典
        /// </summary>  
        public static Dictionary<int, SysFunction> dicFunctions
        {
            get
            {
                return listFunctions.ToDictionary(m => m.ID);
            }
        }

        /// <summary>
        /// 菜单字典
        /// </summary>  
        public static Dictionary<int, SysMenu> dicMenus
        {
            get
            {
                return listMenus.ToDictionary(p => p.ID);
            }
        }
        #endregion

        #region 由缓存获得对应数据

        public static void RemoveCache(string cacheKey)
        {
            CacheHelper.Instance.Remove(cacheKey);
        }

        #endregion
    }

}
