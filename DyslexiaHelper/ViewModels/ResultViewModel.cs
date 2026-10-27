using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace DyslexiaHelper.ViewModels;

public partial class ResultViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    public Bitmap SourceImage { get;}

    public ResultViewModel(MainViewModel mainViewModel, byte[] imageBytes)
    {
        using var memoryStream = new MemoryStream(imageBytes);
        SourceImage = new Bitmap(memoryStream);
        
        _mainViewModel = mainViewModel;
    }

    [RelayCommand]
    private void OpenSettings()
    {
        _mainViewModel.NavigateTo(new SettingsViewModel(_mainViewModel));
    }

    [RelayCommand]
    private void GoBack()
    {
        _mainViewModel.GoBack();
    }
}