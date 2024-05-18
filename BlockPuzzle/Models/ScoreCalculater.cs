using System;

namespace BlockPuzzle.Models;

public class ScoreCalculater
{
    private const int DefaultScore = 10;
    
    public long Calculate(bool isCombo, int weight = 1)
    {
        if (isCombo)
        {
            weight *= 2;
        }

        return DefaultScore * weight;
    }
}