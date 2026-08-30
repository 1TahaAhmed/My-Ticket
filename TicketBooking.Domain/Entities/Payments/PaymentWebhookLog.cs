using System;
using TicketBooking.Domain.BaseEntity;
        
namespace TicketBooking.Domain.Entities.Payments
{        
    public class PaymentWebhookLog : BaseEntity<Guid>
    {   
        public string Provider { get; private set; } = string.Empty;
        public string EventType { get; private set; } = string.Empty;
        public string PayloadJson { get; private set; } = string.Empty;
        public string RequestHeadersJson { get; private set; } = string.Empty;
        public bool IsProcessed { get; private set; }
        public DateTime ReceivedAtUtc { get; private set; }
        public DateTime? ProcessedAtUtc { get; private set; }
        public string ProcessingError { get; private set; } = string.Empty;

        private PaymentWebhookLog() { }

        public PaymentWebhookLog(string provider, string eventType, string payloadJson, string requestHeadersJson = "")
        {
            if (string.IsNullOrWhiteSpace(provider))
                throw new ArgumentException("Provider is required.", nameof(provider));

            if (string.IsNullOrWhiteSpace(payloadJson))
                throw new ArgumentException("Payload cannot be empty.", nameof(payloadJson));

            Provider = provider.Trim();
            EventType = eventType?.Trim() ?? string.Empty;
            PayloadJson = payloadJson;
            RequestHeadersJson = requestHeadersJson;
            IsProcessed = false;
            ReceivedAtUtc = DateTime.UtcNow;
        }

        public void MarkAsProcessed()
        {
            if (IsProcessed)
                throw new InvalidOperationException("Webhook payload has already been processed.");

            IsProcessed = true;
            ProcessedAtUtc = DateTime.UtcNow;
            ProcessingError = string.Empty;
        }

        public void MarkAsFailed(string errorMessage)
        {
            ProcessingError = string.IsNullOrWhiteSpace(errorMessage) ? "Unknown error during webhook processing" : errorMessage.Trim();
        }
    }   
}       