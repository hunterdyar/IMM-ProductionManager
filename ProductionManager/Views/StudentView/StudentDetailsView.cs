using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class StudentDetailsView : GroupBox
{
    private MainWindow _mainWindow;
    private ToggleButton _weekToggle;
    private StudentWeek _studentWeek;
    private StudentWeekView _studentWeekView;

    public StudentDetailsView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        _weekToggle = new ToggleButton();
        _weekToggle.Text = "Show Grades";
        _weekToggle.Checked = false;
        _weekToggle.CheckedChanged += WeekToggleOnCheckedChanged;
    }

    private void WeekToggleOnCheckedChanged(object? sender, EventArgs e)
    {
        _studentWeekView.Visible = _weekToggle.Checked;
    }

    public Student? CurrentStudent { get; private set; }

    public void SetStudent(Student? student)
    {
        CurrentStudent = student;
        _studentWeek = new StudentWeek(student, _mainWindow.DataStore);
        
        _studentWeekView = new StudentWeekView(_studentWeek);
       // _studentWeekView.MinimumSize = new Size(this.Width-100, 120);
        _studentWeekView.Width = this.Width;
        _studentWeekView.Height = 50;
        _studentWeekView.Visible = _weekToggle.Checked;
        
        var layout = new DynamicLayout();
        layout.Spacing = new Size(4,4);
        layout.BeginVertical();
        //headers. matches below.
        layout.BeginCentered();

        layout.BeginGroup("information");
        
        layout.BeginHorizontal();
        layout.Add(GetText("First"));
        layout.Add(GetText("Last"));
        layout.Add(GetText("Section"));
        layout.Add(GetText("Level"));
        layout.Add(null);
        layout.Add(_weekToggle);
        layout.EndHorizontal();
        layout.BeginHorizontal();
        layout.Add(GetText(CurrentStudent.FirstName));
        layout.Add(GetText(CurrentStudent.LastName));
        layout.Add(GetText(CurrentStudent.Section.ToString()));
        layout.Add(GetText(CurrentStudent.ClassLevel.ToString()));

        layout.EndHorizontal();
        
        layout.EndGroup();
        layout.EndCentered();
        //
        layout.Add(null);
        layout.BeginGroup("Grades");
        layout.AddSeparateRow(_studentWeekView);
        layout.EndGroup();
        layout.EndVertical();
        
       
        Content = layout;
    }

    private Control GetText(string t)
    {
        return new Label()
        {
            Text = t,
        };
    }
}