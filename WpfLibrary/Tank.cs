using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfLibrary
{
    public class Tank
    {
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public Shape Main_tank { get; private set; }
        public Shape Under_Muzzle_tank { get; private set; }
        public Shape UP_Muzzle_tank { get; private set; }
        public Shape Chassis_right { get; private set; }
        public Shape Chassis_left { get; private set; }

        public Canvas canvas { get; set; }
        public Grid Tank_grid { get; set; }
        public Border indeficator { get; private set; }
        public Point CenterUnderMuzzle { get; set; }
        public Point CenterUpMuzzle { get; set; }
        public List<Point> points_of_tanks { get; set; }
        public List<Oponent> list_of_oponents { get; set; }
        public List<Grid> Killed_Oponents { get; set; }
        public int Health = 100;
        public Tank()
        {
            Killed_Oponents = new List<Grid>();
            points_of_tanks = new List<Point>();
            Tank_grid = new Grid();   
            Tank_grid.Height = 30;
            Tank_grid.Width = 30;
            Width = Tank_grid.Width;
            Height = Tank_grid.Height;
            for (int i = 0; i < 11; i++)
            {
                Tank_grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < 11; i++)
            {
                Tank_grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            double width_one_row = Tank_grid.Width / Tank_grid.ColumnDefinitions.Count;
            double height_one_column = Tank_grid.Height / Tank_grid.RowDefinitions.Count;
            double difference = height_one_column - width_one_row;
            Path path = new Path();
            // MAIN_TANK;
            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(width_one_row*3, 0);
            figure.Segments.Add(new LineSegment(new Point(width_one_row*6,0), true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row*9, height_one_column*2), new Size(width_one_row*3, width_one_row * 3), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new LineSegment(new Point(width_one_row*9, height_one_column*5), true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row*6,height_one_column*7), new Size(width_one_row * 3, width_one_row * 3), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new LineSegment(new Point(width_one_row * 3, height_one_column*7), true));
            figure.Segments.Add(new ArcSegment(new Point(0,height_one_column*5), new Size(width_one_row * 3, width_one_row * 3), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new LineSegment(new Point(0,height_one_column*2), true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row*3,0), new Size(width_one_row * 3, width_one_row * 3), 0, false, SweepDirection.Clockwise, true));
            geometry.Figures.Add(figure);
            path.Data = geometry;
            Main_tank = path;
            Main_tank.Fill = Brushes.DarkGreen;
            Main_tank.Stroke = Brushes.Orange;
            Main_tank.StrokeThickness = 1;
            Grid.SetRow(Main_tank, 2);
            Grid.SetColumn(Main_tank, 1);
            Grid.SetRowSpan(Main_tank, 7);
            Grid.SetColumnSpan(Main_tank, 9);
            // Under_MUZZLE_TANK;

            path = new Path();
            geometry = new PathGeometry();
            figure = new PathFigure();
            figure.StartPoint = new Point((width_one_row * 2.5), difference*2.5);
            Point[] poly_segment_points1 = new Point[] { new Point(0, height_one_column * 2.5), new Point(width_one_row * 2.5, (height_one_column * 5) - (difference * 2.5)), new Point(width_one_row * 5, height_one_column * 2.5), new Point(width_one_row * 2.5, difference * 2.5) };
            PolyLineSegment poly_segment = new PolyLineSegment();
            poly_segment.Points = new PointCollection(poly_segment_points1);
            figure.Segments.Add(poly_segment);
            geometry.Figures.Add(figure);
            path.Data = geometry;
            Under_Muzzle_tank = path;
            Under_Muzzle_tank.Fill = Brushes.Black;
            Under_Muzzle_tank.Stroke = Brushes.Black;
            Under_Muzzle_tank.StrokeThickness = 2;
            Grid.SetRow(Under_Muzzle_tank, 3);
            Grid.SetColumn(Under_Muzzle_tank, 3);
            Grid.SetRowSpan(Under_Muzzle_tank, 5);
            Grid.SetColumnSpan(Under_Muzzle_tank, 5);
            //Under_MUZZLE_TANK_FINISH;

            //UP_MUZZLE_TANK
            double s;
            double r;
            s = 2.5 * width_one_row * Math.Sqrt(2);
            r = s / 6;
            path = new Path();
            geometry = new PathGeometry();
            figure = new PathFigure();
            figure.StartPoint = new Point((width_one_row*1.5) - r, (height_one_column * 5.5));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row*1.5,(height_one_column * 5.5) + r), new Size(r, r), 0, false, SweepDirection.Counterclockwise, true));
            figure.Segments.Add(new ArcSegment(new Point((width_one_row * 1.5) + r, height_one_column * 5.5), new Size(r, r), 0, false, SweepDirection.Counterclockwise, true));
            
            figure.Segments.Add(new LineSegment(new Point((width_one_row * 1.5) + r, 0), true));
            figure.Segments.Add(new LineSegment(new Point((width_one_row * 1.5) - r, 0), true));
            figure.Segments.Add(new LineSegment(new Point((width_one_row * 1.5) - r, height_one_column * 5.5), true));
            geometry.Figures.Add(figure);
            path.Data = geometry;
            UP_Muzzle_tank = path;
            UP_Muzzle_tank.Fill = Brushes.Black;
            UP_Muzzle_tank.Stroke = Brushes.Green;
            UP_Muzzle_tank.StrokeThickness = 1;
            Grid.SetRow(UP_Muzzle_tank, 0);
            Grid.SetRowSpan(UP_Muzzle_tank, 6); 
            Grid.SetColumn(UP_Muzzle_tank, 4);
            Grid.SetColumnSpan(UP_Muzzle_tank, 3);

            //UP_MUZZLE_TANK_FINISH


            //Tank chassis right

            path = new Path();
            geometry = new PathGeometry();
            figure = new PathFigure();
            figure.StartPoint = new Point(0, height_one_column*2);
            figure.Segments.Add(new ArcSegment(new Point(width_one_row , 0), new Size(width_one_row, height_one_column * 2), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row*2, height_one_column * 2), new Size(width_one_row, height_one_column * 2), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new LineSegment(new Point(width_one_row*2, height_one_column * 9), true));
            figure.Segments.Add(new ArcSegment(new Point(width_one_row, height_one_column * 11), new Size(width_one_row, height_one_column * 2), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new ArcSegment(new Point(0, height_one_column * 9), new Size(width_one_row, height_one_column * 2), 0, false, SweepDirection.Clockwise, true));
            figure.Segments.Add(new LineSegment(new Point(0, Height * 2), true));
            geometry.Figures.Add(figure);
            figure = new PathFigure();
            figure.StartPoint = new Point(2, height_one_column * 2);
            Point[] points_chassis = new Point[] {
                new Point(width_one_row*2-2,height_one_column*2),
                new Point(width_one_row*2-2,height_one_column*3),
                new Point(0+2,height_one_column*3),
                new Point(0+2,height_one_column*2),
                new Point(0+2,height_one_column*4),
                new Point(width_one_row * 2-2,height_one_column*4),
                new Point(width_one_row * 2-2,height_one_column*5),
                new Point(0+2,height_one_column*5),
                new Point(0+2,height_one_column*4),

                new Point(0+2,height_one_column*6),
                new Point(width_one_row * 2-2,height_one_column*6),
                new Point(width_one_row * 2-2,height_one_column*7),
                new Point(0+2,height_one_column*7),
                new Point(0+2,height_one_column*6),

                new Point(0+2,height_one_column*8),
                new Point(width_one_row * 2-2,height_one_column*8),
                new Point(width_one_row * 2-2, height_one_column*9),
                new Point(0+2,height_one_column*9),
                };
            PolyLineSegment polyLineSegment = new PolyLineSegment();
            polyLineSegment.Points = new PointCollection(points_chassis);
            figure.Segments.Add(polyLineSegment);
            geometry.Figures.Add(figure);
            path.Data = geometry;
            Chassis_right = path;
            Chassis_right.Fill = Brushes.Black;
            
            Chassis_right.StrokeThickness = 1;
            Grid.SetRow(Chassis_right,0);
            Grid.SetColumn(Chassis_right, 9);
            Grid.SetColumnSpan(Chassis_right, 2);
            Grid.SetRowSpan(Chassis_right, 11);

            //Tank chassis right
            //Tank chassis left
            path = new Path();
            path.Data = geometry;

            Chassis_left = path;
            Chassis_left.Fill = Brushes.Black;
            
            Chassis_left.StrokeThickness = 1;
            Grid.SetRow(Chassis_left, 0);
            Grid.SetColumn(Chassis_left, 0);
            Grid.SetColumnSpan(Chassis_left, 2);
            Grid.SetRowSpan(Chassis_left, 11);

            //Tank chassis left

            Tank_grid.Children.Add(Chassis_left);
            Tank_grid.Children.Add(Chassis_right);
            Tank_grid.Children.Add(UP_Muzzle_tank);
            Tank_grid.Children.Add(Main_tank);
            Tank_grid.Children.Add(Under_Muzzle_tank);
            Border border1 = new Border();
            indeficator = new Border();
            Grid.SetRow(indeficator, 8);
            Grid.SetColumn(indeficator, 5);
            Grid.SetRow(border1, 8);
            Grid.SetColumn(border1, 6);
            border1.Background = Brushes.Blue;
            indeficator.Background = Brushes.Red;
            Tank_grid.Children.Add(indeficator);
            Tank_grid.Children.Add(border1);
            Panel.SetZIndex(Main_tank, 2);
            Panel.SetZIndex(Under_Muzzle_tank, 3);
            Panel.SetZIndex(UP_Muzzle_tank, 4);
            Panel.SetZIndex(Chassis_left, 1);
            Panel.SetZIndex(Chassis_right, 1);
            Panel.SetZIndex(indeficator, 3);
            Panel.SetZIndex(border1, 3);
            

        }
        public void Initial_grid_tank()
        {
            
            double play_zone_width = canvas.ActualWidth;
            double play_zone_height = canvas.ActualHeight;
            PositionX = play_zone_width / 2;
            PositionY = 700;
            Canvas.SetLeft(Tank_grid, PositionX);
            Canvas.SetTop(Tank_grid, PositionY);
            canvas.Children.Add(Tank_grid);
            canvas.UpdateLayout();
            Find_points_tank();
        }
        public void Find_points_tank()
        {
            // Points of tank
            Rect cellRect = new Rect(0, 0, Tank_grid.Width, Tank_grid.Height);
            Point cellLeftTop = Tank_grid.TransformToAncestor(canvas).Transform(new Point(0, 0));
            for (double x = cellRect.Left; x <= cellRect.Right; x++)
            {
                for (double y = cellRect.Top; y <= cellRect.Bottom; y++)
                {
                    points_of_tanks.Add(new Point(cellLeftTop.X + x, cellLeftTop.Y + y));
                }
            }
           
            // Point of tank
            }


    }
}
