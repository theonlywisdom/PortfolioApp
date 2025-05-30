using CommunityToolkit.Mvvm.ComponentModel;

namespace PortfolioApp.UI.ViewModel;

public partial class ViewModelBase : ObservableValidator, IViewModelBase
{
    [ObservableProperty]
    private bool _isLoading;

    public IAsyncRelayCommand InitialiseAsyncCommand { get; }

    public ViewModelBase()
    {
        InitialiseAsyncCommand = new AsyncRelayCommand(
            async () =>
            {
                IsLoading = true;
                await Loading(LoadAsync);
                IsLoading = false;
            }, () => !IsLoading);
    }

    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }

    protected async Task Loading(Func<Task> unitOfWork)
    {
        await unitOfWork();
    }
}

public interface IViewModelBase
{
    IAsyncRelayCommand InitialiseAsyncCommand { get; }
}
