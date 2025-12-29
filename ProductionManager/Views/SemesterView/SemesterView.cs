using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class SemesterView : GroupBox
{
    private MainWindow _mainWindow;
    private List<StudentSemesterView> _studentWeekViews;
    private HoverManager _hoverManager;
    private ProjectDetailsView _projectDetailsView;
    public SemesterView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        _hoverManager = new HoverManager(this);
        StackLayout gv = new StackLayout();
        _studentWeekViews = new List<StudentSemesterView>();
        var sws = mainWindow.DataStore.GetStudentWeeks();
        for (var i = 0; i < sws.Count; i++)
        {
            var sw = sws[i];
            var swv = new StudentSemesterView(sw, _hoverManager,i == 0);
            swv.OnClick += OnClick;
            gv.Items.Add(swv);
            swv.MinimumSize = new Size(40 * 15, 20 * mainWindow.DataStore.Students.Count);
        }
        
        _projectDetailsView = new ProjectDetailsView();
        
        var splitter = new Splitter();
        splitter.Orientation = Orientation.Vertical;
        splitter.Panel1 = gv;
        splitter.Panel2 = _projectDetailsView;
        Content = splitter;
    }
    
    private void OnClick(Project project, StudentSemesterView arg2)
    {
        _projectDetailsView.SetProjectView(project);
    }
}