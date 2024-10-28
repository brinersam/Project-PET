using FluentValidation;
using ProjectPet.Application.Validation;
using ProjectPet.Domain.Shared;

namespace ProjectPet.API.Contracts.FileManagement;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(u => u.Title)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull);

        RuleFor(u => u.Files)
            .NotEmpty()
            .WithError(Errors.General.ValueIsEmptyOrNull);
    }
}
