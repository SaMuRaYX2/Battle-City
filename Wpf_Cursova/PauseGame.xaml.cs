using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;
using WpfLibrary;

namespace Wpf_Cursova
{
    /// <summary>
    /// Interaction logic for PauseGame.xaml
    /// </summary>
    public partial class PauseGame : Window
    {
        public bool exit_from_game { get; set; }
        public Bot my_bot { get; set; }
        public PauseGame()
        {
            InitializeComponent();
            Do_Some_Decoration();
            continue_game.MouseLeftButtonDown += Continue_game_MouseLeftButtonDown;
            exit_menu.MouseLeftButtonDown += Exit_menu_MouseLeftButtonDown;
            exit_from_game = false;
            
        }

        private void Exit_menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exit_from_game = true;
            if(my_bot != null)
            {
                my_bot.test_to_exit_pauseGame = exit_from_game;
            }
            StartMenu startMenu = new StartMenu();
            startMenu.Show();
            this.Close();
            foreach(Window window in Application.Current.Windows)
            {
                if(window is MainWindow mainWindow)
                {
                    window.Close();
                }
            }
        }

        private void Continue_game_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Do_Some_Decoration()
        {
            TextDecorationCollection textDecorations = new TextDecorationCollection();
            TextDecoration strikethrough = new TextDecoration();
            strikethrough.Location = TextDecorationLocation.Strikethrough;
            strikethrough.Pen = new Pen(Brushes.Maroon, 10);
            strikethrough.PenOffset = 5;
            textDecorations.Add(strikethrough);
            
            textblock_pause_game.TextDecorations = textDecorations;

            var imageUri = new Uri("D:\\My Homework\\Cursova\\Texture_image\\Background_PauseMenu.gif", UriKind.RelativeOrAbsolute);
            var image = new BitmapImage(imageUri);
            ImageBehavior.SetAnimatedSource(background_gif, image);
        }
    }
}
