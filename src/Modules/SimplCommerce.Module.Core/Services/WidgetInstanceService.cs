﻿using System;
using System.Linq;
using SimplCommerce.Domain.Repositories;
using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.Core.Services
{
    public class WidgetInstanceService : IWidgetInstanceService
    {
        private IRepository<WidgetInstance> _widgetInstanceRepository;

        public WidgetInstanceService(IRepository<WidgetInstance> widgetInstanceRepository)
        {
            _widgetInstanceRepository = widgetInstanceRepository;
        }

        public IQueryable<WidgetInstance> GetPublished()
        {
            return _widgetInstanceRepository.GetAll().Where(x =>
                x.PublishStart.HasValue && x.PublishStart < DateTimeOffset.Now
                && (!x.PublishEnd.HasValue || x.PublishEnd > DateTimeOffset.Now));
        }
    }
}
