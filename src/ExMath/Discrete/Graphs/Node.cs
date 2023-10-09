using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Discrete.Graphs
{
    /// <summary>
    /// Represents a node used in graphs.
    /// </summary>
    public class Node : IEquatable<Node>
    {
        public Node(int uid, string? name = null)
            => (UID, Data) = (uid, name);

        /// <summary>
        /// The name of the node
        /// </summary>
        public string Name => Data == null ? UID.ToString() : Data.ToString();

        /// <summary>
        /// The ID of the node as it appears in the graph
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// Contains user defined underlying data (if desired)
        /// </summary>
        public object? Data { get; set; }

        public bool Equals(Node? other)
        {
            if (other == null) return false;

            if (Name == null)
            {
                return other.Name == null && UID.Equals(other.UID);
            }
            else
            {
                return Name.Equals(other.Name) && UID.Equals(other.UID);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
            => Equals(obj as Node);
        
        public override int GetHashCode()
            => HashCode.Combine(UID, Name);
    }
}
