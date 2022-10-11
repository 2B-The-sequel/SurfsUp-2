using System.Text.Json;

namespace SurfsUp.Models.Repositories
{
    public class EquipmentRepo
    {

        private async static Task<List<Equipment>> GetEquipmentFromAPI()
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            List<Equipment> equipment;

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            //Hent Equipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Equipment"))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    equipment = JsonSerializer.Deserialize<List<Equipment>>(jsonResponse, options)!;
                    return equipment;
                }
                else
                {
                    throw new Exception("The response from api/Equipment failed to succeed");
                }
            }
        }
    }
}
