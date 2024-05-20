using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLibrary
{
    public class MovementOponent : MovementTank
    {
        //public MovementOponent(List<Point> points) :base(points)
        //{

        //}
        
        private double speed_of_tank = 3;
        bool initialization_of_timer = false;
        public int damage_of_oponent = 25;
        public void Press_W(Oponent my_tank)
        {
            if (side_of_move == "UP" && test_point == false)
            {
                return;
            }
            else
            {
                double speed = -speed_of_tank;

                string key = "UP";
                side_of_move = key;
                double value_width_grid = my_tank.Width;
                double value_height_grid = my_tank.Height;
                my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                int row_indeficator = Grid.GetRow(my_tank.indeficator);
                int column_indeficator = Grid.GetColumn(my_tank.indeficator);
                RotateTransform rotate = new RotateTransform(0);
                TRY_TO_ROTATE(my_tank, rotate);
                Move(speed, my_tank, key);
            }
        }
        public void Press_S(Oponent my_tank)
        {
            if (side_of_move == "DOWN" && test_point == false)
            {
                return;
            }
            else
            {
                string key = "DOWN";
                side_of_move = key;
                double speed = speed_of_tank;
                double value_width_grid = my_tank.Width;
                double value_height_grid = my_tank.Height;
                my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                int row_indeficator = Grid.GetRow(my_tank.indeficator);
                int column_indeficator = Grid.GetColumn(my_tank.indeficator);
                RotateTransform rotate = new RotateTransform(180);
                TRY_TO_ROTATE(my_tank, rotate);
                Move(speed, my_tank, key);
            }
        }
        public void Press_A(Oponent my_tank)
        {
            if (side_of_move == "LEFT" && test_point == false)
            {
                return;
            }
            else
            {
                string key = "LEFT";
                side_of_move = key;
                double speed = -speed_of_tank;
                double value_width_grid = my_tank.Width;
                double value_height_grid = my_tank.Height;
                my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                int row_indeficator = Grid.GetRow(my_tank.indeficator);
                int column_indeficator = Grid.GetColumn(my_tank.indeficator);
                RotateTransform rotate = new RotateTransform(-90);
                TRY_TO_ROTATE(my_tank, rotate);
                Move(speed, my_tank, key);
            }
        }
        public void Press_D(Oponent my_tank)
        {
            if (side_of_move == "RIGHT" && test_point == false)
            {
                return;
            }
            else
            {
                string key = "RIGHT";
                side_of_move = key;
                double speed = speed_of_tank;
                double value_width_grid = my_tank.Width;
                double value_height_grid = my_tank.Height;
                my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                int row_indeficator = Grid.GetRow(my_tank.indeficator);
                int column_indeficator = Grid.GetColumn(my_tank.indeficator);
                RotateTransform rotate = new RotateTransform(90);
                TRY_TO_ROTATE(my_tank, rotate);
                Move(speed, my_tank, key);
            }
        }
        public void TRY_TO_ROTATE(Oponent my_tank, RotateTransform rotate)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotate);
            my_tank.Tank_grid.RenderTransform = transformGroup;
        }
        public void Move(double speed, Oponent tank_move, string side)
        {
            Point point_center_of_tank_int_canvas = tank_move.Tank_grid.TransformToAncestor(canvas).Transform(new Point(tank_move.Width / 2, tank_move.Height / 2));
            point_for_bullet = point_center_of_tank_int_canvas;
            double default_positionX = tank_move.PositionX;
            double default_positionY = tank_move.PositionY;
            Test_to_Move(tank_move, side, speed);
            if (test_point == true)
            {
                Canvas.SetLeft(tank_move.Tank_grid, tank_move.PositionX);
                Canvas.SetTop(tank_move.Tank_grid, tank_move.PositionY);
                tank_move.points_of_tanks.Clear();
                tank_move.points_of_tanks = GetPointTank(tank_move);
            }
            else
            {
                tank_move.PositionX = default_positionX;
                tank_move.PositionY = default_positionY;
                if (side == "UP")
                {
                    speed = -1;
                }
                else if (side == "DOWN")
                {
                    speed = 1;
                }
                else if (side == "LEFT")
                {
                    speed = -1;
                }
                else if (side == "RIGHT")
                {
                    speed = 1;
                }


                Test_to_Move(tank_move, side, speed);
                if (test_point == true)
                {
                    Canvas.SetLeft(tank_move.Tank_grid, tank_move.PositionX);
                    Canvas.SetTop(tank_move.Tank_grid, tank_move.PositionY);
                    tank_move.points_of_tanks.Clear();
                    tank_move.points_of_tanks = GetPointTank(tank_move);
                }
                else
                {
                    tank_move.PositionX = default_positionX;
                    tank_move.PositionY = default_positionY;
                }
            }
        }

        public void Test_to_Move(Oponent tank_move, string side, double speed)
        {
            delegate_of_bonus delegate_bonus = my_field.bonus.CalcAllPoints_of_bonus;
            if (initialization_of_timer == false)
            {
                timer.Interval = TimeSpan.FromMilliseconds(10000);
                
                timer.Tick += (s, e) => TimerAction();
                initialization_of_timer = true;
            }
            if (side == "UP" || side == "DOWN")
            {
                tank_move.PositionY += speed;
            }
            if (side == "LEFT" || side == "RIGHT")
            {
                tank_move.PositionX += speed;
            }
            test_point = true;
            Point tank_position = new Point();
            if (side == "RIGHT" || side == "DOWN")
            {
                tank_position = new Point(tank_move.PositionX + tank_move.Width, tank_move.PositionY + tank_move.Height);
            }
            else
            {
                tank_position = new Point(tank_move.PositionX, tank_move.PositionY);
            }
            my_field.side_move = side;
            points_denied = new List<Point>();
            points_denied = my_field.GetAllPoints();
            points_denied.AddRange(my_field.GetPointForOponent());
            points_of_bonus = new List<Point>(delegate_bonus());
            points_denied.AddRange(points_of_bonus);
            
            foreach (var point in points_denied)
            {

                if (tank_position == point)
                {
                    UIElement element = GetUIElementAtPoint(my_field.Grid_field, point);
                    if (element != null)
                    {
                        if (DoSomeAction_with_bonus(element))
                        {
                            my_field.Grid_field.Children.Remove(element);
                            my_field.bonus.list_bonus.Remove((Image)element);
                            my_field.bonus.InitializeAsync();
                            test_point = false;
                            break;
                        }
                        else
                        {
                            test_point = false;
                            break;
                        }
                    }
                    
                    else
                    {
                        test_point = false;
                        break;

                    }
                    
                    
                    
                }
            }

            
        }
        public List<Point> GetPointTank(Oponent oponent)
        {
            List<Point> some_points = new List<Point>();
            Rect cellRect = new Rect(0, 0, oponent.Tank_grid.Width, oponent.Tank_grid.Height);
            //Point topleftposition = oponent.Tank_grid.TransformToAncestor(canvas).Transform(new Point(0, 0));
            for (double x = cellRect.Left; x <= cellRect.Right; x++)
            {
                for (double y = cellRect.Top; y <= cellRect.Bottom; y++)
                {
                    some_points.Add(new Point(oponent.PositionX + x, oponent.PositionY + y));
                }
            }
            return some_points;
        }
        public override UIElement GetUIElementAtPoint(Grid grid, Point point)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(grid, point);
            return hitTestResult?.VisualHit as UIElement;
        }
        public override bool DoSomeAction_with_bonus(UIElement element)
        {
            bool test_to_bonus = false;
            if (element is Image)
            {
                Image tempImage = (Image)element;
                ImageSource source = tempImage.Source;
                if (source != null && source.ToString() == "file:///D:/My Homework/Cursova/Texture_image/speed_bonus.png")
                {
                    test_to_bonus = true;
                    if (timer.IsEnabled != true)
                    {
                        speed_of_tank = speed_of_tank * 2;
                        timer.Start();
                    }
                    else if(timer.IsEnabled == true && speed_of_tank <= 3)
                    {
                        speed_of_tank = speed_of_tank * 2;
                        
                    }
                    
                }
                else if (source != null && source.ToString() == "file:///D:/My Homework/Cursova/Texture_image/slow_bonus.png")
                {
                    test_to_bonus = true;
                    if (timer.IsEnabled != true)
                    {  
                        speed_of_tank = speed_of_tank / 3;
                        timer.Start();
                    }
                    else if(timer.IsEnabled == true && speed_of_tank >= 3)
                    {
                        speed_of_tank = speed_of_tank / 3;
                        
                    }
                }
                else if (source != null && source.ToString() == "file:///D:/My Homework/Cursova/Texture_image/buff_muzzle.png")
                {
                    test_to_bonus = true;
                    if (timer.IsEnabled != true)
                    {
                        damage_of_oponent += 25;
                        timer.Start();
                    }
                    else if (timer.IsEnabled = true && damage_of_oponent > 25 && damage_of_oponent < 100)
                    {
                        damage_of_oponent += 25;
                    }
                }
                return test_to_bonus;
            }
            else
            {
                return test_to_bonus;
            }
        }
        public override void TimerAction()
        {
            speed_of_tank = 3;
            damage_of_oponent = 25;
            timer.Stop();
        }
    }
}
