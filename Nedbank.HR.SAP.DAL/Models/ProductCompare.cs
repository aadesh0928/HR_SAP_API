using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue
{
    [BsonIgnoreExtraElements]
    public class ProductCompare
    {
        [BsonElement("monthlyFee")]
        public FeeAttribute MonthlyFee { get; set; }
        [BsonElement("userType")]
        public ValueUnitsAttribute UserType { get; set; }
        [BsonElement("transactions")]
        public ValueUnitsAttribute Transactions { get; set; }
        [BsonElement("chequeCard")]
        public ValueUnitsAttribute ChequeCard { get; set; }
        [BsonElement("valueAdds")]
        public ValueUnitsAttribute ValueAdds { get; set; }
        [BsonElement("addOns")]
        public ValueUnitsAttribute AddOns { get; set; }
    }
}
