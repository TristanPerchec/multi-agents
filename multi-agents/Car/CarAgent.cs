using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi_agents.Car
{
    public class CarAgent : ObjectInWorld
    {
        protected const double STEP = 3;
        protected const double DISTANCE_MIN = 5;
        protected const double SQUARE_DISTANCE_MIN = 25;
        //protected const double DISTANCE_MAX = 40;
        protected const double SQUARE_DISTANCE_MAX = 1600;

        protected double speedX;
        public double SpeedX { get { return speedX; } }

        protected double speedY;
        public double SpeedY { get { return speedY; } }

        internal CarAgent(double _x, double _y, double _dir)
        {
            PosX = _x;
            PosY = _y;
            speedX = Math.Cos(_dir);
            speedY = Math.Sin(_dir);
        }

        internal void UpdatePosition()
        {
            PosX += STEP * SpeedX;
            PosY += STEP * SpeedY;
        }

        private bool Near(CarAgent _car)
        {
            double squareDistance = SquareDistanceTo(_car);
            return squareDistance < SQUARE_DISTANCE_MAX && squareDistance > SQUARE_DISTANCE_MIN;
        }

        internal double DistanceToWall(double _wallXMin, double _wallYMin, double _wallXMax, double _wallYMax)
        {
            double min = double.MaxValue;
            min = Math.Min(min, PosX - _wallXMin);
            min = Math.Min(min, PosY - _wallYMin);
            min = Math.Min(min, _wallYMax - PosY);
            min = Math.Min(min, _wallXMax - PosX);
            return min;
        }

        internal void ComputeAverageDirection(CarAgent[] _carList)
        {
            List<CarAgent> carUsed = _carList.Where(x => Near(x)).ToList();
            int nbCar = carUsed.Count;
            if (nbCar >= 1)
            {
                double speedXTotal = 0;
                double speedYTotal = 0;
                foreach (CarAgent neighbour in carUsed)
                {
                    speedXTotal += neighbour.SpeedX;
                    speedYTotal += neighbour.SpeedY;
                }

                speedX = (speedXTotal / nbCar + speedX) / 2;
                speedY = (speedYTotal / nbCar + speedY) / 2;
                Normalize();
            }
        }

        protected void Normalize()
        {
            double speedLength = Math.Sqrt(SpeedX * SpeedX + SpeedY * SpeedY);
            speedX /= speedLength;
            speedY /= speedLength;
        }

        internal void Update(CarAgent[] _carList, double _max_width, double _max_height)
        {
            double squareDistanceMin = _carList.Where(x => !x.Equals(this)).Min(x => x.SquareDistanceTo(this));

            ComputeAverageDirection(_carList);

            UpdatePosition();
        }
    }
}
