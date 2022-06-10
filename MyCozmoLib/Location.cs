using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyCozmoLib.Dynamics
{
    /// <summary>
    /// Class to handle location point X,Y
    /// </summary>
    public class Location
    {
        public static int X_LIMIT = 4;
        public static int Y_LIMIT = 4;

        /// <summary>
        /// Represents X coordinate of the location
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Represents Y coordinate of the location
        /// </summary>
        public int Y { get; private set; }

        public Location(int x = -1, int y = -1)
        {
            this.X = x;
            this.Y = y;
        }

        public static Location operator+ (Location a, Direction d)
        {
            var p = new Location(a.X + d.X, a.Y + d.Y);

            p.Update();

            return p;
        }

        public bool IsValid => this.X >= 0 && this.X <= X_LIMIT && this.Y >= 0 && this.Y <= Y_LIMIT;

        public  void Update()
        {
            if (this.X < 0)
            {
                this.X = 0;
            }

            if (this.Y < 0)
            {
                this.Y = 0;
            }

            if (this.X > X_LIMIT)
            {
                this.X = X_LIMIT;
            }

            if (this.Y > Y_LIMIT)
            {
                this.Y = Y_LIMIT;
            }
        }
    }
}
