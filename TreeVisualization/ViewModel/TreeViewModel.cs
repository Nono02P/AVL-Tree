using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Trees;

namespace TreeVisualization
{
    public class TreeViewModel : BaseViewModel
    {
        private enum eInsertionMethod { Random, Ascending, Descending };

        private BaseBinaryTree<int, int> _tree;

        public NodeViewModel Root { get; private set; }
        public int Count => _tree.Count;
        public int CurrentKey { get; set; }
        public int CurrentValue { get; set; }
        public int NbNodesToGenerate { get; set; } = 100;

        public ICommand GenerateBinaryTreeCommand { get; set; }
        public ICommand GenerateAVLTreeCommand { get; set; }
        public ICommand GenerateRedBlackTreeCommand { get; set; }
        public ICommand RandomizeInsertionCommand { get; set; }
        public ICommand AscendingInsertionCommand { get; set; }
        public ICommand DescendingInsertionCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        #region Constructor
        public TreeViewModel()
        {
            _tree = new AVLTree<int, int>();

            GenerateBinaryTreeCommand = new SimpleCommand(GenerateBinaryTree);
            GenerateAVLTreeCommand = new SimpleCommand(GenerateAVLTree);
            GenerateRedBlackTreeCommand = new SimpleCommand(GenerateRedBlackTree);

            RandomizeInsertionCommand = new SimpleCommand(() => RangeInsertion(eInsertionMethod.Random));
            AscendingInsertionCommand = new SimpleCommand(() => RangeInsertion(eInsertionMethod.Ascending));
            DescendingInsertionCommand = new SimpleCommand(() => RangeInsertion(eInsertionMethod.Descending));

            AddCommand = new SimpleCommand(Add);
            DeleteCommand = new SimpleCommand(Delete);
            ClearCommand = new SimpleCommand(Clear);
            Root = _tree.RootNode?.Transform();
        } 
        #endregion

        #region Generate trees methods
        private void GenerateBinaryTree()
        {
            Task.Run(() =>
            {
                lock (_tree)
                {
                    _tree = new BinaryTree<int, int>();
                    Root = null;
                }
            });
        }


        private void GenerateAVLTree()
        {
            Task.Run(() =>
            {
                lock (_tree)
                {
                    _tree = new AVLTree<int, int>();
                    Root = null;
                }
            });
        }

        private void GenerateRedBlackTree()
        {
            Task.Run(() =>
            {
                lock (_tree)
                {
                    _tree = new RedBlackTree<int, int>();
                    Root = null;
                }
            });
        }
        #endregion

        #region Adding methods
        private void Add()
        {
            try
            {
                _tree.Add(CurrentKey, CurrentValue);
                RaisePropertyChanged("Count");
            }
            catch (Exception ex) { }
            Root = _tree.RootNode.Transform();
        }

        private void RangeInsertion(eInsertionMethod method)
        {
            Task.Run(() =>
            {
                lock (_tree)
                {
                    _tree.Clear();

                    IEnumerable<int> enumerable = Enumerable.Range(0, NbNodesToGenerate);
                    List<int> list = null;
                    
                    switch (method)
                    {
                        case eInsertionMethod.Random:
                            list = enumerable.OrderBy(a => Guid.NewGuid()).ToList();
                            break;
                        case eInsertionMethod.Ascending:
                            list = enumerable.ToList();
                            break;
                        case eInsertionMethod.Descending:
                            list = enumerable.Reverse().ToList();
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        int item = list[i];
                        _tree.Add(item, item);
                        Root = _tree.RootNode.Transform();
                        RaisePropertyChanged("Count");
                        Thread.Sleep(5);
                    }
                }
            });
        }
        #endregion

        #region Deletion methods
        private void Delete()
        {
            try
            {
                _tree.Delete(CurrentKey);
                RaisePropertyChanged("Count");
            }
            catch { }
            Root = _tree.RootNode.Transform();
        }

        private void Clear()
        {
            _tree.Clear();
            RaisePropertyChanged("Count");
            Root = null;
        }
        #endregion
    }
}