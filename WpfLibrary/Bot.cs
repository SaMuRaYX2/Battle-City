using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WpfLibrary
{
    
    public class Bot
    {
        protected Point Center_of_bot { get; set; }
        public Point Center_of_my_tank { get; set; }
        protected List<Point> Center_of_cell_field {  get; set; }
        public List<int> EmptyRow {private get; set; }
        public List<int> EmptyColumn { private get; set; }
        public Field field_of_playground {private get; set; }
        public Grid grid_field { get; set; }
        public List<Oponent> oponents { private get; set; }
        public Timer movement_timer;
        public Timer rotate_muzzle_timer;
        public Timer refresh_way_timer;
        private bool shouldContinue_rotate = true;
        private bool allow_to_refresh = true;
        private bool allow_move = true;
        public Tank my_tank { get; set; }
        private MovementOponent move_bot;
        public List<Point> Texture_point { get; set; }
        public Canvas canvas { get; set; }
        public string side_of_move { get; set; }
        public Battle battle;
        public int damage { get; set; }

        public bool test_to_finish_game = false;

        private bool test_to_visible_my_tank = false;
        public List<List<Point>> all_ways_to_my_tank_by_center_point;
        public List<Point> temp_list_to_all_list;
        public List<int> mass_of_move;
        public int iteration_of_move;

        public Bot(Field field,List<Oponent> list_oponents,Tank my_tank,MovementOponent move_oponent,List<Point> Points,Canvas play_zone,int damage_of_oponents)
        {
            this.damage = damage_of_oponents;
            battle = new Battle();
            Texture_point = Points;
            canvas = play_zone;
            Center_of_cell_field = new List<Point>();
            this.move_bot = move_oponent;
            this.my_tank = my_tank;
            this.field_of_playground = field;
            EmptyRow = field_of_playground.EmptyRow;
            EmptyColumn = field_of_playground.EmptyColumn;
            grid_field = field.Grid_field;
            Find_Center_of_cell_field();
            oponents = new List<Oponent>();
            this.oponents = list_oponents;
            TimerCallback timerCallback_move = new TimerCallback(Movement_timer);
            movement_timer = new Timer(timerCallback_move, null, 2000, 20);
            TimerCallback timerCallBack_muzzle = new TimerCallback(Rotate_Muzzle_timer);
            rotate_muzzle_timer = new Timer(timerCallBack_muzzle, null, 1000, 2000);
            TimerCallback timerRefresh_way = new TimerCallback(Update_way_timer);
            refresh_way_timer = new Timer(timerRefresh_way, null, 1000, 30000);
        }
        public void Update_way_timer(object state)
        {
            if (allow_to_refresh == true)
            {
                Task.Run(() =>
                {
                    allow_move = false;
                    UpdateWayToMyTank();
                    allow_to_refresh = false;
                    allow_move = true;
                });
            }
        }
        public void Movement_timer(object state)
        {
            if (allow_move == true)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Move_bot(mass_of_move[iteration_of_move]);
                    iteration_of_move++;
                });
            }
        }
        public void Rotate_Muzzle_timer(object state)
        {
            if (shouldContinue_rotate)
            {
                Task.Delay(1000);
                Task.Run(() =>
                {
                    Test_to_Fire();
                });
            }
        }
        public void Refresh_information_to_bot()
        {
            double PositionX = oponents[0].PositionX;
            double PositionY = oponents[0].PositionY;
            double WidthOponent = oponents[0].Width;
            double HeightOponent = oponents[0].Height;
            Center_of_bot = new Point(PositionX + (WidthOponent / 2), PositionY + (HeightOponent / 2));
            double My_PositionX = my_tank.PositionX;
            double My_PositionY = my_tank.PositionY;
            double My_width = my_tank.Width;
            double My_height = my_tank.Height;
            Center_of_my_tank = new Point(My_PositionX + (My_width / 2), My_PositionY + (My_height / 2));
            if (battle.ActualWidth_Up_Muzzle == 0)
            {
                battle.ActualWidth_Up_Muzzle = oponents[0].UP_Muzzle_tank.ActualWidth;
            }
            if (battle.ActualHeight_Up_Muzzle == 0)
            {
                battle.ActualHeight_Up_Muzzle = oponents[0].UP_Muzzle_tank.ActualHeight;
            }
            if (oponents[0].CenterUnderMuzzle.X == 0 && oponents[0].CenterUnderMuzzle.Y == 0)
            {
                Point topLeftInGrid_Under_muzzle = oponents[0].Under_Muzzle_tank.TransformToAncestor(oponents[0].Tank_grid).Transform(new Point(0, 0));
                oponents[0].CenterUnderMuzzle = oponents[0].Under_Muzzle_tank.TransformToAncestor(canvas).Transform(topLeftInGrid_Under_muzzle);
            }
            if (oponents[0].CenterUpMuzzle.X == 0 && oponents[0].CenterUpMuzzle.Y == 0)
            {
                Point topLeftInGrid_Up_muzzle = oponents[0].UP_Muzzle_tank.TransformToAncestor(oponents[0].Tank_grid).Transform(new Point(0, 0));
                oponents[0].CenterUpMuzzle = oponents[0].UP_Muzzle_tank.TransformToAncestor(canvas).Transform(topLeftInGrid_Up_muzzle);
            }
        }
        
        public void Move_bot(int i)
        {
            allow_to_refresh = false;
            move_bot.Points_denied = Texture_point;
            move_bot.canvas = canvas;
            move_bot.my_field = field_of_playground;
            switch (i)
            {
                case 1:
                    {
                        move_bot.Press_W(oponents[0]);
                        side_of_move = "UP";
                        break;
                    }
                case 2:
                    {
                        move_bot.Press_S(oponents[0]);
                        side_of_move = "DOWN";
                        break;
                    }
                case 3:
                    {
                        move_bot.Press_D(oponents[0]);
                        side_of_move = "RIGHT";
                        break;
                    }
                case 4:
                    {
                        move_bot.Press_A(oponents[0]);
                        side_of_move = "LEFT";
                        break;
                    }
            }
            allow_to_refresh = true;

        }
        public void UpdateWayToMyTank()
        {
            iteration_of_move = 0;
            List<int> bot_position_on_grid = Find_Cell_from_Point_in_Grid(Center_of_bot, Center_of_my_tank);
            int bot_row = bot_position_on_grid[0];
            int bot_column = bot_position_on_grid[1];
            int my_row = bot_position_on_grid[2];
            int my_column = bot_position_on_grid[3];
            int number_of_way = 1;
            all_ways_to_my_tank_by_center_point = new List<List<Point>>();
            mass_of_move = new List<int>();
            if (Center_of_bot.Y < Center_of_my_tank.Y)
            {
                while (number_of_way != 0) {
                    number_of_way--;
                    int start_row = bot_row;
                    int start_column = bot_column;
                    temp_list_to_all_list = new List<Point>();
                    while (start_row != my_row && start_column != my_column) {
                        bool test_to_row = true;
                        bool test_to_column = true;
                        while (test_to_row == true && start_row < my_row)
                        {
                            test_to_row = Try_to_move_down(ref start_row, ref start_column);
                        }
                        test_to_row = true;
                        bool test_to_left = true;
                        bool test_to_right = true;
                        while (test_to_left == true || test_to_right == true)
                        {
                            int choose_some_way = 0;
                            if (test_to_right == true && test_to_left == true)
                            {
                                Random rnd = new Random();
                                choose_some_way = rnd.Next(101);
                            }
                            else if (test_to_right == false && test_to_left == true)
                            {
                                choose_some_way = 80;
                            }
                            else if (test_to_left == false && test_to_right == true)
                            {
                                choose_some_way = 10;
                            }
                            if (choose_some_way <= 50)
                            {
                                int current_row = start_row;
                                while (test_to_row == true || test_to_column == true)
                                {
                                    if (start_row < my_row)
                                    {
                                        test_to_row = true;
                                        test_to_column = true;
                                        test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                        test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                    }
                                }
                                if(current_row == start_row)
                                {
                                    test_to_right = false;
                                }
                            }
                            else
                            {
                                int current_row = start_row;
                                while (test_to_row == true || test_to_column == true)
                                {
                                    if (start_row != my_row)
                                    {
                                        test_to_row = true;
                                        test_to_column = true;
                                        test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                        test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                    }
                                }
                                if(current_row == start_row)
                                {
                                    test_to_left = false;
                                }

                            }
                        }
                    }
                    all_ways_to_my_tank_by_center_point.Add(temp_list_to_all_list);
                }
            }
            //else if(Center_of_bot.Y > Center_of_my_tank.Y)
            //{
            //    if (Center_of_bot.X < Center_of_my_tank.X)
            //    {

            //    }
            //    else if (Center_of_bot.X > Center_of_my_tank.X)
            //    {

            //    }
            //}


            //IF MOVE UP == 1;
            //IF MOVE DOWN == 2;
            //IF MOVE RIGHT == 3;
            //IF MOVE LEFT == 4;
            double Center_X = Center_of_bot.X;
            double Center_Y = Center_of_bot.Y;
            for(int i = 0; i < all_ways_to_my_tank_by_center_point[0].Count; i++)
            {
                bool test_to_position_X = false;
                bool test_to_position_Y = false;
                Point point_from_grid = all_ways_to_my_tank_by_center_point[0][i];
                while(test_to_position_X  == false || test_to_position_Y == false)
                {
                    if(test_to_position_X == false)
                    {
                        bool test_to_mass = true;
                        while (Center_X < point_from_grid.X && test_to_mass == true)
                        {
                            mass_of_move.Add(3);
                            Center_X += 3;
                            if(Center_X >= point_from_grid.X)
                            {
                                test_to_mass = false;
                            }
                        }
                        while (Center_X > point_from_grid.X && test_to_mass == true)
                        {
                            mass_of_move.Add(4);
                            Center_X -= 3;
                            if(Center_X <= point_from_grid.X)
                            {
                                test_to_mass = false;
                            }
                        }
                        test_to_position_X = true;
                    }
                    if(test_to_position_Y == false)
                    {
                        bool test_to_mass = true;
                        while (Center_Y < point_from_grid.Y && test_to_mass == true)
                        {
                            mass_of_move.Add(2);
                            Center_Y += 3;
                            if(Center_Y >= point_from_grid.Y)
                            {
                                test_to_mass = false;
                            }
                        }
                        while (Center_Y > point_from_grid.Y && test_to_mass == true)
                        {
                            mass_of_move.Add(1);
                            Center_Y -= 3;
                            if(Center_Y <= point_from_grid.Y)
                            {
                                test_to_mass = false;
                            }
                        }
                        test_to_position_Y = true;
                    }
                }
            }
            
        }
        public bool Try_to_move_down(ref int start_row,ref int start_column)
        {
            List<Point> temp_list = new List<Point>();
            bool test_to_row = false;
     
            start_row++;
            for (int i = 0; i < EmptyRow.Count; i++)
            {
                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list.Add(Get_Center_Point_From_Grid_Cell(start_row, start_column));
                    test_to_row = true;
                    break;
                }

            }
            if (!test_to_row)
            {
                start_row--;
            }

            temp_list_to_all_list.AddRange(temp_list);
            return test_to_row;
        }
        public bool Try_to_move_left(ref int start_row,ref int start_column)
        {
            List<Point> temp_list = new List<Point>();
            bool test_to_column = false;
            start_column--;

            for (int i = 0; i < EmptyColumn.Count; i++)
            {

                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list.Add(Get_Center_Point_From_Grid_Cell(start_row, start_column));
                    test_to_column = true;
                }

            }
            if (!test_to_column)
            {
                start_column++;
            }

            temp_list_to_all_list.AddRange(temp_list);
            return test_to_column;
        }
        public bool Try_to_move_right(ref int start_row,ref int start_column)
        {
            List<Point> temp_list = new List<Point>();
            bool test_to_column = false;
            start_column++;

            for (int i = 0; i < EmptyColumn.Count; i++)
            {

                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list.Add(Get_Center_Point_From_Grid_Cell(start_row, start_column));
                    test_to_column = true;
                }

            }
            if (!test_to_column)
            {
                start_column--;
            }

            temp_list_to_all_list.AddRange(temp_list);
            return test_to_column;
        }
        public Point Get_Center_Point_From_Grid_Cell(int row,int column)
        {
            Point center_point;
            double cellWidth = grid_field.ActualWidth / grid_field.ColumnDefinitions.Count;
            double cellHeight = grid_field.ActualHeight / grid_field.RowDefinitions.Count;
            double CenterX = (cellWidth * column) + (cellWidth / 2);
            double CenterY = (cellHeight * row) + (cellHeight / 2);
            center_point = new Point(CenterX, CenterY);
            return center_point;
        }
        public List<int> Find_Cell_from_Point_in_Grid(Point Point_of_Bot,Point Point_of_my_tank)
        {
            List<Point> temp_list = new List<Point>();
            temp_list.Add(Point_of_Bot);
            temp_list.Add(Point_of_my_tank);
            List<int> list_of_cell = new List<int>();
            
            
            for (int l = 0; l < temp_list.Count; l++)
            {
                //Знайти рядок;
                double cumulativeHeight = 0;
                for (int i = 0; i < grid_field.RowDefinitions.Count; i++)
                {
                    cumulativeHeight += grid_field.RowDefinitions[i].ActualHeight;
                    if (temp_list[l].Y <= cumulativeHeight)
                    {
                        list_of_cell.Add(i);
                    }
                }
                //Знайти стовпчик;
                double cumulativeWidth = 0;
                for (int i = 0; i < grid_field.ColumnDefinitions.Count; i++)
                {
                    cumulativeWidth += grid_field.ColumnDefinitions[i].ActualWidth;
                    if (temp_list[l].X < cumulativeWidth)
                    {
                        list_of_cell.Add(i);
                    }
                }

            }
            return list_of_cell;
        }

        public void Test_to_Fire()
        {
            shouldContinue_rotate = false;
            test_to_visible_my_tank = true;
            List<Point> line_to_mytank = GetLinePoint(Center_of_bot,Center_of_my_tank);
            for(int i = 0; i < line_to_mytank.Count; i++)
            {
                for(int j = 0; j < Texture_point.Count; j++)
                {
                    if (line_to_mytank[i] == Texture_point[j])
                    {
                        test_to_visible_my_tank = false;
                        break;
                    }
                }
                if(test_to_visible_my_tank == false)
                {
                    break;
                }
            }
            if(test_to_visible_my_tank == true)
            {
                Application.Current.Dispatcher.InvokeAsync( async() =>
                {
                    
                    battle.Rotate_muzzle(Center_of_my_tank, oponents[0], canvas, side_of_move);
                    

                    Bullet_Oponent bullet_oponent = new Bullet_Oponent(canvas, oponents[0], oponents[0].Under_Muzzle_tank, Center_of_bot, side_of_move, battle.degrees_for_bullet, Texture_point, damage);
                    bullet_oponent.GetOponents(oponents);
                    bullet_oponent.GetMainTank(my_tank);
                    my_tank = bullet_oponent.ReturnMainTank();
                    bullet_oponent.bot_type = true;
                    await bullet_oponent.Make_a_shot();
                    //Для наступного SHOWDIALOG для завершення гри;
                    my_tank = bullet_oponent.ReturnMainTank();
                    
                    if (my_tank == null)
                    {
                        movement_timer.Dispose();
                        rotate_muzzle_timer.Dispose();
                        test_to_finish_game = true;
                    }
                }); 
            }
            shouldContinue_rotate = true;
        }
        public List<Point> GetLinePoint(Point CenterOfBot,Point CenterOfMyTank)
        {
            List<Point> points = new List<Point>();
            int x0 = (int)CenterOfBot.X;
            int x1 = (int)CenterOfMyTank.X;
            int y0 = (int)CenterOfBot.Y;
            int y1 = (int)CenterOfMyTank.Y;
            int dx = Math.Abs(x0 - x1);
            int dy = Math.Abs(y0 - y1);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;
            while (true)
            {
                points.Add(new Point(x0, y0));
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }
                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }

            }
            return points;
        }
        
        public void Find_Center_of_cell_field()
        {
            double cellWidth = grid_field.ActualWidth / grid_field.ColumnDefinitions.Count;
            double cellHeight = grid_field.ActualHeight / grid_field.RowDefinitions.Count;
            for (int i = 0; i < EmptyRow.Count - 1; i++)
            {
                for (int j = 0; j < EmptyColumn.Count - 1; j++)
                {
                    double CenterX = (EmptyColumn[j] * cellWidth) + (cellWidth / 2);
                    double CenterY = (EmptyRow[i] * cellHeight) + (cellHeight / 2);
                    Center_of_cell_field.Add(new Point(CenterX, CenterY));
                }
            }
        }   
    }
}
