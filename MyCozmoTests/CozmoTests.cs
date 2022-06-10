using MyCozmoLib;
using MyCozmoLib.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyCozmoTests
{
    public class CozmoTests
    {
        [Fact]
        public void CozmoDefaultLocation()
        {
            var cozmo = new Cozmo();

            Assert.False(cozmo.IsOnTable);
        }

        [Fact]
        public void CozmoValidPlacement()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(2, 2), new Direction(DirectionFlag.SOUTH));
            Assert.True(cozmo.IsOnTable);
        }

        [Fact]
        public void CozmoInvalidPlacement_Negative()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(-1, 0), new Direction(DirectionFlag.NORTH));
            Assert.False(cozmo.IsOnTable);
        }

        [Fact]
        public void CozmoInvalidPlacement_Positive()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(20, 1000), new Direction(DirectionFlag.EAST));
            Assert.False(cozmo.IsOnTable);
        }

        [Fact]
        public void CozmoMove_3Steps()
        {
            var cozmo = new Cozmo();
            var startDirection = DirectionFlag.EAST;

            cozmo.PlaceOnTable(new Location(0, 0), new Direction(startDirection));
            cozmo.Move();
            cozmo.Move();
            cozmo.Move();

            Assert.Equal(3, cozmo.Location.X);
            Assert.Equal(0, cozmo.Location.Y);
            Assert.Equal(startDirection, cozmo.Direction.Flag);
        }

        [Fact]
        public void CozmoMove_ToLimits()
        {
            var cozmo = new Cozmo();
            var startDirection = DirectionFlag.WEST;

            cozmo.PlaceOnTable(new Location(1, 1), new Direction(startDirection));
            cozmo.Move();
            cozmo.Move();
            cozmo.Move();

            Assert.Equal(0, cozmo.Location.X);
            Assert.Equal(1, cozmo.Location.Y);
            Assert.Equal(startDirection, cozmo.Direction.Flag);
        }

        [Fact]
        public void CozmoMoveAndRotate_Left()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(2, 0), new Direction(DirectionFlag.NORTH));
            cozmo.Move();
            cozmo.Move();
            cozmo.TurnLeft();
            cozmo.TurnLeft();
            cozmo.Move();
            cozmo.Move();

            Assert.Equal(2, cozmo.Location.X);
            Assert.Equal(0, cozmo.Location.Y);
            Assert.Equal(DirectionFlag.SOUTH, cozmo.Direction.Flag);
        }

        [Fact]
        public void CozmoMoveAndRotate_LeftAndRight()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(2, 0), new Direction(DirectionFlag.NORTH));
            cozmo.Move();
            cozmo.TurnLeft();
            cozmo.Move();
            cozmo.Move();
            cozmo.TurnRight();
            cozmo.Move();

            Assert.Equal(0, cozmo.Location.X);
            Assert.Equal(2, cozmo.Location.Y);
            Assert.Equal(DirectionFlag.NORTH, cozmo.Direction.Flag);
        }

        [Fact]
        public void CozmoRotate360_Left()
        {
            var cozmo = new Cozmo();
            var startDirection = DirectionFlag.WEST;
            var startX = 2;
            var startY = 3;

            cozmo.PlaceOnTable(new Location(startX, startY), new Direction(startDirection));
            cozmo.TurnLeft();
            cozmo.TurnLeft();
            cozmo.TurnLeft();
            cozmo.TurnLeft();

            Assert.Equal(startX, cozmo.Location.X);
            Assert.Equal(startY, cozmo.Location.Y);
            Assert.Equal(startDirection, cozmo.Direction.Flag);
        }

        [Fact]
        public void CozmoRotate360_Right()
        {
            var cozmo = new Cozmo();
            var startDirection = DirectionFlag.WEST;
            var startX = 1;
            var startY = 0;

            cozmo.PlaceOnTable(new Location(startX, startY), new Direction(startDirection));
            cozmo.TurnRight();
            cozmo.TurnRight();
            cozmo.TurnRight();
            cozmo.TurnRight();

            Assert.Equal(startX, cozmo.Location.X);
            Assert.Equal(startY, cozmo.Location.Y);
            Assert.Equal(startDirection, cozmo.Direction.Flag);
        }

        [Fact]
        public void CozmoPosition_Label()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(1, 2), new Direction(DirectionFlag.SOUTH));
            cozmo.Move();
            cozmo.TurnLeft();
            cozmo.Move();
            cozmo.Move();

            Assert.Equal("3,1,EAST", cozmo.PrintPosition());
        }

        [Fact]
        public void CozmoPosition_Label_2()
        {
            var cozmo = new Cozmo();

            cozmo.PlaceOnTable(new Location(1, 1), new Direction(DirectionFlag.NORTH));
            cozmo.Move();
            cozmo.TurnRight();
            cozmo.Move();
            cozmo.Move();
            cozmo.TurnLeft();
            cozmo.Move();

            Assert.Equal("3,3,NORTH", cozmo.PrintPosition());
        }

        [Fact]
        public void CozmoProcessCommand_Invalid()
        {
            var cozmo = new Cozmo();
            var cmd = "xyz";
            string err;
            var doContinue = cozmo.ProcessCommand(cmd, out err);

            Assert.True(doContinue);
            Assert.NotNull(err);
            Assert.Contains("Invalid command", err);
        }

        [Fact]
        public void CozmoProcessCommand_ValidNotPlaced()
        {
            var cozmo = new Cozmo();
            var cmd = "MOVE";
            string err = null;
            var doContinue = cozmo.ProcessCommand(cmd, out err);

            Assert.True(doContinue);
            Assert.NotNull(err);
            Assert.Contains("Please PLACE me", err);
        }

        [Fact]
        public void CozmoProcessCommand_ValidPlaced()
        {
            var cozmo = new Cozmo();
            var param = "1,1,SOUTH";
            var cmd = "PLACE " + param;
            string err;
            var doContinue = cozmo.ProcessCommand(cmd, out err);

            Assert.True(doContinue);
            Assert.Equal(param, cozmo.PrintPosition());
            Assert.Equal(param, err);
        }

        [Fact]
        public void CozmoProcessCommand_InvalidPlaceParam()
        {
            var cozmo = new Cozmo();
            var param = "";
            Location loc;
            Direction dir;
            string err;
            var isValid = cozmo.ProcessPosition(param, out loc, out dir, out err);

            Assert.False(isValid);
            Assert.NotEmpty(err);
        }

        [Fact]
        public void CozmoProcessCommand_InvalidPlaceParam_2()
        {
            var cozmo = new Cozmo();
            var param = ",,";
            Location loc;
            Direction dir;
            string err;
            var isValid = cozmo.ProcessPosition(param, out loc, out dir, out err);

            Assert.False(isValid);
            Assert.NotEmpty(err);
        }

        [Fact]
        public void CozmoProcessCommand_InvalidPlaceParam_Direction()
        {
            var cozmo = new Cozmo();
            var param = "1,0,south";
            Location loc;
            Direction dir;
            string err;
            var isValid = cozmo.ProcessPosition(param, out loc, out dir, out err);

            Assert.False(isValid);
            Assert.NotEmpty(err);
            Assert.Equal(1, loc.X);
            Assert.Equal(0, loc.Y);
        }

        [Fact]
        public void CozmoProcessCommand_ValidPlaceParam()
        {
            var cozmo = new Cozmo();
            var param = "2,1,NORTH";
            Location loc;
            Direction dir;
            string err;
            var isValid = cozmo.ProcessPosition(param, out loc, out dir, out err);

            Assert.True(isValid);
            Assert.Empty(err);
            Assert.Equal(2, loc.X);
            Assert.Equal(1, loc.Y);
            Assert.Equal(DirectionFlag.NORTH, dir.Flag);
        }
    }
}
