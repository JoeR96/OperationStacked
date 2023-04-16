using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using OperationStacked.Requests;

namespace OperationStackedTests.Functional;

[TestFixture
]
public class UserTests : BaseApiTest
{
    private const string updateUrl = "/user/update";
    private const string registerUrl = "/auth/register";
    private const string userName = "username";
    private const string password = "password";
    private const string email = "email@email.com";
    private const string loginUrl = "/auth/login";
    private const string getGetCurrentWeekAndDay = "week-and-day/";

    [Test]
    public async Task CurrentDayIncreasesIfWorkoutsRemainInWeek()
    {
        // Act
        var request = new RegistrationRequest()
        {
            UserName = userName,
            EmailAddress = email,
            Password = password
        };
        await _client.PostAsJsonAsync(registerUrl
            , request);


        var loginresponse = await _client.PostAsJsonAsync(loginUrl, request);
        var id = await loginresponse.Content.ReadAsStringAsync();

        var deserializeObject = JsonConvert.DeserializeObject<jsonObject>(id);
        //var userId = id.Data.First().UserId;
        var myObject = "1";

        JsonContent content = JsonContent.Create(myObject);

        await _client.PostAsync(updateUrl, content);
        var newWeekAndDay = await _client.PostAsJsonAsync(loginUrl, request);

        var id2 = await newWeekAndDay.Content.ReadAsStringAsync();

        var deserializeObject2 = JsonConvert.DeserializeObject<jsonObject>(id2);

        var result = deserializeObject2.Data.CurrentDay - deserializeObject.Data.CurrentDay;
        result.Should().Be(1);
    }
    
    //[Test]
    //public async Task CurrentWeekIncreasesIfDayIsEqualToTotalDays()
    //{
    //    // Act
    //    var request = new RegistrationRequest()
    //    {
    //        UserName = userName,
    //        EmailAddress = email,
    //        Password = password
    //    };
    //   await _client.PostAsJsonAsync(registerUrl
    //        , request);


    //    var loginresponse = await _client.PostAsJsonAsync(loginUrl, request);
    //    var id = await loginresponse.Content.ReadAsStringAsync();

    //    var deserializeObject = JsonConvert.DeserializeObject<jsonObject>(id);
    //    //var userId = id.Data.First().UserId;
    //    var myObject = "1";

    //    JsonContent content = JsonContent.Create(myObject);

    //    await _client.GetAsync(getGetCurrentWeekAndDay + deserializeObject.Data.UserId);
    //    await _client.PostAsync(updateUrl, content);
    //    await _client.PostAsync(updateUrl, content);
    //    var currentWeekAndDay = await _client.PostAsync(updateUrl, content);

    //    var newWeekAndDay = await _client.PostAsJsonAsync(loginUrl, request);

    //    var id2 = await newWeekAndDay.Content.ReadAsStringAsync();

    //    var deserializeObject2 = JsonConvert.DeserializeObject<jsonObject>(id2);

    //    var result = deserializeObject2.Data.CurrentWeek - deserializeObject.Data.CurrentWeek;
        
    //    result.Should().Be(1);
    //}
}

public class FakeContent : HttpContent
{
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        throw new NotImplementedException();
    }

    protected override bool TryComputeLength(out long length)
    {
        throw new NotImplementedException();
    }
}

public class jsonObject
{
    public LoginDataResponse Data { get; set; }
}

public class LoginDataResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
    public int CurrentDay { get; set; }
    public int CurrentWeek { get; set; }
    public string UserName { get; set; }
}