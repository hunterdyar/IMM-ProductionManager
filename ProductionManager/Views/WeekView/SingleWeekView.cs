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
        
        _dropDown.SelectedIndex = Settings.Instance.SelectedWeek-1;
        _dropDown.SelectedValueChanged += DropDownOnSelectedValueChanged;
        Items.Add(_dropDown);
        RemakeList();
        var scrollable = new Scrollable();
        scrollable.Content = _projects;
        Items.Add(scrollable);
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
                var p = new SingleProjectView(_mainWindow, student, project);
                _projects.Items.Add(p);
            }
            else
            {
                var createProject = new GroupBox();
                createProject.Text = student.ToString();
                var button = new Button();
                button.Text = "Create";
                button.Click += (sender, args) =>
                {
                    _mainWindow.DataStore.AddProject(Project.Create(student, SelectedWeek));
                    Console.WriteLine("Created Project");
                    RemakeList();
                };
                createProject.Content = button;
                _projects.Items.Add(createProject);
            }
        }
    }
    private void DropDownOnSelectedValueChanged(object? sender, EventArgs e)
    {
        var selected = _dropDown.SelectedIndex;
        Settings.Instance.SelectedWeek = selected+1;
        RemakeList();
    }
}