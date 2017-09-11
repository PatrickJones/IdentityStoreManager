using System;
using System.Collections.Generic;

namespace NuUserStoreManager.EF
{
    public partial class SharedAreas
    {
        public int DataShareCategoryId { get; set; }
        public int RequestId { get; set; }
        public int ShareId { get; set; }

        public DataShareRequestLog Request { get; set; }
    }
}
