﻿namespace MotoRent.Api.Middleware
{
	public class ValidationExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ValidationExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (FluentValidation.ValidationException ex)
			{
				context.Response.StatusCode = StatusCodes.Status400BadRequest;
				context.Response.ContentType = "application/json";

				var errors = ex.Errors
					.GroupBy(e => e.PropertyName)
					.ToDictionary(
						g => g.Key,
						g => g.Select(e => e.ErrorMessage).ToArray()
					);

				await context.Response.WriteAsJsonAsync(new
				{
					Message = "Validation Failed",
					Status = 400,
					Errors = errors
				});
			}
		}
	}

}
