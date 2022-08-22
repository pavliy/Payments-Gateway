using Swashbuckle.AspNetCore.Annotations;

namespace ApplicationServices.PaymentsManagement.Dto;

public record CardDetailsWithSecure(
    string Number,
    int ExpireMonth,
    int ExpireYear,
    [property: SwaggerSchema(
        Description = "Cvv code to use for selected card",
        Format = "integer",
        Nullable = false)
    ]
    int CvvCode) : CardDetails(
    Number,
    ExpireMonth,
    ExpireYear);