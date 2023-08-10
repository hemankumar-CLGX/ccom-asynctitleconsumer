using CCOM.AsyncTitleServiceConsumer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCOM.AsyncTitleServiceConsumer.Services
{
    public interface ITitleAsyncService
    {
        void Handle(AysncTitleProviderMessage aysncTitleProviderMessage);
    }
}
