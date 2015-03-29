﻿/******************************************************************************
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
