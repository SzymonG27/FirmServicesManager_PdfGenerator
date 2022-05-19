using PDFGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
