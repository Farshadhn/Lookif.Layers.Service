﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lookif.Layers.Core.Infrastructure;
using Lookif.Layers.Core.Infrastructure.Base;
using Lookif.Layers.Core.Infrastructure.Base.Repositories;
using Lookif.Layers.Core.MainCore.Base; 
using Microsoft.EntityFrameworkCore;
namespace Lookif.Layers.Service.Services.Base;

public class BaseService<T, J> : IBaseService<T, J>
where T : class, IEntity<J>
{

    private readonly IRepository<T, J> repository;

    public BaseService(IRepository<T, J> repository)
    {
        this.repository = repository;
    }


    public virtual async Task<T> AddAsync(T t, CancellationToken cancellationToken, bool save = true)
    {
        await repository.AddAsync(t, cancellationToken, save);
        return t;
    }

    public virtual T Add(T t, bool save = true)
    {
        repository.Add(t, save);
        return t;
    }


    public virtual async Task<T> UpdateAsync(T t, CancellationToken cancellationToken, bool save = true)
    {
        await repository.UpdateAsync(t, cancellationToken, save);
        return t;
    }

    public virtual async Task<T> UpdateViaIdAsync(J t, CancellationToken cancellationToken, bool save = true)
    { 
        var res = await GetByIdAsync(t, cancellationToken);
        await repository.UpdateAsync(res, cancellationToken, save);
        return res;
    }


    public virtual async Task<T> DeleteAsync(T t, CancellationToken cancellationToken, bool save = true)
    {
        await repository.DeleteAsync(t, cancellationToken, save);
        return t;
    }
    public virtual IQueryable<T> GetAll()
    {
        return repository.TableNoTracking.OrderByDescending(x => x.LastEditedDateTime);
    }

    public virtual async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return await GetAll().ToListAsync(cancellationToken); ;
    }
  
    public virtual async Task<List<T>> GetAllByCondition(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        return await repository.TableNoTracking.Where(condition).OrderByDescending(x=>x.LastEditedDateTime).ToListAsync(cancellationToken);
    }
    public virtual IQueryable<T> QueryByCondition(Expression<Func<T, bool>> condition)
    {
        return repository.TableNoTracking.Where(condition).AsQueryable<T>();
    }
    public virtual async Task<T> ExistAny(Expression<Func<T, bool>> condition, CancellationToken cancellationToken)
    {
        return await repository.TableNoTracking.Where(condition).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<T> GetByIdAsync(J id, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(cancellationToken, id);
    }

    public virtual async Task<IEnumerable<T>> AddRangeAsync(List<T> t, CancellationToken cancellationToken, bool save = true)
    {
        await repository.AddRangeAsync(t, cancellationToken, save);
        return t;

    }

    public IQueryable<T> GetTemporal<Temporal>() where Temporal : ITemporal, T
    {
        return repository.GetTemporal<Temporal>();
    }

    public Task<List<T>> GetTemporal<Temporal>(CancellationToken cancellationToken) where Temporal : ITemporal, T
    {
        return repository.GetTemporal<Temporal>(cancellationToken);
    }
}






public class BaseService<T> : BaseService<T, Guid>
    where T : class, IEntity<Guid>
{
    private readonly IRepository<T> repository;

    public BaseService(IRepository<T> repository) : base(repository)
    {
        this.repository = repository;
    }
}
