using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class WeekView : GroupBox
{
    private MainWindow _mainWindow;
    private List<StudentWeekView> _studentWeekViews;
    public WeekView(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;

        StackLayout gv = new StackLayout();
        Content = gv;
        _studentWeekViews = new List<StudentWeekView>();
        var sws = mainWindow.DataStore.GetStudentWeeks();
        foreach (var sw in sws)
        { 
            var swv =  new StudentWeekView(sw);
            gv.Items.Add(swv);      
            swv.MinimumSize = new Size(40*15, 20*mainWindow.DataStore.Students.Count);
        }
        Content = gv;
    }
}