using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CA.SharePoint.EventReveivers
{
    public static class EventReceiverManager
    {
        /// <summary>
        /// 设置事件接收器（若存在，先删除）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <param name="eventTypes"></param>
        public static void SetEventReceivers(SPList list, Type t, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
            {
                SPEventReceiverDefinition def = list.EventReceivers[i];

                if (def.Class == className)
                    def.Delete();
            }

            foreach (SPEventReceiverType et in eventTypes)
            {
                list.EventReceivers.Add(et, assambly, className);
            }
            list.Update();
        }

        /// <summary>
        /// 设置列表的事件处理器
        /// </summary>
        /// <param name="list">要附加事件处理器的列表</param>
        /// <param name="t">事件处理器的类型</param>
        /// <param name="eventData">关联的数据</param>
        /// <param name="eventTypes">事件类型</param>
        public static void SetEventReceivers(SPList list, Type t, string eventData, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;
            //若制定类型的处理器已经存在则先删出
            for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
            {
                SPEventReceiverDefinition def = list.EventReceivers[i];

                if (def.Class == className)
                    def.Delete();
            }

            foreach (SPEventReceiverType et in eventTypes)
            {
                SPEventReceiverDefinition ef = list.EventReceivers.Add();

                ef.Assembly = assambly;
                ef.Class = className;
                ef.Type = et;
                ef.Data = eventData;
                ef.Update();
            }
        }

        /// <summary>
        /// 删除事件接收器
        /// </summary>
        /// <param name="list">要删除事件处理器的列表</param>
        /// <param name="t">要删除事件处理器类型</param>
        public static void RemoveEventReceivers(SPList list, Type t)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
            {
                SPEventReceiverDefinition def = list.EventReceivers[i];

                if (def.Class == className)
                    def.Delete();
            }

            list.Update();
        }

        /// <summary>
        /// 获取事件处理器定义
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public static SPEventReceiverDefinition GetEventDefinition(SPList list, Type t, SPEventReceiverType eventType)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
            {
                SPEventReceiverDefinition def = list.EventReceivers[i];

                if (def.Class == className && def.Type == eventType)
                    return def;
            }

            return null;
        }

        /// <summary>
        /// 添加事件接收器
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="t">事件处理器的类型</param>
        /// <param name="eventTypes">事件类型</param>
        public static void AddEventReceivers(SPList list, Type t, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            foreach (SPEventReceiverType et in eventTypes)
            {
                list.EventReceivers.Add(et, assambly, className);
            }

            list.Update();
        }

        public static void AddEventReceivers(SPList list, Type t, string eventData, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            foreach (SPEventReceiverType et in eventTypes)
            {
                SPEventReceiverDefinition ef = list.EventReceivers.Add();

                ef.Assembly = assambly;
                ef.Class = className;
                ef.Type = et;
                ef.Data = eventData;
                ef.Update();
            }
        }


    }
}
