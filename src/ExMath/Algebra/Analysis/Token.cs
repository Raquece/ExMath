using static ExMath.Algebra.Expression;

namespace ExMath.Algebra.Analysis
{
    /// <summary>
    /// The type of token.
    /// </summary>
    public enum LexicalToken
    {
        NULL,
        NUMERIC_LITERAL,
        OPERATOR,
        OPEN_PARANTHESIS,
        CLOSE_PARANTESIS,
        FUNCTION,
        NUMERIC_UNKNOWN
    }

    /// <summary>
    /// The precendence of an operator in an equation.
    /// </summary>
    public enum OperatorPrecendence
    {
        NULL,
        ADDITION,
        MULTIPLICATION,
        INDEX,
        BRACKET
    }

    /// <summary>
    /// A token which forms a part of an expression.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Initialises a new instance of a token which forms part of an expression.
        /// </summary>
        /// <param name="token">The type represented in this token.</param>
        /// <param name="data">The underlying data of the token.</param>
        /// <param name="precendence">The precendence of the operator represented in this token (if applicable).</param>
        public Token(LexicalToken token, object? data, OperatorPrecendence precendence = OperatorPrecendence.NULL)
            => (LexicalToken, Data, Precendence) = (token, data, precendence);

        /// <summary>
        /// The type represented in this token.
        /// </summary>
        public LexicalToken LexicalToken { get; set; }

        /// <summary>
        /// The underlying data of the token.
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// The precendence of the operator represented in this token (if applicable).
        /// </summary>
        public OperatorPrecendence Precendence { get; set; }

        /// <summary>
        /// The number of operands used in this operator (if applicable).
        /// </summary>
        public int OperandCount { get; private set; } = 2;

        /// <summary>
        /// Gets whether this token is an operator.
        /// </summary>
        public bool IsOperator => Precendence != OperatorPrecendence.NULL;

        /// <summary>
        /// Returns a string that represents this token.
        /// </summary>
        /// <returns>The string format of the token.</returns>
        public override string ToString()
        {
            return Data == null ? "NULL" : Data.ToString();
        }
    }
}
