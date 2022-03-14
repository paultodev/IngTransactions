using DAL;
using DAL.Models;
using DAL.Repository;
using IngTransactions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace IngTransactions
{
   
    public class Startup
    {
        X509Certificate2 signingCertificate;
        X509Certificate2 tlsCertificate;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            signingCertificate = new X509Certificate2(
                configuration.GetValue<string>("Certificates:signing:Path"),
                configuration.GetValue<string>("Certificates:signing:Password"));

            tlsCertificate = new X509Certificate2(
                configuration.GetValue<string>("Certificates:tls:Path"),
                configuration.GetValue<string>("Certificates:tls:Password"));

            

            using (var db = new SqliteDbContext())
            {
                db.Database.EnsureCreated();
                db.Account.Add(new Account()
                {
                    Currency = "RON",
                    Iban = "NL69INGB0123456789",
                    Product = "Betaalrekening",
                    ResourceId = "450ffbb8-9f11-4ec6-a1e1-df48aefc82ef",
                    Name = "Hr A van Dijk , Mw B Mol-van Dijk",

                });
                db.Transactions.AddRange(
                    new List<Transactions>()
                    {
                        new Transactions()
                        {
                            Amount = 1020.20,
                            Iban = "NL69INGB0123456789",
                            CategoryId = Categorylist.Food,
                            TransactionDate = new DateTime(2020, 09, 23),
                            AccountForeignKey = 1
                        },
                        new Transactions()
                        {
                            Amount = 2050.00,
                            Iban = "NL69INGB0123456789",
                            CategoryId = Categorylist.Entertainment,
                            TransactionDate = new DateTime(2020, 09, 23),
                            AccountForeignKey = 1,
                        }
                    }); ;
                db.SaveChanges();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<SqliteDbContext>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITransactionsRepository, TransactionsRepository>();
            services.AddHttpClient<IClientService, ClientService>(sp=>new ClientService(signingCertificate, tlsCertificate));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IngTransactions", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IngTransactions v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
