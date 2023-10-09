using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Discrete.Graphs
{
    /// <summary>
    /// Represents an edge in a graph between two nodes.
    /// </summary>
    /// <typeparam name="T">The type to be used for weights in relationships</typeparam>
    public class Relationship<T> : IEquatable<Relationship<T>>
        where T : struct, IEquatable<T>
    {
        public Relationship(Node node1, Node node2, T value)
            => (Node1, Node2, Value) = (node1, node2, value);
        
        /// <summary>
        /// The first node in the relationship
        /// </summary>
        public Node Node1 { get; set; }

        /// <summary>
        /// The second node in the relationship
        /// </summary>
        public Node Node2 { get; set; }

        /// <summary>
        /// The weight of the edge
        /// </summary>
        public T Value { get; set; }

        public bool Equals(Relationship<T>? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Node1 == other.Node1 && Node2 == other.Node2 && Value.Equals(other.Value))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Node1.Name + " -> " + Node2.Name;
        }

        public override bool Equals(object? obj)
            => Equals(obj as Relationship<T>);

        public override int GetHashCode()
            => HashCode.Combine(Node1, Node2);
    }
}
