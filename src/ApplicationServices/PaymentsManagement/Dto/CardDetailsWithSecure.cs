namespace ApplicationServices.PaymentsManagement.Dto;

public record CardDetailsWithSecure(string Number, int ExpireMonth, int ExpireYear, int CvvCode) : CardDetails(
    Number,
    ExpireMonth,
    ExpireYear);