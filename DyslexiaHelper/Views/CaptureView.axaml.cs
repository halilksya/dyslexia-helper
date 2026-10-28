using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DyslexiaHelper.ViewModels;
using DyslexiaHelper.Core.Imaging;

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

        var normalizedBytes = await Task.Run(() => ImageNormalizer.Normalize(memoryStream.ToArray()));
        viewModel.LoadImage(normalizedBytes);
    }
}
