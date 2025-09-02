﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MotoRent.Application.Common.Behaviors;

namespace MotoRent.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly)
			);

			services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

			return services;
		}
	}
}
