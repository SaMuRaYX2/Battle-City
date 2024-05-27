using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace WpfLibrary
{
    public class Bullet
    {
        public Shape Shot { get; set; }
        protected Canvas Play_zone { get; set; }
        protected Tank My_tank { get; set; }
        protected Point Locate_mouse { get; set; }
        protected Grid grid_bullet { get; set; }
        protected double degrees { get; set; }
        protected Point point_to_ballet { get; set; }
        public string side_of_rotate_tank { get; set; }
        public List<Point> points_denied { get; set; }
        protected List<Oponent> oponents { get; set; }
        public DispatcherTimer timer {private get; set; }
        public Tank Tank { get; set; }
        
        public Creating_oponent creating_oponent {private get; set; }
        public int damage { get; set; }
        public void GetMainTank( Tank my_tank)
        {
            this.Tank = my_tank;
        }
        public Tank ReturnMainTank()
        {
            return this.Tank;
        }
        public Bullet(Canvas canvas, Tank my_tank, Shape UP_muzzle, Point point_center_tank, string side, double degrees, List<Point> Points, int damage)
        {
            this.damage = damage;
            points_denied = Points;
            point_to_ballet = point_center_tank;
            side_of_rotate_tank = side;
            this.degrees = degrees;
            grid_bullet = new Grid();
            grid_bullet.Width = 5;
            grid_bullet.Height = 10;
            for (int i = 0; i < 5; i++)
            {
                grid_bullet.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid_bullet.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            Play_zone = canvas;
            My_tank = my_tank;
            //Locate_mouse = locate_mouse;
            double width_one_row = grid_bullet.Width / grid_bullet.ColumnDefinitions.Count;
            double height_one_column = grid_bullet.Height / grid_bullet.RowDefinitions.Count;
            Path path = new Path();
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, height_one_column * 5);
            figure.Segments.Add(new LineSegment(new Point(0, height_one_column * 2), true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row * 2.5, 0), new Size(width_one_row * 2.5, height_one_column * 2), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row * 5, height_one_column * 2), new Size(width_one_row * 2.5, height_one_column * 2), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new LineSegment(new Point(width_one_row * 5, height_one_column * 5), true));
            figure.Segments.Add(new LineSegment(new Point(0, height_one_column * 5), true));
            geometry.Figures.Add(figure);
            path.Data = geometry;
            path.Fill = Brushes.Yellow;
            Shot = path;
            grid_bullet.Children.Add(Shot);



            // Position of Bullet;
            grid_bullet.UpdateLayout();

            // Position of Bullet;
            


        }
        public void Make_BOOM_From_Muzzle()
        {
            Grid boom_grid = new Grid();
            //Border border = new Border();
            //border.BorderThickness = new Thickness(5);
            //border.BorderBrush = Brushes.Black;
            //boom_grid.Children.Add(border);
            boom_grid.Width = 20;
            boom_grid.Height = 20;
            double offset = -My_tank.Height;
            // Створення Image для вибуху з дула;
            Image explosion = new Image();
            explosion.Width = boom_grid.Width;
            explosion.Height = boom_grid.Height;
            boom_grid.Children.Add(explosion);
            TransformGroup group = new TransformGroup();
            TranslateTransform translate = new TranslateTransform(0, offset);
            RotateTransform rotate = new RotateTransform(degrees);
            rotate.CenterX = boom_grid.Width / 2;
            rotate.CenterY = boom_grid.Height / 2;
            group.Children.Add(translate);
            group.Children.Add(rotate);
            boom_grid.RenderTransform = group;
            Canvas.SetLeft(boom_grid, point_to_ballet.X - boom_grid.Width / 2);
            Canvas.SetTop(boom_grid, point_to_ballet.Y - boom_grid.Height/2);
            Play_zone.Children.Add(boom_grid);
            //Створення анімації
            var storyboard = new Storyboard();
            storyboard.FillBehavior = FillBehavior.Stop;
            //Створення ключових кадрів
            var animation = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(animation, explosion);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Source"));
            //Додавання ключових кадрів;
            for (int i = 1; i <= 16; i++)
            {
                var keyFrame = new DiscreteObjectKeyFrame   
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * 50)),
                    Value = new BitmapImage(new Uri($"D:\\My Homework\\Cursova\\Texture_image\\muzzle_boom\\muzzle_cadr{i}.png"))
                };
                animation.KeyFrames.Add(keyFrame);
            }
            //Додаємо анімацію до StoryBoard;
            storyboard.Children.Add(animation);
            //Видаляємо вибух після закінчення анімації;
            storyboard.Completed += (s, e) =>
            {
                boom_grid.Children.Remove(explosion);
                Play_zone.Children.Remove(boom_grid);
            };
            //Запуск анімації;
            storyboard.Begin();
        }
        public void Make_Boom_tank(Point position_of_bullet)
        {
            Grid boom_grid = new Grid();
            boom_grid.Width = 50;
            boom_grid.Height = 50;
            
            Image explosion = new Image();
            explosion.Width = boom_grid.Width;
            explosion.Height = boom_grid.Height;
            boom_grid.Children.Add(explosion);
            Canvas.SetLeft(boom_grid, position_of_bullet.X - boom_grid.Width / 2);
            Canvas.SetTop(boom_grid, position_of_bullet.Y - boom_grid.Height / 2);
            Play_zone.Children.Add(boom_grid);
            var storyboard = new Storyboard();
            storyboard.FillBehavior = FillBehavior.Stop;

            var animation = new ObjectAnimationUsingKeyFrames();
            Storyboard.SetTarget(animation, explosion);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Source"));
            for(int i = 1; i <= 5; i++)
            {
                var keyFrame = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * 300)),
                    Value = new BitmapImage(new Uri($"D:\\My Homework\\Cursova\\Texture_image\\tank_boom\\cadr{i}_boom.png"))
                };
                animation.KeyFrames.Add(keyFrame);
            }
            storyboard.Children.Add(animation);
            storyboard.Completed += (s, e) =>
            {
                boom_grid.Children.Remove(explosion);
                Play_zone.Children.Remove(boom_grid);
            };
            storyboard.Begin();
        }

        

        public void GetOponents(List<Oponent> all_oponents)
        {
            this.oponents = all_oponents;
        }
        public virtual async Task Make_a_shot()
        {

            Point position_of_bullet;

            double offsetY = -My_tank.Height;
            TranslateTransform translateTransform = new TranslateTransform(0, offsetY);
            RotateTransform rotate = new RotateTransform(degrees);
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotate);
            grid_bullet.RenderTransform = transformGroup;
            Grid.SetRow(Shot, 0);
            Grid.SetColumn(Shot, 0);
            Grid.SetRowSpan(Shot, 5);
            Grid.SetColumnSpan(Shot, 5);
            Canvas.SetLeft(grid_bullet, point_to_ballet.X - grid_bullet.Width / 2);
            Canvas.SetTop(grid_bullet, point_to_ballet.Y);
            Play_zone.Children.Add(grid_bullet);
           
            Make_BOOM_From_Muzzle();
            Play_zone.UpdateLayout();
            bool bullet_test = true;
            while (bullet_test)
            {
                Play_zone.UpdateLayout();
                position_of_bullet = grid_bullet.TransformToAncestor(Play_zone).Transform(new Point(0, 0));
                for (int i = 0; i < points_denied.Count; i++)
                {
                    if (Math.Round(position_of_bullet.X) == points_denied[i].X && Math.Round(position_of_bullet.Y) == points_denied[i].Y)
                    {
                        bullet_test = false;
                        int index_of_Shot = Play_zone.Children.IndexOf(grid_bullet);
                        if (index_of_Shot != -1)
                        {
                            Play_zone.Children.RemoveAt(index_of_Shot);
                            Make_Boom_tank(position_of_bullet);
                        }
                        //MessageBox.Show("Пуля", "Кінець польту пулі", MessageBoxButton.OK, MessageBoxImage.Stop);
                        break;
                    }
                }
                if (bullet_test)
                {
                    bool test_to_fire = false;
                    offsetY -= 10;
                    translateTransform.Y = offsetY;
                    Play_zone.InvalidateVisual();
                    test_to_fire = Test_To_KILL(position_of_bullet);
                    if (test_to_fire)
                    {

                        bullet_test = false;
                    }
                    await Task.Delay(16);
                }
            }

        }
        public virtual bool Test_To_KILL(Point position_of_bullet)
        {
            bool test_to_kill = false;
            for (int i = 0; i < oponents.Count; i++)
            {
                for (int j = 0; j < oponents[i].points_of_tanks.Count; j++)
                {
                    if (Math.Round(position_of_bullet.X) == oponents[i].points_of_tanks[j].X && Math.Round(position_of_bullet.Y) == oponents[i].points_of_tanks[j].Y)
                    {
                        test_to_kill = true;
                        break;
                    }
                    if(test_to_kill == true)
                    {
                        break;
                    }
                }
                if (test_to_kill == true)
                {
                    
                    int index_of_Shot = Play_zone.Children.IndexOf(grid_bullet);
                    if (index_of_Shot != -1)
                    {
                        Play_zone.Children.RemoveAt(index_of_Shot);
                    }
                    oponents[i].Health -= damage;
                    if (oponents[i].Health <= 0)
                    {
                        Destroy_tank(oponents[i]);
                        Make_Boom_tank(position_of_bullet);
                        Tank.Killed_Oponents.Add(oponents[i].Tank_grid);
                        oponents.RemoveAt(i);
                        Play_zone.UpdateLayout();
                        creating_oponent.BackGroundCreatingBots();
                        if (timer.IsEnabled == false)
                        {
                            timer.Start();
                        }
                    }
                    else if (oponents[i].Health < 100 && oponents[i].Health >= 0)
                    {
                        Type_of_Damage_Tank(oponents[i]);
                    }
                    
                    break;
                }
            }
            return test_to_kill;
        }
        public void Destroy_tank(Tank tank)
        {
            tank.Main_tank.Fill = Brushes.DarkRed;
            TransformGroup group_for_right = new TransformGroup();
            TranslateTransform translate_right = new TranslateTransform(tank.Tank_grid.ActualWidth / 4, 0);
            TranslateTransform translate_left = new TranslateTransform(-tank.Tank_grid.ActualWidth / 4, 0);
            RotateTransform chassi_right_rotate = new RotateTransform(25);
            RotateTransform chassi_left_rotate = new RotateTransform(-20);
            group_for_right.Children.Add(translate_right);
            group_for_right.Children.Add(chassi_right_rotate);
            tank.Chassis_right.RenderTransform = group_for_right;
            TransformGroup group_for_left = new TransformGroup();
            group_for_left.Children.Add(translate_left);
            group_for_left.Children.Add(chassi_left_rotate);
            tank.Chassis_left.RenderTransform = group_for_left;
            tank.UP_Muzzle_tank.Fill = Brushes.SaddleBrown;
            RotateTransform rotate_muzzle = new RotateTransform(60);
            TranslateTransform translate_muzzle = new TranslateTransform(tank.UP_Muzzle_tank.ActualHeight, tank.UP_Muzzle_tank.ActualHeight/2);
            TransformGroup group_muzzle = new TransformGroup();
            group_muzzle.Children.Add(rotate_muzzle);
            group_muzzle.Children.Add(translate_muzzle);
            tank.UP_Muzzle_tank.RenderTransform = group_muzzle;
        }
        public void Type_of_Damage_Tank(Tank tank)
        {
            if (tank.Health == 75)
            {
                tank.Main_tank.Fill = Brushes.PeachPuff;
                tank.UP_Muzzle_tank.Fill = Brushes.DarkBlue;
                TransformGroup group_for_right = new TransformGroup();
                TranslateTransform translate_right = new TranslateTransform(tank.Tank_grid.ActualWidth /16, 0);
                TranslateTransform translate_left = new TranslateTransform(-tank.Tank_grid.ActualWidth / 16, 0);
                RotateTransform chassi_right_rotate = new RotateTransform(2);
                RotateTransform chassi_left_rotate = new RotateTransform(-2);
                group_for_right.Children.Add(translate_right);
                group_for_right.Children.Add(chassi_right_rotate);
                tank.Chassis_right.RenderTransform = group_for_right;
                TransformGroup group_for_left = new TransformGroup();
                group_for_left.Children.Add(translate_left);
                group_for_left.Children.Add(chassi_left_rotate);
                tank.Chassis_left.RenderTransform = group_for_left;
            }
            else if(tank.Health == 50)
            {
                tank.Main_tank.Fill = Brushes.Peru;
                tank.UP_Muzzle_tank.Fill = Brushes.Maroon;
                TransformGroup group_for_right = new TransformGroup();
                TranslateTransform translate_right = new TranslateTransform(tank.Tank_grid.ActualWidth / 12, 0);
                TranslateTransform translate_left = new TranslateTransform(-tank.Tank_grid.ActualWidth / 12, 0);
                RotateTransform chassi_right_rotate = new RotateTransform(5);
                RotateTransform chassi_left_rotate = new RotateTransform(-5);
                group_for_right.Children.Add(translate_right);
                group_for_right.Children.Add(chassi_right_rotate);
                tank.Chassis_right.RenderTransform = group_for_right;
                TransformGroup group_for_left = new TransformGroup();
                group_for_left.Children.Add(translate_left);
                group_for_left.Children.Add(chassi_left_rotate);
                tank.Chassis_left.RenderTransform = group_for_left;
            }
            else if(tank.Health == 25)
            {
                tank.Main_tank.Fill = Brushes.DarkRed;
                tank.UP_Muzzle_tank.Fill = Brushes.Sienna;
                TransformGroup group_for_right = new TransformGroup();
                TranslateTransform translate_right = new TranslateTransform(tank.Tank_grid.ActualWidth / 8, 0);
                TranslateTransform translate_left = new TranslateTransform(-tank.Tank_grid.ActualWidth / 8, 0);
                RotateTransform chassi_right_rotate = new RotateTransform(10);
                RotateTransform chassi_left_rotate = new RotateTransform(-10);
                group_for_right.Children.Add(translate_right);
                group_for_right.Children.Add(chassi_right_rotate);
                tank.Chassis_right.RenderTransform = group_for_right;
                TransformGroup group_for_left = new TransformGroup();
                group_for_left.Children.Add(translate_left);
                group_for_left.Children.Add(chassi_left_rotate);
                tank.Chassis_left.RenderTransform = group_for_left;
            }
        }
    }
}
