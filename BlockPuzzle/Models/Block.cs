using System;
using System.Collections.Generic;

namespace BlockPuzzle.Models
{
    public class Block : ICloneable
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public required List<BlockCell> Cells { get; set; }
        public bool IsUsed { get; set; } = false;
        
        public object Clone()
        {
            return new Block
            {
                Id = Id,
                Size = Size,
                Cells = new List<BlockCell>(Cells),
                IsUsed = IsUsed
            };
        }
    }
}
