using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using PetIsland.Models.ORS;

namespace PetIslandWeb.Services.ORS;

#pragma warning disable IDE0290
public class GeocodingService
{
    private readonly HttpClient _httpClient;
    private string? ORS_Key { get; set; }
    private static readonly string UrlSearch = "https://api.openrouteservice.org/geocode/search?api_key=";
    private static readonly string UrlDistance = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=";

    public GeocodingService(HttpClient httpClient, IConfiguration _config)
    {
        _httpClient = httpClient;
        ORS_Key = _config.GetValue<string>("ORS:Key");
    }

    public async Task<(double lon, double lat)?> GeocodeSearchAsync(string address)
    {
        string encodedAddress = Uri.EscapeDataString(address);
        string url = $"{UrlSearch}{ORS_Key}&text={encodedAddress}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        JObject result = JObject.Parse(json);

        var features = result["features"];
        if (features == null || !features.Any())
            return null;

        var coords = features[0]["geometry"]?["coordinates"];
        if (coords == null)
            return null;

        double lon = coords[0]?.Value<double>() ?? 0;
        double lat = coords[1]?.Value<double>() ?? 0;

        return (lon, lat);
    }
    public async Task<long> CalculateDistance(ORSprofile user, ORSprofile company)
    {
        var companyAddr = $"{company.lon},{company.lat}";
        var userAddr = $"{user.lon},{user.lat}";
        string url = $"{UrlDistance}{ORS_Key}&start={companyAddr}&end={userAddr}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return 0;

        var json = await response.Content.ReadAsStringAsync();
        JObject result = JObject.Parse(json);
        var features = result["features"];
        if (features == null || !features.Any())
            return 0;

        var summary = features[0]["properties"]?["summary"];
        if (summary == null)
            return 0;

        var length = summary[0]?.Value<long>() ?? 0;

        return length;
    }

}
