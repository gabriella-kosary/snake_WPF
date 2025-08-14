using System;
using Snake.Persistence;
using Snake.Model;

namespace SnakeTest
{
    [TestClass]
    public class SnakeGameModelTest
    {
        SnakeGameModel game = new SnakeGameModel(new System.Timers.Timer(1000));

        [TestMethod]
        public void SnakeGameModelConstructorTest()
        {
            Assert.IsNotNull(game);
            Assert.IsNotNull(game.snakeTable);
            Assert.IsNotNull(game.timer);
            Assert.IsTrue(game.timer.AutoReset);
        }

        [TestMethod]
        public void NewGameTest()
        {
            int tableSize = 0;
            List<PointP> walls = new List<PointP>();
            PointP? egg = null;
            int eggCount = 0;

            bool IsGenerateTableInvoked = false;
            game.GenerateTable += (_, eventArgs) => 
            {
                IsGenerateTableInvoked = true;
                tableSize = eventArgs.TableSize;
                walls = eventArgs.Walls;
            };
            bool IsDrawEggInvoked = false;
            game.DrawEgg += (_, eventArgs) =>
            {
                IsDrawEggInvoked = true;
                egg = eventArgs.Egg;
                eggCount = eventArgs.EggCount;
            };

            game.NewGame("Input\\szint_15.txt");
            Assert.IsNotNull(game.snakeTable);
            Assert.AreEqual(game.snakeTable.tableSize, 15);
            Assert.AreEqual(game.snakeTable.walls.Count, 20);
            Assert.AreEqual(game.snakeTable.snake.Count, 5);
            Assert.AreEqual(game.snakeTable.move.x, -1);
            Assert.AreEqual(game.snakeTable.move.y, 0);
            Assert.AreEqual(game.snakeTable.eggCount, 0);
            Assert.IsTrue(game.canSnakeTurn);
            Assert.IsTrue(game.timer.Enabled);
            Assert.IsTrue(IsGenerateTableInvoked);
            Assert.AreEqual(tableSize, 15);
            Assert.AreEqual(walls.Count, 20);
            Assert.IsTrue(IsDrawEggInvoked);
            Assert.IsNotNull(egg);
            Assert.AreEqual(eggCount, 0);

            IsGenerateTableInvoked = false;
            IsDrawEggInvoked = false;
            egg = null;
            game.NewGame("Input\\szint_20.txt");
            Assert.IsNotNull(game.snakeTable);
            Assert.AreEqual(game.snakeTable.tableSize, 20);
            Assert.AreEqual(game.snakeTable.walls.Count, 34);
            Assert.AreEqual(game.snakeTable.snake.Count, 5);
            Assert.AreEqual(game.snakeTable.move.x, -1);
            Assert.AreEqual(game.snakeTable.move.y, 0);
            Assert.AreEqual(game.snakeTable.eggCount, 0);
            Assert.IsTrue(game.canSnakeTurn);
            Assert.IsTrue(game.timer.Enabled);
            Assert.IsTrue(IsGenerateTableInvoked);
            Assert.AreEqual(tableSize, 20);
            Assert.AreEqual(walls.Count, 34);
            Assert.IsTrue(IsDrawEggInvoked);
            Assert.IsNotNull(egg);
            Assert.AreEqual(eggCount, 0);

            IsGenerateTableInvoked = false;
            IsDrawEggInvoked = false;
            egg = null;
            game.NewGame("Input\\szint_25.txt");
            Assert.IsNotNull(game.snakeTable);
            Assert.AreEqual(game.snakeTable.tableSize, 25);
            Assert.AreEqual(game.snakeTable.walls.Count, 52);
            Assert.AreEqual(game.snakeTable.snake.Count, 5);
            Assert.AreEqual(game.snakeTable.move.x, -1);
            Assert.AreEqual(game.snakeTable.move.y, 0);
            Assert.AreEqual(game.snakeTable.eggCount, 0);
            Assert.IsTrue(game.canSnakeTurn);
            Assert.IsTrue(game.timer.Enabled);
            Assert.IsTrue(IsGenerateTableInvoked);
            Assert.AreEqual(tableSize, 25);
            Assert.AreEqual(walls.Count, 52);
            Assert.IsTrue(IsDrawEggInvoked);
            Assert.IsNotNull(egg);
            Assert.AreEqual(eggCount, 0);
        }

        [TestMethod]
        public void SnakeGrowTest()
        {
            game.NewGame("Input\\szint_20.txt");
            Assert.AreEqual(game.snakeTable.snake.Count, 5);
            for (int j = 0; j < 5; j++)
            {
                List<PointP> prevSnake = new List<PointP>();
                for (int i = 0; i < game.snakeTable.snake.Count; i++)
                {
                    prevSnake.Add(game.snakeTable.snake[i]);
                }
                game.SnakeGrow(game.snakeTable.snake, game.snakeTable.move);
                Assert.AreEqual(game.snakeTable.snake.Count, prevSnake.Count + 1);
                Assert.AreEqual(game.snakeTable.snake[game.snakeTable.snake.Count - 1].AddPoint(game.snakeTable.move).x, prevSnake[prevSnake.Count - 1].x);
                Assert.AreEqual(game.snakeTable.snake[game.snakeTable.snake.Count - 1].AddPoint(game.snakeTable.move).y, prevSnake[prevSnake.Count - 1].y);
            }
        }

        [TestMethod]
        public void DrawEggTest()
        {
            PointP? egg = null;
            int eggCount = 0;
            bool IsDrawEggInvoked = false;
            game.DrawEgg += (_, eventArgs) =>
            {
                IsDrawEggInvoked = true;
                egg = eventArgs.Egg;
                eggCount = eventArgs.EggCount;
            };

            game.NewGame("Input\\szint_20.txt");
            EggWasEatenEventArgs ewe = new EggWasEatenEventArgs(game.snakeTable.egg, game.snakeTable.snake, game.snakeTable.move, game.snakeTable.eggCount);
            game.ManageEgg(null, ewe);
            Assert.IsTrue(IsDrawEggInvoked);
            Assert.IsNotNull(egg);
            Assert.AreEqual(eggCount, 0);
        }

        [TestMethod]
        public void TurnSnakeTest()
        {
            game.NewGame("Input\\szint_20.txt");

            Assert.AreEqual(game.snakeTable.snake[0].x, 10);
            Assert.AreEqual(game.snakeTable.snake[0].y, 10);

            game.TurnSnakeRight();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 10);
            Assert.AreEqual(game.snakeTable.snake[0].y, 11);
            game.canSnakeTurn = true;

            game.TurnSnakeRight();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 11);
            Assert.AreEqual(game.snakeTable.snake[0].y, 11);
            game.canSnakeTurn = true;

            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 12);
            Assert.AreEqual(game.snakeTable.snake[0].y, 11);

            game.TurnSnakeRight();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 12);
            Assert.AreEqual(game.snakeTable.snake[0].y, 10);
            game.canSnakeTurn = true;

            game.TurnSnakeRight();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 11);
            Assert.AreEqual(game.snakeTable.snake[0].y, 10);
            game.canSnakeTurn = true;

            game.TurnSnakeLeft();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 11);
            Assert.AreEqual(game.snakeTable.snake[0].y, 9);
            game.canSnakeTurn = true;

            game.TurnSnakeLeft();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 12);
            Assert.AreEqual(game.snakeTable.snake[0].y, 9);
            game.canSnakeTurn = true;

            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 13);
            Assert.AreEqual(game.snakeTable.snake[0].y, 9);

            game.TurnSnakeLeft();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 13);
            Assert.AreEqual(game.snakeTable.snake[0].y, 10);
            game.canSnakeTurn = true;

            game.TurnSnakeLeft();
            game.snakeTable.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(game.snakeTable.snake[0].x, 12);
            Assert.AreEqual(game.snakeTable.snake[0].y, 10);
            game.canSnakeTurn = true;
        }

        [TestMethod]
        public void PauseGameTest()
        {
            (bool igo, bool poc) b;
            game.NewGame("Input\\szint_20.txt");
            Assert.IsTrue(game.timer.Enabled);
            b = game.PauseGame();
            Assert.IsFalse(game.timer.Enabled);
            Assert.IsFalse(b.igo);
            Assert.IsTrue(b.poc);
            b = game.PauseGame();
            Assert.IsTrue(game.timer.Enabled);
            Assert.IsFalse(b.igo);
            Assert.IsFalse(b.poc);
            b = game.PauseGame();
            Assert.IsFalse(game.timer.Enabled);
            Assert.IsFalse(b.igo);
            Assert.IsTrue(b.poc);
            b = game.PauseGame();
            Assert.IsTrue(game.timer.Enabled);
            Assert.IsFalse(b.igo);
            Assert.IsFalse(b.poc);
        }

        [TestMethod]
        public void TimerStopTest()
        {
            bool IsTimerStopInvoked = false;
            game.snakeTable.GameOver += (_, eventArgs) =>
            {
                IsTimerStopInvoked = true;
            };

            game.NewGame("Input\\szint_15.txt");

            while(!IsTimerStopInvoked)
            {
                game.snakeTable.MoveSnake(null, EventArgs.Empty); 
            }
            Assert.IsTrue(IsTimerStopInvoked);
        }
    }
}