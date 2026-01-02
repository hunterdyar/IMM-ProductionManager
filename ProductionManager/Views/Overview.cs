using Eto.Forms;

namespace ProductionManager.Views;

public class Overview : GroupBox
{
    private MainWindow _mainWindow;
    //todo: control bar
        //close (go to open/save state)
    
    //todo: single student view
    public Overview(MainWindow mainWindow) : base()
    {
        _mainWindow = mainWindow;
        var sv = new StudentView(mainWindow);
        var week = new SingleWeekView(mainWindow);
        var pv = new Label();
        var sem = new SemesterView(mainWindow);
        pv.Text = "projects view goes here";
        Content = new TabControl()
        {
            Pages = { new TabPage(sv)
                {
                    Text = "Students",
                },
                new TabPage(week)
                {
                    Text = "Week",
                },
                new TabPage(sem)
                {
                    Text = "Semester",
                }
                ,new TabPage(pv)
                {
                    Text = "All Projects",
                } 
            },
            SelectedIndex = 1,
        };
    }
}