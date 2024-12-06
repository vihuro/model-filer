using AutoMapper;
using MediatR;
using ModelFilter.Application.Utils;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils;

namespace ModelFilter.Application.UseCases
{
    public abstract class DefaultHandle
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        protected ICustomNotification _notification;
        protected IMapper _mapper;

        protected DefaultHandle(IMediator mediator, IUnitOfWork unitOfWork, ICustomNotification notification, IMapper mapper)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _notification = notification;
            _mapper = mapper;
        }

        protected async Task Commit(CancellationToken cancellationToken)
        {
            await _unitOfWork.Commit(cancellationToken);
        }
        protected bool OperationIsValid()
        {
            return !_notification.HaveNotification();
        }
        protected void Notify(string message)
        {
            _notification.Handle(new Notification(message));
        }
        protected void EntityIsValid<T>(T entity)
        {
            if (!entity.IsValid())
            {
                var erros = entity.GetValidationErros();
                foreach(var item in erros)
                {
                    _notification.Handle(new Notification($"{item.MemberNames.FirstOrDefault()} - {item.ErrorMessage}"));
                }
            }
        }
    }
}
