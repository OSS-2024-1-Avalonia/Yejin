using System.Collections.Generic;

namespace BlockPuzzle.Models
{
    public class Block
    {
        public int Size { get; set; }
        public required List<BlockCell> Cells { get; set; }
    }
}
