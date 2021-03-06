﻿using System;
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

        public override string ToString()
        {
            return $"Count: {Count}";
        }
    }
}