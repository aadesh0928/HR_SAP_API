using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue
{
    [BsonIgnoreExtraElements]
    public class CurrencyAttribute
    {
        [BsonElement("attributeType")]
        public string AttributeType { get; set; }
        [BsonElement("currency")]
        public string Currency { get; set; }
        [BsonElement("pricingType")]
        public string PricingType { get; set; }
        [BsonElement("attributeName")]
        public string AttributeName { get; set; }
        [BsonElement("value")]
        public decimal Value { get; set; }
    }
}
