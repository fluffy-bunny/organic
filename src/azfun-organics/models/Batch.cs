using System;
using System.Collections.Generic;

namespace azfun_organics.models
{
    public class Batch
    {
        public string id { get; set; }
        public string BatchId { get; set; }
        public List<string> Files { get; set; }
        public DateTime? LastUpdate { get; set; }
    }

}
