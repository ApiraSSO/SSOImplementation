using System;
using System.IO;
using System.Net;
using Data.Importing.Infrastructure.Contexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Importing.Tests.ContextTests
{
    [TestClass]
    [Ignore]
    public class SourceContextTests
    {
        [TestMethod]
        public void CreateFileRequestTest()
        {
            //ASSERT
            var path = @"localhost/Test.txt";
            var schema = @"ftp://";
            var uri = new Uri(schema + path);
            //ACT
            var request = WebRequest.Create(uri);
            request.Credentials = new NetworkCredential("Test", "Password1");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            var buffer = new byte[ms.Length];
            ms.Read(buffer, 0, (int)ms.Length);
            string fileString = System.Text.Encoding.UTF8.GetString(buffer);
            ///ASSERET
            //Assert.IsInstanceOfType(webRequest, typeof(FileWebRequest));
        }

        [TestMethod]
        public void CreateWebRequestTest()
        {
            //ASSERT
            var path = @"google.com";
            var schema = @"http://";
            var uri = new Uri(schema + path);
            //ACT
            var request = WebRequest.Create(uri);
            request.Credentials = new NetworkCredential("Test", "Password1");
            request.Method = WebRequestMethods.Http.Get;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Position = 0;
            var buffer = new byte[ms.Length];
            ms.Read(buffer, 0, (int)ms.Length);
            string fileString = System.Text.Encoding.UTF8.GetString(buffer);
            ///ASSERET
            //Assert.IsInstanceOfType(webRequest, typeof(FileWebRequest));
        }

        [TestMethod]
        public void GetStreamFileRequestTest()
        {
            //ASSERT
            var path = @"D://FtpExchange/Test.txt";
            var schema = @"file://";
            var uri = new Uri(schema + path);
            //ACT
            var request = WebRequest.Create(uri);
            request.Credentials = new NetworkCredential("Test", "Password1");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            var response = request.GetResponse();
            
            var stream = response.GetResponseStream();

            //var ms = new MemoryStream();
            //stream.CopyTo(ms);
            //ms.Position = 0;
            var buffer = new byte[response.ContentLength];
            stream.Read(buffer, 0, (int)response.ContentLength);
            string fileString = System.Text.Encoding.UTF8.GetString(buffer);
        }

        [TestMethod]
        public void GetStreamFileRequestTest1()
        {
            //ASSERT
            var path = @"D://FtpExchange/Test.txt";
            var schema = @"file://";
            var uri = new Uri(schema + path);
            //ACT
            var request = WebRequest.CreateDefault(uri);
            request.Credentials = new NetworkCredential("Test", "Password1");
            request.Method = WebRequestMethods.File.DownloadFile;
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            string fileString = System.Text.Encoding.UTF8.GetString(buffer);
        }
    }
}