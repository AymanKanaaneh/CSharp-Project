   M S B A M L   `   `   `  ����J  FFourInRowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FourInRowClientFourInRowClient  5   	        FourInRowClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FourInRowClient     5   	   
���   �  � Title$$  Login | Register - ConnectFour��5	   	    � 
ResizeMode$ NoResize=�60   $	��350��6F   $	��300��6S   .��5      � -   5         �Name$ TheGrid��5       � RowDefinitions 5   
   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   �� 5      $��* �6   6+   .+�5   
   �� 
����  �$��0��6&   $��2��63   $��2��6C   $��2��6W   $��LOGIN��6h   $��10,0,0,0q�6x   $��Center=�6�   $
��Left=�6�   $��20(�6�   $��	UltraBold"�6�   6�   �� 5    
   
����  �$��
User Name:��6&   $��2��6;   $��0��6H   $��2��6X   $��Center=�5!      $��Right=�6-   $��	UltraBold"�6I   6b   �� 5"   
   -   5         ��Name$ 
tbUsername��5"      $��2��6&   $��2��63   $��3��6C   $��5q�6W   6e   �� 5$   
   
����  �$��	password:��6&   $��3��6:   $��0��6G   $��2��6Y   $��Center=�5%      $��Right=�6-   $��	UltraBold"�6I   6b   G� 5&   
   -   5        $��
tbPassword��5&   G   $��2��6   $��3��6&   $��3��63    G� PasswordChar$	 •��6Y   $��Center=�6j   $��5q�6�   �� 5'   
   -   5         ��Name$ btLogin��5'   (   $��	UltraBold"�6   $��Login��69   $��5��6I   $��0��6V   $��2��6f   
����  �6�   �� 5)   
   -   5        $ 
btRigester��5)   (   $��	UltraBold"�6   $��Register��6<   $��5��6O   $��4��6\   $��2��6l   
����  �6�   5,      5.      