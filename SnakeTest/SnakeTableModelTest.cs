using System;
using Snake.Persistence;
using Snake.Model;
using System.Reflection;
using System.Xml.Linq;
using System.Drawing;

namespace SnakeTest
{
    [TestClass]
    public class SnakeTableModelTest
    {
        SnakeTableModel table = new SnakeTableModel();

        [TestMethod]
        public void SnakeTableModelConstructorTest()
        {
            Assert.IsNotNull(table);
            Assert.IsNotNull(table.walls);
            Assert.IsNotNull(table.snake);
            Assert.AreEqual(table.move.x, -1);
            Assert.AreEqual(table.move.y, 0);
            Assert.IsNotNull(table.egg);
            Assert.IsNotNull(table.tail);
        }

        [TestMethod]
        public void NewTableTest()
        {
            table.NewTable(15);
            Assert.AreEqual(table.tableSize, 15);
            Assert.AreEqual(table.walls.Count, 0);
            Assert.AreEqual(table.snake.Count, 5);
            Assert.AreEqual(table.move.x, -1);
            Assert.AreEqual(table.move.y, 0);
            Assert.AreEqual(table.eggCount, 0);

            table.NewTable(20);
            Assert.AreEqual(table.tableSize, 20);
            Assert.AreEqual(table.walls.Count, 0);
            Assert.AreEqual(table.snake.Count, 5);
            Assert.AreEqual(table.move.x, -1);
            Assert.AreEqual(table.move.y, 0);
            Assert.AreEqual(table.eggCount, 0);

            table.NewTable(25);
            Assert.AreEqual(table.tableSize, 25);
            Assert.AreEqual(table.walls.Count, 0);
            Assert.AreEqual(table.snake.Count, 5);
            Assert.AreEqual(table.move.x, -1);
            Assert.AreEqual(table.move.y, 0);
            Assert.AreEqual(table.eggCount, 0);
        }

        [TestMethod]
        public void MoveSnakeTest()
        {
            List<PointP> snake = new List<PointP>();
            PointP tail = new PointP();

            bool IsEventGameOverInvoked = false;
            table.GameOver += (_, _) => 
            {
                IsEventGameOverInvoked = true;
            };
            bool IsDrawSnakeInvoked = false;
            table.DrawSnake += (_, eventArgs) => { 
                IsDrawSnakeInvoked = true;
                snake = eventArgs.Snake;
                tail = eventArgs.Tail;
            };


            // kimegy a pályáról
            table.NewTable(10);
            table.egg = new PointP(0,0);
            IsEventGameOverInvoked = false;
            IsDrawSnakeInvoked = false;

            Assert.AreEqual(table.snake[0].x, 5);
            Assert.AreEqual(table.snake[0].y, 5);
            for (int i = 0; i < 5; i++)
            {
                IsDrawSnakeInvoked = false;
                table.MoveSnake(null, EventArgs.Empty);
                Assert.AreEqual(table.snake[0].x, 5 - i - 1);
                Assert.AreEqual(table.snake[0].y, 5);
                Assert.IsTrue(IsDrawSnakeInvoked);
                Assert.AreEqual(snake[0].x, 5 - i - 1);
                Assert.AreEqual(snake[0].y, 5);
                Assert.AreEqual(tail.x, 5 - i + 4);
                Assert.AreEqual(tail.y, 5);
            }
            Assert.IsFalse(IsEventGameOverInvoked);
            Assert.AreEqual(table.snake[0].x, 0);
            Assert.AreEqual(table.snake[0].y, 5);
            IsDrawSnakeInvoked = false;
            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsTrue(IsEventGameOverInvoked);
            Assert.IsFalse(IsDrawSnakeInvoked);
            Assert.AreEqual(table.snake[0].x, 0);
            Assert.AreEqual(table.snake[0].y, 5);


            // saját magába megy bele
            table.NewTable(10);
            table.egg = new PointP(0, 0);
            IsEventGameOverInvoked = false;
            IsDrawSnakeInvoked = false;

            Assert.AreEqual(table.snake[0].x, 5);
            Assert.AreEqual(table.snake[0].y, 5);
            
            table.move = new PointP(0, 1);
            table.MoveSnake(null, EventArgs.Empty);
            Assert.AreEqual(table.snake[0].x, 5);
            Assert.AreEqual(table.snake[0].y, 6);
            Assert.IsTrue(IsDrawSnakeInvoked);
            Assert.AreEqual(snake[0].x, 5);
            Assert.AreEqual(snake[0].y, 6);
            Assert.AreEqual(tail.x, 9);
            Assert.AreEqual(tail.y, 5);

            IsDrawSnakeInvoked = false;
            table.move = new PointP(1, 0);
            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsFalse(IsEventGameOverInvoked);
            Assert.AreEqual(table.snake[0].x, 6);
            Assert.AreEqual(table.snake[0].y, 6);
            Assert.IsTrue(IsDrawSnakeInvoked);
            Assert.AreEqual(snake[0].x, 6);
            Assert.AreEqual(snake[0].y, 6);
            Assert.AreEqual(tail.x, 8);
            Assert.AreEqual(tail.y, 5);

            IsDrawSnakeInvoked = false;
            table.move = new PointP(0, -1);
            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsTrue(IsEventGameOverInvoked);
            Assert.IsFalse(IsDrawSnakeInvoked);
            Assert.AreEqual(table.snake[0].x, 6);
            Assert.AreEqual(table.snake[0].y, 6);

            IsDrawSnakeInvoked = false;
            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsTrue(IsEventGameOverInvoked);
            Assert.IsFalse(IsDrawSnakeInvoked);
            Assert.AreEqual(table.snake[0].x, 6);
            Assert.AreEqual(table.snake[0].y, 6);

            // nekimegy a falnak
            table.NewTable(10);
            table.walls.Add(new PointP(4, 5));
            table.egg = new PointP(0, 0);
            IsEventGameOverInvoked = false;
            IsDrawSnakeInvoked = false;

            Assert.IsFalse(IsEventGameOverInvoked);
            Assert.AreEqual(table.snake[0].x, 5);
            Assert.AreEqual(table.snake[0].y, 5);

            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsTrue(IsEventGameOverInvoked);
            Assert.IsFalse(IsDrawSnakeInvoked);
            Assert.AreEqual(table.snake[0].x, 5);
            Assert.AreEqual(table.snake[0].y, 5);

            IsDrawSnakeInvoked = false;
            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsTrue(IsEventGameOverInvoked);
            Assert.IsFalse(IsDrawSnakeInvoked);
            Assert.AreEqual(table.snake[0].x, 5);
            Assert.AreEqual(table.snake[0].y, 5);
        }

        [TestMethod]
        public void NewEggTest()
        {
            for (int i = 0; i < 5; i++)
            {
                PointP prevEgg = table.egg;
                table.NewEgg();
                Assert.AreNotEqual(prevEgg, table.egg);
                Assert.IsFalse(table.egg.IsInList(table.walls));
                Assert.IsFalse(table.egg.IsInList(table.snake));
            }
        }

        [TestMethod]
        public void EggWasEaten()
        {
            PointP egg = new PointP();
            List<PointP> snake = new List<PointP>();
            PointP move = new PointP();
            int eggCount = 0;

            bool IsEggWasEatenInvoked = false;
            table.EggWasEaten += (_, eventArgs) =>
            {
                IsEggWasEatenInvoked = true;
                egg = eventArgs.Egg;
                snake = eventArgs.Snake;
                move = eventArgs.Move;
                eggCount = eventArgs.EggCount;
            };
            bool IsRemoveEggInvoked = false;
            table.RemoveEgg += (_, eventArgs) =>
            {
                IsRemoveEggInvoked = true;
                egg = eventArgs;
            };

            table.NewTable(10);
            table.egg = new PointP(4, 5);

            table.MoveSnake(null, EventArgs.Empty);
            Assert.IsTrue(IsRemoveEggInvoked);
            Assert.IsTrue(egg.Equals(table.egg));
            Assert.IsTrue(IsEggWasEatenInvoked);
            Assert.IsTrue(egg.Equals(table.egg));
            Assert.IsTrue(move.Equals(table.move));
            Assert.AreEqual(eggCount, 1);

        }
    }
}