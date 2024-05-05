using System;
using BlockPuzzle.Models;
using ReactiveUI;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

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

        public void Drop(Block block, Point selectedPoint, Panel? board)
        {
            if (board == null) return;
            
            var columnIndex = (int) Math.Round(selectedPoint.X / ImageSize);
            var rowIndex = (int) Math.Round(selectedPoint.Y / ImageSize);

            var cells = block.Cells;
            var boardCellElements = board.Children;
            foreach (var cell in cells)
            {
                if (cell.IsVisible == false) continue;
                
                var cellIndex = (rowIndex + cell.X) * Size + columnIndex + cell.Y;
                if (cellIndex < 0 || cellIndex >= Size * Size) return;

                var image = (boardCellElements[cellIndex] as ContentPresenter)?.Child as Image;
                if (image != null)
                    image.Source = new Bitmap(AssetLoader.Open(new Uri("avares://BlockPuzzle/Assets/Block.png")));
            }
        }
    }
}
