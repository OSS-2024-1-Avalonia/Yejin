using System.Collections.Generic;

namespace BlockPuzzle.Models
{
    public class Block
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public required List<BlockCell> Cells { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
