using Calculator.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Lexer
{
    internal class Lexer
    {
        public List<Token> TokenList { get; } = new();

        private readonly string _source;
        private int _currentIndex;

        public Lexer(string source)
        {
            _source = source;
        }

        public void Lex()
        {
            while (_currentIndex < _source.Length)
            {
                char current = _source[_currentIndex];

                if (char.IsWhiteSpace(current))
                {
                    _currentIndex++;
                }
                else if (char.IsDigit(current) || current == '.')
                {
                    TokenList.Add(LexNumber());
                }
                else
                {
                    TokenList.Add(LexSymbol());
                }
            }

            TokenList.Add(new EndOfLine());
        }

        private Number LexNumber()
        {
            int start = _currentIndex;

            while (_currentIndex < _source.Length &&
                   (char.IsDigit(_source[_currentIndex]) ||
                    _source[_currentIndex] == '.'))
            {
                _currentIndex++;
            }

            string value = _source[start.._currentIndex];

            if (!decimal.TryParse(value, out decimal number))
            {
                throw new Exception($"Invalid number: {value}");
            }

            return new Number(number);
        }

        private Symbol LexSymbol()
        {
            SymbolType symbol = _source[_currentIndex] switch
            {
                '+' => SymbolType.Plus,
                '-' => SymbolType.Minus,
                '*' => SymbolType.Multiply,
                '/' => SymbolType.Divide,
                '^' => SymbolType.Power,
                '(' => SymbolType.LeftParen,
                ')' => SymbolType.RightParen,

                _ => throw new Exception(
                    $"Unexpected character: {_source[_currentIndex]}")
            };

            _currentIndex++;

            return new Symbol(symbol);
        }
    }
}
