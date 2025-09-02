using FluentValidation;
using MotoRent.Application.Common.Validation;
using MotoRent.Domain.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MotoRent.Application.Commands.Couriers.CreateCourier
{
	public class CreateCourierCommandValidator : AbstractValidator<CreateCourierCommand>
	{
		public CreateCourierCommandValidator(ICourierRepository courierRepository)
		{
			RuleFor(x => x.Identifier)
				.NotEmpty()
				.OverridePropertyName("Identificador")
				.WithMessage("Identificador é obrigatório");

			RuleFor(x => x.Name)
				.NotEmpty()
				.OverridePropertyName("Nome")
				.WithMessage("Nome é obrigatório");

			RuleFor(x => x.Cnpj)
				.NotEmpty()
				.OverridePropertyName("Cnpj")
				.WithMessage("Cnpj é obrigatório")
				.Must(IsValidCnpj).WithMessage("Cnpj inválido")
				.MustAsync(async (cnpj, ct) =>
				{
					bool exists = await courierRepository.ExistsByCnpjAsync(cnpj!, ct);
					return !exists;
				}).WithMessage("Cnpj já está cadastrado");

			RuleFor(x => x.BirthDate)
				.NotEmpty()
				.OverridePropertyName("Data_Nascimento")
				.WithMessage("Data_Nascimento é obrigatória")
				.Must(BeAtLeast18YearsOld)
				.WithMessage("O entregador deve ter pelo menos 18 anos");

			RuleFor(x => x.DriverLicenseNumber)
				.NotEmpty()
				.OverridePropertyName("Numero_CNH")
				.WithMessage("Numero_CNH é obrigatório")
				.MustAsync(async (number, ct) =>
				{
					bool exists = await courierRepository.ExistsByDriverLicenseNumberAsync(number!, ct);
					return !exists;
				}).WithMessage("Numero_CNH já está cadastrado");

			RuleFor(x => x.DriverLicenseType)
				.NotEmpty()
				.OverridePropertyName("Tipo_CNH")
				.WithMessage("Tipo_CNH é obrigatório")
				.Must(type => type == "A" || type == "B" || type == "A+B")
				.WithMessage("Tipo_CNH deve ser A, B ou A+B");

			RuleFor(x => x.FileStream)
				.NotNull().WithMessage("Imagem da CNH é obrigatória")
				.Must(fs => fs.Length > 0).WithMessage("Arquivo de imagem da CNH não pode estar vazio")
				.Must(fs => fs.Length <= FileValidationConstants.MaxImageFileSizeBytes)
				.WithMessage($"Imagem da CNH deve ter no máximo {FileValidationConstants.MaxImageFileSizeBytes / 1024 / 1024}MB");

			RuleFor(x => x.FileName)
				.NotEmpty().WithMessage("Nome do arquivo da CNH é obrigatório")
				.Must(fileName => FileValidationConstants.AllowedImageExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
				.WithMessage($"Imagem da CNH deve estar nos formatos: {string.Join(", ", FileValidationConstants.AllowedImageExtensions)}");

			RuleFor(x => x.ContentType)
				.NotEmpty().WithMessage("Content-Type da imagem é obrigatório")
				.Must(ct => FileValidationConstants.AllowedImageContentTypes.Contains(ct.ToLowerInvariant()))
				.WithMessage($"Imagem da CNH deve ter Content-Type: {string.Join(", ", FileValidationConstants.AllowedImageContentTypes)}");
		}

		private bool IsValidCnpj(string? cnpj)
		{
			if (string.IsNullOrWhiteSpace(cnpj))
				return false;

			cnpj = Regex.Replace(cnpj, "[^0-9]", "");
			if (cnpj.Length != 14)
				return false;

			int[] multiplicator1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicator2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCnpj, digit;
			int sum, remainder;

			tempCnpj = cnpj.Substring(0, 12);
			sum = 0;

			for (int i = 0; i < 12; i++)
				sum += int.Parse(tempCnpj[i].ToString()) * multiplicator1[i];

			remainder = (sum % 11);
			if (remainder < 2)
				remainder = 0;
			else
				remainder = 11 - remainder;

			digit = remainder.ToString();
			tempCnpj += digit;
			sum = 0;

			for (int i = 0; i < 13; i++)
				sum += int.Parse(tempCnpj[i].ToString()) * multiplicator2[i];

			remainder = (sum % 11);
			if (remainder < 2)
				remainder = 0;
			else
				remainder = 11 - remainder;

			digit += remainder.ToString();
			return cnpj.EndsWith(digit);
		}

		private bool BeAtLeast18YearsOld(DateTime birthDate)
		{
			var today = DateTime.UtcNow.Date;
			int age = today.Year - birthDate.Year;
			if (birthDate.Date > today.AddYears(-age)) age--;
			return age >= 18;
		}
	}
}
