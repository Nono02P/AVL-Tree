namespace TreeVisualization
{
    public class NodeDesignModel : NodeViewModel
    {
        public static NodeDesignModel Instance => new NodeDesignModel();

        public NodeDesignModel() : base()
        {
            Key = 10;
            Value = 30;
            Left = new NodeDesignModel(5, 50)
            {
                IsRed = true,
                Left = new NodeDesignModel(1, 20),
                Right = new NodeDesignModel(7, 25),
            };

            Right = new NodeDesignModel(25, 95)
            {
                IsRed = true,
                Left = new NodeDesignModel(22, 55),
            };
        }

        public NodeDesignModel(int key, int value) : base()
        {
            Key = key;
            Value = value;
        }
    }
}