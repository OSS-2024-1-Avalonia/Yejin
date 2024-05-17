using System;
using BlockPuzzle.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using DynamicData;

namespace BlockPuzzle.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private const int ImageSize = 32;
        private const int MaxBlockCount = 3;
        
        public List<BoardCell> BoardCells => _board.BoardCells;
        
        public ObservableCollection<Block> Blocks { get; } = new();

        private Block _selectedBlock;
        public Block SelectedBlock
        {
            get =>  _selectedBlock;
            private set => this.RaiseAndSetIfChanged(ref _selectedBlock, value);
        }

        public long Score => _scoreCalculater.Score;
        
        private bool _isGameOver = false;
        public bool IsGameOver
        {
            get => _isGameOver;
            private set => this.RaiseAndSetIfChanged(ref _isGameOver, value);
        }
        
        private const int Size = 8;
        private readonly Board _board;
        private readonly BlockGenerator _blockGenerator;
        private readonly ScoreCalculater _scoreCalculater = new();

        public MainViewModel()
        {
            _board = new Board(Size, MaxBlockCount);
            _blockGenerator = new BlockGenerator(_board);
            
            Blocks.AddRange(_blockGenerator.GenerateBlocks());
            _selectedBlock = Blocks[0];
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
            
            if (columnIndex < 0 || rowIndex < 0) return false;
            
            foreach (var cell in block.Cells)
            { 
                if (cell.IsVisible == false) continue;
                
                var cellIndex = (rowIndex + cell.X) * Size + columnIndex + cell.Y;
                if (rowIndex + cell.X >= Size || columnIndex + cell.Y >= Size) return false;
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
                
                BoardCells[cellIndex].Count++;
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
            
            IsGameOver = Blocks.All(b => b.IsUsed || !_board.CanPlaceBlock(b));
            if (IsGameOver)
            {
                Console.WriteLine("Game Over!");
            }
            else
            {
                Console.WriteLine("Next Turn!");
            }
        }
    }
}
