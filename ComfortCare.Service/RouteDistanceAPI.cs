using ComfortCare.Domain.BusinessLogic;
using System.Text;
using System.Text.Json;

namespace ComfortCare.Service
{
    /// <summary>
    /// The class is used to get the distance from one point to another.
    /// </summary>
    public class RouteDistanceAPI : IDistance
    {
        private readonly HttpClient _httpClient;

        public RouteDistanceAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// This method returns the distance from a to b in seconds and meters in a float datatype.
        /// </summary>
        /// <param name="startLongitude"></param>
        /// <param name="startLatitude"></param>
        /// <param name="endLongitude"></param>
        /// <param name="endLatitude"></param>
        /// <returns>Tuple floats that contains the distance in seconds and meters</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<(double DistanceInSeconds, double DistanceInMeters)> GetDistance(string startLongitude, string startLatitude, string endLongitude, string endLatitude)
        {
            string _apiUrl = "http://192.168.3.73:8080/ors/v2/directions/driving-car";
            var jsonPayload = $"{{\"coordinates\":[[{startLongitude},{startLatitude}],[{endLongitude},{endLatitude}]],\"instructions\":\"false\",\"preference\":\"recommended\"}}";
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(_apiUrl, content);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody, options);

            if (double.TryParse(jsonResponse.Routes[0].Summary.Duration, out var distanceInSeconds) &&
                double.TryParse(jsonResponse.Routes[0].Summary.Distance, out var distanceInMeters))
            {
                return (distanceInSeconds, distanceInMeters);
            }

            throw new InvalidOperationException("Failed to parse API response.");
        }

        // these records are only used for the open api for getting the distance and duration
        private record ApiResponse(List<Route> Routes, Metadata Metadata);

        private record Route(RouteSummary Summary);

        private record RouteSummary(string Distance, string Duration);

        private record Metadata(Query Query);

        private record Query(List<List<double>> Coordinates, string Profile, string Preference);
    }    
}
