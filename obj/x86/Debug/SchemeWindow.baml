   M S B A M L   `   `   `  ����G  CMillitaryApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MillitaryAppMillitaryApp  5   	       MillitaryApp.SchemeWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:MillitaryApp     5   	   -   5          � Title$A  ;Просмотр схемы аппаратуры связи��5   	   
������� � WindowStyle$
 None=�5	   	    � 
ResizeMode$ NoResize=�6    � Icon$ Icon.ico��62    � Topmost .  � WindowStartupLocation$ CenterScreen=�5
   	    � WindowState$ 	Maximized=�6.   $
��1920��6F   $
��1080��6S    � 	Resources 5       �� MergedDictionaries	 �� Source   Style.Button.Base
 �� 
TargetType  Main.Style.Button �� BasedOn �� Property �� Value  Style.Button  Style.Animation.Click �� 	Resources  Key �� TargetProperty g� RepeatBehavior �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value �� Triggers :� RoutedEvent  OnLoaded  Description.ModeTitle   Description.ModeTitle.Screen  Description.StepDescription &	 "Description.StepDescription.Screen 
 Description.Title  Description.Title.Screen  Description.Panel  Description.Panel.Screen�� 5   
    5      �� 5      $.	 (/MillitaryApp;component/Styles/Main.xaml?�6%   %I  &	        7  &	 �     7   &	 �     7  &	 �    &	 �    &	 %    &		 �    &	
     &	     &	 �    &	 `    �� 5      "
 ��6.   8   .�5      ��  � ��$ 22(�6-   �� 5       � ��$	 132��6*   �� 5       �@��Padding$ 0,3,0,5q�6,   5      �� 5      "
 ��6)   8   .�5      ��  � ��$ 16,0,0,0q�6+   5      �� 5      "
 ��62    5      ���� 5       OnLoaded .��5!      U� $ (UIElement.Opacity)�68   $ Forever��6h    6�   Z� .\�5"        $ 0��63   $ 1Q�6?     5#      $ 0:0:1��63   $ 0Q�6C     5$      $ 0:0:2��63   $ 1Q�6C   5%      5&      5'       5(      :� 5)      $ FrameworkElement.Loaded��6#   .[�5*      �� 8��  6*   5+      5-      �� 50      "
 ��62   .�51      ��  � ��$ 24(�6-   52      �� 53      "
 ��69   .�54      ��  � ��$ 34(�6-   55      �� 57      "
 ��68   .�58      ��  � ��$ 	0,98,0,70q�6+   �� 59       � ��$ 20(�6-   5:      �� 5;      "
 ��6?   .�5<      ��  � ��$ 
0,118,0,70q�6+   �� 5=       � ��$ 30(�6-   5>      �� 5@      "
 ��6.   .�5A      ��  � ��$ 22(�6-   �� 5B       � ��$ 	30,60,0,0q�6+   5C      �� 5D      "
 ��65   .�5E      ��  � ��$ 32(�6-   �� 5F       � ��$ 	30,70,0,0q�6+   5G      �� 5I      "
 �6.   .�5J      ��  � ��$	 850��6*   �� 5K       � ��$	 470��6+   5L      �� 5M      "
 �65   .�5N      ��  � ��$
 1000��6*   �� 5O       � ��$	 600��6+   5P      5Q      .��5S      �  � RowDefinitions 5T   
   �� 5U      .+�5W   
    � -   5          �Name$ SchemeFrame��5W      $��60,80,60,90q�6%     � NavigationUIVisibility$ Hidden=�6:   �� 5X   
   -   5         ��Name$ NextStepMessage��5X      
������  � Cursor$
 Hand��6C   $y�Hidden=�6n    � 5Y   
   -   5        $ SchemeElementsFrame��5Y      $ Hidden=�6`   ��6-   �� *Margin+ �� ElementName$ SchemeFrame��6`   �� 5Z   
   -   5        
���   � J� Opacity$	 0.5Q�5Z   e   $
 Hand��6�   y�6!   �� *
Visibility+$ StepDescriptionPanel��6�   �� 5[   
   -   5         ��Name$ 
MenuButton��5[      #��[ 6(   $��Меню��6N   $��Bottom=�6Z   $��	50,0,0,30q�6u   $
��Left=�6�   �� 5\   
   $��Right=�6   $��Bottom=�61    �� Orientation$ 
Horizontal=�6L   $��	0,0,50,30q�6e   .+�5]      �� -   5        $ PauseButton��5]      #��[ 6-   $��
Пауза��6S   6`   �� 5^      -   5        $ DescriptionButton��5^      #��[ 63   $%��Описание + Повтор��6Y   $��236.167��6�   $��37.108��6�   �� 5_      -	   5        $ BackStepButton��5_      #��[ 60   $��
Назад��6V   6c   �� 5`      -
   5        $ NextStepButton��5`      #��[ 60   $��Вперед��6V   6d   5a      �� 5b   
   -   5        $ AppTitle��5b      $
��Wrap=�6&   $	��Top=�6:   $��28(�6R   $��220,20,30,0q�6`   $
��1800��6u   $��65��6�    �� TextAlignment$ Right=�6�   $
��Bold"�6�   $6��0Название аппаратуры связи��6�   �� 5c   
   -   5        $ 	StepSteps��5c      $	��Top=�6'   $ Center=�6?   $��Шаг 1 из 12��6V   $
��Left=�6i     J� IsHitTestVisible  .  $��	30,20,0,0q�6�   $
��Bold"�6�   $��28(�6�   � 5d   
   -   5        ! �Name$! StepDescriptionPanel��5d      
�������$
 Hand��6@   #��[ 6p   $y�Hidden=�6�   .+�5e      �� 
���   �$��2q�6)   $��-2q�6=     .  6I   �� 5f      -   5        $ 	ModeTitle��5f      $#��Название режима��6+   $
��Bold"�6B   $
��Left=�6T   $	��Top=�6o   $ Center=�6�   #��[ 6�     .  $��	30,19,0,0q�6�   �� 5g      -   5        $ StepDescription��5g      $
��Wrap=�61   $!��Описание этапа��6E   " �� Padding$
" 30,0q�6[   #��[ 6j     .  $ Justify=�6�   �� 5h      -   5        $ DescriptionTitle��5h      $	��Top=�62   $��Описание:��6J   $
��Left=�6[   $
��Bold"�6v   #��[
 6�     .  6�   5i      5j      5k      