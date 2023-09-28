using ExMath.Extensions;
using ExMath.Geometry;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExMath.Matrix
{
    /// <summary>
    /// Contains data and logic for a matrix
    /// </summary>
    /// <typeparam name="T">The type of each element in the array</typeparam>
    [Serializable]
    public class Matrix<T> :
        IEquatable<Matrix<T>>, ICloneable
        where T : struct
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix{T}"/> class.
        /// </summary>
        /// <param name="rows">The number of rows in the matrix</param>
        /// <param name="columns">The number of columns in the matrix</param>
        /// <param name="def">The default value for each element in the list</param>
        public Matrix(int rows, int columns, T def = default)
        {
            // Check that parameters and type parameters are valid
            if (rows < 0)
                throw new ArgumentOutOfRangeException(nameof(rows), "Parameter 'Rows' is invalid - must be a positive integer");
            if (columns < 0)
                throw new ArgumentOutOfRangeException(nameof(columns), "Parameter 'Columns' is invalid - must be a positive integer");
            Numerics.ThrowIfNotNumber<T>();

            Data = new T[rows, columns];

            // Set all to the specified default value
            SetAll(def);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of rows in the matrix
        /// </summary>
        public int Rows => Data.GetLength(0);

        /// <summary>
        /// Gets the number of columns in the matrix
        /// </summary>
        public int Columns => Data.GetLength(1);

        /// <summary>
        /// Gets or sets the elements of the matrix
        /// </summary>
        public T[,] Data { get; set; }

        /// <summary>
        /// Gets if the matrix identifies as a square matrix
        /// </summary>
        public bool IsSquare => Rows == Columns;

        /// <summary>
        /// Gets the dimensions of the matrix.
        /// </summary>
        public Size Size => new Size(Rows, Columns);

        #endregion

        #region Operators

        /// <summary>
        /// Compares whether two matrices are equal
        /// </summary>
        /// <param name="left">The first matrix to compare</param>
        /// <param name="right">The second matrix to compare</param>
        /// <returns>If the matrices are equal</returns>
        public static bool operator ==(Matrix<T> left, Matrix<T> right)
        {
            return EqualityComparer<Matrix<T>>.Default.Equals(left, right);
        }

        /// <summary>
        /// Compares whether two matrices are not equal
        /// </summary>
        /// <param name="left">The first matrix to compare</param>
        /// <param name="right">The second matrix to compare</param>
        /// <returns>If the matrices are not equal</returns>
        public static bool operator !=(Matrix<T> left, Matrix<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Multiplies two matrices together
        /// </summary>
        /// <param name="left">The first matrix to multiply</param>
        /// <param name="right">The second matrix to multiply</param>
        /// <returns>The product matrix</returns>
        public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
        {
            return Multiply(left, right);
        }

        #endregion

        #region Arithmetic

        /// <summary>
        /// Multiplies two matrices together.
        /// </summary>
        /// <param name="matrix">The matrix that the current object will be multiplied by.</param>
        public void Multiply(Matrix<T> matrix)
        {
            // Check that the two matrices are valid
            if (Columns != matrix.Rows)
                throw new InvalidOperationException("The matrices are incompatible for multiplication");

            Matrix<T> m = new Matrix<T>(Rows, matrix.Columns);

            // Matrix product
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    // Dot product of values in col
                    T sum = default;
                    for (int k = 0; k < Columns; k++)
                    {
                        dynamic dataA = Data[i, k];
                        dynamic dataB = matrix.Data[k, j];
                        sum += dataA * dataB;
                    }

                    m.Data[i, j] = sum;
                }
            }

            Data = m.Data;
        }

        /// <summary>
        /// Multiplies the matrix by a scalar
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix by</param>
        public void Multiply(T scalar)
        {
            dynamic s = scalar;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Data[i, j] *= s;
                }
            }
        }

        /// <summary>
        /// Adds a number to all elements.
        /// </summary>
        /// <param name="addition">The value to add to each element.</param>
        public void Add(T addition)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Data[i, j] += (dynamic)addition;
                }
            }
        }

        /// <summary>
        /// Adds a matrix to this matrix.
        /// </summary>
        /// <param name="addition">The matrix to add.</param>
        public void Add(Matrix<T> addition)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Data[i, j] += (dynamic)addition.Data[i, j];
                }
            }
        }

        #endregion

        #region Manipulation

        /// <summary>
        /// Sets an individual element to a value.
        /// </summary>
        /// <param name="i">The row of the element.</param>
        /// <param name="j">The column of the element.</param>
        /// <param name="value">The value to set the element equal to.</param>
        public void SetAt(int i, int j, T value)
        {
            Data[i, j] = value;
        }

        /// <summary>
        /// Adds a number to an element.
        /// </summary>
        /// <param name="i">The row of the element.</param>
        /// <param name="j">The column of the element.</param>
        /// <param name="addition">The value to add to the element.</param>
        public void AddAt(int i, int j, T addition)
        {
            Data[i, j] += (dynamic)addition;
        }

        /// <summary>
        /// Sets all elements in matrix to one value.
        /// </summary>
        /// <param name="value">The value to set all elements as.</param>
        public void SetAll(T value)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    SetAt(i, j, value);
                }
            }
        }

        /// <summary>
        /// Maps a function to all elements in a matrix.
        /// </summary>
        /// <param name="function">The function to map.</param>
        public void Map(Func<T, T> function)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    MapAt(i, j, function);
                }
            }
        }

        /// <summary>
        /// Maps a function to an element in a matrix.
        /// </summary>
        /// <param name="i">The row of the element.</param>
        /// <param name="j">The column of the element.</param>
        /// <param name="function">The function to map.</param>
        public void MapAt(int i, int j, Func<T, T> function)
        {
            Data[i, j] = function(Data[i, j]);
        }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        /// <returns>The transposed matrix.</returns>
        public Matrix<T> Transpose()
        {
            Matrix<T> m = new Matrix<T>(Columns, Rows);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    m.Data[j, i] = Data[i, j];
                }
            }

            return m;
        }

        #endregion

        #region Query

        /// <summary>
        /// Finds the first occurrence of a predicate
        /// </summary>
        /// <param name="predicate">The predicate to match</param>
        /// <returns>The row, column and value of the match</returns>
        public Tuple<int, int, T> Find(Func<T, bool> predicate)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (predicate(Data[i, j]))
                        return new Tuple<int, int, T>(i, j, Data[i, j]);
                }
            }

            return null;
        }

        /// <summary>
        /// Finds all occurrences of a predicate
        /// </summary>
        /// <param name="predicate">The predicate to match</param>
        /// <returns>A list of the rows, columns and values of the matches</returns>
        public List<Tuple<int, int, T>> FindAll(Func<T, bool> predicate)
        {
            List<Tuple<int, int, T>> tuples = new List<Tuple<int, int, T>>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (predicate(Data[i, j]))
                        tuples.Add(new Tuple<int, int, T>(i, j, Data[i, j]));
                }
            }

            return tuples;
        }

        /// <summary>
        /// Retrieves a row from the matrix
        /// </summary>
        /// <param name="row">The index of the row that is being retrieved</param>
        /// <returns>The array of values in that row</returns>
        public T[] GetRow(int row)
        {
            if (row < 0 && Rows > row)
                throw new ArgumentOutOfRangeException(nameof(row), $"Parameter 'Row' is invalid - must be a positive integer between 0 and the number of rows (below {Rows})");

            List<T> values = new List<T>();
            for (int i = 0; i < Columns; i++)
            {
                values.Add(Data[row, i]);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Retrieves a column from the matrix
        /// </summary>
        /// <param name="col">The index of the column that is being retrieved</param>
        /// <returns>The array of values in that column</returns>
        public T[] GetColumn(int col)
        {
            if (col < 0 && Columns > col)
                throw new ArgumentOutOfRangeException(nameof(col), $"Parameter 'Col' is invalid - must be a positive integer between 0 and the number of rows (below {Columns})");

            List<T> values = new List<T>();
            for (int i = 0; i < Rows; i++)
            {
                values.Add(Data[i, col]);
            }

            return values.ToArray();
        }

        #endregion

        #region Equals

        /// <summary>
        /// Compares whether two matrices are equal
        /// </summary>
        /// <param name="obj">The matrix to compare</param>
        /// <returns>If the matrices are equal</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix<T>);
        }

        /// <summary>
        /// Compares whether two matrices are equal
        /// </summary>
        /// <param name="other">The matrix to compare</param>
        /// <returns>If the matrices are equal</returns>
        public bool Equals(Matrix<T> other)
        {
            return other != null
                && Rows == other.Rows
                && Columns == other.Columns
                && Match(other)
                && IsSquare == other.IsSquare;
        }

        /// <summary>
        /// Gets the hash code of the object
        /// </summary>
        /// <returns>The hash code of the object</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    hash = (hash * 31) + Convert.ToInt32(Data[i, j]);
                }
            }

            return hash;
        }

        /// <summary>
        /// Compares each value of a different matrix with this matrix
        /// </summary>
        /// <param name="matrix">The matrix to compare</param>
        /// <returns>Whether the matrices are equal</returns>
        private bool Match(Matrix<T> matrix)
        {
            for (int i = 0; i < Data.GetLength(0); i++)
            {
                for (int j = 0; j < Data.GetLength(1); j++)
                {
                    if ((dynamic)matrix.Data[i, j] != Data[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Clone

        /// <summary>
        /// Performs a shallow clone of the matrix.
        /// </summary>
        /// <returns>The cloned matrix</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Displays matrix info in string form.
        /// </summary>
        /// <returns>The string containing information about the matrix.</returns>
        public override string ToString()
        {
            return "Matrix " + Size;
        }

        #endregion

        #region Serialize

#pragma warning disable SYSLIB0011 // Type or member is obsolete

        /// <summary>
        /// Converts the matrix into a serializable object.
        /// </summary>
        /// <returns>The serialized matrix, in memory stream form.</returns>
        public MemoryStream Serialize()
        {
            var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, this);
            return ms;
        }

        /// <summary>
        /// Deserializes a MemoryStream into a Matrix object.
        /// </summary>
        /// <param name="input">The <see cref="MemoryStream"/> containing the serialized matrix data.</param>
        /// <returns>The <see cref="Matrix{T}"/> object.</returns>
        public static Matrix<T> Deserialize(MemoryStream input)
        {
            Numerics.ThrowIfNotNumber<T>();
            var formatter = new BinaryFormatter();
            input.Seek(0, SeekOrigin.Begin);
            return (Matrix<T>)formatter.Deserialize(input);
        }

#pragma warning restore SYSLIB0011 // Type or member is obsolete

        #endregion

        #region Static Arithmetic

        /// <summary>
        /// Multiplies two matrices together
        /// </summary>
        /// <param name="matrixA">The first matrix to multiply</param>
        /// <param name="matrixB">The second matrix to multiply</param>
        /// <returns>The product matrix</returns>
        public static Matrix<T> Multiply(Matrix<T> matrixA, Matrix<T> matrixB)
        {
            // Check that the two matrices are valid
            if (matrixA.Columns != matrixB.Rows)
                throw new InvalidOperationException("The matrices are incompatible for multiplication");

            Matrix<T> m = new Matrix<T>(matrixA.Rows, matrixB.Columns);

            // Matrix product
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    // Dot product of values in col
                    T sum = default;
                    for (int k = 0; k < matrixA.Columns; k++)
                    {
                        dynamic dataA = matrixA.Data[i, k];
                        dynamic dataB = matrixB.Data[k, j];
                        sum += dataA * dataB;
                    }

                    m.Data[i, j] = sum;
                }
            }

            return m;
        }

        /// <summary>
        /// Adds a number to all elements.
        /// </summary>
        /// <param name="matrix">The matrix to apply the change to.</param>
        /// <param name="addition">The value to add to each element.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> Add(Matrix<T> matrix, T addition)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.Add(addition);
            return m;
        }

        /// <summary>
        /// Adds a matrix to this matrix.
        /// </summary>
        /// <param name="matrix">The matrix to apply the change to.</param>
        /// <param name="addition">The matrix to add.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> Add(Matrix<T> matrix, Matrix<T> addition)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.Add(addition);
            return m;
        }

        /// <summary>
        /// Multiplies the matrix by a scalar.
        /// </summary>
        /// <param name="matrix">The matrix to apply the scalar to.</param>
        /// <param name="scalar">The scalar to multiply the matrix by.</param>
        /// <returns>The matrix with the applied scalar product.</returns>
        public static Matrix<T> Multiply(Matrix<T> matrix, T scalar)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.Multiply(scalar);
            return m;
        }

        #endregion

        #region Static Manipulation

        /// <summary>
        /// Sets an individual element to a value.
        /// </summary>
        /// <param name="matrix">The matrix to apply the change to.</param>
        /// <param name="i">The row of the element.</param>
        /// <param name="j">The column of the element.</param>
        /// <param name="value">The value to set the element equal to.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> SetAt(Matrix<T> matrix, int i, int j, T value)
        {
            var m = matrix.Clone() as Matrix<T>;
            matrix.SetAt(i, j, value);
            return m;
        }

        /// <summary>
        /// Adds a number to an element.
        /// </summary>
        /// <param name="matrix">The matrix to apply the change to.</param>
        /// <param name="i">The row of the element.</param>
        /// <param name="j">The column of the element.</param>
        /// <param name="addition">The value to add to the element.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> AddAt(Matrix<T> matrix, int i, int j, T addition)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.AddAt(i, j, addition);
            return m;
        }

        /// <summary>
        /// Sets all elements in matrix to one value.
        /// </summary>
        /// <param name="matrix">The matrix to apply the change to.</param>
        /// <param name="value">The value to set all elements as.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> SetAll(Matrix<T> matrix, T value)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.SetAll(value);
            return m;
        }

        /// <summary>
        /// Maps a function to all elements in a matrix.
        /// </summary>
        /// <param name="matrix">The matrix to apply the change to.</param>
        /// <param name="function">The function to map.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> Map(Matrix<T> matrix, Func<T, T> function)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.Map(function);
            return m;
        }

        /// <summary>
        /// Maps a function to an element in a matrix.
        /// </summary>
        /// <param name="matrix">The matrix to apply the scalar to.</param>
        /// <param name="i">The row of the element.</param>
        /// <param name="j">The column of the element.</param>
        /// <param name="function">The function to map.</param>
        /// <returns>The altered matrix.</returns>
        public static Matrix<T> MapAt(Matrix<T> matrix, int i, int j, Func<T, T> function)
        {
            var m = matrix.Clone() as Matrix<T>;
            m.MapAt(i, j, function);
            return m;
        }

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="matrix">The matrix to apply the transformation to.</param>
        /// <returns>The transposed matrix.</returns>
        public static Matrix<T> Transpose(Matrix<T> matrix)
        {
            return matrix.Transpose();
        }

        #endregion
    }
}
