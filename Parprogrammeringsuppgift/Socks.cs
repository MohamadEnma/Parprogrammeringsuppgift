using System.Text.Json;

namespace Parprogrammeringsuppgift
{
    internal class Socks
    {
        public List<Socks> SocksOrderList = new List<Socks>();
        public List<string> FilesNames = new List<string>();

        public int Size { get; set; }

        public string Color { get; set; } = "";

        private int _rating;

        public int Rating
        {
            get { return _rating; }
            set
            {
                if (value < 0 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("Betyget måste vara mellan 1 till 5");
                }
                _rating = value;
            }
        }

        public Socks(int size, string color, int rating)
        {
            Size = size;
            Color = color;
            Rating = rating;
            SocksOrderList.Add(this);
        }

        public Socks()
        {
        }

        public void RunApp()
        {
            while (true)
            {
                MainMenu();
                HandleUserInput();
            }
        }
        public void MainMenu()
        {
            Console.WriteLine("----------------MENY---------------");
            Console.WriteLine("Tryck 1 för att registrera nya strumpor");
            Console.WriteLine("Tryck 2 för att exportera registrerade strumpor till en fil");
            Console.WriteLine("Tryck 3 för att läsa registrerade strumpor från en fil");
            Console.WriteLine("Tryck 4 för att avsluta");
        }

        public void HandleUserInput()
        {
            int choice;

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Felaktig inmatning. Ange ett tal mellan 1 och 4: ");
            }

            switch (choice)
            {
                case 1:
                    RegisterNewSocks();
                    break;
                case 2:
                    ExportToFile();
                    break;
                case 3:
                    ReadFromFile();
                    break;
                case 4:
                    EndApp();
                    break;
                default:
                    Console.WriteLine("Fel inträffat")
                        ; break;
            }
        }

        public void RegisterNewSocks()
        {
            try
            {
                int size;
                Console.Write("Ange storlek: ");
                while (!int.TryParse(Console.ReadLine(), out size) || size < 12 || size > 47)
                {
                    Console.WriteLine("Felaktig inmatning. Vänligen ange ett heltal mellan 12 och 47.");
                    Console.Write("Ange storlek (16-47): ");
                }

                string? color;
                do
                {
                    Console.Write("Ange färg: ");
                    color = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(color))
                    {
                        Console.WriteLine("Felaktig inmatning. Du måste ange en färg.");
                    }
                } while (string.IsNullOrEmpty(color));

                int rating;
                Console.Write("Ange betyg (1-5): ");
                while (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5)
                {
                    Console.WriteLine("Felaktig inmatning. Vänligen ange ett heltal mellan 1 och 5.");
                    Console.Write("Ange betyg (1 - 5): ");
                }

                Socks newSock = new Socks(size, color, rating);
                SocksOrderList.Add(newSock);
                Console.WriteLine("Strumpan har nu registrerats.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid registrering: " + ex.Message);
            }
        }

        public void ExportToFile()
        {
            string? filename;
            Console.WriteLine("Ange fil namn");
            filename = Console.ReadLine();
            string text = JsonSerializer.Serialize(SocksOrderList);
            File.WriteAllText(filename, text);
            FilesNames.Add(filename);
            Console.WriteLine($"All intries har sparat till {filename}");
        }

        public void ReadFromFile()
        {
            Console.WriteLine("Tillgängliga filer:");
            string fileListJson = JsonSerializer.Serialize(FilesNames);
            Console.WriteLine(fileListJson);


            Console.Write("Ange filnamn: ");
            string? readFile = Console.ReadLine();

            if (string.IsNullOrEmpty(readFile))
            {
                Console.WriteLine("Inget filnamn angivet.");
                return;
            }

            if (!File.Exists(readFile))
            {
                Console.WriteLine("Filen hittades inte.");
                return;
            }

            string fileContent = File.ReadAllText(readFile);


            try
            {
                List<Socks>? loadedSocks = JsonSerializer.Deserialize<List<Socks>>(fileContent);
                if (loadedSocks != null)
                {
                    Console.WriteLine("Innehåll i filen:");
                    foreach (Socks sock in loadedSocks)
                    {
                        Console.WriteLine($"Storlek: {sock.Size}, Färg: {sock.Color}, Betyg: {sock.Rating}");
                    }
                }
                else
                {
                    Console.WriteLine("Inga strumpor kunde läsas från filen.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid deserialisering: " + ex.Message);
            }
        }


        public void EndApp()
        {
            Console.WriteLine("Applikationen avslutas. Tack för idag!");
            Environment.Exit(0);
        }
    }
}
