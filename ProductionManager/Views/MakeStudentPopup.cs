using Eto.Forms;

namespace ProductionManager.Views;

public class MakeStudentPopup : Dialog
{
    public MakeStudentPopup(MainWindow mainWindow)
    {
        Title = "New Student";
        DynamicLayout layout = new DynamicLayout();
        layout.BeginVertical();
        layout.BeginCentered();
        layout.Add(new Label()
        {
            Text = "Add New Student"
        });
        layout.EndCentered();
        layout.BeginHorizontal();
        layout.Add(new Label()
        {
            Text = "First Name"
        });
        var firstNameBox = new TextBox();
        layout.Add(firstNameBox);
        layout.EndHorizontal();
        layout.BeginHorizontal();
        layout.Add(new Label()
        {
            Text = "Last Name"
        });
        var lastNameBox = new TextBox();
        layout.Add(lastNameBox);
        layout.EndHorizontal();
        layout.BeginHorizontal();
        layout.Add(new Label()
        {
            Text = "ID"
        });
        var idBox = new TextBox();
        layout.Add(idBox);
        layout.EndHorizontal();
        layout.BeginHorizontal();
        layout.Add(new Label()
        {
            Text = "Section"
        });
        var section = new TextBox()
        {
            Text = "1",
        };
        layout.Add(section);
        layout.EndHorizontal();
        layout.EndVertical();
        
        layout.BeginHorizontal();
        layout.Add(new Label()
        {
            Text = "Level"
        });
        var level = new ClassLevelDropdown();
        layout.Add(level);
        layout.EndHorizontal();
        layout.EndVertical();
        

        layout.BeginHorizontal();
        var errorLabel = new Label(); 
        layout.Add(new Button((e, a) =>
        {
            if (!int.TryParse(section.Text, out int sectionId))
            {
               errorLabel.Text = "Invalid Section ID";
               return;
            }

            if (level.SelectedIndex == -1)
            {
                errorLabel.Text = "No Class Level Selected";
                return;
            }

            if (firstNameBox.Text == "")
            {
                errorLabel.Text = "No First Name";
                return;
            }

            if (mainWindow.DataStore.Students.Any(x => x.StudentID == idBox.Text.Trim()))
            {
                errorLabel.Text = "ID Matches an existing student. This cannot be possible";
                return;
            }
            var s = new Student()
            {
                FirstName = firstNameBox.Text.Trim(),
                LastName = lastNameBox.Text.Trim(),
                StudentID = idBox.Text.Trim(),
                Section = sectionId,
                ClassLevel = level.SelectedLevel(),
            };
            mainWindow.DataStore.AddStudent(s);
            Close();
        })
        {
            Text = "Add Student"
        });
        layout.Add(new Button((e, a) =>
        {
            //add student
            Close();
        })
        {
            Text = "Cancel"
        });
        layout.EndHorizontal();
        layout.AddCentered(errorLabel);
        Content = layout;
    }
}