using Microsoft.AspNetCore.Routing;

namespace WorkplaceTasks.Api;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        // depois você implementa os endpoints aqui chamando a camada Application
        return app;
    }
}