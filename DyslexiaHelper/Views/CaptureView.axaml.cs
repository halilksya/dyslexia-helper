using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DyslexiaHelper.ViewModels;

namespace DyslexiaHelper.Views;

public partial class CaptureView : UserControl
{
    public CaptureView()
    {
        InitializeComponent();
    }

    private async void OnPickImageClick(object? sender, RoutedEventArgs eventArgs)
    {
        if(DataContext is not CaptureViewModel viewModel)
            return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null)
            return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Fotoğraf Seç",
            AllowMultiple = false,
            FileTypeFilter = new[] {FilePickerFileTypes.ImageAll}
        });

        if(files.Count == 0)
            return;

        await using var fileStream = await files[0].OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);

        viewModel.LoadImage(memoryStream.ToArray());
    }


}