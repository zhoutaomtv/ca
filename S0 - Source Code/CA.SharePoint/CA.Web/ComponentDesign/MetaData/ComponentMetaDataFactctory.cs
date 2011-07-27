//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：ComponentMetaDataFactctory.cs
// 文件功能描述：组件元数据工厂 
// 
// 
// 创建标识： 张建义 2007-7-3
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace CA.Web.ComponentDesign
{
    public static class ComponentMetaDataFactctory
    {
        private static readonly IDictionary<string, ComponentSet> _ComponentSets = new Dictionary<string, ComponentSet>();

        static public ComponentSet GetMetaData(Type t)
        {
            return new ComponentSet(t); //暂时禁用缓存

            if ( _ComponentSets.ContainsKey(t.FullName) )
            {
                return _ComponentSets[t.FullName];
            }
            else
            {
                ComponentSet set = new ComponentSet( t);

                try
                {
                    _ComponentSets.Add(t.FullName, set);
                }
                catch { } //防止多线程问题

                return set;
            }
        }

    }
}
