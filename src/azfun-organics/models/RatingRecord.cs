using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Text;

namespace azfun_organics.models
{
    public class ESDateTimeConverter : IsoDateTimeConverter
    {
        public ESDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-ddThh:mm:ssZ";
        }
    }

    /*
     {
      "userId": "cc20a6fb-a91f-4192-874d-132493685376",
      "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
      "locationName": "Sample ice cream shop",
      "rating": 5,
      "userNotes": "I love the subtle notes of orange in this ice cream!"
    }
     */
    public class RatingRecord
    {
        public string id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public string UserNotes { get; set; }
        public int Rating { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
