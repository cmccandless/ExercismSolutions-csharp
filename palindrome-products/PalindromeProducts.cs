using System;
using System.Collections.Generic;
using System.Linq;

public static class PalindromeProducts
{
	public static (int, (int, int)[]) Smallest(int minFactor, int maxFactor)
	{
		if (minFactor > maxFactor)
			throw new ArgumentException();
		for (int m = minFactor; m <= maxFactor; m++)
			for (int n = minFactor; n <= m; n++)
			{
				var p = new Product(m, n);
				if (p.Palindrome)
					return p.AsResult();
			}
		throw new ArgumentException();
	}

	public static (int, (int, int)[]) Largest(int minFactor, int maxFactor)
	{
		if (minFactor > maxFactor)
			throw new ArgumentException();
		var best = new Product(minFactor, minFactor);
		for (int m = maxFactor; m >= minFactor; m--)
		{
			var n = Math.Max((int)(m * 0.9) - 10, minFactor);
			for (; n <= m; n++)
				best = best.KeepLargest(new Product(m, n));
			if (best.Palindrome)
				return Product.Create(
					best.Value, minFactor, maxFactor
				).AsResult();
		}
		throw new ArgumentException();
	}
}

class Product
{
	private static bool IsPalindrome(string s) =>
		s == new String(s.Reverse().ToArray());
	public static bool IsPalindrome(int n) => Product.IsPalindrome($"{n}");

	public bool Palindrome { get; }
	public int Value { get; }
	public (int, int)[] Factors { get; }
	public Product(int m, int n)
	{
		this.Value = m * n;
		if (m < n)
			(m, n) = (n, m);
		this.Factors = new[] {(m, n)};
		this.Palindrome = Product.IsPalindrome(this.Value);
	}

	private Product(int value, (int, int)[] factors, bool isPalindrome)
	{
		this.Value = value;
		this.Factors = factors;
		this.Palindrome = isPalindrome;
	}

	public Product Merge(Product other) => new Product(
			this.Value,
			this.Factors.Concat(other.Factors).OrderBy(x => x).ToArray(),
			true
		);

	private Product Keep(Product other, Func<Product, Product, bool> isBetter)
	{
		if (!other.Palindrome) return this;
		if (!this.Palindrome || isBetter(other, this)) return other;
		if (isBetter(this, other)) return this;
		return Merge(other);
	}
	
	public Product KeepSmallest(Product other) =>
		Keep(other, (a, b) => a.Value < b.Value);
	public Product KeepLargest(Product other) =>
		Keep(other, (a, b) => a.Value > b.Value);

	public (int, (int, int)[]) AsResult() => (this.Value, this.Factors);

	public static Product Create(int value, int minFactor, int maxFactor) =>
		new Product(
			value,
			(
				from m in Enumerable.Range(minFactor, maxFactor)
					.TakeWhile(m => m <= value / m)
				where value % m == 0
				let n = value / m
				where n <= maxFactor
				select (m, n)
			).ToArray(),
			Product.IsPalindrome(value)
		);
}
