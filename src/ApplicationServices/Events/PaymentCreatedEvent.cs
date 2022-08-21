namespace ApplicationServices.Events;

public class PaymentCreatedEvent : IntegrationEvent
{
    public PaymentCreatedEvent(Guid paymentId, int cvvCode)
    {
        this.PaymentId = paymentId;
        this.CvvCode = cvvCode;
    }

    public Guid PaymentId { get; }

    public int CvvCode { get; }
}