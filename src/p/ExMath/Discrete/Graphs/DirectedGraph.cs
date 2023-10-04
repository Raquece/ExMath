using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Discrete.Graphs
{
    /// <summary>
    /// Represents a directed graph
    /// </summary>
    /// <typeparam name="T">The type to be used for weights in relationships</typeparam>
    public class DirectedGraph<T> : Graph<T>
        where T : struct, IEquatable<T>
    {
        /// <summary>
        /// Adds a directed edge to the graph
        /// </summary>
        /// <param name="relationship">The edge to add</param>
        public override void AddRelation(Relationship<T> relationship)
        {
            Adjacency.MapAt(relationship.Node1.UID, relationship.Node2.UID, w => (dynamic)w + 1);

            _relations.Add(relationship);
        }

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="relationship">The edge to add</param>
        public override void RemoveRelation(Relationship<T> relationship)
        {
            Adjacency.MapAt(relationship.Node1.UID, relationship.Node2.UID, w => (dynamic)w - 1);

            _relations.Remove(relationship);
        }
    }
}
