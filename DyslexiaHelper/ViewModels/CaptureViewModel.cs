using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DyslexiaHelper.ViewModels;

public partial class CaptureViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    private byte[]? _imageBytes;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenResultCommand))]
    private Bitmap? _previewImage;

    public CaptureViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }

    public void LoadImage(byte[] imageBytes)
    {
        _imageBytes = imageBytes;
        using var memoryStream = new MemoryStream(imageBytes);
        PreviewImage = new Bitmap(memoryStream);
    }

    private bool CanOpenResult() => _imageBytes is not null;

    [RelayCommand(CanExecute = nameof(CanOpenResult))]
    private void OpenResult()
    {
        _mainViewModel.NavigateTo(new ResultViewModel(_mainViewModel, _imageBytes!));
    }

    [RelayCommand]
    private void OpenSettings()
    {
        _mainViewModel.NavigateTo(new SettingsViewModel(_mainViewModel));
    }
}