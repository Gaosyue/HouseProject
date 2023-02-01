using House.Core;
using House.IRepository;
using House.Model;
using Microsoft.EntityFrameworkCore;

//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace House.Repository
{
    // <summary>
    // 通用泛型基类
    //</summary>
    // <typeparam name = "T" ></ typeparam >
    public class BaseService<T> : IBaseService<T> where T : class
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private readonly MyDbConText _db;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public BaseService(MyDbConText db)
        {
            _db = db;
        }

        /// <summary>
        /// 通过泛型，从数据库上下文中获取领域模型
        /// </summary>
        public virtual DbSet<T> Table => _db.Set<T>();

        public IQueryable<T> GetAll()
        {
            return Table.AsQueryable();
        }

        public List<T> GetAllList()
        {
            return GetAll().ToList();
        }

        public async Task<List<T>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public List<T> GetAllList(Expression<Func<T, bool>> predicate)
        {
            //lambda  匿名函数
            //var pre = (a) => { m=>m.Id==a };
            return GetAll().Where(predicate).ToList();
        }

        /// <summary>
        /// 表达式树  链式表达式
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllListAsync(Expression<Func<T, bool>> predicate, EntityBase entityBase)
        {
            var result = await GetAll().Where(predicate).ToListAsync();
            return await GetAll().Where(predicate).OrderBy(m => entityBase.Id).Skip((entityBase.PageIndex - 1) * entityBase.PageSize).Take(entityBase.PageSize).ToListAsync();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            //  var model =  goods.Where(m=>m.id==1);
            //  var model =  goods.FirstOrDefault(m=>m.id==1);
            //  var count =  goods.count(m=>m.id==1);

            return GetAll().FirstOrDefault(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public bool Insert(T entity)
        {
            var newEntity = Table.Add(entity).Entity;
            return Save();
        }

        public async Task<bool> InsertAsync(T entity)
        {
            var newEntity = (await Table.AddAsync(entity)).Entity;
            return await SaveAsync();
        }

        public bool Update(T entity)
        {
            AttachIfNot(entity);
            _db.Entry(entity).State = EntityState.Modified;

            return Save();
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            AttachIfNot(entity);
            _db.Entry(entity).State = EntityState.Modified;
            return await SaveAsync();
        }

        public bool Delete(T entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            return Save();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            return await SaveAsync();
        }

        public bool Delete(Expression<Func<T, bool>> predicate)
        {
            var result = false;
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                result = Delete(entity);
            }
            return result;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var result = false;
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                result = await DeleteAsync(entity);
            }
            return result;
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public async Task<long> LongCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        protected bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        /// <summary>
        /// 检查实体是否处于跟踪状态，如果是，则返回；如果不是，则添加跟踪状态
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void AttachIfNot(T entity)
        {
            var entry = _db.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        /// <summary>
        /// 提交异步事务
        /// </summary>
        /// <returns></returns>
        protected async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}