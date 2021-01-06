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
        /// <param name="inserted">The node to insert in the tree.</param>
        /// <param name="existing">The current node (already exists in the tree).</param>
        /// <param name="parent">The parent of the current node (already exists in the tree).</param>
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
        /// A recursive method to refresh the tree by checking the different scenario of the red black tree. 
        /// If necessary performs rotation and recolor nodes.
        /// </summary>
        /// <param name="node">The node to refresh.</param>
        /// <param name="parent">The parent of the node to refresh.</param>
        /// <param name="grandParent">The grandparent of the node to refresh.</param>
        private void RefreshTree(RedBlackNode<K, V> node, RedBlackNode<K, V> parent, RedBlackNode<K, V> grandParent)
        {
            if (node != null)
            {
                if (node == RootNode) // the node is the Root node
                    node.IsBlack = true;
                else if (parent.IsBlack)  // If parent is black, the tree is already correct
                    return;
                else
                {
                    RedBlackNode<K, V> uncle = GetBrother(parent, grandParent);
                    Ancesters<K, V> ancesters = SearchAncesters(grandParent, RootNode);   // Find ancesters (parent and grandparent) of the grandparent node.
                    if (uncle != null && uncle.IsRed)   // If uncle is red : recolor parent, uncle, and grandparent (is it is not the root). 
                    {
                        parent.IsBlack = true;
                        uncle.IsBlack = true;

                        if (grandParent != RootNode)
                            grandParent.IsRed = true;

                        RefreshTree(grandParent, ancesters.Parent, ancesters.GrandParent); // Then refresh the grandparent.
                    }
                    else // If there is no uncle or if it is a black uncle.
                    {
                        // Find the position of the grandparent in the tree. 
                        // Performs the rotation and put in place the new topmost node
                        if (ancesters.Parent != null)
                        {
                            if (ancesters.Parent.Left == grandParent)
                                ancesters.Parent.Left = Rotate(node, parent, grandParent);
                            else if (ancesters.Parent.Right == grandParent)
                                ancesters.Parent.Right = Rotate(node, parent, grandParent);
                        }
                        else
                            RootNode = Rotate(node, parent, grandParent);
                    }
                }
            }
            else
                throw new Exception("The node to refresh could not be null");
        }

        #endregion

        #region Rotations

        /// <summary>
        /// Determines the rotation to do depending of the position of the node in his ancesters.
        /// </summary>
        /// <param name="node">The future topmost node.</param>
        /// <returns>Gives the new top node after performing the rotation.</returns>
        private RedBlackNode<K, V> Rotate(RedBlackNode<K, V> node, RedBlackNode<K, V> parent, RedBlackNode<K, V> grandParent)
        {
            grandParent.IsRed = true;

            if (parent == grandParent.Left)
            {
                if (node == parent.Left)        // Left-Left
                {
                    parent.IsBlack = true;
                    return (RedBlackNode<K, V>)LeftLeftRotation(grandParent);
                }
                else if (node == parent.Right)  // Left-Right
                {
                    node.IsBlack = true;
                    return (RedBlackNode<K, V>)LeftRightRotation(grandParent);
                }
            }
            else if (parent == grandParent.Right)
            {
                if (node == parent.Left)        // Right-Left
                {
                    node.IsBlack = true;
                    return (RedBlackNode<K, V>)RightLeftRotation(grandParent);
                }
                else if (node == parent.Right)  // Right-Right
                {
                    parent.IsBlack = true;
                    return (RedBlackNode<K, V>)RightRightRotation(grandParent);
                }
            }
            throw new Exception("Unexpected rotation");
        }

        #endregion

        #region Searching familly methods

        /// <summary>
        /// Get the brother of a specific node.
        /// </summary>
        /// <param name="node">The node that need to find his brother.</param>
        /// <param name="parent">The common parent of the nodes.</param>
        /// <returns>The brother of the specified <paramref name="node"/>.</returns>
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

        /// <summary>
        /// Search the ancesters (parent, grandparent) of the <paramref name="searchedNode"/>.
        /// </summary>
        /// <param name="searchedNode">The node which need to find his ancesters.</param>
        /// <param name="currentNode">The current node (starts typically from the root node and recursively search in the successor).</param>
        /// <param name="parent">The parent of the current node.</param>
        /// <param name="grandParent">The grandparent of the current node.</param>
        /// <returns>The ancesters of the specified <paramref name="searchedNode"/>.</returns>
        private Ancesters<K, V> SearchAncesters(RedBlackNode<K, V> searchedNode, RedBlackNode<K, V> currentNode, RedBlackNode<K, V> parent = null, RedBlackNode<K, V> grandParent = null)
        {
            switch (currentNode.Key.CompareTo(searchedNode.Key))
            {
                case 1: // The currentNode is greather than searchedNode
                    return SearchAncesters(searchedNode, currentNode.Left, currentNode, parent);

                case -1: // The currentNode is less than searchedNode
                    return SearchAncesters(searchedNode, currentNode.Right, currentNode, parent);

                default: // The currentNode is the searchedNode
                    return new Ancesters<K, V>(parent, grandParent);
            }
        }

        #endregion
    }
}