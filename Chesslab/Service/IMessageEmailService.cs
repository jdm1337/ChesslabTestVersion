using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chesslab.Service
{
    public interface IMessageEmailService
    {
        Task SendMessage(string email, string subject, string message);
    }
}
