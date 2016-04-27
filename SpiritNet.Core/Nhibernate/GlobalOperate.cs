using SpiritNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Nhibernate
{
    public class GlobalOperate
    {
        /// <summary>
        /// 强制同步session数据到数据库
        /// </summary>
        public static void Flush()
        {
            NHibernateSessionManager.Instance.GetSession().Flush();
        }
        /// <summary>
        /// 强制清空session数据
        /// </summary>
        public static void Clear()
        {
            NHibernateSessionManager.Instance.GetSession().Clear();
        }
        /// <summary>
        /// 强制关闭session
        /// </summary>
        public static void Close()
        {
            NHibernateSessionManager.Instance.GetSession().Close();
        }
        /// <summary>
        /// 事务开始
        /// </summary>        
        [Obsolete("换用Attribute方式")]
        public static void BeginChanges()
        {
            NHibernateSessionManager.Instance.BeginTransaction();
        }
        /// <summary>
        /// 事务提交
        /// </summary>

        [Obsolete("换用Attribute方式")]
        public static void CommitChanges()
        {
            NHibernateSessionManager.Instance.CommitTransaction();
        }
        /// <summary>
        /// 事务回滚
        /// </summary>

        [Obsolete("换用Attribute方式")]
        public static void RollbackChanges()
        {
            NHibernateSessionManager.Instance.RollbackTransaction();
        }
        /// <summary>
        /// 从session中移除指定对象
        /// </summary>
        /// <param name="obj"></param>
        public static void Evict(object obj)
        {
            NHibernateSessionManager.Instance.GetSession().Evict(obj);
        }
        /// <summary>
        /// 合并session中与obj标识id相同的对象。
        /// </summary>
        /// <param name="obj"></param>
        public static void Merge(object obj)
        {
            NHibernateSessionManager.Instance.GetSession().Merge(obj);
        }
        /// <summary>
        /// 判断session中是否存在指定对象。
        /// </summary>
        /// <param name="obj"></param>
        public static bool Contain(object obj)
        {
            return NHibernateSessionManager.Instance.GetSession().Contains(obj);
        }
        /// <summary>
        /// 判断session中是否存在指定对象。
        /// </summary>
        /// <param name="obj"></param>
        public static void Refresh(object obj)
        {
            NHibernateSessionManager.Instance.GetSession().Refresh(obj);
        }
    }
}
