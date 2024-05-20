using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Security.Cryptography;
using System.Windows.Media.TextFormatting;

namespace WpfLibrary
{
    public class Bonus
    {
        public List<Point> list_of_bonus_grid_field { get; set; }
        public Canvas canvas { get; set; }
        public List<Image> list_bonus { get; set; }
        public BitmapImage speed_bonus_bitmap { get; set; }
        public BitmapImage slow_bonus_bitmap { get; set; }
        public BitmapImage buff_Muzzle { get; set; }
        public List<int> EmptyRow { get; set; }
        public List<int> EmptyColumn { get; set; }
        public Grid grid { get; set; }
        public int number_of_bonus { get; set; }
        public DispatcherTimer timer;
        private bool finish_background_operation = false;
        private Thread backgroundthread;
        private Field field;
        public Bonus(List<Point> list_of_bonus_grid_field,Canvas play_zone, int number_of_bonus, List<int> EmptyRow,List<int> EmptyColumn,Grid Grid_of_field, Field my_field)
        {
            field = my_field;
            list_bonus = new List<Image>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (s, e) => Timer_tick();
            this.grid = Grid_of_field;
            this.EmptyColumn = EmptyColumn;
            this.EmptyRow = EmptyRow;
            this.number_of_bonus = number_of_bonus;
            this.list_of_bonus_grid_field = list_of_bonus_grid_field;
            this.canvas = play_zone;
            speed_bonus_bitmap = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\speed_bonus.png", UriKind.RelativeOrAbsolute));
            slow_bonus_bitmap = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\slow_bonus.png", UriKind.RelativeOrAbsolute));
            buff_Muzzle = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\buff_muzzle.png", UriKind.RelativeOrAbsolute));
            InitializeAsync();
            
        }

        private void Timer_tick()
        {
            if(finish_background_operation == true)
            {
                backgroundthread.Join();
                timer.Stop();
            }
        }

        public void InitializeAsync()
        {
            finish_background_operation = false;
            backgroundthread = new Thread(new ThreadStart(SpawnBonus));
            backgroundthread.SetApartmentState(ApartmentState.STA);
            backgroundthread.IsBackground = true;
            backgroundthread.Start();
            timer.Start();
        }
        public void SpawnBonus()
        {
            Random rand_of_position = new Random();
            Random rand_of_bonus = new Random();
            Image img = new Image();
            Application.Current.Dispatcher.Invoke(() =>
            {
                for (int i = list_bonus.Count; i < number_of_bonus; i++)
                {
                    
                    img = new Image();
                    while (img.Source == null)
                    {
                        int random_position = rand_of_position.Next(EmptyRow.Count);
                        int random_bonus = rand_of_bonus.Next(1, 10);
                        if (random_bonus == 1)
                        {
                            img.Source = speed_bonus_bitmap;
                            img.Stretch = Stretch.Fill;
                            img.HorizontalAlignment = HorizontalAlignment.Stretch;
                            img.VerticalAlignment = VerticalAlignment.Stretch;

                        }
                        else if (random_bonus == 2)
                        {
                            img.Source = slow_bonus_bitmap;
                            img.Stretch = Stretch.Fill;
                            img.HorizontalAlignment = HorizontalAlignment.Stretch;
                            img.VerticalAlignment = VerticalAlignment.Stretch;
                        }
                        else if (random_bonus == 3)
                        {
                            img.Source = buff_Muzzle;
                            img.Stretch = Stretch.Fill;
                            img.HorizontalAlignment = HorizontalAlignment.Stretch;
                            img.VerticalAlignment = VerticalAlignment.Stretch;
                        }
                        if (img.Source != null)
                        {
                            Grid.SetRow(img, EmptyRow[random_position]);
                            Grid.SetColumn(img, EmptyColumn[random_position]);
                            list_bonus.Add(img);
                            grid.Children.Add(img);
                        }
                    }
                    
                    
                }


            });
            finish_background_operation = true;
        }
        public List<Point> CalcAllPoints_of_bonus()
        {
            List<Point> temp_points = new List<Point>();
            foreach (var texture in list_bonus)
            {
                temp_points.AddRange(field.Find_All_Points_of_texture(texture));
            }
            return temp_points;
        }
       
    }
}
