   M S B A M L   `   `   `  ����J  FFourInRowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FourInRowClientFourInRowClient  5   	   !    FourInRowClient.EntryWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FourInRowClient     5   	   -   5          � Title$  ConnectFour��5      $	��500��6   $	��900��6)    � 
ResizeMode$ NoResize=�6N   ��5	      �� 5
   
    �� ImageSource$$ images/fourInrowBackground.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5       $��* �6   .+�5"   
   �� -   5        $��lblName��5"   �   $��Welcome to four in row��6   $��0��61   $��1��6>   $��5��6N   $��Center=�6b   $��Center=�6   
����  �$��30(�6�   $
��Bold"�6�   $��14,0,0,0q�6�   �� 5#   
   $��5��6   $��1��6    $��Center=�6.   $��Center=�6I   $��Players Online:��6f   $��	ExtraBold"�6�   $��16(�6�   $��5q�6�   $��5��6�   
����  �6�   �� 5%   
   -   5        $��lbUsers��5%   i   $��14(�6   
���   �
�������$��1��6K   $��8��6X   $��
51,50,0,30q�6y   $��5��6�    �� SelectionMode$ Single=�6�   $��2��6�   $
��Left=�6�   $	��197��6  �� 5'   
   -   5         ��Name$ 	Searchbtn��5'   L   $��17(�6   
����  �
�������$��to search window��6`   $��3��6{   $��0��6�   $��2��6�   $	��150��6�   6�   5(      5)      