namespace BlockPuzzle.Models
{
    public class BlockCell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsVisible { get; set; } = false;
    }
}
