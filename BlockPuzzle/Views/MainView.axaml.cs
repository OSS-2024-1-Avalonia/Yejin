using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using BlockPuzzle.Models;
using BlockPuzzle.ViewModels;
using System;
using System.Linq;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

namespace BlockPuzzle.Views
{
    public partial class MainView : UserControl
    {
        private Point _selectedPosition = new(0, 0);

        public MainView()
        {
            InitializeComponent();

            AddHandler(DragDrop.DragOverEvent, DragOver);
            AddHandler(DragDrop.DropEvent, Drop);
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            SelectedBlockGrid.IsVisible = false;
            base.OnLoaded(e);
        }

        private async void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            Console.WriteLine("Pointer pressed");

            if (sender is not UniformGrid grid) return;
            if (grid.DataContext is not Block block) return;

            var selectedPos = SelectedBlockGrid.Bounds.Position;
            _selectedPosition = new Point(selectedPos.X, selectedPos.Y);

            var mousePos = e.GetPosition(MainPanel);
            var offsetX = mousePos.X - selectedPos.X;
            var offsetY = mousePos.Y - selectedPos.Y;
            SelectedBlockGrid.RenderTransform = new TranslateTransform(offsetX, offsetY);

            if (DataContext is not MainViewModel viewModel) return;
            viewModel.StartDrag(block);

            SelectedBlockGrid.IsVisible = true;

            var dragData = new DataObject();
            dragData.Set("Block", block);
            var result = await DragDrop.DoDragDrop(e, dragData, DragDropEffects.Move);
            Console.WriteLine($"Drag result: {result}");
            SelectedBlockGrid.IsVisible = false;
        }

        private void DragOver(object? sender, DragEventArgs e)
        {
            var currentPosition = e.GetPosition(MainPanel);

            var offsetX = currentPosition.X - _selectedPosition.X;
            var offsetY = currentPosition.Y - _selectedPosition.Y;

            SelectedBlockGrid.RenderTransform = new TranslateTransform(offsetX, offsetY);
            e.DragEffects = DragDropEffects.Move;

            if (DataContext is not MainViewModel vm) return;
            var data = e.Data.Get("Block");
            if (data is not Block block) return;
            
            var boardPosition = e.GetPosition(Board);
            if (Board.Children.First() is not ItemsControl itemsControl) return;
            var boardPanel = itemsControl.ItemsPanelRoot;
            boardPosition -= boardPanel?.Bounds.Position ?? new Point();

            e.DragEffects = vm.IsDestinationValid(block, boardPosition, boardPanel)
                ? DragDropEffects.Move
                : DragDropEffects.None;
        }

        private void Drop(object? sender, DragEventArgs e)
        {
            Console.WriteLine("Drop");
            
            var data = e.Data.Get("Block");

            if (data is not Block block) return;
            if (DataContext is not MainViewModel vm) return;
            
            var boardPosition = e.GetPosition(Board);
            if (Board.Children.First() is not ItemsControl itemsControl) return;
            var boardPanel = itemsControl.ItemsPanelRoot;
            boardPosition -= boardPanel?.Bounds.Position ?? new Point();

            if (vm.IsDestinationValid(block, boardPosition, boardPanel))
                vm.Drop(block, boardPosition, boardPanel);
        }
    }
}