using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LinkedList
{
    /// <summary>
    /// Represents a strongly typed linked list of objects. Provides methods to search, sort, and manipulate linked lists.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SinglyLinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : IComparable<T>
    {
        private class Node<T1>
        {
            private T1 _data;
            private Node<T1> _next;

            public Node(T1 data, Node<T1> next)
            {
                _data = data;
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
        }

        private Node<T> _sentinel = null;
        private int count = 0;

        /// <summary>Gets the number of elements contained in the SinglyLinkedList&lt;T&gt;.
        /// <returns>A return value is the number of elements contained in the SinglyLinkedList&lt;T&gt;.</returns>
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>Gets a value indicating whether the SinglyLinkedList&lt;T&gt; is read-only.
        /// <returns>A return value is false since collection can be modified.</returns>
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Default constructor for SinglyLinkedList&lt;T&gt; collection.
        /// </summary>
        public SinglyLinkedList()
        {
            _sentinel = new Node<T>(default(T), null);
        }

        /// <summary>
        /// Constructor for SinglyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="item">The first object which will be contained in collection.</param>
        public SinglyLinkedList(T item)
        {
            Node<T> newnode = new Node<T>(item, null);
            _sentinel = new Node<T>(default(T), newnode);
            count++;
        }

        /// <summary>
        /// Adds value in the beginning of SinglyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="item">Adeed object.</param>
        public void AddInTheBeginning(T item)
        {
            _sentinel.Next = new Node<T>(item, _sentinel.Next);
            count++;
        }

        /// <summary>
        /// Adds value in the end of SinglyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="item">Added object.</param>
        public void Add(T item)
        {
            Node<T> node = _sentinel.Next;
            while (node.Next != null)
                node = node.Next;
            node.Next = new Node<T>(item, null);
            count++;
        }

        /// <summary>
        /// Adds value to SinglyLinkedList&lt;T&gt; collection.
        /// </summary>
        /// <param name="afterme">The value after which new one will be added.</param>
        /// <param name="item">Added object.</param>
        /// <exception cref="ArgumentException">Thrown when there is no searched element in list.</exception>
        public void Add(T afterme, T item)
        {
            Node<T> afterMeNode = Search(afterme);
            if (afterMeNode == null)
                throw new ArgumentException("There is no such \"afterme\" element in this list.");
            Node<T> newNode = new Node<T>(item, afterMeNode.Next);
            afterMeNode.Next = newNode;
            count++;
        }

        /// <summary>
        /// Removes an object from collection. 
        /// </summary>
        /// <param name="item">Object to delete.</param>
        /// <returns>True if object have been removed. False otherwise.</returns>
        public bool Remove(T item)
        {
            Node<T> node = _sentinel;
            while (node.Next != null)
            {
                if (EqualityComparer<T>.Default.Equals(node.Next.Data, item))
                {
                    node.Next = node.Next.Next;
                    count--;
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        /// <summary>
        /// Clears collection.
        /// </summary>
        public void Clear()
        {
            _sentinel.Next = null;
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
                throw new ArgumentException("There is not enough space in array to copy linked list in there.");
            Node<T> node = _sentinel.Next;
            for (int i = Index; i < arr.Length; i++)
            {
                arr[i] = node.Data;
                node = node.Next;
            }
        }

        private Node<T> Search(T item)
        {
            Node<T> node = _sentinel.Next;
            while (node != null)
            {
                if (EqualityComparer<T>.Default.Equals(node.Data, item))
                    return node;
                node = node.Next;
            }
            return null;
        }

        /// <summary>
        /// Gets copy of list.
        /// </summary>
        /// <returns>Returns a new instance of SinglyLinkedList&lt;T&gt; with the same elements.</returns>
        public SinglyLinkedList<T> GetCopy()
        {
            SinglyLinkedList<T> newList = new SinglyLinkedList<T>(_sentinel.Next.Data);
            Node<T> node = _sentinel.Next.Next;
            while (node != null)
            {
                newList.Add(node.Data);
                node = node.Next;
            }
            return newList;
        }

        /// <summary>
        /// Insertion sort of all contained elements.
        /// </summary>
        /// <param name="ascending">If true will sort in ascending order; Otherwise - in descending.</param>
        public void InsertionSort(bool ascending = false)
        {
            if (ascending)
                InsertionAscendingSort();
            else
                InsertionDescendingSort();
        }

        private void InsertionDescendingSort()
        {
            SinglyLinkedList<T> newList = new SinglyLinkedList<T>(_sentinel.Next.Data);
            Node<T> nodeA = _sentinel.Next.Next;
            Node<T> temp = null;
            while (nodeA != null)
            {
                Node<T> nodeB = newList._sentinel;
                while (nodeB != null)
                {
                    if (nodeB.Next.Data.CompareTo(nodeA.Data) <= 0)
                    {
                        temp = nodeA.Next;
                        nodeA.Next = nodeB.Next;
                        nodeB.Next = nodeA;
                        break;
                    }
                    nodeB = nodeB.Next;
                    if (nodeB.Next == null)
                    {
                        newList.Add(nodeA.Data);
                        temp = nodeA.Next;
                        break;
                    }
                }
                nodeA = temp;
            }
            _sentinel = newList._sentinel;
        }

        private void InsertionAscendingSort()
        {
            SinglyLinkedList<T> newList = new SinglyLinkedList<T>(_sentinel.Next.Data);
            Node<T> nodeA = _sentinel.Next.Next;
            Node<T> temp = null;
            while (nodeA != null)
            {
                Node<T> nodeB = newList._sentinel;
                while (nodeB != null)
                {
                    if (nodeB.Next.Data.CompareTo(nodeA.Data) > 0)
                    {
                        temp = nodeA.Next;
                        nodeA.Next = nodeB.Next;
                        nodeB.Next = nodeA;
                        break;
                    }
                    nodeB = nodeB.Next;
                    if (nodeB.Next == null)
                    {
                        newList.Add(nodeA.Data);
                        temp = nodeA.Next;
                        break;
                    }
                }
                nodeA = temp;
            }
            _sentinel = newList._sentinel;
        }

          /// <summary>
        /// Selection sort of all contained elements.
        /// </summary>
        /// <param name="ascending">If true will sort in ascending order; Otherwise - in descending.</param>
        public void SelectionSort(bool ascending = false)
        {
            if (ascending)
                SelectionAscendingSort();
            else
                SelectionDescendingSort();
        }

        private void SelectionAscendingSort()
        {
            T maximum = MaxValue();
            SinglyLinkedList<T> newList = new SinglyLinkedList<T>(maximum);
            Remove(maximum);
            while(_sentinel.Next != null)
            {
                maximum = MaxValue();
                newList.AddInTheBeginning(maximum);
                Remove(maximum);
            }
            _sentinel = newList._sentinel;
        }

        private void SelectionDescendingSort()
        {
            T minimum = MinValue();
            SinglyLinkedList<T> newList = new SinglyLinkedList<T>(minimum);
            Remove(minimum);
            while (_sentinel.Next != null)
            {
                minimum = MaxValue();
                newList.Add(minimum);
                Remove(minimum);
            }
            _sentinel = newList._sentinel;
        }

        public T MaxValue()
        {
            T max = _sentinel.Next.Data;
            foreach(T value in this)
            {
                if (value.CompareTo(max) > 0)
                    max = value;
            }
            return max;
        }

        public T MinValue()
        {
            T min = _sentinel.Next.Data;
            foreach (T value in this)
            {
                if (value.CompareTo(min) < 0)
                    min = value;
            }
            return min;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the SinglyLinkedList&lt;T&gt;
        /// </summary>
        /// <returns>SinglyLinkedList&lt;T&gt; Enumerator for the SinglyLinkedList&lt;T&gt;</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new SinglyLinkedListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private class SinglyLinkedListEnumerator : IEnumerator<T>
        {
            private SinglyLinkedList<T> list { set; get; }
            private int position;
            private Node<T> current;

            public SinglyLinkedListEnumerator(SinglyLinkedList<T> list)
            {
                this.list = list;
                current = list._sentinel;
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
            Node<T> node = _sentinel.Next;
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
            SinglyLinkedList<T> objList = obj as SinglyLinkedList<T>;
            if (objList != null)
            {
                if (objList.GetHashCode() == GetHashCode())
                    return true;
                return false;
            }
            else
                throw new InvalidCastException("Argument obj cannot be casted into SinglyLinkedList<T1> type.");
        }

        /// <summary>
        /// Hash function for SinglyLinkedList&lt;T&gt;.
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