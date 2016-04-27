using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Interfaces
{
    internal interface IDALCommon<TEntity, TID>
    {
        /// <summary>
        /// 抓取所有的表中的所有记录
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();
        /// <summary>
        /// 根据id抓取一条流程记录
        /// </summary>
        /// <param name="commonID"></param>
        /// <returns></returns>
        TEntity GetById(TID commonID);
        /// <summary>
        /// 增加一条流程记录
        /// </summary>
        /// <param name="commonEntity"></param>
        /// <returns></returns>
        TID Save2(TEntity commonEntity);
        /// <summary>
        /// 删除一条流程记录
        /// </summary>
        /// <param name="commonEntity"></param>
        /// <returns></returns>
        void Delete(TEntity commonEntity);
        /// <summary>
        /// 删除一个集合
        /// </summary>
        /// <param name="entities"></param>
        void Delete(List<TEntity> entities);
        /// <summary>
        /// 根据主键id删除一条记录
        /// </summary>
        /// <param name="id"></param>
        void Delete(TID id);
        /// <summary>
        /// 更新一条流程记录
        /// </summary>
        /// <param name="commonEntity"></param>
        /// <returns></returns>
        TEntity Update(TEntity commonEntity);
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entites"></param>
        void Update(List<TEntity> entites);
        /// <summary>
        /// 保存或更新一条流程记录
        /// </summary>
        /// <param name="commonEntity"></param>
        /// <returns></returns>
        TEntity SaveOrUpdate(TEntity commonEntity);

        List<TEntity> GetByQuery(string queryString);
        List<TEntity> GetByCriteria(params ICriterion[] criterion);
    }
}
