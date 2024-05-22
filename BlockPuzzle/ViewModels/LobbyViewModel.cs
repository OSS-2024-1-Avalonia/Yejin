namespace BlockPuzzle.ViewModels;

public class LobbyViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    
    public LobbyViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }
    
    public void LoadGame()
    {
        _mainViewModel.LoadGame();
    }
}