using MediatR;
using NLayersApp.CQRS.Requests;
using NLayersApp.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLayersApp.CQRS.Handlers
{
    public class CreateEntityHandler<TKey, TEntity> : IRequestHandler<CreateEntityRequest<TEntity>, TEntity>
        where TEntity: class
    {
        IContext innerDataContext { get; }
        public CreateEntityHandler(IContext context)
        {
            innerDataContext = context;
        }
        public async Task<TEntity> Handle(CreateEntityRequest<TEntity> request, CancellationToken cancellationToken)
        {
            var resultToReturn = (await innerDataContext.Set<TEntity>().AddAsync(request.Entity)).Entity;
            await innerDataContext.SaveChangesAsync(cancellationToken);
            return resultToReturn;
        }
    }
}
