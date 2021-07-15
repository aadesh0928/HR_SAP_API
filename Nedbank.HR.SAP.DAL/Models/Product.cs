using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonElement("productIdentifier")]
        public int ProductIdentifier { get; set; }

        [BsonElement("productFamily")]
        public int ProductFamily { get; set; }
        [BsonElement("productName")]
        public string ProductName { get; set; }
        [BsonElement("displayName")]
        public string DisplayName { get; set; }
        [BsonElement("displayNameSuffix")]
        public string DisplayNameSuffix { get; set; }
        [BsonElement("productPricing")]
        public CurrencyAttribute ProductPricing { get; set; }
        [BsonElement("shortDescription")]
        public string ShortDescription { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("salesHighlights")]
        public List<string> SalesHighlights { get; set; }
        [BsonElement("productOnAPageWebURL")]
        public string ProductOnAPageWebURL { get; set; }
        [BsonElement("productFeatures")]
        public List<ValueAttribute> ProductFeatures { get; set; }
        [BsonElement("valueAddedServices")]
        public List<ValueAddedAttribute> ValueAddedServices { get; set; }
        [BsonElement("eligibilityRules")]
        public List<ValueAttribute> EligibilityRules { get; set; }
        [BsonElement("productCompare")]
        public ProductCompare ProductCompare { get; set; }
        [BsonElement("productInfoLinks")]
        public List<ValueUnitAttribute> ProductInfoLinks { get; set; }
        [BsonElement("optionalExtras")]
        public List<ImageAttribute> OptionalExtras { get; set; }
    }
}
