using Eto.Forms;
using ProductionManager;

namespace ProductionManagerLinux;

class Program
{
    
    [STAThread]
    public static void Main(string[] args)
    {
        Settings.Instance = new Settings();
        var f = new Settings().GetLastUsedItem(out var p);
        new Application().Run(new MainWindow(p));
    }
}