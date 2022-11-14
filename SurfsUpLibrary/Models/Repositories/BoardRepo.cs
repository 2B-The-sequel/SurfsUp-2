using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SurfsUpLibrary.Models.Repositories
{
    public class BoardRepo
    {
        public async static Task<List<Board>> GetAllFromAPI()
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            List<Board> boards;
            List<BoardEquipment> boardEquipment;
            List<Equipment> equipment = await EquipmentRepo.Retrieve();

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Boards fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Boards?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boards = JsonSerializer.Deserialize<List<Board>>(jsonResponse, options)!;
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/BoardEquipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardEquipment = JsonSerializer.Deserialize<List<BoardEquipment>>(jsonResponse, options)!;
            }

            // Kombinér boards, boardEquipment og equipment
            foreach (BoardEquipment be in boardEquipment)
            {
                Board b = null;
                Equipment eq = null;

                // FIND BOARD
                int i = 0;
                while (i < boards.Count && b == null)
                {
                    if (boards[i].Id == be.BoardId)
                    {
                        b = boards[i];
                        be.Board = b;
                    }
                    else
                        i++;
                }
                if (b == null)
                    throw new Exception($"Hov det board ({be.BoardId}) findes vist ikke...");

                // FIND EQUIPMENT
                i = 0;
                while (i < equipment.Count && eq == null)
                {
                    if (equipment[i].Id == be.EquipmentId)
                    {
                        eq = equipment[i];
                        be.Equipment = eq;
                    }
                    else
                        i++;
                }
                if (eq == null)
                    throw new Exception($"Hov det equipment ({be.EquipmentId}) findes vist ikke...");

                // INDSÆT I BOARD OG EQUIPMENT NAVIGATION PROPERTIES
                b.Equipment.Add(eq);
                eq.Boards.Add(b);
            }

            return boards;
        }

        public async static Task<Board> GetFromAPI(int id)
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            Board board;
            List<BoardEquipment> boardEquipment;
            List<Equipment> equipment;

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Boards fra API
            using (HttpResponseMessage response = await client.GetAsync($"api/Boards/{id}?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                { board = JsonSerializer.Deserialize<Board>(jsonResponse, options)!; }
                else { throw new Exception("Exception"); }
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/BoardEquipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                { boardEquipment = JsonSerializer.Deserialize<List<BoardEquipment>>(jsonResponse, options)!; }
                else { throw new Exception("Exception"); }
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Equipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {  equipment = JsonSerializer.Deserialize<List<Equipment>>(jsonResponse, options)!; }
                else { throw new Exception("Exception"); }
            }

            
            // Kombinér board med tilhørende equipment
            foreach (BoardEquipment be in boardEquipment)
            {
                Equipment eq = null;

                // FIND EQUIPMENT
                int i = 0;
                while (i < equipment.Count && eq == null)
                {
                    if (equipment[i].Id == be.EquipmentId)
                    {
                        eq = equipment[i];
                    }
                    i++;
                }

                if (eq == null)
                    throw new Exception($"Hov det equipment ({be.EquipmentId}) findes vist ikke...");

                if (board.Id == be.BoardId)
                {
                    board.Equipment.Add(eq);
                }
            }

            return board;
        }

        public async static Task<Board> PostToAPI(Board board)
        {
            Board boardToCheck = new Board();
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            HttpRequestMessage message = new(HttpMethod.Post, "api/Boards?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
            HttpContent content = new StringContent(JsonSerializer.Serialize(board), Encoding.UTF8, "application/json");
            message.Content = content;

            // Tjek om Board er postet fra API
            using (HttpResponseMessage response = await client.SendAsync(message))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardToCheck = JsonSerializer.Deserialize<Board>(jsonResponse, options)!;

                if (board == null)
                {
                    throw new Exception("There was an error Posting the board");
                }
                return boardToCheck;
            }
        }

        public async static Task<Board> PutToAPI(Board board)
        {
            if (board == null)
            { 
                throw new Exception();
            }
            Board boardToCheck = new Board();
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            HttpRequestMessage message = new(HttpMethod.Put, "api/Boards?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
            HttpContent content = new StringContent(JsonSerializer.Serialize(board),Encoding.UTF8,"application/json");
            message.Content = content;

            List<BoardEquipment> boardEquipmentList;
            using (HttpResponseMessage response = await client.GetAsync("api/BoardEquipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardEquipmentList = JsonSerializer.Deserialize<List<BoardEquipment>>(jsonResponse, options)!;
            }

            //Først delete alle boardId relationer til det board med det samme id board.id
            foreach (BoardEquipment be in boardEquipmentList)
            {
                if (be.BoardId == board.Id)
                {
                    HttpRequestMessage message2 = new(HttpMethod.Delete, $"api/BoardEquipment/{be.Id}?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
                    HttpContent content2 = new StringContent(JsonSerializer.Serialize(be), Encoding.UTF8, "application/json");
                    message2.Content = content2;
                    using (HttpResponseMessage response = await client.SendAsync(message2))
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        BoardEquipment beToCheck = JsonSerializer.Deserialize<BoardEquipment>(jsonResponse, options)!;

                        if (be == null)
                        {
                            throw new Exception("There was an error deleting the BoardEquipment");
                        }
                    }
                }
                
            }

            foreach (Equipment eq in board.Equipment)
            {
                BoardEquipment be = new();
                be.BoardId = board.Id;
                be.EquipmentId = eq.Id;

                HttpRequestMessage message3 = new(HttpMethod.Post, $"api/BoardEquipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47");
                HttpContent content3 = new StringContent(JsonSerializer.Serialize(be), Encoding.UTF8, "application/json");
                message3.Content = content3;
                using (HttpResponseMessage response = await client.SendAsync(message3))
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    BoardEquipment beToCheck = JsonSerializer.Deserialize<BoardEquipment>(jsonResponse, options)!;

                    if (be == null)
                    {
                        throw new Exception("There was an error Posting the BoardEquipment");
                    }
                }
            }

            //Hent det board der bliver sendt tilbage fra API
            using (HttpResponseMessage response = await client.SendAsync(message))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardToCheck = JsonSerializer.Deserialize<Board>(jsonResponse, options)!;

                if (board == null)
                {
                    throw new Exception("There was an error Posting the board");
                }

                return boardToCheck;

            }

        }

        public async static Task<Board> DeleteToAPI(int id)
        {
            Board boardToCheck = new Board();
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Boards fra API
            using (HttpResponseMessage response = await client.DeleteAsync($"api/Boards/{id}?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardToCheck = JsonSerializer.Deserialize<Board>(jsonResponse, options)!;

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                { return boardToCheck; }
                else
                { throw new Exception($"{response.StatusCode}"); }

            }

        }
    }
}
