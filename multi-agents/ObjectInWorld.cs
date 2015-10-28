﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi_agents
{
    public class ObjectInWorld
    {
        public double PosX;

        public double PosY;

        public ObjectInWorld() { }

        public ObjectInWorld(double _x, double _y)
        {
            PosX = _x;
            PosY = _y;
        }

        public double DistanceTo(ObjectInWorld _object)
        {
            return Math.Sqrt((_object.PosX - PosX) * (_object.PosX - PosX) + (_object.PosY - PosY) * (_object.PosY - PosY));
        }

        public double SquareDistanceTo(ObjectInWorld _object)
        {
            return (_object.PosX - PosX) * (_object.PosX - PosX) + (_object.PosY - PosY) * (_object.PosY - PosY);
        }
    }
}