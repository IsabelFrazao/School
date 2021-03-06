﻿using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            entity.isActive = true;

            await _context.Set<T>().AddAsync(entity);

            await SaveAllAsync();
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteAsync(T entity)
        {
            //_context.Set<T>().Remove(entity);

            entity.isActive = false;

            _context.Set<T>().Update(entity);

            await SaveAllAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);//e=Entity Genérica
        }

        public async Task UpdateAsync(T entity)
        {
            entity.isActive = true;

            _context.Set<T>().Update(entity);

            await SaveAllAsync();
        }
    }
}
