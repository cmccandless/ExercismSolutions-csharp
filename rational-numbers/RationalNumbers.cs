using System;
// using System.Diagnostics;

public static class RealNumberExtension
{
    public static double Expreal(this int realNumber, RationalNumber r)
    {
        return r.Expreal(realNumber);
    }

    public static int GCD(this int a, int b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }
        return a == 0 ? b : a;
    }
}

public struct RationalNumber
{
    internal int numerator, denominator;
    public RationalNumber(int numerator, int denominator)
    {
        int gcd = Math.Abs(numerator).GCD(Math.Abs(denominator));
        this.numerator = Math.Abs(numerator) / gcd;
        this.denominator = Math.Abs(denominator) / gcd;
        if (numerator < 0 != denominator < 0)
            this.numerator *= -1;
    }

    public RationalNumber Add(RationalNumber r)
    {
        return new RationalNumber(
            numerator * r.denominator + r.numerator * denominator,
            denominator * r.denominator
        );
    }

    public static RationalNumber operator +(RationalNumber r1, RationalNumber r2)
    {
        return r1.Add(r2);
    }

    public RationalNumber Sub(RationalNumber r)
    {
        return new RationalNumber(
            numerator * r.denominator - r.numerator * denominator,
            denominator * r.denominator
        );
    }

    public static RationalNumber operator -(RationalNumber r1, RationalNumber r2)
    {
        return r1.Sub(r2);
    }

    public RationalNumber Mul(RationalNumber r)
    {
        return new RationalNumber(
            numerator * r.numerator,
            denominator * r.denominator
        );
    }

    public static RationalNumber operator *(RationalNumber r1, RationalNumber r2)
    {
        return r1.Mul(r2);
    }

    public RationalNumber Div(RationalNumber r)
    {
        return new RationalNumber(
            numerator * r.denominator,
            denominator * r.numerator
        );
    }

    public static RationalNumber operator /(RationalNumber r1, RationalNumber r2)
    {
        return r1.Div(r2);
    }

    public RationalNumber Abs()
    {
        return new RationalNumber(
            Math.Abs(numerator),
            denominator
        );
    }

    public RationalNumber Reduce()
    {
        return this;
    }

    public RationalNumber Exprational(int power)
    {
        RationalNumber r = new RationalNumber(1, 1);
        for (int i=0; i < power; i++) {
            r *= this;
        }
        return r;
    }

    public double Expreal(int baseNumber)
    {
        return Math.Pow(Math.Pow(baseNumber, numerator), 1.0 / denominator);
    }
}