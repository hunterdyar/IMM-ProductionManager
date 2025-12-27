using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using ClosedXML.Excel;
 
namespace ProductionManager;

/// <summary>
/// Interface into getting and saving data to the spreadsheet.
/// </summary>
public class DataStore
{
    private XLWorkbook _workbook;
    private FileInfo _workbookFile;

    private List<Student> _students = new List<Student>();
    public List<Student> Students => _students;

    public List<Project> Projects => _projects;
    private List<Project> _projects = new List<Project>();

    private FileInfo _backingStore;

    private bool _dirty = false;
    public static Action<DataStore> OnCreated;
    public DataStore(string filePath, bool createOrOverwrite = false)
    {
        _backingStore = new FileInfo(filePath);
        if (_backingStore.Exists)
        {
            LoadFromBacking();
        }
        else if (createOrOverwrite)
        {
            var info = new FileInfo(filePath);
            if (info.Exists)
            {
                info.Delete();
            }

            var stream = info.Create();
            _workbook = new XLWorkbook();
            _workbook.AddWorksheet("students");
            _workbook.AddWorksheet("projects");
            _workbook.SaveAs(stream);
            stream.Close();
        }else
        {
            throw new FileNotFoundException(filePath);
        }
    }

    private void LoadFromBacking()
    {
        _workbook = new XLWorkbook(_backingStore.FullName);

        _students.Clear();
        _projects.Clear();

        var studentSheet = _workbook.Worksheet("students");
        foreach (var row in studentSheet.Rows().Skip(1))
        {
            _students.Add(Student.StudentFromRow(row));
        }
        
        var projectSheet = _workbook.Worksheet("projects");
        foreach (var row in projectSheet.Rows().Skip(1))
        {
            _projects.Add(Project.ProjectFromRow(row, this));
        }
        
        Settings.Instance.SetLastUsedItem(_backingStore.FullName);
        _dirty = false;
    }

    private void SaveToBacking(bool onlyIfDirty = true)
    {
        Console.WriteLine("todo: save!");
    }


    public List<StudentWeek> GetStudentWeeks()
    {
        var sws = new List<StudentWeek>();
        foreach (var s in _students)
        {
            var sw = new StudentWeek(s, this);
            sws.Add(sw);
        }

        return sws;
    }

    public bool TryGetStudentWithID(string id, [NotNullWhen(true)]out Student o)
    {
        id = id.Trim();
        o = _students.Find(x => x.StudentID == id);
        return o != null;
    }
}