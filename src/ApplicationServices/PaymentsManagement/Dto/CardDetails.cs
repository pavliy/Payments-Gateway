using Swashbuckle.AspNetCore.Annotations;

namespace ApplicationServices.PaymentsManagement.Dto;

public record CardDetails(
    [property: SwaggerSchema(
        Description = "Card number to use for payment",
        Format = "string",
        Nullable = false)
    ]
    string Number,
    [property: SwaggerSchema(
        Description = "Card number month expiration",
        Format = "integer",
        Nullable = false)
    ]
    int ExpireMonth,
    [property: SwaggerSchema(
        Description = "Card number year expiration",
        Format = "integer",
        Nullable = false)
    ]
    int ExpireYear);