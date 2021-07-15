using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue
{
    [BsonIgnoreExtraElements]
    public class ValueAddPricing
    {
        [BsonElement("currency")]
        public string Currency { get; set; }
        [BsonElement("pricingType")]
        public string PricingType { get; set; }
        [BsonElement("value")]
        public int? Value { get; set; }
    }
}
