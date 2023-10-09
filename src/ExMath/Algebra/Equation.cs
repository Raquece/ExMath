using ExMath.Algebra.Analysis;
using ExMath.Calculus;
using ExMath.Discrete.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Algebra
{
    /// <summary>
    /// Represents an equation of two expressions.
    /// </summary>
    public class Equation
    {
        public Equation(Expression lhs, Expression rhs)
        {
            (LHS, RHS) = (lhs, rhs);

            unknownCount = GetUnknownOccurences();
        }

        /// <summary>
        /// Gets or sets the left hand side of this equation.
        /// </summary>
        public Expression LHS { get; private set; }

        /// <summary>
        /// Gets or sets the right hand side of this equation.
        /// </summary>
        public Expression RHS { get; private set; }

        /// <summary>
        /// The number of occurences of each unknown variable in this equation.
        /// </summary>
        private readonly Dictionary<NumericVariable, int> unknownCount;

        /// <summary>
        /// Gets the number of occurences of each unknown variable in this equation.
        /// </summary>
        public IReadOnlyDictionary<NumericVariable, int> UnknownCount => unknownCount.AsReadOnly();

        /// <summary>
        /// Solves this equation for a given variable, assuming that variable only occurs once in the equation.
        /// </summary>
        /// <param name="solvingParameter">The variable to solve for.</param>
        /// <returns>The value of the variable.</returns>
        public dynamic SimpleSolve(NumericVariable solvingParameter)
        {
            return SimpleSolve(solvingParameter, null);
        }

        /// <summary>
        /// Solves this equation for a given variable, assuming that variable only occurs once in the equation.
        /// </summary>
        /// <param name="solvingParameter">The variable to solve for.</param>
        /// <param name="unknowns">Dictionary containing the values for any unknowns occuring in the equation.</param>
        /// <returns>The value of the variable.</returns>
        public dynamic SimpleSolve(NumericVariable solvingParameter, Dictionary<NumericVariable, dynamic>? unknowns)
        {
            if (unknowns != null && unknowns!.ContainsKey(solvingParameter))
            {
                throw new InvalidOperationException("Cannot solve for a variable that has been set a value.");
            }

            if (!UnknownCount.ContainsKey(solvingParameter))
            {
                throw new InvalidOperationException("Solving parameter not present in equation.");
            }

            if (UnknownCount.Count != 1 || UnknownCount[solvingParameter] != 1)
            {
                throw new InvalidOperationException("The simple solve requires 1 occurence of 1 variable to solve.");
            }

            LHS.Simplify();
            RHS.Simplify();

            Tree<Token>? startingNodeLeft = Tree<Token>.FindParent(LHS.Tree, t => IsSolvingParameterIsToken(t, solvingParameter));
            Tree<Token>? startingNodeRight = Tree<Token>.FindParent(RHS.Tree, t => IsSolvingParameterIsToken(t, solvingParameter));
            bool isOnLeft = startingNodeLeft != null;

            // Tree is already representing the equation for the solution of the variable
            if (startingNodeLeft == null && startingNodeRight == null)
            {
                if (LHS.Tree.Data.LexicalToken == LexicalToken.NUMERIC_UNKNOWN)
                {
                    return RHS.Evaluate();
                }
                else if (RHS.Tree.Data.LexicalToken == LexicalToken.NUMERIC_UNKNOWN)
                {
                    return LHS.Evaluate();
                }
                else
                {
                    throw new InvalidOperationException("Solving parameter could not be found.");
                }
            }

            Tree<Token>? parentNode = isOnLeft ? startingNodeLeft : startingNodeRight;

            // Keep applying inverse rules until LHS or RHS contains only the solving parameter
            while (isOnLeft
                ? !(LHS.Tree.Data.LexicalToken == LexicalToken.NUMERIC_UNKNOWN && (NumericVariable)LHS.Tree.Data.Data == solvingParameter)
                : !(RHS.Tree.Data.LexicalToken == LexicalToken.NUMERIC_UNKNOWN && (NumericVariable)RHS.Tree.Data.Data == solvingParameter))
            {
                if (parentNode == null)
                {
                    throw new InvalidOperationException("Unable to locate parent node");
                }

                if (parentNode.Nodes == null)
                {
                    throw new InvalidOperationException("Parent node has no child nodes");
                }

                parentNode = isOnLeft ? LHS.Tree : RHS.Tree;

                if (parentNode.Data.Data is IFunction func)
                {
                    if (isOnLeft)
                    {
                        RHS.AddOperation(new Token(
                            LexicalToken.FUNCTION,
                            func.InverseFunction,
                            OperatorPrecendence.BRACKET),
                            0,
                            parentNode.Nodes.Where(t => !(IsSolvingParameterIsToken(t.Data, solvingParameter) || t.Data.IsOperator)) // Add all non solving parameter operands
                                .Select(n => n.Data)
                                .ToArray());
                        RHS.Simplify();
                    }
                    else
                    {
                        LHS.AddOperation(new Token(
                            LexicalToken.FUNCTION,
                            func.InverseFunction,
                            OperatorPrecendence.BRACKET),
                            0,
                            parentNode.Nodes.Where(t => !(IsSolvingParameterIsToken(t.Data, solvingParameter) || t.Data.IsOperator)) // Add all non solving parameter operands
                                .Select(n => n.Data)
                                .ToArray());

                        LHS.Simplify();
                    }

                    // Remove topmost operation from the tree
                    if (isOnLeft)
                    {
                        LHS.Tree = LHS.Tree.Nodes.First(t => t.Data.IsOperator || t.Data.LexicalToken == LexicalToken.NUMERIC_UNKNOWN);
                    }
                    else
                    {
                        RHS.Tree = RHS.Tree.Nodes.First(t => t.Data.IsOperator || t.Data.LexicalToken == LexicalToken.NUMERIC_UNKNOWN);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Could not apply inverse function on parent node of unknown");
                }
            }

            return isOnLeft ? RHS.Evaluate() : LHS.Evaluate();
        }

        /// <summary>
        /// Creates a dictionary containing the number of occurences of each unknown variable in the sets of equations
        /// </summary>
        /// <returns>The dictionary of variables and their occurences.</returns>
        /// <exception cref="ArgumentNullException">Throws if either expression tree is not formed.</exception>
        private Dictionary<NumericVariable, int> GetUnknownOccurences()
        {
            var dict = new Dictionary<NumericVariable, int>();

            if (LHS.Tree == null)
            {
                throw new ArgumentNullException(nameof(LHS.Tree), "LHS expression tree not formed.");
            }

            if (RHS.Tree == null)
            {
                throw new ArgumentNullException(nameof(RHS.Tree), "RHS expression tree not formed.");
            }

            GetUnknownOccurencesBranch(dict, LHS.Tree);
            GetUnknownOccurencesBranch(dict, RHS.Tree);

            return dict;
        }

        /// <summary>
        /// Adds a branch of an expression tree to the dictionary of occurences.
        /// </summary>
        /// <param name="count">The current dictionary of occurences.</param>
        /// <param name="branch">The branch to evaluate.</param>
        private void GetUnknownOccurencesBranch(Dictionary<NumericVariable, int> count, Tree<Token> branch)
        {
            if (branch.Data.Data is NumericVariable v)
            {
                if (count.ContainsKey(v))
                {
                    count[v]++;
                }
                else
                {
                    count.Add(v, 1);
                }
            }

            if (branch.Nodes != null)
            {
                foreach (var node in branch.Nodes)
                {
                    GetUnknownOccurencesBranch(count, node);
                }
            }
        }

        /// <summary>
        /// Gets whether the variable in a token is equal to a variable.
        /// </summary>
        /// <param name="token">The token to query.</param>
        /// <param name="solvingVariable">The variable to compare to.</param>
        /// <returns></returns>
        private bool IsSolvingParameterIsToken(Token token, NumericVariable solvingVariable)
        {
            return token.LexicalToken == LexicalToken.NUMERIC_UNKNOWN
                && (NumericVariable)token.Data == solvingVariable;
        }
    }
}
