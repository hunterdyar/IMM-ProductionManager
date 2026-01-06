using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using Eto.Forms;
using Command = Eto.Forms.Command;
using ContextMenu = Eto.Forms.ContextMenu;
using GroupBox = Eto.Forms.GroupBox;
using Orientation = Eto.Forms.Orientation;
using Size = Eto.Drawing.Size;

namespace ProductionManager.Views;

public class SingleProjectView : StackLayout
{
    public const int ProjHeight = 100;
    private MainWindow _mainWindow;
    private readonly GradeDropdown _grade;
    private TextBox _rubricTextBox;
    private TextArea _noteTextArea;
    private DropDown _rubricQuickPickDropdown;
    private GroupBox _weekLengthGroupBox;
    private TextBox _weeksTextBox;
    private Project _project;
    private Student _student;
    //
    private readonly Label StudentName;
    public Action NeedsUpdate;

    public SingleProjectView(MainWindow mainWindow, Student primary, Project project)
    {
        _mainWindow = mainWindow;
        _project = project;
        this.Orientation = Orientation.Horizontal;
        this.Height = ProjHeight;
        //
       
        //
        var nameBox = new GroupBox();
        nameBox.MinimumSize = new Size(200, ProjHeight);
        nameBox.Text = "Name";
        nameBox.Height = ProjHeight;
        StudentName = new Label();
        
        nameBox.Content = StudentName;
        StudentName.Width = nameBox.MinimumSize.Width;
        Items.Add(nameBox);
        nameBox.Padding = 2;
        //
        //menu
        StudentName.ContextMenu = new ContextMenu();
        StudentName.ContextMenu.Items.Add(new Command(((sender, args) =>
        {
            var window = new AddStudentToProjectPopup(mainWindow,_project);
            window.ShowModal();
            NeedsUpdate?.Invoke();
        }))
        {
            MenuText = "Edit Students",
        });
        StudentName.ContextMenu.Items.Add(new Command(((sender, args) =>
        {
            mainWindow.DataStore.RemoveProject(_project);
            NeedsUpdate?.Invoke();
        }))
        {
            MenuText = "Delete Project",
        });

        //in the current layout, there's nowhere to right click in this menu, so using student names.
        this.ContextMenu = StudentName.ContextMenu;
        
        //
        _weekLengthGroupBox = new GroupBox();
        _weekLengthGroupBox.Text = $"Week Length";
        _weekLengthGroupBox.Height = ProjHeight;
        _weekLengthGroupBox.Padding = 2;
        _weeksTextBox = new TextBox();
        _weeksTextBox.TextChanging += (sender, args) =>
        {
            if (!int.TryParse(args.NewText, out int i))
            {
                args.Cancel = true;
            }
            else
            {
                if (i < 0)
                {
                    args.Cancel = true;
                }
            }
        };
        _weeksTextBox.TextChanged += (sender, args) =>
        {
            if (int.TryParse(_weeksTextBox.Text, out int i))
            {
                _project.Length = i;
            }
        };
        _weekLengthGroupBox.Content = _weeksTextBox;
        Items.Add(_weekLengthGroupBox);
        //
        _rubricQuickPickDropdown =  new DropDown();
        _rubricQuickPickDropdown.Height = ProjHeight / 4;
        _rubricQuickPickDropdown.SelectedValueChanged += (sender, args) =>
        {
            var i = _rubricQuickPickDropdown.SelectedIndex;
            if (i >= 0)
            {
                _rubricTextBox.Text = _rubricQuickPickDropdown.Items[i].Text;
            }
        };
        GroupBox rubricGroupBox = new GroupBox();
        rubricGroupBox.Padding = 2;
        rubricGroupBox.Text = "Rubric";
        var rgblayout = new DynamicLayout();
        rgblayout.BeginVertical();
        rgblayout.Add(_rubricQuickPickDropdown);
        
        _rubricTextBox = new TextBox();
        _rubricTextBox.ToolTip = "Rubric";
        _rubricTextBox.TextChanged += (sender, args) =>
        {
            _project.Rubric = _rubricTextBox.Text;
        };
        
        rgblayout.Add(_rubricTextBox);
        rgblayout.EndVertical();
        rubricGroupBox.Content = rgblayout;
        Items.Add(rubricGroupBox);
        //
        
        //
       
        //
        var noteGroupbox = new GroupBox();
        noteGroupbox.Padding = 2;
        noteGroupbox.Text = "Note";
        _noteTextArea = new TextArea();
        _noteTextArea.TextChanged += (sender, args) =>
        {
            _project.Note = _noteTextArea.Text;
        };
        noteGroupbox.Content = _noteTextArea;
        noteGroupbox.Height = ProjHeight;
        Items.Add(noteGroupbox);
        //
        var gradeBox = new GroupBox();
        gradeBox.Padding = 2;
        gradeBox.Height = ProjHeight;
        gradeBox.Text = "Grade";
        _grade = new GradeDropdown();
        _grade.OnSelectedGradeChanged += GradeOnSelectedValueChanged;
        gradeBox.Content = _grade;
        Items.Add(null);//space to push grade to the right.
        Items.Add(gradeBox);
        SetProject(primary, project);
    }

    
    private void GradeOnSelectedValueChanged(Grade g)
    {
        if (g != Grade.NotStarted && g != Grade.Unknown)
        {
            _project.Grade = g;
            //mark dirty.
        }
    }

    public void SetProject(Student student, Project project)
    {
        if (_project != null)
        {
            _project.OnChange -= OnChange;
        }
        _project = project;
        _student = student;
        if (student == null || project == null)
        {
            return;
        }
        _project.OnChange += OnChange;

        _weekLengthGroupBox.Text = $"Week {project.Week}, Length";
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
        _weeksTextBox.Text = $"{project.Length}";
        //set grade
       _grade.SetGrade(project.Grade);

        _rubricQuickPickDropdown.Items.Clear();
        foreach (var rubric in _mainWindow.DataStore._allRubrics)
        {
            _rubricQuickPickDropdown.Items.Add(rubric);
        }
        var index = Array.IndexOf(_mainWindow.DataStore._allRubrics,project.Rubric);
        _rubricQuickPickDropdown.SelectedIndex = index;//-1 is valid right?
        
        _noteTextArea.Text = project.Note;
        
    }

    private void OnChange()
    {
       SetProject(_student, _project);
    }

    protected override void Dispose(bool disposing)
    {
        _project.OnChange -= OnChange;
        base.Dispose(disposing);
    }
}