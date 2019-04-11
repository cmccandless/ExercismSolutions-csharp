using System;
using System.Collections.Generic;
using System.Linq;

public class BinTree : IEquatable<BinTree>
{
    public BinTree(int value, BinTree left, BinTree right)
    {
        Value = value;
        Left = left;
        Right = right;
    }

    public int Value { get; }
    public BinTree Left { get; }
    public BinTree Right { get; }

    public bool Equals(BinTree other) =>
        other != null &&
        Value.Equals(other.Value) &&
        ((Left == null && other.Left == null) || Left.Equals(other.Left)) &&
        ((Right == null && other.Right == null) || Right.Equals(other.Right));
}

public class Zipper : IEquatable<Zipper>
{
    public int Value() => value;

    public Zipper SetValue(int newValue) => new Zipper(newValue, left, right, Ancestors);

    public Zipper SetLeft(BinTree binTree) => new Zipper(value, binTree, right, Ancestors);

    public Zipper SetRight(BinTree binTree) => new Zipper(value, left, binTree, Ancestors);

    public Zipper Left() => left == null ?
        null :
        new Zipper(
            left.Value,
            left.Left,
            left.Right,
            new Stack<Zipper>(Ancestors.Reverse().Append(new ZipperLeft(value, left, right, Ancestors)))
        );

    public Zipper Right() => right == null ?
        null :
        new Zipper(
            right.Value,
            right.Left,
            right.Right,
            new Stack<Zipper>(Ancestors.Reverse().Append(new ZipperRight(value, left, right, Ancestors)))
        );

    public Zipper Up()
    {
        Zipper parent;
        if (!Ancestors.TryPop(out parent)) return null;
        var tree = new BinTree(value, left, right);
        if (parent is ZipperLeft zl)
            zl.SetLeft(tree);
        else if (parent is ZipperRight zr)
            zr.SetRight(tree);
        return parent;
    }

    public BinTree ToTree()
    {
        var tree = new BinTree(value, left, right);
        Zipper parent;
        while (Ancestors.TryPop(out parent))
        {
            if (parent is ZipperLeft zl)
                tree = new BinTree(zl.value, tree, zl.right);
            else if (parent is ZipperRight zr)
                tree = new BinTree(zr.value, zr.left, tree);
        }
        return tree;
    }

    public static Zipper FromTree(BinTree tree) => new Zipper(tree.Value, tree.Left, tree.Right);

    public bool Equals(Zipper other) => value == other.value && left == other.left && right == other.right;

    class ZipperLeft : Zipper
    {
        public ZipperLeft(int value, BinTree left = null, BinTree right = null, Stack<Zipper> ancestors = null)
            : base(value, left, right, ancestors) { }
    }
    class ZipperRight : Zipper
    {
        public ZipperRight(int value, BinTree left = null, BinTree right = null, Stack<Zipper> ancestors = null)
            : base(value, left, right, ancestors) { }
    }

    private Stack<Zipper> Ancestors { get; }
    private int value { get; }
    private BinTree left { get; }
    private BinTree right { get; }

    private Zipper(int value, BinTree left = null, BinTree right = null, Stack<Zipper> ancestors = null)
    {
        this.value = value;
        this.left = left;
        this.right = right;
        Ancestors = ancestors ?? new Stack<Zipper>();
    }
}