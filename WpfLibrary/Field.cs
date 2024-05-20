using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace WpfLibrary
{
    public class Field
    {
        public Grid Grid_field { get; private set; }
        public Canvas canvas { get; set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public List<Image> Texture { get; private set; }

        public List<Point> points_of_texture { get; private set; }
        public List<Oponent> oponents { get; set; }
        public Tank My_Tank { get; set; }
        public List<Point> points_for_my_tank { get; set; }
        public List<Point> points_for_oponent { get; set; }
        public string side_move { get; set; }

        public List<Point> list_of_bonus_grid_field { get; private set; }
        public List<int> FillRow { get; private set; }
        public List<int> FillColumn { get; private set; }
        public List<int> EmptyRow { get; set; }
        public List<int> EmptyColumn { get; set; }

        public Bonus bonus { get; set; }
        public Field()
        {

        }
        public Field(Canvas canvas)
        {
            list_of_bonus_grid_field = new List<Point>();
            EmptyColumn = new List<int>();
            EmptyRow = new List<int>();
            FillRow = new List<int>();
            FillColumn = new List<int>();
            this.canvas = canvas;
            Grid_field = new Grid();
            Texture = new List<Image>();
            BitmapImage bitmap_image_1 = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\Wall_stone3.jpg", UriKind.RelativeOrAbsolute));
            BitmapImage bitmap_image_2 = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\Wall_stone3.jpg", UriKind.RelativeOrAbsolute));
            BitmapImage bitmap_image_3 = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\Wall_3.jpg", UriKind.RelativeOrAbsolute));
            BitmapImage bitmap_image_brick = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\Wall_brick.png", UriKind.RelativeOrAbsolute));
            BitmapImage bitmap_image_tree = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\Wall_tree.jpg", UriKind.RelativeOrAbsolute));
            BitmapImage bitmap_image_wood = new BitmapImage(new Uri("D:\\My Homework\\Cursova\\Texture_image\\Wall_wood.png", UriKind.RelativeOrAbsolute));

            Grid_field.Width = canvas.ActualWidth;
            Grid_field.Height = canvas.ActualHeight;
            Width = Grid_field.Width;
            Height = Grid_field.Height;

            for (int i = 0; i < 20; i++)
            {
                Grid_field.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                Grid_field.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            double width_one_row = Width / Grid_field.ColumnDefinitions.Count;
            double height_one_column = Height / Grid_field.RowDefinitions.Count;
            for (int i = 0; i < Grid_field.ColumnDefinitions.Count; i++)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                Grid.SetRow(border, 0);
                Grid.SetColumn(border, i);
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                Image img = new Image();
                img.Source = bitmap_image_1;
                img.Stretch = Stretch.Fill;
                img.HorizontalAlignment = HorizontalAlignment.Center;
                img.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(img, 0);
                Grid.SetColumn(img, i);
                Texture.Add(img);
                Grid_field.Children.Add(img);
                Grid_field.Children.Add(border);
            }
            for (int i = 1; i < Grid_field.RowDefinitions.Count; i++)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                Grid.SetRow(border, i);
                Grid.SetColumn(border, 0);
                Image img = new Image();
                img.Source = bitmap_image_2;
                img.Stretch = Stretch.Fill;
                img.HorizontalAlignment = HorizontalAlignment.Stretch;
                img.VerticalAlignment = VerticalAlignment.Stretch;
                Grid.SetRow(img, i);
                Grid.SetColumn(img, 0);
                Texture.Add(img);
                Grid_field.Children.Add(img);
                Grid_field.Children.Add(border);
            }
            for (int i = 1; i < Grid_field.RowDefinitions.Count; i++)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                Grid.SetRow(border, i);
                Grid.SetColumn(border, Grid_field.ColumnDefinitions.Count - 1);
                Image img = new Image();
                img.Source = bitmap_image_2;
                img.Stretch = Stretch.Fill;
                img.HorizontalAlignment = HorizontalAlignment.Stretch;
                img.VerticalAlignment = VerticalAlignment.Stretch;
                Grid.SetRow(img, i);
                Grid.SetColumn(img, Grid_field.ColumnDefinitions.Count - 1);
                Texture.Add(img);
                Grid_field.Children.Add(img);
                Grid_field.Children.Add(border);
            }
            for (int i = 1; i < Grid_field.ColumnDefinitions.Count - 1; i++)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                Grid.SetRow(border, Grid_field.RowDefinitions.Count - 1);
                Grid.SetColumn(border, i);
                Image img = new Image();
                img.Source = bitmap_image_2;
                img.Stretch = Stretch.Fill;
                img.HorizontalAlignment = HorizontalAlignment.Stretch;
                img.VerticalAlignment = VerticalAlignment.Stretch;
                Grid.SetRow(img, Grid_field.RowDefinitions.Count - 1);
                Grid.SetColumn(img, i);
                Texture.Add(img);
                Grid_field.Children.Add(img);
                Grid_field.Children.Add(border);
            }
            for (int i = 1; i < Grid_field.RowDefinitions.Count - 1; i++)
            {
                for (int j = 1; j < Grid_field.ColumnDefinitions.Count - 1; j++)
                {
                    if (i == 1 && j == 1)
                    {
                        Create_Texture_brick(bitmap_image_brick, i, j);
                    }
                    if (i == 1 && j == Grid_field.ColumnDefinitions.Count - 2)
                    {
                        Create_Texture_brick(bitmap_image_brick, i, j);
                    }
                    if (i == Grid_field.RowDefinitions.Count - 2 && j == 1)
                    {
                        Create_Texture_brick(bitmap_image_brick, i, j);
                    }
                    if (i == Grid_field.RowDefinitions.Count - 2 && j == Grid_field.ColumnDefinitions.Count - 2)
                    {
                        Create_Texture_brick(bitmap_image_brick, i, j);
                    }
                    if (i == 5)
                    {
                        if (j == 3 || j == 5 || j == 6 || j == 7 || j == 9 || j == 10 || j == 11 || j == 13 || j == 14 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                    }
                    if (i == 6)
                    {
                        if (j == 3 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                    }
                    if (i == 7)
                    {
                        if (j == 3 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                        if (j == 5 || j == 6 || j == 7 || j == 12 || j == 13 || j == 14)
                        {
                            Create_Texture_grass(bitmap_image_tree, i, j);
                        }
                    }
                    if (i == 8)
                    {
                        if (j == 5 || j == 7 || j == 12 || j == 14)
                        {
                            Create_Texture_grass(bitmap_image_tree, i, j);
                        }
                        if (j == 6 || j == 13)
                        {
                            Create_Texture_wood(bitmap_image_wood, i, j);
                        }
                        if (j == 9 || j == 10)
                        {
                            Create_Texture_stone(bitmap_image_3, i, j);
                        }
                    }
                    if (i == 9)
                    {
                        if (j == 3 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                        if (j == 5 || j == 6 || j == 7 || j == 12 || j == 13 || j == 14)
                        {
                            Create_Texture_grass(bitmap_image_tree, i, j);
                        }
                    }
                    if (i == 10)
                    {
                        if (j == 9 || j == 10)
                        {
                            Create_Texture_stone(bitmap_image_3, i, j);
                        }
                    }
                    if (i == 11)
                    {
                        if (j == 3 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                        if (j == 9 || j == 10)
                        {
                            Create_Texture_stone(bitmap_image_3, i, j);
                        }
                    }
                    if (i == 12)
                    {
                        if (j == 5 || j == 6 || j == 7 || j == 12 || j == 13 || j == 14)
                        {
                            Create_Texture_grass(bitmap_image_tree, i, j);
                        }
                    }
                    if (i == 13)
                    {
                        if (j == 5 || j == 7 || j == 12 || j == 14)
                        {
                            Create_Texture_grass(bitmap_image_tree, i, j);
                        }
                        if (j == 6 || j == 13)
                        {
                            Create_Texture_wood(bitmap_image_wood, i, j);
                        }
                        if (j == 9 || j == 10)
                        {
                            Create_Texture_stone(bitmap_image_3, i, j);
                        }
                    }
                    if (i == 14)
                    {
                        if (j == 3 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                        if (j == 5 || j == 6 || j == 7 || j == 12 || j == 13 || j == 14)
                        {
                            Create_Texture_grass(bitmap_image_tree, i, j);
                        }
                    }
                    if (i == 15)
                    {
                        if (j == 3 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                    }
                    if (i == 16)
                    {
                        if (j == 3 || j == 5 || j == 6 || j == 7 || j == 9 || j == 10 || j == 11 || j == 13 || j == 14 || j == 16)
                        {
                            Create_Texture_brick(bitmap_image_brick, i, j);
                        }
                    }

                }
            }
            
        }
        public void Find_Empty_Field()
        {
            bool test_to_empty_grid = true;
            for (int i = 4; i < Grid_field.RowDefinitions.Count - 3; i++)
            {
                
                for (int j = 1; j < Grid_field.ColumnDefinitions.Count - 1; j++)
                {
                    test_to_empty_grid = true;
                    for (int k = 0; k < FillRow.Count; k++)
                    {
                        if (i == FillRow[k] && j == FillColumn[k])
                        {
                            test_to_empty_grid = false;
                        }
                    }
                    if (test_to_empty_grid == true)
                    {
                        EmptyRow.Add(i);
                        EmptyColumn.Add(j);
                    }
                }
                

            }
        }
        public void Create_Texture_stone(BitmapImage bitmap_image_3, int i, int j)
        {
            Image img = new Image();
            img.Source = bitmap_image_3;
            img.Stretch = Stretch.Fill;
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetRow(img, i);
            Grid.SetColumn(img, j);
            Texture.Add(img);
            Grid_field.Children.Add(img);
            FillRow.Add(i);
            FillColumn.Add(j);
        }
        public void Create_Texture_wood(BitmapImage bitmap_image_wood,int i,int j)
        {
            Image img = new Image();
            img.Source = bitmap_image_wood;
            img.Stretch = Stretch.Fill;
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetRow(img, i);
            Grid.SetColumn(img, j);
            Texture.Add(img);
            Grid_field.Children.Add(img);
            FillRow.Add(i);
            FillColumn.Add(j);
        }
        public void Create_Texture_grass(BitmapImage bitmap_image_5, int i, int j)
        {
            Image img = new Image();
            img.Source = bitmap_image_5;
            img.Stretch = Stretch.Fill;
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetRow(img, i);
            Grid.SetColumn(img, j);
            Texture.Add(img);
            Grid_field.Children.Add(img);
            FillRow.Add(i);
            FillColumn.Add(j);
        }
        public void Create_Texture_brick(BitmapImage bitmap_image_4,int i,int j)
        {
            Image img = new Image();
            img.Source = bitmap_image_4;
            img.Stretch = Stretch.Fill;
            img.HorizontalAlignment = HorizontalAlignment.Stretch;
            img.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetRow(img, i);
            Grid.SetColumn(img, j);
            Texture.Add(img);
            Grid_field.Children.Add(img);
            FillRow.Add(i);
            FillColumn.Add(j);
        }
        
        public void Initial_field()
        {
            Canvas.SetLeft(Grid_field, 0);
            Canvas.SetTop(Grid_field, 0);
            canvas.Children.Add(Grid_field);
            Find_Empty_Field();
            int number_of_bonus = 10;
            bonus = new Bonus(list_of_bonus_grid_field, canvas, number_of_bonus,EmptyRow,EmptyColumn,Grid_field,this);
        }
        public List<Point> GetAllPoints()
        {

            points_of_texture = new List<Point>();
            for (int i = 0; i < Texture.Count; i++)
            {
                Grid_field.UpdateLayout();
                List<Point> temp_point = Find_All_Points_of_texture(Texture[i]);
                points_of_texture.AddRange(temp_point);
            }
            return points_of_texture;
        }
        
        public List<Point> GetPointForMytank()
        {
            points_for_my_tank = new List<Point>();
            for (int i = 0; i < oponents.Count; i++)
            {
                points_for_my_tank.AddRange(Find_All_Points_to_tank(oponents[i]));
                
            }
            return points_for_my_tank;
        }
        
        public List<Point> GetPointForOponent()
        {
            points_for_oponent = new List<Point>();
            for (int i = 1; i < oponents.Count; i++)
            {
                points_for_oponent.AddRange(Find_All_Points_to_tank(oponents[i]));
            }
            
            points_for_oponent.AddRange(Find_All_Points_to_tank(My_Tank));
            return points_for_oponent;
        }
        public List<Point> Find_All_Points_to_tank(Tank oponents)
        {

            List<Point> temp_point = new List<Point>();
            int pad = 5;
            Rect cellRectHorizontal = new Rect(0, 0, oponents.Width, pad);
            Rect cellRectVertical = new Rect(0, pad, pad, oponents.Height - pad * 2);
            Rect cellRectRightTopHorizontal = new Rect(oponents.Width, 0, oponents.Width, pad);
            Rect cellRectRightTopVertical = new Rect(oponents.Width - pad, -oponents.Height, pad, oponents.Height);
            Rect cellRectLeftBottomHorizontal = new Rect(-oponents.Width, oponents.Height - pad, oponents.Width, pad);
            Rect cellRectLeftBottomVertical = new Rect(0, oponents.Height, pad, oponents.Height);

            Point topleftpoint = (new Point(oponents.PositionX, oponents.PositionY));
            if (side_move == "LEFT")
            {
                for (double j = cellRectRightTopVertical.Left; j <= cellRectRightTopVertical.Right; j++)
                {
                    for (double k = cellRectRightTopVertical.Top; k <= cellRectRightTopVertical.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            else if (side_move == "RIGHT")
            {
                for (double j = cellRectLeftBottomVertical.Left; j <= cellRectLeftBottomVertical.Right; j++)
                {
                    for (double k = cellRectLeftBottomVertical.Top; k <= cellRectLeftBottomVertical.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            else if (side_move == "UP")
            {
                for (double j = cellRectLeftBottomHorizontal.Left; j <= cellRectLeftBottomHorizontal.Right; j++)
                {
                    for (double k = cellRectLeftBottomHorizontal.Top; k <= cellRectLeftBottomHorizontal.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            else if (side_move == "DOWN")
            {
                for (double j = cellRectRightTopHorizontal.Left; j <= cellRectRightTopHorizontal.Right; j++)
                {
                    for (double k = cellRectRightTopHorizontal.Top; k <= cellRectRightTopHorizontal.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            for (double j = cellRectHorizontal.Left; j <= cellRectHorizontal.Right; j++)
            {
                for (double k = cellRectHorizontal.Top; k <= cellRectHorizontal.Bottom; k++)
                {
                    temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + oponents.Height - k));
                }
            }
            for (double j = cellRectVertical.Left; j <= cellRectVertical.Right; j++)
            {
                for (double k = cellRectVertical.Top; k <= cellRectVertical.Bottom; k++)
                {
                    temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    temp_point.Add(new Point(topleftpoint.X + oponents.Width - j, topleftpoint.Y + k));
                }
            }
            return temp_point;


        }
        public List<Point> Find_All_Points_of_texture(Image texture)
        {

            List<Point> temp_point = new List<Point>();
            int pad = 5;

            Rect cellRectHorizontal = new Rect();
            Application.Current.Dispatcher.Invoke(() =>
            {
                cellRectHorizontal = new Rect(0, 0, texture.ActualWidth, pad);
            });
            
            Rect cellRectVertical = new Rect();
            Application.Current.Dispatcher.Invoke(() =>
            {
                cellRectVertical = new Rect(0, pad, pad, texture.ActualHeight - pad * 2);
            });
            Rect cellRectRightTopHorizontal = new Rect();
            Application.Current.Dispatcher.Invoke(() =>
            {
                cellRectRightTopHorizontal = new Rect(texture.ActualWidth, 0, My_Tank.Tank_grid.ActualWidth, pad);
            });
            Rect cellRectRightTopVertical = new Rect();
            Application.Current.Dispatcher.Invoke(() =>
            {
                cellRectRightTopVertical = new Rect(texture.ActualWidth - pad, -My_Tank.Tank_grid.ActualHeight, pad, My_Tank.Tank_grid.ActualHeight);
            });
            Rect cellRectLeftBottomHorizontal = new Rect();
            Application.Current.Dispatcher.Invoke(() =>
            {
                cellRectLeftBottomHorizontal = new Rect(-My_Tank.Tank_grid.ActualWidth, texture.ActualHeight - pad, My_Tank.Tank_grid.ActualWidth, pad);
            });
            Rect cellRectLeftBottomVertical = new Rect();
            Application.Current.Dispatcher.Invoke(() =>
            {
                cellRectLeftBottomVertical = new Rect(0, texture.ActualHeight, pad, My_Tank.Tank_grid.ActualHeight);
            });

            Point topleftpoint = texture.Dispatcher.Invoke(() => texture.TransformToAncestor(canvas).Transform(new Point(0, 0)));


            if (side_move == "LEFT")
            {
                for (double j = cellRectRightTopVertical.Left; j <= cellRectRightTopVertical.Right; j++)
                {
                    for (double k = cellRectRightTopVertical.Top; k <= cellRectRightTopVertical.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            else if (side_move == "RIGHT")
            {
                for (double j = cellRectLeftBottomVertical.Left; j <= cellRectLeftBottomVertical.Right; j++)
                {
                    for (double k = cellRectLeftBottomVertical.Top; k <= cellRectLeftBottomVertical.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            else if (side_move == "UP")
            {
                for (double j = cellRectLeftBottomHorizontal.Left; j <= cellRectLeftBottomHorizontal.Right; j++)
                {
                    for (double k = cellRectLeftBottomHorizontal.Top; k <= cellRectLeftBottomHorizontal.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            else if (side_move == "DOWN")
            {
                for (double j = cellRectRightTopHorizontal.Left; j <= cellRectRightTopHorizontal.Right; j++)
                {
                    for (double k = cellRectRightTopHorizontal.Top; k <= cellRectRightTopHorizontal.Bottom; k++)
                    {
                        temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    }
                }
            }
            for (double j = cellRectHorizontal.Left; j <= cellRectHorizontal.Right; j++)
            {
                for (double k = cellRectHorizontal.Top; k <= cellRectHorizontal.Bottom; k++)
                {
                    temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + texture.ActualHeight - k));
                }
            }
            for (double j = cellRectVertical.Left; j <= cellRectVertical.Right; j++)
            {
                for (double k = cellRectVertical.Top; k <= cellRectVertical.Bottom; k++)
                {
                    temp_point.Add(new Point(topleftpoint.X + j, topleftpoint.Y + k));
                    temp_point.Add(new Point(topleftpoint.X + texture.ActualWidth - j, topleftpoint.Y + k));
                }
            }
            return temp_point;


        }
    }
}
