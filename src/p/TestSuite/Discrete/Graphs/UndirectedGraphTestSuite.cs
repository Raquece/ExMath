using ExMath.Discrete.Graphs;
using ExMath.Matrix;

namespace TestSuite.Discrete.Graphs
{
    public class UndirectedGraphTestSuite
    {
        [Fact]
        public void AddRelation_ValidCall_ShouldNotThrow()
        {
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();

            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
        }

        [Fact]
        public void AddRelation_ValidCall_ReturnsCorrectMatrix()
        {
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));

            int[,] expected =
            {
                { 0, 0, 1 },
                { 0, 0, 0 },
                { 1, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void AddRelation_ValidCall_ReturnsCorrectRelationCount()
        {
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));

            Assert.Single(graph.Relations);
        }

        [Fact]
        public void AddRelation_MultipleNodes_ReturnsCorrectMatrix()
        {
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));

            int[,] expected =
            {
                { 0, 1, 1 },
                { 1, 0, 0 },
                { 1, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void AddRelation_MultipleNodes_ReturnsCorrectRelationSize()
        {
            var graph = new UndirectedGraph<int>();

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
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));
            graph.RemoveRelation(graph.Relations[0]);

            int[,] expected =
            {
                { 0, 1, 0 },
                { 1, 0, 0 },
                { 0, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void RemoveRelation_FirstRelation_ReturnsCorrectRelationCount()
        {
            var graph = new UndirectedGraph<int>();

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
            var graph = new UndirectedGraph<int>();

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
                { 1, 0, 0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);
        }

        [Fact]
        public void RemoveRelation_SecondRelation_ReturnsCorrectRelationCount()
        {
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[0], 1));
            graph.RemoveRelation(graph.Relations[1]);

            Assert.Single(graph.Relations);
        }

        [Fact]
        public void TesT()
        {
            var graph = new UndirectedGraph<int>();

            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddNode();
            graph.AddNode();

            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[4], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[3], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[0], graph.Nodes[1], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[3], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[1], graph.Nodes[2], 1));
            graph.AddRelation(new Relationship<int>(graph.Nodes[3], graph.Nodes[4], 1));

            int[,] expected =
            {
                {0,1,0,1,1 },
                {1,0,1,1,0 },
                {0,1,0,0,0 },
                {1,1,0,0,1 },
                {1,0,0,1,0 }
            };

            Assert.Equal(expected, graph.Adjacency.Data);

            Matrix<int> m2 = Matrix<int>.Multiply(graph.Adjacency, graph.Adjacency);

            int[,] expected2 =
{
                {4,6,1,5,5 },
                {6,2,3,6,2 },
                {1,3,0,1,2 },
                {5,6,1,4,5 },
                {5,2,2,5,2 }
            };

            Matrix<int> m3 = Matrix<int>.Multiply(m2, graph.Adjacency);

            Assert.Equal(expected2, m3.Data);

            int[,] expecte3d =
{
                {0,1,0,1,1 },
                {1,0,1,1,0 },
                {0,1,0,0,0 },
                {1,1,0,0,1 },
                {1,0,0,1,0 }
            };

            Assert.Equal(expecte3d, graph.Adjacency.Data);
        }
    }
}
