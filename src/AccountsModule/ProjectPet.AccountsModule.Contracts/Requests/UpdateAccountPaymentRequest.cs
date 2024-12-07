using ProjectPet.AccountsModule.Contracts.Dto;
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.AccountsModule.Contracts.Requests;

public record class UpdateAccountPaymentRequest(List<PaymentInfoDto> PaymentInfos);
