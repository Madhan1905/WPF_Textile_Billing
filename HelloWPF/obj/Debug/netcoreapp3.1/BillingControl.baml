   M S B A M L   `   `   `  ����E  ATextileApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:HelloWPFHelloWPF  5          HelloWPF.BillingControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      (localclr-namespace:HelloWPF     5      -   5          � 	Resources  5
         ColumnHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value  RowHeaderStyle:  4System.Windows.Controls.Primitives.DataGridRowHeader  	CellStyle*  $System.Windows.Controls.DataGridCell �� Triggers P� Property P� Value  NameColumnStyle  TextColumnStyle��%e  &	        &	 �     &	 �     &	 �    &	 �    �� 5   
   "  6*   .�5      ��  � ��$ 40��6'   �� 5       � ��$ DemiBold"�6+   �� 5       � ��$ 25(�6)   5      �� 5   
   "  6'   .�5      ��  � ��$ 25��6&   5      �� 5   
   "  6"   .�5      ��  � ��$ 22(�6)   �� 5       � ��$ Center=�69    5      P� 5       �@J�	IsFocused$
 True��6/   . �5      ��  � ��
 ��� �63   5      5      �� 5   
   " ��6(   .�5      -   5        5      �� 5   
   " ��6(   .�5       -   5        5!      5"      .��5#      �  � RowDefinitions 5$   
   �� 5%      $��0.15* �6   �� 5&      $��* �6   �� 5'      $��0.05* �6   �� 5(      $��0.25* �6    � ColumnDefinitions 5*   
   �� 5+      $��0.01* �6   �� 5,      $��* �6   �� 5-      $��0.01* �6   .+�5/   
   � $��1��6   	 � UseLayoutRounding$
	 True��6   
 J� SnapsToDevicePixels$

 True��68     50       �� 	Resources��%�   '��       �� 51      " ��6    52      ���� 53      '��       " ��6    .�54      ��  �@��CornerRadius$ 5��6=   55      56      57      58       59      �� 5:      $��0.05* �6    �� 5;      �� 5<      $��0.05* �6     5>      �� 5?      $��* �6#   �� 5@      $
��0.3* �6#   .+�5B      � $��6��6   $��1��6#    5C      �� 5D      $��0.05* �6'   �� 5E      $��0.45* �6'   �� 5F      $��0.45* �6'   �� 5G      $��0.05* �6'    5I      �� 5J      $��0.05* �6$   �� 5K      $
��0.3* �6$   �� 5L      $
��0.1* �6$   �� 5M      $
��0.3* �6$   �� 5N      $��0.05* �6$   .+�5P      �� $��Bill No.��6   $��1��6,   $��1��6<   $��Center=�6I   $
��Bold"�6d   $��15(�6v   �� 5Q      -   5         ��Name$ invoiceTextBox��5Q      $��1��62   $��2��6;   $��1��6K   $��Center=�6X    �� 
IsReadOnly$
 True��6z   $��15(�6�   �� 5R      $
��Date��6   $��1��6(   $��3��68   $��Center=�6E   $
��Bold"�6a   $��15(�6s   �� 5S      -   5        $ dateTextBox��5S      $��2��6/   $��3��6?   $��Center=�6L   $
 True��6n   $��15(�6�   5T      5U      &   System.Windows.Controls.DataGrid  5V   
   -   5        $��	billTable��5V         AutoGenerateColumns$ False��6$   $��1��6@   $��1��5W         ColumnHeaderStyle# [  6#      	CellStyle# [ 6Z      RowHeaderStyle# [ 5X   U      Columns 5[      0  *System.Windows.Controls.DataGridTextColumn  5\      ,  &System.Windows.Controls.DataGridColumn   Header$ Barcode��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.2* 6R   1  +System.Windows.Controls.DataGridBoundColumn   EditingElementStyle# [ 6_      Binding 66   �� *	Barcode+6_     5]      $
 Name��6%   $ 0.35* 6L   # [ 6Z    63   �� *Name+6Z     5^      $
 Qty.��6%   $
 0.1* 6P   # [ 6]    63   �� *
Quantity+6]     5_      $	 MRP��6%   $
 0.1* 6J      
IsReadOnly$
 True��6W    62   �� *MRP+6W     5`      $ Total��6%   $ 0.09* 6N   $
 True��6\    64   �� *Total+6\   5b      � 5c   
   -   5         �Name$ productPopup��5c      $��Center=�6&   $��False��6k    � PlacementTarget 69   ��  �� ElementName$ 	billTable��  5d        myHeaderStyle��%Y   &	       �� 5e      " ��6.   .�5f      ��  � y�$ 	Collapsed=�63   5g      5h      .��5i      � 
�����F� 5j      �� 5k      $��0.05* �6$   �� 5l      $��0.95* �6$   .+�5n      �� $��Select an Item��6   ���#012e7c���#012e7c$��18(�6[   $��DemiBold"�6i   
�������$��Center=�5o   -   $��Center=�6Q   $
 True��6s   �� 5p      -   5         ��Name$ products_list��5p      
�����F�$��1��6I   $��Hidden=�6V   $��20(�6�    �� View 5q      �� 5r      ! �� ColumnHeaderContainerStyle# [ 6#   .P�5s      �� -	   5        barcode_column�� �� Width$	 120��5s   E    �� DisplayMemberBinding 6Q   �� *	Barcode+�� 5t      -
   5        name_column��$ 50��5t   B    6M   �� *Name+�� 5u      -   5        cost_column��$	 150��5u   B    6N   �� *MRP+5v      5x      5y      5z      � 5{   
   $��3��6   $��1��6   
�����F� 5|      �� 5}      $
��0.5* �6    �� 5~      $
��0.5* �6     5�      �� 5�      $
��0.1* �6#   �� 5�      $
��0.2* �6#   �� 5�      $
��0.2* �6#   �� 5�      $
��0.3* �6#   �� 5�      $
��0.2* �6#   .+�5�      � $��1��6   $��Center=�6#    5�      �� 5�      $
��0.5* �6'   �� 5�      $
��0.5* �6'   .+�5�      �� -   5          ��Name$  totalItemsText��5�      $��Total Items:��64   $��20(�6H   5�      � 5�      $��2��6   $��Center=�6#    5�      �� 5�      $
��0.5* �6'   �� 5�      $
��0.5* �6'   .+�5�      �� $��Discount��6   $��20(�6,   �� 5�      -   5        $ discountText��5�   *   $��1��6   $��Center=�5�   <   6~   5�      � 5�      $��3��6   $��2��6#   $��Center=�67    5�      �� 5�      $
��0.5* �6'   �� 5�      $��0.45* �6'   �� 5�      $��0.05* �6'   .+�5�      �� $��Grand Total��6   $��40(�6/   $
��Bold"�6=   
���  ��6O   �� 5�      -   5        $ grandTotalText��5�      $��1��62   $��0��6B   $��Center=�6K   $��40(�6m   $
��Bold"�5�      $
 True��6,   
���  ��6>   5�      � 5�      $��4��6   $��1��6#   $��Center=�60     5�      ��%�   '��       �� 5�      " ��6    5�      ���� 5�      '��       " ��6$   .�5�   "   ��  �@��CornerRadius$ 3��6A   5�      5�      5�      5�       5�      �� 5�      $��0.45* �6'   �� 5�      $��0.05* �6'   �� 5�      $��0.45* �6'   �� 5�      $��0.05* �6'   .+�5�      �� -   5        ! ��Name$! save_button��5�      $��20(�6.   $
��Bold"�6<   .��6g   Save6m   �� 5�      -   5        $��20(�5�      $
��Bold"�6'   $��2��69   .��6b   Exit6h   5�      5�      5�      5�      