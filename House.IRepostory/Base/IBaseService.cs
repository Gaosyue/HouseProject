using House.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace House.IRepository
{
    /// <summary>
    /// 通用的泛型基类
    /// 泛型约束： 类、基类、公共无参类、接口、struct
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public interface IBaseService<T> where T : class
    {
        #region 查询

        /// <summary>
        /// 获取用于从整个表中检索实体的IQueryable
        /// </summary>
        /// <returns>可用于从数据库中选择实体</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// 用于获取所有实体
        /// </summary>
        /// <returns>所有实体列表</returns>
        List<T> GetAllList();

        /// <summary>
        /// 用于获取所有实体的异步实现
        /// </summary>
        /// <returns>所有实体列表</returns>
        Task<List<T>> GetAllListAsync();

        /// <summary>
        /// 用于获取传入本方法的所有实体
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        /// <returns>所有实体列表</returns>
        List<T> GetAllList(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 用于获取传入本方法的所有实体
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        /// <returns>所有实体列表</returns>
        Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 用于获取传入本方法的所有实体
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        /// <returns>所有实体列表</returns>
        Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate, EntityBase entityBase);

        /// <summary>
        /// 通过传入的筛选条件来获取实体信息
        /// 如果查询不到返回值，则会引发异常
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Single(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件来获取实体信息
        /// 如果查询不到返回值，则会引发异常
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件查询实体信息，如果没有找到，则返回null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件查询实体信息，如果没有找到，则返回null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        #endregion 查询

        #region 添加

        /// <summary>
        /// 添加一个新实体
        /// </summary>
        /// <param name="entity">被添加的实体</param>
        /// <returns></returns>
        bool Insert(T entity);

        /// <summary>
        /// 添加一个新实体
        /// </summary>
        /// <param name="entity">被添加的实体</param>
        /// <returns></returns>
        Task<bool> InsertAsync(T entity);

        #endregion 添加

        #region 修改

        /// <summary>
        /// 更新现有实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 更新现有实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        #endregion 修改

        #region 删除

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">无返回值</param>
        bool Delete(T entity);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">无返回值</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// 传入的条件可删除多个实体
        /// 注意：所有符合给定条件的实体都将被检索和删除
        /// 如果条件比较多，则待删除的实体也比较多，则可能会导致主要的性能问题
        /// </summary>
        /// <param name="predicate"></param>
        bool Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 传入的条件可删除多个实体
        /// 注意：所有符合给定条件的实体都将被检索和删除
        /// 如果条件比较多，则待删除的实体也比较多，则可能会导致主要的性能问题
        /// </summary>
        /// <param name="predicate"></param>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);

        #endregion 删除

        #region 总和计算

        /// <summary>
        /// 获取此仓储中所有实体的总和
        /// </summary>
        /// <returns>实体的总数</returns>
        int Count();

        /// <summary>
        /// 获取此仓储中所有实体的总和
        /// </summary>
        /// <returns>实体的总数</returns>
        Task<int> CountAsync();

        /// <summary>
        /// 支持条件筛选计算仓储中的实体总和
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>实体的总数</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 支持条件筛选计算仓储中的实体总和
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>实体的总数</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 获取此存储库中所有的实体的总和，如果返回值大于了Int.MaxValue值，则推荐该方法
        /// </summary>
        /// <returns>实体的总和</returns>
        long LongCount();

        /// <summary>
        /// 获取此存储库中所有的实体的总和，如果返回值大于了Int.MaxValue值，则推荐该方法
        /// </summary>
        /// <returns>实体的总和</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// 获取此存储库中所有的实体的总和，如果返回值大于了Int.MaxValue值，则推荐该方法
        /// </summary>
        /// <returns>实体的总和</returns>
        long LongCount(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 获取此存储库中所有的实体的总和，如果返回值大于了Int.MaxValue值，则推荐该方法
        /// </summary>
        /// <returns>实体的总和</returns>
        Task<long> LongCountAsync(Expression<Func<T, bool>> predicate);

        #endregion 总和计算
    }
}