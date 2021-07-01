using Jeremy.IdentityCenter.Database.Common.Extensions;
using Jeremy.IdentityCenter.Database.Common.Models;
using Jeremy.IdentityCenter.Database.Common.Repositories.Interfaces;
using Jeremy.Tools.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Jeremy.IdentityCenter.Database.Common.Repositories
{
    public abstract class BaseRepository<TEntity, TContext, TRepository> : Repository<TContext, TRepository>, IBaseRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
        where TRepository : IBaseRepository<TEntity>
    {
        protected BaseRepository(TContext db, ILogger<TRepository> logger) : base(db, logger)
        {
        }

        #region 查

        public virtual TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            return Db.Set<TEntity>().FirstOrDefault(expression);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await Db.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public virtual List<TEntity> GetRange(Expression<Func<TEntity, bool>> expression = null)
        {
            return Db.Set<TEntity>().WhereIf(expression != null, expression).ToList();
        }

        public virtual async Task<List<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return await Db.Set<TEntity>().WhereIf(expression != null, expression).ToListAsync();
        }

        public virtual PageList<TEntity> GetRange<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10,
            bool isDescending = false)
        {
            var res = Db.Set<TEntity>().Where(expression).PageBy(orderBy, page, pageSize, isDescending).ToList();
            return res.ToPageList(res.Count, pageSize);
        }

        public virtual async Task<PageList<TEntity>> GetRangeAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, int page = 1, int pageSize = 10, bool isDescending = false)
        {
            return (await Db.Set<TEntity>().Where(expression).PageBy(orderBy, page, pageSize, isDescending).ToListAsync())
                .ToPageList(await Db.Set<TEntity>().Where(expression).CountAsync(), pageSize);
        }

        public async Task<PageList<TEntity>> GetRangeAsync<TParam, TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, TParam param = null, int page = 1, int pageSize = 10,
            bool isDescending = false) where TParam : class
        {
            var res = await Db.Set<TEntity>().Where(expression).ToListAsync();
            if (param != null)
                res = param.ToDictionary()
                    .Aggregate(res, (current, o) => current
                        .Where(x => Equals(x.GetType().GetProperty(o.Key)?.GetValue(x), o.Value)).AsQueryable()
                        .PageBy(orderBy, page, pageSize, isDescending)
                        .ToList());

            return res.ToPageList(res.Count, pageSize);
        }

        public abstract Task<PageList<TEntity>> GetRangeAsync(Expression<Func<TEntity, bool>> expression, int page,
            int pageSize = 10);

        public virtual async Task<TAccessory> GetAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TEntity>> includeExpression = null) where TAccessory : class, new()
        {
            return await Db.Set<TAccessory>().IncludeIf(includeExpression).FirstOrDefaultAsync(expression);
        }

        public virtual async Task<PageList<TAccessory>> GetAccessoryRangeAsync<TAccessory, TKey>(Expression<Func<TAccessory, bool>> expression, Expression<Func<TAccessory, TKey>> orderBy, int page, int pageSize) where TAccessory : class, new()
        {
            return (await Db.Set<TAccessory>().Where(expression).PageBy(orderBy, page, pageSize).ToListAsync())
                .ToPageList(await Db.Set<TAccessory>().Where(expression).CountAsync(), pageSize);
        }

        #endregion


        #region 增

        public virtual bool Add(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Add(entity);
            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            if (entity != null) await Db.Set<TEntity>().AddAsync(entity);
            return await Db.SaveChangesAsync() > 0;
        }

        public virtual bool AddRange(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().AddRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                await Db.Set<TEntity>().AddRangeAsync(entities);
            }

            return await Db.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new()
        {
            if (entity != null) await Db.Set<TAccessory>().AddAsync(entity);
            return await Db.SaveChangesAsync() > 0;
        }

        #endregion



        #region 改

        public virtual bool Update(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Update(entity);

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Update(entity);

            return await Db.SaveChangesAsync() > 0;
        }

        public virtual bool UpdateRange(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().UpdateRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().UpdateRange(entities);
            }

            return await Db.SaveChangesAsync() > 0;
        }

        #endregion




        #region 删

        public virtual bool Delete(TEntity entity)
        {
            if (entity != null) Db.Set<TEntity>().Remove(entity);

            return Db.SaveChanges() > 0;
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> expression)
        {
            var entity = Get(expression);
            if (entity != null) Db.Set<TEntity>().Remove(entity);

            return Db.SaveChanges() > 0;
        }

        public virtual bool DeleteRange(IEnumerable<TEntity> entities)
        {
            entities = entities.ToList();
            if (entities.Any())
            {
                Db.Set<TEntity>().RemoveRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> expression)
        {
            var entities = GetRange(expression);
            if (entities.Any())
            {
                Db.Set<TEntity>().RemoveRange(entities);
            }

            return Db.SaveChanges() > 0;
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = await GetRangeAsync(expression);
            if (entities.Any())
            {
                Db.Set<TEntity>().RemoveRange(entities);
            }

            return await Db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAccessoryAsync<TAccessory>(TAccessory entity) where TAccessory : class, new()
        {
            if (entity != null) Db.Set<TAccessory>().Remove(entity);

            return await Db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAccessoryAsync<TAccessory>(Expression<Func<TAccessory, bool>> expression) where TAccessory : class, new()
        {
            var entity = await GetAccessoryAsync(expression);
            if (entity != null) Db.Set<TAccessory>().Remove(entity);
            return await Db.SaveChangesAsync() > 0;
        }

        #endregion



        public virtual bool Save()
        {
            try
            {
                return Db.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync()
        {
            try
            {
                return await Db.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
        }
    }
}