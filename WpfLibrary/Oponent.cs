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
    public class Oponent : Tank
    {
        public Oponent() : base()
        {
            list_of_oponents = new List<Oponent>();
            Main_tank.Fill = Brushes.Thistle;
            Main_tank.Stroke = Brushes.Black;

        }

        public void Initial_grid_oponent()
        {
            double play_zone_width = canvas.ActualWidth;
            double play_zone_height = canvas.ActualHeight;
            PositionY = 100;
            int index = 0;
            bool test_to_find_point = true;
            List<Point> point_to_test = new List<Point>();
            for (int i = 150; i < play_zone_width - 100 && index < list_of_oponents.Count; i += 150, index++)
            {
                test_to_find_point = true;
                PositionX = i;
                point_to_test = new List<Point>();
                Rect cellRect = new Rect(0, 0, Tank_grid.Width, Tank_grid.Height);
                for (double x = cellRect.Left; x <= cellRect.Right; x++)
                {
                    for (double y = cellRect.Top; y <= cellRect.Bottom; y++)
                    {
                        point_to_test.Add(new Point(i + x, PositionY + y));
                    }
                }
                List<Point> points_of_tank_j;
                points_of_tank_j = new List<Point>();
                if (list_of_oponents[index].points_of_tanks != null)
                {
                    points_of_tank_j = list_of_oponents[index].points_of_tanks;
                    while (test_to_find_point == true)
                    {
                        for (int k = 0; k < point_to_test.Count; k++)
                        {
                            for (int l = 0; l < points_of_tank_j.Count; l++)
                            {
                                if (point_to_test[k] == points_of_tank_j[l])
                                {
                                    test_to_find_point = false;
                                    break;
                                }
                            }
                            if (test_to_find_point == false)
                            {
                                break;
                            }
                        }
                        if (test_to_find_point == true)
                        {
                            break;
                        }
                    }
                    
                }
                
                if (test_to_find_point == true)
                {
                    break;
                }
            }
            if (test_to_find_point == true)
            {
                TransformGroup group = new TransformGroup();
                RotateTransform rotate = new RotateTransform(180);
                group.Children.Add(rotate);
                Tank_grid.RenderTransformOrigin = new Point(0.5, 0.5);
                Tank_grid.RenderTransform = group;
                Canvas.SetLeft(Tank_grid, PositionX);
                Canvas.SetTop(Tank_grid, PositionY);
                points_of_tanks = point_to_test;
            }
        }
    }
}
