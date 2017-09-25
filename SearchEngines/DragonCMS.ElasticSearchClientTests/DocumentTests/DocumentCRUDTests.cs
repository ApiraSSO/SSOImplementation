using System;
using System.Linq;
using System.Threading;
using DragonCMS.ElasticSearchClientTests.MockData;
using DragonCMS.ElasticSearchClientTests.MockDataFactories;
using Nest;
using NUnit.Framework;

namespace DragonCMS.ElasticSearchClientTests.DocumentTests
{
    [TestFixture]
    [Ignore("Infrastructure tests. No need to run.")]
    public class DocumentCRUDTests
    {
        [Test]
        public void CreateDocumentTest()
        {
            //ARRANGE
            var indexId = Guid.NewGuid();
           
            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            
            var index = new IndexName { Name = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId).ToLower(), Type = typeof(ParentTestClass) };
            
            var testClass = new ParentTestClass();
            testClass.Children.Add(new ChildClass());

            var documentPath = new DocumentPath<ParentTestClass>(testClass)
                .Index(index)
                .Type(typeof(ParentTestClass));

            var createRequest = new CreateRequest<ParentTestClass>(documentPath);

            //ACT
            try
            {
                var createResponse = client.Create(createRequest);
                Thread.Sleep(1000);

                var getResponse = client.Get<ParentTestClass>(documentPath);
                var document = getResponse.Source;
                //ASSERT
                Assert.IsTrue(createResponse.IsValid);
                Assert.IsTrue(getResponse.IsValid);
                Assert.AreEqual(testClass.Id, document.Id);
                Assert.AreEqual(testClass.Children.Count, document.Children.Count);
                Assert.AreEqual(testClass.Children.ElementAt(0).Id, document.Children.ElementAt(0).Id);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void UpdateDocument_load_update_commit_Test()
        {
            //ARRANGE
            
            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            var indexId = Guid.NewGuid();
            var index = new IndexName { Name = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId).ToLower(), Type = typeof(ParentTestClass) };
            
            var testClass = new ParentTestClass();
            testClass.Children.Add(new ChildClass());

            var documentPath = new DocumentPath<ParentTestClass>(testClass)
                .Index(index)
                .Type(typeof(ParentTestClass));
            
            //ACT
            try
            {
                //create a document
                var createRequest = new CreateRequest<ParentTestClass>(documentPath);
                var createResponse = client.Create(createRequest);
                Thread.Sleep(1000);

                //load the created document and assign a property and commit
                var getResponse = client.Get<ParentTestClass>(documentPath);
                var loadedDoc = getResponse.Source;
                var isChildNull = loadedDoc.Child == null;

                var childToAdd = new ChildClass();
                var documentToUpdate = loadedDoc;
                documentToUpdate.Child = childToAdd;

                var updateDocumentPath = new DocumentPath<ParentTestClass>(documentToUpdate)
                 .Index(index)
                 .Type(typeof(ParentTestClass));

                var updateRequest = new UpdateRequest<ParentTestClass, ParentTestClass>(updateDocumentPath);
                updateRequest.Doc = loadedDoc;
                
                var updateResponse = client.Update<ParentTestClass>(updateDocumentPath, d => d.Doc(loadedDoc));

                var getResponseUpdated = client.Get<ParentTestClass>(updateDocumentPath);
                var updatedDocument = getResponseUpdated.Source;

                //ASSERT
                Assert.IsTrue(createResponse.IsValid);
                Assert.IsTrue(getResponse.IsValid);
                Assert.AreEqual(testClass.Id, loadedDoc.Id);
                Assert.AreEqual(1, loadedDoc.Children.Count);
                Assert.IsTrue(isChildNull);
                Assert.AreEqual(testClass.Id, updatedDocument.Id);
                Assert.AreEqual(1, updatedDocument.Children.Count);
                Assert.NotNull(updatedDocument.Child);
                Assert.AreEqual(childToAdd.Id, updatedDocument.Child.Id);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void PartialUpdateDocument_by_doc()
        {
            //ARRANGE

            //set up search client
            
            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            
            //create an index name
            var indexId = Guid.NewGuid();
            var index = new IndexName { Name = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId).ToLower(), Type = typeof(ParentTestClass) };
            
            //create a parent class and add 2 children
            var testClass = new ParentTestClass();
            var child1 = new ChildClass();
            var child2 = new ChildClass();
            testClass.Children.Add(child1);
            testClass.Children.Add(child2);

            //create document path
            var documentPath = new DocumentPath<ParentTestClass>(testClass)
                .Index(index)
                .Type(typeof(ParentTestClass));

            //ACT
            try
            {
                //create a new parent class document
                var createRequest = new CreateRequest<ParentTestClass>(documentPath);
                var createResponse = client.Create(createRequest);
                //wait to be indexed
                Thread.Sleep(1000);

                //load the created document
                var getResponse = client.Get<ParentTestClass>(documentPath);
                var loadedDocAfterCreated = getResponse.Source;
                
                //create new new path on created parent id only
                var updateDocumentPath = new DocumentPath<ParentTestClass>(testClass.Id)
                 .Index(index)
                 .Type(typeof(ParentTestClass));

                //create an anonymous type to update property child and update parent document
                var childToUpdate = new ChildClass();
                var partialUpdate = new { Child = childToUpdate };
                var updateResponse = client.Update<ParentTestClass, dynamic>(updateDocumentPath, d => d.Doc(partialUpdate));

                //load updated document
                getResponse = client.Get<ParentTestClass>(updateDocumentPath);
                var loadedDocAfterUpdated = getResponse.Source;

                //ASSERT
                Assert.AreEqual(testClass.Id, loadedDocAfterCreated.Id);
                Assert.AreEqual(testClass.Id, loadedDocAfterUpdated.Id);
                Assert.AreEqual(2, loadedDocAfterCreated.Children.Count);
                Assert.AreEqual(2, loadedDocAfterUpdated.Children.Count);
                Assert.Null(loadedDocAfterCreated.Child);
                Assert.NotNull(loadedDocAfterUpdated.Child);
                Assert.AreEqual(childToUpdate.Id, loadedDocAfterUpdated.Child.Id);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void PartialUpdateDocument_by_script_add_to_collection()
        {
            //ARRANGE
            
            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();
            
            //create a new index
            var indexId = Guid.NewGuid();
            var index = new IndexName { Name = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId).ToLower(), Type = typeof(ParentTestClass) };
           
            //create a parent class and add 2 children
            var testClass = new ParentTestClass();
            var child1 = new ChildClass();
            var child2 = new ChildClass();
            
            testClass.Children.Add(child1);
            testClass.Children.Add(child2);

            //create document path
            var documentPath = new DocumentPath<ParentTestClass>(testClass)
                .Index(index)
                .Type(typeof(ParentTestClass));

            //ACT
            try
            {
                //create a new parent class document
                var createRequest = new CreateRequest<ParentTestClass>(documentPath);
                var createResponse = client.Create(createRequest);
                
                //wait to be indexed
                Thread.Sleep(1000);

                //load the created document
                var getResponse = client.Get<ParentTestClass>(documentPath);
                var loadedDocAfterCreated = getResponse.Source;

                //create new new path on created parent id only
                var updateDocumentPath = new DocumentPath<ParentTestClass>(testClass.Id)
                 .Index(index)
                 .Type(typeof(ParentTestClass));

                
                var child3 =  new ChildClass { Id = Guid.NewGuid(), ChildDateFiled = DateTime.Now, ChildEmail = "d.com", ChildIntField = 10, Name = "Somename"};

                var lang = "painless";
               
                var scriptToUpdate = "ctx._source.children.add(params.p)";
                var updateResponse = client.Update<ParentTestClass, dynamic>(updateDocumentPath,
                    d => d.Script(des =>
                    des.Inline(scriptToUpdate)
                    .Lang(lang)
                    .Params(p => p.Add("p", child3)))
                    .Upsert(testClass));

                //load updated document
                getResponse = client.Get<ParentTestClass>(updateDocumentPath);
                var loadedDocAfterUpdated = getResponse.Source;

                //ASSERT
                Assert.AreEqual(testClass.Id, loadedDocAfterCreated.Id);
                Assert.AreEqual(testClass.Id, loadedDocAfterUpdated.Id);
                Assert.AreEqual(2, loadedDocAfterCreated.Children.Count);
                Assert.AreEqual(3, loadedDocAfterUpdated.Children.Count);
                Assert.AreEqual(child1.Id, loadedDocAfterUpdated.Children.ElementAt(0).Id);
                Assert.AreEqual(child2.Id, loadedDocAfterUpdated.Children.ElementAt(1).Id);
                Assert.AreEqual(child3.Id, loadedDocAfterUpdated.Children.ElementAt(2).Id);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }

        [Test]
        public void PartialUpdateDocument_by_script_remove_from_collection()
        {
            //ARRANGE

            var client = SearchClientFactory.GetClient();
            var clientFactory = SearchClientFactory.GetClientFactory();

            //create a new index
            var indexId = Guid.NewGuid();
            var index = new IndexName { Name = String.Format("{0}_{1}", typeof(ParentTestClass).Name, indexId).ToLower(), Type = typeof(ParentTestClass) };

            //create a parent class and add 2 children
            var testClass = new ParentTestClass();
            var child1 = new ChildClass();
            var child2 = new ChildClass();
            testClass.TextCollection.Add("T1");
            testClass.TextCollection.Add("T2");
            testClass.Children.Add(child1);
            testClass.Children.Add(child2);

            //create document path
            var documentPath = new DocumentPath<ParentTestClass>(testClass)
                .Index(index)
                .Type(typeof(ParentTestClass));

            //ACT
            try
            {
                //create a new parent class document
                var createRequest = new CreateRequest<ParentTestClass>(documentPath);
                var createResponse = client.Create(createRequest);

                //wait to be indexed
                Thread.Sleep(1000);

                //load the created document
                var getResponse = client.Get<ParentTestClass>(documentPath);
                var loadedDocAfterCreated = getResponse.Source;

                //create new new path on created parent id only
                var updateDocumentPath = new DocumentPath<ParentTestClass>(testClass.Id)
                 .Index(index)
                 .Type(typeof(ParentTestClass));
                
                var lang = "painless";

                //var scriptToUpdate = "ctx._source.children.remove(params.p)";
                var scriptToUpdate = "ctx._source.children.remove(ctx._source.children.indexOf(params.p))";
                var updateResponse = client.Update<ParentTestClass, dynamic>(updateDocumentPath,
                    d => d.Script(des =>
                    des.Inline(scriptToUpdate)
                    .Lang(lang)
                    .Params(p => p.Add("p", child1))));
                    
                //load updated document
                getResponse = client.Get<ParentTestClass>(updateDocumentPath);
                var loadedDocAfterUpdated = getResponse.Source;

                //ASSERT
                Assert.AreEqual(testClass.Id, loadedDocAfterCreated.Id);
                Assert.AreEqual(testClass.Id, loadedDocAfterUpdated.Id);
                Assert.AreEqual(2, loadedDocAfterCreated.Children.Count);
                Assert.AreEqual(1, loadedDocAfterUpdated.Children.Count);
                Assert.AreEqual(child2.Id, loadedDocAfterUpdated.Children.ElementAt(0).Id);
            }
            finally
            {
                var deleteIndexResult = client.DeleteIndex(index);
            }
        }
    }
}