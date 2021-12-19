using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    public enum NodeType
    {
        LeftBracket,
        RightBracket,
        Value
    };

    internal class SFNode
    {
        public SFNode(NodeType ntype)
        {
            NodeType = ntype;
            Value = 0;
            Depth = 0;
        }

        public SFNode(int value, int depth)
        {
            NodeType = NodeType.Value;
            Value = value;
            Depth = depth;
        }

        public SFNode(SFNode from)
        {
            NodeType = from.NodeType;
            Value = from.Value;
            Depth = from.Depth;
        }

        public NodeType NodeType { get; set; }
        public int Value { get; set; }
        public int Depth { get; set; }

        public override string ToString()
        {
            return String.Format("Type: {0}, Value: {1}, Depth: {2}", NodeType, Value, Depth);
        }
    }
}
