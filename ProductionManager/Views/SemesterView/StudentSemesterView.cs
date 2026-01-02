using Eto.Drawing;
using Eto.Forms;

namespace ProductionManager.Views;

public class StudentSemesterView : Drawable
{
    private StudentWeek _studentWeek;
    private Font _font;
    private Pen _dashed = new Pen(Colors.Gray, 1f);
    private Pen _solidLines = new Pen(Colors.Black, 2f);
    private bool ShowWeekNumbers;
    private HoverManager _hoverManager;

    private int _baseWidth;
    private int _nameColW;

    public Action<Project, StudentSemesterView> OnClick;
    public StudentSemesterView(StudentWeek studentWeek, HoverManager hoverManager, bool showWeekNumbers = false)
    {
        _studentWeek = studentWeek;
        var ff = FontFamilies.Sans.Typefaces.First();
        _font = new Font(ff, 12);
        ShowWeekNumbers = showWeekNumbers;
        _hoverManager = hoverManager;
        foreach (var project in studentWeek.Projects)
        {
            project.OnChange += SetDirty;
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        foreach (var project in _studentWeek.Projects)
        {
            project.OnChange += SetDirty;
        }
    }

    public Student Student => _studentWeek.Student;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        _nameColW = 50;
        _baseWidth = (Width-_nameColW) / Settings.TotalWeeks;
        Brush b = new SolidBrush(Colors.Black);
        e.Graphics.DrawText(_font, b, 0, Height/2- (_font.LineHeight/2), _studentWeek.Student.ToString());
        for (int i = 0; i < Settings.TotalWeeks; i++)
        {
            var p = _studentWeek.GetProjectForWeek(i+1);
            
            if (p == null)
            {
                p = Project.EmptyProject;
            }
            var c = GetColor(p.Grade);
            var y = 0;
            var x = _nameColW+(i*_baseWidth);
            var h = Height;
            var w = _baseWidth * p.Length;
            e.Graphics.FillRectangle(c, x, y, w, h);
            if (p.Hovering)
            {
                e.Graphics.FillEllipse(Colors.Yellow, x, y, w, h);
            }
            if (p.Length != 1)
            {
                for (int j = 0; j < p.Length; j++)
                {
                    var dx = _nameColW+((i+j)*_baseWidth);
                    e.Graphics.DrawRectangle(_dashed, dx, y, _baseWidth, h);
                }
            }
            e.Graphics.DrawRectangle(_solidLines, x, y, w, h);

            if (ShowWeekNumbers)
            {
                e.Graphics.DrawText(_font, b, x+4, Height/2- (_font.LineHeight/2), (i+1).ToString());
            }

            i += p.Length - 1;//skip ahead for multi-week cells.
        }

        if (_hoverManager.HoveredStudentSemesterView == this)
        {
            _hoverManager.DoPaint(e);
        }
    }
    
    private Color GetColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.NotStarted:
                return Colors.LightGrey;
            case Grade.Unsatisfactory:
                return Colors.DarkRed;
            case Grade.Satisfactory:
                return Colors.Green;
            case Grade.Started:
                return Colors.Lavender;
        }

        return Color.Parse("#FFFFFF");
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var mx = e.Location.X;
        mx -= _nameColW;
        var wn = Single.Floor(mx / _baseWidth)+1;
        if (wn > 0 && wn <= Settings.TotalWeeks)
        {
            _hoverManager.SetHoveredProject(_studentWeek.GetProjectForWeek((int)wn), this, e);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        var mx = e.Location.X;
        mx -= _nameColW;
        var wn = Single.Floor(mx / _baseWidth)+1;
        if (wn > 0 && wn <= Settings.TotalWeeks)
        { 
            var p = _studentWeek.GetProjectForWeek((int)wn); 
            OnClick?.Invoke(p,this);
        }
    }

    public void SetDirty()
    {
        this.Invalidate();
    }

    public PointF GetProjectScreenPosition(Project project)
    {
        var w = project.Week;
        var x =_nameColW+(w*_baseWidth);
        return PointToScreen(new PointF(x,0));
    }
}