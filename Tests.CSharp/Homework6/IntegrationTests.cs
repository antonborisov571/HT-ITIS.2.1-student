using Hw6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tests.RunLogic.Attributes;

namespace Tests.CSharp.Homework6;

public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<App.Startup>>
{
    private readonly HttpClient _httpClient;

    public IntegrationTests(CustomWebApplicationFactory<App.Startup> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Homework(Homeworks.HomeWork6)]
    public async Task TestRouteResponse()
    {
        // Act
        var request = await _httpClient.GetAsync("/");
        var response = await request.Content.ReadAsStringAsync();

        // Assert
        Assert.True(request.IsSuccessStatusCode);
        Assert.Equal("Use //calculate?value1=<VAL1>&operation=<OPERATION>&value2=<VAL2>", response);
    }

    [HomeworkTheory(Homeworks.HomeWork6)]
    [InlineData("/calculate")]
    [InlineData("/calculate?operation=Plus&val2=0")]
    [InlineData("/calculate?value1=0&value2=0")]
    [InlineData("/calculate?value1=2&operation=Plus")]
    public async Task TestRouteWithoutParam(string uri)
    {
        // arrange
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        // act
        var response = await _httpClient.SendAsync(request);
        var responseStr = await response.Content.ReadAsStringAsync();

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("parameter is not passed", responseStr);
    }

    [Homework(Homeworks.HomeWork6)]
    public async Task TestCalculateRoute()
    {
        // arrange
        var req = new HttpRequestMessage(HttpMethod.Get, "/calculate?value1=1000&operation=Minus&value2=7");

        // act
        var response = await _httpClient.SendAsync(req);
        var responseStr = await response.Content.ReadAsStringAsync();

        // assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("993", responseStr);
    }

    [Homework(Homeworks.HomeWork6)]
    public async Task TestNonExistentRouteError()
    {
        // act
        var response = await _httpClient.GetAsync("/NonExistent");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
