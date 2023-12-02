using System;
using System.Collections.Generic;

namespace BadQueue.BadQueue
{
    public class BadQueue<T>
    {
        private T[] array;
        
        public int Count { get; private set; }
        private int head;
        private int tail;
        
        
        // Constructors
        public BadQueue(params T[] values) : this()
        {
            array = new T[4];

            foreach (T value in values)
            {
                Enqueue(value);
            }
        }
        public BadQueue(IEnumerable<T> collection) : this()
        {
            array = new T[4];

            foreach (T value in collection)
            {
                Enqueue(value);
            }
        }
        public BadQueue(int capacity)
        {
            if (capacity < 0)
            {
                throw new InvalidOperationException($"the capacity cannot be less than 0, given capacity{capacity}");
            }

            array = new T[capacity];

            head = 0;
            tail = 0;
            Count = 0;
        }
        public BadQueue()
        {
            array = Array.Empty<T>();
            head = 0;
            tail = 0;
            Count = 0;
        }


        // Queue Exposed Functions
        public void Enqueue(T value)
        {
            if (Count == array.Length)
            {
                int cap = array.Length * 2;
                
                if (cap < array.Length + 4)
                {
                    cap = array.Length + 4;
                }
                
                SetCapacity(cap);
            }
            
            array[tail] = value;
            tail = (tail + 1) % array.Length;
            Count++;
        }
        public void Enqueue(params T[] values)
        {
            foreach (T value in values)
            {
                Enqueue(value);
            }
        }
        
        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("the queue is empty");
            
            T res = array[head];
            
            array[head] = default(T);
            
            head = (head + 1) % array.Length;

            Count--;

            return res;
        }
        
        public T Peek()
        {
            return array[head];
        }

        public void Clear()
        {
            tail = 0;
            head = 0;
            Count = 0;
        }


        // Queue Internal Functions
        private void SetCapacity(int newCapacity)
        {
            T[] newArray = new T[newCapacity];

            if (Count > 0)
            {
                if (head < tail)
                {
                    Array.Copy(array, head, newArray, 0, Count);
                }
                else
                {
                    Array.Copy(array, head, newArray, 0 , array.Length - head);
                    Array.Copy(array, 0, newArray, array.Length - head, tail);
                }
            }

            array = newArray;
            head = 0;
            tail = newCapacity == Count ? 0 : Count;
        }
    }
}