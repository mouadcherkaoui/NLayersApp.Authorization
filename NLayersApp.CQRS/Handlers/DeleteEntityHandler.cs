using MediatR;
using NLayersApp.CQRS.Requests;
using NLayersApp.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLayersApp.CQRS.Handlers
{
    public class DeleteEntityHandler<TKey, TEntity> : IRequestHandler<DeleteEntityRequest<TKey, TEntity>, bool>
        where TEntity: class
    {
        IContext innerDataContext { get; }
        public DeleteEntityHandler(IContext context)
        {
            innerDataContext = context;
        }
        public async Task<bool> Handle(DeleteEntityRequest<TKey, TEntity> request, CancellationToken cancellationToken)
        {
            var entityToDelete = await innerDataContext.Set<TEntity>().FindAsync(request.Key);

            var entry = innerDataContext.Set<TEntity>().Remove(entityToDelete);
            await innerDataContext.SaveChangesAsync(cancellationToken);

            return entry.Property<bool>("IsDeleted").CurrentValue;
        }
    }
}
