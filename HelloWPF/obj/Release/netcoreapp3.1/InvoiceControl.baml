   M S B A M L   `   `   `  ���� E  ATextileApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:HelloWPFHelloWPF      HelloWPF.InvoiceControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:HelloWPF     .���   � RowDefinitions  �� $��0.15* ��� $��* ��� $��0.05* ��� $��0.15* � � ColumnDefinitions �� $��0.01* ��� $��* ��� $��0.01* �.+�� $��1�� � UseLayoutRounding$
 True�� J� SnapsToDevicePixels$
 True�� � 	Resources  �� 
TargetType �� 	Resources �� Property �� Value��%V   '��       �� " �� ���� '��       " ��.���  �@��CornerRadius$ 5��  �� $��0.05* ��� �� $��0.05* � �� $
��0.1* ��� $��0.01* ��� $
��0.1* ��� $��0.01* ��� $
��0.1* ��� $��* ��� $
��0.3* �.+��� -   $��1��$��0��
�������.���   �� �� .+��� $��/Images/view_invoice.png���� $��1��$��	View Bill��$��12(��� -   $��1��$��2��
�������.���   �� �� .+��� $��/Images/print.png���� $��1��$��
Print Bill��$��12(�� $��1��$��6��$��Bottom=� :  4System.Windows.Controls.Primitives.DatePickerTextBox��%:   '        �� "  .���  �@��
IsReadOnly$
 True��.+�(  "System.Windows.Controls.DatePicker  -   	  Name$	 dateBox��&   System.Windows.Controls.DataGrid  -   $��invoiceTable��
   AutoGenerateColumns$
 False��   CanUserAddRows$ False��$��1��$��1��   CanUserDeleteRows$ False��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Bill No.��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 0.25*    
IsReadOnly$
 True��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Number+  $ 	Bill Date��$ 0.25* $
 True�� �� *Date+  $ 	Bill Time��$ 0.25* $
 True�� �� *Time+  $ Total Amount��$ 0.25* $
 True�� �� *Total+