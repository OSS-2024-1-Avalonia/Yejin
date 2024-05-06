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
        private struct Line
        {
            public int Index;
            public bool IsHorizontal;
        }
        
        private const int ImageSize = 32;
        private const int MaxBlockCount = 1;
        
        public List<BoardCell> BoardCells { get; } = new List<BoardCell>();
        public List<Block> Blocks { get; }

        private Block _selectedBlock;
        public Block SelectedBlock
        {
            get { return _selectedBlock; }
            private set { this.RaiseAndSetIfChanged(ref _selectedBlock, value); }
        }

        private const int Size = 8;
        private BlockGenerator blockGenerator;
        private readonly Bitmap[] _fillTiles = new Bitmap[4];
        public MainViewModel()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    BoardCells.Add(new BoardCell { X = i, Y = j });
                }
            }

            blockGenerator = new BlockGenerator();
            Blocks = blockGenerator.GenerateBlocks();
            _selectedBlock = Blocks[0];

            _fillTiles[0] = new Bitmap(AssetLoader.Open(new Uri("avares://BlockPuzzle/Assets/Tile.png")));
            for (var i = 1; i <= MaxBlockCount; i++)
            {
                _fillTiles[i] = new Bitmap(AssetLoader.Open(new Uri($"avares://BlockPuzzle/Assets/FillTile{i}.png")));
            }
        }

        public void StartDrag(Block block)
        {
            SelectedBlock = block;
        }

        public bool IsDestinationValid(Block block, Point selectedPoint, Panel? board)
        {
            if (board == null) return false;
            
            var columnIndex = (int) Math.Round(selectedPoint.X / ImageSize);
            var rowIndex = (int) Math.Round(selectedPoint.Y / ImageSize);
            
            foreach (var cell in block.Cells)
            { 
                if (cell.IsVisible == false) continue;
                
                var cellIndex = (rowIndex + cell.X) * Size + columnIndex + cell.Y;
                if (cellIndex < 0 || cellIndex >= Size * Size) return false;
                if (BoardCells[cellIndex].Count >= MaxBlockCount) return false;
            }
            
            return true;
        }

        public void Drop(Block block, Point selectedPoint, Panel? board)
        {
            if (board == null) return;
            
            // TODO: ㅗ, ㄹ 모양 버그 수정해야 함
            var columnIndex = (int) Math.Round(selectedPoint.X / ImageSize);
            var rowIndex = (int) Math.Round(selectedPoint.Y / ImageSize);
            
            var boardCellElements = board.Children;
            foreach (var cell in block.Cells)
            {
                if (cell.IsVisible == false) continue;
                
                var cellIndex = (rowIndex + cell.X) * Size + columnIndex + cell.Y;
                if (cellIndex < 0 || cellIndex >= Size * Size) return;

                var boardCell = BoardCells[cellIndex];
                boardCell.Count++;
                var image = (boardCellElements[cellIndex] as ContentPresenter)?.Child as Image;
                if (image != null)
                    image.Source = _fillTiles[boardCell.Count];
            }
            
            RemoveLines(FindRemoveLine(), boardCellElements);
        }
        
        private IEnumerable<Line> FindRemoveLine()
        {
            List<Line> lines = new List<Line>();
            for (var i = 0; i < Size; i++)
            {
                var horizontalCount = 0;
                var verticalCount = 0;
                for (var j = 0; j < Size; j++)
                {
                    if (BoardCells[i * Size + j].Count == MaxBlockCount)
                    {
                        horizontalCount++;
                    }
                    if (BoardCells[j * Size + i].Count == MaxBlockCount)
                    {
                        verticalCount++;
                    }
                }
                
                if (horizontalCount == Size)
                {
                    lines.Add(new Line { Index = i, IsHorizontal = true });
                }
                if (verticalCount == Size)
                {
                    lines.Add(new Line { Index = i, IsHorizontal = false });
                }
            }
            
            return lines;
        }
        
        private void RemoveLines(IEnumerable<Line> lines, Controls boardElements)
        {
            foreach (var line in lines)
            {
                for (var i = 0; i < Size; i++)
                {
                    var cellIndex = (line.IsHorizontal) ? line.Index * Size + i : i * Size + line.Index;
                    BoardCells[cellIndex].Count = 0;
                    var image = (boardElements[cellIndex] as ContentPresenter)?.Child as Image;
                    if (image != null)
                        image.Source = _fillTiles[0];
                }
            }
        }
    }
}
