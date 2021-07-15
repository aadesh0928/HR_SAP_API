using MongoDB.Driver;
using Nedbank.HR.SAP.DAL.Interfaces;
using Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        public ProductRepository(IMongoConnector<Product> connector)
        {
            _products = connector.Collection;

        }
        public async Task<List<Product>> FetchProductsAsync(Expression<Func<Product, bool>> expression)
        {
            return await _products.Find<Product>(expression).ToListAsync();
        }

        public async Task<List<Product>> FetchProductsAsync(FilterDefinition<Product> filter)
        {
            
            return await _products.Find<Product>(filter).ToListAsync();
        }
    }
}
