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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace Wpf_Cursova
{
    /// <summary>
    /// Interaction logic for Result_of_Game.xaml
    /// </summary>
    public partial class Result_of_Game : Window
    {
        public string Name_of_winner { get; set; }
        public Result_of_Game()
        {
            InitializeComponent();
            Fill_Window();
            exit_to_menu.MouseLeftButtonDown += Exit_to_menu_MouseLeftButtonDown;
           
        }
        public void Enter_Winner_Name()
        {
            if (!string.IsNullOrEmpty(Name_of_winner))
            {
                winner_of_game.Text = $"The Winner is {Name_of_winner}";
            }
            else
            {
                winner_of_game.Text = "";
            }
        }

        private void Exit_to_menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StartMenu startMenu = new StartMenu();
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1,0,TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, a) =>
            {
                startMenu.Show();
                foreach(Window window in Application.Current.Windows)
                {
                    if(window is MainWindow)
                    {
                        window.Close();
                        break;
                    }
                }
                this.Close();
                
                DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                startMenu.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
            };
            this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
        }

        public void Fill_Window()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("D:\\My Homework\\Cursova\\Texture_image\\background_result.gif", UriKind.RelativeOrAbsolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(background_gif, image);

        }
    }
}
