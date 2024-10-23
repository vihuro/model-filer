using MediatR;
using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.Behavior
{
    public sealed class ValidationBehavior<TRequest, TResponse> :
                                      IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICustomNotification _notifications;

        public ValidationBehavior(ICustomNotification notifications)
        {
            _notifications = notifications;
        }

        public async Task<TResponse> Handle(TRequest request,
                                      RequestHandlerDelegate<TResponse> next,
                                      CancellationToken cancellationToken)
        {

            if (!_notifications.HaveNotification()) return await next();

            var response = new ReturnDefault<object>
            {
                Erros = _notifications.GetNotifications().Select(x => x.Message).ToList(),
                Sucess = false
            };

            // Retorna a resposta com erros
            return (TResponse)(object)response; // Cast para TResponse

        }
    }
}
