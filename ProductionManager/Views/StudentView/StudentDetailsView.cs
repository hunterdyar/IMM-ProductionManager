using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class StudentDetailsView : GroupBox
{
    private MainWindow _mainWindow;
    private ToggleButton _weekToggle;
    private StudentWeek _studentWeek;
    private StudentSemesterView _studentSemesterView;
    private HoverManager _hoverManager = new HoverManager(null);
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
        _studentSemesterView.Visible = _weekToggle.Checked;
        _weekToggle.Text = _weekToggle.Checked ? "Hide Grades" : "Show Grades";
    }

    public Student? CurrentStudent { get; private set; }

    public void SetStudent(Student? student)
    {
        CurrentStudent = student;
        _studentWeek = new StudentWeek(student, _mainWindow.DataStore);
        _studentSemesterView = new StudentSemesterView(_studentWeek, _hoverManager,true);
       // _studentSemesterView.MinimumSize = new Size(this.Width-100, 120);
        _studentSemesterView.Width = this.Width;
        _studentSemesterView.Height = 50;
        _studentSemesterView.Visible = _weekToggle.Checked;
        
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
        layout.AddSeparateRow(_studentSemesterView);
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