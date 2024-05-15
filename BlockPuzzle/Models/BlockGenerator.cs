using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockPuzzle.Models
{
    public class BlockGenerator
    {
        private readonly Board _board;
        private readonly List<Block> _blocks;

        public BlockGenerator(Board board)
        {
            _board = board;
            _blocks = new List<Block>()
            {
                new Block{ Size = 1, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 } } },
                new Block{ Size = 2, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 } } },
                new Block{ Size = 2, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 1, Y = 0 } } }, 
                new Block{ Size = 2, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 1, Y = 0 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 } } },
                new Block{ Size = 2, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 1, Y = 1 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 1, Y = 1 }, new BlockCell { X = 1, Y = 2 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 1, Y = 1 }, new BlockCell { X = 1, Y = 2 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 1, Y = 1 } } },
                new Block{ Size = 4, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 2, Y = 0 }, new BlockCell { X = 3, Y = 0 } } },
                new Block{ Size = 5, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 0, Y = 3 }, new BlockCell { X = 0, Y = 4 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 1, Y = 2 }, new BlockCell { X = 2, Y = 2 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 1, Y = 1 }, new BlockCell { X = 1, Y = 2 }, new BlockCell { X = 2, Y = 0 }, new BlockCell { X = 2, Y = 1 }, new BlockCell { X = 2, Y = 2 } } },
                new Block{ Size = 1, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 } } },
            };

            foreach (var block in _blocks)
            {
                block.Cells.ForEach(cell => cell.IsVisible = true);

                var cells = block.Cells;
                for (int i = 0; i < block.Size; i++)
                {
                    for (int j = 0; j < block.Size; j++)
                    {
                        if (!cells.Any(cell => cell.X == i && cell.Y == j))
                        {
                            cells.Add(new BlockCell { X = i, Y = j });
                        }
                    }
                }

                block.Cells.Sort((a, b) => a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));
            }
        }

        private int SelectValidBlock()
        {
            var random = new Random();
            var indices = Enumerable.Range(0, _blocks.Count).OrderBy(x => random.Next()).ToList();
            foreach (var index in indices)
            {
                var block = _blocks[index];
                if (_board.CanPlaceBlock(block)) return index;    
            }

            return -1;
        }

        public List<Block> GenerateBlocks()
        {
            var indices = new int[3];
            var firstBlockIndex = SelectValidBlock();

            var count = 0;
            if (firstBlockIndex != -1)
            {
                indices[count] = firstBlockIndex;
                count++;
            }

            var random = new Random();
            while (count < 3)
            {
                var index = random.Next(_blocks.Count);
                if (!indices.Contains(index))
                {
                    indices[count] = index;
                    count++;
                }
            }

            var result = new List<Block>();
            var maxRow = indices.Select(i => _blocks[i].Size).Max();
            for (var i = 0; i < 3; i++)
            {
                var block = _blocks[indices[i]].Clone() as Block;
                block.Id = i;
                block.IsUsed = false;
                ModifyRow(maxRow, ref block);
                result.Add(block);
            }
            return result;
        }

        private void ModifyRow(int maxRow, ref Block block)
        {
            var cells = block.Cells;
            for (var j = block.Size; j < maxRow; j++)
            {
                for (var k = 0; k < block.Size; k++)
                {
                    cells.Add(new BlockCell { X = j, Y = k });
                }
            }
        }
    }
}
