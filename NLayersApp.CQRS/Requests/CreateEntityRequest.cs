using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayersApp.CQRS.Requests
{
    public class CreateEntityRequest<TEntity>: IRequest<TEntity>
    {
        public TEntity Entity { get; }
        public CreateEntityRequest(TEntity entity)
        {
            Entity = entity;
        }
    }
}
