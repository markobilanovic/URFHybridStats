   M S B A M L   `   `   `  ����S  OLoL_URF_HybridStats_vs12, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:LoL_URF_HybridStats_vs12.UI_Controls$LoL_URF_HybridStats_vs12.UI_Controls  5      Q-clr-namespace:LoL_URF_HybridStats_vs12.ChartsLoL_URF_HybridStats_vs12.Charts  5      D @WpfCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:WpfCharts;assembly=WpfCharts	WpfCharts 5      9    3LoL_URF_HybridStats_vs12.UI_Controls.MainWindowPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      I
UIControls2clr-namespace:LoL_URF_HybridStats_vs12.UI_Controls     5      @charts-clr-namespace:LoL_URF_HybridStats_vs12.Charts     5      @	WpfCharts*clr-namespace:WpfCharts;assembly=WpfCharts     5      -   5        |�.   � 	Resources  5       �� Source�� 5   
   $! ..\Styles\BlackCrystal.xaml?�6   %    ��5      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ Black��6    � Offset$ 0Q�6)   � 5      $ 	#FFBB2A2A��6   $ 1Q�6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $	��350 �6   �� 5      $
��Auto �6   �� 5      $	��270 �6   .+�5    
   � $��0��6    � ColumnDefinitions 5!      �� 5"      $��* �6#   �� 5#      $��* �6#   �� 5$      $��* �6#   .+�5&      �� $��5q�6`   $	��100��6k   $
��Left=�6w   
��� � �$
��Bold"�6�   $��16(�6�   ��6   �� *	Regions+ �� SelectedIndex 67   �� *SelectedRegion+6�   �� 5'      -   5        $��btnGlobalStats��5'   �   $��1��6   $��Global Stats��6%   $��5q�6=   
��� � �$
��Bold"�6[   6�   5)      � 5+   
   $��1��6   .+�5,      :   4LoL_URF_HybridStats_vs12.UI_Controls.CarouselControl  -   5        	  Name$	 carouselControl��5,   :   :   4LoL_URF_HybridStats_vs12.UI_Controls.CarouselControl
   OnElementSelected$'
 !carouselControl_OnElementSelected��5-   G      OnElementShowWindow$) #carouselControl_OnElementShowWindow��5.   &   $��1��5,   )   $	��900��6S   $��Stretch=�6_   $��Stretch=�6}    J� RenderTransformOrigin$ 0.5,0.5&�5-   &   5.   &   5/      � 52   
   $��2��6   $��5q�6    53      �� 54      $��* �6#   �� 55      $��* �6#   �� 56      $��* �6#   .+�58      �� -   5        $��btnSelectPrevious��58   8   $��0��6   $��Previous��6%   
��� � �$
��Bold"�6�   |�.  6�   �� 59      -   5        $��tbSearch��59   �   $��1��6   $��Search��6&   $��18(�64   $��Center=�6B   $��Center=�6d   
��� � �$
��Bold"�6�   $	��220��6�   6  �� 5:      -   5        $��btnSelectNext��5:   4   $��2��6   $
��Next��6%   
��� � �$
��Bold"�6y   |�.  6�   5;      � 5>   
   $��3��6   $��5q�6    5?      �� 5@      $	��250 �6#   �� 5A      $��* �6#   �� 5B      $	��250 �6#   .+�5E      � $��0��6   $��21,0,0,0q�6#    5F      �� 5G      $
��Auto �6$   �� 5H      $��* �6$    5J      �� 5K      �� 5L      .+�5N      �� $��Average��6   
�������6*   �� 5O      
��� � �$��3��6U   $��Right=�6e   ��6   �� *SelectedChampion.Name+6e     WpfCharts.SpiderChart  5Q      $��2��6(   $��1��6<      Minimum$ 0Q�5R          Maximum$	 100Q�6,      Ticks$ 0��6:      
ShowLegend .  
������    Lines 5Q   J   �� *SpiderLeftLines+   Axis 6l   �� *SpiderLeftAxes+5R   W   5S      � 5U      $��1��6   $
��40,0q�6#    5V      �� 5W      $
��Auto �6$   �� 5X      $��* �6$   .+�5Z      �� $	��250��6m   ��6   �� *GraphCriterium+ �� SelectedItem 6B   �� *SelectedCriterium+6m   �� 5[      $��1��6!   ��6.   �� *SelectedChart+5\      � 5_      $��2��6   $��0,0,21,0q�6#    5`      �� 5a      $
��Auto �6$   �� 5b      $��* �6$    5d      �� 5e      �� 5f      .+�5h      �� $��Average��6   
�������6*   �� 5i      
��� � �$��3��6U   $��Right=�6e   ��6   �� *SelectedChampion.Name+6e     5k      $��1��6)   $��2��66   $ 0Q�5l       $	 100Q�6,   $ 0��6:    .  
������  5k   J   �� *SpiderRightLines+ 6m   �� *SpiderRightAxes+5l   W   5m      5q      5r      5s      