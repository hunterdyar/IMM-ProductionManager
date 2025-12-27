using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class StudentWeekView : Drawable
{
    private StudentWeek _studentWeek;
    private Color Black = Color.Parse("#000000");
    private Font _font;
    private Pen _dashed = new Pen(Colors.Gray, 1f);
    private Pen _solidLines = new Pen(Colors.Black, 2f);
    public StudentWeekView(StudentWeek studentWeek)
    {
        _studentWeek = studentWeek;
        var ff = FontFamilies.Sans.Typefaces.First();
        _font = new Font(ff, 12);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        int nameColW = 50;
        int baseWidth = (Width-nameColW) / 15;
        Brush b = new SolidBrush(Black);
        e.Graphics.DrawText(_font, b, 0, Height/2- (_font.LineHeight/2), _studentWeek.Student.ToString());
        for (int i = 0; i < 15; i++)
        {
            var p = _studentWeek.GetProjectForWeek(i+1);
            var c = GetColor(p.Grade);
            var y = 0;
            var x = nameColW+(i*baseWidth);
            var h = Height;
            var w = baseWidth * p.Length;
            e.Graphics.FillRectangle(c, x, y, w, h);
            if (p.Length != 1)
            {
                for (int j = 0; j < p.Length; j++)
                {
                    var dx = nameColW+((i+j)*baseWidth);
                    e.Graphics.DrawRectangle(_dashed, dx, y, baseWidth, h);
                }
            }
            e.Graphics.DrawRectangle(_solidLines, x, y, w, h);


            i += p.Length - 1;//skip ahead for multi-week cells.

        }
    }
    
    private Color GetColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.NotStarted:
                return Color.Parse("#CCCCCC");
            case Grade.Unsatisfactory:
                return Color.Parse("#FF0000");
            case Grade.Satisfactory:
                return Color.Parse("#00FF00");
        }

        return Color.Parse("#FFFFFF");
    }
}