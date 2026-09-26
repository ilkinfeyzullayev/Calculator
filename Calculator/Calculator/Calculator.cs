using System;
using System.Collections.Generic;
using System.Text;

using Calculator.Parser.Expressions;

namespace Calculator;

internal class Calculator
{
    public decimal Calculate(string input)
    {
        Lexer.Lexer lexer = new(input);
        lexer.Lex();

        Parser.Parser parser = new(lexer.TokenList);
        Expression ast = parser.Parse();

        return Evaluator.Evaluator.Evaluate(ast);
    }
}
