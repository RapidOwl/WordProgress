using System;
using System.Collections.Generic;
using WordProgress.Edument;

namespace WordProgress.Web
{
    public class EdumentDispatcher : IEdumentDispatcher
    {
        public IMessageDispatcher Dispatcher { get; private set; }

        public EdumentDispatcher(IMessageDispatcher messageDispatcher)
        {
            Dispatcher = messageDispatcher;
        }

        public void Setup(IEnumerable<Type> typesToScan)
        {
            // TODO Make this scan the whole of the assembly instead of taking the type parameters?
            foreach (var type in typesToScan)
            {
                Dispatcher.ScanInstance(Activator.CreateInstance(type));
            }
        }
    }
}