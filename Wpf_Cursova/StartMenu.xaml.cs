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
using System.Windows.Media.Animation;
using NAudio.Wave;
using NAudio.Vorbis;


namespace Wpf_Cursova
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Window
    {
        private bool test_to_enable_sound_main_menu = true;
        public IWavePlayer waveOutDevice;
        public WaveStream mainOutputStream;
        public WaveChannel32 volumeStream;
        public float initialVolume = 1.0f;
        private string filePath;
        public StartMenu()
        {
            InitializeComponent();
            filePath = @"D:\My Homework\Cursova\Texture_image\main_menu_sound.ogg";
            hide_window.MouseLeftButtonDown += Hide_window_Click;
            this.Activated += StartMenu_Activated;
            Fill_Windows();
            exit_game.MouseLeftButtonDown += Exit_game_MouseLeftButtonDown;
            start_game_two_player.MouseLeftButtonDown += Start_game_two_player_MouseLeftButtonDown;
            start_game_one_player.MouseLeftButtonDown += Start_game_one_player_MouseLeftButtonDown;
            start_multiplayer.MouseLeftButtonDown += Start_multiplayer_MouseLeftButtonDown;
            grid_of_window.MouseLeftButtonDown += Grid_of_window_MouseLeftButtonDown;
            sound_icon.MouseLeftButtonDown += Sound_icon_MouseLeftButtonDown;
            PlayBackgroundMusic(filePath);
            this.Closed += StartMenu_Closed;
        }

        protected void StartMenu_Closed(object sender, EventArgs e)
        {
            waveOutDevice.Stop();
            mainOutputStream.Dispose();
            waveOutDevice.Dispose();
        }

        private void PlayBackgroundMusic(string filename)
        {
            waveOutDevice = new WaveOut();
            mainOutputStream = CreateInputStream(filename);
            waveOutDevice.Init(mainOutputStream);
            waveOutDevice.Play();
        }
        private WaveStream CreateInputStream(string filename)
        {
            if (filename.EndsWith(".ogg"))
            {
                var vorbisReader = new VorbisWaveReader(filename);
                volumeStream = new WaveChannel32(vorbisReader);
                volumeStream.Volume = initialVolume;
            }
            else
            {
                throw new InvalidOperationException("Unsupported extension");
            }
            return volumeStream;
        }
        
        private void Sound_icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(test_to_enable_sound_main_menu == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\silence_icon.png", UriKind.RelativeOrAbsolute));
                sound_icon.Stretch = Stretch.Fill;
                sound_icon.Source = bitmap;
                if(volumeStream != null)
                {
                    volumeStream.Volume = 0.0f;
                }
                test_to_enable_sound_main_menu = false;
            }
            else
            {
                
                BitmapImage bitmap = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\sound_icon.png", UriKind.RelativeOrAbsolute));
                sound_icon.Stretch = Stretch.Fill;
                sound_icon.Source = bitmap;
                if(volumeStream != null)
                {
                    volumeStream.Volume = initialVolume;
                }
                test_to_enable_sound_main_menu = true;
            }
        }

        private void Grid_of_window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Start_multiplayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, a) =>
            {
                mainWindow.Show();
                this.Close();
                DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                mainWindow.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
                mainWindow.Start_game_for_multiplayer();
            };
            this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
        }

        private void Start_game_one_player_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnterInformationGamer enterInformationGamer = new EnterInformationGamer();
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, a) =>
            {
                enterInformationGamer.Show();
                enterInformationGamer.player_2.Text = "You choose game for one";
                enterInformationGamer.player_2.FontSize = 18;
                enterInformationGamer.player_2.IsReadOnly = true;
                enterInformationGamer.Type_of_game = "Гра з ботом";
                this.Close();
                DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                enterInformationGamer.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
            };
            this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
        }

        private void Start_game_two_player_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EnterInformationGamer enterInformationGamer = new EnterInformationGamer();
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, a) =>
            {
                enterInformationGamer.Show();
                enterInformationGamer.Type_of_game = "Гра на двох";
                this.Close();
                DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                enterInformationGamer.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
            };
            this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
        }

        private void Exit_game_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
            fadeOutAnimation.Completed += (s, a) =>
            {
                this.Close();
            };
            this.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
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
