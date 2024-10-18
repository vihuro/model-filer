using ModelFilter.Domain.Utils.Filters;

namespace ModelFilter.Domain.Interface
{
    public interface IFilterInterpreterFactory
    {
        IFilterTypeInterpreter<TType> Create<TType>(Filter filtroItem);
    }
}
