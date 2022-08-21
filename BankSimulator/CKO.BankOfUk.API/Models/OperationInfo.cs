namespace CKO.BankOfUk.API.Models;

public class OperationInfo
{
    public string? CardNumber { get; set; }
    
    public int CvvCode { get; set; }
    
    public int ExpiresMonth { get; set; }
    
    public int ExpiresYear { get; set; }
    
    public decimal AmountNum { get; set; }
    
    public string? AmountCurrency { get; set; }
}