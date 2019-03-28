using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Maze_Generator
{
    /// <summary>
    /// Creates, solves and draws mazes
    /// </summary>
    internal class Maze
    {
        /// <summary>
        /// Initializes a maze with a maximum size
        /// </summary>
        /// <param name="totalWidth">The maximum width of the maze</param>
        /// <param name="totalHeight">The maximum height of the maze</param>
        public Maze(int totalWidth, int totalHeight)
        {
            this.maze = new Cell[totalHeight, totalWidth];
        }

        /// <summary>
        /// Indicates the current sleeping time (used to slow operation)
        /// </summary>
        public int Sleep
        {
            get;
            set;
        }

        private const int sleepPeriod = 1000;

        /// <summary>
        /// Indicates the currnet width the user selects
        /// </summary>
        private int width;

        /// <summary>
        /// /// Indicates the currnet height the user selects
        /// </summary>
        private int height;

        /// <summary>
        /// Used to draw the red line while the maze is being generated
        /// </summary>
        private Point currentGenerateLocation;

        /// <summary>
        /// Used to distinguish different directions in the right-hand rule
        /// </summary>
        private enum Directions
        {
            Up,
            Down,
            Left,
            Right
        }

        /// <summary>
        /// The array that carries all the Cell instances in the maze
        /// </summary>
        private Cell[,] maze;

        /// <summary>
        /// Indicates the maze begin
        /// </summary>
        private Cell begin;

        /// <summary>
        /// Indicates the maze end
        /// </summary>
        private Cell end;

        /// <summary>
        /// Used to view current position when the maze is being solved
        /// </summary>
        private Point currentSolvePos;

        /// <summary>
        /// Indicates the pen of location drawing
        /// </summary>
        private Pen locationPen = new Pen(Brushes.IndianRed, 5);

        /// <summary>
        /// Indicates the pen used to draw the maze walls
        /// </summary>
        private Pen mazePen = new Pen(Brushes.WhiteSmoke, 3);

        /// <summary>
        /// Indicates the width of one rectangle on the maze
        /// </summary>
        private int unitX
        {
            get { return this.maze.GetLength(1) / this.width; }
        }

        /// <summary>
        /// Indicates the height of one rectangle on the maze
        /// </summary>
        private int unitY
        {
            get { return this.maze.GetLength(0) / this.height; }
        }

        /// <summary>
        /// Gets a value whether the maze is busy in a job
        /// </summary>
        private bool working;

        /// <summary>
        /// Used to draw the found path
        /// </summary>
        private List<Cell> foundPath = new List<Cell>();

        /// <summary>
        /// Gets a value indicates whether the Maze is busy in solving
        /// </summary>
        private bool solving;

        /// <summary>
        /// Used to generate maze
        /// </summary>
        private Random random = new Random();

        /// <summary>
        /// Generates a maze with the specific size
        /// </summary>
        /// <param name="width">Number of squares in width</param>
        /// <param name="height">Number of squares in height</param>
        /// <param name="method">indicates the method used to generate the maze</param>
        public void Generate(int width, int height, int method)
        {
            this.working = true;

            this.initailze(this.maze, width, height);

            this.mazePen.Dispose();
            this.mazePen = this.unitX < 5 ? new Pen(Brushes.WhiteSmoke, 1) : new Pen(Brushes.WhiteSmoke, 3);
            if (method == 0)
                this.depthFirstSearchMazeGeneration(this.maze, this.width, this.height);
            else
                this.breadthFirstSearchMazeGeneration(this.maze, this.width, this.height);
            this.working = false;
        }

        /// <summary>
        /// Resets a maze array
        /// </summary>
        /// <param name="arr">The maze array</param>
        /// <param name="width">Number of squares in width</param>
        /// <param name="height">Number of squares in height</param>
        private void initailze(Cell[,] arr, int width, int height)
        {
            this.width = width;
            this.height = height;

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    arr[i, j] = new Cell(new Point(j * this.unitX, i * this.unitY), new Point(j, i));
                }
            }
        }

        /// <summary>
        /// Generate a maze with the Depth-First Search approach
        /// </summary>
        /// <param name="arr">the array of cells</param>
        /// <param name="width">A width for the maze</param>
        /// <param name="height">A height for the maze</param>
        private void depthFirstSearchMazeGeneration(Cell[,] arr, int width, int height)
        {
            Stack<Cell> stack = new Stack<Cell>();
            Random random = new Random();

            Cell location = arr[this.random.Next(height), this.random.Next(width)];
            stack.Push(location);
            
            while (stack.Count > 0)
            {
                List<Point> neighbours = this.getNeighbours(arr, location, width, height);
                if (neighbours.Count > 0)
                {
                    Point temp = neighbours[random.Next(neighbours.Count)];

                    this.currentGenerateLocation = temp;

                    this.knockWall(arr, ref location, ref arr[temp.Y, temp.X]);

                    stack.Push(location);
                    location = arr[temp.Y, temp.X];
                }
                else
                {
                    location = stack.Pop();
                }

                Thread.SpinWait(this.Sleep * sleepPeriod);
            }

            this.makeMazeBeginEnd(this.maze);
        }

        /// <summary>
        /// Generate a maze with the Breadth-First Search approach
        /// </summary>
        /// <param name="arr">the array of cells</param>
        /// <param name="width">A width for the maze</param>
        /// <param name="height">A height for the maze</param>
        private void breadthFirstSearchMazeGeneration(Cell[,] arr, int width, int height)
        {
            Queue<Cell> queue = new Queue<Cell>();
            Random random = new Random();

            Cell location = arr[this.random.Next(height), this.random.Next(width)];
            queue.Enqueue(location);

            while (queue.Count > 0)
            {
                List<Point> neighbours = this.getNeighbours(arr, location, width, height);
                if (neighbours.Count > 0)
                {
                    Point temp = neighbours[random.Next(neighbours.Count)];

                    this.currentGenerateLocation = temp;

                    this.knockWall(arr, ref location, ref arr[temp.Y, temp.X]);

                    queue.Enqueue(location);
                    location = arr[temp.Y, temp.X];
                }
                else
                {
                    location = queue.Dequeue();
                }

                Thread.SpinWait(this.Sleep * sleepPeriod);
            }

            this.makeMazeBeginEnd(this.maze);
        }

        /// <summary>
        /// Used to create a begin and end for a maze
        /// </summary>
        /// <param name="arr">The array of the maze</param>
        private void makeMazeBeginEnd(Cell[,] arr)
        {
            Point temp = new Point();
            temp.Y = this.random.Next(this.height);
            arr[temp.Y, temp.X].LeftWall = false;
            this.begin = arr[temp.Y, temp.X];

            temp.Y = this.random.Next(this.height);
            temp.X = this.width - 1;
            arr[temp.Y, temp.X].RightWall = false;
            this.end = arr[temp.Y, temp.X];
        }

        /// <summary>
        /// Knocks wall between two neighbor cellls
        /// </summary>
        /// <param name="maze">The maze array</param>
        /// <param name="current">the current cell</param>
        /// <param name="next">the next neighbor cell</param>
        private void knockWall(Cell[,] maze, ref Cell current, ref Cell next)
        {
            // The next is down
            if (current.Position.X == next.Position.X && current.Position.Y > next.Position.Y)
            {
                maze[current.Position.Y, current.Position.X].UpWall = false;
                maze[next.Position.Y, next.Position.X].DownWall = false;
            }
            // the next is up
            else if (current.Position.X == next.Position.X)
            {
                maze[current.Position.Y, current.Position.X].DownWall = false;
                maze[next.Position.Y, next.Position.X].UpWall = false;
            }
            // the next is right
            else if (current.Position.X > next.Position.X)
            {
                maze[current.Position.Y, current.Position.X].LeftWall = false;
                maze[next.Position.Y, next.Position.X].RightWall = false;
            }
            // the next is left
            else
            {
                maze[current.Position.Y, current.Position.X].RightWall = false;
                maze[next.Position.Y, next.Position.X].LeftWall = false;
            }
        }

        /// <summary>
        /// Determines whether a particular cell has all its walls intact
        /// </summary>
        /// <param name="arr">the maze array</param>
        /// <param name="cell">The cell to check</param>
        /// <returns></returns>
        private bool allWallsIntact(Cell[,] arr, Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!arr[cell.Position.Y, cell.Position.X][i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets all neighbor cells to a specific cell, 
        /// where those neighbors exist and not visited already
        /// </summary>
        /// <param name="arr">The maze array</param>
        /// <param name="cell">The current cell to get neighbors</param>
        /// <param name="width">The width of the maze</param>
        /// <param name="height">The height of the maze</param>
        /// <returns></returns>
        private List<Point> getNeighbours(Cell[,] arr, Cell cell, int width, int height)
        {
            Point temp = cell.Position;
            List<Point> availablePlaces = new List<Point>();

            // Left
            temp.X = cell.Position.X - 1;
            if (temp.X >= 0 && this.allWallsIntact(arr, arr[temp.Y, temp.X]))
            {
                availablePlaces.Add(temp);
            }
            // Right
            temp.X = cell.Position.X + 1;
            if (temp.X < width && this.allWallsIntact(arr, arr[temp.Y, temp.X]))
            {
                availablePlaces.Add(temp);
            }

            // Up
            temp.X = cell.Position.X;
            temp.Y = cell.Position.Y - 1;
            if (temp.Y >= 0 && this.allWallsIntact(arr, arr[temp.Y, temp.X]))
            {
                availablePlaces.Add(temp);
            }
            // Down
            temp.Y = cell.Position.Y + 1;
            if (temp.Y < height && this.allWallsIntact(arr, arr[temp.Y, temp.X]))
            {
                availablePlaces.Add(temp);
            }
            return availablePlaces;
        }

        /// <summary>
        /// Draws the maze on a specific surface
        /// </summary>
        /// <param name="g">a surface to draw on</param>
        public void Draw(Graphics g)
        {
            g.Clear(Color.Black);

            // in case Generate() have not been called yet
            if (this.width == 0)
                return;

            // draws begin
            g.FillRectangle(Brushes.Red, new Rectangle(this.begin.Location, new Size(this.unitX, this.unitY)));

            // loap on every cell in the bounds
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    // Visited cell: fill green square
                    if (this.maze[i, j].Visited)
                    {
                        g.FillRectangle(Brushes.Green, new Rectangle(this.maze[i, j].Location, new Size(this.unitX, this.unitY)));
                    }
                    
                    // draws a red line indicates the current generation location
                    if (this.working && this.currentGenerateLocation.X == j && this.currentGenerateLocation.Y == i)
                    {
                        Point target = new Point(j * unitX, i * unitY);
                        drawVerticalWall(g, ref target, unitY, this.locationPen);
                    }

                    // fills the current square in the solving process
                    if (this.solving && this.currentSolvePos.X == j && this.currentSolvePos.Y == i)
                    {
                        g.FillRectangle(Brushes.IndianRed, new Rectangle(this.maze[i,j].Location, new Size(this.unitX, this.unitY)));
                    }

                    // Draw the intact walls
                    this.maze[i, j].Draw(g, mazePen, new Size(this.unitX, this.unitY));

                    if (this.maze[i, j].Path != Cell.Paths.None)
                    {
                        switch (this.maze[i, j].Path)
                        {
                            case Cell.Paths.Up:
                                g.DrawLine(this.locationPen,
                    new Point(this.maze[i, j].Location.X + unitX / 2, this.maze[i, j].Location.Y + unitY / 2),
                    new Point(this.maze[i - 1, j].Location.X + unitX / 2, this.maze[i - 1, j].Location.Y + unitY / 2));
                                break;
                            case Cell.Paths.Down:
                                g.DrawLine(this.locationPen,
                    new Point(this.maze[i, j].Location.X + unitX / 2, this.maze[i, j].Location.Y + unitY / 2),
                    new Point(this.maze[i + 1, j].Location.X + unitX / 2, this.maze[i + 1, j].Location.Y + unitY / 2));
                                break;
                            case Cell.Paths.Right:
                                g.DrawLine(this.locationPen,
                    new Point(this.maze[i, j].Location.X + unitX / 2, this.maze[i, j].Location.Y + unitY / 2),
                    new Point(this.maze[i, j + 1].Location.X + unitX / 2, this.maze[i, j + 1].Location.Y + unitY / 2));
                                break;
                            default:
                                g.DrawLine(this.locationPen,
                    new Point(this.maze[i, j].Location.X + unitX / 2, this.maze[i, j].Location.Y + unitY / 2),
                    new Point(this.maze[i, j - 1].Location.X + unitX / 2, this.maze[i, j - 1].Location.Y + unitY / 2));
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws the found path on a specific surface
        /// </summary>
        /// <param name="g">a surface to draw on</param>
        public void DrawPath(Graphics g)
        {
            // maze-begin square
            g.FillRectangle(Brushes.Red, new Rectangle(this.begin.Location, new Size(this.unitX, this.unitY)));

            // loap on every item in the list
            for (int i = 1; i < this.foundPath.Count; i++)
            {
                // draw a line between the (i item) and (i - 1 item)
                // line begins in the middle of both squares so we add unit / 2
                g.DrawLine(this.locationPen,
                    new Point(this.foundPath[i].Location.X + unitX / 2, this.foundPath[i].Location.Y + unitY / 2),
                    new Point(this.foundPath[i - 1].Location.X + unitX / 2, this.foundPath[i - 1].Location.Y + unitY / 2));
            }
        }

        /// <summary>
        /// Draws a vertical line with the specifc height and pen
        /// </summary>
        /// <param name="g">a surface to draw on</param>
        /// <param name="location">The point of line begin</param>
        /// <param name="height">The height of line</param>
        /// <param name="pen">The pen used to draw</param>
        private static void drawVerticalWall(Graphics g, ref Point location, int height, Pen pen)
        {
            g.DrawLine(pen,
                location,
                new Point(location.X, location.Y + height));
        }

        /// <summary>
        /// Used to reset all cells
        /// </summary>
        /// <param name="arr">The maze array to reset elements</param>
        private void unvisitAll(Cell[,] arr)
        {
            for (int i = 0; i < this.maze.GetLength(0); i++)
            {
                for (int j = 0; j < this.maze.GetLength(1); j++)
                {
                    arr[i, j].Visited = false;
                    arr[i, j].Path = Cell.Paths.None;
                }
            }
        }

        /// <summary>
        /// Solves the current maze using a specific method
        /// </summary>
        /// <param name="method">The used method to solve with</param>
        public unsafe void Solve(int method)
        {
            this.solving = true;
            // initialize
            this.foundPath.Clear();
            this.unvisitAll(this.maze);
            
            // selecting the method
            if (method == 0)
            {
                if (this.height * this.width < 40 * 80)
                    fixed (Cell* ptr = &this.begin)
                        this.depthFirstSearchSolve(ptr, ref this.end);
                else
                    this.iterativeDepthFirstSearchSolve(this.begin, this.end);
            }
            else if (method == 1)
                this.breadthFirstSearchSolve(this.begin, this.end);
            else if (method == 2)
                this.iterativeRightHandRuleSolve(this.begin, Directions.Right);

            this.solving = false;
        }

        /// <summary>
        /// Solves a maze with recursive backtracking DFS - 
        /// DON'T USE! IT IS FOR DEMONSTRATING PURPOSES ONLY!
        /// </summary>
        /// <param name="start">The start of the maze cell</param>
        /// <param name="end">The end of the maze cell</param>
        /// <returns>returrns true if the path is found</returns>
        private unsafe bool depthFirstSearchSolve(Cell *st, ref Cell end)
        {
            // base condition
            if (st->Position == end.Position)
            {
                // make it visited in order to be drawed with green
                this.maze[st->Position.Y, st->Position.X].Visited = true;
                // add end point to the foundPath
                this.foundPath.Add(*st);
                return true;
            }
            

            // has been visited alread, return
            if (this.maze[st->Position.Y, st->Position.X].Visited)
                return false;

            // for the graphics
            this.currentSolvePos = st->Position;
            // used to slow the process
            Thread.SpinWait(this.Sleep * sleepPeriod);

            // mark as visited
            this.maze[st->Position.Y, st->Position.X].Visited = true;

            // Check every neighbor cell
            // If it exists (not outside the maze bounds)
            // and if there is no wall between start and it
            // recursive call this method with it
            // if it returns true, add the current start to foundPath and return true too
            // else complete

            // Left
            if (st->Position.X - 1 >= 0 && !this.maze[st->Position.Y, st->Position.X - 1].RightWall)
            {
                this.maze[st->Position.Y, st->Position.X].Path = Cell.Paths.Left;
                fixed (Cell *ptr = &this.maze[st->Position.Y, st->Position.X - 1])
                {
                    if (this.depthFirstSearchSolve(ptr, ref end))
                    {
                        this.foundPath.Add(*st);
                        return true;
                    }
                }
            }
            // for the graphics
            this.currentSolvePos = st->Position;
            // used to slow the process
            Thread.SpinWait(this.Sleep * sleepPeriod);
            // Right
            if (st->Position.X + 1 < this.width && !this.maze[st->Position.Y, st->Position.X + 1].LeftWall)
            {
                this.maze[st->Position.Y, st->Position.X].Path = Cell.Paths.Right;
                fixed (Cell* ptr = &this.maze[st->Position.Y, st->Position.X + 1])
                {
                    if (this.depthFirstSearchSolve(ptr, ref end))
                    {
                        this.foundPath.Add(*st);
                        return true;
                    }
                }
            }
            // for the graphics
            this.currentSolvePos = st->Position;
            // used to slow the process
            Thread.SpinWait(this.Sleep * sleepPeriod);
            
            // Up
            if (st->Position.Y - 1 >= 0 && !this.maze[st->Position.Y - 1, st->Position.X].DownWall)
            {
                this.maze[st->Position.Y, st->Position.X].Path = Cell.Paths.Up;
                fixed (Cell* ptr = &this.maze[st->Position.Y - 1, st->Position.X])
                {
                    if (this.depthFirstSearchSolve(ptr, ref end))
                    {
                        this.foundPath.Add(*st);
                        return true;
                    }
                }
            }

            // for the graphics
            this.currentSolvePos = st->Position;
            // used to slow the process
            Thread.SpinWait(this.Sleep * sleepPeriod);

            // Down
            if (st->Position.Y + 1 < this.height && !this.maze[st->Position.Y + 1, st->Position.X].UpWall)
            {
                this.maze[st->Position.Y, st->Position.X].Path = Cell.Paths.Down;
                fixed (Cell* ptr = &this.maze[st->Position.Y + 1, st->Position.X])
                {
                    if (this.depthFirstSearchSolve(ptr, ref end))
                    {
                        this.foundPath.Add(*st);
                        return true;
                    }
                }
            }
            this.maze[st->Position.Y, st->Position.X].Path = Cell.Paths.None;
            return false;
        }

        /// <summary>
        /// Solves a maze with iterative backtracking DFS
        /// </summary>
        /// <param name="start">The start of the maze cell</param>
        /// <param name="end">The end of the maze cell</param>
        /// <returns>returrns true if the path is found</returns>
        private unsafe bool iterativeDepthFirstSearchSolve(Cell start, Cell end)
        {
            // unsafe indicates that this method uses pointers
            Stack<Cell> stack = new Stack<Cell>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                Cell temp = stack.Pop();

                // base condition
                if (temp.Position == end.Position)
                {
                    // add end point to foundPath
                    this.foundPath.Add(temp);
                    // dereference all pointers chain until you reach the begin
                    while (temp.Previous != null)
                    {
                        this.foundPath.Add(temp);
                        temp = *temp.Previous;
                    }
                    // add begin point to foundPath
                    this.foundPath.Add(temp);
                    // to view green square on it
                    this.maze[temp.Position.Y, temp.Position.X].Visited = true;
                    return true;
                }

                // for the graphics
                this.currentSolvePos = temp.Position;

                // mark as visited to prevent infinite loops
                this.maze[temp.Position.Y, temp.Position.X].Visited = true;

                // used to slow operation
                Thread.SpinWait(this.Sleep * sleepPeriod);


                // Check every neighbor cell
                // If it exists (not outside the maze bounds)
                // and if there is no wall between start and it
                    // set the next.Previous to the current cell
                    // push next into stack
                // else complete


                // Left
                if (temp.Position.X - 1 >= 0
                    && !this.maze[temp.Position.Y, temp.Position.X - 1].RightWall
                    && !this.maze[temp.Position.Y, temp.Position.X - 1].Visited)
                {
                    // fixed must be used to indicate that current memory-location won't be changed
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y, temp.Position.X - 1].Previous = cell;
                    stack.Push(this.maze[temp.Position.Y, temp.Position.X - 1]);
                }

                // Right
                if (temp.Position.X + 1 < this.width
                    && !this.maze[temp.Position.Y, temp.Position.X + 1].LeftWall
                    && !this.maze[temp.Position.Y, temp.Position.X + 1].Visited)
                {
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y, temp.Position.X + 1].Previous = cell;
                    stack.Push(this.maze[temp.Position.Y, temp.Position.X + 1]);
                }

                // Up
                if (temp.Position.Y - 1 >= 0
                    && !this.maze[temp.Position.Y - 1, temp.Position.X].DownWall
                    && !this.maze[temp.Position.Y - 1, temp.Position.X].Visited)
                {
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y - 1, temp.Position.X].Previous = cell;
                    stack.Push(this.maze[temp.Position.Y - 1, temp.Position.X]);
                }

                // Down
                if (temp.Position.Y + 1 < this.height
                    && !this.maze[temp.Position.Y + 1, temp.Position.X].UpWall
                    && !this.maze[temp.Position.Y + 1, temp.Position.X].Visited)
                {
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y + 1, temp.Position.X].Previous = cell;
                    stack.Push(this.maze[temp.Position.Y + 1, temp.Position.X]);
                }
            }
            // no solution found
            return false;
        }

        /// <summary>
        /// Solves a maze with iterative backtracking BFS
        /// </summary>
        /// <param name="start">The start of the maze cell</param>
        /// <param name="end">The end of the maze cell</param>
        /// <returns>returrns true if the path is found</returns>
        private unsafe bool breadthFirstSearchSolve(Cell start, Cell end)
        {
            // unsafe indicates that this method uses pointers
            Queue<Cell> queue = new Queue<Cell>();

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                Cell temp = queue.Dequeue();

                // base condition
                if (temp.Position == end.Position)
                {
                    // add end point to foundPath
                    this.foundPath.Add(temp);
                    // dereference all pointers chain until you reach the begin
                    while (temp.Previous != null)
                    {
                        this.foundPath.Add(temp);
                        temp = *temp.Previous;
                    }
                    // add begin point to foundPath
                    this.foundPath.Add(temp);
                    // to view green square on it
                    this.maze[temp.Position.Y, temp.Position.X].Visited = true;
                    return true;
                }

                // for the graphics
                this.currentSolvePos = temp.Position;

                // mark as visited to prevent infinite loops
                this.maze[temp.Position.Y, temp.Position.X].Visited = true;

                // used to slow operation
                Thread.SpinWait(this.Sleep * sleepPeriod);


                // Check every neighbor cell
                // If it exists (not outside the maze bounds)
                // and if there is no wall between start and it
                    // set the next.Previous to the current cell
                    // add next into queue
                // else complete


                // Left
                if (temp.Position.X - 1 >= 0
                    && !this.maze[temp.Position.Y, temp.Position.X - 1].RightWall
                    && !this.maze[temp.Position.Y, temp.Position.X - 1].Visited)
                {
                    // fixed must be used to indicate that current memory-location won't be changed
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y, temp.Position.X - 1].Previous = cell;
                    queue.Enqueue(this.maze[temp.Position.Y, temp.Position.X - 1]);
                }

                // Right
                if (temp.Position.X + 1 < this.width
                    && !this.maze[temp.Position.Y, temp.Position.X + 1].LeftWall
                    && !this.maze[temp.Position.Y, temp.Position.X + 1].Visited)
                {
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y, temp.Position.X + 1].Previous = cell;
                    queue.Enqueue(this.maze[temp.Position.Y, temp.Position.X + 1]);
                }

                // Up
                if (temp.Position.Y - 1 >= 0
                    && !this.maze[temp.Position.Y - 1, temp.Position.X].DownWall
                    && !this.maze[temp.Position.Y - 1, temp.Position.X].Visited)
                {
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y - 1, temp.Position.X].Previous = cell;
                    queue.Enqueue(this.maze[temp.Position.Y - 1, temp.Position.X]);
                }

                // Down
                if (temp.Position.Y + 1 < this.height
                    && !this.maze[temp.Position.Y + 1, temp.Position.X].UpWall
                    && !this.maze[temp.Position.Y + 1, temp.Position.X].Visited)
                {
                    fixed (Cell* cell = &this.maze[temp.Position.Y, temp.Position.X])
                        this.maze[temp.Position.Y + 1, temp.Position.X].Previous = cell;
                    queue.Enqueue(this.maze[temp.Position.Y + 1, temp.Position.X]);
                }
            }
            // no solution found
            return false;
        }

        /// <summary>
        /// Solves the maze with the right-hand role iteratively
        /// </summary>
        /// <param name="start">The maze begin</param>
        /// <param name="dir">the initial direction</param>
        private void iterativeRightHandRuleSolve(Cell start, Directions dir)
        {
            // look at your right (with respect to your direction)
            // No wall? go in it.
            // Wall? look at your front (with respect to your direction)
            // Wall too? look at your left (with respect to your direction)
            // Wall too? go back (in the reverse of your direction)

            // note that the right of the right is down, the left of down is right, ect
        
            bool flag;
            // repeat while you didn't reach the end
            while (start.Position != this.end.Position)
            {
                // for graphics
                this.maze[start.Position.Y, start.Position.X].Visited = true;
                // for graphics
                this.currentSolvePos = start.Position;

                // to slow operation
                Thread.SpinWait(this.Sleep * sleepPeriod);

                switch (dir)
                {
                    case Directions.Right:
                        // has up wall?
                        flag = start.Position.Y + 1 < height;
                        if (!flag || this.maze[start.Position.Y + 1, start.Position.X].UpWall)
                        {
                            // has left wall?
                            flag = start.Position.X + 1 < width;
                            if (!flag || maze[start.Position.Y, start.Position.X + 1].LeftWall)
                            {
                                // has down wall ?
                                flag = start.Position.Y - 1 >= 0;
                                if (!flag || maze[start.Position.Y - 1, start.Position.X].DownWall)
                                {
                                    start = this.maze[start.Position.Y, start.Position.X];
                                    dir = Directions.Left;
                                }
                                else
                                {
                                    start = maze[start.Position.Y - 1, start.Position.X];
                                    dir = Directions.Up;
                                }
                            }
                            else
                            {
                                start = this.maze[start.Position.Y, start.Position.X + 1];
                                dir = Directions.Right;
                            }
                        }
                        else
                        {
                            start = this.maze[start.Position.Y + 1, start.Position.X];
                            dir = Directions.Down;
                        }
                        break;
                    case Directions.Left:
                        flag = start.Position.Y - 1 >= 0;
                        if (!flag || maze[start.Position.Y - 1, start.Position.X].DownWall)
                        {
                            flag = start.Position.X - 1 >= 0;
                            if (!flag || maze[start.Position.Y, start.Position.X - 1].RightWall)
                            {
                                flag = start.Position.Y + 1 < this.height;
                                if (!flag || maze[start.Position.Y + 1, start.Position.X].UpWall)
                                {
                                    start = maze[start.Position.Y, start.Position.X];
                                    dir = Directions.Right;
                                }
                                else
                                {
                                    start = maze[start.Position.Y + 1, start.Position.X];
                                    dir = Directions.Down;
                                }
                            }
                            else
                            {
                                start = maze[start.Position.Y, start.Position.X - 1];
                                dir = Directions.Left;
                            }
                        }
                        else
                        {
                            start = maze[start.Position.Y - 1, start.Position.X];
                            dir = Directions.Up;
                        }
                        break;
                    case Directions.Up:
                        flag = start.Position.X + 1 < this.width;
                        if (!flag || maze[start.Position.Y, start.Position.X + 1].LeftWall)
                        {
                            flag = start.Position.Y - 1 >= 0;
                            if (!flag || maze[start.Position.Y - 1, start.Position.X].DownWall)
                            {
                                flag = start.Position.X - 1 >= 0;
                                if (!flag || maze[start.Position.Y, start.Position.X - 1].RightWall)
                                {
                                    start = maze[start.Position.Y, start.Position.X];
                                    dir = Directions.Down;
                                }
                                else
                                {
                                    start = maze[start.Position.Y, start.Position.X - 1];
                                    dir = Directions.Left;
                                }
                            }
                            else
                            {
                                start = maze[start.Position.Y - 1, start.Position.X];
                                dir = Directions.Up;
                            }
                        }
                        else
                        {
                            start = maze[start.Position.Y, start.Position.X + 1];
                            dir = Directions.Right;
                        }
                        break;
                    default:
                        flag = start.Position.X - 1 >= 0;
                        if (!flag || maze[start.Position.Y, start.Position.X - 1].RightWall)
                        {
                            flag = start.Position.Y + 1 < this.height;
                            if (!flag || maze[start.Position.Y + 1, start.Position.X].UpWall)
                            {
                                flag = start.Position.X + 1 < this.width;
                                if (!flag || maze[start.Position.Y, start.Position.X + 1].LeftWall)
                                {
                                    start = maze[start.Position.Y, start.Position.X];
                                    dir = Directions.Up;
                                }
                                else
                                {
                                    start = maze[start.Position.Y, start.Position.X + 1];
                                    dir = Directions.Right;
                                }
                            }
                            else
                            {
                                start = maze[start.Position.Y + 1, start.Position.X];
                                dir = Directions.Down;
                            }
                        }
                        else
                        {
                            start = maze[start.Position.Y, start.Position.X - 1];
                            dir = Directions.Left;
                        }
                        break;
                }
            }

            this.maze[start.Position.Y, start.Position.X].Visited = true;
            this.currentSolvePos = start.Position;
        }

        /// <summary>
        /// Solves the maze with the right-hand role recursively -
        /// DON'T USE! IT IS FOR DEMONSTRATING PURPOSES ONLY!
        /// </summary>
        /// <param name="start">The maze begin</param>
        /// <param name="dir">the initial direction</param>
        private void rightHandRuleSolve(Cell start, Directions dir)
        {
        	if (start.Position == this.end.Position)
            {
        		return;
        	}

            // mark it as visited for graphics
            this.maze[start.Position.Y, start.Position.X].Visited = true;
            // to view the current solving location
            this.currentSolvePos = start.Position;
            // to slow operation
            Thread.SpinWait(this.Sleep * sleepPeriod);

            bool flag;

        	switch (dir)
        	{
        	case Directions.Right:
                    flag = start.Position.Y + 1 < height;
        		if (!flag || this.maze[start.Position.Y + 1, start.Position.X].UpWall)
        		{
                    flag = start.Position.X + 1 < width;
        			if (!flag || maze[start.Position.Y, start.Position.X + 1].LeftWall)
        			{
                        flag = start.Position.Y - 1 >= 0;
        				if (!flag || maze[start.Position.Y - 1, start.Position.X].DownWall)
                        {
        					rightHandRuleSolve(this.maze[start.Position.Y, start.Position.X], Directions.Left);
        				}
        				else
                        {
        					rightHandRuleSolve(maze[start.Position.Y - 1, start.Position.X], Directions.Up);
        				}
        			}
        			else
                    {
        				rightHandRuleSolve(this.maze[start.Position.Y, start.Position.X + 1], Directions.Right);
        			}
        		}
        		else
                {
        			rightHandRuleSolve(this.maze[start.Position.Y + 1, start.Position.X], Directions.Down);
        		}
        		break;
        
        	case Directions.Left:
                flag = start.Position.Y - 1 >= 0;
        		if (!flag || maze[start.Position.Y - 1, start.Position.X].DownWall)
        		{
                    flag = start.Position.X - 1 >= 0;
        			if (!flag || maze[start.Position.Y, start.Position.X - 1].RightWall)
        			{
                        flag = start.Position.Y + 1 < this.height;
        				if (!flag || maze[start.Position.Y + 1, start.Position.X].UpWall)
        				{
        					rightHandRuleSolve(maze[start.Position.Y, start.Position.X], Directions.Right);
        				}
        				else
                        {
        					rightHandRuleSolve(maze[start.Position.Y + 1, start.Position.X], Directions.Down);
        				}
        			}
        			else
                    {
        				rightHandRuleSolve(maze[start.Position.Y, start.Position.X - 1], Directions.Left);
        			}
        		}
        		else
                {
        			rightHandRuleSolve(maze[start.Position.Y - 1, start.Position.X], Directions.Up);
        		}
        		break;
        
        	case Directions.Up:
                flag = start.Position.X + 1 < this.width;
        		if (!flag || maze[start.Position.Y, start.Position.X + 1].LeftWall)
        		{
                    flag = start.Position.Y - 1 >= 0;
        			if (!flag || maze[start.Position.Y - 1, start.Position.X].DownWall)
        			{
                        flag = start.Position.X - 1 >= 0;
        				if (!flag || maze[start.Position.Y, start.Position.X - 1].RightWall)
        				{
        					rightHandRuleSolve(maze[start.Position.Y, start.Position.X], Directions.Down);
        				}
        				else
                        {
        					rightHandRuleSolve(maze[start.Position.Y, start.Position.X - 1], Directions.Left);
        				}
        			}
        			else
                    {
        				rightHandRuleSolve(maze[start.Position.Y - 1, start.Position.X], Directions.Up);
        			}
        		}
        		else
                {
        			rightHandRuleSolve(maze[start.Position.Y, start.Position.X + 1], Directions.Right);
        		}
        		break;
        	default:
                flag = start.Position.X - 1 >= 0;
        		if (!flag || maze[start.Position.Y, start.Position.X - 1].RightWall)
        		{
                    flag = start.Position.Y + 1 < this.height;
        			if (!flag || maze[start.Position.Y + 1, start.Position.X].UpWall)
        			{
                        flag = start.Position.X + 1 < this.width;
        				if (!flag || maze[start.Position.Y, start.Position.X + 1].LeftWall)
        				{
        					rightHandRuleSolve(maze[start.Position.Y, start.Position.X], Directions.Up);
        				}
        				else
                        {
        					rightHandRuleSolve(maze[start.Position.Y, start.Position.X + 1], Directions.Right);
        				}
        			}
        			else
                    {
        				rightHandRuleSolve(maze[start.Position.Y + 1, start.Position.X], Directions.Down);
        			}
        		}
        		else
                {
        			rightHandRuleSolve(maze[start.Position.Y, start.Position.X - 1], Directions.Left);
        		}
        		break;
        	}
        }
    }
}