using Eto.Forms;

namespace ProductionManager.Views;

public class StudentListView : GroupBox
{
    public MainWindow _mainWindow;
    public Action<Student?> OnSelectedChanged { get; set; }
    public StudentListView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        Scrollable scrollable = new Scrollable();
        ListControl listControl = new ListBox();
        listControl.DataStore = _mainWindow.DataStore.Students;
        scrollable.Content = listControl;

        listControl.SelectedValueChanged += (sender, args) =>
        {
            OnSelectedChanged?.Invoke(listControl.SelectedValue as Student);
        };
        
        Content = scrollable;
    }
}