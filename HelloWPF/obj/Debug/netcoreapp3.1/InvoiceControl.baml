   M S B A M L   `   `   `  ����E  ATextileApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:HelloWPFHelloWPF  5          HelloWPF.InvoiceControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (localclr-namespace:HelloWPF     5      .��5	      �   � RowDefinitions  5
   
   �� 5      $��0.15* �6   �� 5      $��* �6   �� 5      $��0.05* �6   �� 5      $��0.15* �6    � ColumnDefinitions 5   
   �� 5      $��0.01* �6   �� 5      $��* �6   �� 5      $��0.01* �6   .+�5   
   � $��1��6    � UseLayoutRounding$
 True��6    J� SnapsToDevicePixels$
 True��68    � 	Resources 5       �� 
TargetType �� 	Resources �� Property �� Value��%�   '��       �� 5      " ��6    5      ���� 5      '��       " ��6    .�5      ��  �@��CornerRadius$ 5��6=   5      5      5      5        5      �� 5       $��0.05* �6    �� 5!      �� 5"      $��0.05* �6     5$      �� 5%      $
��0.1* �6#   �� 5&      $��0.01* �6#   �� 5'      $
��0.1* �6#   �� 5(      $��0.01* �6#   �� 5)      $
��0.1* �6#   �� 5*      $��* �6#   �� 5+      $
��0.3* �6#   .+�5-      �� -   5        $��1��5-      $��0��6"   
�������.��5.      �   5/      �� 50      �� 51      .+�53      �� $��/Images/view_invoice.png��6   �� 54      $��1��6    $��	View Bill��6-   $��12(�6>   55      56      �� 57      -   5        $��1��57      $��2��6"   
�������.��58      �   59      �� 5:      �� 5;      .+�5=      �� $��/Images/print.png��6   �� 5>      $��1��6    $��
Print Bill��6-   $��12(�6?   5?      5@      � 5A      $��1��6   $��6��6    $��Bottom=�60    5B      :  4System.Windows.Controls.Primitives.DatePickerTextBox��%_   '        �� 5C      "  6   .�5D      ��  �@��
IsReadOnly$
 True��67   5E      5F      .+�5G      (  "System.Windows.Controls.DatePicker  -   5        	  Name$	 dateBox��5G      6.   5H      5I      &   System.Windows.Controls.DataGrid  5J   
   -   5        $��invoiceTable��5J      
   AutoGenerateColumns$
 False��6'      CanUserAddRows$ False��6C   $��1��6Z   $��1��5K         CanUserDeleteRows$ False��6E      Columns 5L      0  *System.Windows.Controls.DataGridTextColumn  5M      ,  &System.Windows.Controls.DataGridColumn   Header$ Bill No.��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 0.25* 6R      
IsReadOnly$
 True��6`   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Number+6`     5N      $ 	Bill Date��6%   $ 0.25* 6Q   $
 True��6_    68   �� *Date+6_     5O      $ 	Bill Time��6%   $ 0.25* 6Q   $
 True��6_    68   �� *Time+6_     5P      $ Total Amount��6%   $ 0.25* 6U   $
 True��6c    6;   �� *Total+6c   5R      5S      5T      