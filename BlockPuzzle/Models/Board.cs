using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace BlockPuzzle.Models;

public class Board
{
    private struct Line
    {
        public int Index;
        public bool IsHorizontal;
    }

    private readonly int _size;
    private readonly int _maxBlockCount;
    private readonly Bitmap _tileImage;
    
    public List<BoardCell> BoardCells { get; }
    
    public Board(int size, int maxBlockCount)
    {
        _size = size;
        _maxBlockCount = maxBlockCount;
        
        BoardCells = new List<BoardCell>();
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                BoardCells.Add(new BoardCell { X = i, Y = j });
            }
        }
        
        _tileImage = new Bitmap(AssetLoader.Open(new Uri("avares://BlockPuzzle/Assets/Tile.png")));
    }

    public bool HasRemovableLine()
    {
        return FindRemoveLine().Any();
    }
    
    public int RemoveLines(Avalonia.Controls.Controls boardElements)
    {
        var lines = FindRemoveLine();
        
        foreach (var line in lines)
        {
            for (var i = 0; i < _size; i++)
            {
                var cellIndex = (line.IsHorizontal) ? line.Index * _size + i : i * _size + line.Index;
                BoardCells[cellIndex].Count = 0;
                var image = (boardElements[cellIndex] as ContentPresenter)?.Child as Image;
                if (image != null)
                    image.Source = _tileImage;
            }
        }

        return lines.Count();
    }
    
    private IEnumerable<Line> FindRemoveLine()
    {
        List<Line> lines = new List<Line>();
        for (var i = 0; i < _size; i++)
        {
            var horizontalCount = 0;
            var verticalCount = 0;
            for (var j = 0; j < _size; j++)
            {
                if (BoardCells[i * _size + j].Count == _maxBlockCount)
                {
                    horizontalCount++;
                }
                if (BoardCells[j * _size + i].Count == _maxBlockCount)
                {
                    verticalCount++;
                }
            }
                
            if (horizontalCount == _size)
            {
                lines.Add(new Line { Index = i, IsHorizontal = true });
            }
            if (verticalCount == _size)
            {
                lines.Add(new Line { Index = i, IsHorizontal = false });
            }
        }
            
        return lines;
    }
    
    public bool CanPlaceBlock(Block block)
    {
        for (var i = 0; i < _size; i++)
        {
            for (var j = 0; j < _size; j++)
            {
                if (BoardCells[i * _size + j].Count >= _maxBlockCount) continue;
                
                if (CanPlaceBlock(block, i, j)) return true;
            }
        }
        
        return false;
    }
    
    private bool CanPlaceBlock(Block block, int x, int y)
    {
        for (var i = 0; i < block.Size; i++)
        {
            for (var j = 0; j < block.Size; j++)
            {
                if (!block.Cells[i * block.Size + j].IsVisible) continue;
                
                if (x + i >= _size || y + j >= _size)
                {
                    return false;
                }
                    
                if (BoardCells[(x + i) * _size + y + j].Count >= _maxBlockCount)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}