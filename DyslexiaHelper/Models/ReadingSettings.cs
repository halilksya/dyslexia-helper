using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DyslexiaHelper.Models;

public partial class ReadingSettings : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LineHeight))]
    private double _fontSize = 22;

    [ObservableProperty]
    private double _letterSpacing = 1.5;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LineHeight))]
    private double _lineSpacingFactor = 1.6;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BackgroundBrush))]
    [NotifyPropertyChangedFor(nameof(ForegroundBrush))]
    private ReadingTheme _selectedTheme = ReadingTheme.DefaultThemes[0];

    public double LineHeight => FontSize * LineSpacingFactor;
    public IBrush BackgroundBrush => SelectedTheme.Background;
    public IBrush ForegroundBrush => SelectedTheme.Foreground;
}