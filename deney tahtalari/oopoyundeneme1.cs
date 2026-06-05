using System;
using System.Collections.Generic;

namespace LiquidatorGame
{
    public interface IGameEntity
    {
        string Name { get; }
    }

    public class Player
    {
        public int Radiation { get; private set; }
        public int ItemCount { get; private set; }

        public Player()
        {
            Radiation = 0;
            ItemCount = 0;
        }

        public void AddRadiation(int amount)
        {
            Radiation += amount;
        }

        public void SetRadiation(int amount) 
        {
            Radiation = amount;
        }

        public void CollectItem()
        {
            ItemCount++;
        }

        public bool IsAlive()
        {
            return Radiation < 100;
        }

        public bool HasWon()
        {
            return ItemCount >= 5 && IsAlive();
        }
    }

    public class Location : IGameEntity
    {
        public string Name { get; private set; }
        private bool _hasItem;

        public Location(string name)
        {
            Name = name;
            _hasItem = true; 
        }

        public bool HasItem()
        {
            return _hasItem;
        }

        public bool CollectItem()
        {
            if (_hasItem)
            {
                _hasItem = false;
                return true; 
            }
            return false; 
        }
    }

    public class GameEngine
    {
        private Player _player;
        private List<Location> _locations;
        private Random _random;
        private int _currentLocationIndex;

        public GameEngine()
        {
            _player = new Player();
            _locations = new List<Location>();
            _random = new Random();
            _currentLocationIndex = -1; 
            
            SetupLocations();
        }

        private void SetupLocations()
        {
            _locations.Add(new Location("Hastane"));
            _locations.Add(new Location("Kızıl Orman"));
            _locations.Add(new Location("Fabrika"));
            _locations.Add(new Location("4. Reaktör Çatısı"));
            _locations.Add(new Location("Pompa İstasyonu"));
        }

        public void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LIQUIDATOR - 1986 Çernobil (OOP Version)\n");
            
            GameLoop(); 
        }

        private void GameLoop()
        {
            while (_player.IsAlive() && !_player.HasWon())
            {
                Console.WriteLine($"\nRadyasyon: {_player.Radiation}/100 | Toplanan Parça: {_player.ItemCount}/5");
                Console.Write("Komut Gir (1-5 Mekan Seç, ara, durum, kontrol): ");
                string command = Console.ReadLine().ToLower().Trim();
                
                ProcessCommand(command);
            }

            if (_player.HasWon())
            {
                Console.WriteLine("\nKAZANDIN! Valfleri açtın ve milyonları kurtardın.");
            }
            else
            {
                Console.WriteLine("\nKAYBETTİN! Hücrelerin parçalandı, radyasyondan öldün.");
            }
            
            Console.ReadLine();
        }

        private void ProcessCommand(string command)
        {
            if (command == "1" || command == "2" || command == "3" || command == "4" || command == "5")
            {
                _currentLocationIndex = int.Parse(command) - 1;
                Console.WriteLine($">>> {_locations[_currentLocationIndex].Name} bölgesine geldin.");
                
                int radIncrease = _random.Next(5, 15);
                _player.AddRadiation(radIncrease);
                
                if (_random.Next(100) < 5) 
                { 
                    _player.SetRadiation(100); 
                    Console.WriteLine("Grafit parçasına değdin, anında zehirlendin!"); 
                    return; 
                }
                
                if (_random.Next(100) < 35) 
                {
                    TriggerEnemyEncounter();
                }
            }
            else if (command == "ara")
            {
                if (_currentLocationIndex != -1)
                {
                    _player.AddRadiation(5);
                    Location currentLocation = _locations[_currentLocationIndex];
                    
                    if (currentLocation.CollectItem())
                    {
                        _player.CollectItem();
                        Console.WriteLine("Görev eşyasını buldun!");
                    }
                    else
                    {
                        Console.WriteLine("Burada artık bir şey yok.");
                    }
                }
                else
                {
                    Console.WriteLine("Önce bir mekana gitmelisin!");
                }
            }
            else if (command == "durum") 
            {
                Console.WriteLine("Hedef: 5 eşyayı topla ve radyasyon 100 olmadan hayatta kal.");
            }
            else if (command == "kontrol") 
            {
                if (_currentLocationIndex == 3) 
                    Console.WriteLine("3.6 Roentgen. Harika değil, korkunç da değil.");
                else 
                    Console.WriteLine("Dozimetre cızırdıyor...");
            }
            else
            {
                Console.WriteLine("Geçersiz komut!");
            }
        }

        private void TriggerEnemyEncounter()
        {
            Console.Write("Düşman belirdi! 1-Sersemlet (Zar 4-6), 2-Kaç (Zar 5-6): ");
            string secim = Console.ReadLine();
            int zar = _random.Next(1, 7);
            
            if (secim == "1" && zar >= 4) 
                Console.WriteLine($"Zar {zar}: Fenerle sersemlettin!");
            else if (secim == "2" && zar >= 5) 
                Console.WriteLine($"Zar {zar}: Kaçmayı başardın!");
            else 
            { 
                Console.WriteLine($"Zar {zar}: Başarısız oldun! Radyasyon aldın."); 
                _player.AddRadiation(20); 
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GameEngine engine = new GameEngine();
            engine.StartGame();
        }
    }
}