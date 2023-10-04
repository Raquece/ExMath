using ExMath.Discrete.Graphs;
using ExMath.Geometry;
using System.Xml.Linq;

namespace TestSuite.Discrete.Graphs
{
    public class GraphTestSuite
    {
        [Fact]
        public void GenerateGraph_ValidParams_ShouldNotThrow()
        {
            Graph<int> _ = new UndirectedGraph<int>();
        }

        [Fact]
        public void GenerateGraph_InvalidParams_ShouldThrow()
        {
            Assert.Throws<InvalidNumericTypeException>(() =>
            {
                Graph<InvalidStruct> _ = new UndirectedGraph<InvalidStruct>();
            });
        }

        [Fact]
        public void AddNode_ValidCall_ReturnsCorrectMatrixSize()
        {
            Graph<int> g = new UndirectedGraph<int>();
            g.AddNode();
            Assert.Equal(new Size(1, 1), g.Adjacency.Size);
        }

        [Fact]
        public void AddNode_OneNode_ReturnsCorrectMatrixData()
        {
            int[,] expected =
            {
                { 0 }
            };

            Graph<int> g = new UndirectedGraph<int>();
            g.AddNode();
            Assert.Equal(expected, g.Adjacency.Data);
        }

        [Fact]
        public void AddNode_OneNode_ReturnsCorrectNodeIndexing()
        {
            Graph<int> g = new UndirectedGraph<int>();
            g.AddNode();
            Assert.Equal(0, g.Nodes[0].UID);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(12)]
        public void AddNode_MultipleNodes_ReturnsCorrectNodeIndexing(int cnode)
        {
            Graph<int> g = new UndirectedGraph<int>();

            for (int i = 0; i < cnode; i++)
            {
                g.AddNode();
            }

            for (int i = 0; i < cnode; i++)
            {
                Assert.Equal(i, g.Nodes[i].UID);
            }
        }

        [Fact]
        public void RemoveNode_EndingNode_ReturnsCorrectMatrix()
        {
            Graph<int> g = new UndirectedGraph<int>();

            g.AddNode();
            g.AddNode();
            g.AddNode();
            g.AddNode();

            g.AddRelation(new(g.Nodes[1], g.Nodes[3], 1));
            g.AddRelation(new(g.Nodes[2], g.Nodes[0], 1));
            
            g.RemoveNode(3);

            int[,] expected =
            {
                { 0, 0, 1 },
                { 0, 0, 0 },
                { 1, 0, 0 }
            };

            Assert.Equal(expected, g.Adjacency.Data);
        }

        [Fact]
        public void RemoveNode_MiddleNode_ReturnsCorrectMatrix()
        {
            Graph<int> g = new UndirectedGraph<int>();

            g.AddNode();
            g.AddNode();
            g.AddNode();
            g.AddNode();

            g.AddRelation(new(g.Nodes[1], g.Nodes[3], 1));
            g.AddRelation(new(g.Nodes[2], g.Nodes[0], 1));

            g.RemoveNode(2);

            int[,] expected =
            {
                { 0, 0, 0 },
                { 0, 0, 1 },
                { 0, 1, 0 }
            };

            Assert.Equal(expected, g.Adjacency.Data);
        }

        [Fact]
        public void RemoveNode_MiddleNode_ReturnsCorrectIndexing()
        {
            Graph<int> g = new UndirectedGraph<int>();

            g.AddNode();
            g.AddNode();
            g.AddNode();
            g.AddNode();

            g.AddRelation(new(g.Nodes[1], g.Nodes[3], 1));
            g.AddRelation(new(g.Nodes[2], g.Nodes[0], 1));
            g.RemoveNode(2);

            for (int i = 0; i < g.Nodes.Count; i++)
            {
                Assert.Equal(i, g.Nodes[i].UID);
            }
        }
    }
}
