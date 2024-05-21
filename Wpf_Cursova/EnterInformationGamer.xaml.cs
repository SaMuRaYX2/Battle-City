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
    /// Interaction logic for EnterInformationGamer.xaml
    /// </summary>
    public partial class EnterInformationGamer : Window
    {
        public string Name_of_player_1 { get; set; }
        public string Name_of_player_2 { get; set; }
        public string Type_of_game { get; set; }
        public EnterInformationGamer()
        {
            InitializeComponent();
            Fill_Window();
            player_1.GotFocus += Player_1_GotFocus;
            player_1.LostFocus += Player_1_LostFocus;
            player_2.GotFocus += Player_2_GotFocus;
            player_2.LostFocus += Player_2_LostFocus;
            arrow_next.MouseLeftButtonDown += Arrow_next_MouseLeftButtonDown;
        }

        private void Arrow_next_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            helper_focus.Focus();
            if (Type_of_game == "Гра на двох")
            {
                if (!string.IsNullOrEmpty(Name_of_player_1) && !string.IsNullOrEmpty(Name_of_player_2))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Name_of_player_1 = this.Name_of_player_1;
                    mainWindow.Name_of_player_2 = this.Name_of_player_2;
                    DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                    fadeOutAnimation.Completed += (s, a) =>
                    {
                        mainWindow.Show();
                        this.Close();
                        DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                        mainWindow.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
                        mainWindow.Start_game_Click_for_two_player();
                    };
                    this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Name_of_player_1))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Name_of_player_1 = this.Name_of_player_1;
                    mainWindow.Name_of_player_2 = "Bot";
                    DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                    fadeOutAnimation.Completed += (s, a) =>
                    {
                        mainWindow.Show();
                        this.Close();
                        DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                        mainWindow.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
                        mainWindow.Start_game_for_one_player();
                    };
                    this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
                }
            }
        }

        private void Player_2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(player_2.Text) && player_2.Text != "Enter")
            {
                Name_of_player_2 = player_2.Text;
            }
            else
            {
                player_2.Text = "Enter Right";
            }
        }

        private void Player_2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (player_2.Text == "Enter" || player_2.Text == "Enter Right")
            {
                player_2.Text = "";
            }
        }

        private void Player_1_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(player_1.Text) && player_1.Text != "Enter")
            {
                Name_of_player_1 = player_1.Text;
            }
            else
            {
                player_1.Text = "Enter Right";
            }
        }

        private void Player_1_GotFocus(object sender, RoutedEventArgs e)
        {
            if(player_1.Text == "Enter" || player_1.Text == "Enter Right")
            {
                player_1.Text = "";
            }
        }

        public void Fill_Window()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("D:\\My Homework\\Cursova\\Texture_image\\background_enter_window.gif", UriKind.RelativeOrAbsolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(background_gif, image);
        }
    }
}
