using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinancialStructures.Stocks.Persistence.Database
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// Add an entity if the entity does not exist based on the predicate.
        /// </summary>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
        /// <returns></returns>
        public static EntityEntry<TEntity> AddIfNotExists<TEntity>(this DbSet<TEntity> dbSet, TEntity entity, Expression<Func<TEntity, bool>> predicate = null)
            where TEntity : class
        {
            bool exists = predicate != null 
                ? dbSet.Any(predicate) 
                : dbSet.Any();
            return !exists 
                ? dbSet.Add(entity) 
                : null;
        }
    }
}