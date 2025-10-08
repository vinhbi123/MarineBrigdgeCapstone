using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Models.Authentication;
using ShipCapstone.Domain.Models.Settings;

namespace ShipCapstone.Application.Services.Implements;

public class OAuthService : IOAuthService
{
    private readonly OAuthSettings _settings;
    private readonly HttpClient _httpClient;

    public OAuthService(IOptions<OAuthSettings> options, HttpClient httpClient)
    {
        _settings = options.Value;
        _httpClient = httpClient;
    }

    public async Task<GoogleUserDto> GetUserByCode(string code, bool isAndroid = false)
    {
        var values = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", isAndroid ? _settings.ClientIdAndroid : _settings.ClientId },
            { "grant_type", "authorization_code" }
        };
        
        if (!isAndroid)
        {
            values.Add("client_secret", _settings.ClientSecret);
            values.Add("redirect_uri", _settings.RedirectUrl);
        }

        var response = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(values));
        var responseString = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Google token request failed: {response.StatusCode}, {responseString}");
        }

        var token = await response.Content.ReadFromJsonAsync<GoogleTokenResponse>();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token.IdToken);

        return new GoogleUserDto
        {
            Email = jwtToken.Claims.First(c => c.Type == "email").Value,
            Name = jwtToken.Claims.First(c => c.Type == "name").Value,
            Picture = jwtToken.Claims.First(c => c.Type == "picture").Value
        };
    }
}