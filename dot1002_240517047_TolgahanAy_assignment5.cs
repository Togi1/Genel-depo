namespace LiquidatorGame
{
    public interface IGameEntity
    {
        string Name { get; }
    }

    public class Player
    {
        private int _radiation;
        private int _inventorySize;
        private int _itemCount;

        public int Radiation 
        { 
            get { return _radiation; } 
            private set { _radiation = value; } 
        }

        public Player()
        {
            _radiation = 0;
            _inventorySize = 5;
            _itemCount = 0;
        }

        public void AddRadiation(int amount)
        {
            Radiation += amount;
        }

        public bool SearchItem()
        {
            throw new NotImplementedException(); 
        }

        public int GetRadiation()
        {
            return Radiation;
        }

        public int GetItemCount()
        {
            return _itemCount;
        }
    }

    public class Location : IGameEntity
    {
        public string Name { get; private set; }
        
        private bool _hasItem;

        public Location(string name, bool hasItem)
        {
            Name = name;
            _hasItem = hasItem;
        }

        public string GetName()
        {
            return Name;
        }

        public bool HasItem()
        {
            return _hasItem;
        }

        public bool CollectItem()
        {
            throw new NotImplementedException();
        }
    }

    public class GameEngine
    {
        private Player _player;
        private List<Location> _locations;
        private Random _random;

        public GameEngine()
        {
            _player = new Player();
            _locations = new List<Location>();
            _random = new Random();
            
            SetupLocations();
        }

        private void SetupLocations()
        {
        }

        // Methods
        public void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LIQUIDATOR - 1986 Çernobil (OOP Version)\n");
            
        
        }

        private void GameLoop()
        {
            throw new NotImplementedException();
        }

        private void ProcessCommand(string command)
        {
            throw new NotImplementedException();
        }

        private void TriggerEnemyEncounter()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GameEngine engine = new GameEngine();
            engine.StartGame();
            
            Console.ReadLine();
        }
    }
}