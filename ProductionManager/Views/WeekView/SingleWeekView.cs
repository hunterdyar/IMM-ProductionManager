using Eto.Forms;

namespace ProductionManager.Views;

public class SingleWeekView : StackLayout
{
    private int SelectedWeek => Settings.Instance.SelectedWeek;
    private DropDown _sectionDropDown;
    private DropDown _weekDropDown;
    private StackLayout _projects;
    private MainWindow _mainWindow;

    private int _selectedSection = -1;
    //a drop-down box at the top to pick a week (saved/remembered)
    //a list of what each student is working on this single week (project details)
    public SingleWeekView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        _weekDropDown = new DropDown();
        _sectionDropDown = new DropDown();
        _projects = new StackLayout();
        for (int i = 0; i < Settings.TotalWeeks; i++)
        {
            _weekDropDown.Items.Add((i+1).ToString());
        }

        var sectionGroups = _mainWindow.DataStore.Students.GroupBy(x => x.Section);
        _sectionDropDown.Items.Add("All","-1");
        foreach (var grouping in sectionGroups)
        {
            _sectionDropDown.Items.Add(grouping.First().Section.ToString());
        }
        _weekDropDown.SelectedIndex = Settings.Instance.SelectedWeek-1;
        _weekDropDown.SelectedValueChanged += WeekDropDownOnSelectedValueChanged;

        var topBar = new StackLayout()
        {
            Orientation = Orientation.Horizontal
        };
        
        topBar.Items.Add(new Label(){Text = "Week:"});
        topBar.Items.Add(_weekDropDown);
        topBar.Items.Add(new Label() { Text = "Section:" });
        topBar.Items.Add(_sectionDropDown);
        _sectionDropDown.SelectedIndexChanged += SectionDropDownOnSelectedIndexChanged;
        Items.Add(topBar);
        RemakeList();
        var scrollable = new Scrollable();
        scrollable.Content = _projects;
        Items.Add(scrollable);
    }

    private void SectionDropDownOnSelectedIndexChanged(object? sender, EventArgs e)
    {
        var x = _sectionDropDown.SelectedKey.ToString();
        if (int.TryParse(x, out var val))
        {
            _selectedSection = val;
            RemakeList();
        }
        else
        {
            throw new Exception("unable to select section value");
        }
    }


    void RemakeList()
    {
        _projects.Items.Clear();
        if (_mainWindow.DataStore.Students.Count == 0)
        {
            return;
        }
        foreach (var student in _mainWindow.DataStore.Students)
        {
            if (_selectedSection >= 0)
            {
                if (student.Section != _selectedSection)
                {
                    continue;
                }
            }
            if(_mainWindow.DataStore.TryGetProject(student, SelectedWeek, out Project project))
            {
                var p = new SingleProjectView(_mainWindow, student, project);
                p.NeedsUpdate += RemakeList;
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
    private void WeekDropDownOnSelectedValueChanged(object? sender, EventArgs e)
    {
        var selected = _weekDropDown.SelectedIndex;
        Settings.Instance.SelectedWeek = selected+1;
        RemakeList();
    }
}