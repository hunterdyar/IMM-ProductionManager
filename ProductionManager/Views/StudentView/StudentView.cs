using Eto.Forms;

namespace ProductionManager.Views;

public class StudentView : GroupBox
{
    private readonly StudentDetailsView _details;
    private readonly StudentListView _listView;
    
    public StudentView(MainWindow mainWindow)
    {
        _listView = new StudentListView(mainWindow);
        _details = new StudentDetailsView(mainWindow);

        _listView.OnSelectedChanged += student =>
        {
            _details.SetStudent(student);
        };
        
        Splitter splitter = new Splitter();
        splitter.Orientation = Orientation.Vertical;
        splitter.Panel1 = _listView;
        splitter.Panel2 = _details;
        
        
        
        Content = splitter;
    }
}