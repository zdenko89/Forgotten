using System;

public interface IPriorityQueue<T>
{
    /// Number of items in the queue.
    int Count { get; }

    /// Method adds item to the queue.
    void Enqueue(T item, int priority);

    /// Method returns item with the LOWEST priority value.
    T Dequeue();
}
class PriorityQueueNode<T> : IComparable
{
    public T Item { get; private set; }
    public float Priority { get; private set; }

    public PriorityQueueNode(T item, float priority)
    {
        Item = item;
        Priority = priority;
    }

    public int CompareTo(object obj)
    {
        return Priority.CompareTo((obj as PriorityQueueNode<T>).Priority);
    }
}

