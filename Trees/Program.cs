using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            AVLTree<int, int> tree = new AVLTree<int, int>();

            List<int> list = Enumerable.Range(0, 1000).OrderBy(a => Guid.NewGuid()).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                int item = list[i];
                tree.Add(item, item);
            }

            Console.WriteLine("Insertion done.");

            Random rnd = new Random(0);
            while (list.Count > 0)
            {
                int indexToRemove = rnd.Next(list.Count);
                int keyToRemove = list[indexToRemove];
                list.RemoveAt(indexToRemove);
                tree.Delete(keyToRemove);

                try
                {
                    tree.GetValue(keyToRemove);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "This key doesnt exist" || ex.Message == "The tree is empty")
                    {

                    }
                    else
                    {
                        throw;
                    }
                }

                Debug.Assert(list.Count == tree.Count);

                CheckTree(tree);
            }

            Console.WriteLine(tree.ToString());

            Console.ReadLine();
        }

        private static void CheckTree<K, V>(AVLTree<K, V> tree) where K : IComparable
        {
            K previous = default(K);
            foreach (Tuple<K, V> item in tree)
            {
                if (item.Item1.CompareTo(previous) < 0)
                    throw new Exception("Wrong tree");
                previous = item.Item1;
            }
        }
    }
}