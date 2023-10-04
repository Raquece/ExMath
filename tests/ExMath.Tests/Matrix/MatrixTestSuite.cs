using ExMath.Matrix;

namespace TestSuite.Matrix
{
    public class MatrixTestSuite
    {
        #region Constructors

        [Fact]
        public void GenerateMatrix_InvalidRows_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    _ = new Matrix<int>(-1, 1);
                });
        }

        [Fact]
        public void GenerateMatrix_InvalidColumns_ShouldThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _ = new Matrix<int>(1, -1);
            });
        }

        [Fact]
        public void GenerateMatrix_ValidParams_ShouldNotThrow()
        {
            _ = new Matrix<int>(2, 2);
        }

        [Fact]
        public void GenerateMatrix_InvalidType_ShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidNumericTypeException>(() =>
            {
                _ = new Matrix<InvalidStruct>(1, 1);
            });
        }

        [Fact]
        public void GenerateMatrix_NoSpecifiedDefault_ReturnsDefaultValue()
        {
            int[,] expected =
{
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };

            var m = new Matrix<int>(5, 5);
            Assert.Equal(expected, m.Data);
        }

        [Fact]
        public void GenerateMatrix_SpecifiedDefault_ReturnsSpecifiedValue()
        {
            int[,] expected =
{
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };

            var m = new Matrix<int>(5, 5, 4);
            Assert.Equal(expected, m.Data);
        }

        #endregion

        #region Properties

        [Fact]
        public void Rows_ValidCall_ReturnsCorrectRows()
        {
            var m = new Matrix<int>(3, 4);
            Assert.Equal(3, m.Rows);
        }

        [Fact]
        public void Columns_ValidCall_ReturnsCorrectColumns()
        {
            var m = new Matrix<int>(3, 4);
            Assert.Equal(4, m.Columns);
        }

        [Fact]
        public void IsSquare_NotSquare_ReturnsFalse()
        {
            var m = new Matrix<int>(3, 2);
            Assert.False(m.IsSquare);
        }

        [Fact]
        public void IsSquare_Square_ReturnsTrue()
        {
            var m = new Matrix<int>(3, 3);
            Assert.True(m.IsSquare);
        }

        #endregion

        #region Operators

        [Fact]
        public void EqualsOperator_SameMatrix_ReturnsTrue()
        {
            var a = new Matrix<int>(5, 1, 2);
            var b = new Matrix<int>(5, 1, 2);

            Assert.True(a == b);
        }

        [Fact]
        public void EqualsOperator_DifferentMatrix_ReturnsFalse()
        {
            var a = new Matrix<int>(5, 1, 2);
            var b = new Matrix<int>(5, 3, 2);

            Assert.False(a == b);
        }

        [Fact]
        public void NotEqualsOperator_DifferentMatrix_ReturnsTrue()
        {
            var a = new Matrix<int>(5, 1, 2);
            var b = new Matrix<int>(5, 3, 2);

            Assert.True(a != b);
        }

        [Fact]
        public void NotEqualsOperator_SameMatrix_ReturnsFalse()
        {
            var a = new Matrix<int>(5, 1, 2);
            var b = new Matrix<int>(5, 1, 2);

            Assert.False(a != b);
        }

        [Fact]
        public void MultiplyOperator_ValidCall_ReturnsDotProduct()
        {
            var a = new Matrix<int>(2, 3);
            int[,] aData = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };
            a.Data = aData;

            var b = new Matrix<int>(3, 2);
            int[,] bData = new int[,]
            {
                { 7, 8 },
                { 9, 10 },
                { 11, 12 }
            };
            b.Data = bData;

            int[,] expected =
            {
                { 58, 64 },
                { 139, 154 }
            };
            var product = a * b;
            Assert.Equal(expected, product.Data);
        }

        #endregion

        #region Arithmetic

        [Fact]
        public void Add_ValidCall_ReturnsCorrect()
        {
            var a = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 2, 4 },
                    { 4, 3 },
                    { 2, 1 }
                }
            };
            var b = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 5, 2 },
                    { 4, 6 },
                    { 1, 9 }
                }
            };
            var EXPECTED = new int[,]
            {
                { 7, 6 },
                { 8, 9 },
                { 3, 10 }
            };

            a.Add(b);

            Assert.Equal(EXPECTED, a.Data);
        }

        [Fact]
        public void Add_ValidCall_ReturnsAddedValue()
        {
            var m = GetBasicMatrix();
            int[,] expected =
            {
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };
            m.Add(4);
            Assert.Equal(expected, m.Data);
        }

        [Fact]
        public void Multiply_ValidCall_ReturnsDotProduct()
        {
            var a = new Matrix<int>(2, 3);
            int[,] aData = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };
            a.Data = aData;

            var b = new Matrix<int>(3, 2);
            int[,] bData = new int[,]
            {
                { 7, 8 },
                { 9, 10 },
                { 11, 12 }
            };
            b.Data = bData;

            int[,] expected =
            {
                { 58, 64 },
                { 139, 154 }
            };
            a.Multiply(b);
            Assert.Equal(expected, a.Data);
        }

        [Fact]
        public void Multiply_InvalidMatrices_ThrowsException()
        {
            var a = new Matrix<int>(4, 6);
            var b = new Matrix<int>(2, 9);

            Assert.ThrowsAny<Exception>(() => a.Multiply(b));
        }

        [Fact]
        public void MultiplyScalar_ValidCall_ReturnsCorrect()
        {
            var a = new Matrix<int>(2, 2)
            {
                Data = new int[,]
                {
                    { 2, 3 },
                    { 4, 5 }
                }
            };

            a.Multiply(2);

            int[,] expected = new int[,]
            {
                { 4, 6 },
                { 8, 10 }
            };
            Assert.Equal(expected, a.Data);
        }

        #endregion

        #region Manipulation

        [Fact]
        public void SetAt_ValidCall_ReturnsNewValue()
        {
            var m = GetBasicMatrix();
            m.SetAt(2, 2, 4);
            Assert.Equal(4, m.Data[2, 2]);
        }

        [Fact]
        public void SetAll_ValidCall_ReturnsNewValue()
        {
            int[,] expected =
            {
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };
            var m = GetBasicMatrix();
            m.SetAll(4);
            Assert.Equal(expected, m.Data);
        }

        [Fact]
        public void MapAt_ValidCall_ReturnsMappedValue()
        {
            var m = GetBasicMatrix();
            int[,] expected =
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };
            m.MapAt(2, 1, _ => 4);
            Assert.Equal(expected, m.Data);
        }

        [Fact]
        public void AddAt_ValidCall_ReturnsAddedValue()
        {
            var m = GetBasicMatrix();
            int[,] expected =
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };
            m.AddAt(2, 1, 4);
            Assert.Equal(expected, m.Data);
        }

        [Fact]
        public void Transpose_ReturnsTransposedMatrix()
        {
            var m = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 6, 4, 24 },
                    { 1, -9, 8 }
                }
            };
            int[,] expected = new int[,]
            {
                { 6, 1 },
                { 4, -9 },
                { 24, 8 }
            };
            var t = m.Transpose();

            Assert.NotSame(t, m); // Makes sure that it is independent
            Assert.Equal(expected, t.Data);
        }

        [Fact]
        public void Map_ValidCall_ReturnsMappedValue()
        {
            var m = GetBasicMatrix();
            int[,] expected =
            {
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };
            m.Map(_ => 4);
            Assert.Equal(expected, m.Data);
        }

        #endregion

        #region Query

        [Fact]
        public void GetRow_ValidCall_ReturnsCorrectRow()
        {
            var m = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 2, 4 },
                    { 4, 3 },
                    { 2, 1 }
                }
            };

            int[] expected = new int[] { 4, 3 };
            Assert.Equal(expected, m.GetRow(1));
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(5)]
        public void GetRow_InvalidRow_ThrowsException(int ROW)
        {
            var m = new Matrix<int>(2, 3);
            Assert.ThrowsAny<Exception>(() => m.GetRow(ROW));
        }

        [Fact]
        public void GetColumn_ValidCall_ReturnsCorrectRow()
        {
            var m = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 2, 4 },
                    { 4, 3 },
                    { 2, 1 }
                }
            };

            int[] expected = new int[] { 4, 3, 1 };
            Assert.Equal(expected, m.GetColumn(1));
        }

        [Theory]
        [InlineData(-2)]
        [InlineData(5)]
        public void GetColumn_InvalidColumn_ThrowsException(int COLUMN)
        {
            var m = new Matrix<int>(2, 3);
            Assert.ThrowsAny<Exception>(() => m.GetColumn(COLUMN));
        }

        [Fact]
        public void Find_ValidCall_ReturnsNewValue()
        {
            var m = GetBasicMatrix();
            m.Data[3, 2] = 2;
            Assert.Equal(new Tuple<int, int, int>(3, 2, 2), m.Find(x => x == 2));
        }

        [Fact]
        public void Find_NoResults_ReturnsNull()
        {
            var m = GetBasicMatrix();
            m.Data[3, 2] = 2;
            Assert.Null(m.Find(x => x == 4));
        }

        [Fact]
        public void FindAll_ValidCall_ReturnsNewValue()
        {
            var m = GetBasicMatrix();
            m.Data[3, 2] = 2;
            m.Data[0, 4] = 2;
            List<Tuple<int, int, int>> expected = new List<Tuple<int, int, int>>()
            {
                new Tuple<int, int, int>(0, 4, 2),
                new Tuple<int, int, int>(3, 2, 2)
            };
            Assert.Equal(expected, m.FindAll(x => x == 2));
        }

        #endregion

        #region Clone

        [Fact]
        public void Clone_ShouldCreateClonedObject()
        {
            Matrix<int> a = GetBasicMatrix();
            Matrix<int> b = a.Clone() as Matrix<int>;
            Assert.Equal(a, b); // Checks that the two matrices are equal
            Assert.NotSame(a, b); // Checks that they are not the same instance
        }

        #endregion

        #region Equals

        [Fact]
        public void Equals_EqualMatrices_ReturnsTrue()
        {
            Matrix<int> a = new Matrix<int>(2, 3);
            Matrix<int> b = new Matrix<int>(2, 3);

            Assert.True(a.Equals(b));
        }

        [Fact]
        public void EqualsObjectOverride_EqualMatrices_ReturnsTrue()
        {
            Matrix<int> a = new Matrix<int>(2, 3);
            object b = new Matrix<int>(2, 3);

            Assert.True(a.Equals(b));
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "RCS1202:Avoid NullReferenceException.", Justification = "Definite Value Specified")]
        public void EqualsObjectOverride_NotEqualMatrices_ReturnsFalse()
        {
            Matrix<int> a = new Matrix<int>(2, 3);
            object b = new Matrix<int>(3, 3);
            object c = new Matrix<int>(2, 3);
            (c as Matrix<int>).Data[0, 0] = -2;

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
        }

        [Fact]
        public void EqualsObjectOverride_NotMatrix_ReturnsFalse()
        {
            Matrix<int> a = new Matrix<int>(2, 3);
            object b = 3;

            Assert.False(a.Equals(b));
        }

        [Fact]
        public void Equals_NotEqualMatrices_ReturnsFalse()
        {
            Matrix<int> a = new Matrix<int>(2, 3);
            Matrix<int> b = new Matrix<int>(3, 3);
            Matrix<int> c = new Matrix<int>(2, 3);
            c.Data[0, 0] = -2;

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
        }

        [Fact]
        public void GetHashCode_EqualMatrices_ReturnsSameCode()
        {
            Matrix<int> a = new Matrix<int>(7, 2, 1);
            Matrix<int> b = new Matrix<int>(7, 2, 1);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NotEqualMatrices_ReturnsDifferentCode()
        {
            Matrix<int> a = GetBasicMatrix();
            Matrix<int> b = new Matrix<int>(7, 2, 45);
            Assert.True(a.GetHashCode() != b.GetHashCode());
        }

        #endregion

        #region Serialization

        [Fact]
        public void SerializeDeserialize_ReturnsSameMatrixData()
        {
            Matrix<int> m = GetBasicMatrix();
            int[,] expected =
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 3, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };
            m.Data = expected;

            using MemoryStream stream = m.Serialize();
            Matrix<int> actual = Matrix<int>.Deserialize(stream);
            Assert.Equal(expected, actual.Data);

        }

        [Fact]
        public void Deserialize_InvalidStruct_ThrowsRelevantException()
        {
            Assert.Throws<InvalidNumericTypeException>(() => Matrix<InvalidStruct>.Deserialize(null));
        }

        #endregion

        #region Static Manipulation

        [Fact]
        public void SetAllStatic_ValidCall_ReturnsNewValue()
        {
            int[,] expected =
            {
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };
            Matrix<int> m = GetBasicMatrix();
            Matrix<int> actual = Matrix<int>.SetAll(m, 4);

            Assert.NotSame(actual, m); // Makes sure that it is independent
            Assert.Equal(expected, actual.Data);
        }

        [Fact]
        public void SetAtStatic_ValidCall_ReturnsNewValue()
        {
            Matrix<int> m = GetBasicMatrix();
            var actual = Matrix<int>.SetAt(m, 2, 2, 4);

            Assert.NotSame(actual, m); // Makes sure that it is independent
            Assert.Equal(4, m.Data[2, 2]);
        }

        [Fact]
        public void MapAtStatic_ValidCall_ReturnsMappedValue()
        {
            Matrix<int> m = GetBasicMatrix();
            int[,] expected =
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };
            var actual = Matrix<int>.MapAt(m, 2, 1, _ => 4);

            Assert.NotSame(actual, m); // Makes sure that it is independent
            Assert.Equal(expected, actual.Data);
        }

        [Fact]
        public void AddAtStatic_ValidCall_ReturnsAddedValue()
        {
            Matrix<int> m = GetBasicMatrix();
            int[,] expected =
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 4, 0, 0, 0 },
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0 }
            };
            var actual = Matrix<int>.AddAt(m, 2, 1, 4);

            Assert.NotSame(actual, m);
            Assert.Equal(expected, actual.Data);
        }

        [Fact]
        public void TransposeStatic_ReturnsTransposedMatrix()
        {
            Matrix<int> m = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 6, 4, 24 },
                    { 1, -9, 8 }
                }
            };
            int[,] expected = new int[,]
            {
                { 6, 1 },
                { 4, -9 },
                { 24, 8 }
            };
            Matrix<int> t = Matrix<int>.Transpose(m);

            Assert.NotSame(t, m); // Makes sure that it is independent
            Assert.Equal(expected, t.Data);
        }

        [Fact]
        public void MapStatic_ValidCall_ReturnsMappedValue()
        {
            Matrix<int> m = GetBasicMatrix();
            int[,] expected =
            {
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };
            var actual = Matrix<int>.Map(m, _ => 4);

            Assert.NotSame(actual, m); // Makes sure that it is independent
            Assert.Equal(expected, actual.Data);
        }

        #endregion

        #region Static Arithmetic

        [Fact]
        public void AddStatic_ValidCall_ReturnsAddedValue()
        {
            Matrix<int> m = GetBasicMatrix();
            int[,] expected =
            {
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 },
                { 4, 4, 4, 4, 4 }
            };
            var actual = Matrix<int>.Add(m, 4);

            Assert.NotSame(actual, m);
            Assert.Equal(expected, actual.Data);
        }

        [Fact]
        public void AddStatic_ValidCall_ReturnsCorrect()
        {
            Matrix<int> a = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 2, 4 },
                    { 4, 3 },
                    { 2, 1 }
                }
            };
            Matrix<int> b = new Matrix<int>(2, 3)
            {
                Data = new int[,]
                {
                    { 5, 2 },
                    { 4, 6 },
                    { 1, 9 }
                }
            };
            var EXPECTED = new int[,]
            {
                { 7, 6 },
                { 8, 9 },
                { 3, 10 }
            };

            var actual = Matrix<int>.Add(a, b);

            Assert.NotSame(actual, a); // Makes sure that it is independent
            Assert.Equal(EXPECTED, actual.Data);
        }

        [Fact]
        public void MultiplyStatic_ValidCall_ReturnsDotProduct()
        {
            Matrix<int> a = new Matrix<int>(2, 3);
            int[,] aData = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };
            a.Data = aData;

            Matrix<int> b = new Matrix<int>(3, 2);
            int[,] bData = new int[,]
            {
                { 7, 8 },
                { 9, 10 },
                { 11, 12 }
            };
            b.Data = bData;

            int[,] expected =
            {
                { 58, 64 },
                { 139, 154 }
            };

            var product = Matrix<int>.Multiply(a, b);

            Assert.NotSame(product, a); // Makes sure that it is independent
            Assert.Equal(expected, product.Data);
        }

        [Fact]
        public void MultiplyStatic_InvalidMatrices_ThrowsException()
        {
            Matrix<int> a = new Matrix<int>(4, 6);
            Matrix<int> b = new Matrix<int>(2, 9);

            Assert.ThrowsAny<Exception>(() => Matrix<int>.Multiply(a, b));
        }

        [Fact]
        public void MultiplyScalarStatic_ValidCall_ReturnsCorrect()
        {
            Matrix<int> a = new Matrix<int>(2, 2)
            {
                Data = new int[,]
                {
                    { 2, 3 },
                    { 4, 5 }
                }
            };

            Matrix<int> actual = Matrix<int>.Multiply(a, 2);

            int[,] expected = new int[,]
            {
                { 4, 6 },
                { 8, 10 }
            };

            Assert.NotSame(actual, a); // Makes sure that it is independent
            Assert.Equal(expected, actual.Data);
        }

        #endregion

        private Matrix<int> GetBasicMatrix()
        {
            return new Matrix<int>(5, 5);
        }
    }
}
