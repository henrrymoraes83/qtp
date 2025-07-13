using System.Linq;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;

namespace AvaloniaApplication1.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private IFactory? _factory;

    private IRootDock? _layout;

    public IRootDock? Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }

    private DocumentDock _documentDock;
    public MainWindowViewModel() //Window _hostWindow
    {
        _factory = new DockFactory();
        Layout = _factory.CreateLayout();

        _factory.InitLayout(Layout);

        _documentDock = Layout.VisibleDockables
            .OfType<DocumentDock>()
            .FirstOrDefault();

    }
    
    public async Task Click_New()
    {
        var untitledFileViewModel = new DockViewmodel()
        {
            Title = "Test"
        };

        var files = _factory?.GetDockable<IDocumentDock>("Chart");

        if (Layout is { } && files is { })
        {
            _factory?.AddDockable(files, untitledFileViewModel);
            _factory?.SetActiveDockable(untitledFileViewModel);
            _factory?.SetFocusedDockable(Layout, untitledFileViewModel);
        }
    }
}