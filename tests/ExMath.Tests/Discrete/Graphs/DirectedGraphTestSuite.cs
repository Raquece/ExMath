using ExMath.Discrete.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.Discrete.Graphs
{
    public class DirectedGraphTestSuite
    {
        [Fact]
        public void AddRelation_ValidCall_ShouldNotThrow()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();

            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
        }

        [Fact]
        public void AddRelation_ValidCall_ReturnsCorrectMatrix()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));

            int[,] expected =
            {
                { 0, 0, 1 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void AddRelation_ValidCall_ReturnsCorrectRelationCount()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));

            Assert.Single(graph.Relations);
        }

        [Fact]
        public void AddRelation_MultipleNodes_ReturnsCorrectMatrix()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));

            int[,] expected =
            {
                { 0, 0, 1 },
                { 1, 0, 0 },
                { 0, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void AddRelation_MultipleNodes_ReturnsCorrectRelationSize()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));

            Assert.Equal(2, graph.Relations.Count);
        }

        [Fact]
        public void RemoveRelation_FirstRelation_ReturnsCorrectMatrix()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));
            graph.RemoveRelation(graph.Relations[0]);

            int[,] expected =
            {
                { 0, 0, 0 },
                { 1, 0, 0 },
                { 0, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void RemoveRelation_FirstRelation_ReturnsCorrectRelationCount()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));
            graph.RemoveRelation(graph.Relations[0]);

            Assert.Single(graph.Relations);
        }

        [Fact]
        public void RemoveRelation_SecondRelation_ReturnsCorrectMatrix()
        {
            var graph = new DirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));
            graph.RemoveRelation(graph.Relations[1]);

            int[,] expected =
            {
                { 0, 0, 1 },
                { 0, 0, 0 },
                { 0, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }
    }
}
