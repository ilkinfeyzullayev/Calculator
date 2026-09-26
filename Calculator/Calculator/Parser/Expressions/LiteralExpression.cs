using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Parser.Expressions
{
    internal class LiteralExpression : Expression
    {
        public decimal Value { get; }

        public LiteralExpression(decimal value)
        {
            Value = value;
        }
    }
}
