   M S B A M L   `   `   `  ����J  FFourInRowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FourInRowClientFourInRowClient  5   	   "    FourInRowClient.SearchWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FourInRowClient     5   	     � Title$  SearchWindow��5	   	   $	��380��6   $	��480��6+    � 
ResizeMode$ NoResize=�67   .��5      ]� .+�5   
   o� 
���   �
�������.@�5      m� $��search��6   .@�5      m� -   5        
���   �$��
games list��5   .   6X   m� 5      -   5        
���   �$��ongoing games��5   .   6c   m� 5      -   5        
���   �$��users��5   .   6S   5      m� 5      $
��sort��6   .@�5      m� -   5        
���   �$��sort window��5   .   6X   5      5      � 5   
   
���   � � RowDefinitions 5      �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-   �� 5      $��* �6    6-    � ColumnDefinitions 5!      �� 5"      $��* �6#   6/   �� 5#      $��* �6#   6/   �� 5$      $��* �6#   6/   �� 5%      $��* �6#   6/   �� 5&      $��* �6#   6/   �� 5'      $��* �6#   6/   �� 5(      $��* �6#   6/   �� 5)      $��* �6#   6/   .+�5,      �� 
����  �
�������$��1��6C   $��7��6P   $/��)to see games go to search then games list��6d   6�   �� 5-      
����  �
�������$��2��6C   $��7��6P   $:��4to see ongoing games go to search then ongoing games��6d   6�   �� 5.      
����  �
�������$��3��6C   $��7��6P   $8��2to see games between users go to search then users��6d   6�   �� 5/      
����  �
�������$��4��6C   $��7��6P   $4��.to see sorted data go to sort then sort window��6d   6�   50      52      54      