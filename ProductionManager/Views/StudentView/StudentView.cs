using Eto.Forms;

namespace ProductionManager.Views;

public class StudentView : GroupBox
{
    public MainWindow _mainWindow;

    public StudentView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        ListControl listControl = new ListBox();
        listControl.DataStore = _mainWindow.DataStore.Students;
        Content = listControl;
    }
}