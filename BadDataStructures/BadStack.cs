using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OfficialBadStackFanClub.BadStack
{
    public class BadStack<T> : IEnumerable<T>
    {
        private T[] values;
        
        /// <summary>
        /// The amount of elements in this stack
        /// </summary>
        public int Count { get; private set; }
        
        /// <summary>
        /// The amount of elements in this stack
        /// </summary>
        public int Length => Count;

        /// <summary>
        /// determines whether the stack can expand or not
        /// </summary>
        private bool FreeSize = default;

        /// <summary>
        /// index access to the stack
        /// </summary>
        /// <param name="index">the index to access</param>
        public T this[int index] => values[index];

        
        
        // Constructors
        /// <summary>
        /// Creates an Empty BadStack Instance with the default size
        /// this way, the stack is Free size
        /// </summary>
        public BadStack()
        {
            SetFreeSize(true);
            values = Array.Empty<T>();
        }

        /// <summary>
        /// Creates an instance of the stack with the specified capacity
        /// this way, the stack is not Free size
        /// </summary>
        /// <param name="capacity">the max amount of allowed elements</param>
        public BadStack(int capacity)
        {
            values = new T[capacity];
            SetFreeSize(false);
        }

        /// <summary>
        /// Creates an instance of the stack with the specified capacity and the specified values
        /// this way, the stack is not Free size
        /// </summary>
        /// <param name="capacity">the max amount of allowed elements</param>
        /// <param name="values">the initial values to add to the stack</param>
        /// <exception cref="IndexOutOfRangeException">if the given capacity is smaller than the amount of values entered, this is thrown</exception>
        public BadStack(int capacity, params T[] values) : this(capacity)
        {
            if (capacity < values.Length)
            {
                throw new IndexOutOfRangeException("the number of elements you entered is greater than the capacity you have given");
            }
            
            Push(values);
        }

        
        
        // Stack Modification
        /// <summary>
        /// Adds an element Atop the stack
        /// </summary>
        /// <param name="value">the element to add</param>
        /// <exception cref="FullStackException">if the stack is full and isn't free size, this is thrown</exception>
        public void Push(T value)
        {
            if (Count == values.Length)
            {
                if (FreeSize)
                {
                    ExpandStack();
                }
                else
                {
                    throw new FullStackException("the stack is full");
                }
            }
            if (Count == 0)
            {
                values[0] = value;
                IncrementCount();
                return;
            }

            values[Count] = value;
            
            IncrementCount();
        }
        
        /// <summary>
        /// Adds a range of elements Atop the stack
        /// </summary>
        /// <param name="values">the elements to add</param>
        /// <exception cref="FullStackException">if the stack is full and isn't free size, this is thrown</exception>
        public void Push(T[] values)
        {
            foreach (var value in values)
            {
                Push(value);
            }
        }

        /// <summary>
        /// Returns the last element of the stack before returning it
        /// </summary>
        /// <returns>the last element of the stack</returns>
        /// <exception cref="InvalidOperationException">if the stack is empty, this is thrown</exception>
        public T Pop()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("the stack is empty");
            }

            T last = values[Count - 1];
            values[Count - 1] = default(T);
            
            DecrementCount();

            return last;
        }

        /// <summary>
        /// Returns the last element of the stack without removing it
        /// </summary>
        /// <returns>the last element of the stack</returns>
        /// <exception cref="InvalidOperationException">if the stack is empty, this is thrown</exception>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("the stack is empty");
            }

            return values[Count - 1];
        }
        
        
        
        // stack Checks
        /// <summary>
        /// Checks whether the stack contains an instance of the given value
        /// </summary>
        /// <param name="value">the value to check for</param>
        /// <returns>(bool) true if the value exists, false if not</returns>
        public bool Contains(T value) => values.Contains(value);



        // Stack Utility
        /// <summary>
        /// Returns an array with the contents of the stack
        /// </summary>
        /// <returns>an array with the contents of the stack, in order</returns>
        public T[] ToArray()
        {
            var result = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                result[i] = values[i];
            }

            return result;
        }
        
        /// <summary>
        /// Clears the stack
        /// </summary>
        public void Clear()
        {
            Count = 0;
        }
        
        /// <summary>
        /// Configures if the stack can expand or not
        /// </summary>
        /// <param name="freeSize">whether the stack can expand or not</param>
        public void SetFreeSize(bool freeSize) => FreeSize = freeSize;

        /// <summary>
        /// Trims the excess cells within the stack, if the amount of elements is less that 90% of the length
        /// </summary>
        public void TrimExcess()
        {
            if ((float)Count / values.Length > 0.9f) return;

            T[] trimmed = values;

            values = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                values[i] = trimmed[i];
            }
        }

        /// <summary>
        /// Set the capacity of the stack, the new capacity cannot be less than the amount of items already in the stack
        /// </summary>
        /// <param name="newCap">the new capacity</param>
        /// <exception cref="InvalidOperationException">if the new capacity is less than the amount of items in the stack, this is thrown</exception>
        public void SetCapacity(int newCap)
        {
            if (newCap < Count)
                throw new InvalidOperationException(
                    "the new capacity is smaller than the amount of elements in the stack, elements will be lost");
            
            if (values.Length == 0)
            {
                values = new T[newCap];
                return;
            }
            
            var temp = values;

            values = new T[newCap];
            
            temp.CopyTo(values, 0);
        }
        
        
        
        // Stack internal functions
        /// <summary>
        /// Expands the stack by doubling its size
        /// if the stack size is 0, the size is set to 4
        /// </summary>
        private void ExpandStack()
        {
            if (values.Length == 0)
            {
                values = new T[4];
                return;
            }
            
            var temp = values;

            values = new T[values.Length * 2];
            
            temp.CopyTo(values, 0);
        }
        
        /// <summary>
        /// increments the element count
        /// </summary>
        private void IncrementCount() => Count++;
        
        /// <summary>
        /// decrements the element count
        /// </summary>
        private void DecrementCount()
        {
            if(Count == 0) return;
            Count--;
        }



        // Enumerators
        
        /// <summary>
        /// Returns the enumerator instance of the stack
        /// </summary>
        /// <returns>the enumerator instance of the stack</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return values.Take(Count).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FullStackException : Exception
    {
        public FullStackException()
        {
        }
        protected FullStackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public FullStackException(string? message) : base(message)
        {
        }
        public FullStackException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}