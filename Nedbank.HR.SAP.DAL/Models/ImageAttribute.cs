using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue
{
    [BsonIgnoreExtraElements]
    public class ImageAttribute
    {
        [BsonElement("attributeType")]
        public string AttributeType { get; set; }
        [BsonElement("attributeName")]
        public string AttributeName { get; set; }
        [BsonElement("isComparable")]
        
        public bool IsComparable { get; set; }
        [BsonElement("image")]
        public string Image { get; set; }
        [BsonElement("value")]
        public string Value { get; set; }
    }
}
