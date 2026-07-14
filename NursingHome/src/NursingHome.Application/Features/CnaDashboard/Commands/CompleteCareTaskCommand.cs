using MediatR;

namespace NursingHome.Application.Features.CnaDashboard.Commands;

public record CompleteCareTaskCommand(long TaskId, long CnaUserId) : IRequest<Unit>;
