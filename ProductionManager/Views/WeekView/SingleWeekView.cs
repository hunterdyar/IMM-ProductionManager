using Eto.Forms;

namespace ProductionManager.Views;

public class SingleWeekView : StackLayout
{
    private int SelectedWeek => Settings.Instance.SelectedWeek;
    
    private DropDown _dropDown;
    private StackLayout _projects;

    private MainWindow _mainWindow;
    //a drop-down box at the top to pick a week (saved/remembered)
    //a list of what each student is working on this single week (project details)
    public SingleWeekView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        _dropDown = new DropDown();
        for (int i = 0; i < Settings.TotalWeeks; i++)
        {
            _dropDown.Items.Add((i+1).ToString());
        }
        
        _dropDown.SelectedIndex = Settings.Instance.SelectedWeek;
        _dropDown.SelectedValueChanged += DropDownOnSelectedValueChanged;
        Items.Add(_dropDown);
        RemakeList();
        Items.Add(_projects);
    }

    void RemakeList()
    {
        if (_projects == null)
        {
            _projects = new StackLayout();
        }
        _projects.Items.Clear();
        foreach (var student in _mainWindow.DataStore.Students)
        {
            if(_mainWindow.DataStore.TryGetProject(student, SelectedWeek, out Project project))
            {
                var p = new SingleProjectView(student, project);
                _projects.Items.Add(p);
            }
            else
            {
                //No project set. Add or Join?
            }
        }
    }
    private void DropDownOnSelectedValueChanged(object? sender, EventArgs e)
    {
        var selected = _dropDown.SelectedIndex;
        Settings.Instance.SelectedWeek = selected;
       

        RemakeList();
    }
}