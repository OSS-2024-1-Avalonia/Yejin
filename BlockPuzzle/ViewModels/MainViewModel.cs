using System;
using BlockPuzzle.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DynamicData;

namespace BlockPuzzle.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int ImageSize = 32;
        private const int MaxBlockCount = 1;
        
        public List<BoardCell> BoardCells => _board.BoardCells;
        
        public ObservableCollection<Block> Blocks { get; } = new();

        private Block _selectedBlock;
        public Block SelectedBlock
        {
            get =>  _selectedBlock;
            private set => this.RaiseAndSetIfChanged(ref _selectedBlock, value);
        }

        public long Score => _scoreCalculater.Score;
        
        private const int Size = 8;
        private readonly Board _board;
        private readonly BlockGenerator _blockGenerator = new();
        private readonly ScoreCalculater _scoreCalculater = new();
        private readonly Bitmap[] _fillTiles = new Bitmap[4];
        public MainViewModel()
        {
            _board = new Board(Size, MaxBlockCount);
            Blocks.AddRange(_blockGenerator.GenerateBlocks());
            _selectedBlock = Blocks[0];
            
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
            if (block.IsUsed || board == null) return false;
            
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
            
            // TODO: 영역 벗어나도 블록 놔지는 버그 수정해야 함
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

            if (_board.HasRemovableLine())
            {
                
                var lineCount = _board.RemoveLines(boardCellElements);
                _scoreCalculater.AddScore(lineCount);
                this.RaisePropertyChanged(nameof(Score));
            }
            else
            {
                _scoreCalculater.IsCombo = false;
            }
            
            Blocks[block.Id].IsUsed = true;
            if (Blocks.All(b => b.IsUsed))
            {
                Blocks.Clear();
                Blocks.AddRange(_blockGenerator.GenerateBlocks());
            }
        }
    }
}
