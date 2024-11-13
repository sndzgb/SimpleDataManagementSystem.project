using SimpleDataManagementSystem.Backend.Logic.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendAsync(Email email);
        void Send(Email email);
    }
}
