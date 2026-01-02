using Eto.Forms;
using ProductionManager.Views;

namespace ProductionManager;

public enum MainWindowState
{
    SaveLoad,
    OverView   
}
public class MainWindow : Form
{
    private MainWindowState _state = MainWindowState.SaveLoad;
    public DataStore? DataStore { get; private set; }

    public MainWindow(string filePath)
    {
        if (!String.IsNullOrEmpty(filePath))
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                DataStore = new DataStore(filePath);
                SwitchToMode(MainWindowState.OverView);
                return;
            }
        }
        
        SwitchToMode(MainWindowState.SaveLoad);
    }

    public void SetDatastore(DataStore dataStore)
    {
        DataStore = dataStore;
    }

    public void SwitchToMode(MainWindowState state)
    {
        _state = state;
        switch (_state)
        {
            case MainWindowState.SaveLoad:
                Content = new OpenOrSaveView(this);
                break;
            case MainWindowState.OverView:
                Content = new Overview(this);
                break;
        }
        this.UpdateLayout();
    }

    protected override void OnClosed(EventArgs e)
    {
        Settings.Instance.SaveIfNeeded();
        DataStore?.SaveToBacking();
        base.OnClosed(e);
    }
}