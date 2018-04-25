using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Core
{
    public interface IHandler<T> where T : IMessage
    {
        void Handle(T message);
    }

}
