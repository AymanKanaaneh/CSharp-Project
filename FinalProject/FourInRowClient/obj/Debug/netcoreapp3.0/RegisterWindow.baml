   M S B A M L   `   `   `  ����J  FFourInRowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FourInRowClientFourInRowClient  5   	   $    FourInRowClient.RegisterWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FourInRowClient     5   	   
���   �  � Title$  RegisterWindow��5	   	   $	��300��6    $	��300��6-   .��5      � -   5         �Name$ TheGrid��5       � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5    
   �� 
����  �$��Register��6&   $��2��69   $��2��6I   $��2��6]   $��Center=�5!      $��Center=�6-   $��22(�6H   6X   �� 5"   
   
����  �$��	UltraBold"�6&   $��	username:��6=   $��Right=�6Q   $��Center=�6m   $��0��6�   $��3��6�   $��2��6�   6�   �� 5#   
   
����  �$��	UltraBold"�6&   $��	password:��6=   $��Right=�6Q   $��Center=�6m   $��0��6�   $��4��6�   $��2��6�   6�   �� 5%   
   -   5         ��Name$ 
tbUserName��5%      $��3��6&   $��2��63   $��5q�6C   $��3��6N   6e   G� 5&   
   -   5        $��
tbPassword��5&   X   $��2��6   $��4��6&   $��1��63   $��3��6D    G� PasswordChar$	 •��6j   $��Center=�6{   $��5q�6�   �� 5(   
   -   5         ��Name$ 
btRigester��5(   >   
����  �$��	UltraBold"�6'   $��Rigester��6R   $��4��6f   $��7��6v   $��2��6�   6�   5)      5*      