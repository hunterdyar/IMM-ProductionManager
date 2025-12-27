using Eto.Forms;

namespace ProductionManager.Views;

public class HoverManager
{
    private Project? HoveredProject;
    
    public void SetHoveredProject(Project? project, Control c, MouseEventArgs e)
    {
        if (HoveredProject != null)
        {
            HoveredProject.Hovering = false;
        }
        if(HoveredProject != project){
            HoveredProject = project;
            Console.WriteLine("HoveredProject: " + HoveredProject);
        }
    }
}