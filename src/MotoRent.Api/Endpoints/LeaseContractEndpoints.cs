using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoRent.Api.Request;
using MotoRent.Api.Response;
using MotoRent.Application.Commands.LeaseContracts.CreateLeaseContract;
using MotoRent.Application.Commands.LeaseContracts.UpdateLeaseContractEndContract;
using MotoRent.Application.Queries.LeaseContracts.GetLeaseContractById;
using MotoRent.Application.Queries.LeaseContracts.GetLeaseContracts;

namespace MotoRent.Api.Endpoints
{
	public static class LeaseContractEndpoints
	{
		public static void MapLeaseContractEndpoints(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/locacao")
						   .WithTags("locação");

			group.MapPost("/", CreateLeaseContract)
				 .WithSummary("Alugar uma moto")
				 .WithDescription("Registra um contrato de locação entre um novo entregador e uma moto no sistema e retorna o ID criado.")
				 .Produces<Guid>(StatusCodes.Status201Created)
				 .ProducesValidationProblem(StatusCodes.Status400BadRequest);

			group.MapGet("/{id}", GetLeaseContract)
			.WithSummary("Consultar locação existentes por id")
			.WithDescription("Recupera os detalhes de um contrato de locação a partir do seu identificador único.")
			.Produces<CourierResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

			group.MapGet("/", ListLeaseContract)
				 .WithSummary("Consultar locações existentes")
				 .WithDescription("Lista locações as motocicletas cadastradas, com filtro opcional pela tipo de Plano.")
				 .Produces<IEnumerable<CourierResponse>>(StatusCodes.Status200OK);

			group.MapPut("/{id}/devolucao", UpdateLeaseContractEndContract)
				 .WithSummary("Informar data de devolução e calcular valor")
				 .WithDescription("Informar data de devolução e calcular valores de um contrato de locação existente pelo seu identificador único.")
				 .Produces(StatusCodes.Status204NoContent)
				 .Produces(StatusCodes.Status404NotFound);
		}

		private static async Task<IResult> CreateLeaseContract(
			[FromBody] CreateLeaseContractRequest request,
			[FromServices] IMediator mediator)
		{
			CreateLeaseContractCommand command = new(request.CourierId, request.MotorcycleId, request.StartDate, request.EndDate, request.ExpectedEndDate, request.Plan);
			var id = await mediator.Send(command);
			return Results.Created($"/locacao/{id}", new { Id = id });
		}

		private static async Task<IResult> GetLeaseContract(
			[FromRoute] Guid id,
			[FromServices] IMediator mediator)
		{
			var query = new GetLeaseContractByIdQuery(id);
			var leaseContract = await mediator.Send(query);

			if (leaseContract is null)
				return Results.NotFound();

			return Results.Ok(leaseContract.ToResponse());
		}

		private static async Task<IResult> ListLeaseContract(
		   [FromQuery] int? tipoPlano,
		   [FromServices] IMediator mediator)
		{
			var query = new GetLeaseContractsQuery(tipoPlano);
			var leaseContracts = await mediator.Send(query);
			return Results.Ok(leaseContracts.ToResponse());
		}

		private static async Task<IResult> UpdateLeaseContractEndContract(
			[FromRoute] Guid id,
			[FromBody] UpdateLeaseContractDelivereyDateRequest request,
			[FromServices] IMediator mediator)
		{
			bool success = await mediator.Send(new UpdateLeaseContractEndContractCommand(id, request.DeliveryDate));
			return success ? Results.NoContent() : Results.NotFound();
		}
	}
}
