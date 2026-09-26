using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Parser.Expressions
{
    internal class UnaryExpression : Expression
    {
        public SymbolType Operator { get; }
        public Expression Operand { get; }

        public UnaryExpression(SymbolType @operator, Expression operand)
        {
            Operator = @operator;
            Operand = operand;
        }
    }
}
