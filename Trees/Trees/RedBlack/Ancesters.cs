using System;

namespace Trees
{
    internal struct Ancesters<K, V> where K : IComparable
    {
        public RedBlackNode<K, V> Parent;
        public RedBlackNode<K, V> GrandParent;

        public Ancesters(RedBlackNode<K, V> parent, RedBlackNode<K, V> grandParent)
        {
            Parent = parent;
            GrandParent = grandParent;
        }
    }
}