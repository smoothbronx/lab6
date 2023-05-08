namespace lab6.TList
{
    public class PNode<T>
    {
        public T Value { get; set; }
        public PNode<T>? Next { get; set; }
        public PNode<T>? Previous { get; set; }

        public PNode(T value)
        {
            Value = value;
        }
    }
}
