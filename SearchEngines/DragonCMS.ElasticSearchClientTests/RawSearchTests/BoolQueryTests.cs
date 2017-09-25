using System;
using System.Linq;
using System.Threading;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.RawSearchTests
{
    [TestFixture]
    [Ignore("Infrastructure tests. Needed to test elastic search functionality. No need to run.")]
    public class BoolQueryTests
    {
        [Test]
        public void Bool_must_query_test()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            //set up search client
            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();

            //create index name
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            try
            {

                var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.Properties(p => p.Keyword(d => d.Name(n => n.Email)))));
                    //m.AutoMap()));

                var createIndexRequest = client.CreateIndex(indexDescriptor);

                var testClass = new ParentTestClass { IntField = 0, Email = "Text0"};
                testClass.Children.Add(new ChildClass());

                var testClass1 = new ParentTestClass { IntField = 1, Email = "Text1" };
                testClass1.Children.Add(new ChildClass());

                var testClass2 = new ParentTestClass { IntField = 2, Email = "Text2" };
                testClass2.Children.Add(new ChildClass());

                var testClass3 = new ParentTestClass { IntField = 3, Email = "Text3" };
                testClass2.Children.Add(new ChildClass());

                var createDocumentResponse = client.Create(testClass, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                var createDocumentResponse1 = client.Create(testClass1, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                var createDocumentResponse2 = client.Create(testClass2, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                var createDocumentResponse3 = client.Create(testClass3, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                Thread.Sleep(1000);
                var i = client.GetIndex(new GetIndexRequest(index));
                //ACT
                //Search
                var searchMatchResponse = client
                   .Search<ParentTestClass>(s =>
                   s.Query(q =>
                            q.Bool(b =>
                            b.Should(must =>
                                    must.Term(term =>
                                                term.Field(field => field.Email)
                                                .Value("Text1"))
                                   ,must => 
                                   must.Term(term => 
                                                term.Field(f => f.Email).
                                                Value("Text2"))
                                  , must =>
                                    must.Term(term =>
                                                 term.Field(f => f.Email).
                                                 Value("Text3")))
                                    .Filter(filter =>
                                             filter.Range(range => 
                                                            range.Field(field => field.IntField)
                                                            .GreaterThan(1)
                                                            .LessThan(10)))
                                    )
                            )
                            .Index(index));

                
                //ASSERT match response
                Assert.IsTrue(searchMatchResponse.IsValid);
                Assert.AreEqual(1, searchMatchResponse.Documents.Count());
                //Assert.AreEqual(testClass.Id, searchMatchResponse.Documents.First().Id);
                //Assert.AreEqual(1, searchMatchResponse.Documents.First().Children.Count());
                //Assert.AreEqual(testClass.Children.First().Id, searchMatchResponse.Documents.First().Children.First().Id);

                //ASSERT term response
                //Assert.IsTrue(searchTermResponse.IsValid);
                //Assert.AreEqual(1, searchTermResponse.Documents.Count());
                //Assert.AreEqual(testClass.Id, searchTermResponse.Documents.First().Id);
                //Assert.AreEqual(1, searchTermResponse.Documents.First().Children.Count());
                //Assert.AreEqual(testClass.Children.First().Id, searchTermResponse.Documents.First().Children.First().Id);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void Search_by_email_mapped_term_search_Test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();
            var client = SearchClientFactory.GetClient();

            //create index name
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            //ACT
            try
            {
                var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.AutoMap()
                    .Properties(prop => prop.Nested<ChildClass>(o =>
                                        o.Name(parent => parent.Child))
                                        )
                   .Properties(prop => prop.Keyword(kw => kw.Name(parent => parent.Email)))));
                var createIndexRequest = client.CreateIndex(indexDescriptor);

                var testClass = new ParentTestClass { Email = "email@domain.com" };
                testClass.Children.Add(new ChildClass());
                var createDocumentResponse = client.Create(testClass, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();
                var mappings = first.Value.Mappings.First()
                    .Value.Properties.Select(p => new { p.Key.Name, p.Value.Type, PropertyName = p.Value.Name });
                //ACT
                //Search
                var searchResponse = client
                   .Search<ParentTestClass>(s =>
                   s.Query(q =>
                            q.Term(m => m.Field(g => g.Email)
                                    .Value("email@domain.com")))
                   .Index(index));

                var allRecords = client
                    .Search<ParentTestClass>(q => q
                    .Index(Indices.Index(index)));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(testClass.Id, searchResponse.Documents.First().Id);
                Assert.AreEqual(testClass.Email, searchResponse.Documents.First().Email);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void Search_by_email_not_mapped_term_search_Test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();
            var client = SearchClientFactory.GetClient();

            //create index name
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            //ACT
            try
            {
                var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.AutoMap()
                    .Properties(prop => prop.Nested<ChildClass>(o =>
                                        o.Name(parent => parent.Child))
                                        )));
                var createIndexRequest = client.CreateIndex(indexDescriptor);

                var testClass = new ParentTestClass { Email = "email@domain.com" };
                testClass.Children.Add(new ChildClass());
                var createDocumentResponse = client.Create(testClass, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();
                var mappings = first.Value.Mappings.First()
                    .Value.Properties.Select(p => new { p.Key.Name, p.Value.Type, PropertyName = p.Value.Name });
                //ACT
                //Search
                var searchResponse = client
                   .Search<ParentTestClass>(s =>
                   s.Query(q =>
                            q.Term(m => m.Field(g => g.Email)
                                    .Value("email@domain.com")))
                   .Index(index));

                var allRecords = client
                    .Search<ParentTestClass>(q => q
                    .Index(Indices.Index(index)));

                //ASSERT
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(0, searchResponse.Documents.Count());
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void Search_by_email_not_mapped_match_term_comparison_search_Test()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var dependencyResolver = new DependencyResolverMock();
            var client = SearchClientFactory.GetClient();

            //create index name
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            //ACT
            try
            {
                var indexDescriptor = new CreateIndexDescriptor(index)
                    .Mappings(map => map.Map<ParentTestClass>(m =>
                    m.AutoMap()
                    .Properties(prop => prop.Nested<ChildClass>(o =>
                                        o.Name(parent => parent.Child))
                                        )));
                var createIndexRequest = client.CreateIndex(indexDescriptor);

                var email = "foreName.lastName@domain.com";
                var testClass = new ParentTestClass { Email = email };
                testClass.Children.Add(new ChildClass());
                var createDocumentResponse = client.Create(testClass, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

                Thread.Sleep(1000);
                var indexReqiest = new GetIndexRequest(index);
                var indexResponse = client.GetIndex(indexReqiest);
                var indices = indexResponse.Indices.ToList();
                var first = indices.First();
                var mappings = first.Value.Mappings.First()
                    .Value.Properties.Select(p => new { p.Key.Name, p.Value.Type, PropertyName = p.Value.Name });
                //ACT
                //Search
                var searchResponse = client
                   .Search<ParentTestClass>(s =>
                   s.Query(q =>
                            q.Match(m => m.Field(g => g.Email)
                                    .Query(email)))
                   .Index(index));

                var searchResponse_partial = client
                   .Search<ParentTestClass>(s =>
                   s.Query(q =>
                            q.Match(m => m.Field(g => g.Email)
                                    .Query("foreName.lastName")))
                   .Index(index));

                var searchResponse_full_term = client
                   .Search<ParentTestClass>(s =>
                   s.Query(q =>
                            q.Term(m => m.Field(g => g.Email)
                                    .Value(email)))
                   .Index(index));
                
                //ASSERT match full search response
                Assert.IsTrue(searchResponse.IsValid);
                Assert.AreEqual(1, searchResponse.Documents.Count());
                Assert.AreEqual(testClass.Id, searchResponse.Documents.First().Id);
                Assert.AreEqual(testClass.Email, searchResponse.Documents.First().Email);

                //ASSERT partial match search response
                Assert.IsTrue(searchResponse_partial.IsValid);
                Assert.AreEqual(1, searchResponse_partial.Documents.Count());
                Assert.AreEqual(testClass.Id, searchResponse_partial.Documents.First().Id);
                Assert.AreEqual(testClass.Email, searchResponse_partial.Documents.First().Email);

                //ASSERT full term search response
                Assert.IsTrue(searchResponse_full_term.IsValid);
                Assert.AreEqual(0, searchResponse_full_term.Documents.Count());
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void SearchNestedTypeTest()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            //set up search client
            var dependencyResolver = new DependencyResolverMock();

            var client = SearchClientFactory.GetClient();

            //create index name
            var indexName = String.Format("{0}_{1}", typeof(ParentTestClass).Name, id).ToLower();
            var index = new IndexName { Name = indexName, Type = typeof(ParentTestClass) };

            try
            {
                var indexDescriptor = new CreateIndexDescriptor(index)
                   .Mappings(map => map.Map<ParentTestClass>(m =>
                   m.AutoMap()
                   .Properties(p => p.Nested<ChildClass>(n => n.Name(parent => parent.Children)))));

                var createIndexRequest = client.CreateIndex(indexDescriptor);

                var testClass = new ParentTestClass();
                testClass.Children.Add(new ChildClass());
                var createDocumentResponse = client.Create(testClass, d => d.Index(index)
                .Type(typeof(ParentTestClass)));

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
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }
    }
}
