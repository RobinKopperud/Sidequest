using Newtonsoft.Json;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string baseUrl = "https://data.brreg.no/enhetsregisteret/api/enheter/";

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Enter a name or organization number to search:");
            string input = Console.ReadLine();

            // Sjekk om input er et organisasjonsnummer (kun tall)
            if (IsOrgNumber(input))
            {
                await SearchByOrgNo(input);
            }
            else // Anta at det er et navn
            {
                await SearchByName(input);
            }
        }
    }

    static bool IsOrgNumber(string input)
    {
        // Enkel validering for å sjekke om input kun inneholder tall
        return int.TryParse(input, out _);
    }

    static async Task SearchByOrgNo(string orgNo)
    {
        string url = $"{baseUrl}{orgNo}";
        await PerformSearch(url);
    }

    static async Task SearchByName(string name)
    {
        string url = $"{baseUrl}?navn={name}";
        await PerformSearch(url);
    }

    static async Task PerformSearch(string url)
    {
        try
        {
            string response = await client.GetStringAsync(url);
            var result = JsonConvert.DeserializeObject<dynamic>(response);
            Console.WriteLine(result);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
        }
    }
}
