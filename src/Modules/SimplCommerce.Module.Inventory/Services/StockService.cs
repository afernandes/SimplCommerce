﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Catalog.Models;
using SimplCommerce.Module.Inventory.Models;

namespace SimplCommerce.Module.Inventory.Services
{
    public class StockService : IStockService
    {
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<StockHistory> _stockHistoryRepository;

        public StockService(IRepository<Stock> stockRepository, IRepository<Product> productRepository, IRepository<StockHistory> stockHistoryRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
            _stockHistoryRepository = stockHistoryRepository;
        }

        public async Task AddAllProduct(Warehouse warehouse)
        {
            var stocks = await _productRepository.GetAll().Where(x => !x.HasOptions && x.VendorId == warehouse.VendorId)
                .GroupJoin(_stockRepository.GetAll().Where(x => x.WarehouseId == warehouse.Id),
                    product => product.Id, stock => stock.ProductId,
                    (product, stockCollection) => new {IsNew = !stockCollection.Any(), ProductId = product.Id})
                .Where(x => x.IsNew)
                .Select(x => new Stock
                {
                    ProductId = x.ProductId,
                    WarehouseId = warehouse.Id,
                    Quantity = 0
                }).ToArrayAsync();
            _stockRepository.Insert(stocks);
            await _stockRepository.SaveChangesAsync();
        }

        public async Task UpdateStock(StockUpdateRequest stockUpdateRequest)
        {
            var product = await _productRepository.GetAll().FirstOrDefaultAsync(x => x.Id == stockUpdateRequest.ProductId);
            var stock = await _stockRepository.GetAll().FirstOrDefaultAsync(x => x.ProductId == stockUpdateRequest.ProductId && x.WarehouseId == stockUpdateRequest.WarehouseId);

            stock.Quantity = stock.Quantity + stockUpdateRequest.AdjustedQuantity;
            product.StockQuantity = product.StockQuantity + stockUpdateRequest.AdjustedQuantity;
            var stockHistory = new StockHistory
            {
                ProductId = stockUpdateRequest.ProductId,
                WarehouseId = stockUpdateRequest.WarehouseId,
                AdjustedQuantity = stockUpdateRequest.AdjustedQuantity,
                Note = stockUpdateRequest.Note,
                CreatedById = stockUpdateRequest.UserId,
                CreatedOn = DateTimeOffset.Now,
            };

            _stockHistoryRepository.Insert(stockHistory);
            await _stockHistoryRepository.SaveChangesAsync();
        }
    }
}
