using System.Text;
using System.Text.Json;

namespace SurfsUpLibrary.Models.Repositories
{
    public class RentalRepo
    {
        public async static Task<List<Rental>> GetAllFromAPI()
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            List<Rental> rentals;
            List<Board> boards = await BoardRepo.GetAllFromAPI();

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Rental fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Rentals?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                rentals = JsonSerializer.Deserialize<List<Rental>>(jsonResponse, options)!;
            }

            //Fylder hvert Rental objekt med dets tilhørende board
            foreach (Rental rental in rentals)
            {
                Board board = null;
                int i = 0;
                while (i < boards.Count && board == null)
                {
                    if (rentals[i].BoardId == board.Id)
                    {
                        board = boards[i];
                        rental.Board = board;
                    }
                    else
                        i++;
                }
                if (board == null)
                    throw new Exception($"Hov det board ({rental.BoardId}) findes vist ikke...");
            }

            return rentals;
        }

        public async static Task<Rental> GetFromAPI(int id)
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            Rental returnRental = new Rental();
            List<Board> boards = await BoardRepo.GetAllFromAPI();

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Rental fra API
            using (HttpResponseMessage response = await client.GetAsync($"api/Rentals?Id={id}&apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                returnRental = JsonSerializer.Deserialize<Rental>(jsonResponse, options)!;
            }

            //Sætter tilhørende navigationproperty Board til Rental Objekt
            foreach (Board board in boards)
            {
                if (board.Id == returnRental.BoardId)
                {
                    returnRental.Board = board;
                }
            }
            if (returnRental.Board == null)
                throw new Exception($"Der kunne ikke findes et tilhørende board med Id: {returnRental.BoardId}");

            return returnRental;
        }

        public async static Task<Rental> PostToAPI(Rental rental)
        {
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            HttpRequestMessage message = new(HttpMethod.Post, "api/Rental?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
            HttpContent content = new StringContent(JsonSerializer.Serialize(rental), Encoding.UTF8, "application/json");
            message.Content = content;

            //Tjek response fra API
            using (HttpResponseMessage response = await client.SendAsync(message))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Rental rentalToCheck = JsonSerializer.Deserialize<Rental>(jsonResponse, options)!;
                    return rentalToCheck;
                }
                else
                    throw new Exception("There was an error Posting the rental");
            }
        }

        public async static Task<Rental> PutToAPI(Rental rental)
        {
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    
            HttpRequestMessage message = new(HttpMethod.Put, "api/Rental?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
            HttpContent content = new StringContent(JsonSerializer.Serialize(rental), Encoding.UTF8, "application/json");
            message.Content = content;

            // Tjek response fra API
            using (HttpResponseMessage response = await client.SendAsync(message))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Rental rentalToCheck = JsonSerializer.Deserialize<Rental>(jsonResponse, options)!;
                    return rentalToCheck;
                }
                else
                    throw new Exception("There was an error Updating the rental");
            }
        }

        public async static Task<Rental> DeleteToAPI(int id)
        {
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Boards fra API
            using (HttpResponseMessage response = await client.DeleteAsync($"api/Rental?Id={id}&apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Rental rentalToCheck = JsonSerializer.Deserialize<Rental>(jsonResponse, options)!;
                    return rentalToCheck;
                }
                else
                    throw new Exception("There was an error Updating the rental");
            }
        }
    }
}