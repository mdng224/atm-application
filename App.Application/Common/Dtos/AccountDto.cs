namespace App.Application.Common.Dtos;

public sealed record class AccountDto(Guid Id, string Name, decimal Balance);