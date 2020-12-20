using System;
using System.Collections.Generic;
using System.Linq;

namespace AVL
{
    class Program
    {
        static void Main(string[] args)
        {
            AVLTree<int, int> tree = new AVLTree<int, int>();

            List<int> list = Enumerable.Range(0, 10000).OrderBy(a => Guid.NewGuid()).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                int item = list[i];
                tree.Add(item, item);
            }

            tree.GetValue(555);

            Console.WriteLine(tree.ToString());
            
            Console.ReadLine();
        }
    }
}