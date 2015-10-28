using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi_agents.Car
{
    public delegate void RoadUpdated(CarAgent[] _car);

    public class Road
    {
        public event RoadUpdated roadUpdatedEvent;

        CarAgent[] carList = null;

        Random randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Road(int _carNb, double _width, double _height)
        {
            MAX_WIDTH = _width;
            MAX_HEIGHT = _height;
            randomGenerator = new Random();

            carList = new CarAgent[_carNb];

            for (int i = 0; i < _carNb; i++)
            {
                carList[i] = new CarAgent(randomGenerator.NextDouble() * MAX_WIDTH, randomGenerator.NextDouble() * MAX_HEIGHT, randomGenerator.NextDouble() * 2 * Math.PI);
            }
        }

        public void UpdateEnvironnement()
        {
            UpdateCar();
            if (roadUpdatedEvent != null)
            {
                roadUpdatedEvent(carList);
            }
        }

        private void UpdateCar()
        {
            foreach (CarAgent car in carList)
            {
                car.Update(carList, MAX_WIDTH, MAX_HEIGHT);
            }
        }
    }
}
