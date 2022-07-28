using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solution
{
    class Program
    {
        static List<Node> _Nodes = new List<Node>();
        static int[][] _Points;
        static int _Value = 1;
        static int _Level = 0;
        static void Main(string[] args)
        {

            _Points = ReadFile("map.txt"); 
            for (int i = 0; i < _Points.Length; i++)
            {
                for (int j = 0; j < _Points[i].Length; j++)
                {
                    AddNodeLevel(new Point(i, j), _Points[i][j]); 
                }
            }
            while (AddSubNodeLevel() > 0) { } 

            var longestPath = _Nodes.OrderByDescending(x => x._Level).FirstOrDefault()._Level; 
            var steepestPathNodes = new List<int[]>(); 
            _Nodes.Where(y => y._Level == longestPath).OrderBy(x => x._Id).ToList().ForEach(y => steepestPathNodes.Add(GetTreeNodeValues(y)));
            var steepestPath = steepestPathNodes.OrderByDescending(x => x.Max() - x.Min()).FirstOrDefault(); 
            Console.WriteLine("The drop is : {0} ", steepestPath.Max() - steepestPath.Min());
            Console.WriteLine("The path is :{0}", string.Join(",", steepestPath));
            Console.ReadLine();

        }

        /// <summary>
        /// Read file using System.IO
        /// </summary>
        /// <param name="path"> Location path: string </param>
        /// <returns></returns>
        static int[][] ReadFile(string path)
        {
            var filePath = path ; 
            return File.ReadLines(filePath).Skip(1).Select(x => x.Split(' ').Select(y => int.Parse(y)).ToArray()).ToArray();
        }
        /// <summary>
        /// Evaluate possible routes according to current nodes
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        static int[] GetTreeNodeValues(Node node)
        {
            var nodePath = new List<int>() { node._NodeValue };
            Node parentNode = null;
            do
            {
                parentNode = _Nodes.Where(x => x._Id == node._ParentID).SingleOrDefault();
                if (parentNode != null)
                    nodePath.Add(parentNode._NodeValue);
                node = parentNode;
            } while (parentNode != null);
            List<int> reverse = Enumerable.Reverse(nodePath).ToList();
            return reverse.ToArray();
        }

        /// <summary>
        /// add a new node level on node list
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeValue"></param>
        static void AddNodeLevel(Point node, int nodeValue)
        {
            _Nodes.Add(new Node(_Value++, node, nodeValue, 0, 0)); 
        }

        /// <summary>
        /// Add a new sublevel of nodes
        /// </summary>
        /// <returns></returns>
        static int AddSubNodeLevel()
        {
            int count = 0;
            _Nodes.Where(x => x._Level == _Level).ToArray().ToList().ForEach(x =>
            {
                count += AddPoint(Direction.North, x) + AddPoint(Direction.South, x) + AddPoint(Direction.East, x) + AddPoint(Direction.West, x);
            });
            _Level++;
            return count;
        }

        /// <summary>
        ///  Add a new location to use in a node
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        static int AddPoint(Direction dir, Node node)
        {
            Point neighbour = FindPoint(dir, node._Location);
            if (neighbour != null && _Points[neighbour._X][neighbour._Y] < node._NodeValue)
            {
                _Nodes.Add(new Node(_Value++, neighbour, _Points[neighbour._X][neighbour._Y], node._Id, _Level + 1));
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// find point from direction 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        static Point FindPoint(Direction dir, Point node)
        {
            Point nextLoc = null;
            switch (dir)
            {
                case Direction.North:
                    nextLoc = node._X - 1 >= 0 ? new Point(node._X - 1, node._Y) : null;
                    break;
                case Direction.South:
                    nextLoc = node._X + 1 < _Points.GetLength(0) ? new Point(node._X + 1, node._Y) : null;
                    break;
                case Direction.East:
                    nextLoc = node._Y + 1 < _Points.GetLength(0) ? new Point(node._X, node._Y + 1) : null;
                    break;
                case Direction.West:
                    nextLoc = node._Y - 1 >= 0 ? new Point(node._X, node._Y - 1) : null;
                    break;
                default:
                    break;
            }
            return nextLoc;

        }
    }
}
