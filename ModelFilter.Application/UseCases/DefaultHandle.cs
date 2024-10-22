using MediatR;
using ModelFilter.Domain.Interface;

namespace ModelFilter.Application.UseCases
{
    public abstract class DefaultHandle
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        protected DefaultHandle(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        protected async Task Commit(CancellationToken cancellationToken)
        {
            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
