using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public Tank my_tank { get; set; }
        private MovementOponent move_bot;
        public List<Point> Texture_point { get; set; }
        public Canvas canvas { get; set; }
        
        public Bot(Field field,List<Oponent> list_oponents,Tank my_tank,MovementOponent move_oponent,List<Point> Points,Canvas play_zone)
        {
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
            TimerCallback timerCallback = new TimerCallback(Movement_timer);
            movement_timer = new Timer(timerCallback, null, 1000, 25);

        }
        public void Movement_timer(object state)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Move_bot();
            });
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
        }
        
        public void Move_bot()
        {
            Random rand = new Random();
            int random_value = rand.Next(4);
            move_bot.Points_denied = Texture_point;
            move_bot.canvas = canvas;
            move_bot.my_field = field_of_playground;
            switch (random_value)
            {
                case 0:
                    {
                        move_bot.Press_W(oponents[0]);
                        break;
                    }
                case 1:
                    {
                        move_bot.Press_S(oponents[0]);
                        break;
                    }
                case 2:
                    {
                        move_bot.Press_D(oponents[0]);
                        break;
                    }
                case 3:
                    {
                        move_bot.Press_A(oponents[0]);
                        break;
                    }
            }
        }
        public void UpdatePosition()
        {

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
