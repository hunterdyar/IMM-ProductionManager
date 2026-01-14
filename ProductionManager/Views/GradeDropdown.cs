using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class GradeDropdown : DropDown
{
    public Action<Grade> OnSelectedGradeChanged;
    public GradeDropdown() : base()
    {
        Items.Add("Not Started");
        Items.Add("Started");
        Items.Add("Submitted/Ungraded");
        Items.Add("Satisfactory");
        Items.Add("Not Satisfactory");
        SelectedValueChanged += OnSelectedValueChanged;
        //this.BackgroundColor = GetDropdownBackgroundColor(SelectedGrade());
    }

    public Grade SelectedGrade()
    {
        var i = SelectedIndex;
        Grade g = Grade.Unknown;
        switch (i)
        {
            case 0:
                g = Grade.NotStarted;
                break;
            case 1:
                g = Grade.Started;
                break;
            case 2:
                g = Grade.Ungraded;
                break;
            case 3:
                g = Grade.Satisfactory;
                break;
            case 4:
                g = Grade.Unsatisfactory;
                break;
            // case -1:
            default:
                g = Grade.Unknown;
                break;
        }

        return g;
    }
    private void OnSelectedValueChanged(object? sender, EventArgs e)
    {
        var g = SelectedGrade();
       // this.BackgroundColor = GetDropdownBackgroundColor(g);
        
        OnSelectedGradeChanged?.Invoke(g);
    }

    private static Color GetDropdownBackgroundColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.Satisfactory:
                return Colors.Green;
            case Grade.Unsatisfactory:
                return Colors.Red;
            case Grade.NotStarted:
                return Colors.LightGrey;
            case Grade.Started:
                return Colors.Lavender;
            case Grade.Ungraded:
                return Colors.Cornsilk;
        }

        return Colors.White;
    }

    public void SetGrade(Grade grade)
    {
        //Items.Add("Not Started");
        // Items.Add("Started");
        // Items.Add("Satisfactory");
        // Items.Add("Not Satisfactory");
        switch (grade)
        {
            case Grade.NotStarted:
                SelectedIndex = 0;
                break;
            case Grade.Started:
                SelectedIndex = 1;
                break;
            case Grade.Ungraded:
                SelectedIndex = 2;
                break;
            case Grade.Satisfactory:
                SelectedIndex = 3;
                break;
            case Grade.Unsatisfactory:
                SelectedIndex = 4;
                break;
            default:
                SelectedIndex = -1;
                break;
        }
    }
}