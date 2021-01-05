using System;

namespace Trees
{
    public class RedBlackNode<K, V> : Node<K, V> where K : IComparable
    {
        #region Color properties

        private bool _isRed;

        public bool IsRed
        {
            get { return _isRed; }
            set { _isRed = value; }
        }

        public bool IsBlack
        {
            get { return !_isRed; }
            set { _isRed = !value; }
        } 

        #endregion

        public new RedBlackNode<K, V> Left { get { return (RedBlackNode<K, V>)base.Left; } set { base.Left = value; } }
        public new RedBlackNode<K, V> Right { get { return (RedBlackNode<K, V>)base.Right; } set { base.Right = value; } }

        public RedBlackNode(K key, V value) : base(key, value) { }
    }
}