using Eto.Forms;
using ProductionManager;

namespace ProductionManagerLinux;

class Program
{

	[STAThread]
	public static void Main(string[] args)
	{
		Settings.Instance = new Settings();
		new Application().Run(new MainWindow(Settings.Instance.LastUsedPath));
	}
}