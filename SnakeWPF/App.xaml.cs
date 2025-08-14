using SnakeWPF.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using Snake.Model;
using SnakeWPF.View;

namespace SnakeWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel? _mainViewModel;
        private MainWindow? _mainWindow;
        private SnakeGameModel? _game;

        public App()
        {
            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _game = new SnakeGameModel(new System.Timers.Timer(1000));
            _mainViewModel = new MainViewModel(_game);
            _mainWindow = new MainWindow();
            _mainWindow.DataContext = _mainViewModel;

            _game.GameOver += GameOver;

            _mainWindow.Show();
        }

        private void GameOver(object? sender, GameOverEventArgs eventArgs)
        {
            MessageBox.Show($"Játékkal töltött idő:\n" +
                $"{eventArgs.Seconds / 60} perc {eventArgs.Seconds % 60} másodperc\n" +
                $"Felszedett tojások száma:\n" +
                $"{eventArgs.EggCount} db", "Játék vége");
        }
    }

}
