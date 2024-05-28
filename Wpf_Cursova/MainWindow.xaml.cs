using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfLibrary;
using NAudio.Wave;

namespace Wpf_Cursova
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Type_of_Game { get; set; }
        public string Name_of_player_1 { get; set; }
        public string Name_of_player_2 { get; set; }
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

        public List<Point> Points { get; set; } // Points of Field;

        public Point locate_mouse { get; private set; }

        public Battle muzzle_rotate { get; private set; }
        public Grid grid_tank { get; private set; }
        public string side { get; set; }
        public Point point_bullet { get; set; }
        public Point point_bullet_to_oponent { get; set; }

        public MovementTank move;
        public MovementOponent move_oponent;

        public string side_rotate_oponent { get; set; }
        //Creating Bots or Oponents;
        Creating_oponent creating = new Creating_oponent();
        //Creating Bots or Oponents;
        //Initial Bot
        private Bot bot;
        //Initial Bot
        private readonly HashSet<Key> pressedKeysTank1 = new HashSet<Key>();
        private readonly HashSet<Key> pressedKeysTank2 = new HashSet<Key>();

        private DispatcherTimer tank1Timer;
        private DispatcherTimer tank2Timer;
        private DispatcherTimer fireTimer;
        protected DispatcherTimer Clear_Killed_Tank;
        public Timer timer_check_finish_game;

        private bool canShoot = true;
        private bool canShootMouse = true;
        private DispatcherTimer firebymouse;
        private List<Key> list_key_tank1 = new List<Key>();
        private List<Key> list_key_tank2 = new List<Key>();
        public int max_number_of_oponents { get; set; }
        public int number_of_oponents_now { get; set; }
        public int damage_of_my_tank { get; set; }
        public int damage_of_oponent { get; set; }
        public bool exit_from_pause_menu { get; set; }
        private bool sound_oponent_finish = true;
        private bool sound_tank_finish = true;
        
        WaveOutEvent waveOut_tank;
        WaveOutEvent waveOut_oponent;
        AudioFileReader audioFileReader_tank;
        AudioFileReader audioFileReader_oponent;


        public MainWindow()
        {
            InitializeComponent();
            pause_game.MouseLeftButtonDown += Pause_game_MouseLeftButtonDown;
            exit_from_pause_menu = false;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;

        }

        
        public void PlayShotTank()
        {
            if (sound_tank_finish == true)
            {
                sound_tank_finish = false;
                audioFileReader_tank = new AudioFileReader("D:\\My Homework\\Cursova\\Texture_image\\boom9.wav");
                waveOut_tank = new WaveOutEvent();
                waveOut_tank.PlaybackStopped += WaveOut_tank_PlaybackStopped;
                waveOut_tank.Init(audioFileReader_tank);
                waveOut_tank.Play();
            }
        }

        private void WaveOut_tank_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            audioFileReader_tank.Dispose();
            waveOut_tank.Dispose();
            sound_tank_finish = true;
        }

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

        private void Pause_game_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PauseGame pauseGame = new PauseGame();
            pauseGame.my_bot = bot;
            pauseGame.ShowDialog();
            
            
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (Type_of_Game == "Гра на двох")
            {
                if (IsTank1Key(e.Key))
                {
                    list_key_tank1.Add(e.Key);
                    lock (pressedKeysTank1)
                    {
                        if (list_key_tank1.Count != 1)
                        {
                            if (list_key_tank1[list_key_tank1.Count - 1] == list_key_tank1[list_key_tank1.Count - 2])
                            {
                                pressedKeysTank1.Add(e.Key);
                            }
                            else
                            {
                                pressedKeysTank1.Clear();
                                pressedKeysTank1.Add(e.Key);
                            }
                        }
                        else
                        {
                            pressedKeysTank1.Add(e.Key);
                        }
                    }
                }
                else if (IsTank2Key(e.Key))
                {
                    list_key_tank2.Add(e.Key);
                    lock (pressedKeysTank2)
                    {
                        if (list_key_tank2.Count != 1)
                        {
                            if (list_key_tank2[list_key_tank2.Count - 1] == list_key_tank2[list_key_tank2.Count - 2])
                            {
                                pressedKeysTank2.Add(e.Key);
                            }
                            else
                            {
                                pressedKeysTank2.Clear();
                                pressedKeysTank2.Add(e.Key);
                            }
                        }
                        else
                        {
                            pressedKeysTank2.Add(e.Key);
                        }
                    }
                }
                else if (e.Key == Key.Space)
                {

                    Do_Fire_By_Space();

                }
            }
            else
            {
                if (IsTank1Key(e.Key))
                {
                    list_key_tank1.Add(e.Key);
                    lock (pressedKeysTank1)
                    {
                        if (list_key_tank1.Count != 1)
                        {
                            if (list_key_tank1[list_key_tank1.Count - 1] == list_key_tank1[list_key_tank1.Count - 2])
                            {
                                pressedKeysTank1.Add(e.Key);
                            }
                            else
                            {
                                pressedKeysTank1.Clear();
                                pressedKeysTank1.Add(e.Key);
                            }
                        }
                        else
                        {
                            pressedKeysTank1.Add(e.Key);
                        }
                    }
                }
            }
        }
        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            if (Type_of_Game == "Гра на двох")
            {

                if (IsTank1Key(e.Key))
                {
                    lock (pressedKeysTank1)
                    {
                        pressedKeysTank1.Remove(e.Key);
                    }
                }

                else if (IsTank2Key(e.Key))
                {
                    lock (pressedKeysTank2)
                    {
                        pressedKeysTank2.Remove(e.Key);
                    }
                }
            }
            else
            {
                if (IsTank1Key(e.Key))
                {
                    lock (pressedKeysTank1)
                    {
                        pressedKeysTank1.Remove(e.Key);
                    }
                }
            }

        }

        private bool IsTank1Key(Key key)
        {
            return key == Key.Up || key == Key.Down || key == Key.Left || key == Key.Right;
        }
        private bool IsTank2Key(Key key)
        {
            return key == Key.W || key == Key.S || key == Key.A || key == Key.D;
        }
        private void UpdateTank1()
        {
            List<Key> keys;
            lock (pressedKeysTank1)
            {
                keys = new List<Key>(pressedKeysTank1);
            }
            if (my_tank != null)
            {
                foreach (var key in keys)
                {

                    move.my_field = my_field;
                    move.Points_denied = Points;
                    move.canvas = play_zone;
                    switch (key)
                    {
                        case Key.Up:
                            {
                                move.Press_PageUp(my_tank);
                                break;
                            }
                        case Key.Down:
                            {
                                move.Press_PageDown(my_tank);
                                break;
                            }
                        case Key.Left:
                            {
                                move.Press_Home(my_tank);
                                break;
                            }
                        case Key.Right:
                            {
                                move.Press_End(my_tank);
                                break;
                            }

                    }
                    damage_of_my_tank = move.damage_of_my_tank;

                }
            }

        }
        private void UpdateTank2()
        {
            List<Key> keys;
            lock (pressedKeysTank2)
            {
                keys = new List<Key>(pressedKeysTank2);
            }
            if (oponents_tank.Count != 0)
            {
                foreach (var key in keys)
                {


                    move_oponent.Points_denied = Points;
                    move_oponent.canvas = play_zone;
                    move_oponent.my_field = my_field;
                    switch (key)
                    {
                        case Key.W:
                            {
                                move_oponent.Press_W(oponents_tank[0]);
                                break;
                            }
                        case Key.S:
                            {
                                move_oponent.Press_S(oponents_tank[0]);
                                break;
                            }
                        case Key.A:
                            {
                                move_oponent.Press_A(oponents_tank[0]);
                                break;
                            }
                        case Key.D:
                            {
                                move_oponent.Press_D(oponents_tank[0]);
                                break;
                            }


                    }
                    damage_of_oponent = move_oponent.damage_of_oponent;



                }
            }
        }


        // <--------------------------------------->

        private async void Do_Fire_By_Space()
        {
            if (!canShoot) return;
            canShoot = false;
            fireTimer.Stop();
            if (oponents_tank.Count != 0)
            {
                My_tank = my_tank;

                if (move_oponent.point_for_bullet.X != 0 && move_oponent.point_for_bullet.Y != 0)
                {
                    point_bullet_to_oponent = move_oponent.point_for_bullet;
                }
                else
                {
                    Point point_center_of_tank_int_canvas = oponents_tank[0].Tank_grid.TransformToAncestor(play_zone).Transform(new Point(oponents_tank[0].Width / 2, oponents_tank[0].Height / 2));
                    point_bullet_to_oponent = point_center_of_tank_int_canvas;
                }
                side_rotate_oponent = move_oponent.side_of_move;

                Bullet_Oponent bullet_oponent = new Bullet_Oponent(play_zone, oponents_tank[0], oponents_tank[0].Under_Muzzle_tank, point_bullet_to_oponent, side_rotate_oponent, 0, Points, damage_of_oponent);
                bullet_oponent.GetOponents(oponents_tank);
                bullet_oponent.GetMainTank(my_tank);
                PlayShotOponents();
                await Task.WhenAll(bullet_oponent.Make_a_shot());



                my_tank = bullet_oponent.ReturnMainTank();
                if (my_tank == null)
                {
                    Result_of_Game result_Of_Game = new Result_of_Game();
                    result_Of_Game.Name_of_winner = Name_of_player_2;
                    result_Of_Game.Enter_Winner_Name();
                    result_Of_Game.ShowDialog();
                    DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                    result_Of_Game.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
                }
                fireTimer.Start();



            }
        }



        private async void Play_zone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!canShootMouse) return;
            canShootMouse = false;
            if (my_tank != null)
            {
                firebymouse.Stop();
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

                Bullet bullet = new Bullet(play_zone, my_tank, my_tank.Under_Muzzle_tank, point_bullet, side, muzzle_rotate.degrees_for_bullet, Points, damage_of_my_tank);
                bullet.GetOponents(oponents_tank);
                bullet.GetMainTank(my_tank);
                bullet.timer = Clear_Killed_Tank;
                bullet.creating_oponent = creating;
                PlayShotTank();
                await Task.WhenAll(bullet.Make_a_shot());
                if(oponents_tank.Count == 0)
                {
                    Result_of_Game result_Of_Game = new Result_of_Game();
                    result_Of_Game.Name_of_winner = Name_of_player_1;
                    result_Of_Game.Enter_Winner_Name();
                    result_Of_Game.ShowDialog();
                    DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                    result_Of_Game.BeginAnimation(Window.OpacityProperty, fadeInAnimation);
                }
                firebymouse.Start();
                

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
                muzzle_rotate.Rotate_muzzle(locate_mouse, my_tank, play_zone, side);
            }
            
        }

        internal void Start_game_Click_for_two_player()
        {
            Type_of_Game = "Гра на двох";
            points_tanks = new List<Point>();
            play_zone.Background = Brushes.Thistle;
            oponents_tank = new List<Oponent>();
            this.Background = Brushes.Black;
            muzzle_rotate = new Battle();
            this.KeyDown += WindowKeyDown;
            this.KeyUp += WindowKeyUp;
            tank1Timer = new DispatcherTimer();
            tank2Timer = new DispatcherTimer();
            fireTimer = new DispatcherTimer();
            firebymouse = new DispatcherTimer();
            Clear_Killed_Tank = new DispatcherTimer();


            tank1Timer.Interval = TimeSpan.FromMilliseconds(16);
            tank2Timer.Interval = TimeSpan.FromMilliseconds(16);
            fireTimer.Interval = TimeSpan.FromMilliseconds(2000);
            firebymouse.Interval = TimeSpan.FromMilliseconds(2000);
            Clear_Killed_Tank.Interval = TimeSpan.FromMilliseconds(10000);

            tank1Timer.Tick += (s, e) => UpdateTank1();
            tank2Timer.Tick += (s, e) => UpdateTank2();
            fireTimer.Tick += (s, e) => canShoot = true;
            firebymouse.Tick += (s, e) => canShootMouse = true;
            Clear_Killed_Tank.Tick += (s, e) => Clear_Loyaut_from_oponents();

            tank1Timer.Start();
            tank2Timer.Start();
            fireTimer.Start();
            firebymouse.Start();

            play_zone.MouseLeftButtonDown += Play_zone_MouseLeftButtonDown;
            play_zone.MouseMove += Play_zone_MouseMove;
            

            //Для подальшого редагування для Start Game
            move = new MovementTank();
            move_oponent = new MovementOponent();
            damage_of_my_tank = move.damage_of_my_tank;
            damage_of_oponent = move_oponent.damage_of_oponent;
            //Для 2-ох іграків;
            max_number_of_oponents = 8;
            number_of_oponents_now = 0;
            //Для 2-ох іграків;
            
            if (my_tank == null)
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
            play_zone.UpdateLayout();

            //OPONENTS
            creating.oponents_tank = oponents_tank;
            creating.Points = this.Points;
            creating.play_zone = this.play_zone;
            creating.max_number_of_oponents = this.max_number_of_oponents;
            creating.number_of_oponents_now = this.number_of_oponents_now;
            creating.BackGroundCreatingBots();
        }
        internal async void Start_game_for_one_player()
        {
            //MessageBoxResult result = MessageBox.Show("Покищо розробка режиму гри на одного гравця відбувається в планах!!!", "Зачекайте зовсім скоро(OK - вийти в Menu,Cancel - вийти з гри)", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            //if (result == MessageBoxResult.OK)
            //{
            //    StartMenu menu = new StartMenu();
            //    menu.Show();
            //    this.Close();
            //}
            //else if (result == MessageBoxResult.Cancel)
            //{
            //    this.Close();
            //}
            Type_of_Game = "Гра з ботом";
            points_tanks = new List<Point>();
            play_zone.Background = Brushes.Thistle;
            oponents_tank = new List<Oponent>();
            this.Background = Brushes.Black;
            muzzle_rotate = new Battle();
            this.KeyDown += WindowKeyDown;
            this.KeyUp += WindowKeyUp;
            tank1Timer = new DispatcherTimer();
            firebymouse = new DispatcherTimer();
            Clear_Killed_Tank = new DispatcherTimer();

            tank1Timer.Interval = TimeSpan.FromMilliseconds(16);
            firebymouse.Interval = TimeSpan.FromMilliseconds(2000);
            Clear_Killed_Tank.Interval = TimeSpan.FromMilliseconds(10000);
            tank1Timer.Tick += (s, e) => UpdateTank1();
            firebymouse.Tick += (s, e) => canShootMouse = true;
            Clear_Killed_Tank.Tick += (s, e) => Clear_Loyaut_from_oponents();

            tank1Timer.Start();
            firebymouse.Start();

            play_zone.MouseLeftButtonDown += Play_zone_MouseLeftButtonDown;
            play_zone.MouseMove += Play_zone_MouseMove;

            move = new MovementTank();
            move_oponent = new MovementOponent();
            damage_of_my_tank = move.damage_of_my_tank;
            damage_of_oponent = move_oponent.damage_of_oponent;
            //Для 1-го іграка;
            max_number_of_oponents = 6;
            number_of_oponents_now = 0;
            //Для 1-го іграка;
            if (my_tank == null)
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
            play_zone.UpdateLayout();

            //OPONENTS
            creating.oponents_tank = oponents_tank;
            creating.Points = this.Points;
            creating.play_zone = this.play_zone;
            creating.max_number_of_oponents = this.max_number_of_oponents;
            creating.number_of_oponents_now = this.number_of_oponents_now;
            await creating.BackGroundCreatingBots();
            //Initialization bot
            Initial_Bot();
        }
        public void Initial_Bot()
        {
            bot = new Bot(my_field, oponents_tank, my_tank, move_oponent, Points, play_zone, damage_of_oponent);
            
            TimerCallback timerCallBack = new TimerCallback(OnTimerEvent);
            timer_check_finish_game = new Timer(timerCallBack,null,1000,1000);
            
        }
        public void OnTimerEvent(object state)
        {
            if (bot.test_to_finish_game == true)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {

                    timer_check_finish_game.Dispose();
                    Result_of_Game result_Of_Game = new Result_of_Game();
                    result_Of_Game.Name_of_winner = Name_of_player_2;
                    result_Of_Game.Enter_Winner_Name();
                    result_Of_Game.ShowDialog();
                    DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                    result_Of_Game.BeginAnimation(Window.OpacityProperty, fadeInAnimation);

                    //bot.Refresh_information_to_bot();

                });
            }
            
        }
        internal void Start_game_for_multiplayer()
        {
            MessageBoxResult result = MessageBox.Show("Покищо розробка режиму гри Multiplayer відбувається в планах!!!", "Зачекайте зовсім скоро(OK - вийти в Menu,Cancel - вийти з гри)", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                StartMenu menu = new StartMenu();
                menu.Show();
                this.Close();
            }
            else if(result == MessageBoxResult.Cancel)
            {
                this.Close();
            }
        }
        
        private void Clear_Loyaut_from_oponents()
        {
            if (my_tank != null)
            {
                if (my_tank.Killed_Oponents.Count != 0)
                {
                    play_zone.Children.Remove(my_tank.Killed_Oponents[0]);
                    my_tank.Killed_Oponents.RemoveAt(0);
                }
                else
                {
                    Clear_Killed_Tank.Stop();
                }
            }
            else
            {
                Clear_Killed_Tank.Stop();
            }
        }

    }
}
