using Microsoft.AspNetCore.Http;

namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record UploadFileRequest(
    string Title,
    IFormFileCollection Files);

