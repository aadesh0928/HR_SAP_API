using MongoDB.Driver;
using Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> FetchProductsAsync(Expression<Func<Product, bool>> expression);
        Task<List<Product>> FetchProductsAsync(FilterDefinition<Product> filter);
    }
}
