using System;
using System.Collections.Generic;
using System.Linq;

public class SgfTree : IEquatable<SgfTree>
{
    public SgfTree(IDictionary<string, string[]> data, params SgfTree[] children)
    {
        Data = data;
		Children = children;
    }

    private IDictionary<string, string[]> Data { get; }
    private SgfTree[] Children { get; set; }

	public void AddChild(SgfTree child) => Children = Children.Append(child).ToArray();

	public void AddData((string key, string[] values) keyValuePair) => AddData(keyValuePair.key, keyValuePair.values);
	public void AddData(string key, string[] values) =>
		Data.Add(
			key.AssertThat(k => k != string.Empty, "Property key cannot be empty"),
			values.AssertThat(vs => vs.Length != 0, "Property must have at least one value")
		);

    public bool Equals(SgfTree other) => $"{this}" == $"{other}";

	private static string FormatData(IDictionary<string, string[]> data) => string.Join(
		"",
		from key in data.Keys
		let values = data[key].ToString(delimiter: "][")
		select $"{key}[{values}]"
	);

	private static string FormatChildren(SgfTree[] children)
	{
		var ret = children.ToString(delimiter: string.Empty);
		if (children.Length == 1) ret = ret.Substring(1, ret.Length - 2);
		return ret;
	}

	public override string ToString() => $"(;{FormatData(Data)}{FormatChildren(Children)})";
}

public class SgfParser
{
	private const char TREE_START = '(';
	private const char TREE_END = ')';
	private const char NODE_START = ';';
	private const char PROPERTY_VALUE_START = '[';
	private const char PROPERTY_VALUE_END = ']';

	private static string ParsePropertyKey(Stack<char> stack) =>
		new string(stack.PopWhile(Char.IsUpper).ToArray());

	private static string ParsePropertyValue(Stack<char> stack)
	{
		stack.Pop(); // Ignore PROPERTY_VALUE_START
		var value = new string(stack.PopWhile(ch => ch != PROPERTY_VALUE_END, s => s.PopEscapable()).ToArray());
		stack.Pop(); // Ignore PROPERTY_VALUE_END
		return value;
	}

	private static IEnumerable<string> ParsePropertyValues(Stack<char> stack) =>
		stack.PopWhile(ch => ch == PROPERTY_VALUE_START, ParsePropertyValue);

	private static (string key, string[] values) ParseProperty(Stack<char> stack) => (
		ParsePropertyKey(stack),
		ParsePropertyValues(stack).ToArray()
	);

	private static SgfTree ParseTree(Stack<char> stack)
	{
		char ch ;
		if (!stack.TryPop(out ch) || ch != TREE_START)
			throw new ArgumentException($"missing '{TREE_START}'");
		if (!stack.TryPop(out ch) || ch != NODE_START)
			throw new ArgumentException($"missing '{NODE_START}'");

		var root = new SgfTree(new Dictionary<string, string[]>());
		while (stack.TryPeek(out ch))
		{
			switch (ch)
			{
				// Single Child
				case NODE_START:
					root.AddChild(ParseTree(stack.Append(TREE_END).Reverse().Append(TREE_START).ToStack()));
					return root;

				// New Subtree
				case TREE_START:
					root.AddChild(ParseTree(stack));
					break;

				// End current tree
				case TREE_END:
					stack.Pop();
					return root;

				// Property
				default:
					root.AddData(ParseProperty(stack));
					break;
			}
		}
		throw new ArgumentException("incomplete tree");
	}

    public static SgfTree ParseTree(string input) => ParseTree(input.Reverse().ToStack());
}

static class Extensions
{
	public static IEnumerable<T> PopWhile<T>(this Stack<T> stack, Func<T, bool> predicate) =>
		stack.PopWhile(predicate, s => s.Pop());

	public static IEnumerable<TOut> PopWhile<TIn, TOut>(this Stack<TIn> stack, Func<TIn, bool> predicate, Func<Stack<TIn>, TOut> popFunc)
	{
		TIn outVal;
		while (stack.TryPeek(out outVal) && predicate(outVal))
			yield return popFunc(stack);
	}
	public static char PopEscapable(this Stack<char> stack)
	{
		var escapables = new Dictionary<char, char>{
			['n'] = '\n', // Preserve escaped newline
			['t'] = ' ',  // Replace escaped tab with space
		};
		var ch = stack.Pop();
		return ch == '\\' ?
			escapables.GetValueOrDefault(stack.Peek(), stack.Pop()) :
			ch;
	}

	public static Stack<T> ToStack<T>(this IEnumerable<T> col) => new Stack<T>(col);

	public static string ToString<T>(this IEnumerable<T> col, string delimiter = ",") =>
		string.Join(delimiter, col.Select(c => c.ToString()));

	public static T AssertThat<T>(this T obj, Func<T, bool> predicate, string msg = null)
	{
		if (predicate(obj)) return obj;
		throw new ArgumentException(msg);
	}
}
