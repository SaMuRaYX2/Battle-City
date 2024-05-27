using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Security.Policy;

namespace WpfLibrary
{
    public class Bullet_Oponent : Bullet
    {
        public bool bot_type { get; set; }
        public Bullet_Oponent(Canvas canvas, Oponent my_tank, Shape UP_muzzle, Point point_center_tank, string side, double degrees, List<Point> Points, int damage) : base(canvas, my_tank, UP_muzzle, point_center_tank, side, degrees, Points, damage)
        {

        }

        public override async Task Make_a_shot()
        {
            RotateTransform rotate = new RotateTransform(degrees);
            if (bot_type == false)
            {
                if (side_of_rotate_tank == "UP")
                {
                    rotate.Angle = 0;
                    degrees = 0;
                }
                else if (side_of_rotate_tank == "DOWN")
                {
                    rotate.Angle = 180;
                    degrees = 180;
                }
                else if (side_of_rotate_tank == "LEFT")
                {
                    rotate.Angle = -90;
                    degrees = -90;
                }
                else if (side_of_rotate_tank == "RIGHT")
                {
                    rotate.Angle = 90;
                    degrees = 90;

                }
            }
            Point position_of_bullet;
            double offsetY = - My_tank.Height;
            TranslateTransform translateTransform = new TranslateTransform(0, offsetY);
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
                    offsetY -= 10;
                    translateTransform.Y = offsetY;
                    grid_bullet.InvalidateVisual();
                    bool test_to_fire = false;
                    test_to_fire = Test_To_KILL(position_of_bullet);
                    if (test_to_fire)
                    {
                        bullet_test = false;
                    }
                    await Task.Delay(16);
                }
            }
        }
        public override bool Test_To_KILL(Point position_of_bullet)
        {
            bool test_to_kill = false;
            if (Tank != null)
            {
                for (int j = 0; j < Tank.points_of_tanks.Count; j++)
                {
                    if (Math.Round(position_of_bullet.X) == Tank.points_of_tanks[j].X && Math.Round(position_of_bullet.Y) == Tank.points_of_tanks[j].Y)
                    {
                        test_to_kill = true;
                        break;
                    }
                    if (test_to_kill == true)
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
                    Tank.Health -= damage;
                    if (Tank.Health <= 0)
                    {
                        Destroy_tank(Tank);
                        Make_Boom_tank(position_of_bullet);
                        Tank = null;
                    }
                    else if (Tank.Health < 100 && Tank.Health > 0)
                    {
                        Type_of_Damage_Tank(Tank);
                    }
                    
                    Play_zone.UpdateLayout();
                }
            }
            return test_to_kill;
        }
    }
}
