using Xunit;
using MyCozmoLib.Dynamics;

namespace MyCozmoTests
{
    public class DynamicsTests
    {
        public DynamicsTests()
        {

        }

        [Fact]
        public void DirectionMustRotateRight_North()
        {
            var north = new Direction(DirectionFlag.NORTH);

            Assert.Equal(0, north.X);
            Assert.Equal(1, north.Y);

            north.RotateRight();

            Assert.Equal(DirectionFlag.EAST, north.Flag);
        }

        [Fact]
        public void DirectionMustRotateRight_South()
        {
            var north = new Direction(DirectionFlag.SOUTH);

            Assert.Equal(0, north.X);
            Assert.Equal(-1, north.Y);

            north.RotateRight();
            north.RotateRight();

            Assert.Equal(DirectionFlag.NORTH, north.Flag);
        }

        [Fact]
        public void DirectionMustRotateLeft_West()
        {
            var north = new Direction(DirectionFlag.WEST);

            Assert.Equal(-1, north.X);
            Assert.Equal(0, north.Y);

            north.RotateLeft();

            Assert.Equal(DirectionFlag.SOUTH, north.Flag);
        }

        [Fact]
        public void DirectionMustRotateLeft_East()
        {
            var north = new Direction(DirectionFlag.EAST);

            Assert.Equal(1, north.X);
            Assert.Equal(0, north.Y);

            north.RotateLeft();
            north.RotateLeft();

            Assert.Equal(DirectionFlag.WEST, north.Flag);
        }

        [Fact]
        public void DirectionFlagLabel_North()
        {
            var flag = Direction.GetDirection("NORTH");
            var direction = new Direction(flag);

            Assert.Equal(DirectionFlag.NORTH, direction.Flag);
        }

        [Fact]
        public void DirectionFlagLabel_South()
        {
            var flag = Direction.GetDirection("SOUTH");
            var direction = new Direction(flag);

            Assert.Equal(DirectionFlag.SOUTH, direction.Flag);
        }

        [Fact]
        public void DirectionFlagLabel_MustFail()
        {
            DirectionFlag flag;

            Assert.Throws<System.ArgumentException>(() => flag = Direction.GetDirection("North"));
        }

        [Fact]
        public void DirectionFlagLabel_MustFail_2()
        {
            DirectionFlag flag;

            Assert.Throws<System.ArgumentException>(() => flag = Direction.GetDirection("South"));
        }

        [Fact]
        public void LocationDefault()
        {
            var p = new Location();

            Assert.False(p.IsValid);
        }

        [Fact]
        public void LocationMustBeValid()
        {
            var p = new Location(2, 1);

            Assert.True(p.IsValid);
        }

        [Fact]
        public void LocationLimits()
        {
            var p = new Location(20, 1);

            Assert.False(p.IsValid);
        }

        [Fact]
        public void LocationLimits_X()
        {
            var p = new Location(5, 1);
            Assert.False(p.IsValid);

            p += new Direction(DirectionFlag.EAST);
            Assert.Equal(Location.X_LIMIT, p.X);
            Assert.True(p.IsValid);
        }

        [Fact]
        public void LocationLimits_XY()
        {
            var p = new Location(5, -6);
            Assert.False(p.IsValid);

            p += new Direction(DirectionFlag.NORTH);
            Assert.Equal(Location.X_LIMIT, p.X);
            Assert.Equal(0, p.Y);
            Assert.True(p.IsValid);
        }

        [Fact]
        public void LocationLimits_Y()
        {
            int start_x = 2;
            var p = new Location(start_x, 1000);
            Assert.False(p.IsValid);

            p += new Direction(DirectionFlag.SOUTH);
            Assert.Equal(Location.Y_LIMIT, p.Y);
            Assert.Equal(start_x, p.X);
            Assert.True(p.IsValid);
        }
        [Fact]
        public void LocationAddDirection()
        {
            var south = new Direction(DirectionFlag.SOUTH);
            var p = new Location(1, 1);
            Assert.True(p.IsValid);
            p += south;
            Assert.Equal(1, p.X);
            Assert.Equal(0, p.Y);
        }

        [Fact]
        public void LocationMoveAround()
        {
            var east = new Direction(DirectionFlag.EAST);
            var west = new Direction(DirectionFlag.WEST);
            var north = new Direction(DirectionFlag.NORTH);
            var south = new Direction(DirectionFlag.SOUTH);
            var p = new Location(0, 0);

            for (var i = 0; i < 5; i++)
                p += north;
            for (var i = 0; i < 5; i++)
                p += east;
            Assert.Equal(4, p.X);
            Assert.Equal(4, p.Y);
 
            for (var i = 0; i < 5; i++)
                p += south;
            for (var i = 0; i < 5; i++)
                p += west;

            Assert.Equal(0, p.X);
            Assert.Equal(0, p.Y);
        }
    }
}
