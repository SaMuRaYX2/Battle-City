﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLibrary
{
    public class MovementTank
    {
        protected List<Image> my_texture;

        protected List<Point> points_denied;

        public Canvas canvas { get; set; }
        
        public Point point_for_bullet { get; set; }

        public List<Point> Points_denied
        {
            get { return points_denied; }
            set { points_denied = value; }
        }
        //public MovementTank(List<Point> points)
        //{
        //    points_denied = points;
        //}
        public Field my_field { get; set; }
        public string side_of_move { get; set; }
        public bool test_point { get; set; }

        public void Press_PageUp( Tank my_tank)
        {
            double speed = -5.0;
            
            string key = "UP";
            side_of_move = key;
            double value_width_grid = my_tank.Width;
            double value_height_grid = my_tank.Height;
            my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            int row_indeficator = Grid.GetRow(my_tank.indeficator);
            int column_indeficator = Grid.GetColumn(my_tank.indeficator);
            RotateTransform rotate = new RotateTransform(0);
            TRY_TO_ROTATE( my_tank, rotate);
            Move(speed, my_tank, key);
        }
        public void Press_PageDown( Tank my_tank)
        {
            string key = "DOWN";
            side_of_move = key;
            double speed = 5.0;
            double value_width_grid = my_tank.Width;
            double value_height_grid = my_tank.Height;
            my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            int row_indeficator = Grid.GetRow(my_tank.indeficator);
            int column_indeficator = Grid.GetColumn(my_tank.indeficator);
            RotateTransform rotate = new RotateTransform(180);
            TRY_TO_ROTATE( my_tank, rotate);
            Move(speed, my_tank, key);
        }
        public void Press_Home( Tank my_tank)
        {
            string key = "LEFT";
            side_of_move = key;
            double speed = -5.0;
            double value_width_grid = my_tank.Width;
            double value_height_grid = my_tank.Height;
            my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            int row_indeficator = Grid.GetRow(my_tank.indeficator);
            int column_indeficator = Grid.GetColumn(my_tank.indeficator);
            RotateTransform rotate = new RotateTransform(-90);
            TRY_TO_ROTATE( my_tank, rotate);
            Move(speed, my_tank, key);
        }
        public void Press_End( Tank my_tank)
        {
            string key = "RIGHT";
            side_of_move = key;
            double speed = 5.0;
            double value_width_grid = my_tank.Width;
            double value_height_grid = my_tank.Height;
            my_tank.Tank_grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            int row_indeficator = Grid.GetRow(my_tank.indeficator);
            int column_indeficator = Grid.GetColumn(my_tank.indeficator);
            RotateTransform rotate = new RotateTransform(90);
            TRY_TO_ROTATE( my_tank, rotate);
            Move(speed, my_tank, key);
        }
        public void TRY_TO_ROTATE(Tank my_tank,RotateTransform rotate)
        {
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotate);
            my_tank.Tank_grid.RenderTransform = transformGroup;                                                                                                                                                                         
        }
        public async void Move(double speed, Tank tank_move,string side)
        {
            Point point_center_of_tank_int_canvas = tank_move.Tank_grid.TransformToAncestor(canvas).Transform(new Point(tank_move.Width / 2, tank_move.Height / 2));
            point_for_bullet = point_center_of_tank_int_canvas;
            double default_positionX = tank_move.PositionX;
            double default_positionY = tank_move.PositionY;
            await Task.WhenAll(Test_to_Move(tank_move, side, speed));
            if (test_point == true)
            {
                Canvas.SetLeft(tank_move.Tank_grid, tank_move.PositionX);
                Canvas.SetTop(tank_move.Tank_grid, tank_move.PositionY);
                tank_move.points_of_tanks = GetPointTank(tank_move);
            }
            else
            {
                tank_move.PositionX = default_positionX;
                tank_move.PositionY = default_positionY;
                if(side == "UP")
                {
                    speed = -1;
                }
                else if(side == "DOWN")
                {
                    speed = 1;
                }
                else if(side == "LEFT")
                {
                    speed = -1;
                }
                else if(side == "RIGHT")
                {
                    speed = 1;
                }
                
                
                await Task.WhenAll(Test_to_Move(tank_move, side, speed));
                if(test_point == true)
                {
                    Canvas.SetLeft(tank_move.Tank_grid, tank_move.PositionX);
                    Canvas.SetTop(tank_move.Tank_grid, tank_move.PositionY);
                    tank_move.points_of_tanks = GetPointTank(tank_move);
                }
                else
                {
                    tank_move.PositionX = default_positionX;
                    tank_move.PositionY = default_positionY;
                }
            }
        }
        
        public async Task Test_to_Move( Tank tank_move, string side, double speed)
        {
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
            points_denied = my_field.GetAllPoints();
            points_denied.AddRange(my_field.GetPointForMytank());
            foreach (var point in points_denied)
            {
                if (tank_position == point)
                {
                    test_point = false;
                }
            }


        }
        public List<Point> GetPointTank(Tank tank)
        {
            List<Point> some_points = new List<Point>();
            Rect cellRect = new Rect(0, 0, tank.Tank_grid.Width, tank.Tank_grid.Height);
            //Point topleftposition = oponent.Tank_grid.TransformToAncestor(canvas).Transform(new Point(0, 0));
            for (double x = cellRect.Left; x <= cellRect.Right; x++)
            {
                for (double y = cellRect.Top; y <= cellRect.Bottom; y++)
                {
                    some_points.Add(new Point(tank.PositionX + x, tank.PositionY + y));
                }
            }
            return some_points;
        }
    }
}
