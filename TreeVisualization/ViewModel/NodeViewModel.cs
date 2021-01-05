namespace TreeVisualization
{
    public class NodeViewModel : BaseViewModel
    {
        public int Key { get; set; }
        public int Value { get; set; }
        public bool IsRed { get; set; }
        public bool IsRoot { get; set; }

        public bool IsLeaf => Left == null && Right == null;
        public bool HasLeftNode => Left != null;
        public bool HasRightNode => Right != null;

        public NodeViewModel Left { get; set; }
        public NodeViewModel Right { get; set; }
    }
}