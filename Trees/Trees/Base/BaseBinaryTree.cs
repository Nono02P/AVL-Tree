using System;
using System.Collections;
using System.Collections.Generic;

namespace Trees
{
    public abstract class BaseBinaryTree<K, V> : IEnumerable<Tuple<K, V>> where K : IComparable
    {
        public Node<K, V> RootNode { get; protected set; }
        public int Count { get; protected set; }

        public abstract void Add(K key, V value);
        public abstract void Delete(K key);

        public void Clear()
        {
            RootNode = null;
            Count = 0;
        }

        #region Search

        /// <summary>
        /// Get the value from a key.
        /// </summary>
        /// <param name="key">The key associated with the value to return.</param>
        /// <returns>The value associated with the key.</returns>
        public V GetValue(K key)
        {
            if (RootNode == null)
                throw new Exception("The tree is empty");
            else
            {
                return RecursiveGetValue(RootNode, key);
            }
        }

        /// <summary>
        /// Recursively call the children nodes to find the value associated with the key.
        /// </summary>
        /// <param name="node">The current node to look at.</param>
        /// <param name="key">The key associated with the value to return.</param>
        /// <returns>The value associated with the key.</returns>
        private V RecursiveGetValue(Node<K, V> node, K key)
        {
            switch (node.Key.CompareTo(key))
            {
                case 1: // This node is greather than other
                    if (node.Left != null)
                        return RecursiveGetValue(node.Left, key);
                    break;

                case -1: // This node is less than other
                    if (node.Right != null)
                        return RecursiveGetValue(node.Right, key);
                    break;

                default: // This node is equal than other
                    return node.Value;
            }
            throw new Exception("This key doesnt exist");
        }

        #endregion

        #region IEnumerable implementations

        public IEnumerator<Tuple<K, V>> GetEnumerator()
        {
            return GetEnumerator(RootNode);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(RootNode);
        }

        /// <summary>
        /// Recursively enumerate each children to return the Tuple(Key, Value).
        /// </summary>
        /// <param name="node">The current node (recursively call his children).</param>
        /// <returns>An enumerator on a Tuple(Key, Value)</returns>
        private IEnumerator<Tuple<K, V>> GetEnumerator(Node<K, V> node)
        {
            if (node != null)
            {
                if (node.Left != null)
                {
                    IEnumerator<Tuple<K, V>> enumerator = GetEnumerator(node.Left);
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }

                yield return new Tuple<K, V>(node.Key, node.Value);

                if (node.Right != null)
                {
                    IEnumerator<Tuple<K, V>> enumerator = GetEnumerator(node.Right);
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        #endregion

        #region Rotations

        /// <summary>
        /// Performs a left-left rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        protected Node<K, V> LeftLeftRotation(Node<K, V> node)
        {
            Node<K, V> c = node;
            node = node.Left;
            Node<K, V> r = node.Right;
            node.Right = c;
            c.Left = r;

            return node;
        }

        /// <summary>
        /// Performs a left-right rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        protected Node<K, V> LeftRightRotation(Node<K, V> node)
        {
            Node<K, V> c = node;
            node = node.Left.Right;
            Node<K, V> l = node.Left;
            Node<K, V> r = node.Right;

            node.Left = c.Left;
            node.Right = c;

            c.Left.Right = l;
            c.Left = r;


            return node;
        }

        /// <summary>
        /// Performs a right-right rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        protected Node<K, V> RightRightRotation(Node<K, V> node)
        {
            Node<K, V> c = node;
            node = node.Right;
            Node<K, V> l = node.Left;
            node.Left = c;
            c.Right = l;

            return node;
        }

        /// <summary>
        /// Performs a right-left rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        protected Node<K, V> RightLeftRotation(Node<K, V> node)
        {
            Node<K, V> c = node;
            node = node.Right.Left;
            Node<K, V> l = node.Left;
            Node<K, V> r = node.Right;

            node.Right = c.Right;
            node.Left = c;

            c.Right.Left = r;
            c.Right = l;

            return node;
        }

        #endregion

        public override string ToString()
        {
            return $"Count: {Count}";
        }
    }
}