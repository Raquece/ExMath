using ExMath.Matrix;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ExMath.Discrete.Graphs
{
    /// <summary>
    /// Represents a graph (from graph theory)
    /// </summary>
    /// <typeparam name="T">The type to be used for weights in relationships</typeparam>
    public abstract class Graph<T>
        where T : struct, IEquatable<T>
    {
        public Graph()
        {
            Adjacency = new Matrix<T>(0, 0);
        }

        protected readonly List<Node> _nodes = new();
        protected readonly List<Relationship<T>> _relations = new();

        /// <summary>
        /// The adjacency matrix of the graph
        /// </summary>
        public Matrix<T> Adjacency { get; private set; }

        /// <summary>
        /// Gets the read only collection of nodes in the graph.
        /// </summary>
        public ReadOnlyCollection<Node> Nodes => _nodes.AsReadOnly();

        /// <summary>
        /// Gets the read only collection of nodes in the graph.
        /// </summary>
        public ReadOnlyCollection<Relationship<T>> Relations => _relations.AsReadOnly();

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        public void AddNode(string? name = null)
        {
            _nodes.Add(new Node(_nodes.Count, name));

            Matrix<T> m = new(_nodes.Count, _nodes.Count);

            for (int i = 0; i < Adjacency.Rows; i++)
            {
                for (int j = 0; j < Adjacency.Columns; j++)
                {
                    m.SetAt(i, j, Adjacency.Data[i,j]);
                }
            }

            Adjacency = m;
        }

        /// <summary>
        /// Removes a node from the graph.
        /// </summary>
        /// <param name="index">The index of the node.</param>
        public void RemoveNode(int index)
        {
            // Remove each edge connected to the node
            _relations.Where(r => r.Node1.UID == index || r.Node2.UID == index).ToList()
                .ForEach(RemoveRelation);

            _nodes.RemoveAt(index);

            Matrix<T> m = new(_nodes.Count, _nodes.Count);

            int gap_i = 0;
            int gap_j = 0;

            for (int i = 0; i < Adjacency.Rows; i++)
            {
                if (i == index)
                {
                    gap_i = 1;
                    continue;
                }

                for (int j = 0; j < Adjacency.Columns; j++)
                {
                    if (j == index)
                    {
                        gap_j = 1;
                        continue;
                    }

                    m.SetAt(i - gap_i, j - gap_j, Adjacency.Data[i, j]);
                }

                gap_j = 0;
            }

            // Reset UIDs for nodes to remove gap
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].UID = i;
            }

            Adjacency = m;
        }

        /// <summary>
        /// Adds an edge to the graph
        /// </summary>
        /// <param name="relationship">The edge to add</param>
        public abstract void AddRelation(Relationship<T> relationship);

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="relationship">The edge to add</param>
        public abstract void RemoveRelation(Relationship<T> relationship);
    }
}
