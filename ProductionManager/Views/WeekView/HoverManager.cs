using Eto.Forms;

namespace ProductionManager.Views;

//todo: remove and move into WeekView.
public class HoverManager
{
    private Project? HoveredProject;
    private StudentWeekView? _hoveredWeekView;
    private WeekView _weekView;
    public HoverManager(WeekView weekView)
    {
        _weekView = weekView;
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
            }

            if (c != null)
            {
                c.SetDirty();
            }
            _hoveredWeekView = c;
        }
        
        if(HoveredProject != project){
            
            if (HoveredProject != null && HoveredProject.Students.Length > 2)
            {
                _weekView?.Invalidate();
            }
            
            HoveredProject = project;
            if (HoveredProject != null)
            {
                HoveredProject.Hovering = true;
                if (HoveredProject.Students.Length > 1)
                {
                   _weekView?.Invalidate();
                }
                c.SetDirty();
            }
        }
    }
}