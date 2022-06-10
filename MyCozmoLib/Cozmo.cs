using MyCozmoLib.Dynamics;
using System;

namespace MyCozmoLib
{
    public enum CozmoCommand
    {
        HELP = 0,
        EXIT,
        PLACE,
        MOVE,
        LEFT,
        RIGHT,
        REPORT,
        UNKNOWN
    }

    /// <summary>
    /// Class to handle the table robot activity
    /// </summary>
    public class Cozmo
    {
        /// <summary>
        /// Property to get curent location of Cozmo
        /// </summary>
        public Location Location { get; private set; }

        /// <summary>
        /// Property to get curent direction of cozmo
        /// </summary>
        public Direction Direction { get; private set; }

        public Cozmo()
        {
            this.Location = new Location();
        }

        public bool IsOnTable => this.Location.IsValid;

        public static CozmoCommand GetCommand(string command)
        {
            return (CozmoCommand)Enum.Parse(typeof(CozmoCommand), command);
        }

        /// <summary>
        /// Method to place Cozmo on a table location and set current direction (orientation).
        /// </summary>
        /// <param name="location">The location</param>
        /// <param name="direction">The direction</param>
        /// <returns>True if the new location is valid, otherwise False</returns>
        public bool PlaceOnTable(Location location, Direction direction)
        {
            if (!location.IsValid)
            {
                return false;
            }

            this.Location = location;
            this.Direction = direction;

            return true;
        }

        /// <summary>
        /// Method to move Cozmo on a table by one step depending on the current direction
        /// </summary>
        public void Move()
        {
            this.Location += this.Direction;
        }

        public void TurnLeft()
        {
            this.Direction.RotateLeft();
        }

        public void TurnRight()
        {
            this.Direction.RotateRight();
        }

        public string PrintPosition()
        {
            return $"{this.Location.X},{this.Location.Y},{this.Direction.GetFlagString()}";
        }

        public bool ProcessCommand(string cmd, out string message)
        {
            message = "";

            if (string.IsNullOrEmpty(cmd))
            {
                message = "Command must not be empty";
                return true;
            }

            CozmoCommand command = CozmoCommand.UNKNOWN;
            string param = "";

            try
            {
                var str = cmd.Split(' ');

                cmd = str[0];
                if (str.Length > 1)
                {
                    param = str[1];
                }

                command = Cozmo.GetCommand(cmd);

                if (command == CozmoCommand.PLACE && string.IsNullOrEmpty(param))//we process parameters for PLACE command only
                {
                    message = $"Command '{cmd}' cannot have empty parameters.";
                    return true;
                }
            }
            catch (System.ArgumentException ex1)
            {
                message = $"Invalid command. {ex1.Message}";

                return true;
            }
            catch (Exception ex2)
            {
                message = $"Unknown error. {ex2.Message}";
                return true;
            }

            switch(command)
            {
                case CozmoCommand.HELP:
                    message = Cozmo.GetHelp();
                    break;
                case CozmoCommand.MOVE:
                    if (!this.IsOnTable)
                    {
                        message = "I am not on the Table yet. Please PLACE me ...";
                        break;
                    }
                    this.Move();
                    message = this.PrintPosition();
                    break;
                case CozmoCommand.PLACE:
                    Location loc;
                    Direction dir;
                    string msg;
                    if (!this.ProcessPosition(param, out loc, out dir, out msg))
                    {
                        message = msg;
                        break;
                    }

                    if (!loc.IsValid)
                    {
                        message = "Sorry, I can not go on that location. I will fall off the Table";
                        break;
                    }

                    this.Location = loc;
                    this.Direction = dir;
                    message = this.PrintPosition();
                    break;
                case CozmoCommand.REPORT:
                    if (!this.IsOnTable)
                    {
                        message = "Sorry, you have to PLACE me on the Table first ...";
                        break;
                    }
                    message = this.PrintPosition();
                    break;
                case CozmoCommand.LEFT:
                    if (!this.IsOnTable)
                    {
                        message = "Sorry, I am not on the right PLACE yet ...";
                        break;
                    }
                    this.TurnLeft();
                    message = this.PrintPosition();
                    break;
                case CozmoCommand.RIGHT:
                    if (!this.IsOnTable)
                    {
                        message = "Sorry, you didn't PLACE me somewhere on the Table yet ...";
                        break;
                    }
                    this.TurnRight();
                    message = this.PrintPosition();
                    break;
                case CozmoCommand.EXIT:
                    message = "Bye bye! Hope to see you again soon!";
                    return false;
                default:
                    message = "Sorry, didn't get the command. Please try again ...";
                    break;
            }

            return true;
        }

        public bool ProcessPosition(string position, out Location location, out Direction direction, out string err)
        {
            location = new Location();
            direction = new Direction(DirectionFlag.UNKNOWN);
            err = "";

            if (string.IsNullOrEmpty(position))
            {
                err = "Invalid position location or direction. Empty values.";
                return false;
            }

            var lines = position.Split(',');

            if (lines.Length != 3)
            {
                err = "Invalid position location or direction. Valid Example: 1,1,SOUTH";
                return false;
            }

            int y;
            int x;

            if (!int.TryParse(lines[0], out x))
            {
                err = "Invalid location. X value must be integer. Valid Example: 1,0,EAST";
                return false;
            }

            if (!int.TryParse(lines[1], out y))
            {
                err = "Invalid location. Y value must be integer. Valid Example: 2,1,NORTH";
                return false;
            }

            DirectionFlag flag;

            if (!Direction.TryGetDirection(lines[2], out flag))
            {
                location = new Location(x, y);
                err = "Invalid direction. It can be: NORTH/EAST/SOUTH/WEST only. Valid Example: 3,1,WEST";
                return false;
            }

            location = new Location(x, y);
            direction = new Direction(flag);

            return true;
        }

        public static string GetHelp()
        {
            string help = "Hello. My name is Cozmo. I am a Table Robot.\nYou can move me on the tablespace 5x5 units.\n";
            
            help += "I understand the following Commands (CAPITAL LETTERS ONLY, please :-)):\n";
            help += "\tHELP - Print this help\n";
            help += "\tEXIT - Terminate the game\n";
            help += "\tPLACE X,Y,Direction - Put me on the table in position X,Y and facing NORTH, SOUTH, EAST or WEST.\n";
            help += "\t\tThe table is facing Up (NORTH). Bottom-Left corner has coordinate 0,0\n";
            help += "\t\tDirection can be: NORTH/EAST/SOUTH/WEST. Bottom-Left corner has 0,0 coordinate\n";
            help += "\t\tEAST is to the Right, WEST - Left, South - Down.\n";
            help += "\t\tExample: PLACE 1,1,SOUTH - I will be placed at the coordinate 2,2 facing SOUTH\n";
            help += "\tMOVE - Move me one unit forward in the direction it is currently facing.\n";
            help += "\tLEFT - Rotate me 90 degrees to the left without changing my position.\n";
            help += "\tRIGHT - Rotate me 90 degrees to the right without changing my position.\n";
            help += "\tREPORT - Announce my X,Y and Direction.\n\n";
            return help;
        }
    }
}
