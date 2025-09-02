using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoRent.Api.Request;
using MotoRent.Api.Response;
using MotoRent.Application.Commands.Couriers.CreateCourier;
using MotoRent.Application.Commands.Couriers.UpdateCourierDriverLicenseImage;
using MotoRent.Application.Queries.Couriers.GetCourierById;
using MotoRent.Application.Queries.Couriers.GetCouriers;

namespace MotoRent.Api.Endpoints
{
	public static class CourierEndpoints
	{
		public static void MapCourierEndpoints(this IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("/entregadores")
						   .WithTags("entregadores");

			group.MapPost("/", CreateCourier)
				 .WithSummary("Cadastrar um entregador")
				 .WithDescription("Registra um novo entregador no sistema e retorna o ID criado.")
				 .Accepts<CreateCourierRequest>("multipart/form-data")
				 .Produces<Guid>(StatusCodes.Status201Created)
				 .ProducesValidationProblem(StatusCodes.Status400BadRequest)
				 .DisableAntiforgery();

			group.MapGet("/{id}", GetCourier)
			.WithSummary("Consultar entregadores existentes por id")
			.WithDescription("Recupera os detalhes de uma motocicleta a partir do seu identificador único.")
			.Produces<CourierResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

			group.MapGet("/", ListCourier)
				 .WithSummary("Consultar entregadores existentes")
				 .WithDescription("Lista entregadores as motocicletas cadastradas, com filtro opcional pela tipo de CNH.")
				 .Produces<IEnumerable<CourierResponse>>(StatusCodes.Status200OK);

			group.MapPut("/{id}/cnh", UpdateCourierDriverLicenseImage)
				 .WithSummary("Enviar foto da CNH")
				 .WithDescription("Modificar a foto da CNH de um entregador existente pelo seu identificador único.")
				 .Accepts<UpdateCourierDriverLicenseImageRequest>("multipart/form-data")
				 .Produces(StatusCodes.Status204NoContent)
				 .Produces(StatusCodes.Status404NotFound);
		}

		private static async Task<IResult> CreateCourier(
			[FromForm] CreateCourierRequest request,
			[FromServices] IMediator mediator)
		{

			await using var stream = request.Imagem_Cnh.OpenReadStream();

			CreateCourierCommand command = new(
				request.Identificador,
		request.Nome,
		request.Cnpj,
		request.Data_Nascimento,
		request.Numero_Cnh,
		request.Tipo_Cnh,
		stream,
		request.Imagem_Cnh.FileName,
		request.Imagem_Cnh.ContentType
			);
			var id = await mediator.Send(command);
			return Results.Created($"/entregadores/{id}", new { Id = id });
		}

		private static async Task<IResult> GetCourier(
			[FromRoute] Guid id,
			[FromServices] IMediator mediator)
		{
			var query = new GetCourierByIdQuery(id);
			var courier = await mediator.Send(query);

			if (courier is null)
				return Results.NotFound();

			return Results.Ok(courier.ToResponse());
		}

		private static async Task<IResult> ListCourier(
		   [FromQuery] string? tipoCnh,
		   [FromServices] IMediator mediator)
		{
			var query = new GetCouriersQuery(tipoCnh);
			var couriers = await mediator.Send(query);
			return Results.Ok(couriers.ToResponse());
		}

		private static async Task<IResult> UpdateCourierDriverLicenseImage(
			[FromRoute] Guid id,
			[FromForm] UpdateCourierDriverLicenseImageRequest request,
			[FromServices] IMediator mediator)
		{
			await using var stream = request.File.OpenReadStream();

			var command = new UpdateCourierDriverLicenseImageCommand
			{
				Id = id,
				FileName = request.File.FileName,
				ContentType = request.File.ContentType,
				FileStream = stream
			};
			bool success = await mediator.Send(command);
			return success ? Results.NoContent() : Results.NotFound();
		}
	}
}
