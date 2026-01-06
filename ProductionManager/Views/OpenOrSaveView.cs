using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Eto.Forms;

namespace ProductionManager.Views;

public class OpenOrSaveView : GroupBox
{
    private MainWindow _mainWindow;
    private Button _openButton;
    private Button _newButton;
    public OpenOrSaveView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        DynamicLayout layout = new DynamicLayout();
        layout.Add(new Label() { Text = "Production Manager" });
        _openButton = new Button()
        {
            Text = "Open",
        };
        _openButton.Click += OpenButtonOnClick;
        layout.Add(_openButton);
        
        _newButton = new Button()
        {
            Text = "Create New",
        };
        _newButton.Click += NewButtonOnClick;
        
        layout.Add(_newButton);
        layout.AddSpace();
        Content = layout;
    }

    private void OpenButtonOnClick(object? sender, EventArgs e)
    {
        var o = new OpenFileDialog();
        o.CheckFileExists = true;
        o.MultiSelect =false;
        o.Title = "Open";
        o.ShowDialog(this);
        var f = o.FileName;
        if (f != null)
        {
            _mainWindow.SetDatastore(new DataStore(f));
            _mainWindow.SwitchToMode(MainWindowState.OverView);
        }
        //tell main window that state has changed. or DataStore can.
    }

    private void NewButtonOnClick(object? sender, EventArgs e)
    {
       var o = new SaveFileDialog();
       o.Filters.Add(new FileFilter("Excel Workbook", ".xlsx"));
       o.CheckFileExists = true;
       o.Title = "Save";
       o.CheckFileExists = false;
       o.ShowDialog(this);
       var f = o.FileName;
       if (f != null)
       {
           if (Path.GetExtension(f) != ".xlsx")
           {
               f = Path.Combine(f, ".xlsx");
           }
           _mainWindow.SetDatastore(new DataStore(f, true));
           _mainWindow.SwitchToMode(MainWindowState.OverView);
       }
    }
}