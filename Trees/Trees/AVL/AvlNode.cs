using System;

namespace Trees
{
    public class AvlNode<K, V> : Node<K, V> where K : IComparable
    {
        public int Depth { get; set; }
        public int Weight { get; set; }

        public new AvlNode<K, V> Left { get { return (AvlNode<K, V>)base.Left; } set { base.Left = value; } }
        public new AvlNode<K, V> Right { get { return (AvlNode<K, V>)base.Right; } set { base.Right = value; } }

        public AvlNode(K key, V value) : base(key, value) { }
    }
}