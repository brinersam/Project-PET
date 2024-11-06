﻿namespace ProjectPet.Application.Dto;
public record HealthInfoDto(
    string Health,
    bool IsSterilized,
    bool IsVaccinated,
    float Weight,
    float Height);