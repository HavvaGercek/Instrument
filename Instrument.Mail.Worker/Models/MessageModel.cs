using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument.Mail.Worker.Models
{
    public class MessageModel
    {
        public string Symbol { get; set; }
        public string Email { get; set; }
        public double Price { get; set; }
    }
}
