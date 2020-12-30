using System;

namespace Trees
{
    public class RedBlackNode<K, V> : Node<K, V> where K : IComparable
    {
        public bool IsRed { get; set; }

        public RedBlackNode(K key, V value) : base(key, value) { }
    }
}
