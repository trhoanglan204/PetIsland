using Newtonsoft.Json.Linq;
using PetIsland.Models.ORS;
using PetIsland.DataAccess.Data;

namespace PetIslandWeb.Services.ORS;

#pragma warning disable IDE0290
public class GeocodingService
{
    private readonly HttpClient _httpClient;
    private string _apiKey = string.Empty;
    private readonly ApplicationDbContext _context;

    private static readonly string UrlSearch = "https://api.openrouteservice.org/geocode/search?api_key=";
    private static readonly string UrlDistance = "https://api.openrouteservice.org/v2/directions/driving-car?api_key=";
    //account.heigit.org/manage/key
    public GeocodingService(HttpClient httpClient, ApplicationDbContext context)
    {
        _httpClient = httpClient;
        _context = context;

        var key = _context.Contact.FirstOrDefault()?.ORS_Key;
        if (!string.IsNullOrEmpty(key))
        {
            _apiKey = key;
        }
    }
    public void SetKey(string key)
    {
        _apiKey = key;
    }

    public async Task<ORSprofile?> GeocodeSearchAsync(string address)
    {
        if (string.IsNullOrEmpty(_apiKey))
            return null;

        string encodedAddress = Uri.EscapeDataString(address);
        string url = $"{UrlSearch}{_apiKey}&text={encodedAddress}";
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

        return new ORSprofile { lon = lon, lat = lat};
    }
    public async Task<long> CalculateDistance(ORSprofile A, ORSprofile B)
    {
        if (string.IsNullOrEmpty(_apiKey))
            return 0;

        var from = $"{B.lon},{B.lat}";
        var to = $"{A.lon},{A.lat}";
        string url = $"{UrlDistance}{_apiKey}&start={from}&end={to}";

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
