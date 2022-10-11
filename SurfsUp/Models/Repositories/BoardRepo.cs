using System.Text.Json;

namespace SurfsUp.Models.Repositories
{
    public class BoardRepo
    {

        private async static Task<List<Board>> GetAllFromAPI()
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            List<Board> boards;
            List<BoardEquipment> boardEquipment;
            List<Equipment> equipment;

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Boards fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Boards"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boards = JsonSerializer.Deserialize<List<Board>>(jsonResponse, options)!;
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/BoardEquipment"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardEquipment = JsonSerializer.Deserialize<List<BoardEquipment>>(jsonResponse, options)!;
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Equipment"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                equipment = JsonSerializer.Deserialize<List<Equipment>>(jsonResponse, options)!;
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
                    if (boards[i].BoardId == be.BoardId)
                    {
                        b = boards[i];
                        be.Board = b;
                        b.BoardEquipments.Add(be);
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
                    if (equipment[i].EquipmentId == be.EquipmentId)
                    {
                        eq = equipment[i];
                        be.Equipment = eq;
                        eq.BoardEquipments.Add(be);
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

        private async static Task<List<Board>> GetFromAPI(int id)
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            List<Board> boards;
            List<BoardEquipment> boardEquipment;
            List<Equipment> equipment;

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Boards fra API
            using (HttpResponseMessage response = await client.GetAsync($"api/Board/{id}"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boards = JsonSerializer.Deserialize<List<Board>>(jsonResponse, options)!;
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/BoardEquipment"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardEquipment = JsonSerializer.Deserialize<List<BoardEquipment>>(jsonResponse, options)!;
            }

            // Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Equipment"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                equipment = JsonSerializer.Deserialize<List<Equipment>>(jsonResponse, options)!;
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
                    if (boards[i].BoardId == be.BoardId)
                    {
                        b = boards[i];
                        be.Board = b;
                        b.BoardEquipments.Add(be);
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
                    if (equipment[i].EquipmentId == be.EquipmentId)
                    {
                        eq = equipment[i];
                        be.Equipment = eq;
                        eq.BoardEquipments.Add(be);
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

    }
}
