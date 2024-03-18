using OperationStacked.Entities;

namespace OperationStacked.Response;

public record ActiveSessionResponse(bool hasActiveSessio, Session session = null);