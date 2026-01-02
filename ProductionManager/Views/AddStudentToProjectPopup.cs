using Eto.Forms;

namespace ProductionManager.Views;

public class AddStudentToProjectPopup : Dialog
{
    CheckBox onlyThisSection = new CheckBox();
    public AddStudentToProjectPopup(MainWindow mainWindow, Project project)
    {
        this.Size = new Eto.Drawing.Size(800, 800);
        Title = "Add/Remove Student";
        Owner = mainWindow;
        onlyThisSection.Text = $"Only Section {project.Students[0].Section}";
        onlyThisSection.Checked = true;
        var layout = new DynamicLayout();
        layout.AddRow(onlyThisSection);
        var thisSection = project.Students[0].Section;
        layout.BeginGroup("Students");
        foreach (Student student in mainWindow.DataStore.Students)
        {
            if (onlyThisSection.Checked.Value)
            {
                if (student.Section != thisSection)
                {
                    continue;
                }
            }
            
            bool hasProjectThisWeek = false;
            for (int i = project.Week; i < project.Week+project.Length; i++)
            {
                if (mainWindow.DataStore.TryGetProject(student, i, out var otherProject))
                {
                    if (otherProject != project)
                    {
                        hasProjectThisWeek = true;
                        break;
                    }
                }
            }
            
            var sc = new CheckBox();
            sc.Enabled = !hasProjectThisWeek;
            sc.Text = student.ToString();
            var inProj = project.Students.Contains(student);
            sc.Checked = inProj;
            sc.CheckedChanged += (sender, e) =>
            {
                var include = sc.Checked.Value;
                if (include)
                {
                    if (!project.TryAddStudent(student))
                    {
                        //cancel the add.
                        sc.Checked = false;
                    }
                }
                else
                {
                    if (!project.TryRemoveStudent(student))
                    {
                        //cancel remove
                        sc.Checked = true;
                    }
                }
            };
            layout.Add(sc);
        }
        layout.EndGroup();

        layout.Add(null);
        var close = new Button()
        {
            Text = "Done",
            Command = new Command((o,a) =>
            {
                this.Close();
            })
        };
        layout.AddCentered(close);
        Content = layout;
    }
    
}