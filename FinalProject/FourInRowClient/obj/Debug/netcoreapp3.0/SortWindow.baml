   M S B A M L   `   `   `  ����J  FFourInRowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FourInRowClientFourInRowClient  5   	        FourInRowClient.SortWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FourInRowClient     5   	   
���   �  � Title$  
SortWindow��5	   	   $	��450��6   $
��1000��6)    � 
ResizeMode$ NoResize=�66   .��5
      � -   5         �Name$ TheGrid��5
       � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5       $��* �6   6+   �� 5!      $��* �6   6+   �� 5"      $��* �6   6+   �� 5#      $��* �6   6+   .+�5%   
   �� 
����  �$��	Lets Sort��6&   $��Center=�6:   $��Center=�6W   $��5��6r   $��3��6�   $��24(�5&      6!   �� 5(   
   -   5         ��Name$ lbusers��5(   M   $��14(�6   
�������
����  �$��1��6^   $��1��6k   $��6��6{   $��7��6�   6�   �� 5*   
   -   5         ��Name$ btn_usernameSort��5*   >   
�������
����  �$��sort by username��6X   $
��Left=�6s   $��8��6�   $��2��6�   $��2��5+      6G   �� 5,   
   -   5        $ btn_gamesnumberSort��5,   >   
�������
����  �$��sort by number of games��6[   $��Center=�6}   $��9��6�   $��4��5-      $��2��6   6V   �� 5.   
   -   5        $ btn_winnsSort��5.   >   
�������
����  �$��8��6U   $��6��6e   $��sort by winns��5/      $
��Left=�6)   6F   �� 50   
   -   5        $ btn_lose_Sort��50   >   
�������
����  �$��11��6U   $��2��6f   $��sort by loses��51      $
��Left=�6)   6F   �� 52   
   -   5        $ btn_points_Sort��52   >   
�������
����  �$��11��6W   $��6��6h   $��sort by points��53      $
��Left=�6*   6G   55      56      