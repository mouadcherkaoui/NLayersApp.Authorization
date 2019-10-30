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
    public class ReadEntityHandler<TKey, TEntity> : IRequestHandler<ReadEntityRequest<TKey, TEntity>, TEntity>
        where TEntity: class
    {
        IContext innerDataContext { get; }
        public ReadEntityHandler(IContext context)
        {
            innerDataContext = context;
        }
        public async Task<TEntity> Handle(ReadEntityRequest<TKey, TEntity> request, CancellationToken cancellationToken)
        {
            var resultToReturn = await innerDataContext.Set<TEntity>().FindAsync(request.Key);
            return resultToReturn;
        }
    }
}
