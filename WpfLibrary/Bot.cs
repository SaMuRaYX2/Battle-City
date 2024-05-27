using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using NAudio.Wave;

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
        public Timer timer_refresh_point_of_tanks;
        private bool shouldContinue_rotate = true;
        private bool allow_to_update = false;
        private bool allow_move = false;
        private bool allow_to_refresh = false;
        public Tank my_tank { get; set; }
        private MovementOponent move_bot;
        public List<Point> Texture_point { get; set; }
        public Canvas canvas { get; set; }
        public string side_of_move { get; set; }
        public Battle battle;
        public int damage { get; set; }

        public bool test_to_finish_game = false;
        public bool test_to_exit_pauseGame = false;

        private bool test_to_visible_my_tank = false;
        public List<List<Point>> all_ways_to_my_tank_by_center_point;
        public List<Point> temp_list_to_all_list;
        public List<Point> the_best_way;
        public List<int> mass_of_move;
        public int iteration_of_move;
        bool first_start_to_find_empty_row_and_column = false;
        List<int> previous_mass_of_move_count;
        bool first_iteration = true;
        bool test_to_allow_changing_EmptyCell = false;
        WaveOutEvent waveOut_oponent;
        AudioFileReader audioFileReader_oponent;
        private bool sound_oponent_finish = true;

        public void PlayShotOponents()
        {
            if (sound_oponent_finish == true)
            {
                sound_oponent_finish = false;
                audioFileReader_oponent = new AudioFileReader("D:\\My Homework\\Cursova\\Texture_image\\boom8.wav");
                waveOut_oponent = new WaveOutEvent();
                waveOut_oponent.PlaybackStopped += WaveOut_oponent_PlaybackStopped;
                waveOut_oponent.Init(audioFileReader_oponent);
                waveOut_oponent.Play();
            }

        }

        private void WaveOut_oponent_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            audioFileReader_oponent.Dispose();
            waveOut_oponent.Dispose();
            sound_oponent_finish = true;
        }

        public Bot(Field field,List<Oponent> list_oponents,Tank my_tank,MovementOponent move_oponent,List<Point> Points,Canvas play_zone,int damage_of_oponents)
        {
            
            previous_mass_of_move_count = new List<int>();
            this.damage = damage_of_oponents;
            battle = new Battle();
            Texture_point = Points;
            canvas = play_zone;
            Center_of_cell_field = new List<Point>();
            this.move_bot = move_oponent;
            move_bot.IsOponentBot = true;
            this.my_tank = my_tank;
            this.field_of_playground = field;
            EmptyRow = field_of_playground.EmptyRow;
            EmptyColumn = field_of_playground.EmptyColumn;
            grid_field = field.Grid_field;
            Find_Center_of_cell_field();
            
            oponents = new List<Oponent>();
            this.oponents = list_oponents;
            //First iteration of timer
            Refresh_information_to_bot();
            UpdateWayToMyTank();
            //Movement timer
            TimerCallback timerCallback_move = new TimerCallback(Movement_timer);
            movement_timer = new Timer(timerCallback_move, null, 0, 80);
            //Rotate muzzle timer
            TimerCallback timerCallBack_muzzle = new TimerCallback(Rotate_Muzzle_timer);
            rotate_muzzle_timer = new Timer(timerCallBack_muzzle, null, 0, 1400);
            //Find_way_to_my_tank timer
            TimerCallback timerRefresh_way = new TimerCallback(Update_way_timer);
            refresh_way_timer = new Timer(timerRefresh_way, null, 100, 5000);
            //Refresh some points timer
            TimerCallback timerCallback_refresh_position = new TimerCallback(Refresh_some_point_of_tanks);
            timer_refresh_point_of_tanks = new Timer(timerCallback_refresh_position, null, 0, 16);
        }

        public void Refresh_some_point_of_tanks(object state)
        {
            if (test_to_exit_pauseGame == false)
            {
                if (oponents.Count != 0 && my_tank != null)
                {
                    if (allow_to_refresh == true)
                    {
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            if (test_to_allow_changing_EmptyCell == true)
                            {
                                Refresh_information_to_bot();
                            }
                            //Refresh_information_to_bot();
                        });

                    }
                }
            }
        }
        public void Update_way_timer(object state)
        {
            if (allow_to_update == true)
            {
                if (first_start_to_find_empty_row_and_column == true)
                {
                    Task.Run(() =>
                    {
                        UpdateWayToMyTank();
                    });
                }
            }
        }
        public void Movement_timer(object state)
        {
            if (allow_move == true)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (iteration_of_move < mass_of_move.Count)
                    {
                        allow_to_update = false;
                        Move_bot(mass_of_move[iteration_of_move]);
                        iteration_of_move++;

                    }
                    if (iteration_of_move == mass_of_move.Count)
                    {

                        allow_to_update = true;
                    }
                });

            }
        }
        public void Rotate_Muzzle_timer(object state)
        {
            if (shouldContinue_rotate)
            {
                Task.Run(() =>
                {
                    //Task.Delay(4000);
                    Test_to_Fire();
                });
                
            }
        }
        public void Refresh_information_to_bot()
        {
            //allow_to_update = false;
            //allow_to_refresh = false;
            canvas.UpdateLayout();
            if (oponents.Count != 0)
            {
                if (first_start_to_find_empty_row_and_column == false)
                {
                    field_of_playground.first_iteration_to_find_empty_cell = false;
                    field_of_playground.Find_Empty_Field();
                }
                else
                {
                    field_of_playground.Find_Empty_Field();
                }

                first_start_to_find_empty_row_and_column = true;
            }
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

            //allow_to_update = true;
            //allow_to_refresh = false;
        }
        
        
        public void Move_bot(int i)
        {

            if (oponents.Count != 0)
            {
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

            }

        }
        public void UpdateWayToMyTank()
        {
            test_to_allow_changing_EmptyCell = false;
            allow_move = false;
            allow_to_refresh = false;
            allow_to_update = false;
            iteration_of_move = 0;
            List<int> bot_position_on_grid = Find_Cell_from_Point_in_Grid(Center_of_bot, Center_of_my_tank);
            int bot_row = bot_position_on_grid[0];
            int bot_column = bot_position_on_grid[1];
            int my_row = bot_position_on_grid[2];
            int my_column = bot_position_on_grid[3];
            int number_of_way = 5;
            all_ways_to_my_tank_by_center_point = new List<List<Point>>();
            the_best_way = new List<Point>();
            mass_of_move = new List<int>();
            if (Center_of_bot.Y < Center_of_my_tank.Y)
            {

                while (number_of_way != 0)
                {
                    number_of_way--;
                    int start_row = bot_row;
                    int start_column = bot_column;
                    temp_list_to_all_list = new List<Point>();

                    bool test_to_row = true;
                    bool test_to_column = true;
                    Random rnd = new Random();
                    //IF rnd < 50 move is left;
                    //IF rnd >=50 move if right;
                    int type_to_move = rnd.Next(100);
                    while (test_to_row == true && start_row < my_row)
                    {
                        test_to_row = Try_to_move_down(ref start_row, ref start_column);
                    }


                    bool test_to_top = false;
                    bool rand_choose_right;
                    bool rand_choose_left;
                    if (type_to_move < 50)
                    {
                        rand_choose_right = false;
                        rand_choose_left = true;
                    }
                    else
                    {
                        rand_choose_right = true;
                        rand_choose_left = false;
                    }



                    while (start_row != my_row || start_column != my_column)
                    {
                        while (start_row < my_row)
                        {
                            if (test_to_top == false)
                            {
                                test_to_row = true;
                                test_to_column = true;
                            }
                            if (test_to_top == false)
                            {
                                test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                test_to_top = false;
                            }
                            
                            if (test_to_row == true && start_column < my_column)
                            {
                                test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    test_to_top = false;
                                }
                                
                            }
                            else if (test_to_row == true && start_column > my_column)
                            {
                                test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                if(test_to_column == true)
                                {
                                    test_to_top = false;
                                }
                                
                            }
                            if(test_to_top == true)
                            {
                                test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                test_to_top = true;
                            }

                            if (test_to_row == false)
                            {

                                test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                if (rand_choose_right == true)
                                {

                                    test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                    if (test_to_column == false)
                                    {
                                        rand_choose_right = false;
                                        rand_choose_left = true;
                                    }

                                }
                                else if (rand_choose_left == true)
                                {

                                    test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                    if (test_to_column == false)
                                    {
                                        rand_choose_left = false;
                                        rand_choose_right = true;
                                    }

                                }

                            }
                            //Допрацювати цей код точніше;
                            //else if (test_to_row == false && test_to_column == false)
                            //{
                            //    while (test_to_column == false)
                            //    {
                            //        test_to_row = Try_to_move_top(ref start_row, ref start_column);
                            //        if (start_column < my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_right(ref start_row, ref start_column);

                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = true;
                            //                rand_choose_left = false;
                            //            }
                            //        }
                            //        else if (start_column > my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = false;
                            //                rand_choose_left = true;
                            //            }
                            //        }
                            //        if (test_to_row == false)
                            //        {
                            //            while (test_to_row = Try_to_move_down(ref start_row, ref start_column) == true || test_to_column == true)
                            //            {
                            //                if (start_column < my_column)
                            //                {
                            //                    test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //                }
                            //                else if (start_column > my_column)
                            //                {
                            //                    test_to_column = Try_to_move_right(ref start_row, ref start_column);
                            //                }
                            //            }
                            //        }

                            //    }
                            //}

                        }
                        while (start_row == my_row && start_column != my_column)
                        {
                            test_to_column = false;
                            if (start_column < my_column)
                            {
                                test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_right = true;
                                    rand_choose_left = false;
                                }
                            }
                            else if (start_column > my_column)
                            {
                                test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_left = true;
                                    rand_choose_right = false;
                                }
                            }
                            if (test_to_column == false)
                            {
                                test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                if (test_to_row == false)
                                {
                                    test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                    test_to_top = true;
                                }
                            }
                        }
                        while (start_row > my_row)
                        {
                            

                            if (start_column < my_column)
                            {
                                test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_right = true;
                                    rand_choose_left = false;
                                }

                            }
                            else if (start_column > my_column)
                            {
                                test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_right = false;
                                    rand_choose_left = true;
                                }

                            }

                            if (start_column != my_column)
                            {
                                if (test_to_column == false)
                                {
                                    test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                }
                                if (test_to_column == true)
                                {
                                    test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                }
                            }
                            else if(start_column == my_column)
                            {
                                test_to_row = Try_to_move_top(ref start_row, ref start_column);
                            }
                            if (test_to_row == false)
                            {

                                while (test_to_row == false)
                                {
                                    if (rand_choose_left == true)
                                    {
                                        test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                        if (test_to_column == false)
                                        {
                                            rand_choose_left = false;
                                            rand_choose_right = true;
                                        }
                                    }
                                    else if (rand_choose_right == true)
                                    {
                                        test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                        if (test_to_column == false)
                                        {
                                            rand_choose_right = false;
                                            rand_choose_left = true;
                                        }
                                    }
                                    test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                }



                            }




                            //Допрацювати даний код
                            //else if (test_to_row == false && test_to_column == false)
                            //{
                            //    while (test_to_column == false)
                            //    {
                            //        test_to_row = Try_to_move_top(ref start_row, ref start_column);
                            //        if (start_column < my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_right(ref start_row, ref start_column);

                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = true;
                            //                rand_choose_left = false;
                            //            }
                            //        }
                            //        else if (start_column > my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = false;
                            //                rand_choose_left = true;
                            //            }
                            //        }
                            //        if (test_to_row == false)
                            //        {
                            //            while (test_to_row = Try_to_move_down(ref start_row, ref start_column) == true || test_to_column == true)
                            //            {
                            //                if (start_column < my_column)
                            //                {
                            //                    test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //                }
                            //                else if (start_column > my_column)
                            //                {
                            //                    test_to_column = Try_to_move_right(ref start_row, ref start_column);
                            //                }
                            //            }
                            //        }

                            //    }
                            //}
                        }

                    }
                    all_ways_to_my_tank_by_center_point.Add(temp_list_to_all_list);

                }
            }
            else if (Center_of_bot.Y > Center_of_my_tank.Y)
            {
                while (number_of_way != 0)
                {
                    number_of_way--;
                    int start_row = bot_row;
                    int start_column = bot_column;
                    temp_list_to_all_list = new List<Point>();

                    bool test_to_row = true;
                    bool test_to_column = true;
                    Random rnd = new Random();
                    //IF rnd < 50 move is left;
                    //IF rnd >=50 move if right;
                    int type_to_move = rnd.Next(100);
                    while (test_to_row == true && start_row > my_row)
                    {
                        test_to_row = Try_to_move_top(ref start_row, ref start_column);
                    }


                    bool test_to_down = false;
                    bool rand_choose_right;
                    bool rand_choose_left;
                    if (type_to_move < 50)
                    {
                        rand_choose_right = false;
                        rand_choose_left = true;
                    }
                    else
                    {
                        rand_choose_right = true;
                        rand_choose_left = false;
                    }



                    while (start_row != my_row || start_column != my_column)
                    {
                        while (start_row > my_row)
                        {
                            if (test_to_down == false)
                            {
                                test_to_row = true;
                                test_to_column = true;
                            }
                            if (test_to_down == false)
                            {
                                test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                test_to_down = false;
                            }
                            
                            if (test_to_row == true && start_column < my_column)
                            {
                                test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    test_to_down = false;
                                }

                            }
                            else if (test_to_row == true && start_column > my_column)
                            {
                                test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    test_to_down = false;
                                }

                            }
                            if (test_to_down == true)
                            {
                                test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                test_to_down = true;
                            }

                            if (test_to_row == false)
                            {

                                test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                if (rand_choose_right == true)
                                {

                                    test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                    if (test_to_column == false)
                                    {
                                        rand_choose_right = false;
                                        rand_choose_left = true;
                                    }

                                }
                                else if (rand_choose_left == true)
                                {

                                    test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                    if (test_to_column == false)
                                    {
                                        rand_choose_left = false;
                                        rand_choose_right = true;
                                    }

                                }

                            }
                            //Допрацювати цей код точніше;
                            //else if (test_to_row == false && test_to_column == false)
                            //{
                            //    while (test_to_column == false)
                            //    {
                            //        test_to_row = Try_to_move_top(ref start_row, ref start_column);
                            //        if (start_column < my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_right(ref start_row, ref start_column);

                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = true;
                            //                rand_choose_left = false;
                            //            }
                            //        }
                            //        else if (start_column > my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = false;
                            //                rand_choose_left = true;
                            //            }
                            //        }
                            //        if (test_to_row == false)
                            //        {
                            //            while (test_to_row = Try_to_move_down(ref start_row, ref start_column) == true || test_to_column == true)
                            //            {
                            //                if (start_column < my_column)
                            //                {
                            //                    test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //                }
                            //                else if (start_column > my_column)
                            //                {
                            //                    test_to_column = Try_to_move_right(ref start_row, ref start_column);
                            //                }
                            //            }
                            //        }

                            //    }
                            //}

                        }
                        while (start_row == my_row && start_column != my_column)
                        {
                            test_to_column = false;
                            if (start_column < my_column)
                            {
                                test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_right = true;
                                    rand_choose_left = false;
                                }
                            }
                            else if (start_column > my_column)
                            {
                                test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_left = true;
                                    rand_choose_right = false;
                                }
                            }
                            if (test_to_column == false)
                            {
                                test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                if (test_to_row == false)
                                {
                                    test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                    test_to_down = true;
                                }
                            }
                        }
                        while (start_row < my_row)
                        {

                            if (start_column < my_column)
                            {
                                test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_right = true;
                                    rand_choose_left = false;
                                }

                            }
                            else if (start_column > my_column)
                            {
                                test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                if (test_to_column == true)
                                {
                                    rand_choose_right = false;
                                    rand_choose_left = true;
                                }

                            }

                            if (start_column != my_column)
                            {
                                if (test_to_column == false)
                                {
                                    test_to_row = Try_to_move_top(ref start_row, ref start_column);
                                }
                                if (test_to_column == true)
                                {
                                    test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                }
                            }
                            else if (start_column == my_column)
                            {
                                test_to_row = Try_to_move_down(ref start_row, ref start_column);
                            }
                            if (test_to_row == false)
                            {

                                while (test_to_row == false)
                                {
                                    if (rand_choose_left == true)
                                    {
                                        test_to_column = Try_to_move_left(ref start_row, ref start_column);
                                        if (test_to_column == false)
                                        {
                                            rand_choose_left = false;
                                            rand_choose_right = true;
                                        }
                                    }
                                    else if (rand_choose_right == true)
                                    {
                                        test_to_column = Try_to_move_right(ref start_row, ref start_column);
                                        if (test_to_column == false)
                                        {
                                            rand_choose_right = false;
                                            rand_choose_left = true;
                                        }
                                    }
                                    test_to_row = Try_to_move_down(ref start_row, ref start_column);
                                }



                            }





                            //Допрацювати даний код
                            //else if (test_to_row == false && test_to_column == false)
                            //{
                            //    while (test_to_column == false)
                            //    {
                            //        test_to_row = Try_to_move_top(ref start_row, ref start_column);
                            //        if (start_column < my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_right(ref start_row, ref start_column);

                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = true;
                            //                rand_choose_left = false;
                            //            }
                            //        }
                            //        else if (start_column > my_column && test_to_row == true)
                            //        {
                            //            test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //            if (test_to_column == true)
                            //            {
                            //                rand_choose_right = false;
                            //                rand_choose_left = true;
                            //            }
                            //        }
                            //        if (test_to_row == false)
                            //        {
                            //            while (test_to_row = Try_to_move_down(ref start_row, ref start_column) == true || test_to_column == true)
                            //            {
                            //                if (start_column < my_column)
                            //                {
                            //                    test_to_column = Try_to_move_left(ref start_row, ref start_column);
                            //                }
                            //                else if (start_column > my_column)
                            //                {
                            //                    test_to_column = Try_to_move_right(ref start_row, ref start_column);
                            //                }
                            //            }
                            //        }

                            //    }
                            //}
                        }

                    }
                    all_ways_to_my_tank_by_center_point.Add(temp_list_to_all_list);

                }

            }

            the_best_way = Choose_the_best_way(all_ways_to_my_tank_by_center_point);


            
            //IF MOVE UP == 1;
            //IF MOVE DOWN == 2;
            //IF MOVE RIGHT == 3;
            //IF MOVE LEFT == 4;
            double Center_X = Center_of_bot.X;
            double Center_Y = Center_of_bot.Y;
            for(int i = 0; i < the_best_way.Count; i++)
            {
                bool test_to_position_X = false;
                bool test_to_position_Y = false;
                Point point_from_grid = the_best_way[i];
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
            //if(first_iteration == false)
            //{
            //    if(mass_of_move.Count > previous_mass_of_move_count.Count)
            //    {
            //        mass_of_move = previous_mass_of_move_count;
            //    }
            //}
            //previous_mass_of_move_count = mass_of_move;
            first_iteration = false;
            allow_move = true;
            allow_to_refresh = true;
            test_to_allow_changing_EmptyCell = true;

        }
        public bool Try_to_move_down(ref int start_row,ref int start_column)
        {
            Point temp_list = new Point();
            bool test_to_row = false;
     
            start_row++;
            for (int i = 0; i < EmptyRow.Count; i++)
            {
                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list = Get_Center_Point_From_Grid_Cell(start_row, start_column);
                    test_to_row = true;
                    break;
                }
            }
            
            if (!test_to_row)
            {
                start_row--;
            }
            if (temp_list.X != 0 && temp_list.Y != 0)
            {
                temp_list_to_all_list.Add(temp_list);
            }
            return test_to_row;
        }
        public bool Try_to_move_left(ref int start_row, ref int start_column)
        {
            Point temp_list = new Point();
            bool test_to_column = false;
            start_column--;

            for (int i = 0; i < EmptyColumn.Count; i++)
            {
                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list = Get_Center_Point_From_Grid_Cell(start_row, start_column);
                    test_to_column = true;
                    break;
                }
            }
            
            if (!test_to_column)
            {
                start_column++;
            }
            if (temp_list.X != 0 && temp_list.Y != 0)
            {
                temp_list_to_all_list.Add(temp_list);
            }
            return test_to_column;
        }
        public bool Try_to_move_right(ref int start_row,ref int start_column)
        {
            Point temp_list = new Point();
            bool test_to_column = false;
            start_column++;

            for (int i = 0; i < EmptyColumn.Count; i++)
            {
                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list = Get_Center_Point_From_Grid_Cell(start_row, start_column);
                    test_to_column = true;
                    break;
                }
            }
            
            if (!test_to_column)
            {
                start_column--;
            }
            if (temp_list.X != 0 && temp_list.Y != 0)
            {
                temp_list_to_all_list.Add(temp_list);
            }
            return test_to_column;
        }
        public bool Try_to_move_top(ref int start_row,ref int start_column)
        {
            Point temp_list = new Point();
            bool test_to_row = false;

            start_row--;
            for (int i = 0; i < EmptyRow.Count; i++)
            {
                if (start_row == EmptyRow[i] && start_column == EmptyColumn[i])
                {
                    temp_list = Get_Center_Point_From_Grid_Cell(start_row, start_column);
                    test_to_row = true;
                    break;
                }
            }
            
            if (!test_to_row)
            {
                start_row++;
            }
            if (temp_list.X != 0 && temp_list.Y != 0)
            {
                temp_list_to_all_list.Add(temp_list);
            }
            return test_to_row;
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
                        break;
                    }
                }
                //Знайти стовпчик;
                double cumulativeWidth = 0;
                for (int i = 0; i < grid_field.ColumnDefinitions.Count; i++)
                {
                    cumulativeWidth += grid_field.ColumnDefinitions[i].ActualWidth;
                    if (temp_list[l].X <= cumulativeWidth)
                    {
                        list_of_cell.Add(i);
                        break;
                    }
                }

            }
            return list_of_cell;
        }

        public List<Point> Choose_the_best_way(List<List<Point>> all_finded_ways)
        {
            List<Point> best_way = new List<Point>();
            int the_best_index = 0;
            int max_count = int.MinValue;
            int min_count = int.MaxValue;

            for (int i = 0; i < all_finded_ways.Count; i++)
            {
                if (all_finded_ways[i].Count >= max_count)
                {
                    max_count = all_finded_ways[i].Count;

                }
                else if (all_finded_ways[i].Count <= min_count)
                {
                    min_count = all_finded_ways[i].Count;
                    the_best_index = i;
                }
            }

            best_way = all_finded_ways[the_best_index];
            return best_way;
        }
        public void Test_to_Fire()
        {
            
            shouldContinue_rotate = false;
            test_to_visible_my_tank = false;
            bool temp_test = true;
            List<Point> line_to_mytank = GetLinePoint(Center_of_bot,Center_of_my_tank);
            for(int i = 0; i < line_to_mytank.Count; i++)
            {
                for(int j = 0; j < Texture_point.Count; j++)
                {
                    if (line_to_mytank[i] == Texture_point[j])
                    {
                        temp_test = false;
                        test_to_visible_my_tank = false;
                        break;
                    }
                    
                }
                if(temp_test == true)
                {
                    test_to_visible_my_tank = true;
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
                    PlayShotOponents();
                    allow_move = false;
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
                    allow_move = true;
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
            for (int i = 0; i < EmptyRow.Count; i++)
            {

                double CenterX = (EmptyColumn[i] * cellWidth) + (cellWidth / 2);
                double CenterY = (EmptyRow[i] * cellHeight) + (cellHeight / 2);
                Center_of_cell_field.Add(new Point(CenterX, CenterY));

            }
        }   
    }
}
