using ReactiveUI;

namespace BlockPuzzle.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		public MainViewModel MainView { get; } = new();
	}
}