using System;
using System.Collections.Generic;
using System.Linq;
using Piglet.Parser;

public static class Wordy
{
    public static int Answer(string input)
    {
        try
        {
            return (int)Parser.Parse(input);
        }
        catch (Exception ex)
        {
            switch(ex)
            {
                case Piglet.Lexer.LexerException lex:
                case Piglet.Parser.ParseException pex:
                    throw new ArgumentException();
            }
            throw;
        }
    }
    
    private static IParser<object> Parser;

    static Wordy()
    {
        var config = ParserFactory.Fluent();
        var sentence = config.Rule();
        var expr = config.Rule();
        var number = config.Rule();
        var op = config.Rule();

        sentence.IsMadeUp.By("What is ").Followed.By(expr).As("Expression").Followed.By("?").WhenFound(f => f.Expression);

        expr.IsMadeUp.By(expr).As("Left").Followed.By(" ").Followed.By(op).As("Operator").Followed.By(" ").Followed.By(number).As("Right")
            .WhenFound(f => {
                switch(f.Operator)
                {
                    case '+': return f.Left + f.Right;
                    case '-': return f.Left - f.Right;
                    case '*': return f.Left * f.Right;
                    case '/': return f.Left / f.Right;
                }
                throw new InvalidOperationException();
            })
            .Or.By(number);

        op.IsMadeUp.By("plus").WhenFound(f => '+')
            .Or.By("minus").WhenFound(f => '-')
            .Or.By("multiplied by").WhenFound(f => '*')
            .Or.By("divided by").WhenFound(f => '/');

        number.IsMadeUp.By<int>();

        Parser = config.CreateParser();
    }
}

