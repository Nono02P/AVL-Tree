using System;
using System.Collections;
using System.Collections.Generic;

namespace AVL
{
    public partial class AVLTree<K, V> : IEnumerable<Tuple<K, V>> where K : IComparable
    {
        private Node _rootNode;

        public int Count { get; private set; }

        #region Insertion

        /// <summary>
        /// Insert an element (Key, Value) in the AVL tree.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value associated with the key.</param>
        public void Add(K key, V value)
        {
            Node node = new Node(key, value);
            if (_rootNode == null)
                _rootNode = node;
            else
                RecursiveInsertion(_rootNode, node);

            _rootNode = RefreshTree(_rootNode);
            Count++;
        }

        /// <summary>
        /// Recursively call the children (from the root node) to insert the new node at the correct place.
        /// </summary>
        /// <param name="existing">The current node (already exists in the tree).</param>
        /// <param name="inserted">The node to insert in the tree.</param>
        private void RecursiveInsertion(Node existing, Node inserted)
        {
            switch (existing.Key.CompareTo(inserted.Key))
            {
                case 1: // The existing node is greather than inserted
                    if (existing.Left == null)
                        existing.Left = inserted;
                    else
                        RecursiveInsertion(existing.Left, inserted);
                    break;

                case -1: // The existing node is less than inserted
                    if (existing.Right == null)
                        existing.Right = inserted;
                    else
                        RecursiveInsertion(existing.Right, inserted);
                    break;

                default: // The existing node is equal than inserted
                    throw new Exception("Duplicate key");
            }
        }

        #endregion

        #region Refresh nodes

        /// <summary>
        /// A recursive method to refresh the tree by calculating the depth/weight of a node and his children. 
        /// If necessary performs rotation.
        /// </summary>
        /// <param name="node">The node to refresh (will recursively call his children).</param>
        /// <returns></returns>
        private Node RefreshTree(Node node)
        {
            if (node.Left == null && node.Right == null)
            {
                node.Depth = 1;
                node.Weight = 0;
            }
            else if (node.Left != null && node.Right != null)
            {
                node.Left = RefreshTree(node.Left);
                node.Right = RefreshTree(node.Right);
                node.Depth = Math.Max(node.Left.Depth, node.Right.Depth) + 1;
                node.Weight = node.Left.Depth - node.Right.Depth;
            }
            else if (node.Left != null)
            {
                node.Left = RefreshTree(node.Left);
                node.Depth = node.Left.Depth + 1;
                node.Weight = node.Left.Depth;
            }
            else
            {
                node.Right = RefreshTree(node.Right);
                node.Depth = node.Right.Depth + 1;
                node.Weight = -node.Right.Depth;
            }

            // If tree is unbalanced, it needs some rotations
            if (Math.Abs(node.Weight) > 1)  
                node = Rotate(node);

            return node;
        }

        #endregion

        #region Rotations

        /// <summary>
        /// Determines the rotation to do from the unbalanced node.
        /// </summary>
        /// <param name="node">The unbalanced node</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private Node Rotate(Node node)
        {
            switch (node.Weight)
            {
                case 2: // Need left rotation
                    switch (node.Left.Weight)
                    {
                        case 1:
                            node = LeftLeftRotation(node);
                            break;
                        case -1:
                            node = LeftRightRotation(node);
                            break;
                        default:
                            break;
                    }
                    break;
                
                case -2: // Need right rotation
                    switch (node.Right.Weight)
                    {
                        case 1:
                            node = RightLeftRotation(node);
                            break;
                        case -1:
                            node = RightRightRotation(node);
                            break;
                        default:
                            break;
                    }
                    break;
                
                default:
                    break;
            }
            // After a rotation, recalculate depth/weight of the node and his children 
            return RefreshTree(node);
        }

        /// <summary>
        /// Performs a left-left rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private Node LeftLeftRotation(Node node)
        {
            Node c = node;
            node = node.Left;
            Node r = node.Right;
            node.Right = c;
            c.Left = r;

            return node;
        }

        /// <summary>
        /// Performs a left-right rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private Node LeftRightRotation(Node node)
        {
            Node c = node;
            node = node.Left.Right;
            Node l = node.Left;
            Node r = node.Right;

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
        private Node RightRightRotation(Node node)
        {
            Node c = node;
            node = node.Right;
            Node l = node.Left;
            node.Left = c;
            c.Right = l;

            return node;
        }

        /// <summary>
        /// Performs a right-left rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private Node RightLeftRotation(Node node)
        {
            Node c = node;
            node = node.Right.Left;
            Node l = node.Left;
            Node r = node.Right;

            node.Right = c.Right;
            node.Left = c;

            c.Right.Left = r;
            c.Right = l;

            return node;
        }

        #endregion

        #region Search

        /// <summary>
        /// Get the value from a key.
        /// </summary>
        /// <param name="key">The key associated with the value to return.</param>
        /// <returns>The value associated with the key.</returns>
        public V GetValue(K key)
        {
            if (_rootNode == null)
                throw new Exception("The tree is empty");
            else
            {
                return RecursiveGetValue(_rootNode, key);
            }
        }

        /// <summary>
        /// Recursively call the children nodes to find the value associated with the key.
        /// </summary>
        /// <param name="node">The current node to look at.</param>
        /// <param name="key">The key associated with the value to return.</param>
        /// <returns>The value associated with the key.</returns>
        private V RecursiveGetValue(Node node, K key)
        {
            switch (node.Key.CompareTo(key))
            {
                case 1: // This node is greather than other
                    if (node.Left == null)
                        throw new Exception("This key doesnt exist");
                    else
                        return RecursiveGetValue(node.Left, key);

                case -1: // This node is less than other
                    if (node.Right == null)
                        throw new Exception("This key doesnt exist");
                    else
                        return RecursiveGetValue(node.Right, key);

                default: // This node is equal than other
                    return node.Value;
            }
        }

        #endregion

        #region IEnumerable implementations

        public IEnumerator<Tuple<K, V>> GetEnumerator()
        {
            return GetEnumerator(_rootNode);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(_rootNode);
        }

        /// <summary>
        /// Recursively enumerate each children to return the Tuple(Key, Value).
        /// </summary>
        /// <param name="node">The current node (recursively call his children).</param>
        /// <returns>An enumerator on a Tuple(Key, Value)</returns>
        private IEnumerator<Tuple<K, V>> GetEnumerator(Node node)
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

        #endregion

        public override string ToString()
        {
            return $"Count: {Count}, Depth: {_rootNode.Depth}";
        }
    }
}