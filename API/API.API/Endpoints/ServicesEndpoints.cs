using System.Net.Mime;
using API.API.Common;
using API.Application.Common;
using API.Application.Services.Contracts;
using API.Application.Services.Create;
using API.Application.Services.Delete;
using API.Application.Services.DTOs;
using API.Application.Services.GetById;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Endpoints;

public static class ServicesEndpoints
{
    public static void MapServicesEndpoints(
        this IEndpointRouteBuilder app
    )
    {
        var group = app.MapGroup("/services").WithTags("Services");
        group.WithMetadata(new ProducesAttribute(MediaTypeNames.Application.Json));

        /// Create
        group.MapPost("/", async Task<IResult> (
            [FromBody] CreateServiceCommand cmd,
            CreateServiceHandler handler,
            HttpContext http) =>
        {
            var result = await handler.HandleAsync(cmd, http.RequestAborted);
            return HttpResultsMapper.ToHttp(result,
                onSuccess: dto => Results.Created($"/services/{dto.Id}", dto),
                conflictWhen: e => e.Contains("already exists", StringComparison.OrdinalIgnoreCase));
        })
        .Produces<ServiceDTO>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict);

        /// GetById
        group.MapGet("/{id:guid}", async Task<IResult> (
            Guid id,
            GetServiceByIdHandler handler,
            HttpContext http) =>
        {
            var result = await handler.HandleAsync(new GetServiceByIdQuery(id), http.RequestAborted);
            return HttpResultsMapper.ToHttp(result,
                onSuccess: dto => Results.Ok(dto),
                notFoundWhen: e => e.Contains("not found", StringComparison.OrdinalIgnoreCase));
        })
        .Produces<ServiceDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);

        /// List (paginado simple)
        group.MapGet("/", async Task<Ok<IReadOnlyList<ServiceProjection>>> (
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            IServiceRepository repo = default!,
            HttpContext http = default!) =>
        {
            var req = new PagedRequest(page, size);
            var items = await repo.ListAsync(req.Skip, req.Take, http.RequestAborted);
            return TypedResults.Ok(items);
        })
        .Produces<IReadOnlyList<ServiceProjection>>(StatusCodes.Status200OK);

        /// Delete
        group.MapDelete("/{id:guid}", async Task<IResult> (
            Guid id,
            DeleteServiceHandler handler,
            HttpContext http) =>
        {
            var result = await handler.HandleAsync(new DeleteServiceCommand(id), http.RequestAborted);
            return HttpResultsMapper.ToHttp(result,
                onSuccess: () => Results.NoContent(),
                notFoundWhen: e => e.Contains("not found", StringComparison.OrdinalIgnoreCase));
        })
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}