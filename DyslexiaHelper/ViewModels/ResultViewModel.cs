using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using DyslexiaHelper.Models;

namespace DyslexiaHelper.ViewModels;

public partial class ResultViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    public Bitmap SourceImage { get;}

    public ReadingSettings Settings => _mainViewModel.ReadingSettings;

    [ObservableProperty]
    private string _recognizedText = string.Empty;

    [ObservableProperty]
    private bool _isRecognizing;

    public ResultViewModel(MainViewModel mainViewModel, byte[] imageBytes, Bitmap sourceImage)
    {
        _mainViewModel = mainViewModel;
        SourceImage = sourceImage;
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
            RecognizedText = $"Metin tanıma başarısız: {exception.Message}";
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