namespace ApplicationServices.PaymentsManagement.RemoteBanks;

public record BankOfUkWithdrawModel(
    string CardNumber,
    int CvvCode,
    int ExpiresMonth,
    int ExpiresYear,
    decimal AmountNum,
    string AmountCurrency);