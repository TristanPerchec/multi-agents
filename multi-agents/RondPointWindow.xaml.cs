using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace multi_agents
{
    public partial class RondPointWindow : Window
    {
        // ===============================
        // DEBUG MODE CONTROL
        public const bool DEBUGMODE = false;
        // ===============================

        private const int WINDOW_WIDTH = 540;
        private const int WINDOW_HEIGHT = 503;

        // number of cars
        public static int carCount = 0;

        // array sorting
        private int n = 0;

        public static Random rnd = new Random();
        private int randomRoll;

        private World world;

        private Road[] roads;

        public RondPointWindow()
        {
            InitializeComponent();

            Width = WINDOW_WIDTH;
            Height = WINDOW_HEIGHT;

            worldCanvas.Width = WINDOW_WIDTH;
            worldCanvas.Height = WINDOW_HEIGHT;
            worldCanvas.HorizontalAlignment = HorizontalAlignment.Left;
            worldCanvas.VerticalAlignment = VerticalAlignment.Top;
            worldCanvas.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/rondpoint.jpg")));

            Loaded += RondPointWindowLoaded;
        }

        void RondPointWindowLoaded(object _sender, RoutedEventArgs _e) {
            world = new World();
            world.WorldUpdatedEvent += world_WorldUpdatedEvent;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, CarAgent.STEP);
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            world.UpdateEnvironnement();
        }

        void world_WorldUpdatedEvent(CarAgent[] cars, Road[] roads)
        {
            worldCanvas.Children.Clear();

            randomRoll = rnd.Next(1, (CarAgent.SPAWN_RATE + 1));

            if (randomRoll == 1)
            {
                //winning roll : spawning car
                n = 0;

                while (n <= carCount)
                {
                    // finding first position in array
                    if (cars[n] == null)
                    {
                        cars[n] = new CarAgent(n, cars, world);
                        n = carCount;
                    }
                    n++;
                }

                carCount++;
            }

            if (carCount > 0)
            {
                foreach (CarAgent car in cars)
                {
                    if (car != null) {
                        DrawCar(car, cars, roads);
                    }
                }
            }

            worldCanvas.UpdateLayout();

        }

        private void DrawCar(CarAgent paramCar, CarAgent[] cars, Road[] roads)
        {
            if (paramCar.PosX > 540 || paramCar.PosX < 0 || paramCar.PosY < 0 || paramCar.PosY > 503 || paramCar.FlagAbort == true)
            {
                // The car left the world or failed to spawn, RIP.
                cars[paramCar.Id] = null;
                carCount--;
            }
            else
            {
                Rectangle body = new Rectangle();
                ImageBrush image;
                
                if (paramCar.Road == roads[0])
                {
                    //left
                    body.Height = CarAgent.CAR_WIDTH;
                    body.Width = CarAgent.CAR_HEIGHT;
                    image = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/car_left.png")));
                }
                else if (paramCar.Road == roads[1])
                {
                    //top
                    body.Height = CarAgent.CAR_HEIGHT;
                    body.Width = CarAgent.CAR_WIDTH;
                    image = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/car_top.png")));
                }
                else if (paramCar.Road == roads[2])
                {
                    //bottom
                    body.Height = CarAgent.CAR_HEIGHT;
                    body.Width = CarAgent.CAR_WIDTH;
                    image = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/car_bottom.png")));
                }
                else
                {
                    //right
                    body.Height = CarAgent.CAR_WIDTH;
                    body.Width = CarAgent.CAR_HEIGHT;
                    image = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/car_right.png")));
                }

                body.Fill = image;
                body.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform myRotateTransform = new RotateTransform(0);
                body.RenderTransform = myRotateTransform;
                myRotateTransform.Angle = paramCar.angle;

                Canvas.SetTop(body, (paramCar.PosY - CarAgent.HALF_CAR_HEIGHT));
                Canvas.SetLeft(body, (paramCar.PosX - CarAgent.HALF_CAR_WIDTH));
                worldCanvas.Children.Add(body);
            }
        }
    }
}
