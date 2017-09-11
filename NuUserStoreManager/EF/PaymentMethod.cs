using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class PaymentMethod
    {
        public int MethodId { get; set; }
        public string MethodName { get; set; }
    }
}
