using BlockPuzzle.Models;
using ReactiveUI;
using System.Collections.Generic;

namespace BlockPuzzle.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public const int ImageSize = 32;
        
        public List<Cell> BoardCells { get; } = new List<Cell>();
        public List<Block> Blocks { get; }

        private Block _selectedBlock;
        public Block SelectedBlock
        {
            get { return _selectedBlock; }
            private set { this.RaiseAndSetIfChanged(ref _selectedBlock, value); }
        }

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
            _selectedBlock = Blocks[0];
        }

        public void StartDrag(Block block)
        {
            SelectedBlock = block;
        }
    }
}
