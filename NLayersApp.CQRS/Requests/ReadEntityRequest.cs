using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace NLayersApp.CQRS.Requests
{
    public class ReadEntityRequest<TKey, TEntity>: IRequest<TEntity>
    {
        public TKey Key { get; }
        public ReadEntityRequest(TKey key)
        {
            Key = key;
        }
    }
}
