using AVL;

namespace TreeVisualization
{
    public static class NodeTransformationHelper
    {
        public static NodeViewModel Transform(this AVLTree<int, int>.Node node)
        {
            NodeViewModel result = new NodeViewModel()
            {
                Key = node.Key,
                Value = node.Value
            };
            if (node.HasLeftChild)
                result.Left = node.Left.Transform();

            if (node.HasRightChild)
                result.Right = node.Right.Transform();
            return result;
        }
    }
}