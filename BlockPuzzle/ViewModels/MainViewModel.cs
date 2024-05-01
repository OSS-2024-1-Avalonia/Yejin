using BlockPuzzle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlockPuzzle.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public List<Cell> BoardCells { get; } = new List<Cell>();
        public List<Block> Blocks { get; }
        public int MaxSize => Blocks.Select(b => b.Size).Max();

        private const int Size = 8;
        private BlockGenerator blockGenerator;
        public MainViewModel()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    BoardCells.Add(new Cell { X = i, Y = j });
                }
            }

            blockGenerator = new BlockGenerator();
            Blocks = blockGenerator.GenerateBlocks();
        }
    }
}
