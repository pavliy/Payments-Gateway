using System.Diagnostics.CodeAnalysis;

using ApplicationServices.PaymentsManagement.Dto;

using AutoMapper;

using Domain.Payments;

namespace ApplicationServices.PaymentsManagement.Mapping;

// ReSharper disable once UnusedType.Global
[ExcludeFromCodeCoverage]
public class PaymentMapProfile : Profile
{
    public PaymentMapProfile()
    {
        this.CreateMap<Payment, PaymentDetails>();
        this.CreateMap<Card, CardDetails>();
        this.CreateMap<Expense, ExpenseDetails>();
    }
}