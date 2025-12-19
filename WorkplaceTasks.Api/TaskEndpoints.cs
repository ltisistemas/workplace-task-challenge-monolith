using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using WorkplaceTasks.Application.Interfaces;
using WorkplaceTasks.Application.Tasks.Dtos;
using WorkplaceTasks.Application.Tasks;

namespace WorkplaceTasks.Api;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks").RequireAuthorization();

        // GET /tasks
        group.MapGet("/", async (ITaskService taskService, CancellationToken cancellationToken) =>
        {
            try
            {
                var tasks = await taskService.GetAllAsync(cancellationToken);
                return Results.Ok(tasks);
            }
            catch (System.Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("GetAllTasks")
        .WithSummary("Get all tasks")
        .Produces<IEnumerable<TaskResponse>>(StatusCodes.Status200OK);

        // GET /tasks/{id}
        group.MapGet("/{id}", async (Guid id, ITaskService taskService, CancellationToken cancellationToken) =>
        {
            try
            {
                if (id == Guid.Empty) return Results.BadRequest("Id da tarefa é obrigatório");
                var task = await taskService.GetByIdAsync(id, cancellationToken);
                if (task == null) return Results.NotFound("Tarefa não encontrada");
                return Results.Ok(task);
            }
            catch (System.Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("GetTaskById")
        .WithSummary("Get a task by id")
        .Produces<TaskResponse>(StatusCodes.Status200OK);

        // POST /tasks
        group.MapPost("/", async (CreateTaskItemRequest req, ITaskService taskService, CancellationToken cancellationToken, HttpContext httpContext) =>
        {
            try
            {
                var userId = JwtHelper.GetUserId(httpContext);
                
                var task = await taskService.CreateAsync(req, userId, cancellationToken);
                return Results.Ok(task);
            }
            catch (System.Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("CreateTask")
        .WithSummary("Create a new task")
        .Produces<TaskResponse>(StatusCodes.Status201Created);

        // PUT /tasks/{id}
        group.MapPut("/{id}", async (Guid id, UpdateTaskItemRequest request, ITaskService taskService, CancellationToken cancellationToken, HttpContext httpContext) =>
        {
            try
            {
                var userId = JwtHelper.GetUserId(httpContext);
                request = request with { Id = id };
                var task = await taskService.UpdateAsync(request, userId, cancellationToken);
                if (task == null) return Results.NotFound("Tarefa não encontrada");
                return Results.Ok(task);
            }
            catch (System.Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("UpdateTask")
        .WithSummary("Update a task")
        .Produces<TaskResponse>(StatusCodes.Status200OK);

        // DELETE /tasks/{id}
        group.MapDelete("/", async (Guid taskId, Guid taskUserid, ITaskService taskService, CancellationToken cancellationToken, HttpContext httpContext) =>
        {
            try
            {
                var userId = JwtHelper.GetUserId(httpContext);
                var task = await taskService.DeleteAsync(new DeleteTaskItemRequest(taskId, taskUserid), userId, cancellationToken);
                return Results.Ok(task);
            }
            catch (System.Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("DeleteTask")
        .WithSummary("Soft delete a task")
        .Produces<TaskResponse>(StatusCodes.Status200OK);

        // depois você implementa os endpoints aqui chamando a camada Application
        return app;
    }
}