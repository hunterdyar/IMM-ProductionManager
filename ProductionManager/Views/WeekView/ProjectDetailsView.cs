using Eto.Forms;

namespace ProductionManager.Views;

public class ProjectDetailsView : GroupBox
{
    public Project? SelectedProject;

    public void SetProjectView(Project? project)
    {
        if (project == null)
        {
            Content = null;
        }

        Content = new Label()
        {
            Text = project.ToString() + " r: " + project.Rubric + " note: " + project.Note,
        };
        
        SelectedProject = project;
    }
}