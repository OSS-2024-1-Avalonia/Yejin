using ReactiveUI;

namespace BlockPuzzle.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		public MainViewModel MainView { get; }
		public MainWindowViewModel()
        {
            MainView = new MainViewModel();
        }
    }
}