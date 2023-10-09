using ExMath.Algebra.Analysis;
using ExMath.Calculus;
using ExMath.Calculus.Functions;
using ExMath.Calculus.Operators;
using ExMath.Discrete.Graphs;
using ExMath.Exceptions;
using System.Net;
using System.Text.RegularExpressions;

namespace ExMath.Algebra
{
    /// <summary>
    /// A mathematical expresion represented in a parse tree format.
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// Gets the list of tokens devised from lexical analysis of this expression.
        /// </summary>
        public List<Token>? LexicalTokens { get; private set; }

        /// <summary>
        /// The parse tree of this expression.
        /// </summary>
        public Tree<Token>? Tree { get; internal set; }

        /// <summary>
        /// Performs lexical analysis of a string representing an equation.
        /// </summary>
        /// <param name="equation">The string of the equation.</param>
        /// <returns>The list of tokens from the analysis.</returns>
        /// <exception cref="Exception">Thrown if an unknown error occurs.</exception>
        internal static List<Token> LexicalAnalysis(string equation)
        {
            var tokensStr = equation.Split(' ');

            List<Token> tokensInfix = new();

            foreach (var token in tokensStr)
            {
                for (int i = 0; i < token.Length; i++)
                {
                    char c = token[i];

                    if (c == '(')
                    {
                        tokensInfix.Add(new Token(LexicalToken.OPEN_PARANTHESIS, null));
                    }
                    else if (c == ')')
                    {
                        tokensInfix.Add(new Token(LexicalToken.CLOSE_PARANTESIS, null));
                    }
                    else if (char.IsLetter(c))
                    {
                        // Matches the next series of alphabetic characters (the identifier)
                        Regex r = new(@"[A-Za-z]+");
                        string identifier = r.Match(token[i..]).Value;

                        tokensInfix.Add(new Token(LexicalToken.NUMERIC_UNKNOWN, new NumericVariable(identifier)));

                        // Pushes the pointer to after the appearance of this text identifier
                        i += identifier.Length - 1;
                    }
                    else if (char.IsDigit(c))
                    {
                        // Matches the next series of numeric characters and decimal points (the value)
                        Regex r = new(@"[\d.]+");
                        string valueStr = r.Match(token[i..]).Value;

                        if (double.TryParse(valueStr, out double value))
                        {
                            tokensInfix.Add(new Token(LexicalToken.NUMERIC_LITERAL, value));
                        }

                        // Pushes the pointer to after the appearance of this text identifier
                        i += valueStr.Length - 1;
                    }
                    else if (c == '+' || c == '-' || c == '/' || c == '*' || c == '^')
                    {
                        OperatorPrecendence p = OperatorPrecendence.NULL;
                        IFunction f;

                        // Get the precendence of the operator
                        switch (c)
                        {
                            case '+':
                                p = OperatorPrecendence.ADDITION;
                                f = new AddOperator();
                                break;
                            case '-':
                                p = OperatorPrecendence.ADDITION;
                                f = new SubtractOperator();
                                break;
                            case '/':
                                p = OperatorPrecendence.MULTIPLICATION;
                                f = new DivideOperator();
                                break;
                            case '*':
                                p = OperatorPrecendence.MULTIPLICATION;
                                f = new MultiplyOperator();
                                break;
                            case '^':
                                p = OperatorPrecendence.INDEX;
                                f = new PowerOperator();
                                break;
                            default:
                                throw new Exception("Unknown error occured");
                        }

                        tokensInfix.Add(new Token(LexicalToken.OPERATOR, f, p));
                    }
                    else
                    {
                        throw new Exception("Unknown error occured");
                    }
                }
            }

            // Convert from infix notation to postfix notation
            List<Token> tokens = new();
            Stack<Token> stack = new();

            foreach (var token in tokensInfix)
            {

                if (token.LexicalToken == LexicalToken.NUMERIC_UNKNOWN || token.LexicalToken == LexicalToken.NUMERIC_LITERAL)
                {
                    tokens.Add(token);
                }
                else if (token.LexicalToken == LexicalToken.OPEN_PARANTHESIS)
                {
                    stack.Push(token);
                }
                else if (token.LexicalToken == LexicalToken.CLOSE_PARANTESIS)
                {
                    while (stack.Peek().LexicalToken != LexicalToken.OPEN_PARANTHESIS)
                    {
                        tokens.Add(stack.Pop());
                    }

                    _ = stack.Pop(); // Remove paranethesis from stack
                }
                else
                {
                    while (stack.Count != 0 && token.Precendence <= stack.Peek().Precendence)
                    {
                        tokens.Add(stack.Pop());
                    }

                    stack.Push(token);
                }
            }

            while (stack.Count != 0)
            {
                tokens.Add(stack.Pop());
            }

            return tokens;
        }

        /// <summary>
        /// Parses an expression from a string.
        /// </summary>
        /// <param name="expression">The string of the equation to parse.</param>
        /// <returns>The parsed expression.</returns>
        public static Expression Parse(string expression)
        {
            Expression e = new();

            var tokens = LexicalAnalysis(expression);
            Stack<Tree<Token>> stack = new();

            foreach (var token in tokens)
            {
                if (!token.IsOperator)
                {
                    stack.Push(new Tree<Token>(token, null));
                }
                else
                {
                    Tree<Token>[] operands = new Tree<Token>[token.OperandCount];

                    // Adds each operand from the stack in reverse order (right to left)
                    for (int i = 0; i < token.OperandCount; i++)
                    {
                        operands[i] = stack.Pop();
                    }

                    // Adds each operand from the stack in correct order (left to right)
                    stack.Push(new Tree<Token>(token, operands.Reverse().ToArray()));
                }
            }

            // Create tree from tree node
            var node = stack.Pop();

            e.Tree = node;
            e.LexicalTokens = tokens;

            return e;
        }

        /// <summary>
        /// Evaluate the expression.
        /// </summary>
        /// <returns>The resulting value of the evaluation.</returns>
        public dynamic Evaluate()
        {
            return Evaluate(null);
        }

        /// <summary>
        /// Evaluates the expression using a dictionary of values for numeric variables.
        /// </summary>
        /// <param name="variables">The dictionaries of values for numeric variables.</param>
        /// <returns>The resulting value of the evaluation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if an expression tree has not yet been created.</exception>
        public dynamic Evaluate(Dictionary<NumericVariable, dynamic>? variables)
        {
            if (Tree == null)
            {
                throw new InvalidOperationException("Expression tree not formed");
            }

            return ResolveBranch(Tree, variables);
        }

        /// <summary>
        /// Simplifies this expression by evaluating each branch that can be evaluated.
        /// </summary>
        public void Simplify()
        {
            Simplify(null);
        }

        /// <summary>
        /// Simplifies this expression by evaluating each branch that can be evaluated and by using a dictionary of each unknown's value.
        /// </summary>
        /// <param name="variables">The dictionaries of values for numeric variables.</param>
        /// <exception cref="InvalidOperationException">Thrown if the expression tree is not formed.</exception>
        public void Simplify(Dictionary<NumericVariable, dynamic>? variables)
        {
            if (Tree == null)
            {
                throw new InvalidOperationException("Expression tree not formed");
            }

            SimplifyBranch(Tree, variables);
        }

        /// <summary>
        /// Adds an operation to the expression.
        /// </summary>
        /// <param name="operation">The operation to add.</param>
        /// <param name="position">The position of the current expression in the operand order.</param>
        /// <param name="operands">The extra operands to add to this equation.</param>
        /// <exception cref="InvalidOperationException">Thrown if the expression tree has not yet been formed.</exception>
        /// <exception cref="ArgumentNullException">Thrown if token is null.</exception>
        /// <exception cref="ArgumentException">Thrown if token is not a valid operation.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if position is not within the boundaries of the operands array.</exception>
        public void AddOperation(Token operation, int position, params Token[] operands)
        {
            if (Tree == null)
            {
                throw new InvalidOperationException("Expression tree has not been formed.");
            }

            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Token cannot be null");
            }

            if (!operation.IsOperator)
            {
                throw new ArgumentException("Token must be an operator or a function.", nameof(operation));
            }

            if (position >= operation.OperandCount)
            {
                throw new ArgumentOutOfRangeException(nameof(position), "Position must be within the boundaries of the operands array.");
            }

            if (operands.Length + 1 != operation.OperandCount)
            {
                throw new ArgumentOutOfRangeException(nameof(operands), $"The total number of operands must equal {operation.OperandCount}");
            }

            Tree<Token> newBase = new(operation);
            newBase.Nodes = new Tree<Token>[operation.OperandCount];
            newBase.Nodes[position] = Tree;

            int i = 0;
            for (int j = 0; j < operation.OperandCount; j++)
            {
                if (newBase.Nodes[j] == null)
                {
                    newBase.Nodes[j] = new Tree<Token>(operands[i]);
                    i++;
                }
            }

            Tree = newBase;
        }

        /// <summary>
        /// Evaluates a branch of the expression tree.
        /// </summary>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="variables">The dictionaries of values for numeric variables.</param>
        /// <returns>The resulting value of the evaluation.</returns>
        private dynamic ResolveBranch(Tree<Token> node, Dictionary<NumericVariable, dynamic>? variables)
        {
            if (node.Data.IsOperator)
            {
                IFunction p = (IFunction)node.Data.Data;
                return p.Evaluate(variables, node.Nodes.Select(n => ResolveBranch(n, variables)).ToArray());
            }
            else
            {
                return node.Data.Data;
            }
        }

        /// <summary>
        /// Simplifies a branch of the expression tree.
        /// </summary>
        /// <param name="node">The node to simplify.</param>
        /// <param name="variables">The dictionaries of values for numeric variables.</param>
        private void SimplifyBranch(Tree<Token> node, Dictionary<NumericVariable, dynamic>? variables)
        {
            if (node.Data.IsOperator)
            {
                var operatorNodes = node.Nodes.Where(n => n.Data.IsOperator);

                foreach (var n in operatorNodes)
                {
                    SimplifyBranch(n, variables);
                }

                IFunction p = (IFunction)node.Data.Data;

                try
                {
                    node.Data.Data = ResolveBranch(node, variables);
                        // p.Evaluate(variables, node.Nodes.Select(n => n.Data.Data).ToArray());
                    node.Data.Precendence = OperatorPrecendence.NULL;
                    node.Data.LexicalToken = LexicalToken.NUMERIC_LITERAL;
                    node.Nodes = null;
                }
                catch (UnknownNumericVariableException e)
                {
                    // Branch cannot be simplified further
                }
            }
        }
    }
}
