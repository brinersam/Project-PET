using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProjectPet.API.Contracts.FileManagement
{
    public record UploadFileDto(
        [ValidateNever] string Title,
        [ValidateNever] IFormFileCollection Files);

    //we use [ValidateNever] to skip .net validation so we can rely on fluentvalidation
}

