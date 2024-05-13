using System;

namespace BlockPuzzle.Models;

public class ScoreCalculater
{
    private const int DefaultScore = 10;

    private long _score;
    public long Score => _score;

    public bool IsCombo { get; set; }
    
    public void AddScore(int weight = 1)
    {
        if (IsCombo)
        {
            weight *= 2;
        }

        _score += DefaultScore * weight;
        IsCombo = true;
    }
}