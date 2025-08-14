using Snake.Persistence;
using System.Xml;

namespace Snake.Model
{
    public class SnakeTableModel
    {
        private Random rnd;

        #region Propertik
        public int tableSize { get; private set; }
        public List<PointP> walls {  get; set; }
        public List<PointP> snake { get; private set; }
        public PointP egg { get; set; }
        public int eggCount { get; private set; }
        public PointP move { get; set; }
        public PointP tail { get; private set; }
        #endregion

        #region Konstruktor
        public SnakeTableModel()
        {
            walls = new List<PointP>();
            snake = new List<PointP>();
            move = new PointP(-1, 0);
            egg = new PointP();
            tail = new PointP();
            rnd = new Random();
        }
        public void NewTable(int size)
        {
            tableSize = size;

            // FALAK
            walls.Clear();

            // KÍGYÓ
            snake.Clear();
            for (int i = 0; i < 5; i++)
            {
                snake.Add(new PointP(tableSize/2 + i, tableSize/2));
            }
            move = new PointP(-1, 0);

            // TOJÁS
            eggCount = 0;
        }
        #endregion

        #region Események
        public event EventHandler<EggWasEatenEventArgs>? EggWasEaten;
        private void OnEggWasEaten()
        {
            EggWasEaten?.Invoke(this, new EggWasEatenEventArgs(egg, snake, move, eggCount));
        }    
        public event EventHandler<PointP>? RemoveEgg;
        private void OnRemoveEgg()
        {
            RemoveEgg?.Invoke(this, egg);
        }
        public event EventHandler? GameOver;
        private void OnGameOver()
        {
            GameOver?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler<DrawSnakeEventArgs>? DrawSnake;
        private void OnDrawSnake()
        {
            DrawSnake?.Invoke(this, new DrawSnakeEventArgs(snake, tail));
        }
        #endregion

        #region Tojás számának növelése
        public void PlusEggCount()
        {
            eggCount++;
        }
        #endregion

        #region Kígyó mozgatása
        public void MoveSnake(object? sender, EventArgs e)
        {
            if (!StepCheck()) { OnGameOver(); return;  }
            tail = snake[snake.Count - 1];
            for (int i = snake.Count() - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }
            snake[0] = snake[0].AddPoint(move);
            OnDrawSnake();
        }
        private bool StepCheck()
        {
            PointP head = snake[0].AddPoint(move);

            // KIMEGY A PÁLYÁRÓL
            if (head.x < 0 || head.y < 0 || head.x >= tableSize || head.y >= tableSize)
            { return false; }
            // FALAKNAK MEGY NEKI
            if (head.IsInList(walls)) { return false; }
            // SAJÁT MAGÁNAK MEGY NEKI
            if (head.IsInList(snake)) { return false; }
            // TOJÁST SZED FEL
            if (head.Equals(egg)) 
            {
                PlusEggCount();
                OnRemoveEgg();
                NewEgg();
                OnEggWasEaten();
            }
            return true;
        }
        #endregion

        #region Tojás
        public void NewEgg()
        {
            do
            {
                egg = new PointP(rnd.Next(tableSize), rnd.Next(tableSize));
            }
            while (egg.IsInList(snake) || egg.IsInList(walls));
        }
        #endregion
    }
}
