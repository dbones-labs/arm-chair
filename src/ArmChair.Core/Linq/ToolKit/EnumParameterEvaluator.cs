namespace IQToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class EnumParameterEvaluator
    {
        public static Expression Eval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            return SubtreeEvaluator.Eval(Nominator.Nominate(fnCanBeEvaluated, expression), expression);
        }

        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression Eval(Expression expression)
        {
            return Eval(expression, CanBeEvaluatedLocally);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType == ExpressionType.Convert;
        }

        /// <summary>
        /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
        /// </summary>
        class SubtreeEvaluator : ExpressionVisitor
        {
            HashSet<Expression> candidates;

            private SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }

            internal static Expression Eval(HashSet<Expression> candidates, Expression exp)
            {
                return new SubtreeEvaluator(candidates).Visit(exp);
            }

            public override Expression Visit(Expression exp)
            {
                if (exp == null)
                {
                    return null;
                }
                if (this.candidates.Contains(exp))
                {
                    return this.Evaluate(exp);
                }
                return base.Visit(exp);
            }

            private Expression Evaluate(Expression e)
            {
                Type type = e.Type;
                if (e.NodeType == ExpressionType.Convert)
                {
                    // check for unnecessary convert & strip them
                    var u = (UnaryExpression)e;
                    if (u.Operand.Type.GetTypeInfo().IsEnum)
                    {
                        e = ((UnaryExpression)e).Operand;
                    }
                }
                return e;
            }
        }

        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        class Nominator : ExpressionVisitor
        {
            readonly Func<Expression, bool> _fnCanBeEvaluated;
            readonly HashSet<Expression> _candidates;
            bool _cannotBeEvaluated;

            private Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                _candidates = new HashSet<Expression>();
                _fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal static HashSet<Expression> Nominate(Func<Expression, bool> fnCanBeEvaluated, Expression expression)
            {
                Nominator nominator = new Nominator(fnCanBeEvaluated);
                nominator.Visit(expression);
                return nominator._candidates;
            }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                return base.VisitConstant(c);
            }

            public override Expression Visit(Expression expression)
            {
                if (expression == null) return expression;

                _cannotBeEvaluated = false;
                base.Visit(expression);

                if (!_cannotBeEvaluated)
                {
                    if (_fnCanBeEvaluated(expression))
                    {
                        _candidates.Add(expression);
                    }
                }

                return expression;
            }
        }
    }
}