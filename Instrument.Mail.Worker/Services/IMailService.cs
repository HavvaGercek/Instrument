using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument.Mail.Worker.Services
{
    public interface IMailService
    {
        void Send(string to, string subject, string html);
    }
}
