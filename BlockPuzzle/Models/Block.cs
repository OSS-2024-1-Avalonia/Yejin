using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BlockPuzzle.Models
{
    public class Block : ICloneable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public required List<BlockCell> Cells { get; set; }
        
        private bool _isUsed;

        public bool IsUsed
        {
            get => _isUsed;
            set
            {
                if (_isUsed != value)
                {
                    _isUsed = value;
                    OnPropertyChanged(nameof(IsUsed));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
