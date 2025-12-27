using Eto.Forms;

namespace ProductionManager.Views;

public class StudentDetailsView(MainWindow mainWindow) : GroupBox
{
    private MainWindow _mainWindow = mainWindow;
    public Student? CurrentStudent { get; private set; }

    public void SetStudent(Student student)
    {
        CurrentStudent = student;
        if (CurrentStudent != null)
        {
            Content = new Label()
            {
                Text = CurrentStudent?.FirstName,
            };
        }
        else
        {
            Content = null;
        }
    }
    
}