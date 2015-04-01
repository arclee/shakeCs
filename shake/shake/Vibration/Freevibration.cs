using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.ComponentModel;

public class CFreevibration
{
    public double x = 0; //位置.
    double m = 0.1; //質量.
    double k = 1;   //彈簧常數.
    double Amp = 100; //振幅.
    double phase = 0;  //初始相位. 0~360.

    public void update(double t)
    {
        //簡諧振動.
        //mx"+kx=0
        //ω=√k/m.
        //角速度.
        double w = Math.Sqrt(k / m);
        x = Amp * Math.Sin(w*t + phase);
    }

};