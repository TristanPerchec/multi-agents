using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi_agents
{
    public delegate void WorldUpdated(CarAgent[] listCars, Road[] roads);

    public class World
    {
        public event WorldUpdated WorldUpdatedEvent;

        //number of cars
        public CarAgent[] cars = new CarAgent[30];
        public Road[] roads = new Road[4];

        public World()
        {
            //left
            roads[0] = new Road(0, 280, 1);
            //top
            roads[1] = new Road(238, 0, 2);
            //bottom
            roads[2] = new Road(300, 503, 3);
            //right
            roads[3] = new Road(540, 220, 4);
        }

        public void UpdateEnvironnement()
        {
            if (RondPointWindow.carCount > 0)
            {
                foreach (CarAgent car in cars)
                {
                    if (car != null)
                    {
                        car.Update(car, cars, roads);
                    }
                }
            }

            if (WorldUpdatedEvent != null)
            {
                WorldUpdatedEvent(cars, roads);
            }
        }
    }
}
