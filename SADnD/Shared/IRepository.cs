﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<bool> Delete(TEntity entityToDelete);
        Task<bool> Delete(object id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetByID(object id);
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entityToUpdate);
    }
}
