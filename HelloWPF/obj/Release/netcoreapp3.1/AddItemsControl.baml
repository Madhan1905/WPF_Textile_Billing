   M S B A M L   `   `   `  ���� E  ATextileApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TextileApp
TextileApp       TextileApp.AddItemsControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     *localclr-namespace:TextileApp     -   .���   � RowDefinitions  �� $��0.05* ��� $��* ��� $��0.15* ��� $��0.02* �.+��� -    ��Name$ 
error_text��$"��*Please Fill Required Fields�� �� TextAlignment$ Center=�
���  ��$y�	Collapsed=�� $��1�� � 	Resources  �� 
TargetType �� 	Resources �� Property �� Value��%V   '��       �� " �� ���� '��       " ��.���  �@��CornerRadius$ 3�� � ColumnDefinitions �� $��0.05* ��� $
��0.3* ��� $
��0.7* ��� $��0.05* �  �� �� �� �� �� �� .+�� $��1��$��0��$��Center=�$
��Left=�.+��� $��15(�
Barcode:� $��1��$��1��$��Center=�$
��Left=�.+��� $��15(�Name:� $��1��$��2��$��Center=�$
��Left=�.+��� $��15(�Print Name:� $��1��$��3��$��Center=�$
��Left=�.+��� $��15(�Rate:� $��1��$��4��$��Center=�$
��Left=�.+��� $��15(�Stock:� $��2��$��0��.+��� -   	 ��Name$	 barcode��$��1��$��30��$��15(�$��Regular"�$��Center=�� $��2��$��1��$��Center=�.+��� -   $
	 name��$��1��$��30��$��15(�$��Regular"�$��Center=�� $��2��$��2��$��Center=�.+��� -   $	 	printName��$��1��$��30��$��15(�$��Regular"�$��Center=�� $��2��$��3��$��Center=�.+��� -   $		 mrp��$��1��$��30��$��15(�$��Regular"�$��Center=�� $��2��$��4��$��Center=�.+��� -   $	 stock��$��1��$��30��$��15(�$��Regular"�$��Center=�� -   
 �Name$
 
bottomGrid��$��2�� ��%V   '��       �� " �� ���� '��       " ��.���  �@��CornerRadius$ 4�� �� $
��0.4* ��� $
��0.2* ��� $
��0.4* �  �� $
��0.2* ��� $
��0.6* ��� $
��0.2* �.+�� -	    �Name$ createProgress��$��1��$��1�� � Minimum$ 0Q� � Maximum$	 100Q�$y�	Collapsed=�$��20�� � IsIndeterminate$
 True���� -
    ��Name$ submitButton��$��1��$��1��.��Submit