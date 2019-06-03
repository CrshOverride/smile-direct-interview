using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Configuration;
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
        [Theory,  AutoMoqData]
        public async Task GetAllAsync_Returns_All_Results(
            [Frozen] Mock<IConfiguration> config,
            [Frozen] Mock<IHttpClientService> client
        )
        {
            var content = new StringContent("[{\"id\": \"test\", \"full_name\": \"test_name\", \"status\": \"under construction\"}]");
            var responseMessage = new HttpResponseMessage();
            responseMessage.Content = content;

            config.Setup(c => c[It.IsAny<string>()]).Returns("test");
            client.Setup(c => c.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(responseMessage));

            var service = new SpaceXApiLaunchpadService(config.Object, client.Object);
            var response = (await service.GetAllAsync()).ToList();

            Assert.Equal(1, response.Count);
            Assert.Equal("test", response[0].Id);
            Assert.Equal("test_name", response[0].Name);
            Assert.Equal(LaunchpadStatus.UnderConstruction, response[0].Status);
        }
    }
}