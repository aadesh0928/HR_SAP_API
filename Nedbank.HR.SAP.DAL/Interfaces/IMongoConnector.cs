using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.DAL.Interfaces
{
    public interface IMongoConnector<T> where T:class
    {
        IMongoDatabase Database { get; set; }
        IMongoCollection<T> Collection { get; set; }
    }
}
