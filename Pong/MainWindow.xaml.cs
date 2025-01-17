﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TimeSpan = System.TimeSpan;

namespace Pong
{
    public partial class MainWindow : Window
    {
        //Strony w które rusza się gracz
        private bool P1Up = false;
        private bool P1Down = false;
        private bool P2Up = false;
        private bool P2Down = false;
        
        //timery
        private DispatcherTimer PlayerMovementTimer = new DispatcherTimer();
        private DispatcherTimer AnimationTimer = new DispatcherTimer();

        //prędkość kuli
        private double BeginnSpeedX = 140;
        private double BeginnSpeedY = 220;
        private double SpeedX = 140;
        private double SpeedY = 220;

        //liczniki punktów graczy
        private int PlayerLeftCount = 0;
        private int PlayerRightCount = 0;

        //pozycja startowa kuli
        private double BeginnBallX = 200;
        private double BeginnBallY = 150;

        //opuźnienie żeby kula nie odbiła się dwa razy od tej samej paletki
        private bool LeftCooldown = false;
        private bool RightCooldown = false;

        public MainWindow()
        {
            InitializeComponent();
            //rozpoczęcie timera dla ruchu gracza
            PlayerMovementTimer.Interval = TimeSpan.FromMilliseconds(30);
            PlayerMovementTimer.Tick += MovePlayer;
            PlayerMovementTimer.Start();

            //rozpoczęcie timeru dla animacji
            AnimationTimer.Interval = TimeSpan.FromSeconds(0.05);
            AnimationTimer.Tick += Animate;
            AnimationTimer.Start(); 
        }

        private void Animate(object sender, EventArgs e)
        {
            var x = Canvas.GetLeft(Ball);
            var y = Canvas.GetTop(Ball);

            //kolizje kuli z lewą paletką
            if (x <= LeftPaddle.Width 
                && y >= Canvas.GetTop(LeftPaddle) 
                && y + Ball.Height <= Canvas.GetTop(LeftPaddle) + LeftPaddle.Height
                && !LeftCooldown)
            {
                SpeedX = -SpeedX;
                LeftCooldown = true;
                RightCooldown = false;
            }
            //kolizje kuli z prawą paletką
            if (x + Ball.Width >= Gamefield.ActualWidth - (Canvas.GetRight(RightPaddle) + RightPaddle.Width)
                && y + RightPaddle.Width >= Canvas.GetTop(RightPaddle) 
                && y + Ball.Height <= Canvas.GetTop(RightPaddle) + RightPaddle.Height
                && !RightCooldown)
            {
                SpeedX = -SpeedX;
                RightCooldown = true;
                LeftCooldown = false;
            }

            //kolizje kuli z ścianami
            if (y <= 0.0 || y >= Gamefield.ActualHeight - Ball.Height)
            {
                SpeedY = -SpeedY;
            }

            //punkty dla prawego gracza
            if (x <= -10 - Ball.Width)
            {
                SpeedX = BeginnSpeedX;
                SpeedY = BeginnSpeedY;
                PlayerRightCount++;
                LeftCooldown = false;
                RightCooldown = false;
                Canvas.SetLeft(Ball, BeginnBallX);
                Canvas.SetTop(Ball, BeginnSpeedY);
                RightCount.Content = PlayerRightCount.ToString();
                return;
            }

            //punkty dla lewego gracza
            if (x >= 10 + Gamefield.ActualWidth)
            {
                SpeedX = BeginnSpeedX;
                SpeedY = BeginnSpeedY;
                PlayerLeftCount++;
                LeftCooldown = false;
                RightCooldown = false;
                Canvas.SetLeft(Ball, BeginnBallX);
                Canvas.SetTop(Ball, BeginnSpeedY);
                LeftCount.Content = PlayerLeftCount.ToString();
                return;
            }

            //przyśpieszenie kuli
            SpeedX *= 1.001;
            SpeedY *= 1.001;

            //ruch kuli
            x += SpeedX * AnimationTimer.Interval.TotalSeconds;
            y += SpeedY * AnimationTimer.Interval.TotalSeconds;
            Canvas.SetLeft(Ball, x);
            Canvas.SetTop(Ball, y);
        }

        //ruch graczy
        private void MovePlayer (object sender, EventArgs e)
        {
            if (P1Down)
            {
                if (Canvas.GetTop(LeftPaddle) + LeftPaddle.Height < Gamefield.ActualHeight)
                {
                    Canvas.SetTop(LeftPaddle, Canvas.GetTop(LeftPaddle) + 10);
                }
            }
            else if (P1Up)
            {
                if (Canvas.GetTop(LeftPaddle) > 0.0)
                {
                    Canvas.SetTop(LeftPaddle, Canvas.GetTop(LeftPaddle) - 10);
                }
                
            }
            if (P2Down)
            {
                if (Canvas.GetTop(RightPaddle) + 100 < Gamefield.ActualHeight)
                {
                    Canvas.SetTop(RightPaddle, Canvas.GetTop(RightPaddle) + 10);
                }
            }
            else if (P2Up)
            {
                if (Canvas.GetTop(RightPaddle) > 0.0)
                {
                    Canvas.SetTop(RightPaddle, Canvas.GetTop(RightPaddle) - 10);
                }
            }
        }

        #region Player Movement Events

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                {
                    P1Up = true;
                    P1Down = false;
                    break;
                }

                case Key.S:
                {
                    P1Up = false;
                    P1Down = true;
                    break;
                }

                case Key.Up:
                {
                    P2Up = true;
                    P2Down = false;
                    break;
                }

                case Key.Down:
                {
                    P2Up = false;
                    P2Down = true;
                    break;
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                {
                    P1Up = false;
                    P1Down = false;
                    break;
                }

                case Key.S:
                {
                    P1Up = false;
                    P1Down = false;
                    break;
                }

                case Key.Up:
                {
                    P2Up = false;
                    P2Down = false;
                    break;
                }

                case Key.Down:
                {
                    P2Up = false;
                    P2Down = false;
                    break;
                }
            }
        }

        #endregion
    }
}
