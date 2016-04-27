using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Interfaces
{
    public interface IBLLCommon<TEntity, TID>
    {
        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns>IList{`1}.</returns>
        IList<TEntity> GetAll();
        /// <summary>
        /// 根据TID抓取一条记录
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns>返回需要查询的记录</returns>
        TEntity GetByID(TID Key);
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <returns>返回保存的记录</returns>
        TID Save(TEntity Entity);
        /// <summary>
        /// 保存一个List集合
        /// </summary>
        /// <param name="Entities">The entities.</param>
        /// <returns>返回一个主键的集合</returns>
        IList<TEntity> Save(IList<TEntity> Entities);
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="Entity">The entity.</param>
        void Delete(TEntity Entity);
        /// <summary>
        /// 根据id来删除一条记录
        /// </summary>
        /// <param name="id">实体主键.</param>
        /// <param name="keyName">Name of the key.</param>
        int Delete(TID id, string keyName);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="TEntity"></param>
        /// <returns></returns>
        void Delete(IList<TEntity> Entities);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <returns>`1.</returns>
        TEntity Update(TEntity Entity);
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Update(IList<TEntity> entities);
        /// <summary>
        /// 保存或更新一条记录
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        TEntity SaveOrUpdate(TEntity Entity);

        /// <summary>
        /// 合并同一个Id的2个对象，当使用Update发生错误 different object with the same identifier value was already associated with the session
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        object Merge(object entity);
        /// <summary>
        /// 保存或更新多条记录
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>IList{`1}.</returns>
        IList<TEntity> SaveOrUpdate(IList<TEntity> entities);
        /// <summary>
        /// 将泛型IList<>类型的集合，转换为List<>类型的集合。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listObjects"></param>
        /// <returns></returns>
        List<T> ConvertToGenericList<T>(IList<T> listObjects);
        int GetCount<T>(string property, T value);
        //IList<TEntity> GetByQuery(string queryString);
        //IList<TEntity> GetByCriteria(params ICriterion[] criterion);
        /// <summary>
        /// 根据关键字,获取相关表信息
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>IList{`1}.</returns>
        IList<TEntity> GetByKeyWords(string propertyName, object value, params string[] eagerName);


        IList<TEntity> GetByCriteria(params ICriterion[] criterion);

        IList<TEntity> GetByCriteria(
          Dictionary<string, NHibernate.SqlCommand.JoinType> foreigntables,
          ICriterion[] criterion = null,
          Order[] orderby = null,
          int returnnum = 0);


        IList<TEntity> GetByCriteria(Action<ICriteria> criteriaBuilder);

    }
}
