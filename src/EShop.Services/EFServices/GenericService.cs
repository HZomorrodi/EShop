using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : BaseEntity, new()
    {
        public GenericService(IUnitOfWork uow)
        {
            _uow = uow;
            _entity = _uow.Set<TEntity>();
        }

        private readonly IUnitOfWork _uow;
        private readonly DbSet<TEntity> _entity;

        public void Add(TEntity entity)
        {
            _entity.Add(entity);
        }

        public TEntity FindById(int id)
        {
            return _entity.Find(id);
        }

        public void Remove(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _entity.Update(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _entity.FindAsync(id);
        }

        public void Remove(int id)
        {
            var tEntity = new TEntity();
            var idProperty = typeof(TEntity).GetProperty("Id") ?? throw new Exception("The entity doesn't have Id field!");
            idProperty.SetValue(tEntity, id, null);
            _uow.MarkAsDeleted(tEntity);

        }

        public List<TEntity> GetAll()
        {
            return [.. _entity];
        }
    }
}
