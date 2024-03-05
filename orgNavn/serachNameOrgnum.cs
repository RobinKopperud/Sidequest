using Newtonsoft.Json;


namespace egetnavn.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseUrl = "https://data.brreg.no/enhetsregisteret/api/enheter/";

        [HttpGet]
        public async Task<IActionResult> Get(string inputfraweb?)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            string url = IsOrgNumber(query) ? $"{baseUrl}{query}" : $"{baseUrl}?navn={query}";

            try
            {
                string response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<dynamic>(response);
                //resultat må mulig endres?
                return Ok(result);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, "Error contacting the external API.");
            }
        }

        private bool IsOrgNumber(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
