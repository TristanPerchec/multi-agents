using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi_agents
{
    public class RangeCalcable
    {
        public double PosX;
        public double PosY;

        public RangeCalcable()
        {
        }

        public RangeCalcable(double x, double y)
        {
            PosX = x;
            PosY = y;
        }

        public double DistanceTo(RangeCalcable paramObject)
        {
            return Math.Sqrt((paramObject.PosX - PosX) * (paramObject.PosX - PosX) + (paramObject.PosY - PosY) * (paramObject.PosY - PosY));
        }

        public double SqrDistanceTo(RangeCalcable paramObject)
        {
            return (paramObject.PosX - PosX) * (paramObject.PosX - PosX) + (paramObject.PosY - PosY) * (paramObject.PosY - PosY);
        }
    }
}
