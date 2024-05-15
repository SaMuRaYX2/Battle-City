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
        public Tank Tank { get; set; }

        public void GetMainTank( Tank my_tank)
        {
            this.Tank = my_tank;
        }
        public Tank ReturnMainTank()
        {
            return this.Tank;
        }
        public Bullet(Canvas canvas, Tank my_tank, Point locate_mouse, Shape UP_muzzle, Point point_center_tank, string side, double degrees, List<Point> Points)
        {
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
            Locate_mouse = locate_mouse;
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
                        }
                        //MessageBox.Show("Пуля", "Кінець польту пулі", MessageBoxButton.OK, MessageBoxImage.Stop);
                        break;
                    }
                }
                if (bullet_test)
                {
                    bool test_to_fire = false;
                    offsetY -= 4;
                    translateTransform.Y = offsetY;
                    Play_zone.InvalidateVisual();
                    test_to_fire = Test_To_KILL(position_of_bullet);
                    if (test_to_fire)
                    {
                        bullet_test = false;
                    }
                    await Task.Delay(10);
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
                    //Play_zone.Children.Remove(oponents[i].Tank_grid);
                    int index_of_Shot = Play_zone.Children.IndexOf(grid_bullet);
                    if (index_of_Shot != -1)
                    {
                        Play_zone.Children.RemoveAt(index_of_Shot);
                    }
                    Destroy_tank(oponents[i]);
                    
                    oponents.RemoveAt(i);
                    Play_zone.UpdateLayout();
                    break;
                }
            }
            return test_to_kill;
        }
        public void Destroy_tank(Tank tank)
        {
            tank.Main_tank.Fill = Brushes.Brown;
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
    }
}
