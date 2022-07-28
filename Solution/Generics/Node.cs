using System;
using System.Collections.Generic;
using System.Text;

namespace Solution
{
    class Node
    {
        public int _Id { get; set; }
        public Point _Location { get; set; }
        public int _NodeValue { get; set; }
        public int _ParentID { get; set; }
        public int _Level { get; set; }
        public Node(int id, Point loc, int nodeValue, int parentId, int level)
        {
            _Id = id;
            _Location = loc;
            _NodeValue = nodeValue;
            _ParentID = parentId;
            _Level = level;
        }
    }
}
