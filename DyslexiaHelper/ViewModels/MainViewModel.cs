using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DyslexiaHelper.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly Stack<ViewModelBase> _history = new();

    [ObservableProperty]
    private ViewModelBase? _currentPage;

    public MainViewModel()
    {
        CurrentPage = new CaptureViewModel(this);
    }

    public void NavigateTo(ViewModelBase page)
    {
        if (CurrentPage is not null)
            _history.Push(CurrentPage);

        CurrentPage = page;
    }

    public void GoBack()
    {
        if (_history.Count > 0)
            CurrentPage = _history.Pop();
    }
}