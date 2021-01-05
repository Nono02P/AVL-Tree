using System;

namespace Trees
{
    public class BinaryTree<K, V> : BaseBinaryTree<K, V> where K : IComparable
    {
        #region Insertion

        /// <summary>
        /// Insert an element (Key, Value) in the Binary tree.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value associated with the key.</param>
        public override void Add(K key, V value)
        {
            Node<K, V> node = new Node<K, V>(key, value);
            if (RootNode == null)
                RootNode = node;
            else
                RecursiveInsertion(RootNode, node);

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
        }

        /// <summary>
        /// Recursively calls the node and his children to find the element to remove, then removes it.
        /// </summary>
        /// <param name="node">The node to check (starts from the root and recursively calls the children).</param>
        /// <param name="key">The element associated with the key to delete.</param>
        /// <returns>True if deleted, false if the key doesn't exists.</returns>
        private Node<K, V> RecursiveDeletion(Node<K, V> node, K key)
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
                        return RemoveNode(node);
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
        private Node<K, V> RemoveNode(Node<K, V> node)
        {
            if (node.IsLeaf)
            {
                return null;
            }
            else if (node.Left != null && node.Right != null) // Node have 2 children
            {
                Node<K, V> parent = node;
                Node<K, V> biggestLeftNode = node.Left;
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
                    Node<K, V> smallestLeftNode = biggestLeftNode.Left;
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
    }
}