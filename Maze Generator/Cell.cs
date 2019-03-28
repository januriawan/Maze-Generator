using System;
using System.Drawing;

namespace Maze_Generator
{
    /// <summary>
    /// Represents a maze cell
    /// </summary>
    internal struct Cell
    {
        /// <summary>
        /// Initializes a new instance of maze cell with locations
        /// </summary>
        /// <param name="location">The location on the graphics surface</param>
        /// <param name="position">The location on the 2d array</param>
        public unsafe Cell(Point location, Point position)
        {
            this.location = location;
            this.position = position;

            // initially, all walls are intact
            this.LeftWall = true;
            this.RightWall = true;
            this.UpWall = true;
            this.DownWall = true;
            this.Path = Paths.None;

            // must be initialized, since it is a member of a struct
            this.Visited = false;
            this.Previous = null;
        }

        /// <summary>
        /// Gets or sets a value whether the cell has an intact left wall
        /// </summary>
        public bool LeftWall;

        /// <summary>
        /// /// Gets or sets a value whether the cell has an intact right wall
        /// </summary>
        public bool RightWall;

        /// <summary>
        /// Gets or sets a value whether the cell has an intact up wall
        /// </summary>
        public bool UpWall;

        /// <summary>
        /// Gets or sets a value whether the cell has an intact down wall
        /// </summary>
        public bool DownWall;

        /// <summary>
        /// Gets or sets a value whether the cell has been visited already
        /// </summary>
        public bool Visited;

        public enum Paths
        {
            Up, Down, Right, Left, None
        }

        public Paths Path;

        /// <summary>
        /// Gets or sets a pointer to the previous Cell in the found path chain
        /// </summary>
        public unsafe Cell* Previous;

        /// <summary>
        /// Provides indexing to the boolean fields in the cell
        /// </summary>
        /// <param name="index">0 leftW, 1 rightW, 2 UpW, 3 downW, 4 visited</param>
        /// <returns></returns>
        public bool this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.LeftWall;
                    case 1:
                        return this.RightWall;
                    case 2:
                        return this.UpWall;
                    case 3:
                        return this.DownWall;
                    case 4:
                        return this.Visited;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.LeftWall = value;
                        break;
                    case 1:
                        this.RightWall = value;
                        break;
                    case 2:
                        this.UpWall = value;
                        break;
                    case 3:
                        this.DownWall = value;
                        break;
                    case 4:
                        this.Visited = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Point location;
        /// <summary>
        /// The current location on the graphics surface
        /// </summary>
        public Point Location
        {
            get { return this.location; }
        }

        private Point position;
        /// <summary>
        /// The current location on the two-dimensional container
        /// </summary>
        public Point Position
        {
            get { return this.position; }
        }

        /// <summary>
        /// Reset a cell so that all walls are intact and not visited
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < 4; i++)
            {
                this[i] = true;
            }
            this.Visited = false;
        }

        /// <summary>
        /// Draws a cell onto a graphics surface
        /// </summary>
        /// <param name="g">the graphics surface to draw on</param>
        /// <param name="pen">a pen to draw walls</param>
        /// <param name="size">The width of horizontal wall and height of the vertical walls</param>
        public void Draw(Graphics g, Pen pen, Size size)
        {
            // Draws every wall, if it is intact
            if (this.LeftWall)
            {
                g.DrawLine(pen,
                    this.location,
                    new Point(this.location.X, this.location.Y + size.Height));
            }
            if (this.RightWall)
            {
                g.DrawLine(pen,
                    new Point(this.location.X + size.Width, this.location.Y),
                    new Point(this.location.X + size.Width, this.location.Y + size.Height));
            }
            if (this.UpWall)
            {
                g.DrawLine(pen,
                    this.location,
                    new Point(this.location.X + size.Width, this.location.Y));
            }
            if (this.DownWall)
            {
                g.DrawLine(pen,
                    new Point(this.location.X, this.location.Y + size.Height),
                    new Point(this.location.X + size.Width, this.location.Y + size.Height));
            }
        }
    }
}
