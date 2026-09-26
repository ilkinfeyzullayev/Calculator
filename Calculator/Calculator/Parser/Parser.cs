using Calculator.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Parser
{
    internal class Parser
    {
        private readonly List<Token> TokenList;
        private int _position;

        private Token Current => TokenList[_position];
        private bool IsAtEnd => Current is EndOfLine;

        public Parser(List<Token> tokens)
        {
            TokenList = tokens;
        }

        public Expression Parse()
        {
            Expression expression = ParseExpression();

            if (!IsAtEnd)
                throw new Exception("Unexpected token.");

            return expression;
        }

        private Expression ParsePrimary()
        {
            switch (Current)
            {
                case Number number:
                    Advance();
                    return new LiteralExpression(number.Value);

                case Symbol symbol when symbol.SymbolType == SymbolType.LeftParen:
                    Advance();
                    return ParseParenthesizedExpression();

                default:
                    throw new Exception("Expected an expression.");
            }
        }

        private Expression ParseParenthesizedExpression()
        {
            Expression expression = ParseExpression();

            ExpectSymbol(SymbolType.RightParen);

            return expression;
        }

        private Expression ParseUnary()
        {
            switch (Current)
            {
                case Symbol symbol when symbol.SymbolType == SymbolType.Minus:
                    Advance();

                    return new UnaryExpression(
                        SymbolType.Minus,
                        ParseUnary());

                case Symbol symbol when symbol.SymbolType == SymbolType.Plus:
                    Advance();

                    return new UnaryExpression(
                        SymbolType.Plus,
                        ParseUnary());

                default:
                    return ParsePower();
            }
        }

        private Expression ParsePower()
        {
            Expression left = ParsePrimary();

            if (Current is Symbol sym && sym.SymbolType == SymbolType.Power)
            {
                Advance();

                Expression right = ParseUnary();

                return new BinaryExpression(
                    left,
                    SymbolType.Power,
                    right);
            }

            return left;
        }

        private Expression ParseMultiplication()
        {
            Expression left = ParseUnary();

            while (Current is Symbol sym
                && (sym.SymbolType == SymbolType.Multiply
                    || sym.SymbolType == SymbolType.Divide))
            {
                SymbolType @operator = sym.SymbolType;

                Advance();

                Expression right = ParseUnary();

                left = new BinaryExpression(
                    left,
                    @operator,
                    right);
            }

            return left;
        }

        private Expression ParseAddition()
        {
            Expression left = ParseMultiplication();

            while (Current is Symbol sym
                && (sym.SymbolType == SymbolType.Plus
                    || sym.SymbolType == SymbolType.Minus))
            {
                SymbolType @operator = sym.SymbolType;

                Advance();

                Expression right = ParseMultiplication();

                left = new BinaryExpression(
                    left,
                    @operator,
                    right);
            }

            return left;
        }

        private Expression ParseExpression()
        {
            return ParseAddition();
        }

        private void ExpectSymbol(SymbolType symbolType)
        {
            if (Current is Symbol symbol &&
                symbol.SymbolType == symbolType)
            {
                Advance();
                return;
            }

            throw new Exception($"Expected {symbolType}.");
        }

        private void Advance()
        {
            if (_position < TokenList.Count)
                _position++;
        }
    }
}
