using BlockPuzzle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlockPuzzle.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public IEnumerable<Cell> Cells { get; }

        private const int Size = 8;
        public MainViewModel()
        {
            var cells = new List<Cell>();
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    cells.Add(new Cell { X = i, Y = j, Value = $"{i},{j}" });
                }
            }

            Cells = new ObservableCollection<Cell>(cells);

            Console.WriteLine(cells.Count);
        }
    }
}
