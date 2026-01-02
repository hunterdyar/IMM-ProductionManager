using System.Diagnostics.CodeAnalysis;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

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
        
        Settings.Instance.LastUsedPath =_backingStore.FullName;
        _dirty = false;
    }

    public void SaveToBacking(bool onlyIfDirty = true)
    {
        _workbook = new XLWorkbook();
        var studentSheet = _workbook.AddWorksheet("students");
        var projSheet = _workbook.AddWorksheet("projects");

        //add headers
        studentSheet.Row(1).Cell("A").Value = "first name";
        studentSheet.Row(1).Cell("B").Value = "last name";
        studentSheet.Row(1).Cell("C").Value = "ID";
        studentSheet.Row(1).Cell("D").Value = "section";
        studentSheet.Row(1).Cell("E").Value = "level";
        
        projSheet.Row(1).Cell("A").Value = "students";
        projSheet.Row(1).Cell("B").Value = "week";
        projSheet.Row(1).Cell("C").Value = "length";
        projSheet.Row(1).Cell("D").Value = "rubric";
        projSheet.Row(1).Cell("E").Value = "note";
        projSheet.Row(1).Cell("F").Value = "grade";

        for (var i = 0; i < _students.Count; i++)
        {
            var student = _students[i];
            student.SetToRow(studentSheet.Row(i+2));
        }
        for (var i = 0; i < _projects.Count; i++)
        {
            var project = _projects[i];
            project.SetToRow(projSheet.Row(i+2));
        }
        
        _workbook.SaveAs(_backingStore.FullName);
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

    public bool TryGetProject(Student student, int week, out Project project)
    {
        project = Projects.Where(x=>x.Students.Contains(student)).FirstOrDefault(x=>x.Week == week || (x.Week < week && x.Week+x.Length > week));
        if (project == null)
        {
            project = null;
            return false;
        }
        return true;
    }
}