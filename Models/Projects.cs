using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class TaskType
{
    [Key]
    public int IdTaskType { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}

public class Task
{
    [Key]
    public int IdTask { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; }

    public DateTime Deadline { get; set; }

    [ForeignKey("Project")]
    public int IdTeam { get; set; }

    public virtual Project Project { get; set; }

    [ForeignKey("TaskType")]
    public int IdTaskType { get; set; }

    public virtual TaskType TaskType { get; set; }

    [ForeignKey("AssignedTo")]
    public int IdAssignedTo { get; set; }

    public virtual TeamMember AssignedTo { get; set; }

    [ForeignKey("Creator")]
    public int IdCreator { get; set; }

    public virtual TeamMember Creator { get; set; }
}

public class TeamMember
{
    [Key]
    public int IdTeamMember { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}

public class Project
{
    [Key]
    public int IdTeam { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public DateTime Deadline { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}
