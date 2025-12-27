using ClosedXML.Excel;

namespace ProductionManager;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StudentID { get; set; }
    public int Section;
    public ClassLevel ClassLevel { get; set; }

    public static Student StudentFromRow(IXLRow row)
    {
        Student s = new Student()
        {
            FirstName = row.Cell("A").GetString(),
            LastName = row.Cell("B").GetString(),
            StudentID = row.Cell("C").GetString(),
            Section = (int)row.Cell("D").GetDouble(),
            ClassLevel = ToClassLevel(row.Cell("E").GetDouble()),
        };
        return s;
    }

    private static ClassLevel ToClassLevel(double value)
    {
        switch (value)
        {
            case 200:
                return ClassLevel.TwoHundred;
            case 300:
                return ClassLevel.ThreeHundred;
            case 400:
                return ClassLevel.FourHundred;
            default:
                return ClassLevel.Unknown;
        }
    }

    public override string ToString()
    {
        return FirstName+" "+LastName;
    }
}