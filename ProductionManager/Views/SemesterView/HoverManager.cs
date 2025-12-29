using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

//todo: remove and move into WeekView.
public class HoverManager
{
    private Project? HoveredProject;
    private StudentSemesterView? _hoveredSemesterView;
    private SemesterView _semesterView;
    public HoverManager(SemesterView semesterView)
    {
        _semesterView = semesterView;
    }

    public StudentSemesterView HoveredStudentSemesterView => _hoveredSemesterView;
    public void DoPaint(PaintEventArgs e)
    {
        if (HoveredProject == null)
        {
            return;
        }
        var p =_hoveredSemesterView!.GetProjectScreenPosition(HoveredProject);
        p = _semesterView.PointFromScreen(p);
        e.Graphics.DrawRectangle(Colors.MediumVioletRed,p.X,p.Y, 20,20);
    }

    public void SetHoveredProject(Project? project, StudentSemesterView c, MouseEventArgs e)
    {
        if (HoveredProject != null)
        {
            HoveredProject.Hovering = false;//clear old.
        }

        //when we go 'up' or 'down' between sets of weeks.
        
        if(_hoveredSemesterView != c){
            if (_hoveredSemesterView != null)
            {
                _hoveredSemesterView.SetDirty();
            }

            if (c != null)
            {
                c.SetDirty();
            }
            _hoveredSemesterView = c;
        }
        
        if(HoveredProject != project){
            
            if (HoveredProject != null && HoveredProject.Students.Length > 2)
            {
                _semesterView?.Invalidate();
            }else if (project != null && project.Students.Length > 2)
            {
                _semesterView?.Invalidate();
            }
            
            HoveredProject = project;
            if (HoveredProject != null)
            {
                HoveredProject.Hovering = true;
                _semesterView?.Invalidate();//f it, just brute force the redraw. its cheap anyway and none of this matters.
                c.SetDirty();
            }
        }
    }
}