namespace ProductionManager;

public class Settings
{
    public static Settings Instance = new Settings();
    public virtual bool GetLastUsedItem(out string path)
    {
        var s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var file = "immProductionManagerSettings.txt";
        var settingsFile = Path.Combine(s, file);
        FileInfo fi = new FileInfo(settingsFile);
        if (fi.Exists)
        {
            var read = fi.OpenText().ReadToEnd();
            path = read.Split(Environment.NewLine)[0];
            return true;
        }
        else
        {
            fi.Create().Close();
            path = "";
            return false;
        }
    }
    public void SetLastUsedItem(string lastUsed)
    {
        var s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var file = "immProductionManagerSettings.txt";
        var settingsFile = Path.Combine(s, file);
        FileInfo fi = new FileInfo(settingsFile);
        StreamWriter sw = new StreamWriter(settingsFile);
        sw.WriteLine(lastUsed);
        sw.Flush();
        sw.Close();
    }
}