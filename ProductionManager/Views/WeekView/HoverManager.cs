using Eto.Forms;

namespace ProductionManager.Views;

public class HoverManager
{
    private Project? HoveredProject;
    private StudentWeekView? _hoveredWeekView;
    public HoverManager()
    {
    }
    public void SetHoveredProject(Project? project, StudentWeekView c, MouseEventArgs e)
    {
        if (HoveredProject != null)
        {
            HoveredProject.Hovering = false;
        }

        //when we go 'up' or 'down' between sets of weeks.
        
        if(_hoveredWeekView != c){
            if (_hoveredWeekView != null)
            {
                _hoveredWeekView.SetDirty();
                Console.WriteLine("set old hover week dirty");
            }

            if (c != null)
            {
                c.SetDirty();
                Console.WriteLine("set new hover week dirty");

            }
            _hoveredWeekView = c;
        }
        
        if(HoveredProject != project){
            HoveredProject = project;
            if (HoveredProject != null)
            {
                HoveredProject.Hovering = true;
                Console.WriteLine($"set hover to {HoveredProject}");
                c.SetDirty();
               //Console.WriteLine("HoveredProject: " + HoveredProject);
            }
        }
    }
}