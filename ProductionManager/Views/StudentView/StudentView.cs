using Eto.Drawing;
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
        
        var layout = new DynamicLayout();
        layout.BeginGroup("Settings");
        layout.BeginHorizontal();
        layout.Add(new Button((e, a) =>
        {
            var p = new MakeStudentPopup(mainWindow);
            p.ShowModal();
            _listView.Refresh();
        })
        {
            Text = "New Student",
        });
        layout.Add(new Button((e, a) =>
        {
            mainWindow.DataStore.LoadFromBacking();
        })
        {
            Text = "Reload From Sheet",
        });
        layout.Add(new Button((e, a) =>
        {
            mainWindow.DataStore.SaveToBacking();
        })
        {
            Text = "Save",
        });
        layout.EndHorizontal();
        layout.EndGroup();
        Splitter splitter = new Splitter();
        splitter.Orientation = Orientation.Vertical;
        splitter.Panel1 = _listView;
        splitter.RelativePosition = 500;
        splitter.Panel2 = _details;
        layout.AddRow(splitter);
        Content = layout;
    }
}