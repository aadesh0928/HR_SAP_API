using Nedbank.HR.SAP.BAL.Interfaces;
using Nedbank.HR.SAP.DAL.Interfaces;
using Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace Nedbank.HR.SAP.BAL.Business
{
    public class ProductBusiness: IProductBusiness
    {

        private readonly IProductRepository _productRepository;

        private readonly IExpressionFilter<Product> _expressionFilter;
        public ProductBusiness(IProductRepository productRepository, IExpressionFilter<Product> expressionFilter)
        {
            _productRepository = productRepository;
            _expressionFilter = expressionFilter;
        }

        public async Task<List<Product>> FetchProductsAsync()
        {
            int? productFamily = default(int?);
            var productFeatureAttributeName = "secur";
            var productName = "";

            var builder = Builders<Product>.Filter;
            var defaultFilter = builder.Where(item => true);
            if (productFamily.HasValue)
            {
                defaultFilter = builder.Eq(product => product.ProductFamily, productFamily);

            }
            if (!string.IsNullOrEmpty(productFeatureAttributeName))
            {
                defaultFilter = defaultFilter & builder.ElemMatch(product =>
                    product.ProductFeatures,
                    attr => attr.AttributeName.ToLowerInvariant().Contains(productFeatureAttributeName.ToLower()));
            }
            if (!string.IsNullOrEmpty(productName))
            {
                defaultFilter = builder.Eq(product => product.ProductName, productName);
            }

            


             

            // var expression = _expressionFilter.BuildExpressionForNestedType("ProductFeatures.AttributeName", "helo");

            //Expression AndAlso = Expression.Constant(true);
            //var productFamily = "20";
            //var productFeatureAttributeName = "Feature1";

            try
            {
                //if (!string.IsNullOrEmpty(productFamily))
                //{
                //    var parameter = Expression.Parameter(typeof(Product), "x");

                //    var member = Expression.Bind(Expression.PropertyOrField(parameter, "productFeatures").Member, parameter);
                //    var member2 = Expression.Bind(Expression.PropertyOrField(parameter, "attributeType").Member, Expression.PropertyOrField(parameter, "productFeatures"));
                //    var bindings = new List<MemberBinding>();
                //    bindings.Add(member);
                //    bindings.Add(member2);


                //    var rrr = Expression.MemberInit(Expression.New(typeof(Product)), bindings);

                //    var bulilder = Builders<Product>.Filter;
                //    var filter = bulilder.ElemMatch(item => item.ProductFeatures, fet => fet.AttributeName == "aas");
                //    var filter2 = bulilder.Eq(item => item.ProductFamily, int.Parse(productFamily));
                //    var ff = bulilder.And(filter, filter2);


                //    //var member = Expression.Property(parameter, "productFeatures.attributeType");
                //    //var body = Expression.Equal(member, Expression.Constant("aasa"));
                //    //var final = Expression.Lambda<Func<Product, bool>>(body, parameter);
                //}
                return await _productRepository.FetchProductsAsync(defaultFilter);
            }
            catch (Exception ex)
            {

                throw;
            }

            // if()

           
            //return await _productRepository.FetchProductsAsync(expression);
        }
    }
}
