   M S B A M L   `   `   `  ����   RCFilterApp    RCFilterApp.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Passive Low Pass Filters��$	��650��$	��900��.��� 
������� � ColumnDefinitions �� $	��250 ��� $��* �.+��� $��0��
���  ��$��10q�.+��� $��Resistance (Ohms)��
�������$
��Bold"��� -   $��txtResistance��
�������$��0,2,0,10q��� $��Capacitance (Farads)��
�������$
��Bold"��� -   $��txtCapacitance��
�������$��0,2,0,10q��� $��
Vin (Volt)��
�������$
��Bold"��� -   $��txtVin��
�������$��0,2,0,10q��� $��Frequency (Hz)��
�������$
��Bold"��� -   $��txtFrequency��
�������$��0,2,0,10q��� -   $��btnCalculate��$��	Calculate��
���   �
�������$
��Bold"�$��30��$	��100��$��Center=�$��	0,10,0,10q��� 
������� �� Padding$ 10q� �� CornerRadius$ 5��$��0,5,0,0q�._��� .+��� -   $��lblVout��$��Vout = ��
�������$��14(��� -   $��	lblCutOff��$��
Cut-off = ��
�������$��14(��� -   $��lblPhaseShift��$��Phase shift = ��
�������$��14(��� $	��261��$��/Ludwig_von_Draak.png��$
��Fill=�$	��240���� $��1��
�������$��10q�.+��� 
���  ��$ 5q�$ 5��$��0,5,0,5q�._��� $��Gain vs Frequency��$��16(�$
��Bold"�$��Center=�
��������� $	��250��
�������
���   �$��1q�$��10q�._��� -	   $��imgBodePlot��$
��Fill=��� 
���  ��$ 5q�$ 5��$��0,5,0,5q�._��� $��Phase vs Frequency��$��16(�$
��Bold"�$��Center=�
��������� $	��250��
�������
���   �$��1q�$��10q�._��� -
   $��imgPhasePlot��$
��Fill=�