namespace ApplicationServices.PaymentsManagement.Dto;

public record PaymentDetails(CardDetails Card, ExpenseDetails Spent, PaymentDetailsStatus Status);