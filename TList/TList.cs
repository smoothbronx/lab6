using System.Collections.Generic;
using System.Collections;

namespace lab6.TList
{
    public class TList<T> : IEnumerable<T>
    {
        private PNode<T>? First { get; set; }
        private PNode<T>? Last { get; set; }
        private PNode<T>? Current { get; set; }

        public void InsertFirst(T value)
        {
            var node = new PNode<T>(value);
            if (Current == null)
            {
                First = node;
                Last = node;
            }
            else if (Current.Previous == null)
            {
                node.Next = First;
                First!.Previous = node;
                First = node;
            }
            else if (Current.Next == null)
            {
                node.Previous = Last;
                Last!.Next = node;
                Last = node;
            }
            else
            {
                node.Previous = Current.Previous;
                node.Next = Current;
                Current.Previous.Next = node;
                Current.Previous = node;
            }

            Current = node;
        }

        public bool Remove(T data)
        {
            var current = First;
            while (current != null && current.Value != null)
            {
                if (current.Value.Equals(data))
                {
                    break;
                }

                current = current.Next;
            }

            if (current == null) return false;

            if (current.Next != null)
            {
                current.Next.Previous = current.Previous;
            }
            else
            {
                Last = current.Previous;
            }

            if (current.Previous != null)
            {
                current.Previous.Next = current.Next;
            }
            else
            {
                First = current.Next;
            }

            if (current.Equals(Current))
                Current = Last;
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        public IEnumerator<T> GetBackEnumerator()
        {
            var current = Last;
            while (current != null)
            {
                yield return current.Value;
                current = current.Previous;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }
}