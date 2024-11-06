﻿using FluentValidation;
using ProjectPet.Application.Extensions;
using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerPayment;

public class UpdateVolunteerPaymentRequestValidator : AbstractValidator<UpdateVolunteerPaymentRequestDto>
{
    public UpdateVolunteerPaymentRequestValidator()
    {
        RuleForEach(c => c.PaymentInfos)
            .ValidateValueObj(x => PaymentInfo.Create(x.Title, x.Instructions));
    }
}
