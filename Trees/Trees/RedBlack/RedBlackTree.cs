using System;

namespace Trees
{
    public class RedBlackTree<K, V> : BaseBinaryTree<K, V> where K : IComparable
    {
        public new RedBlackNode<K, V> RootNode { get { return (RedBlackNode<K, V>)base.RootNode; } set { base.RootNode = value; } }

        #region Insertion

        /// <summary>
        /// Insert an element (Key, Value) in the Red Black tree.
        /// </summary>
        /// <param name="key">The key of the element.</param>
        /// <param name="value">The value associated with the key.</param>
        public override void Add(K key, V value)
        {
            RedBlackNode<K, V> node = new RedBlackNode<K, V>(key, value);
            if (RootNode == null)
                RootNode = node;
            else
            {
                node.IsRed = true;
                RecursiveInsertion(node, RootNode, null);
            }

            Count++;
        }

        /// <summary>
        /// Recursively call the children (from the root node) to insert the new node at the correct place.
        /// </summary>
        /// <param name="existing">The current node (already exists in the tree).</param>
        /// <param name="inserted">The node to insert in the tree.</param>
        private void RecursiveInsertion(RedBlackNode<K, V> inserted, RedBlackNode<K, V> existing, RedBlackNode<K, V> parent)
        {
            switch (existing.Key.CompareTo(inserted.Key))
            {
                case 1: // The existing node is greather than inserted
                    if (!existing.HasLeftChild)
                    {
                        existing.Left = inserted;
                        RefreshTree(inserted, existing, parent);
                    }
                    else
                        RecursiveInsertion(inserted, existing.Left, existing);
                    break;

                case -1: // The existing node is less than inserted
                    if (!existing.HasRightChild)
                    {
                        existing.Right = inserted;
                        RefreshTree(inserted, existing, parent);
                    }
                    else
                        RecursiveInsertion(inserted, existing.Right, existing);
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
            throw new NotImplementedException();
        }

        #endregion

        #region Refresh nodes

        /// <summary>
        /// A recursive method to refresh the tree by calculating the depth/weight of a node and his children. 
        /// If necessary performs rotation.
        /// </summary>
        /// <param name="node">The node to refresh (will recursively call his children).</param>
        /// <returns></returns>
        private void RefreshTree(RedBlackNode<K, V> node, RedBlackNode<K, V> parent, RedBlackNode<K, V> grandParent)
        {
            if (node != null)
            {
                if (parent == null) // Root node
                    node.IsRed = false;
                else if (parent.IsBlack)  // If parent is black, the tree is correct
                    return;
                else
                {
                    RedBlackNode<K, V> uncle = GetBrother(parent, grandParent);
                    if (uncle != null && uncle.IsRed)
                    {
                        parent.IsBlack = true;
                        uncle.IsBlack = true;

                        if (grandParent != RootNode)
                            grandParent.IsRed = true;

                        Tuple<RedBlackNode<K, V>, RedBlackNode<K, V>> ancesters = SearchAncesters(grandParent, RootNode);
                        RefreshTree(grandParent, ancesters.Item1, ancesters.Item2);
                    }
                    else
                    {
                        Tuple<RedBlackNode<K, V>, RedBlackNode<K, V>> ancesters = SearchAncesters(grandParent, RootNode);
                        if (ancesters.Item1 != null)
                        {
                            if (ancesters.Item1.Left == grandParent)
                                ancesters.Item1.Left = Rotate(node, parent, grandParent);
                            else if (ancesters.Item1.Right == grandParent)
                                ancesters.Item1.Right = Rotate(node, parent, grandParent);
                        }
                        else 
                            RootNode = Rotate(node, parent, grandParent);
                    }
                }
            }
        }

        #endregion

        #region Rotations

        /// <summary>
        /// Determines the rotation to do from the unbalanced node.
        /// </summary>
        /// <param name="node">The unbalanced node</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private RedBlackNode<K, V> Rotate(RedBlackNode<K, V> node, RedBlackNode<K, V> parent, RedBlackNode<K, V> grandParent)
        {
            if (node == grandParent.Left?.Right)
            {
                node.IsBlack = true;
                grandParent.IsRed = true;
                return LeftRightRotation(grandParent);
            }
            else if (node == grandParent.Right?.Left)
            {
                node.IsBlack = true;
                grandParent.IsRed = true;
                return RightLeftRotation(grandParent);
            }
            else if (node == grandParent.Left?.Left)
            {
                parent.IsBlack = true;
                grandParent.IsRed = true;
                return LeftLeftRotation(grandParent);
            }
            else if (node == grandParent.Right?.Right)
            {
                parent.IsBlack = true;
                grandParent.IsRed = true;
                return RightRightRotation(grandParent);
            }
            else
            {
                throw new Exception("Unexpected rotation");
            }
        }

        /// <summary>
        /// Performs a left-left rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private RedBlackNode<K, V> LeftLeftRotation(RedBlackNode<K, V> node)
        {
            RedBlackNode<K, V> c = node;
            node = node.Left;
            RedBlackNode<K, V> r = node.Right;
            node.Right = c;
            c.Left = r;

            return node;
        }

        /// <summary>
        /// Performs a left-right rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private RedBlackNode<K, V> LeftRightRotation(RedBlackNode<K, V> node)
        {
            RedBlackNode<K, V> c = node;
            node = node.Left.Right;
            RedBlackNode<K, V> l = node.Left;
            RedBlackNode<K, V> r = node.Right;

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
        private RedBlackNode<K, V> RightRightRotation(RedBlackNode<K, V> node)
        {
            RedBlackNode<K, V> c = node;
            node = node.Right;
            RedBlackNode<K, V> l = node.Left;
            node.Left = c;
            c.Right = l;

            return node;
        }

        /// <summary>
        /// Performs a right-left rotation from a given node.
        /// </summary>
        /// <param name="node">The node on which apply the rotation.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private RedBlackNode<K, V> RightLeftRotation(RedBlackNode<K, V> node)
        {
            RedBlackNode<K, V> c = node;
            node = node.Right.Left;
            RedBlackNode<K, V> l = node.Left;
            RedBlackNode<K, V> r = node.Right;

            node.Right = c.Right;
            node.Left = c;

            c.Right.Left = r;
            c.Right = l;

            return node;
        }

        #endregion

        #region Helper method

        private RedBlackNode<K, V> GetBrother(RedBlackNode<K, V> node, RedBlackNode<K, V> parent)
        {
            if (parent != null)
            {
                if (node == parent.Left)
                    return parent.Right;
                else if (node == parent.Right)
                    return parent.Left;
            }
            return null;
        }

        private Tuple<RedBlackNode<K, V>, RedBlackNode<K, V>> SearchAncesters(RedBlackNode<K, V> searchedNode, RedBlackNode<K, V> currentNode, RedBlackNode<K, V> parent = null, RedBlackNode<K, V> grandParent = null)
        {
            switch (currentNode.Key.CompareTo(searchedNode.Key))
            {
                case 1: // The currentNode is greather than searchedNode
                    return SearchAncesters(searchedNode, currentNode.Left, currentNode, parent);

                case -1: // The currentNode is less than searchedNode
                    return SearchAncesters(searchedNode, currentNode.Right, currentNode, parent);

                default: // The currentNode is the searchedNode
                    return new Tuple<RedBlackNode<K, V>, RedBlackNode<K, V>>(parent, grandParent);
            }
        }

        #endregion
    }
}