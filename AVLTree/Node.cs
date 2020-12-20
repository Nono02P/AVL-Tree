using System;

namespace AVL
{
    public partial class AVLTree<K, V> where K : IComparable
    {
        private class Node
        {
            public readonly K Key;
            public int Depth { get; set; }
            public int Weight { get; set; }

            public V Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

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
}