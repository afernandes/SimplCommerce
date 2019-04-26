using System;
using System.Linq;
using SimplCommerce.Module.ProductComparison.Models;
using SimplCommerce.Domain.Repositories;

namespace SimplCommerce.Module.ProductComparison.Services
{
    public class ComparingProductService : IComparingProductService
    {
        private readonly IRepository<ComparingProduct> _comparingProductRepository;
        private readonly int MaxNumComparingProduct = 4;

        public ComparingProductService(IRepository<ComparingProduct> productComparisonRepository)
        {
            _comparingProductRepository = productComparisonRepository;
        }

        public void AddToComparison(long userId, long productId)
        {
            var numComparingProduct = _comparingProductRepository.GetAll().Where(x => x.UserId == userId).Count();
            if(numComparingProduct >= MaxNumComparingProduct)
            {
                throw new TooManyComparingProductException(MaxNumComparingProduct);
            }

            var isProductExisted = _comparingProductRepository.GetAll().Any(x => x.ProductId == productId && x.UserId == userId);
            if (!isProductExisted)
            {
                var comparingProduct = new ComparingProduct
                {
                    UserId = userId,
                    ProductId = productId,
                    CreatedOn = DateTimeOffset.Now
                };

                _comparingProductRepository.Insert(comparingProduct);
                _comparingProductRepository.SaveChanges();
            }
        }

        public void MigrateComparingProduct(long fromUserId, long toUserId)
        {
            var comparingProducts = _comparingProductRepository.GetAll().Where(x => x.UserId == fromUserId);
            foreach(var item in comparingProducts)
            {
                item.UserId = toUserId;
            }

            _comparingProductRepository.SaveChanges();
        }
    }
}
