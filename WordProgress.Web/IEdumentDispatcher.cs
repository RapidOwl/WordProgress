using System;
using System.Collections.Generic;
using WordProgress.Edument;

namespace WordProgress.Web
{
    public interface IEdumentDispatcher
    {
        IMessageDispatcher Dispatcher { get; }
        void Setup(IEnumerable<Type> typesToScan);
    }
}