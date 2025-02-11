using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();

    public int Count => heap.Count;

    public void Enqueue(T item)
    {
        heap.Add(item);
        int currentIndex = heap.Count - 1;

        // Bubble up
        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;
            if (heap[currentIndex].CompareTo(heap[parentIndex]) >= 0)
                break;

            // Swap
            (heap[currentIndex], heap[parentIndex]) = (heap[parentIndex], heap[currentIndex]);
            currentIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        T root = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        // Bubble down
        int currentIndex = 0;
        while (true)
        {
            int leftChildIndex = 2 * currentIndex + 1;
            int rightChildIndex = 2 * currentIndex + 2;
            int smallestIndex = currentIndex;

            if (leftChildIndex < heap.Count && heap[leftChildIndex].CompareTo(heap[smallestIndex]) < 0)
                smallestIndex = leftChildIndex;

            if (rightChildIndex < heap.Count && heap[rightChildIndex].CompareTo(heap[smallestIndex]) < 0)
                smallestIndex = rightChildIndex;

            if (smallestIndex == currentIndex)
                break;

            // Swap
            (heap[currentIndex], heap[smallestIndex]) = (heap[smallestIndex], heap[currentIndex]);
            currentIndex = smallestIndex;
        }

        return root;
    }

    public T Peek()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");
        return heap[0];
    }
}
