using System;

namespace Trees
{
    public class Node<K, V> where K : IComparable
    {
        public readonly K Key;

        public V Value { get; set; }
        public Node<K, V> Left { get; set; }
        public Node<K, V> Right { get; set; }
        public bool IsLeaf => Left == null && Right == null;
        public bool HasLeftChild => Left != null;
        public bool HasRightChild => Right != null;

        public Node(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"Key: {Key}, Value: {Value}";
        }
    }
}