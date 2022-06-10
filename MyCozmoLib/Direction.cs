using System;

namespace MyCozmoLib.Dynamics
{
    public enum DirectionFlag
    {
        NORTH = 1,
        EAST,
        SOUTH,
        WEST,
        UNKNOWN
    }

    /// <summary>
    /// Class to handle direction vector. NORTH is UP, EAST is RIGHT
    /// </summary>
    public class Direction
    {
        public DirectionFlag Flag { get; private set; }

        /// <summary>
        /// Represents X coordinate of the direction vector
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Represents Y coordinate of the direction vector
        /// </summary>
        public int Y { get; private set; }

        public static DirectionFlag GetDirection(string direction)
        {
            return (DirectionFlag)Enum.Parse(typeof(DirectionFlag), direction);
        }

        public static bool TryGetDirection(string direction, out DirectionFlag flag)
        {
            try
            {
                flag = (DirectionFlag)Enum.Parse(typeof(DirectionFlag), direction);
            }
            catch 
            {
                flag = DirectionFlag.UNKNOWN;
                return false;

            }

            return true;
        }
        public string GetFlagString()
        {
            return this.Flag.ToString();
        }

        /// <summary>
        /// Method to rotate direction vector to the Left. NORTH is UP, EAST is RIGHT
        /// </summary>
        public void RotateLeft()
        {
            switch (this.Flag)
            {
                case DirectionFlag.NORTH:
                    this.Flag = DirectionFlag.WEST;
                    break;
                case DirectionFlag.EAST:
                    this.Flag = DirectionFlag.NORTH;
                    break;
                case DirectionFlag.SOUTH:
                    this.Flag = DirectionFlag.EAST;
                    break;
                case DirectionFlag.WEST:
                    this.Flag = DirectionFlag.SOUTH;
                    break;
            }

            this.Update();
        }

        /// <summary>
        /// Method to rotate direction vector to the Right. NORTH is UP, EAST is RIGHT
        /// </summary>
        public void RotateRight()
        {
            switch (this.Flag)
            {
                case DirectionFlag.NORTH:
                    this.Flag = DirectionFlag.EAST;
                    break;
                case DirectionFlag.EAST:
                    this.Flag = DirectionFlag.SOUTH;
                    break;
                case DirectionFlag.SOUTH:
                    this.Flag = DirectionFlag.WEST;
                    break;
                case DirectionFlag.WEST:
                    this.Flag = DirectionFlag.NORTH;
                    break;
            }

            this.Update();
        }

        public Direction(DirectionFlag flag)
        {
            this.Flag = flag;
            this.Update();
        }

        /// <summary>
        /// Private method to update direction vector coordinates according to the flag enum value.
        /// </summary>
        private void Update()
        {
            switch (this.Flag)
            {
                case DirectionFlag.NORTH:
                    this.X = 0;
                    this.Y = 1;
                    break;
                case DirectionFlag.EAST:
                    this.X = 1;
                    this.Y = 0;
                    break;
                case DirectionFlag.SOUTH:
                    this.X = 0;
                    this.Y = -1;
                    break;
                case DirectionFlag.WEST:
                    this.X = -1;
                    this.Y = 0;
                    break;
            }
        }
    }
}
