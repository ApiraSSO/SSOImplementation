using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DragonCMS.ElasticSearchClient.ErrorHandling;
using DragonCMS.ElasticSearchClient.IndexAPI;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests
{
    internal class Test
    {
        public virtual Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping> F(Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping> next)
        {
            Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping> func = 
                d => 
                {
                    var nextResult = next(d) as TypeMappingDescriptor<ParentTestClass>;
                    var mapping = nextResult.Properties(x => x.Nested<ChildClass>(n => n.Name(p => p.Children)));
                    return mapping;
                };
            return func;
        }
    }
    internal class Test1 : Test
    {
        public override Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping> F(Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping> next)
        {
            Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping> func =
                d =>
                {
                    var nextResult = next(d) as TypeMappingDescriptor<ParentTestClass>;
                    if (nextResult == null)
                        nextResult = d;
                    var mapping = nextResult.Properties(x => x.Nested<ChildClass>(n => n.Name(p => p.Child)));
                    return mapping;
                };
            return func;
        }
    }

    [TestFixture]
    internal class MiscellaneousTests
    {
        [Test]
        public void Test()
        {
            //ARRANGE

            //set up search client
            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            SearchClientFactory.RegisterDependencies(dependencyResolver);

            //delete existing index
            var index = new IndexName { Name = typeof(ParentTestClass).Name.ToLower(), Type = typeof(ParentTestClass) };
            var deleteIndexResult = client.DeleteIndex(Indices.Index(index));

            //set up document CRUD controller and create a mock document
            var responseHandler = new ResponseHandler();
            var indexManager = new IndexManager(dependencyResolver, clientFactory, responseHandler);
            //var documentclient = new DocumentController(clientFactory, indexManager);

            //dependencyResolver.RegisterFactory<IndexMapper<ParentTestClass>>(t => new TestIndexMapper(), Lifetime.Transient);

            var testClass = new ParentTestClass();
            testClass.Children.Add(new ChildClass());
            //documentclient.CreateDocument(testClass);
            this.CreateDocument(testClass, index, client);

            var indexMeta = client.GetIndex(Indices.Index(index));
            Thread.Sleep(1000);
            //ACT
            //Search
            var searchResponse = client
               .Search<ParentTestClass>(s =>
               s.Query(q =>
                       q.Nested(nq =>
                       nq.Path(p => p.Children)
                       .Query(oq =>
                                 oq.Match(m => m.Field(g => g.Children.First().Name)
                                 .Query(testClass.Children.First().Name.ToString()))).IgnoreUnmapped()))
               .Index(Indices.Index(index)));

            var allRecords = client
                .Search<ParentTestClass>(q => q
                .Index(Indices.Index(index)));

            //ASSERT
            Assert.IsTrue(searchResponse.IsValid);
            Assert.AreEqual(1, searchResponse.Documents.Count());
            Assert.AreEqual(testClass.Id, searchResponse.Documents.First().Id);
            Assert.AreEqual(1, searchResponse.Documents.First().Children.Count());
            Assert.AreEqual(testClass.Children.First().Id, searchResponse.Documents.First().Children.First().Id);
        }

        private void CreateDocument(ParentTestClass document, IndexName index, ElasticClient client)
        {
            //var index = new IndexName { Name = "TestName", Type = typeof(ParentTestClass) };
            var descriptor = new CreateIndexDescriptor(index);
            var t = new Test();

            var t1 = new Test1();

            Func<Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping>, CreateIndexDescriptor> seed = f =>
            {
                descriptor = descriptor.Mappings(m => m.Map<ParentTestClass>(f));
                return descriptor;
            };

            var list = new List<Test> { t, t1 };

            var res = list.Aggregate(seed, (x, next) =>
            {
                //throw new NotImplementedException();

                var returnRes = new Func<Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping>, CreateIndexDescriptor>
                (
                    pr =>
                    {
                        x(next.F(pr));

                        return descriptor;
                        throw new NotImplementedException();
                    });
                return returnRes;
            });

            var ex = res(new Func<TypeMappingDescriptor<ParentTestClass>, ITypeMapping>(q => null));

            client.CreateIndex(descriptor);
            var documentType = document.GetType();
            //var client = this._clientFactory.GetClient();

            var documentPath = new DocumentPath<ParentTestClass>(document);
            //var index = this.GetIndex(document, client);

            documentPath.Type(documentType);
            documentPath.Index(index);

            var createRequest = new CreateRequest<ParentTestClass>(documentPath);

            var response = client.Create(createRequest);
        }
    }
}