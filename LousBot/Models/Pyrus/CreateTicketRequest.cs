namespace LousBot.Models.Pyrus;

public record CreateTicketRequest(
    string Email,
    int ServiceName,
    string Justification,
    string? AnotherServiceName,
    string DirectId);