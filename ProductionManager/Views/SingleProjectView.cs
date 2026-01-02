using Eto.Forms;

namespace ProductionManager.Views;

public class SingleProjectView : StackLayout
{
    private readonly DropDown _grade;
    private Project _project;
    private Student _student;

    //
    private readonly Label StudentName;
    private readonly Label _weekLength;
    private readonly Label _rubric;

    public SingleProjectView(Student primary, Project project)
    {
        _project = project;
        this.Orientation = Orientation.Horizontal;

        StudentName = new Label();
        Items.Add(StudentName);
        //
        _weekLength = new Label();
        Items.Add(_weekLength);
        //
        _rubric = new Label();
        Items.Add(_rubric);
        //
        _grade = new DropDown();
        _grade.Items.Add("Not Started");
        _grade.Items.Add("Started");
        _grade.Items.Add("Satisfactory");
        _grade.Items.Add("Not Satisfactory");
        _grade.SelectedValueChanged += GradeOnSelectedValueChanged;
        Items.Add(_grade);
        SetProject(primary, project);
    }

    
    private void GradeOnSelectedValueChanged(object? sender, EventArgs e)
    {
        var i = _grade.SelectedIndex;
        Grade g = Grade.Unknown;
        switch (i)
        {
            case 0:
                g = Grade.NotStarted;
                break;
            case 1:
                g = Grade.Started;
                break;
            case 2:
                g = Grade.Satisfactory;
                break;
            case 3:
                g = Grade.Unsatisfactory;
                break;
            case -1:
            default:
                g = Grade.Unknown;
                break;
        }

        if (g != Grade.NotStarted && g != Grade.Unknown)
        {
            _project.Grade = g;
            //mark dirty.
        }
    }

    public void SetProject(Student student, Project project)
    {
        _project = project;
        _student = student;
        if (student == null || project == null)
        {
            return;
        }
        
        StudentName.Text = student.ToString();
        if (project.Students.Length > 1)
        {
            StudentName.Text += " +(";
            var others = project.Students.Where(x => x.StudentID != student.StudentID).ToArray();
            for (var i = 0; i < others.Length; i++)
            {
                var other = others[i];
                StudentName.Text += other.FirstName.ToString();
                if (i != others.Length - 1)
                {
                    StudentName.Text += ",";
                }
            }
            StudentName.Text += ")";
        }
        
        //
        _weekLength.Text = $"Length: {project.Length}";
        _rubric.Text = $"Rubric: {project.Rubric}";
        //set grade
        switch (project.Grade)
        {
            case Grade.NotStarted:
                _grade.SelectedIndex = 0;
                break;
            case Grade.Started:
                _grade.SelectedIndex = 1;
                break;
            case Grade.Satisfactory:
                _grade.SelectedIndex = 2;
                break;
            default:
                _grade.SelectedIndex = -1;
                break;
        }
    }
}