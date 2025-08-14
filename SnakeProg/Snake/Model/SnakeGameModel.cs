using Snake.Persistence;
using System;
using System.Timers;

namespace Snake.Model
{
    public class SnakeGameModel
    {
        #region Adattagok
        private ReadFile file;
        //private DateTime startTime;
        //private DateTime pauseTime;
        //private int pausedSeconds = 0;
        private int seconds;
        private bool isGameOver;
        #endregion

        #region Propertik
        public System.Timers.Timer timer { get; set; }
        public SnakeTableModel snakeTable { get; private set; }
        public bool canSnakeTurn { get; set; }
        #endregion

        #region Események
        public event EventHandler<int>? TimerTicks;
        private void OnTimerTicks(object? sender, ElapsedEventArgs e)
        {
            seconds++;
            TimerTicks?.Invoke(this, seconds);
        }
        private void OnSnakeTurn(object? sender, EventArgs e)
        {
            canSnakeTurn = true;
        }
        public event EventHandler<DrawEggEventArgs>? DrawEgg;
        private void OnDrawEgg(PointP egg, int eggCount)
        {
            DrawEgg?.Invoke(this, new DrawEggEventArgs(egg, eggCount));
        }
        public event EventHandler<GenerateTableEventArgs>? GenerateTable;
        private void OnGenerateTable()
        {
            GenerateTable?.Invoke(this, new GenerateTableEventArgs(snakeTable.tableSize, snakeTable.walls));
        }
        public event EventHandler<GameOverEventArgs>? GameOver;
        private void OnGameOver()
        {
            GameOver?.Invoke(this, new GameOverEventArgs(seconds, snakeTable.eggCount));
        }
        #endregion

        #region Konstruktor
        public SnakeGameModel(System.Timers.Timer tTimer)
        {
            file = new ReadFile();
            snakeTable = new SnakeTableModel();

            timer = tTimer;
            //timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimerTicks;
            timer.Elapsed += OnSnakeTurn;
            timer.Elapsed += snakeTable.MoveSnake;
            timer.AutoReset = true;
            snakeTable.GameOver += TimerStop;
            snakeTable.EggWasEaten += ManageEgg;
        }
        public void NewGame(string path)
        {
            timer.Stop();
            file.Load(path, snakeTable);
            OnGenerateTable();
            OnDrawEgg(snakeTable.egg, snakeTable.eggCount);
            canSnakeTurn = true;
            timer.Enabled = true;
            //startTime = DateTime.Now;
            //pausedSeconds = 0;
            seconds = 0;
            isGameOver = false;
        }
        #endregion

        #region Nő a kígyó
        public void SnakeGrow(List<PointP> snake, PointP move)
        {
            PointP p = snake[snake.Count - 1];
            PointP q = new PointP (move.x * (-1), move.y * (-1));
            p = p.AddPoint(q);
            snake.Add(p);
        }
        #endregion

        #region Tojás
        public void ManageEgg(object? sender, EggWasEatenEventArgs eventArgs)
        {
            SnakeGrow(eventArgs.Snake, eventArgs.Move);
            OnDrawEgg(eventArgs.Egg, eventArgs.EggCount);
        }
        #endregion

        #region Elfordul a kígyó
        public void TurnSnakeLeft()
        {
            if (canSnakeTurn && timer.Enabled)
            {
                if (snakeTable.move.Equals(new PointP(0, 1)))
                {
                    snakeTable.move = new PointP(-1, 0);
                }
                else if (snakeTable.move.Equals(new PointP(1, 0)))
                {
                    snakeTable.move = new PointP(0, 1);
                }
                else if (snakeTable.move.Equals(new PointP(0, -1)))
                {
                    snakeTable.move = new PointP(1, 0);
                }
                else if (snakeTable.move.Equals(new PointP(-1, 0)))
                {
                    snakeTable.move = new PointP(0, -1);
                }
                canSnakeTurn = false;
            }
        }
        public void TurnSnakeRight()
        {
            if (canSnakeTurn && timer.Enabled)
            {
                if (snakeTable.move.Equals(new PointP(0, 1)))
                {
                    snakeTable.move = new PointP(1, 0);
                }
                else if (snakeTable.move.Equals(new PointP(1, 0)))
                {
                    snakeTable.move = new PointP(0, -1);
                }
                else if (snakeTable.move.Equals(new PointP(0, -1)))
                {
                    snakeTable.move = new PointP(-1, 0);
                }
                else if (snakeTable.move.Equals(new PointP(-1, 0)))
                {
                    snakeTable.move = new PointP(0, 1);
                }
                canSnakeTurn = false;
            }
        }
        #endregion

        //#region Eltelt idő
        //public int ElapsedTime()
        //{
        //    DateTime now = DateTime.Now;
        //    return (int)(now - startTime).TotalSeconds - pausedSeconds;
        //}
        //#endregion

        #region A játék szüneteltetése
        public (bool, bool) PauseGame()
        {
            if (timer.Enabled)
            {
                timer.Stop();
                //pauseTime = DateTime.Now;
                return (false, true);
            }
            else
            {
                if (isGameOver) return (true, false);
                timer.Start();
                //DateTime now = DateTime.Now;
                //pausedSeconds += (int)(now - pauseTime).TotalSeconds;
                return (false, false);
            }
        }
        #endregion

        #region Timer leállítása
        private void TimerStop(object? sender, EventArgs e)
        {
            isGameOver = true;
            if (timer.Enabled)
            {
                timer.Stop();
            }
            OnGameOver();
        } 
        #endregion

    }
}
