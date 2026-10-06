namespace SmartSupportAI.Services;

public record CustomerOrder(string OrderNumber, string Status, string DeliveryEstimate, bool CanCancel);
public record SupportTicket(string TicketNumber, string OrderNumber, string Issue);
public record ToolActivity(string Tool, string Detail);

public sealed class SupportStore
{
    private readonly Dictionary<string, CustomerOrder> orders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ORD-1001"] = new("ORD-1001", "Processing", "Within 3 business days", true),
        ["ORD-1002"] = new("ORD-1002", "Delivered", "Delivered yesterday", false),
        ["ORD-1003"] = new("ORD-1003", "Delayed in transit", "Within 2 business days", false)
    };
    private readonly List<SupportTicket> tickets = [];
    private readonly List<ToolActivity> activity = [];
    public IReadOnlyList<SupportTicket> Tickets => tickets.AsReadOnly();
    public IReadOnlyList<ToolActivity> Activity => activity.AsReadOnly();
    public CustomerOrder? FindOrder(string? number) =>
        orders.GetValueOrDefault((number ?? "").Trim());
    public void Record(string tool, string detail) => activity.Add(new(tool, detail));
    public void ClearActivity() => activity.Clear();

    public SupportTicket CreateTicket(CustomerOrder order, string issue)
    {
        issue = issue.Trim();
        // Repeat calls for the same order and issue return the existing ticket.
        var existing = tickets.FirstOrDefault(t => t.OrderNumber == order.OrderNumber &&
            string.Equals(t.Issue, issue, StringComparison.OrdinalIgnoreCase));
        if (existing is not null) return existing;
        var ticket = new SupportTicket($"TKT-{tickets.Count + 1:0000}", order.OrderNumber, issue);
        tickets.Add(ticket);
        return ticket;
    }
}