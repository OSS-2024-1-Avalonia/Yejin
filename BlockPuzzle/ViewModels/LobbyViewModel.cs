using System;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using ReactiveUI;

namespace BlockPuzzle.ViewModels;

public class LobbyViewModel : ViewModelBase
{
    private static readonly string[] Scopes = new[] { "https://www.googleapis.com/auth/userinfo.email" };
    private const string SecretPath = "../../../client_secret.json";

    private readonly MainViewModel _mainViewModel;
    public ReactiveCommand<Unit, UserCredential> LoginGoogleCommand { get; } 

    public LobbyViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        LoginGoogleCommand = ReactiveCommand.CreateFromTask(AuthenticateGoogle);
    }
    private static async Task<UserCredential> AuthenticateGoogle()
    {
        UserCredential credential;

        using (var stream = new FileStream(SecretPath, FileMode.Open, FileAccess.Read))
        {
            const string credPath = "token.json";
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)
            );
        }

        var oauthToken = await credential.GetAccessTokenForRequestAsync();
        Console.WriteLine("OAuth token: " + oauthToken);

        return credential;
    }

    public void LoadGame()
    {
        _mainViewModel.LoadGame();
    }
}