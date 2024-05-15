﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLibrary
{
    public class Creating_oponent
    {
        public bool MyAsyncIteration(List<Oponent> list_oponents,List<Point> points_of_tanks, List<Point> points_of_texture, ref Canvas canvas)
        {
            bool test_to_oponent = true;
            if (list_oponents.Count == 0)
            {
                Oponent oponent = new Oponent();
                oponent.canvas = canvas;
                //Для гри на двох;
                oponent.PositionX = 150;
                oponent.PositionY = 100;
                //Для гри з ботами;
                //oponent.PositionX = 100;
                //oponent.PositionY = 100;
                RotateTransform rotate = new RotateTransform(180);
                oponent.Tank_grid.RenderTransform = rotate;
                Canvas.SetLeft(oponent.Tank_grid, oponent.PositionX);
                Canvas.SetTop(oponent.Tank_grid, oponent.PositionY);
                list_oponents.Add(oponent);
                oponent.list_of_oponents = list_oponents;
                canvas.Children.Add(oponent.Tank_grid);
                canvas.UpdateLayout();
                oponent.Find_points_tank();
                oponent.Tank_grid.RenderTransformOrigin = new Point(0.5, 0.5);
                return test_to_oponent = true;
            }
            else if (list_oponents.Count < 4)
            {
                Oponent oponent = new Oponent(); 
                oponent.canvas = canvas;
                list_oponents.Add(oponent);
                oponent.list_of_oponents = list_oponents;
                oponent.Initial_grid_oponent();
                canvas.Children.Add(oponent.Tank_grid);
                oponent.Find_points_tank();
                canvas.UpdateLayout();
                return test_to_oponent = true;
            }
            else
            {
                //MessageBox.Show("Сталася якась помилка в класі Creating_opomnent", "Перевір!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return test_to_oponent = false;
            }
        }
    }
}