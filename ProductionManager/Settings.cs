namespace ProductionManager;

public class Settings
{
    public const int TotalWeeks = 15;
    public static Settings Instance = new Settings();
    
    public string LastUsedPath {
        get
        {
            if (_needsRead)
            {
                LoadSettings();
            }

            return _lastUsedPath;
        }
        set
        {
            _lastUsedPath = value;
            _needsSave = true;
        }
    }
    public int SelectedWeek {
        get
        {
            if (_needsRead)
            {
                LoadSettings();
            }

            return _selectedWeek;
        }
        set
        {
            if (value < 0 || value > TotalWeeks)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            _selectedWeek = value;
            _needsSave = true;
        }
    }
    private string _lastUsedPath;
    private int _selectedWeek;

    //
    private bool _needsRead;
    private bool _needsSave;
    public  Settings()
    {
        _needsRead = true;
        _needsSave = false;
    }
    //todo: load/save settings as single methods that save them here. 
    public virtual bool LoadSettings()
    {
        var s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var file = "immProductionManagerSettings.txt";
        var settingsFile = Path.Combine(s, file);
        FileInfo fi = new FileInfo(settingsFile);
        if (fi.Exists)
        {
            var read = fi.OpenText().ReadToEnd();
            var lines = read.Split(Environment.NewLine);
            if (lines.Length > 0)
            {
                _lastUsedPath = lines[0];
            }
            else
            {
                _lastUsedPath = "";
            }

            if (lines.Length > 1 && int.TryParse(lines[1], out var week))
            {
                _selectedWeek = week;
            }
            else
            {
                Console.WriteLine("Warning. Found path but not selected week. Corrupted settings file? imma ignore that for now.");
                _selectedWeek = 0;
                _needsSave = true; //overwrite this bad data i guess.
            }
            _needsRead = false;
            return true;
        }
        else
        {
            fi.Create().Close();
            _lastUsedPath = "";
            _selectedWeek = 0;
            _needsRead = false;
            return false;
        }
        
    }

    
    public void SaveSettings()
    {
        Console.WriteLine("saving settings...");
        var s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var file = "immProductionManagerSettings.txt";
        var settingsFile = Path.Combine(s, file);
        FileInfo fi = new FileInfo(settingsFile);
        StreamWriter sw = new StreamWriter(settingsFile);
        sw.WriteLine(_lastUsedPath);
        sw.WriteLine(_selectedWeek.ToString());
        sw.Flush();
        sw.Close();
        _needsSave = false;
    }

    public void SaveIfNeeded()
    {
        if (_needsSave)
        {
            SaveSettings();
        }
    }
}