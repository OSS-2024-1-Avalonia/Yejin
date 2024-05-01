using BlockPuzzle.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace BlockPuzzle.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public List<Cell> BoardCells { get; } = new List<Cell>();
        public List<Block> Blocks { get; }

        private Block? selectedBlock;
        public Block? SelectedBlock
        {
            get { return selectedBlock; }
            set { this.RaiseAndSetIfChanged(ref selectedBlock, value); }
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
            selectedBlock = Blocks[0];
        }

        public void StartDrag(Block block)
        {
            SelectedBlock = block;
        }
    }
}
