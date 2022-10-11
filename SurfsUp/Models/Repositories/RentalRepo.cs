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
                        rental.User = us;
                    }
                    else
                        i++;
                }
                if (us == null)
                    throw new Exception($"Hov den User ({rental.UsersId}) findes vist ikke...");
            }

            foreach (ApplicationUser user in Users)
            {
                List<Rental> RentalHolder = new List<Rental>();
                Rental rentalHolder = new Rental();
                int i = 0;
                while (i < rentals.Count)
                {
                    if (rentals[i].UsersId == Users[i].Id)
                    {
                        RentalHolder.Add(rentals[i]);
                        

                    }
                    else
                        i++;
                }

                user.rentals = RentalHolder;
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

            Rental rental = new Rental();
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

            foreach  (Board board in boards)
            {
                
                if (board.BoardId == id)
                {
                    rental.Board = board;
                }
            }

            // Kombinér boards, boardEquipment og equipment
            foreach (BoardEquipment be in boardEquipments)
            {
                Board b = null;
                Equipment eq = null;


                if(rental.Board.BoardId == be.BoardId)
                {
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

                }

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
                while (rental.)
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
                        rental.User = us;
                    }
                    else
                        i++;
                }
                if (us == null)
                    throw new Exception($"Hov den User ({rental.UsersId}) findes vist ikke...");
            }

            foreach (ApplicationUser user in Users)
            {
                List<Rental> RentalHolder = new List<Rental>();
                Rental rentalHolder = new Rental();
                int i = 0;
                while (i < rentals.Count)
                {
                    if (rentals[i].UsersId == Users[i].Id)
                    {
                        RentalHolder.Add(rentals[i]);


                    }
                    else
                        i++;
                }

                user.rentals = RentalHolder;
            }

            return rentals;
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
