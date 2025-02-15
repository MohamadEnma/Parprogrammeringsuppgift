using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Parprogrammeringsuppgift
{

    internal class SocksManager 
    {
        private List<Sock> socksOrderList = new List<Sock>();
        private List<string> fileNames = new List<string>();

        public void RunApp()
        {
            while (true)
            {
                MainMenu();
                HandleUserInput();
            }
        }

        private void MainMenu()
        {
            Console.WriteLine("\n---------------- MENY ---------------");
            Console.WriteLine("1: Registrera nya strumpor");
            Console.WriteLine("2: Exportera registrerade strumpor till en fil");
            Console.WriteLine("3: Läsa registrerade strumpor från en fil");
            Console.WriteLine("4: Avsluta");
            Console.Write("Ange ditt val: ");
        }

        private void HandleUserInput()
        {
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 4)
            {
                Console.WriteLine("Felaktig inmatning. Ange ett tal mellan 1 och 4.");
                return;
            }

            switch (choice)
            {
                case 1:
                    RegisterNewSock();
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
                    Console.WriteLine("Okänt val, försök igen.");
                    break;
            }
        }

        private void RegisterNewSock()
        {
            try
            {
                // Storlek
                Console.Write("Ange storlek (12-47): ");
                if (!int.TryParse(Console.ReadLine(), out int size) || size < 12 || size > 47)
                {
                    Console.WriteLine("Felaktig inmatning. Storlek måste vara mellan 12 och 47.");
                    return;
                }

                // Färg
                string? color;
                do
                {
                    Console.Write("Ange färg: ");
                    color = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(color))
                    {
                        Console.WriteLine("Du måste ange en färg.");
                    }
                } while (string.IsNullOrEmpty(color));

                // Betyg
                Console.Write("Ange betyg (1-5): ");
                if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 5)
                {
                    Console.WriteLine("Felaktig inmatning. Betyg måste vara mellan 1 och 5.");
                    return;
                }

                // Skapa och lägg till strumpa
                Sock newSock = new Sock(size, color, rating);
                socksOrderList.Add(newSock);
                Console.WriteLine("Strumpan har registrerats.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid registrering: " + ex.Message);
            }
        }

        private void ExportToFile()
        {
            Console.Write("Ange filnamn för export: ");
            string? filename = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filename))
            {
                Console.WriteLine("Fel: Inget filnamn angivet.");
                return;
            }

            try
            {
                string jsonText = JsonSerializer.Serialize(socksOrderList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filename, jsonText);
                fileNames.Add(filename);
                Console.WriteLine($"Alla strumpor har sparats till '{filename}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid export: " + ex.Message);
            }
        }

        private void ReadFromFile()
        {
            Console.WriteLine("Tillgängliga filer:");
            if (fileNames.Count == 0)
            {
                Console.WriteLine("Inga filer har exporterats än.");
            }
            else
            {
                foreach (var file in fileNames)
                {
                    Console.WriteLine(file);
                }
            }

            Console.Write("Ange filnamn att läsa: ");
            string? readFile = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(readFile))
            {
                Console.WriteLine("Fel: Inget filnamn angivet.");
                return;
            }

            if (!File.Exists(readFile))
            {
                Console.WriteLine("Filen hittades inte.");
                return;
            }

            try
            {
                string fileContent = File.ReadAllText(readFile);
                List<Sock>? loadedSocks = JsonSerializer.Deserialize<List<Sock>>(fileContent);
                if (loadedSocks != null)
                {
                    Console.WriteLine("Innehåll i filen:");
                    foreach (Sock sock in loadedSocks)
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
                Console.WriteLine("Fel vid läsning/deserialisering: " + ex.Message);
            }
        }

        private void EndApp()
        {
            Console.WriteLine("Applikationen avslutas. Tack för idag!");
            Environment.Exit(0);
        }
    }

}
