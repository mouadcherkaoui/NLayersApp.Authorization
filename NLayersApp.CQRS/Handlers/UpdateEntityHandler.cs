using MediatR;
using Microsoft.EntityFrameworkCore;
using NLayersApp.CQRS.Requests;
using NLayersApp.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLayersApp.CQRS.Handlers
{
    public class UpdateEntityHandler<TKey, TEntity> : IRequestHandler<UpdateEntityRequest<TKey, TEntity>, TEntity>
        where TEntity: class
    {
        IContext innerDataContext { get; }
        public UpdateEntityHandler(IContext context)
        {
            innerDataContext = context;
        }
        public async Task<TEntity> Handle(UpdateEntityRequest<TKey, TEntity> request, CancellationToken cancellationToken)
        {
            var resultToReturn = await innerDataContext.Set<TEntity>().FindAsync(request.Key);
            var entityType = innerDataContext.Model.FindRuntimeEntityType(typeof(TEntity));//.FirstOrDefault(e => e.Name == );
                //.FindEntityType(typeof(TEntity).Name);

            var keyProperties = entityType
                .FindPrimaryKey()?.Properties.Select(p => p.PropertyInfo) ?? new PropertyInfo[] { };
                
            foreach(var property in typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(request.Entity);
                if (!(value is null) && keyProperties.Contains(property)) 
                    property.SetValue(resultToReturn, value);
            }
            await innerDataContext.SaveChangesAsync(cancellationToken);
            return resultToReturn;
        }
    }
}
