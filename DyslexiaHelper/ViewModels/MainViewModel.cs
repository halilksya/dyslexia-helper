using CommunityToolkit.Mvvm.ComponentModel;

namespace DyslexiaHelper.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage = new CaptureViewModel();
}