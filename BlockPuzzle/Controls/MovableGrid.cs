using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;

namespace BlockPuzzle.Controls
{
    public class MovableGrid : UniformGrid
    {
        private bool isPressed;
        private Point positionInBlock;
        private TranslateTransform transform = null!;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            isPressed = true;

            // Move the grid to root
            //if (Parent is StackPanel panel)
            //{
            //    panel.Children.Remove(this);
            //    //if (panel.Parent is StackPanel parentPanel)
            //    //{
            //    //    parentPanel.Children.Remove(panel);
            //    //    panel.Children.Add(panel);
            //    //}
            //}

            positionInBlock = e.GetPosition((Visual?)Parent);

            if (transform != null!)
            {
                positionInBlock = new Point(
                    positionInBlock.X - transform.X,
                    positionInBlock.Y - transform.Y);
            }

            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            isPressed = false;

            base.OnPointerReleased(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (!isPressed) return;
            if (Parent == null) return;

            var position = e.GetPosition((Visual?)Parent);
            var offsetX = position.X - positionInBlock.X;
            var offsetY = position.Y - positionInBlock.Y;

            transform = new TranslateTransform(offsetX, offsetY);
            RenderTransform = transform;

            base.OnPointerMoved(e);
        }
    }
}
