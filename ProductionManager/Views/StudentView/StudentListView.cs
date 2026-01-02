using Eto.Forms;

namespace ProductionManager.Views;

public class StudentListView : GroupBox
{
    public MainWindow _mainWindow;
    private ListControl _listControl;
    public Action<Student?> OnSelectedChanged { get; set; }
    public StudentListView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        Scrollable scrollable = new Scrollable();
        _listControl = new ListBox();
        _listControl.DataStore = _mainWindow.DataStore.Students;
        scrollable.Content = _listControl;

        _listControl.SelectedValueChanged += (sender, args) =>
        {
            OnSelectedChanged?.Invoke(_listControl.SelectedValue as Student);
        };
        
        Content = scrollable;
    }

    public void Refresh()
    {
        _listControl.DataStore = _mainWindow.DataStore.Students;
        _listControl.Invalidate();
    }
}