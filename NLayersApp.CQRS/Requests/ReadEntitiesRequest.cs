using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayersApp.CQRS.Requests
{
    public class ReadEntitiesRequest<TEntity>: IRequest<IEnumerable<TEntity>>
    {
        public string[] PropertiesToInclude { get; }
        public ReadEntitiesRequest(params string[] propertiesToInclude)
        {
            PropertiesToInclude = propertiesToInclude;
        }
    }
}
