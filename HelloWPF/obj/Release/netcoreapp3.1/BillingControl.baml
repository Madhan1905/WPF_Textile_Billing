   M S B A M L   `   `   `  ���� E  ATextileApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:HelloWPFHelloWPF      HelloWPF.BillingControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:HelloWPF     -     � 	Resources     ColumnHeaderStyle �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value  RowHeaderStyle:  4System.Windows.Controls.Primitives.DataGridRowHeader  	CellStyle*  $System.Windows.Controls.DataGridCell �� Triggers P� Property P� Value  NameColumnStyle  TextColumnStyle��%L  &	        &	 U     &	 x     &	 �     &	     �� "  .���  � ��$ 40����  � ��$ DemiBold"���  � ��$ 25(��� "  .���  � ��$ 25���� "  .���  � ��$ 22(���  � ��$ Center=� P�  �@J�	IsFocused$
 True��. ���  � ��
 ��� ��� " ��.�-   �� " ��.�-   .���  � RowDefinitions �� $��0.15* ��� $��* ��� $��0.05* ��� $��0.25* � � ColumnDefinitions �� $��0.01* ��� $��* ��� $��0.01* �.+�� $��1��	 � UseLayoutRounding$
	 True��
 J� SnapsToDevicePixels$

 True��   �� 	Resources��%V   '��       �� " �� ���� '��       " ��.���  �@��CornerRadius$ 5�� �� $��0.05* ��� �� $��0.05* � �� $��* ��� $
��0.3* �.+�� $��6��$��1�� �� $��0.05* ��� $��0.45* ��� $��0.45* ��� $��0.05* � �� $��0.05* ��� $
��0.3* ��� $
��0.1* ��� $
��0.3* ��� $��0.05* �.+��� $��Bill No.��$��1��$��1��$��Center=�$
��Bold"�$��15(��� -    ��Name$ invoiceTextBox��$��1��$��2��$��1��$��Center=� �� 
IsReadOnly$
 True��$��15(��� $
��Date��$��1��$��3��$��Center=�$
��Bold"�$��15(��� -   $ dateTextBox��$��2��$��3��$��Center=�$
 True��$��15(�&   System.Windows.Controls.DataGrid  -   $��	billTable��   AutoGenerateColumns$ False��$��1��$��1��   ColumnHeaderStyle# [     	CellStyle# [    RowHeaderStyle# [    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Barcode��   Width5  /System.Windows.Controls.DataGridLengthConverter$
 0.2* 1  +System.Windows.Controls.DataGridBoundColumn   EditingElementStyle# [    Binding �� *	Barcode+  $
 Name��$ 0.35* # [  �� *Name+  $
 Qty.��$
 0.1* # [  �� *
Quantity+  $	 MRP��$
 0.1*    
IsReadOnly$
 True�� �� *MRP+  $ Total��$ 0.09* $
 True�� �� *Total+� -    �Name$ productPopup��$��Center=�$��False�� � PlacementTarget ��  �� ElementName$ 	billTable��    myHeaderStyle��%4   &	       �� " ��.���  � y�$ 	Collapsed=�.��� 
�����F� �� $��0.05* ��� $��0.95* �.+��� $��Select an Item�����#012e7c���#012e7c$��18(�$��DemiBold"�
�������$��Center=�$��Center=�$
 True���� -    ��Name$ products_list��
�����F�$��1��$��Hidden=�$��20(� �� View �� ! �� ColumnHeaderContainerStyle# [ .P��� -	   barcode_column�� �� Width$	 120�� �� DisplayMemberBinding �� *	Barcode+�� -
   name_column��$ 50�� �� *Name+�� -   cost_column��$	 150�� �� *MRP+� $��3��$��1��
�����F� �� $
��0.5* ��� $
��0.5* � �� $
��0.1* ��� $
��0.2* ��� $
��0.2* ��� $
��0.3* ��� $
��0.2* �.+�� $��1��$��Center=� �� $
��0.5* ��� $
��0.5* �.+��� -     ��Name$  totalItemsText��$��Total Items:��$��20(�� $��2��$��Center=� �� $
��0.5* ��� $
��0.5* �.+��� $��Discount��$��20(��� -   $ discountText��$��1��$��Center=�� $��3��$��2��$��Center=� �� $
��0.5* ��� $��0.45* ��� $��0.05* �.+��� $��Grand Total��$��40(�$
��Bold"�
���  ���� -   $ grandTotalText��$��1��$��0��$��Center=�$��40(�$
��Bold"�$
 True��
���  ��� $��4��$��1��$��Center=�  ��%V   '��       �� " �� ���� '��       " ��.���  �@��CornerRadius$ 3�� �� $��0.45* ��� $��0.05* ��� $��0.45* ��� $��0.05* �.+��� -   ! ��Name$! save_button��$��20(�$
��Bold"�.��Save�� -   $��20(�$
��Bold"�$��2��.��Exit