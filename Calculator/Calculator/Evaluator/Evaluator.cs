using Calculator.Parser;
using Calculator.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Evaluator
{
    internal class Evaluator
    {
        public static decimal Evaluate(Expression expression)
        {
            return expression switch
            {
                LiteralExpression number =>
                    number.Value,

                UnaryExpression unary =>
                    EvaluateUnary(unary),

                BinaryExpression binary =>
                    EvaluateBinary(binary),

                _ => throw new Exception(
                    $"Unknown expression type: {expression.GetType().Name}")
            };
        }

        private static decimal EvaluateUnary(UnaryExpression expression)
        {
            decimal operand = Evaluate(expression.Operand);

            return expression.Operator switch
            {
                SymbolType.Plus => operand,
                SymbolType.Minus => -operand,

                _ => throw new Exception(
                    $"Unsupported unary operator: {expression.Operator}")
            };
        }

        private static decimal EvaluateBinary(BinaryExpression expression)
        {
            decimal left = Evaluate(expression.Left);
            decimal right = Evaluate(expression.Right);

            return expression.Operator switch
            {
                SymbolType.Plus => left + right,
                SymbolType.Minus => left - right,
                SymbolType.Multiply => left * right,
                SymbolType.Divide => left / right,
                SymbolType.Power => Power(left, right),

                _ => throw new Exception(
                    $"Unsupported binary operator: {expression.Operator}")
            };
        }

        private static decimal Power(decimal left, decimal right)
        {
            double result = Math.Pow(
                (double)left,
                (double)right);

            return (decimal)result;
        }
    }
}
