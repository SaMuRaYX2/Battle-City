﻿using System;
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


namespace Wpf_Cursova
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Window
    {
        public StartMenu()
        {
            InitializeComponent();
            hide_window.MouseLeftButtonDown += Hide_window_Click;
            this.Activated += StartMenu_Activated;
            Fill_Windows();
            exit_game.MouseLeftButtonDown += Exit_game_MouseLeftButtonDown;
            start_game_two_player.MouseLeftButtonDown += Start_game_two_player_MouseLeftButtonDown;
            start_game_one_player.MouseLeftButtonDown += Start_game_one_player_MouseLeftButtonDown;
            start_multiplayer.MouseLeftButtonDown += Start_multiplayer_MouseLeftButtonDown;
        }

        private void Start_multiplayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.Start_game_for_multiplayer();
            this.Close();
        }

        private void Start_game_one_player_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.Start_game_for_one_player();
            this.Close();
        }

        private void Start_game_two_player_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.Start_game_Click_for_two_player();
            this.Close();
        }

        private void Exit_game_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void StartMenu_Activated(object sender, EventArgs e)
        {

            this.WindowState = WindowState.Normal;
            this.Activate();

        }

        private void Hide_window_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.WindowState = WindowState.Minimized;
            }
        }
        private RadialGradientBrush Make_Some_Gradient()
        {
            RadialGradientBrush radialGradient = new RadialGradientBrush();
            radialGradient.GradientOrigin = new Point(0.5, 0.5);
            radialGradient.GradientStops.Add(new GradientStop(Colors.DarkGray, 0));
            radialGradient.GradientStops.Add(new GradientStop(Colors.DarkGray, 0.33));
            radialGradient.GradientStops.Add(new GradientStop(Colors.Black, 1));
            return radialGradient;

        }
        private void Fill_Windows()
        {
            this.UpdateLayout();
            double width_of_one_grid = this.Width / grid_of_window.ColumnDefinitions.Count;
            border_of_hiden_button.Background = Make_Some_Gradient();
            TransformGroup group_rotate_hide_button = new TransformGroup();
            RotateTransform rotate = new RotateTransform(90);
            TranslateTransform translate = new TranslateTransform(width_of_one_grid, 5);
            group_rotate_hide_button.Children.Add(rotate);
            group_rotate_hide_button.Children.Add(translate);
            border_of_hiden_button.RenderTransform = group_rotate_hide_button;
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("D:\\My Homework\\Cursova\\Texture_image\\Background_menu.gif", UriKind.RelativeOrAbsolute);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(background_gif, image);
            


        }
    }
}