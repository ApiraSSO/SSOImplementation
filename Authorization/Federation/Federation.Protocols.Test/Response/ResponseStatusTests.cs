using System;
using NUnit.Framework;
using Shared.Federtion.Constants;
using Shared.Federtion.Response;

namespace Federation.Protocols.Test.Response
{
    [TestFixture]
    internal class ResponseStatusTests
    {
        [Test]
        public void ResponseStatusTest_success_response()
        {
            //ARRANGE
            var expectedAggregatedMessage = String.Format("StatusCode: {0}\r\n", StatusCodes.Success);
            var responseStatus = new ResponseStatus();
            responseStatus.StatusCodes.Add(StatusCodes.Success);
            //ACT

            //ASSERT
            Assert.IsTrue(responseStatus.IsSuccess);
            Assert.AreEqual(StatusCodes.Success, responseStatus.StatusCodeMain);
            Assert.IsNull(responseStatus.AdittionalStatusCode);
            Assert.AreEqual(String.Empty, responseStatus.StatusMessage);
            Assert.AreEqual(String.Empty, responseStatus.MessageDetails);
            Assert.AreEqual(expectedAggregatedMessage, responseStatus.AggregatedMessage);
        }

        [Test]
        public void ResponseStatusTest_success_response_with_additional_status_code()
        {
            //ARRANGE
            var expectedAggregatedMessage = String.Format("StatusCode: {0}\r\nAdditional status code: {1}\r\n", StatusCodes.Success, StatusCodes.NoAuthnContext);
            var responseStatus = new ResponseStatus();
            responseStatus.StatusCodes.Add(StatusCodes.Success);
            responseStatus.StatusCodes.Add(StatusCodes.NoAuthnContext);
            //ACT

            //ASSERT
            Assert.IsTrue(responseStatus.IsSuccess);
            Assert.AreEqual(StatusCodes.Success, responseStatus.StatusCodeMain);
            Assert.AreEqual(StatusCodes.NoAuthnContext, responseStatus.AdittionalStatusCode);
            Assert.AreEqual(String.Empty, responseStatus.StatusMessage);
            Assert.AreEqual(String.Empty, responseStatus.MessageDetails);
            Assert.AreEqual(expectedAggregatedMessage, responseStatus.AggregatedMessage);
        }

        [Test]
        public void ResponseStatusTest_fail_response_with_additional_status_code_and_message()
        {
            //ARRANGE
            var expectedStatusMessage = "An error occured.";
            var expectedAggregatedMessage = String.Format("StatusCode: {0}\r\nAdditional status code: {1}\r\nMessage status: {2}\r\n", StatusCodes.Requester, StatusCodes.InvalidNameIdPolicy, expectedStatusMessage);
            
            var responseStatus = new ResponseStatus();
            responseStatus.StatusCodes.Add(StatusCodes.Requester);
            responseStatus.StatusCodes.Add(StatusCodes.InvalidNameIdPolicy);
            responseStatus.StatusMessage = "An error occured.";
            //ACT

            //ASSERT
            Assert.IsFalse(responseStatus.IsSuccess);
            Assert.AreEqual(StatusCodes.Requester, responseStatus.StatusCodeMain);
            Assert.AreEqual(StatusCodes.InvalidNameIdPolicy, responseStatus.AdittionalStatusCode);
            Assert.AreEqual(expectedStatusMessage, responseStatus.StatusMessage);
            Assert.AreEqual(String.Empty, responseStatus.MessageDetails);
            Assert.AreEqual(expectedAggregatedMessage, responseStatus.AggregatedMessage);
        }

        [Test]
        public void ResponseStatusTest_fail_response_with_additional_status_code_and_message_details()
        {
            //ARRANGE
            var expectedStatusMessage = "An error occured.";
            var expectedMessageDetails = "Some details";
            var expectedAggregatedMessage = String.Format("StatusCode: {0}\r\nAdditional status code: {1}\r\nMessage status: {2}\r\nMessage details: {3}", StatusCodes.Requester, StatusCodes.InvalidNameIdPolicy, expectedStatusMessage, expectedMessageDetails);

            var responseStatus = new ResponseStatus();
            responseStatus.StatusCodes.Add(StatusCodes.Requester);
            responseStatus.StatusCodes.Add(StatusCodes.InvalidNameIdPolicy);
            responseStatus.StatusMessage = "An error occured.";
            responseStatus.MessageDetails = expectedMessageDetails;
            //ACT

            //ASSERT
            Assert.IsFalse(responseStatus.IsSuccess);
            Assert.AreEqual(StatusCodes.Requester, responseStatus.StatusCodeMain);
            Assert.AreEqual(StatusCodes.InvalidNameIdPolicy, responseStatus.AdittionalStatusCode);
            Assert.AreEqual(expectedStatusMessage, responseStatus.StatusMessage);
            Assert.AreEqual(expectedMessageDetails, responseStatus.MessageDetails);
            Assert.AreEqual(expectedAggregatedMessage, responseStatus.AggregatedMessage);
        }
    }
}