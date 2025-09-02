using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoRent.Api.Request;
using MotoRent.Api.Response;
using MotoRent.Application.Commands.Motorcycles.CreateMotorcycle;
using MotoRent.Application.Commands.Motorcycles.DeleteMotorcycle;
using MotoRent.Application.Commands.Motorcycles.UpdateMotorcycleLicensePlate;
using MotoRent.Application.Queries.Motorcycles.GetMotorcycleById;
using MotoRent.Application.Queries.Motorcycles.GetMotorcycles;

namespace MotoRent.Api.Endpoints
{
	public static class MotorcycleEndpoints
	{
		public static void MapMotorcycleEndpoints(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/motos")
						   .WithTags("motos");

			group.MapPost("/", CreateMotorcycle)
				 .WithSummary("Cadastrar uma nova moto")
				 .WithDescription("Registra uma nova motocicleta no sistema e retorna o ID criado.")
				 .Produces<Guid>(StatusCodes.Status201Created)
				 .ProducesValidationProblem(StatusCodes.Status400BadRequest);

			group.MapGet("/{id:guid}", GetMotorcycle)
			.WithSummary("Consultar motos existentes por id")
			.WithDescription("Recupera os detalhes de uma motocicleta a partir do seu identificador único.")
			.Produces<MotorcycleResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

			group.MapGet("/", ListMotorcycles)
				 .WithName("ListMotorcycles")
				 .WithSummary("Consultar motos existentes")
				 .WithDescription("Lista todas as motocicletas cadastradas, com filtro opcional pela placa.")
				 .Produces<IEnumerable<MotorcycleResponse>>(StatusCodes.Status200OK);

			group.MapPut("/{id}/placa", UpdateMotorcycleLicensePlate)
				 .WithSummary("Modificar a placa de uma moto")
				 .WithDescription("Modificar a placa de uma motocicleta existente pelo seu identificador único.")
				 .Produces(StatusCodes.Status204NoContent)
				 .Produces(StatusCodes.Status404NotFound);

			group.MapDelete("/{id}", DeleteMotorcycle)
				 .WithSummary("Remover uma moto")
				 .WithDescription("Remove uma motocicleta existente pelo seu identificador único.")
				 .Produces(StatusCodes.Status204NoContent)
				 .Produces(StatusCodes.Status404NotFound);
		}

		private static async Task<IResult> CreateMotorcycle(
			[FromBody] CreateMotorcycleRequest request,
			[FromServices] IMediator mediator)
		{
			CreateMotortcycleCommand command = new(request.Year, request.Model, request.LicensePlate, request.Identifier);
			var id = await mediator.Send(command);
			return Results.Created($"/motos/{id}", new { Id = id });
		}

		private static async Task<IResult> GetMotorcycle(
			[FromRoute] Guid id,
			[FromServices] IMediator mediator)
		{
			var query = new GetMotorcycleByIdQuery(id);
			var motorcycle = await mediator.Send(query);

			if (motorcycle is null)
				return Results.NotFound();

			return Results.Ok(motorcycle.ToResponse());
		}

		private static async Task<IResult> ListMotorcycles(
		   [FromQuery] string? licensePlate,
		   [FromServices] IMediator mediator)
		{
			var query = new GetMotorcyclesQuery(licensePlate);
			var motorcycles = await mediator.Send(query);
			return Results.Ok(motorcycles.ToResponse());
		}

		private static async Task<IResult> UpdateMotorcycleLicensePlate(
			[FromRoute] Guid id,
			[FromBody] UpdateLicensePlateRequest request,
			[FromServices] IMediator mediator)
		{
			bool success = await mediator.Send(new UpdateMotorcycleLicensePlateCommand(id, request.LicensePlate));
			return success ? Results.NoContent() : Results.NotFound();
		}

		private static async Task<IResult> DeleteMotorcycle(
			[FromRoute] Guid id,
			[FromServices] IMediator mediator)
		{
			bool success = await mediator.Send(new DeleteMotorcycleCommand(id));
			return success ? Results.NoContent() : Results.NotFound();
		}
	}
}
