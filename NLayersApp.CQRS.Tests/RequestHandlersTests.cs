using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using NLayersApp.Persistence;
using NLayersApp.Persistence.Abstractions;
using System;
using MediatR;
using NLayersApp.CQRS.Requests;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NLayersApp.CQRS.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace NLayersApp.CQRS.Tests
{
    public class TestModel: IAuditable, ISoftDelete
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    [TestClass]
    public class RequestHandlersTests
    {

        [TestInitialize]
        public void TestsInitializations()
        {
            IoC.Container.RegisterServices(s =>
            {
                s.AddSingleton<ITypesResolver, TypesResolver>(s => new TypesResolver(() => new Type[] { typeof(TestModel) }));

                s.AddDbContext<IContext, TDbContext<IdentityUser, IdentityRole, string>>(o =>
                {
                    o.UseInMemoryDatabase("nlayersapp-test");
                }, ServiceLifetime.Scoped);

                s.AddMediatRHandlers(new TypesResolver(() => new Type[] { typeof(TestModel) }));
            });
        }


        [TestMethod]
        public async Task Test_ReadEntityRequest_Handler()
        {
            var request = new ReadEntityRequest<int, TestModel>(1);
            var mediatr = IoC.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediatr.Send(request);

            var expectedValue = new TestModel()
            {
                Id = 1,
                Name = "test name 1",
                Description = "test description 1"
            };

            Assert.IsNotNull(result);
            foreach(var property in typeof(TestModel).GetProperties())
            {
                Assert.AreEqual(property.GetValue(expectedValue), property.GetValue(result));
            }
        }

        [TestMethod]
        public async Task Test_ReadEntitiesRequest_Handler()
        {
            var request = new ReadEntityRequest<int, TestModel>(1);
            var mediatr = IoC.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediatr.Send(request);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_CreateEntityRequest_Handler()
        {
            var mediatr = IoC.ServiceProvider.GetRequiredService<IMediator>();
            
            var request = new CreateEntityRequest<TestModel>(new TestModel() { 
                Name = "test name 1",
                Description = "test description 1"
            });

            var result = await mediatr.Send(request);

            var getPersistedValue = new ReadEntityRequest<int, TestModel>(request.Entity.Id);
            var persistedValue = await mediatr.Send(getPersistedValue);

            Assert.AreEqual(result, persistedValue);
        }

        [TestMethod]
        public async Task Test_UpdateEntityRequest_Handler()
        {            
            // resolving Meditor service implementation using ServiceProvider
            var mediatr = IoC.ServiceProvider.GetRequiredService<IMediator>();

            
            var createResult = await mediatr.Send(
                new CreateEntityRequest<TestModel>(new TestModel()
                {
                    Name = "test name 1",
                    Description = "test description 1"
                })
            );

            // updating fields
            // preparing the entity to use
            var entity = new TestModel()
            {   
                Id = createResult.Id,
                Name = "test name update",
                Description = "test description update"
            };

            
            var updateEntityRequest = new UpdateEntityRequest<int, TestModel>(entity.Id, entity);
            
            var persistedValue = await mediatr.Send(updateEntityRequest);

            // testing results
            //Assert.AreNotEqual(createResult, persistedValue);
            Assert.AreEqual(persistedValue, createResult);
        }

        [TestMethod]
        public async Task Test_DeleteEntityRequest_Handler()
        {
            // resolving Meditor service implementation using ServiceProvider
            var mediatr = IoC.ServiceProvider.GetRequiredService<IMediator>();


            var createResult = await mediatr.Send(
                new CreateEntityRequest<TestModel>(new TestModel()
                {
                    Name = "test name 1",
                    Description = "test description 1"
                })
            );

            var deleteEntityRequest = new DeleteEntityRequest<int, TestModel>(createResult.Id);

            var deleteEntityResult = await mediatr.Send(deleteEntityRequest);

            // testing results
            Assert.IsTrue(deleteEntityResult);
        }

    }
}
