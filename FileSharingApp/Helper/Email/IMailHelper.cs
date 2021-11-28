using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharingApp.Helper.Email
{
    public interface IMailHelper
    {
        void SendEmail(InputEmailMessage inputEmailMessage);
    }
}
