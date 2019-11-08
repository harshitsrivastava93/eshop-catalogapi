﻿using CatalogAPI.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace CatalogAPI.Infrastructure
{
    public class CatalogContext
    {
        private IConfiguration configuration;
        //var 
        private IMongoDatabase database;
      //  private object enabledsslprotocols;

        public CatalogContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            var connectionString = configuration.GetValue<string>("MongoSettings:ConnectionString");
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
                //MongoClientSettings.FromConnectionString(connectionString);
            MongoClient client = new MongoClient(settings);
            if (client != null)
            {
                this.database = client.GetDatabase(configuration.GetValue<string>("MongoSettings:Database"));
            }
        }

        public IMongoCollection<CatalogItem> Catalog
        {
            get 
            { 
                return this.database.GetCollection<CatalogItem>("products"); 
            }
        }
    }
}
