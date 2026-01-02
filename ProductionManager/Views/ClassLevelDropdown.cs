using Eto.Forms;

namespace ProductionManager.Views;

public class ClassLevelDropdown : DropDown
{
    public ClassLevelDropdown() : base()
    {
        Items.Add("200");
        Items.Add("300");
        Items.Add("400");
        SelectedValueChanged += OnSelectedValueChanged;
    }

    public ClassLevel SelectedLevel()
    {
        switch (SelectedIndex)
        {
            case 0:
                return ClassLevel.TwoHundred;
                break;
            case 1:
                return ClassLevel.ThreeHundred;
                break;
            case 2:
                return ClassLevel.FourHundred;
                break;
            default:
                return ClassLevel.Unknown;
        }
    }
    private void OnSelectedValueChanged(object? sender, EventArgs e)
    {
        var g = SelectedLevel();
       // this.BackgroundColor = GetDropdownBackgroundColor(g);
    }

   

    public void SetLevel(ClassLevel level)
    {
        //Items.Add("Not Started");
        // Items.Add("Started");
        // Items.Add("Satisfactory");
        // Items.Add("Not Satisfactory");
        switch (level)
        {
            case ClassLevel.TwoHundred:
                SelectedIndex = 0;
                break;
            case ClassLevel.ThreeHundred:
                SelectedIndex = 1;
                break;
            case ClassLevel.FourHundred:
                SelectedIndex = 2;
                break;
            default:
                SelectedIndex = -1;
                break;
        }
    }
}