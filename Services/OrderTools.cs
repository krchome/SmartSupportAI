using System.ComponentModel;

namespace SmartSupportAI.Services;

public sealed class OrderTools(SupportStore store, ILogger<OrderTools> logger)
{
    [Description("Look up authoritative order status and delivery estimate. Returns Found=false for unknown orders. Never guess missing order details.")]
    public object GetOrderStatus([Description("Order reference such as ORD-1003")] string orderNumber)
    {
        Trace(nameof(GetOrderStatus), orderNumber);
        var order = store.FindOrder(orderNumber);
        return order is null ? new { Found = false, Message = "Order not found" } :
            (object)new { Found = true, order.OrderNumber, order.Status, order.DeliveryEstimate };
    }
    [Description("Check whether an order is eligible for cancellation. Read only: this never cancels or changes an order.")]
    public object CanCancelOrder([Description("Order reference to check")] string orderNumber)
    {
        Trace(nameof(CanCancelOrder), orderNumber);
        var order = store.FindOrder(orderNumber);
        return order is null ? new { Found = false, Message = "Order not found" } :
            (object)new
            {
                Found = true,
                order.OrderNumber,
                order.CanCancel,
                Reason = order.CanCancel ? "Still processing" : "Already dispatched or delivered"
            };
    }
    [Description("Create a demonstration support ticket for an existing order only when the user explicitly requests a ticket. Requires a clear issue. Returns a real in-memory ticket number; no external service is contacted.")]
    public object CreateSupportTicket(
        [Description("Existing order reference")] string orderNumber,
        [Description("Brief description of the issue the user wants reported")] string issue)
    {
        Trace(nameof(CreateSupportTicket), orderNumber);
        var order = store.FindOrder(orderNumber);
        if (order is null) return new { Created = false, Message = "Order not found. No ticket created." };
        if (string.IsNullOrWhiteSpace(issue) || issue.Trim().Length > 500)
            return new { Created = false, Message = "Provide an issue between 1 and 500 characters." };
        var ticket = store.CreateTicket(order, issue);
        return new { Created = true, ticket.TicketNumber, ticket.OrderNumber, ticket.Issue };
    }
    private void Trace(string tool, string orderNumber)
    {
        store.Record(tool, orderNumber);
        logger.LogInformation("C# tool executed: {Tool} for {Order}", tool, orderNumber);
    }

}