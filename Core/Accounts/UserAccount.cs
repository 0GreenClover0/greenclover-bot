using System;
using System.Collections.Generic;
using System.Text;

namespace GreenClover.Core.Accounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public ulong Points { get; set; }

        public ulong XP { get; set; }

        public string Description { get; set; }
    }
}