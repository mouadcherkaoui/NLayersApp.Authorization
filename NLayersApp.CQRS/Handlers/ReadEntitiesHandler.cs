using MediatR;
using Microsoft.EntityFrameworkCore;
using NLayersApp.CQRS.Requests;
using NLayersApp.Persistence.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NLayersApp.CQRS.Handlers
{
    public class ReadEntitiesHandler<TEntity> : IRequestHandler<ReadEntitiesRequest<TEntity>, IEnumerable<TEntity>>
        where TEntity: class
    {
        IContext innerDataContext { get; }
        public ReadEntitiesHandler(IContext context)
        {
            innerDataContext = context;
        }
        public Task<IEnumerable<TEntity>> Handle(ReadEntitiesRequest<TEntity> request, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> initialSet = innerDataContext.Set<TEntity>();
            if (request.PropertiesToInclude != null && request.PropertiesToInclude.Length > 0)
                request.PropertiesToInclude.ToList().ForEach(p => initialSet = initialSet.Include(p));
            return Task.FromResult(initialSet.AsEnumerable<TEntity>());
        }
    }
}
