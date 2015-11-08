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

namespace multi_agents
{
    public class CarAgent : RangeCalcable
    {
        public const int CAR_HEIGHT = 54;
        public const int CAR_WIDTH = 26;
        public const int HALF_CAR_HEIGHT = CAR_HEIGHT / 2;
        public const int HALF_CAR_WIDTH = CAR_WIDTH / 2;
        public static int SPAWN_RATE = 20;
        public static int STEP = 20;
        public static int STEPS_PER_SECOND = 1000 / STEP;

        // Attributes - Leave public if requested very often to positively impact performances.
        private Road road;
        private World world;
        private int id;
        private double speedX;
        private double speedY;
        private bool flagAbort;
        private int speedMult;
        private double targetX;
        private double targetY;
        public double angle;

        public Road Road
        {
            get { return road; }
            set { road = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public double SpeedX
        {
            get { return speedX; }
            set { speedX = value; }
        }
        public double SpeedY
        {
            get { return speedY; }
            set { speedY = value; }
        }
        public bool FlagAbort
        {
            get { return flagAbort; }
            set { flagAbort = value; }
        }
        public int SpeedMult
        {
            get { return speedMult; }
            set { speedMult = value; }
        }
        public double TargetX
        {
            get { return targetX; }
            set { targetX = value; }
        }
        public double TargetY
        {
            get { return targetY; }
            set { targetY = value; }
        }

        // Constructor
        public CarAgent(int paramId, CarAgent[] cars, World paramWorld)
        {
            // unique ID attribution
            // Well... unique for as long as you only look at existing cars.
            id = paramId;
            flagAbort = false;
            world = paramWorld;

            // Roll of the speed mult.
            int rollSpeedMult = RondPointWindow.rnd.Next(0, 7);
            SpeedMult = 1 - ((rollSpeedMult - 3) / 100); // = 0.97 0.98 0.99 1 1.01 1.02 1.03

            int nbRoad = 0;
            nbRoad = RondPointWindow.rnd.Next(0, 4);
            
            road = world.roads[nbRoad];
            PosX = road.PosX;
            PosY = road.PosY;
            targetY = PosY;
            targetX = PosX;

            // First Range Calculation for spawn-eligibility on the selected road.
            bool spawnable = false;
            bool pass;

            if (RondPointWindow.carCount > 0)
            {
                while (spawnable == false)
                {
                    pass = true;

                    foreach (CarAgent car in cars)
                    {
                        if (car != null)
                        {
                            if (car.Road == road)
                            {
                                if (DistanceTo(car) < 70)
                                {
                                    pass = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (pass == true)
                    {
                        spawnable = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (spawnable == false)
                {
                    flagAbort = true;
                }
            }

            switch (road.Id)
            {
                case 1:
                    //left
                    speedX = Road.MaxSpeedRoad;
                    speedY = 0;
                    break;
                case 2:
                    //top
                    speedX = 0;
                    speedY = Road.MaxSpeedRoad;
                    break;
                case 3:
                    //bottom
                    speedX = 0;
                    speedY = -Road.MaxSpeedRoad;
                    break;
                default:
                    //right
                    speedX = -Road.MaxSpeedRoad;
                    speedY = 0;
                    break;
            }
            

            updateSpeed(paramWorld.roads);
        }

        public void updateSpeed(Road[] roads)
        {
            if (road.getRoadZone(PosX, PosY) == 2)
            {
                int direction;
                int directionMov;

                if (targetX != PosX)
                {
                    if (targetX == roads[0].PosX)
                    {
                        //origin left target bottom
                        direction = 1;
                        directionMov = (speedX >= 0 ? 1 : -1);
                    }
                    else if (targetX == roads[1].PosX)
                    {
                        //origin top target left
                        direction = -1;
                        directionMov = (speedX >= 0 ? 1 : -1);
                    }
                    else if (targetX == roads[2].PosX)
                    {
                        //origin bottom target right
                        direction = 1;
                        directionMov = (speedX >= 0 ? 1 : -1);
                    }
                    else
                    {
                        //origin right target top
                        direction = -1;
                        directionMov = (speedX >= 0 ? 1 : -1);
                    }

                    double differenceAbs = Math.Abs(targetX - PosX);

                    /*if (differenceAbs > 30)
                    {
                        // Lots of Y distance yet
                        speedY += (speedX * direction) / 50;*/
                    speedX = (road.MaxSpeedRoad * direction);
                    /*}
                    else
                    {
                        // Not a lot of Y distance
                        double targetYSpeed = (targetY - PosY) * 1.5;

                        if (Math.Abs(speedY) > Math.Abs(targetYSpeed))
                        {
                            speedY += ((-Road.MaxSpeedRoad * directionMov) / 50);
                        }

                        if (Math.Abs(speedY) < Math.Abs(targetYSpeed))
                        {
                            speedY = targetYSpeed;
                        }
                        else if (Math.Abs(speedY) < Math.Abs(targetYSpeed))
                        {
                            speedY += ((Road.MaxSpeedRoad * direction) / 50);
                        }
                    }*/

                    /*if (Math.Abs(speedY) > (speedX * 0.5))
                    {
                        speedY = (speedX * 0.5 * direction);
                    }*/

                    /*if (Math.Abs(targetY - PosY) < 3)
                    {
                        speedY = 0;
                    }*/
                }

                if (targetY != PosY)
                {
                    if (targetY == roads[0].PosX)
                    {
                        //origin left target bottom
                        direction = 1;
                        directionMov = (speedY >= 0 ? 1 : -1);
                    }
                    else if (targetY == roads[1].PosX)
                    {
                        //origin top target left
                        direction = 1;
                        directionMov = (speedY >= 0 ? 1 : -1);
                    }
                    else if (targetY == roads[2].PosX)
                    {
                        //origin bottom target right
                        direction = -1;
                        directionMov = (speedY >= 0 ? 1 : -1);
                    }
                    else
                    {
                        //origin right target top
                        direction = -1;
                        directionMov = (speedY >= 0 ? 1 : -1);
                    }

                    double differenceAbs = Math.Abs(targetX - PosY);

                    /*if (differenceAbs > 30)
                    {
                        // Lots of Y distance yet
                        speedY += (speedX * direction) / 50;*/
                    speedY = (road.MaxSpeedRoad * direction);
                }
            }
        }

        public void updateTarget(CarAgent car)
        {
            int currentZone = road.getRoadZone(PosX, PosY);
            Road roadDestination;
            switch (currentZone)
            {
                //in middle zone
                case (2):
                    if (car.speedX > 0)
                    {
                        //left
                        roadDestination = world.roads[0];
                        TargetX = roadDestination.PosX;
                        TargetY = roadDestination.PosY;
                    }
                    else if (car.speedY > 0)
                    {
                        //top
                        roadDestination = world.roads[1];
                        TargetX = roadDestination.PosX;
                        TargetY = roadDestination.PosY;
                    }
                    else if (car.speedY < 0)
                    {
                        //bottom
                        roadDestination = world.roads[2];
                        TargetX = roadDestination.PosX;
                        TargetY = roadDestination.PosY;
                    }
                    else
                    {
                        //right
                        roadDestination = world.roads[3];
                        TargetX = roadDestination.PosX;
                        TargetY = roadDestination.PosY;
                    }
                    /*if (car.speedX > 0)
                    {
                        //left
                        road = world.roads[1];
                        TargetX = road.PosX;
                        TargetY = road.PosY;
                    }
                    else if (car.speedY > 0)
                    {
                        //top
                        road = world.roads[3];
                        TargetX = road.PosX;
                        TargetY = road.PosY;
                    }
                    else if (car.speedY < 0)
                    {
                        //bottom
                        road = world.roads[0];
                        TargetX = road.PosX;
                        TargetY = road.PosY;
                    }
                    else
                    {
                        //right
                        road = world.roads[2];
                        TargetX = road.PosX;
                        TargetY = road.PosY;
                    }*/
                    break;
                default:
                    break;
            }
        }

        internal void updatePosition()
        {
            // Apply Speed Multiplier
            speedX = speedX * SpeedMult;
            speedY = speedY * SpeedMult;

            PosX += speedX / STEPS_PER_SECOND;
            PosY += speedY / STEPS_PER_SECOND;
        }

        public void updateAngle()
        {
            /*double oldAngle = angle;
            if (speedX > 10)
            {
                angle = (Math.Atan(speedY / speedX) * 180) / Math.PI;
            }

            if (PosX > Road.ZONE_PEAGE_START && PosX < Road.ZONE_GUICHET_START + Road.ZONE_GUICHET_LENGTH && isBraking == 1 && speedX < 40)
            {
                angle = angle / 1.25;
            }*/
        }
        
        internal void Update(CarAgent car, CarAgent[] cars, Road[] roads)
        {
 	        updateTarget(car);
            updateAngle();
            updateSpeed(roads);
            updatePosition();
        }
    }
}
