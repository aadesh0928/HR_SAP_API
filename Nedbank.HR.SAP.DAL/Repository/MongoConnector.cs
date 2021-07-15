using MongoDB.Driver;
using Nedbank.HR.SAP.DAL.Interfaces;
using Nedbank.HR.SAP.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nedbank.HR.SAP.DAL.Repository
{
    public class MongoConnector<T> : IMongoConnector<T> where T:class
    {
        public MongoConnector(IDatabaseSettings settings)
        {

            /* ACP : Need to remove if settings are not used for connecting, and 
                rather use conectionstrnig */
            //var mongoClientSettings = new MongoClientSettings
            //{
            //    Server = MongoServerAddress.Parse(settings.Server),
            //    Credential = MongoCredential.CreateCredential(settings.DatabaseName, settings.Credential.Username, settings.Credential.Password),
            //    SslSettings = new SslSettings
            //    {
            //        CheckCertificateRevocation = false
            //    }
            //};
            //var client = new MongoClient(mongoClientSettings);

            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
            Collection = Database.GetCollection<T>(settings.CollectionName);
        }
        public IMongoDatabase Database { get; set; }

        public IMongoCollection<T> Collection { get; set; } 
    }
}
