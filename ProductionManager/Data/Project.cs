using System.Text;
using ClosedXML.Excel;

namespace ProductionManager;

public class Project
{
    public Student[] Students => _students;

    public static Project EmptyProject = new Project()
    {
        _students = [],
        Grade = Grade.NotStarted
    };

    private Student[] _students;
    public int Week;
    public int Length = 1;
    public string Rubric = "";
    public string Note = "";
    public Grade Grade;

    public static Project ProjectFromRow(IXLRow row, DataStore dataStore)
    {
        Project p = new Project()
        {
            _students = GetStudents(row.Cell("A").GetString(),dataStore),
            Week = (int)row.Cell("B").GetDouble(),
            Length = (int)row.Cell("C").GetDouble(),
            Rubric = row.Cell("D").GetString(),
            Note = row.Cell("E").GetString(),
            Grade = ToGrade(row.Cell("F").GetString())
        };
        return p;
    }

    private static Student[] GetStudents(string studentData, DataStore dataStore)
    {
        var ids = studentData.Split(',');
        Student[] students = new Student[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            if (dataStore.TryGetStudentWithID(ids[i], out var student))
            {
                students[i] = student;
            }
            else
            {
                throw new Exception("Student not found (ID: " + ids[i] + ")");
            }
        }

        return students;
    }

    private static Grade ToGrade(string grade)
    {
        switch (grade.Trim().ToLower())
        {
            case "s":
                return Grade.Satisfactory;
            case "ns":
            case "n":
            case "us":
                return Grade.Unsatisfactory;
            case "i":
                return Grade.NotStarted;
        }
        return Grade.Unknown;
    }

    public void SetToRow(IXLRow row)
    {
        row.Cell("A").Value = GetStudentsValue();
        row.Cell("B").Value = Week;
        row.Cell("C").Value = Length;
        row.Cell("D").Value = Rubric;
        row.Cell("E").Value = Note;
        row.Cell("F").Value = GetGradeValue();
    }

    private string GetGradeValue()
    {
        switch (Grade)
        {
            case Grade.NotStarted:
                return "i";
            case Grade.Satisfactory:
                return "s";
            case Grade.Unsatisfactory:
                return "ns";
            default:
                return "u";
        }
        
    }

    private string GetStudentsValue()
    {
        StringBuilder _sb = new StringBuilder();
        for (int i = 0; i < _students.Length; i++)
        {
            _sb.Append(_students[i].StudentID);
            if (i < _students.Length - 1)
            {
                _sb.Append(",");
            }
        }

        return _sb.ToString();
    }
}