using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class WeekView : GroupBox
{
    private MainWindow _mainWindow;
    private List<StudentWeekView> _studentWeekViews;
    private HoverManager _hoverManager;
    public WeekView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        _hoverManager = new HoverManager();
        StackLayout gv = new StackLayout();
        Content = gv;
        _studentWeekViews = new List<StudentWeekView>();
        var sws = mainWindow.DataStore.GetStudentWeeks();
        for (var i = 0; i < sws.Count; i++)
        {
            var sw = sws[i];
            var swv = new StudentWeekView(sw, _hoverManager,i == 0);
            gv.Items.Add(swv);
            swv.MinimumSize = new Size(40 * 15, 20 * mainWindow.DataStore.Students.Count);
        }

        Content = gv;
    }
}