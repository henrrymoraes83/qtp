using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;



namespace AvaloniaApplication1.ViewModels;


public class DockFactory : Factory
{
    public  IRootDock? RootDock {get;set;}
    // public IRootDock? RootDock
    // {
    //     get => _ateOffset;
    //     set { SetProperty(ref _ateOffset, value); }
    // }

    private IDocumentDock? _chartDock;
    private IDocumentDock? _bottomDock;

    public override IRootDock CreateLayout()
    {
        var documentDock = new DocumentDock()
        {
            Id = "Chart",
            Title = "Chart",
            IsCollapsable = false,
            Proportion = double.NaN,
            VisibleDockables = CreateList<IDockable>
            (
            ),
            CanCreateDocument = false
        };


        var windowLayout = CreateRootDock();
        windowLayout.Title = "Default";
        var windowLayoutContent = new ProportionalDock
        {
            Orientation = Orientation.Vertical,
            IsCollapsable = false,
            VisibleDockables = CreateList<IDockable>
            (
                documentDock,
                new ProportionalDockSplitter()
               // bottomDock
            )
        };
        windowLayout.IsCollapsable = false;
        windowLayout.VisibleDockables = CreateList<IDockable>(windowLayoutContent);
        windowLayout.ActiveDockable = windowLayoutContent;

        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false;
        rootDock.VisibleDockables = CreateList<IDockable>(windowLayout);
        rootDock.ActiveDockable = windowLayout;
        rootDock.DefaultDockable = windowLayout;


        RootDock = rootDock;
        _chartDock = documentDock;
       // _bottomDock = bottomDock;

        return rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {
            ["Chart"] = () => _chartDock
        };
        DockableLocator = new Dictionary<string, Func<IDockable?>>
        {
            ["Root"] = () => RootDock,
            ["Chart"] = () => _chartDock,
           // ["Bottom"] = () => _bottomDock
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow?>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };


        base.InitLayout(layout);
    }
}