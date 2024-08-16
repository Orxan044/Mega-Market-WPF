using Mega_Market_Data.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mega_Market_Data.Repositoies;

public class Repository<TEntity, TContext> : IRepository<TEntity, TContext> where TEntity : BaseEntity where TContext : DbContext
{
    private readonly TContext _context;

    public Repository(TContext context)
    {
        _context = context;
    }

    DbSet<TEntity> Set => _context.Set<TEntity>();
    public void Add(TEntity entity)
    {
        Set.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Set.AddRange(entities);
    }

    public void Delete(int id)
    {
        var entity = Set.FirstOrDefault(x => x.Id == id);
        if (entity is not null)
            Set.Remove(entity);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public TEntity? Get(int id)
    {
        return Set.FirstOrDefault(x => x.Id == id);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        return Set.FirstOrDefault(predicate);
    }

    public IQueryable<TEntity> GetAll()
    {
        return Set;
    }

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
    {
        return Set.Where(predicate).ToList();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        Set.Update(entity);
    }
}

