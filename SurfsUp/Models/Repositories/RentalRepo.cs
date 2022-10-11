using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using System.Security.Claims;
using System.Text.Json;

namespace SurfsUp.Models.Repositories
{
    public class RentalRepo
    {
        public static ApplicationDbContext CurrentInstance { get; private set; }


        public RentalRepo(ApplicationDbContext context)
        {
            CurrentInstance = context;
        }
        public async static Task<List<Rental>> GetAllFromAPI()
        {
            // BIG CREDIT TO THE OG KC
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            List<Rental> rentals;
            List<Board> boards;
            List<Equipment> equipment;
            List<BoardEquipment> boardEquipments;
            List<ApplicationUser> Users = CurrentInstance.Users.ToList();

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            // Hent Rental fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Rentals?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                rentals = JsonSerializer.Deserialize<List<Rental>>(jsonResponse, options)!;
            }

            // Hent Board fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Board?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                 boards = JsonSerializer.Deserialize<List<Board>>(jsonResponse, options)!;
            }

            // Hent Equipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/Equipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                equipment = JsonSerializer.Deserialize<List<Equipment>>(jsonResponse, options)!;
            }

            //Hent BoardEquipment fra API
            using (HttpResponseMessage response = await client.GetAsync("api/BoardEquipment?apikey=4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardEquipments = JsonSerializer.Deserialize<List<BoardEquipment>>(jsonResponse, options)!;
            }


            // Kombinér boards, boardEquipment og equipment
            foreach (BoardEquipment be in boardEquipments)
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

            foreach (Rental rental in rentals)
            {
                Board bo = new Board();
                int i = 0;
                while (i < boards.Count && bo == null)
                {
                    if (rentals[i].BoardId == bo.BoardId)
                    {
                        bo = boards[i];
                        rental.Board = bo;
                    }
                    else
                        i++;
                }
                if (bo == null)
                    throw new Exception($"Hov det board ({rental.BoardId}) findes vist ikke...");
            }

            foreach (Rental rental in rentals)
            {
                Board bo = new Board();
                int i = 0;
                while (i < boards.Count && bo == null)
                {
                    if (rentals[i].BoardId == boards[i].BoardId)
                    {
                        bo = boards[i];
                        rental.Board = bo;
                    }
                    else
                        i++;
                }
                if (bo == null)
                    throw new Exception($"Hov det board ({rental.BoardId}) findes vist ikke...");
            }

            foreach (Rental rental in rentals)
            {
                ApplicationUser us = new ApplicationUser();
                int i = 0;
                while (i < Users.Count && us == null)
                {
                    if (rentals[i].UsersId == Users[i].Id)
                    {
                        us = Users[i];
                        rental.Board = bo;
                    }
                    else
                        i++;
                }
                if (bo == null)
                    throw new Exception($"Hov det board ({rental.BoardId}) findes vist ikke...");
            }



            rental.UsersId = claims.Value;
            rental.BoardId = id;
            ViewData["SelectedBoardId"] = rental.StartRental;
            rental.Board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            Rental.User = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == rental.UsersId);



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
            using (HttpResponseMessage response = await client.GetAsync($"api/Board/{id}"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                board = JsonSerializer.Deserialize<Board>(jsonResponse, options)!;
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

            // Kombinér board med tilhørende equipment
            foreach (BoardEquipment be in boardEquipment)
            {
                Equipment eq = null;

                // FIND BOARD

                if (board.BoardId == be.BoardId)
                {
                    be.Equipment = eq;
                }


                if (eq == null)
                    throw new Exception($"Hov det equipment ({be.EquipmentId}) findes vist ikke...");

                // INDSÆT EQUIPMENT I BOARD NAVIGATION PROPERTIES EQUIPMENTS
                board.Equipment.Add(eq);

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

            HttpRequestMessage message = new(HttpMethod.Post, "api/Board?4d1bb604-377f-41e0-99c7-59846080bb47");
            HttpContent content = new StringContent(JsonSerializer.Serialize(board));
            message.Content = content;

            // Hent Boards fra API
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
            Board boardToCheck = new Board();
            // BIG CREDIT TO THE OG Pete the Speed
            using HttpClient client = new()
            {
                BaseAddress = new Uri("https://localhost:7122/")
            };

            // NØDVENDIG, så JSON ignorer forskellen mellem f.eks. "Name" og "name" i property navne.
            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

            HttpRequestMessage message = new(HttpMethod.Put, "api/Board?4d1bb604-377f-41e0-99c7-59846080bb47");
            HttpContent content = new StringContent(JsonSerializer.Serialize(board));
            message.Content = content;


            // Hent Boards fra API
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
            using (HttpResponseMessage response = await client.DeleteAsync($"api/Board?Id={id}4d1bb604-377f-41e0-99c7-59846080bb47"))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                boardToCheck = JsonSerializer.Deserialize<Board>(jsonResponse, options)!;

                if (boardToCheck == null)
                {
                    throw new Exception("There was an error deleting the board");
                }
                return boardToCheck;

            }

        }



    }
}
}
