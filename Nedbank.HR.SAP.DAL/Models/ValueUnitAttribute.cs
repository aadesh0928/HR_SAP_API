using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue
{
    [BsonIgnoreExtraElements]
    public class ValueUnitAttribute
    {
        [BsonElement("attributeType")]
        public string AttributeType { get; set; }
        [BsonElement("attributeName")]
        public string AttributeName { get; set; }
        [BsonElement("valueUnit")]
        public string ValueUnit { get; set; }
        [BsonElement("isComparable")]
        public string IsComparable { get; set; }
        [BsonElement("value")]
        public string Value { get; set; }
    }
}
