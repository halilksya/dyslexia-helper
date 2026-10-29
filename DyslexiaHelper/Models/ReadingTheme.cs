using System.Collections.Generic;
using Avalonia.Media;

namespace DyslexiaHelper.Models;

public sealed record ReadingTheme(string Name, IBrush Background, IBrush Foreground)
{
    private static ReadingTheme Create(string name, string background, string foreground) =>
        new(name, new SolidColorBrush(Color.Parse(background)), new SolidColorBrush(Color.Parse(foreground)));

    public static IReadOnlyList<ReadingTheme> DefaultThemes { get; } = new[]
    {
        Create("Krem", "#FBF5E6", "#222222"),
        Create("Açık Mavi", "#E3EEF9", "#1C2733"),
        Create("Açık Sarı", "#FFF7CC", "#2A2A2A"),
        Create("Koyu", "#1F1F1F", "#E8E1D3")
    };
}