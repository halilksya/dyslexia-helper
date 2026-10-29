using System.Collections.Generic;
using DyslexiaHelper.Models;
using CommunityToolkit.Mvvm.Input;

namespace DyslexiaHelper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    public ReadingSettings Settings => _mainViewModel.ReadingSettings;
    public IReadOnlyList<ReadingTheme> Themes => ReadingTheme.DefaultThemes;

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