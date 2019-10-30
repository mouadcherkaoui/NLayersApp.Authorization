using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayersApp.CQRS.Requests
{
    public class DeleteEntityRequest<TKey, TEntity>: IRequest<bool>
    {
        public TKey Key { get; }
        public DeleteEntityRequest(TKey key)
        {
            Key = key;
        }
    }
}
