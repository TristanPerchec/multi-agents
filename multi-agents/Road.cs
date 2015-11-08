using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multi_agents
{
    public class Road
    {
        // position in pixels from the left of the picture
        public const int LEFT_ZONE = 0;     
        public const int MIDDLE_HORIZONTAL_ZONE = 174;
        public const int RIGHT_ZONE = 364;

        // position in pixels from the top of the picture
        public const int TOP_ZONE = 0;
        public const int MIDDLE_VERTICAL_ZONE = 146;
        public const int BOTTOM_ZONE = 325;

        private int maxSpeedRoad;
        private double posX;
        private double posY;
        private int id;

        public Road(double x, double y, int _id)
        {
            maxSpeedRoad = 100;
            posX = x;
            posY = y;
            id = _id;
        }

        public int MaxSpeedRoad
        {
            get { return maxSpeedRoad; }
            set { maxSpeedRoad = value; }
        }

        public double PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public double PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /*
         * 1 : leftZone
         * 2 : middleZone
         * 3 : topZone
         * 4 : bottomZone
         * 5 : rightZone
         */
        public int getRoadZone(double posX, double posY)
        {
            if (posX < MIDDLE_HORIZONTAL_ZONE)
            {
                return 1;
            }
            else if (posX < RIGHT_ZONE && posX > MIDDLE_HORIZONTAL_ZONE && posY > MIDDLE_VERTICAL_ZONE && posY < BOTTOM_ZONE)
            {
                return 2;
            }
            else if (posY < MIDDLE_VERTICAL_ZONE)
            {
                return 3;
            }
            else if (posY > BOTTOM_ZONE)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }
    }
}
