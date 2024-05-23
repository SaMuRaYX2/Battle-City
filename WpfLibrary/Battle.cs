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
    public class Battle
    {
        public double MouseX {  get; private set; }
        public double MouseY { get; private set; }
        public double TankX { get; private set; }
        public double TankY {  get; private set; }
        public Shape Under_muzzle { get; private set; }
        public Shape UP_muzzle { get; private set; }
        public double ActualWidth_Up_Muzzle { get; set; }
        public double ActualHeight_Up_Muzzle { get; set; }
        public double degrees_for_bullet { get; set; }
        public void Rotate_muzzle(Point locate_mouse, Tank my_tank,Canvas canvas,string side)
        {
            MouseX = locate_mouse.X;
            MouseY = locate_mouse.Y;
            TankX = my_tank.PositionX + (my_tank.Width / 2);
            TankY = my_tank.PositionY + (my_tank.Height / 2);
            Under_muzzle = my_tank.Under_Muzzle_tank;
            UP_muzzle = my_tank.UP_Muzzle_tank;
            double sinValue;
            double degrees;
            double bonus_degrees = 0.0d;
            double cat_sin;
            double cat_2;
            double hypotenuse;
            Point CenterXY = my_tank.CenterUnderMuzzle;
            double CenterX = CenterXY.X + my_tank.Under_Muzzle_tank.ActualWidth / 2;
            double CenterY = CenterXY.Y + my_tank.Under_Muzzle_tank.ActualHeight / 2;
            Point CenterUpMuzzle = my_tank.CenterUpMuzzle;
            double CenterUpX = CenterUpMuzzle.X + my_tank.UP_Muzzle_tank.ActualWidth / 2;
            double CenterUpY = CenterUpMuzzle.Y + my_tank.UP_Muzzle_tank.ActualHeight / 2;
            double offsetY = Math.Abs(CenterY - CenterUpY);
            double offsetX = Math.Abs(CenterX - CenterUpX);
            Under_muzzle.RenderTransformOrigin = new Point(0.5 , 0.5);
            UP_muzzle.RenderTransformOrigin = new Point(0.5, offsetY/ActualHeight_Up_Muzzle);
            // TO_TOP_RIGHT_PART;
            if(side == "UP")
            {
                bonus_degrees = 0.0;
            }
            else if(side == "RIGHT")
            {
                bonus_degrees = -90.0;
            }
            else if(side == "DOWN")
            {
                bonus_degrees = -180.0;
            }
            else if(side == "LEFT")
            {
                bonus_degrees = 90.0;
            }
            if(MouseY < TankY && MouseX > TankX)
            {
                cat_sin = MouseX - TankX;
                cat_2 = TankY - MouseY;
                hypotenuse = Math.Sqrt(Math.Pow(cat_sin, 2) + Math.Pow(cat_2, 2));
                sinValue = cat_sin/ hypotenuse;
                degrees = Math.Asin(sinValue) * 180 / Math.PI;
                degrees_for_bullet = degrees;
                RotateTransform rotate = new RotateTransform(degrees + bonus_degrees);
                Under_muzzle.RenderTransform = rotate;
                UP_muzzle.RenderTransform = rotate;
            }
            else if(MouseY > TankY && MouseX > TankX)
            {
                cat_sin = MouseY - TankY;
                cat_2 = MouseX - TankX;
                hypotenuse = Math.Sqrt(Math.Pow(cat_sin, 2) + Math.Pow(cat_2, 2));
                sinValue = cat_sin / hypotenuse;
                degrees = Math.Asin(sinValue) * 180 / Math.PI;
                degrees_for_bullet = degrees + 90;
                RotateTransform rotate = new RotateTransform(degrees + 90 + bonus_degrees);
                Under_muzzle.RenderTransform = rotate;
                UP_muzzle.RenderTransform = rotate;
            }
            else if(MouseY > TankY && MouseX < TankX)
            {
                cat_sin = MouseY - TankY;
                cat_2 = TankX - MouseX;
                hypotenuse = Math.Sqrt(Math.Pow(cat_sin, 2) + Math.Pow(cat_2, 2));
                sinValue = cat_sin / hypotenuse;
                degrees = Math.Asin(sinValue) * 180 / Math.PI;
                degrees_for_bullet = -degrees - 90;
                RotateTransform rotate = new RotateTransform(-degrees - 90 + bonus_degrees);
                Under_muzzle.RenderTransform = rotate;
                UP_muzzle.RenderTransform = rotate;
            }
            else if(MouseY < TankY && MouseX < TankX)
            {
                cat_sin = TankX - MouseX;
                cat_2 = TankY - MouseY;
                hypotenuse = Math.Sqrt(Math.Pow(cat_sin, 2) + Math.Pow(cat_2, 2));
                sinValue = cat_sin / hypotenuse;
                degrees = Math.Asin(sinValue) * 180 / Math.PI;
                degrees_for_bullet = -degrees;
                RotateTransform rotate = new RotateTransform(-degrees + bonus_degrees);
                Under_muzzle.RenderTransform = rotate;
                UP_muzzle.RenderTransform = rotate;
            }
            // TO_TOP_RIGHT_PART;
        }
    }
}
