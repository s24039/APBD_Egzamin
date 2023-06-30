using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Route("api/")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly s24039Context _context;

    public ProjectsController(s24039Context context)
    {
        _context = context;
    }

    [HttpGet("projects/{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _context.Project
            .Include(p => p.Tasks)
                .ThenInclude(t => t.TaskType)
            .Where(p => p.IdTeam == id)
            .FirstOrDefaultAsync();

        if (project == null)
        {
            return NotFound();
        }

        var projectDto = new ProjectDto
        {
            IdTeam = project.IdTeam,
            Name = project.Name,
            Deadline = project.Deadline,
            Tasks = project.Tasks
                        .Select(t => new TaskDto
                        {
                            IdTask = t.IdTask,
                            Name = t.Name,
                            Description = t.Description,
                            Deadline = t.Deadline,
                            TaskTypeName = t.TaskType.Name
                        })
                        .OrderByDescending(t => t.Deadline)
                        .ToList()
        };

        return Ok(projectDto);
    }

    [HttpPost("tasks")]
    public async Task<ActionResult<TaskDto>> AddTaskToProject(int id, TaskDto taskDto)
    {
        var project = await _context.Project.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        var taskType = await _context.TaskTypes.FirstOrDefaultAsync(tt => tt.Name == taskDto.TaskTypeName);

        if (taskType == null)
        {
            // Tworzenie nowego typu zadania, jeśli nie istnieje
            taskType = new TaskType { Name = taskDto.TaskTypeName };
            _context.TaskTypes.Add(taskType);
        }

        var task = new Models.Task
        {
            Name = taskDto.Name,
            Description = taskDto.Description,
            Deadline = taskDto.Deadline,
            TaskType = taskType,
            Project = project
        };

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                transaction.Commit();

                // Utwórz DTO dla nowo utworzonego zadania
                var newTaskDto = new TaskDto
                {
                    IdTask = task.IdTask,
                    Name = task.Name,
                    Description = task.Description,
                    Deadline = task.Deadline,
                    TaskTypeName = task.TaskType.Name
                };

                return CreatedAtAction("GetProject", new { id = project.IdTeam }, newTaskDto);
            }
            catch
            {
                transaction.Rollback();
                return StatusCode(500);
            }
        }
    }

}

// Klasa DTO dla zadania.
public class TaskDto
{
    public int IdTask { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public string TaskTypeName { get; set; }
}

// Klasa DTO dla projektu.
public class ProjectDto
{
    public int IdTeam { get; set; }
    public string Name { get; set; }
    public DateTime Deadline { get; set; }
    public List<TaskDto> Tasks { get; set; }
}
