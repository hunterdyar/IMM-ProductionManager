namespace ProductionManager;

public class StudentWeek
{
    public Student Student;
    public List<Project> Projects;

    public StudentWeek(Student student, DataStore dataStore)
    {
        Student = student;
        Projects = dataStore.Projects.Where(x=>x.Students.Contains(student)).ToList();
    }
    
    public Project GetProjectForWeek(int week)
    {
        foreach (var project in Projects)
        {
            if (project.Week == week)
            {
                return project;
            }

            if (project.Week < week && project.Week + project.Length > week)
            {
                return project;
            }
        }

        return Project.EmptyProject;
    }
}