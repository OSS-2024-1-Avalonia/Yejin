using System.Reactive;
using ReactiveUI;

namespace BlockPuzzle.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;

    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }
    
    public LobbyViewModel LobbyViewModel { get; }
    public GameViewModel GameViewModel { get; }
    
    public MainViewModel()
    {
        LobbyViewModel = new LobbyViewModel(this);
        GameViewModel = new GameViewModel();
        _contentViewModel = LobbyViewModel;
    }
    
    public void LoadGame()
    {
        ContentViewModel = GameViewModel;
    }
}