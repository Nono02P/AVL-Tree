using AVL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TreeVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            AVLTree<int, int> tree = new AVLTree<int, int>();

            List<int> list = Enumerable.Range(0, 100).OrderBy(a => Guid.NewGuid()).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                int item = list[i];
                tree.Add(item, item);
            }

            DataContext = tree.RootNode.Transform();
        }
    }
}