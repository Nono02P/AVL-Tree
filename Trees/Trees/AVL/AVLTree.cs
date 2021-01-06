using System;

namespace Trees
{
    public class AVLTree<K, V> : BaseBinaryTree<K, V> where K : IComparable
    {
        public new AvlNode<K, V> RootNode { get { return (AvlNode<K, V>)base.RootNode; } set { base.RootNode = value; } }

        #region Insertion

        /// <summary>
        /// Insert an element (Key, Value) in the AVL tree.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value associated with the key.</param>
        public override void Add(K key, V value)
        {
            AvlNode<K, V> node = new AvlNode<K, V>(key, value);
            if (RootNode == null)
                RootNode = node;
            else
                RecursiveInsertion(RootNode, node);

            RootNode = RefreshTree(RootNode);
            Count++;
        }

        /// <summary>
        /// Recursively call the children (from the root node) to insert the new node at the correct place.
        /// </summary>
        /// <param name="existing">The current node (already exists in the tree).</param>
        /// <param name="inserted">The node to insert in the tree.</param>
        private void RecursiveInsertion(Node<K, V> existing, Node<K, V> inserted)
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

        #region Deletion

        /// <summary>
        /// Delete the element associated with a defined key.
        /// </summary>
        /// <param name="key">The element associated with the key to delete.</param>
        public override void Delete(K key)
        {
            RootNode = RecursiveDeletion(RootNode, key);
            if (RootNode != null)
                RootNode = RefreshTree(RootNode);

        }

        /// <summary>
        /// Recursively calls the node and his children to find the element to remove, then removes it.
        /// </summary>
        /// <param name="node">The node to check (starts from the root and recursively calls the children).</param>
        /// <param name="key">The element associated with the key to delete.</param>
        /// <returns>True if deleted, false if the key doesn't exists.</returns>
        private AvlNode<K, V> RecursiveDeletion(AvlNode<K, V> node, K key)
        {
            bool deletionError = false;
            if (node == null)
                deletionError = true;
            else
            {
                switch (node.Key.CompareTo(key))
                {
                    case 1:
                        if (node.Left != null)
                            node.Left = RecursiveDeletion(node.Left, key);
                        else
                            deletionError = true;
                        break;

                    case -1:
                        if (node.Right != null)
                            node.Right = RecursiveDeletion(node.Right, key);
                        else
                            deletionError = true;
                        break;

                    default:
                        Count--;
                        AvlNode<K, V> current = RemoveNode(node);
                        return RefreshTree(current);
                }
            }
            if (deletionError)
                throw new Exception($"The deletion cannot be performed on the key = ({key}) because it doesn't exists.");
            else
                return node;
        }

        /// <summary>
        /// Removes the specified node and returns the new topest node.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        /// <returns>The new topest node.</returns>
        private AvlNode<K, V> RemoveNode(AvlNode<K, V> node)
        {
            if (node.IsLeaf)
            {
                return null;
            }
            else if (node.Left != null && node.Right != null) // Node have 2 children
            {
                AvlNode<K, V> parent = node;
                switch (node.Weight)
                {
                    case 1: // Left is heaviest
                    case 0: // Is balanced
                        AvlNode<K, V> biggestLeftNode = node.Left;
                        // Find the bigest node at left side.
                        bool hasBiggest = false;
                        while (biggestLeftNode.Right != null)
                        {
                            parent = biggestLeftNode;
                            biggestLeftNode = biggestLeftNode.Right;
                            hasBiggest = true;
                        }
                        if (biggestLeftNode.Left == null)
                        {
                            if (biggestLeftNode != node.Left)
                            {
                                biggestLeftNode.Left = node.Left;
                            }
                        }
                        else if (hasBiggest)
                        {
                            AvlNode<K, V> smallestLeftNode = biggestLeftNode.Left;
                            while (smallestLeftNode.Left != null)
                            {
                                smallestLeftNode = smallestLeftNode.Left;
                            }
                            smallestLeftNode.Left = node.Left;
                        }
                        if (hasBiggest)
                            parent.Right = null;
                        biggestLeftNode.Right = node.Right;

                        return biggestLeftNode;

                    case -1: // Right is heaviest
                        AvlNode<K, V> smallestRightNode = node.Right;
                        // Find the smallest node at right side.
                        bool hasSmallest = false;
                        while (smallestRightNode.Left != null)
                        {
                            parent = smallestRightNode;
                            smallestRightNode = smallestRightNode.Left;
                            hasSmallest = true;
                        }
                        if (smallestRightNode.Right == null)
                        {
                            if (smallestRightNode != node.Right)
                            {
                                smallestRightNode.Right = node.Right;
                            }
                        }
                        else if (hasSmallest)
                        {
                            AvlNode<K, V> biggestRightNode = smallestRightNode.Right;
                            while (biggestRightNode.Right != null)
                            {
                                biggestRightNode = biggestRightNode.Right;
                            }
                            biggestRightNode.Right = node.Right;
                        }
                        if (hasSmallest)
                            parent.Left = null;
                        smallestRightNode.Left = node.Left;

                        return smallestRightNode;

                    default:
                        throw new Exception("The tree is not balanced correctly before the deletion.");
                }
            }
            else if (node.Left != null) // Node have 1 child at left
            {
                return node.Left;
            }
            else // Node have 1 child at right
            {
                return node.Right;
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
        private AvlNode<K, V> RefreshTree(AvlNode<K, V> node)
        {
            if (node != null)
            {
                if (node.IsLeaf)
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
            }
            return node;
        }

        #endregion

        #region Rotations

        /// <summary>
        /// Determines the rotation to do from the unbalanced node.
        /// </summary>
        /// <param name="node">The unbalanced node</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private AvlNode<K, V> Rotate(AvlNode<K, V> node)
        {
            switch (Math.Sign(node.Weight))
            {
                case 1: // 2 Need left rotation
                    switch (node.Left.Weight)
                    {
                        case 1:
                            node = (AvlNode<K, V>)LeftLeftRotation(node);
                            break;
                        case -1:
                        default:
                            node = (AvlNode<K, V>)LeftRightRotation(node);
                            break;
                    }
                    break;

                case -1: // -2 Need right rotation
                    switch (node.Right.Weight)
                    {
                        case 1:
                            node = (AvlNode<K, V>)RightLeftRotation(node);
                            break;
                        case -1:
                        default:
                            node = (AvlNode<K, V>)RightRightRotation(node);
                            break;
                    }
                    break;

                default:
                    break;
            }
            // After a rotation, recalculate depth/weight of the node and his children 
            return RefreshTree(node);
        }

        #endregion
    }
}