using Snake.Model;
using Snake.Persistence;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Windows;
using System.Windows.Threading;

namespace SnakeWPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private SnakeGameModel _game;
        private int _tableSize;
        private int _eggCount;
        private int _seconds;
        private bool _paused;
        public string PausedText
        {
            get => _paused ? "Folytatás" : "Szünet";
        }
        public ObservableCollection<TableCell> Table {  get; set; }
        public int TableSize 
        {
            get => _tableSize;
            private set
            {
                _tableSize = value;
                OnPropertyChanged();
            }
        }
        public int EggCount 
        {
            get => _eggCount;
            private set
            {
                _eggCount = value;
                OnPropertyChanged();
            }
        }
        public int Seconds
        {
            get => _seconds;
            private set
            {
                _seconds = value;
                OnPropertyChanged();
            }
        }
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand PauseGameCommand { get; set; }
        public DelegateCommand TurnSnakeCommand { get; set; }
        public MainViewModel(SnakeGameModel game)
        {
            Seconds = 0;
            EggCount = 0;
            _paused = false;
            _game = game;
            _game.GenerateTable += GenerateTable;
            _game.TimerTicks += TimerTicks;
            _game.DrawEgg += DrawEgg;
            _game.snakeTable.DrawSnake += DrawSnake;
            _game.snakeTable.RemoveEgg += RemoveEgg;

            Table = new ObservableCollection<TableCell>();

            NewGameCommand = new DelegateCommand(param =>
            {
                string? header;
                header = param as string;
                if (header != null)
                {
                    header = header.Split(' ')[0];
                    _game.NewGame($"Input\\szint_{header}.txt");
                }
            });

            PauseGameCommand = new DelegateCommand(_ =>
            {
                (bool isGameOver, bool isTimerStoped) paused = _game.PauseGame();
                if (!paused.isGameOver)
                { 
                    _paused = paused.isTimerStoped;
                    OnPropertyChanged(nameof(PausedText));
                    OnPropertyChanged(nameof(Table));
                }
            });
            
            TurnSnakeCommand = new DelegateCommand(param =>
            {
                string? key;
                key = param as string;
                if (key == "Left") _game.TurnSnakeLeft();
                else if (key == "Right") _game.TurnSnakeRight();
            });
        }

        private void TimerTicks(object? sender, int seconds)
        {
            Seconds = seconds;
            OnPropertyChanged(nameof(Table));
        }

        private void RemoveEgg(object? sender, PointP egg)
        {
            int k = egg.x * _tableSize + egg.y;
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Table[k] = new TableCell
                {
                    IsWall = false,
                    IsSnake = false,
                    IsEgg = false
                };
            });

            OnPropertyChanged(nameof(Table));
        }
        private void DrawEgg(object? sender, DrawEggEventArgs eventArgs)
        {
            EggCount = eventArgs.EggCount;
            int k = eventArgs.Egg.x * _tableSize + eventArgs.Egg.y;
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Table[k] = new TableCell
                {
                    IsWall = false,
                    IsSnake = false,
                    IsEgg = true
                };
            });
        }
        private void DrawSnake(object? sender, DrawSnakeEventArgs eventArgs)
        {
            int k;
            foreach (PointP p in eventArgs.Snake)
            {
                k = p.x * _tableSize + p.y;
                //Table[k].IsSnake = true;

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Table[k] = new TableCell
                    {
                        IsWall = false,
                        IsSnake = true,
                        IsEgg = false
                    };
                    k = eventArgs.Tail.x * _tableSize + eventArgs.Tail.y;
                    if (k >= 0 && k < _tableSize * _tableSize && !Table[k].IsWall && !Table[k].IsEgg)
                    {
                        Table[k] = new TableCell
                        {
                            IsWall = false,
                            IsSnake = false,
                            IsEgg = false
                        };
                    }
                });
            }

            OnPropertyChanged(nameof(Table));
        }
        private void GenerateTable(object? sender, GenerateTableEventArgs eventArgs)
        {
            Table.Clear();
            TableSize = eventArgs.TableSize;
            for (int i = 0; i < _tableSize * _tableSize; i++)
            {
                Table.Add(new TableCell
                {
                    IsEgg = false,
                    IsWall = false,
                    IsSnake = false
                });
            }

            int k;
            foreach (PointP p in eventArgs.Walls)
            {
                k = p.x * _tableSize + p.y;
                Table[k].IsWall = true;
            }
            //foreach (PointP p in _game.snakeTable.snake)
            //{
            //    k = p.x * _tableSize + p.y;
            //    Table[k].IsSnake = true;
            //}

            OnPropertyChanged(nameof(Table));
        }
    }
}
