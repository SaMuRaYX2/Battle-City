   M S B A M L   `   `   `  ����F  BWpf_Cursova, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Wpf_CursovaWpf_Cursova  5   	       Wpf_Cursova.PauseGame   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Wpf_Cursova     5   	     � Title$  	PauseGame��5   	   $	��500��6   $	��600��6(   $	��500��5	   	   $	��500��6   $	��600��5
   	   $	��600��6    � WindowStyle$
 None=�5   	    � ShowInTaskbar .  � WindowStartupLocation$ CenterScreen=�5   	    � Icon$? 9D:\My Homework\Cursova\Texture_image\Icon_Battle_City.png��5   	   .��5      � -   5         �Name$ grid_background��5       � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      6!   �� 5      6!   �� 5      6!   �� 5       6!   �� 5!      6!   �� 5"      6!   �� 5#      6!   .+�5&   
   �� -   5         ��Name$ background_gif��5&      $
��Fill=�6(   $��8��67   $��7��6H   	 K� ZIndex$	 -2��6\   5'      �� 5(   
   -   5        
 ��Name$
 textblock_pause_game��5(      $��1��62   $��2��6?   $��3��6O   $��
PAUSE GAME��6c    �� TextAlignment$ Center=�6u   $��Center=�6�   $��24(�6�   $��	ExtraBold"�6�   
���iii�6�   �� 5)   
   $��2��6   $��5��6   $��1��62   $��2��6B   $��2q�6S   ���#000000���#1a0600 J� Opacity$	 0.6Q�6�    �� CornerRadius$ 	40,0,40,0��6�   $��60,15,60,15q�6�   6�   �� 5*   
   -   5        $
 continue_game��5*      $��2��6+   $��5��68   $��1��6L   $��2��6\   $��Continue game��6m   $ Center=�6�   $��Center=�6�   $��25(�6�   $��Italic$�6�   $��	ExtraBold"�6�   ���#4d9900$ 1Q�6  $��	60,0,60,0q�6  6"  �� 5+   
   $��4��6   $��5��6   $��1��62   $��2��6B   $��2q�6S   ���#000000���#1a0600$	 0.6Q�6�   $ 	0,40,0,40��6�   $��80,15,80,15q�6�   6�   �� 5,   
   -   5        $
 	exit_menu��5,      $��4��6'   $��5��64   $��1��6H   $��2��6X   $
��EXIT��6i   $ Center=�6u   $��Center=�6�   $��25(�6�   $��Italic$�6�   $��	ExtraBold"�6�   ���#980000$ 1Q�6�   $��	80,0,80,0q�6   6  5.      50      