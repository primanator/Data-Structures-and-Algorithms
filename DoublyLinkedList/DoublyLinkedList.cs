using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DoublyLinkedList
{
    public class DoublyLinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : IComparable<T>
    {
        private class Node<T1>
        {
            private T1 _data;
            private Node<T1> _prev;
            private Node<T1> _next;

            public Node(T1 data, Node<T1> prev, Node<T1> next)
            {
                _data = data;
                _prev = prev;
                _next = next;
            }

            public T1 Data
            {
                get { return _data; }
                set
                {
                    if (value != null)
                        _data = value;
                    else
                        throw new ArgumentNullException();
                }
            }

            public Node<T1> Next
            {
                get { return _next; }
                set
                {
                    if (value is Node<T1> | value is null)
                        _next = value;
                    else
                        throw new ArgumentException("Value cannot be casted to Node<T1> type.");
                }
            }

            public Node<T1> Previous
            {
                get { return _prev; }
                set
                {
                    if (value is Node<T1> | value is null)
                        _prev = value;
                    else
                        throw new ArgumentException("Value cannot be casted to Node<T1> type.");
                }
            }
        }

        private Node<T> _upper_sentinel = null;
        private Node<T> _lower_sentinel = null;
        private int count = 0;

        /// <summary>Gets the number of elements contained in the DoublyLinkedList&lt;T&gt;.
        /// <returns>A return value is the number of elements contained in the DoublyLinkedList&lt;T&gt;.</returns>
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>Gets a value indicating whether the DoublyLinkedList&lt;T&gt; is read-only.
        /// <returns>A return value is false since collection can be modified.</returns>
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Default constructor for DoublyLinkedList&lt;T&gt; collection.
        /// </summary>
        public DoublyLinkedList()
        {
            _upper_sentinel = new Node<T>(default(T), null, null);
            _lower_sentinel = new Node<T>(default(T), null, null);
        }

        /// <summary>
        /// Constructor for DoublyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="item">The first object which will be contained in collection.</param>
        public DoublyLinkedList(T item)
        {
            Node<T> newnode = new Node<T>(item, null, null);
            _upper_sentinel = new Node<T>(default(T), null, newnode);
            _lower_sentinel = new Node<T>(default(T), newnode, null);
            count++;
        }

        /// <summary>
        /// Adds value in the beginning of DoublyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="item">Adeed object.</param>
        public void AddInTheBeginning(T item)
        {
            Node<T> newnode = new Node<T>(item, null, _upper_sentinel.Next);
            _upper_sentinel.Next.Previous = newnode;
            _upper_sentinel.Next = newnode;
            count++;
        }

        /// <summary>
        /// Adds value in the end of DoublyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="item">Added object.</param>
        public void Add(T item)
        {
            Node<T> newnode = new Node<T>(item, _lower_sentinel.Previous, null);
            _lower_sentinel.Previous.Next = newnode;
            _lower_sentinel.Previous = newnode;
            count++;
        }

        /// <summary>
        /// Adds value to DoublyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="afterme">The value after which new one will be added.</param>
        /// <param name="item">Added object.</param>
        /// <exception cref="ArgumentException">Thrown when there is no searched element in list.</exception>
        public void AddAfter(T afterme, T item)
        {
            Node<T> afterMeNode = Search(afterme);
            if (afterMeNode == null)
                throw new ArgumentException("There is no such \"afterme\" element in this list.");
            Node<T> newNode = new Node<T>(item, afterMeNode, afterMeNode.Next);
            afterMeNode.Next = newNode;
            count++;
        }

        /// <summary>
        /// Adds value to DoublyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="afterme">The value before which new one will be added.</param>
        /// <param name="item">Added object.</param>
        /// <exception cref="ArgumentException">Thrown when there is no searched element in list.</exception>
        public void AddBefore(T beforeme, T item)
        {
            Node<T> beforeMeNode = Search(beforeme);
            if (beforeMeNode == null)
                throw new ArgumentException("There is no such \"beforeme\" element in this list.");
            Node<T> newNode = new Node<T>(item, beforeMeNode.Previous, beforeMeNode);
            beforeMeNode.Previous.Next = newNode;
            beforeMeNode.Previous = newNode;
            count++;
        }

        private Node<T> Search(T item)
        {
            Node<T> node = _upper_sentinel.Next;
            while (node != null)
            {
                if (EqualityComparer<T>.Default.Equals(node.Data, item))
                    return node;
                if (node.Next != _lower_sentinel)
                    node = node.Next;
            }
            return null;
        }

        /// <summary>
        /// Gets copy of list.
        /// </summary>
        /// <returns>Returns a new instance of DoublyLinkedList&lt;T&gt; with the same elements.</returns>
        public DoublyLinkedList<T> GetCopy()
        {
            DoublyLinkedList<T> newList = new DoublyLinkedList<T>(_upper_sentinel.Next.Data);
            Node<T> node = _upper_sentinel.Next.Next;
            while (node != null)
            {
                newList.Add(node.Data);
                node = node.Next;
            }
            return newList;
        }

        /// <summary>
        /// Removes an object from collection. 
        /// </summary>
        /// <param name="item">Object to delete.</param>
        /// <returns>True if object have been removed. False otherwise.</returns>
        public bool Remove(T item)
        {
            Node<T> removableNode = Search(item);
            if (removableNode != null)
            {
                if (removableNode.Previous != null & removableNode.Next != null)
                {
                    removableNode.Previous.Next = removableNode.Next;
                    removableNode.Next.Previous = removableNode.Previous;
                }
                else if (removableNode.Previous != null & removableNode.Next == null)
                {
                    _lower_sentinel.Previous = removableNode.Previous;
                    removableNode.Previous.Next = null;
                }
                else if (removableNode.Previous == null & removableNode.Next != null)
                {
                    _upper_sentinel.Next = removableNode.Next;
                    removableNode.Next.Previous = null;
                }
                else if (removableNode.Previous == null & removableNode.Next == null)
                {
                    Clear();
                }
                count--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears collection.
        /// </summary>
        public void Clear()
        {
            _upper_sentinel.Next = null;
            _lower_sentinel.Previous = null;
            count = 0;
        }

        /// <summary>
        /// Checks whether the collection is containing the object.
        /// </summary>
        /// <param name="item">Object to check for being contained.</param>
        /// <returns>True if object have been removed. False otherwise.</returns>
        public bool Contains(T item)
        {
            if (Search(item) != null)
                return true;
            return false;
        }

        /// <summary>
        /// Copying all of the collection`s values to array from defined index.
        /// </summary>
        /// <param name="arr">Array to copy in to.</param>
        /// <param name="Index">Index in array to copy from.</param>
        /// <exception cref="NullReferenceException">Thrown when array is null.</exception>
        /// <exception cref="ArgumentException">Thrown when there is not enough space in array to copy all elements in.</exception>
        public void CopyTo(T[] arr, int Index)
        {
            if (arr == null)
                throw new NullReferenceException("Argument arr cannot be null.");
            if ((arr.Length - Index) < Count)
                throw new ArgumentException("There is not enough space in array to copy doubly linked list in there.");
            Node<T> node = _upper_sentinel.Next;
            for (int i = Index; i < arr.Length; i++)
            {
                arr[i] = node.Data;
                node = node.Next;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the DoublyLinkedList&lt;T&gt;
        /// </summary>
        /// <returns>SinglyLinkedList&lt;T&gt; Enumerator for the SinglyLinkedList&lt;T&gt;</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new DoublyLinkedListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private class DoublyLinkedListEnumerator : IEnumerator<T>
        {
            private DoublyLinkedList<T> list { set; get; }
            private int position;
            private Node<T> current;

            public DoublyLinkedListEnumerator(DoublyLinkedList<T> list)
            {
                this.list = list;
                current = list._upper_sentinel;
                position = -1;
            }

            T IEnumerator<T>.Current { get { return current.Data; } }

            object IEnumerator.Current { get { return current; } }

            void IDisposable.Dispose()
            {
                position = -1;
                current = null;
                list = null;
            }

            bool IEnumerator.MoveNext()
            {
                if (position < list.Count - 1)
                {
                    position++;
                    current = current.Next;
                    return true;
                }
                position = -1;
                return false;
            }

            void IEnumerator.Reset()
            {
                position = -1;
            }
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation (all of its elements common string).
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            Node<T> node = _upper_sentinel.Next;
            StringBuilder sb = new StringBuilder("");
            while (node != null)
            {
                sb.Append(node.Data + " ");
                node = node.Next;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object. Otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            DoublyLinkedList<T> objList = obj as DoublyLinkedList<T>;
            if (objList != null)
            {
                if (objList.GetHashCode() == GetHashCode())
                    return true;
                return false;
            }
            else
                throw new InvalidCastException("Argument obj cannot be casted into DoublyLinkedList<T1> type.");
        }

        /// <summary>
        /// Hash function for DoublyLinkedList&lt;T&gt;.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            int hash = 1;
            foreach (T x in this)
            {
                checked { hash *= x.GetHashCode(); }
            }
            return hash;
        }
    }
}
