using System.Collections;

namespace WordProgress.Edument
{
    public interface IHandleCommand<TCommand>
    {
        IEnumerable Handle(TCommand c);
    }
}