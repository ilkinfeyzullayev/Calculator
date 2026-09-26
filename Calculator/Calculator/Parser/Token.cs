using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Parser
{
    internal enum TokenType
    {
        Number,
        Symbol,
        EndOfLine
    }

    internal abstract record Token(TokenType Type);

    internal record Number(decimal Value) : Token(TokenType.Number);

    internal record Symbol(SymbolType SymbolType) : Token(TokenType.Symbol);
    internal record EndOfLine() : Token(TokenType.EndOfLine);
}
