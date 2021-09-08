﻿using System;
using System.Collections.Generic;
using Kustomaur.Dashboard.Implementation.DashboardMetadataModelBuilders;
using Kustomaur.Models;

namespace Kustomaur.Dashboard
{
    public class DashboardBuilder
    {
        public string SubscriptionId { get; private set; }
        public string ResourceGroup { get; private set; }
        public string Name { get; private set; }

        public Models.Dashboard Dashboard { get; }


        private IDashboardMetadataModelBuilder _timeRangeBuilder;

        private IDashboardMetadataModelBuilder _timeRangeFilterBuilder;

        private readonly List<IBaseBuilder> _builders;
        
        private string _filterLocale = "en-us";
        
        public DashboardBuilder()
        {
            _timeRangeBuilder = new TimeRangeBuilder();
            _builders = new List<IBaseBuilder>();
            Dashboard = new Models.Dashboard();
            InitialiseDashboardPropertiesMetadataModel();

        }
        public DashboardBuilder WithSubscription(string subscriptionId)
        {
            SubscriptionId = subscriptionId;
            return this;
        }
        
        public DashboardBuilder WithResourceGroup(string resourceGroup)
        {
            ResourceGroup = resourceGroup;
            return this;
        }
        
        public DashboardBuilder WithName(string name)
        {
            Name = name;
            if (Dashboard.Tags == null)
            {
                Dashboard.Tags = new Dictionary<string, string>();
            }
            Dashboard.Tags.Add("hidden-title", name);
            return this;
        }

        public DashboardBuilder WithTimeRangeBuilder(TimeRangeBuilder builder)
        {
            _timeRangeBuilder = builder;
            return this;
        }
        
        public DashboardBuilder WithBuilder(IBaseBuilder builder)
        {
            _builders.Add(builder);
            return this;
        }
        
        public DashboardBuilder WithFilterLocale(string locale)
        {
            _filterLocale = locale;
            return this;
        }
        
        private void InitialiseDashboardPropertiesMetadataModel()
        {
            if (Dashboard.Properties == null)
            {
                Dashboard.Properties = new DashboardProperties();
            }

            if (Dashboard.Properties.Metadata == null)
            {
                Dashboard.Properties.Metadata = new PropertiesMetadata();
            }

            if (Dashboard.Properties.Metadata.Model == null)
            {
                Dashboard.Properties.Metadata.Model = new Dictionary<string, DashboardPropertiesMetadataModel>();
            }
        }

        public Models.Dashboard Build()
        {
            Dashboard.Id = BuildId();
            Dashboard.Type = "Microsoft.Portal/dashboards";
            Dashboard.Name = Name;

            // Run each builder
            CombineAndRunBuilders();
            
            //Set filter locale
            Dashboard.Properties.Metadata.Model.Add("filterLocale", new DashboardPropertiesMetadataModel() { Value = _filterLocale });
            
            // Set Filters

            return Dashboard;
        }

        private void CombineAndRunBuilders()
        {
            var builders = new List<IBaseBuilder>();
            builders.AddRange(_builders);
            builders.Add(_timeRangeBuilder);
            builders.ForEach(b => b.Build(Dashboard));
        }

        private string BuildId()
        {
            return
                $"/subscriptions/{SubscriptionId}/resourceGroups/dashboards/providers/Microsoft.Portal/dashboards/{Name}";
        }
    }
}