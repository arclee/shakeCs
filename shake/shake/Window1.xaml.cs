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
namespace shake
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        Rectangle m_ActorRect = new Rectangle();
        Vector m_lastMousePos = new Vector(0, 0);
        bool mousesample = false;
        double scale = 1;
        int lasttimestamp = -1;
        //總移動量ABS.
        int totalmoveX = 0;
        int totalmoveY = 0;

        //上次.
        int lasttotalmoveX = 0;
        int lasttotalmoveY = 0;

        private Stopwatch m_stopWatch;

        //振幅.
        Rectangle m_ActorRect2 = new Rectangle();
        private Stopwatch m_stopWatch2;
        CFreevibration mCFreevibration = new CFreevibration();
        CFreevibrationwithdamping mCFreevibrationwithdamping = new CFreevibrationwithdamping();

        public Window1()
        {
            InitializeComponent();
            this.MouseMove += MouseMoveHandler;


            CompositionTarget.Rendering += _TimerClock_Tick;

            m_ActorRect.Fill = Brushes.Red;
            m_ActorRect.Stroke = Brushes.Red;
            m_ActorRect.StrokeThickness = 2;
            m_ActorRect.Width = 40;
            m_ActorRect.Height = 40;
            Thickness margin = new Thickness();
            margin.Top = 0;
            margin.Left = 0;
            m_ActorRect.Margin = margin;

            Panel.SetZIndex(m_ActorRect, 1);
            DrawCanvas.Children.Add(m_ActorRect);

            m_ActorRect2.Fill = Brushes.Red;
            m_ActorRect2.Stroke = Brushes.Red;
            m_ActorRect2.StrokeThickness = 2;
            m_ActorRect2.Width = 40;
            m_ActorRect2.Height = 40;
            Thickness margin2 = new Thickness();
            margin2.Top = 0;
            margin2.Left = 0;
            m_ActorRect2.Margin = margin2;
            Panel.SetZIndex(m_ActorRect2, 2);
            DrawCanvas.Children.Add(m_ActorRect2);

            //Canvas.SetLeft(m_ActorRect, DrawCanvas.Width*0.5);
            //Canvas.SetTop(m_ActorRect, DrawCanvas.Height * 0.5);
            Canvas.SetLeft(m_ActorRect, 0);
            Canvas.SetTop(m_ActorRect, 0);

            Canvas.SetLeft(m_ActorRect2, 50);
            Canvas.SetTop(m_ActorRect2, 50);
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
           // if (!mousesample)
            {
               // return;
            }
            // Get the x and y coordinates of the mouse pointer.
            System.Windows.Point position = e.GetPosition(this);

            //System.Text.StringBuilder sb = new StringBuilder();
            double dx = (position.X - m_lastMousePos.X) * scale;
            double dy = (position.Y - m_lastMousePos.Y) * scale;

            totalmoveX += (int)Math.Abs(dx);
            totalmoveY += (int)Math.Abs(dx);

            double ax = Canvas.GetLeft(m_ActorRect);
            double ay = Canvas.GetTop(m_ActorRect);

            m_lastMousePos.X = position.X;
            m_lastMousePos.Y = position.Y;

            Canvas.SetLeft(m_ActorRect, ax + dx);
            Canvas.SetTop(m_ActorRect, ay + dy);

            ax = Canvas.GetLeft(m_ActorRect);
            ay = Canvas.GetTop(m_ActorRect);
            if (ax <= 0)
            {
                Canvas.SetLeft(m_ActorRect, 0);
            }
            if (ay <= 0)
            {
                Canvas.SetTop(m_ActorRect, 0);
            }

            //mouseText.Text = (e.Timestamp - lasttimestamp).ToString() + " -- " + position.X.ToString() + " , " + position.Y.ToString() + "  " + dx.ToString() + " , " + dy.ToString();
            mouseText.Text = totalmoveX.ToString();


            lasttimestamp = e.Timestamp;
        }


        void _TimerClock_Tick(object sender, EventArgs e)
        {
            if (m_stopWatch == null)
            {
                m_stopWatch = Stopwatch.StartNew();
            }

            mousesample = false;
            long nElapsedMilliseconds = m_stopWatch.ElapsedMilliseconds;
            float elapseSec = (float)nElapsedMilliseconds / 1000.0f;
            if (nElapsedMilliseconds >= 100)
            {
                m_stopWatch.Reset();
                m_stopWatch.Start();
                mousesample = true;



                Console.WriteLine((totalmoveX - lasttotalmoveX).ToString());

                lasttotalmoveX = totalmoveX;
            }

            //CFreevibration
            if (m_stopWatch2 == null)
            {
                m_stopWatch2 = Stopwatch.StartNew();
                m_stopWatch.Start();
            }
            nElapsedMilliseconds = m_stopWatch2.ElapsedMilliseconds;
            elapseSec = (float)nElapsedMilliseconds / 1000.0f;

            mCFreevibration.update(elapseSec);
            mCFreevibrationwithdamping.update(elapseSec);
            double vx = mCFreevibrationwithdamping.x;
            Console.WriteLine(vx.ToString());

            Canvas.SetLeft(m_ActorRect2, vx + 100);
            Canvas.SetTop(m_ActorRect2, 200);
            
        }
    }
}
