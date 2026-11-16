using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Domain.Processing;

public record class ProcessingResult(
    SessionContext Session,
    ProcessingContext? Context
);
