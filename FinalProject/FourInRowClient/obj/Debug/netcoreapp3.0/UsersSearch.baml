   M S B A M L   `   `   `  ����J  FFourInRowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FourInRowClientFourInRowClient  5   	   !    FourInRowClient.UsersSearch   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FourInRowClient     5   	   
���   �  � Title$  UsersSearch��5	   	   $	��450��6   $	��800��6*    � 
ResizeMode$ NoResize=�66   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5   
   �� 
����  �$��Users Search��6&   $��2��6=   $��3��6M   $��35(�6a   $��Center=�5       $��Center=�6-   6J   �� 5!   
   
����  �$��games between��6&   $��1��6>   $��0��6K   $��Center=�6[   $��Center=�6x   $��2��5"      $��27(�6$   64   �� 5#   
   
����  �$��users��6&   $��Center=�66   $��Center=�6T   $��2��6o   $��1��6   $��2��5$      $��30(�6$   64   �� 5%   
   
����  �$��games��6&   $��Center=�66   $��Center=�6S   $��4��6o   $��1��6   $��3��5&      $��28(�6$   64   �� 5(   
   -   5         ��Name$ lbUsersSearch��5(   M   $��14(�6   
�������
����  �$��2��6d   $��2��6q   $��2��6�   $��5��6�    �� SelectionMode$ Single=�5)   D   $��5q�6[   6h   �� 5*   
   -   5        $ lbShow��5*   M   $��14(�6   
�������
����  �$��2��6]   $��0��6j   $��2��6z   $��3��6�   $��5q�6�   6�   �� 5+   
   -   5        $ lbgames��5+   M   $��14(�6   
�������
����  �$��2��6^   $��4��6k   $��3��6{   $��5��6�   $��5q�6�   6�   �� 5-   
   -   5         ��Name$ 
btn_search��5-   >   
�������
����  �$��search��6R   $��6��6c   $��1��6p   $��5q�6�   6�   5.      50      