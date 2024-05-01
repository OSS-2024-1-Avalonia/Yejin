using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace BlockPuzzle.Models
{
    public class BlockGenerator
    {
        private List<Block> Blocks;

        public BlockGenerator()
        {
            Blocks = new List<Block>()
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
                new Block{ Size = 4, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 1, Y = 1 }, new BlockCell { X = 2, Y = 1 }, new BlockCell { X = 3, Y = 1 } } },
                new Block{ Size = 5, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 0, Y = 3 }, new BlockCell { X = 0, Y = 4 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 1, Y = 2 }, new BlockCell { X = 2, Y = 2 } } },
                new Block{ Size = 3, Cells = new List<BlockCell> { new BlockCell { X = 0, Y = 0 }, new BlockCell { X = 0, Y = 1 }, new BlockCell { X = 0, Y = 2 }, new BlockCell { X = 1, Y = 0 }, new BlockCell { X = 1, Y = 1 }, new BlockCell { X = 1, Y = 2 }, new BlockCell { X = 2, Y = 0 }, new BlockCell { X = 2, Y = 1 }, new BlockCell { X = 2, Y = 2 } } },
            };

            foreach (var block in Blocks)
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

        public List<Block> GenerateBlocks()
        {
            int[] indices = new int[3];
            int count = 0;
            while (count < 3)
            {
                var index = RandomNumberGenerator.GetInt32(Blocks.Count);
                if (!indices.Contains(index))
                {
                    indices[count] = index;
                    count++;
                }
            }

            var result = new List<Block>();
            int maxRow = indices.Select(i => Blocks[i].Size).Max();
            for (int i = 0; i < 3; i++)
            {
                var block = Blocks[indices[i]];
                ModifyRow(maxRow, ref block);
                result.Add(block);
            }
            return result;
        }

        private void ModifyRow(int maxRow, ref Block block)
        {
            var cells = block.Cells;
            for (int j = block.Size; j < maxRow; j++)
            {
                for (int k = 0; k < block.Size; k++)
                {
                    cells.Add(new BlockCell { X = j, Y = k });
                }
            }
        }
    }
}
