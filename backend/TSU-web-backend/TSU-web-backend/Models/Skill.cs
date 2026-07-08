namespace TSU_web_backend.Models;

public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<StudentSkill> StudentSkills { get; set; } = [];
}
