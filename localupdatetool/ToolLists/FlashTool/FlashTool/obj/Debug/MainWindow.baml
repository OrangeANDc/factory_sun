   M S B A M L   `   `   `  ����D  @FlashTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:FlashTool	FlashTool  5   	       FlashTool.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.0.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   #localclr-namespace:FlashTool  5   	     � Title$  刷写工具 V1.0.0��5	   	   $	��450��6   $	��800��6*    � 
ResizeMode$ CanMinimize=�66   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��7* �6   �� 5      $
��2.3* �6    � RowDefinitions 5   
   �� 5      $��3* �6   �� 5      $��7* �6   .+�5   
   �  5      �� 5      $
��2.5* �6#   �� 5      $��7* �6#    5      �� 5      $��7* �6    �� 5      $��3* �6    .+�5      �� -   5        $��选择刷写文件��5      $��Center=�6@   $��Center=�6]   $��18(�6x   �� 5      -   5         ��Name$ 	tbProcess��5      $��1��6,   $��Center=�6:   $��Center=�6W   $��18(�6s   �� 5      $��1��6   $��5q�6)   .+�5      �� -   5         ��Name$ OpenFilePath��5      $��5q�60   $��70��6;   $��Center=�6G    �� AcceptsReturn . {�.   !  MaterialDesignOutlinedTextBox#��[  5        �� TextWrapping$
 Wrap=�6T   " �� VerticalScrollBarVisibility$
 Auto=�6h   5!      � 5"      -   5        	 �Name$	 pbValue��5"      $��12��6+   $��1��67   $��1��6D   $��Center=�6T   $��10q�6o   5#      � 5$   
   $��1��6   $��2��6    5%      �� 5&      �� 5'      �� 5(      �� 5)      �� 5*      �� 5+      �� 5,      �� 5-      �� 5.      .+�50      �� -   5        
 ��Name$
 cbPort��50      $��Center=�6(   $��Center=�6E   $	��100��6`   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	端口号��51      $��16(�6D    & "MaterialDesignFloatingHintComboBox#��[ 6R   �� 52      -   5        $
 
cbBaudRate��52      $��Center=�6,   $��Center=�6I   $	��100��6d   $��1��6p   $ 	波特率��53      $��16(�6D   #��[ 6R   .@�54      �� $
��9600��6   �� 55      $��115200��6   56      �� 57      -   5         ��Name$ btnOpen��57      $��打开串口��6?   $��2��6N   $��Center=�6[   $��Center=�6v   $��18(�6�   �� 58      -   5        $ btnClose��58      $��关闭串口��6A   $��3��6P   $��Center=�6]   $��Center=�6x   $��18(�6�   �� 59      -	   5        $ btnWrite��59      $��开始写入��6@   $��5��6O   $��Center=�6\   $��Center=�6w   $��18(�6�   �� 5:      -
   5        $��6��5:   ,   $��清除日志��69   $��Center=�6H   $��Center=�6c   $��18(�6�   5<      �� 5>   
   $	��Log��6   $��1��6      	NGroupBox#��[ 6-   $��5q�6P   .��5?      &   System.Windows.Controls.DataGrid  -   5          Name$	 gd1��5?         CanUserAddRows .     GridLinesVisibility$
 None=�6=   $��0q�6X      
IsReadOnly .    RowStyle 5@      �� 5A       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6   .�5B      ��  �� Property � �� �� Value$ 31��63   �� 5C       � ��$ 10(�65   5D      5F      5G      5I      5J      