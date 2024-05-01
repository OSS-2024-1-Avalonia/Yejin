using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using BlockPuzzle.Controls;
using BlockPuzzle.Models;
using BlockPuzzle.ViewModels;
using System;

namespace BlockPuzzle.Views
{
    public partial class MainView : UserControl
    {
        private Point selectedPosition = new(0, 0);
        private readonly Point mouseOffset = new(-5, -5);

        public MainView()
        {
            InitializeComponent();

            AddHandler(DragDrop.DragOverEvent, DragOver);
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
            selectedPosition = new Point(selectedPos.X + mouseOffset.X, selectedPos.Y + mouseOffset.Y);

            var mousePos = e.GetPosition(MainPanel);
            var offsetX = mousePos.X - selectedPos.X;
            var offsetY = mousePos.Y - selectedPos.Y + mouseOffset.X;
            SelectedBlockGrid.RenderTransform = new TranslateTransform(offsetX, offsetY);

            if (DataContext is not MainViewModel viewModel) return;
            viewModel.StartDrag(block);

            SelectedBlockGrid.IsVisible = true;

            var dragData = new DataObject();
            dragData.Set("Block", block);
            var result = await DragDrop.DoDragDrop(e, dragData, DragDropEffects.None);
            Console.WriteLine($"Drag result: {result}");
            SelectedBlockGrid.IsVisible = false;
        }

        private void DragOver(object? sender, DragEventArgs e)
        {
            var currentPosition = e.GetPosition(MainPanel);

            var offsetX = currentPosition.X - selectedPosition.X;
            var offsetY = currentPosition.Y - selectedPosition.Y;

            SelectedBlockGrid.RenderTransform = new TranslateTransform(offsetX, offsetY);
            e.DragEffects = DragDropEffects.Move;

            // set drag cursor icon
            //e.DragEffects = DragDropEffects.Move;
            //if (DataContext is not DragAndDropPageViewModel vm) return;
            //var data = e.Data.Get(DragAndDropPageViewModel.CustomFormat);
            //if (data is not TaskItem taskItem) return;
            //if (!vm.IsDestinationValid(taskItem, (e.Source as Control)?.Name))
            //{
            //    e.DragEffects = DragDropEffects.None;
            //}
        }
    }
}