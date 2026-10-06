# SmartSupport AI

A customer support assistant built with ASP.NET Core 10, Blazor Interactive Server, Microsoft Agent Framework and OpenAI. This repository contains the application built in the SmartSupport AI code walkthrough.

## What the application demonstrates

- Look up an order using `GetOrderStatus`.
- Check cancellation eligibility using `CanCancelOrder`.
- Create a support ticket using `CreateSupportTicket`.
- Maintain conversation context for follow-up questions such as “Can I cancel it?”
- Show executed C# tools and support tickets alongside the conversation.
- Start a new conversation and handle missing orders and request failures.

The model interprets requests and phrases responses. C# methods supply order data, enforce cancellation rules and create ticket records. Checking cancellation eligibility does not cancel an order.

## Requirements

- Visual Studio 2026 with the ASP.NET and web development workload, or another compatible IDE.
- .NET 10 SDK.
- An OpenAI API key with active API billing and access to the configured model.

## Run the completed application

1. Clone this repository and open its solution file in Visual Studio.
2. Restore NuGet packages and select the web application as the startup project.
3. Right-click the web project and select **Manage User Secrets**.
4. Add your API key to the local secrets file:

```json
{
  "AI": {
    "ApiKey": "YOUR_OPENAI_API_KEY"
  }
}
```

5. Check the AI settings in `appsettings.json`:

```json
"AI": {
  "Model": "gpt-4.1-mini",
  "Endpoint": ""
}
```

A blank endpoint uses the OpenAI SDK default endpoint. Keep your actual API key in User Secrets. Do not commit it to the repository.

6. Build and run the application. Open the local URL displayed by Visual Studio.

## Try the demonstration

Send these messages in sequence:

1. `Where is order ORD-1003?`
2. `Can I cancel it?`
3. `Please create a support ticket for the delayed delivery.`

Watch the tool activity and ticket panels. Sample orders include `ORD-1001`, `ORD-1002` and `ORD-1003`.

## Starter project

Download `SmartSupportAI-Starter.zip` from this repository's **Releases** page when the tutorial resources release is available. Use it to follow the walkthrough. The repository source contains the completed application.

## Main application files

| File | Responsibility |
| --- | --- |
| `SupportStore.cs` | Sample orders, tickets and tool activity |
| `OrderTools.cs` | C# functions available to the agent |
| `SupportAgentService.cs` | Agent configuration, conversation session and request handling |
| `Program.cs` | Service registration and application setup |
| `Components/Pages/Home.razor` | Chat interface |
| `wwwroot/app.css` | Custom application styling |

## API costs

Live conversations incur OpenAI API usage charges. API billing is separate from a ChatGPT subscription. The demo video shows one observed session's usage; your cost depends on the model and the requests you make. Check your account's current pricing, billing and usage before experimenting.

## Demonstration limitations

This is an educational application with fictional, in-memory order data. Tickets belong to the current browser circuit and are not stored in a database. Starting a new conversation clears agent conversation context while retaining tickets in that circuit.

Ticket duplicate prevention compares the same order and issue text after trimming surrounding spaces and ignoring case. Rephrasing an issue can create another ticket.

The agent prompt requests explicit user intent before ticket creation. This is guidance to the model; the application does not independently enforce a user-confirmation step. Authentication, order ownership checks, durable storage and enforced approval would require additional implementation.

## Video series

The **SmartSupport AI** playlist includes the finished-application demo and the code walkthrough. Video links will be added here after publication.
