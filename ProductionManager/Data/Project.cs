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
}