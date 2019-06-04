using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SmileDirect.Web.Models;
using SmileDirect.Web.Services;
using SmileDirect.Web.Services.Launchpad;
using SmileDirect.Web.Test;
using Xunit;

namespace SmileDirect.Web.Test.Services.Launchpad
{
    public class SpaceXApiLaunchpadService_Test
    {
        private string SimpleContent { get; set; }
        private string ComplexContent { get; set; }
        public SpaceXApiLaunchpadService_Test()
        {
            SimpleContent = "[{\"id\": \"test\", \"full_name\": \"test_name\", \"status\": \"under construction\"}]";

            ComplexContent = @"[
                { ""id"": ""some_test1"", ""full_name"": ""some test1_name"", ""status"": ""active"" },
                { ""id"": ""some_test2"", ""full_name"": ""test2_name"", ""status"": ""retired""},
                { ""id"": ""test3"", ""full_name"": ""some test3_name"", ""status"": ""under construction"" },
                { ""id"": ""test4"", ""full_name"": ""test4_name"", ""status"": ""retired""}
            ]";
        }

        [Theory,  AutoMoqData]
        public async Task GetAllAsync_Returns_All_Results(
            [Frozen] Mock<IConfiguration> config,
            [Frozen] Mock<IHttpClientService> client,
            [Frozen] Mock<ILogger> logger
        )
        {
            var content = new StringContent(SimpleContent);
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = content;

            config.Setup(c => c[It.IsAny<string>()]).Returns("test");
            client.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var service = new SpaceXApiLaunchpadService(config.Object, client.Object, logger.Object);
            var response = (await service.GetAllAsync(null)).ToList();

            Assert.Equal(1, response.Count);
            Assert.Equal("test", response[0].Id);
            Assert.Equal("test_name", response[0].Name);
            Assert.Equal(LaunchpadStatus.UnderConstruction, response[0].Status);
        }

        [Theory, AutoMoqData]
        public async Task GetAllAsync_With_Single_Id_Filter_Returns_Proper_Results(
            [Frozen] Mock<IConfiguration> config,
            [Frozen] Mock<IHttpClientService> client,
            [Frozen] Mock<ILogger> logger
        )
        {
            var content = new StringContent(ComplexContent);
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = content;

            config.Setup(c => c[It.IsAny<string>()]).Returns("test");
            client.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var service = new SpaceXApiLaunchpadService(config.Object, client.Object, logger.Object);
            var response = (await service.GetAllAsync(new List<FilterModel>() { new FilterModel { Field = "id", Value = "some" } })).ToList();

            Assert.Equal(2, response.Count);
            Assert.Equal("some_test1", response[0].Id);
            Assert.Equal("some_test2", response[1].Id);
        }

        [Theory, AutoMoqData]
        public async Task GetAllAsync_With_Single_Name_Filter_Returns_Proper_Results(
            [Frozen] Mock<IConfiguration> config,
            [Frozen] Mock<IHttpClientService> client,
            [Frozen] Mock<ILogger> logger
        )
        {
            var content = new StringContent(ComplexContent);
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = content;

            config.Setup(c => c[It.IsAny<string>()]).Returns("test");
            client.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var service = new SpaceXApiLaunchpadService(config.Object, client.Object, logger.Object);
            var response = (await service.GetAllAsync(new List<FilterModel>() { new FilterModel { Field = "name", Value = "some" } })).ToList();

            Assert.Equal(2, response.Count);
            Assert.Equal("some test1_name", response[0].Name);
            Assert.Equal("some test3_name", response[1].Name);
        }

        [Theory, AutoMoqData]
        public async Task GetAllAsync_With_Single_Status_Filter_Returns_Proper_Results(
            [Frozen] Mock<IConfiguration> config,
            [Frozen] Mock<IHttpClientService> client,
            [Frozen] Mock<ILogger> logger
        )
        {
            var content = new StringContent(ComplexContent);
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = content;

            config.Setup(c => c[It.IsAny<string>()]).Returns("test");
            client.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var service = new SpaceXApiLaunchpadService(config.Object, client.Object, logger.Object);
            var response = (await service.GetAllAsync(new List<FilterModel>() { new FilterModel { Field = "status", Value = "retired" } })).ToList();

            Assert.Equal(2, response.Count);
            Assert.Equal("some_test2", response[0].Id);
            Assert.Equal("test4", response[1].Id);
        }

        [Theory, AutoMoqData]
        public async Task GetAllAsync_With_Compound_Filter_Returns_Proper_Results(
            [Frozen] Mock<IConfiguration> config,
            [Frozen] Mock<IHttpClientService> client,
            [Frozen] Mock<ILogger> logger
        )
        {
            var content = new StringContent(ComplexContent);
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = content;

            config.Setup(c => c[It.IsAny<string>()]).Returns("test");
            client.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var service = new SpaceXApiLaunchpadService(config.Object, client.Object, logger.Object);
            var response = (await service.GetAllAsync(new List<FilterModel>() {
                new FilterModel { Field = "status", Value = "retired" },
                new FilterModel { Field = "id", Value = "some" }
            })).ToList();

            Assert.Equal(1, response.Count);
            Assert.Equal("some_test2", response[0].Id);
        }
    }
}