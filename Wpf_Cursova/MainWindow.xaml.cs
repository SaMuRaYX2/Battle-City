using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfLibrary;

namespace Wpf_Cursova
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public List<Point> points_for_my_tank { get; set; }
        public List<Point> points_for_oponent { get; set; }
        public List<Point> points_tanks { get; set; }

        protected Tank my_tank { get; set; }

        protected Field my_field;
        public List<Oponent> oponents_tank { get; set; }
        public Tank My_tank
        {
            get { return my_tank; }
            set { my_tank = value; }
        }
        public Field My_field { get; set; }

        public List<Image> Texture_image { get; set; }

        public List<Point> Points { get; set; } // Points of Field and Tank;

        public Point locate_mouse { get; private set; }

        public Battle muzzle_rotate { get; private set; }
        public Grid grid_tank { get; private set; }
        public string side { get; set; }
        public Point point_bullet { get; set; }
        public Point point_bullet_to_oponent { get; set; }

        public MovementTank move;
        public MovementOponent move_oponent;
       
        public string side_rotate_oponent { get; set; }

        
        public MainWindow()
        {
            InitializeComponent();
            points_tanks = new List<Point>();
            play_zone.Background = Brushes.LightGray;
            oponents_tank = new List<Oponent>();
            this.Background = Brushes.DarkGray;
            muzzle_rotate = new Battle();
            start_game.Click += Start_game_Click;
            Window_BattleCity.KeyDown += SomeMove;
            //Window_BattleCity.KeyDown += MoveOponent;
            play_zone.MouseMove += Play_zone_MouseMove;
            play_zone.MouseLeftButtonDown += Play_zone_MouseLeftButtonDown;
            this.StateChanged += MainWindow_StateChanged;
            Window_BattleCity.KeyDown += Fire_Oponent_KeyDown;
            //Для подальшого редагування для Start Game
            move = new MovementTank();
            move_oponent = new MovementOponent();
        }

        private async void Fire_Oponent_KeyDown(object sender, KeyEventArgs e)
        {
            await Task.WhenAll(Do_Fire_By_Space(sender,e));
        }

        private async Task Do_Fire_By_Space(object sender,KeyEventArgs e)
        {
            if (oponents_tank.Count != 0)
            {
                My_tank = my_tank;
                if (Keyboard.IsKeyDown(Key.Space))
                {
                    if (move.point_for_bullet.X != 0 && move.point_for_bullet.Y != 0)
                    {
                        point_bullet_to_oponent = move_oponent.point_for_bullet;
                    }
                    else
                    {
                        Point point_center_of_tank_int_canvas = oponents_tank[0].Tank_grid.TransformToAncestor(play_zone).Transform(new Point(oponents_tank[0].Width / 2, oponents_tank[0].Height / 2));
                        point_bullet_to_oponent = point_center_of_tank_int_canvas;
                    }
                    side_rotate_oponent = move_oponent.side_of_move;
                    Bullet_Oponent bullet_oponent = new Bullet_Oponent(play_zone, oponents_tank[0], locate_mouse, oponents_tank[0].Under_Muzzle_tank, point_bullet_to_oponent, side_rotate_oponent , 0 , Points);
                    bullet_oponent.GetOponents(oponents_tank);
                    bullet_oponent.GetMainTank(my_tank);
                    await Task.WhenAll(bullet_oponent.Make_a_shot());
                    my_tank = bullet_oponent.ReturnMainTank();
                }
                await Task.Delay(100);
            }
        }
        

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            
        }

        private async void Play_zone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (my_tank != null)
            {
                My_tank = my_tank;
                if (move.point_for_bullet.X != 0 && move.point_for_bullet.Y != 0)
                {
                    point_bullet = move.point_for_bullet;
                }
                else
                {
                    Point point_center_of_tank_int_canvas = my_tank.Tank_grid.TransformToAncestor(play_zone).Transform(new Point(my_tank.Width / 2, my_tank.Height / 2));
                    point_bullet = point_center_of_tank_int_canvas;
                }
                if (locate_mouse != null)
                {
                    Bullet bullet = new Bullet(play_zone, my_tank, locate_mouse, my_tank.Under_Muzzle_tank, point_bullet, side, muzzle_rotate.degrees_for_bullet, Points);
                    bullet.GetOponents(oponents_tank);
                    await Task.WhenAll(bullet.Make_a_shot());
                    
                }
                else
                {
                    Bullet bullet = new Bullet(play_zone, my_tank, locate_mouse, my_tank.Under_Muzzle_tank, point_bullet, side, muzzle_rotate.degrees_for_bullet, Points);
                    Point temp_locate_mouse = new Point(play_zone.ActualWidth / 2, 0);
                    bullet.GetOponents(oponents_tank);
                    await Task.WhenAll(bullet.Make_a_shot());
                }
            }
        }

        private void Play_zone_MouseMove(object sender, MouseEventArgs e)
        {
            if (my_tank != null)
            {
                if (move != null)
                {
                    side = move.side_of_move;
                }
                else
                {
                    side = "nothing";
                }
                locate_mouse = e.GetPosition(play_zone);
                muzzle_rotate.Rotate_muzzle(locate_mouse, my_tank, play_zone, grid_tank, side);
            }
            
        }

        private async void SomeMove(object sender, KeyEventArgs e)
        {
            Move_tanks(sender, e);
            Move_Oponents(sender, e);
        }
        private async Task Move_tanks(object sender, KeyEventArgs e)
        {
            if (my_tank != null)
            {
                move.my_field = my_field;
                move.Points_denied = Points;
                move.canvas = play_zone;
                move_oponent.Points_denied = Points;
                move_oponent.canvas = play_zone;
                //MY_tank
                if (Keyboard.IsKeyDown(Key.Up))
                {
                    move.Press_PageUp( my_tank); 
                }
                //MY_tank
                if (Keyboard.IsKeyDown(Key.Down))
                {
                    move.Press_PageDown( my_tank);
                }
                //MY_tank
                if (Keyboard.IsKeyDown(Key.Left))
                {
                    move.Press_Home( my_tank);
                }
                //MY_tank
                if (Keyboard.IsKeyDown(Key.Right))
                {
                    move.Press_End( my_tank);
                }
                await Task.Delay(1);
            }
            
        }

        private async Task Move_Oponents(object sender, KeyEventArgs e)
        {
            if (oponents_tank.Count != 0)
            {
                move_oponent.my_field = my_field;
                if (Keyboard.IsKeyDown(Key.W))
                {
                    move_oponent.Press_W(oponents_tank[0]);
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    move_oponent.Press_S(oponents_tank[0]);
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    move_oponent.Press_D(oponents_tank[0]);
                }
                if (Keyboard.IsKeyDown(Key.A))
                {
                    move_oponent.Press_A(oponents_tank[0]);
                }
                await Task.Delay(1);
            }
            
        }


        private async void Start_game_Click(object sender, RoutedEventArgs e)
        {
            if(my_tank == null)
            {
                Tank tank = new Tank();
                tank.canvas = play_zone;
                tank.points_of_tanks = points_tanks;
                tank.Initial_grid_tank();
                my_tank = tank;
                grid_tank = tank.Tank_grid;
                muzzle_rotate.ActualWidth_Up_Muzzle = my_tank.UP_Muzzle_tank.ActualWidth;
                muzzle_rotate.ActualHeight_Up_Muzzle = my_tank.UP_Muzzle_tank.ActualHeight;
                Point topLeftInGrid_Under_muzzle = my_tank.Under_Muzzle_tank.TransformToAncestor(grid_tank).Transform(new Point(0, 0));
                my_tank.CenterUnderMuzzle = my_tank.Under_Muzzle_tank.TransformToAncestor(play_zone).Transform(topLeftInGrid_Under_muzzle);
                Point topLeftInGrid_Up_muzzle = my_tank.UP_Muzzle_tank.TransformToAncestor(grid_tank).Transform(new Point(0, 0));
                my_tank.CenterUpMuzzle = my_tank.UP_Muzzle_tank.TransformToAncestor(play_zone).Transform(topLeftInGrid_Up_muzzle);
            }
            
            Field field = new Field(play_zone);
            Texture_image = new List<Image>();
            Texture_image = field.Texture;
            field.points_for_my_tank = points_for_my_tank;
            field.points_for_oponent = points_for_oponent;
            field.oponents = oponents_tank;
            field.My_Tank = my_tank;
            field.Initial_field();
            Points = field.GetAllPoints();
            my_field = field;
            start_game.Visibility = Visibility.Collapsed;
            play_zone.UpdateLayout();
            
            //OPONENTS
            await BackGroundCreatingBots();
            
        }
        private async Task BackGroundCreatingBots()
        {
            Creating_oponent creating = new Creating_oponent();
            bool test_creating_bot = true;
            while (test_creating_bot = creating.MyAsyncIteration(oponents_tank,points_tanks, Points,ref play_zone))
            {
                if (test_creating_bot == true)
                {
                    await Task.Delay(100);
                }
                
            }
            
        }

    }
}
