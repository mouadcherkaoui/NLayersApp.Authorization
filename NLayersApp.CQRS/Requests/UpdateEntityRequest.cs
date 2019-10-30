using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayersApp.CQRS.Requests
{
    public class UpdateEntityRequest<TKey, TEntity>: IRequest<TEntity>
    {
        public TKey Key { get; }
        public TEntity Entity { get; }
        public UpdateEntityRequest(TKey key, TEntity entity)
        {
            Key = key;
            Entity = entity;
        }
    }
}
