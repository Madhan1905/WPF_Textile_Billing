   M S B A M L   `   `   `  ���� E  ATextileApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TextileApp
TextileApp      TextileApp.ItemsControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     *localclr-namespace:TextileApp     .���   � RowDefinitions  �� $��0.15* ��� $��0.75* ��� $��0.10* �.+�� $��0��$��1�� � ColumnDefinitions �� $��0.01* ��� $��0.075* ��� $��0.583* ��� $��0.10* ��� $��0.25* ��� $��0.10* ��� $��0.25* ��� $��0.01* �  �� $��0.75* ��� $��0.25* �.+��� $��Filter By: ��$��3��$��1��$��Right=�$��Center=�$��15(��� -    ��Name$ filter_combo��$��4��$��1��$��Center=�.@���  �� 
IsSelected$
 True��.��	Barcode�� .��Name�� .��
Print Name�� .��Less Than Stock�� $��Search: ��$��5��$��1��$��Right=�$��Center=�$��15(��� -    ��Name$ search_text��$��6��$��1��$��Center=�,� $��1��$��1�� ,� IsToolBarVisible$ False��.��/� -    /�Name$ itemsDoc��.T��� -    ��Name$ 
itemsTable�� �� BorderBrush
 �   �	 �� BorderThickness$	 1q�
 �� CellSpacing$
 0�� �� Columns ��  �� Width$ 0.05* ��� $ 0.05* ��� $ 0.05* ��� $ 0.15* ��� $
 0.3* ��� $
 0.2* ��� $
 0.1* ��� $
 0.1* ��� $
 0.1* �.��� .��� 
�����F�.���  �� BorderBrush
 �   � �� BorderThickness$ 0,0,1,0q� �� Padding$ 2q�.�J� $
��14pt(�$
��Bold"� �� TextAlignment$ Center=�.*�Edit�� 
 �   �$ 0,0,1,0q�$ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�Delete�� 
 �   �$ 0,0,1,0q�$ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�S.No.�� 
 �   �$ 0,0,1,0q�$ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�	Barcode�� 
 �   �$ 0,0,1,0q�$ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�	Item Name�� 
 �   �$ 0,0,1,0q�$ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�
Print Name�� 
 �   �$ 0,0,1,0q�$ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�Rate�� $ 2q�.�J� $
��14pt(�$
��Bold"�$ Center=�.*�Stock� -    �Name$ navigation_grid��$��2��$��Center=�$��Center=�.+�]� $��Center=�.+��� -   $��5,0,5,0q�$��25��$��/Images/previous.png���� -   $��5,0,5,0q�$��25��$��/Images/next.png��