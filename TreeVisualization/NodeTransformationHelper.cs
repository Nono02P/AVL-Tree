using Trees;

namespace TreeVisualization
{
    public static class NodeTransformationHelper
    {
        public static NodeViewModel Transform(this Node<int, int> node, bool isRoot = true)
        {
            NodeViewModel result = new NodeViewModel()
            {
                IsRoot = isRoot,
                Key = node.Key,
                Value = node.Value,
            };
            if (node.HasLeftChild)
                result.Left = node.Left.Transform(false);

            if (node.HasRightChild)
                result.Right = node.Right.Transform(false);
            return result;
        }
    }
}