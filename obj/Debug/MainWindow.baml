   M S B A M L   `   `   `  ����  NuSMVANDNuXMV    NuXMVRunner.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  NuXMV Runner��5   	   $	��400��6   $	��600��6+   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5      �� $��NuXMV File Path:��6   $��Center=�6/   �� 5      -   5         ��Name$ FilePathTextBox��5      $	��400��6/   $	��5,0q�6;   $��Center=�6H   �� 5      -   5         ��Name$ BrowseButton��5      $��Browse��6+   $��80��6<   6G   5      �� 5   
   -   5        $ ApplyButton��5      $��1��6&   $��Apply��63   $	��100��6C   $
��Left=�6O   $��0,0,0,10q�6j   6|   �� 5   
   -   5        $ 
LogTextBox��5      $��2��6&    �� 
IsReadOnly . " �� VerticalScrollBarVisibility$
 Auto=�6E   $ �� HorizontalScrollBarVisibility$
 Auto=�6h    �� AcceptsReturn . 	 �� 
AcceptsTab	 . 6�   5      5      