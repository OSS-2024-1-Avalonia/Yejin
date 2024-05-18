using System;
using BlockPuzzle.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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
        public ReactiveCommand<Unit, Unit> ResetCommand { get; }
        
        private Block _selectedBlock;
        public Block SelectedBlock
        {
            get =>  _selectedBlock;
            private set => this.RaiseAndSetIfChanged(ref _selectedBlock, value);
        }

        private long _score;
        public long Score
        {
            get => _score;
            private set => this.RaiseAndSetIfChanged(ref _score, value);
        }
        private bool _isCombo;
        
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
            ResetCommand = ReactiveCommand.Create(Reset);
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
                Score += _scoreCalculater.Calculate(_isCombo);
                _isCombo = true;
            }
            else
            {
                _isCombo = false;
            }
            
            Blocks[block.Id].IsUsed = true;
            if (Blocks.All(b => b.IsUsed))
            {
                Blocks.Clear();
                Blocks.AddRange(_blockGenerator.GenerateBlocks());
            }
            
            IsGameOver = Blocks.All(b => b.IsUsed || !_board.CanPlaceBlock(b));
        }

        private void Reset()
        {
            _board.Reset();
            Score = 0;
            IsGameOver = false;
            Blocks.Clear();
            Blocks.AddRange(_blockGenerator.GenerateBlocks());
        }
    }
}
