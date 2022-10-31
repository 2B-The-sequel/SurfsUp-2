using System.Text.Json;

namespace SurfsUp.Models.Repositories
{
    public class EquipmentRepo
    {
      
        public async static Task<Equipment> Create(Equipment equipment)
        {
            return await Request(HttpMethod.Post, equipment);
        }

        public static async Task<Equipment> Retrieve(Equipment equipment)
        {
            return await Request(HttpMethod.Get, equipment);
        }

        public async static Task<List<Equipment>> Retrieve()
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
            using HttpResponseMessage response = await client.GetAsync("api/Equipment");
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

        public async static Task<Equipment> Update(Equipment equipment)
        {
            return await Request(HttpMethod.Put, equipment);
        }

        public async static Task<Equipment> Delete(Equipment equipment)
        {
            return await Request(HttpMethod.Delete, equipment);
        }

        private async static Task<T> Request<T>(HttpMethod method, T item) where T : IIdentifiable
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            HttpRequestMessage message;
            if (method == HttpMethod.Post || method == HttpMethod.Put)
            {
                message = new(method, "api/Equipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
                HttpContent content = new StringContent(JsonSerializer.Serialize(item));
                message.Content = content;
            }
            else
            {
                message = new(method, $"api/Equipment?Id={item.Id}&apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
            }

            //Hent Equipment fra API
            using HttpResponseMessage response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonResponse, options)!;
            }
            else
            {
                throw new Exception("The response from api/Equipment failed to succeed");
            }

            return default(T);
        }
    }
}