using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace DyslexiaHelper.ViewModels;

public partial class ResultViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    public Bitmap SourceImage { get;}

    [ObservableProperty]
    private string _recognizedText = string.Empty;

    [ObservableProperty]
    private bool _isRecognizing;

    public ResultViewModel(MainViewModel mainViewModel, byte[] imageBytes)
    {
        _mainViewModel = mainViewModel;
        using var memoryStream = new MemoryStream(imageBytes);
        SourceImage = new Bitmap(memoryStream);
        _ = RecognizeAsync(imageBytes);
    }

    private async Task RecognizeAsync(byte[] imageBytes)
    {
        IsRecognizing = true;
        try
        {
            RecognizedText = await _mainViewModel.TextRecognizer.RecognizeAsync(imageBytes);
        }
        catch (Exception exception)
        {
            RecognizedText = $"Recognition failed: {exception.Message}";
        }
        finally
        {
            IsRecognizing = false;
        }
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