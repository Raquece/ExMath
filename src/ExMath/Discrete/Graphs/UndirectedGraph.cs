namespace ExMath.Discrete.Graphs
{
    /// <summary>
    /// Represents an undirected graph
    /// </summary>
    /// <typeparam name="T">The type to be used for weights in relationships</typeparam>
    public class UndirectedGraph<T> : Graph<T>
        where T : struct, IEquatable<T>
    {
        /// <summary>
        /// Adds an undirected edge to the graph
        /// </summary>
        /// <param name="relationship">The edge to add</param>
        public override void AddRelation(Relationship<T> relationship)
        {
            Adjacency.MapAt(relationship.Node1.UID, relationship.Node2.UID, w => (dynamic)w + 1);
            Adjacency.MapAt(relationship.Node2.UID, relationship.Node1.UID, w => (dynamic)w + 1);

            _relations.Add(relationship);
        }

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="relationship">The edge to add</param>
        public override void RemoveRelation(Relationship<T> relationship)
        {
            Adjacency.MapAt(relationship.Node1.UID, relationship.Node2.UID, w => (dynamic)w - 1);
            Adjacency.MapAt(relationship.Node2.UID, relationship.Node1.UID, w => (dynamic)w - 1);

            _relations.Remove(relationship);
        }
    }
}
