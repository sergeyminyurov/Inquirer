using Inquirer.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Inquirer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class EntityController<TEntity, TIdentity> : ControllerBase
        where TEntity : class, IEntity<TIdentity>, new()
    {
        protected InquirerDbContext DbContext { get; }
        public EntityController(InquirerDbContext db) { DbContext = db; }
        protected virtual IQueryable<TEntity> GetQuery([CallerMemberName] string methodName = null) => DbContext.Set<TEntity>();

        [HttpGet("list")]
        public async Task<List<TEntity>> GetAll()
        {
            List<TEntity> entities = await GetQuery().ToListAsync();
            return entities;
        }

        [HttpGet("{id}")]
        public async Task<TEntity> Get(TIdentity id)
        {
            TEntity entity = await GetQuery().FirstOrDefaultAsync(t => t.Id.Equals(id));
            return entity;
        }

        [HttpPost]
        public async Task<TEntity> AddNew([FromBody] TEntity entity)
        {
            NewEntity(entity);
            DbContext.Set<TEntity>().Add(entity);
            await DbContext.SaveChangesAsync();
            return await Get(entity.Id);
        }
        protected virtual void NewEntity(TEntity entity) { }

        [HttpPut]
        public async Task Update([FromBody] TEntity entity)
        {
            DbContext.Set<TEntity>().Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        protected virtual void DeleteDependencies(TIdentity id) { }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(TIdentity id)
        {
            try
            {
                DbContext.Remove<TEntity, TIdentity>(id, DeleteDependencies);
                await DbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{GetType().Name}.{nameof(Delete)}: Id={id}; Error={ex.GetBaseException().Message}");
                return false;
            }
        }
    }
}