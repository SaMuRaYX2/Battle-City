   M S B A M L   `   `   `  ����F  BWpf_Cursova, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Wpf_CursovaWpf_Cursova  5   	       Wpf_Cursova.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Wpf_Cursova     5   	   -   5          �Name$  Window_BattleCity��5   	    � Title$ Battle_city��5   	   $	��900��6   $
��1200��6*    � WindowStartupLocation$ CenterScreen=�5	   	   $	��900��5
   	   $
��1200��6    � Icon$? 9D:\My Homework\Cursova\Texture_image\Icon_Battle_City.png��5   	   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      -   5         ��Name$ zone_row��5      $	��800 �6.   6=   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      -   5         ��Name$ zone_column��5      $	��800 �64   6B   �� 5      6!   .+�5   
   �� -   5         ��Name$ 	play_zone��5      $��1��6$   $��1��61   5       �� 5!   
   $��2��6   $��4q�6!   
���� ��	 �� CornerRadius$	 20��6N   
���   �$��	0,0,0,-20q�6s   ._�5"   
   �� -   5        
 ��Name$
 
pause_game��5"      $��----Pause----��6(   $��30(�6=   $
��Bold"�6L   $��Italic$�6^   
���� �� �� TextAlignment$ Center=�6�   $��Center=�6�   
������ 6�   5#      5$      5&      