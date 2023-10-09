using ExMath.Algebra.Analysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Discrete.Graphs
{
    /// <summary>
    /// Represents a node of a tree.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the tree.</typeparam>
    public class Tree<T>
    {
        /// <summary>
        /// Initialises a new instance of a node in a tree.
        /// </summary>
        /// <param name="data">Underlying data of the node.</param>
        /// <param name="nodes">The nodes connected to this node.</param>
        public Tree(T data, params Tree<T>[]? nodes)
            => (Data, Nodes) = (data, nodes);

        /// <summary>
        /// The underlying data of the node.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// The nodes connected to this node.
        /// </summary>
        public Tree<T>[]? Nodes;

        /// <summary>
        /// Returns a string that represents this token.
        /// </summary>
        /// <returns>The string format of data in this tree node.</returns>
        public override string ToString()
        {
            if (Data == null)
            {
                return "NULL";
            }

            return Data.ToString() ?? "NULL";
        }

        /// <summary>
        /// Finds the parent of a node that matches the predicate.
        /// </summary>
        /// <param name="tree">The tree to search.</param>
        /// <param name="predicate">The predicate to match when searching.</param>
        /// <param name="aboveNode">The node above the current (if applicable).</param>
        /// <returns>The parent of the node matching the predicate.</returns>
        public static Tree<T>? FindParent(Tree<T> tree, Func<T, bool> predicate, Tree<T>? aboveNode = null)
        {
            if (predicate.Invoke(tree.Data))
            {
                return aboveNode;
            }

            foreach (var node in tree.Nodes ?? Array.Empty<Tree<T>>())
            {
                var n = Tree<T>.FindParent(node, predicate, tree);

                if (n != null)
                {
                    return n;
                }
            }

            return null;
        }
    }
}
