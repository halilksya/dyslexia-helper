using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using DyslexiaHelper.Core.Imaging;

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

    public async Task LoadImageAsync(byte[] imageBytes)
    {
        var normalizedBytes = await Task.Run(() => ImageNormalizer.Normalize(imageBytes));

        _imageBytes = normalizedBytes;
        using var memoryStream = new MemoryStream(normalizedBytes);
        PreviewImage = new Bitmap(memoryStream);
    }

    public bool CanCapturePhoto => App.CameraService is not null;

    [RelayCommand]
    private async Task CapturePhotoAsync()
    {
        if (App.CameraService is null)
            return;

        var photoBytes = await App.CameraService.CapturePhotoAsync();
        if (photoBytes is null)
            return;

        await LoadImageAsync(photoBytes);
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