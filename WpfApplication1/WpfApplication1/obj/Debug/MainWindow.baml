   M S B A M L   `   `   `  ����  WpfApplication1     WpfApplication1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  currentDisplay��5   	    � Title$ 
MainWindow��5   	   $	��600��6   $
��1100��6)   .��5      � $��0,0,-188,-137q�6   .+�5   
   �� -   5         ��Name$ textContent��5      $
��Left=�6'   $	��330��6B   $��166,123,0,0q�6O    �� TextWrapping$
 Wrap=�6d   $"��Select a mode to begin with.��6x   $	��Top=�6�   $	��719��6�    �� 	IsEnabled . 6�   ]� 5   
   $��33��6   ��.  $��Bottom=�66    J� RenderTransformOrigin$ 0.507,-0.293&�6Q   $��	0,0,0,286q�6v   o� 5	   
   $
��Left=�6   $��22��6*   $	��Top=�66   $	��517��6N   .@�5
      m� -   5         m�Name$ fileMenu��5
      $
��File��6)   .@�5      m� -   5        $��Load Text File��5       � ToolTip$ Loads a single text file.��64   6X   m� 5      -   5        $��Load Assignment��5      $I CLoads an assignment file at the end file will be saved for viewing.��6I   m� 5      -   5        $��	Load Test��5      $F @Loads a test that is timed, upon timer completion file is saved.��6/   6z   m� 5      -   5        $��
Begin Test��5      $M GBegins the current test, then allows the user to enter their response. ��60   6�   m� 5      -   5        $��Begin Assignment��5      $F @Begins the timer, then allows the user to enter their response. ��66   6�   5      m� 5      $
��Edit��6   .@�5      m� -	   5        $��Quit Current Test��5      $&  Stop the current testing session��67   6b   5      m� 5      $
��Help��6   5      �� 5   
   $��Currently Working On:��6   $
��Left=�60   $��
166,30,0,0q�6K   $	��Top=�6_   �� 5   
   -
   5         ��Name$ currentProject��5      $��Nothing��6(   $
��Left=�6:   $��
300,30,0,0q�6U   $	��Top=�6i   $	��148��6�   � 5   
   $
��Left=�6   $��19,61,0,107q�6/   $	��121��6D   .+�5      �� 
���   �$��2q�6)   ._�5      �� -   5        $
 mode��5      $��Mode: Not Selected��6&   $	��121��6C   5      �� 5      
���   �$��2q�6)   ._�5       �� -   5        $ 	sentences��5       $�� ��6+   $	��121��66   5!      �� 5"      
���   �$��2q�6)   ._�5#      �� -   5        $ words��5#      $�� ��6'   $	��121��62   5$      �� 5&      
���   �$��2q�6)   ._�5'      �� -   5        $ timeRemaining��5'      $�� ��6/   $	��121��6:   5(      5)      �� 5+   
   -   5        $
 muID��5+      $
��Left=�6    $��26��6;   $��
300,61,0,0q�6G   $
 Wrap=�6[   $��Enter MUID (901-xxx-xxx) ��6o   $	��Top=�6�   $	��148��6�   �� 5,   
   $��Marshall ID:��6   $
��Left=�6'   $��
166,61,0,0q�6B   $	��Top=�6V   �� 5-   
   $��Name:��6   $
��Left=�6    $��
166,92,0,0q�6;   $	��Top=�6O   �� 5.   
   -   5        $ studentName��5.      $
��Left=�6'   $��19��6B   $��
300,96,0,0q�6N   $
 Wrap=�6b   $��First and Last Name��6v   $	��Top=�6�   $	��148��6�   52      53      