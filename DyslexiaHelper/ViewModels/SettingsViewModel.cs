using CommunityToolkit.Mvvm.Input;

namespace DyslexiaHelper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    public SettingsViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private void GoBack()
    {
        _mainViewModel.GoBack();
    }   
}