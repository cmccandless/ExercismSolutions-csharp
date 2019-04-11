class Deque<T>
{
	private Node<T> Head { get; set; }
	private Node<T> Tail { get; set; }
	public Deque()
	{
		Head = null;
		Tail = null;
	}
	public T Pop()
	{
		var result = Tail.Value;
		if (Tail.Previous == null)
			Head = Tail = null;
		else
		{
			Tail = Tail.Previous;
			Tail.Next = null;
		}
		return result;
	}
	public void Push(T item)
	{
		var node = new Node<T>(item);
		if (Tail == null) Tail = node;
		else
		{
			Tail.Next = node;
			node.Previous = Tail;
			Tail = node;
		}
		if (Head == null) Head = Tail;
	}
	public T Shift()
	{
		var result = Head.Value;
		if (Head.Next == null)
			Tail = Head = null;
		else
		{
			Head = Head.Next;
			Head.Previous = null;
		}
		return result;
	}
	public void Unshift(T item)
	{
		var node = new Node<T>(item);
		if (Head == null) Head = node;
		else
		{
			Head.Previous = node;
			node.Next = Head;
			Head = node;
		}
		if (Tail == null) Tail = Head;
	}
	private class Node<TNode>
	{
		public TNode Value { get; private set; }
		public Node<TNode> Next { get; set; }
		public Node<TNode> Previous { get; set; }
		public Node(TNode value)
		{
			Value = value;
		}
	}
}