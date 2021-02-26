using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AspRouting.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspRoutings.Methods
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFeedService, FeedService>();
            
            // Add health-check service
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Unknown endpoint at this step of pipeline
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                Console.WriteLine($"1. Endpoint: {endpoint?.DisplayName ?? "(null)"}. Meta : {GetMetadata(endpoint)}");
                return next(context);
            });

            app.UseRouting();

            // Potentially known endpoint at this step of pipeline
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                Console.WriteLine($"2. Endpoint: {endpoint?.DisplayName ?? "(null)"}. Meta : {GetMetadata(endpoint)}");
                return next(context);
            });

            app.Use(next => async context =>
            {
                if (context.Request.Path == "/status")
                {
                    await context.Response.WriteAsync("Alive");
                    return;
                }

                await next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                // Map health-check middleware
                endpoints.MapHealthChecks("/health");

                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Feed service"); })
                    .Add(builder => builder.Metadata.Add(new
                    {
                        WithCustomMeta = true
                    }));

                endpoints.MapGet("/feed", async context =>
                {
                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    var feedItems = await feed.GetItemsAsync();

                    var limitString = context.Request.Query["limit"].FirstOrDefault();
                    if (int.TryParse(limitString, out var limit))
                    {
                        feedItems = feedItems.Take(limit);
                    }

                    var response = string.Join('\n', feedItems);
                    await context.Response.WriteAsync(response);
                });

                endpoints.MapGet("/feed/{id:int}", async context =>
                {
                    var id = int.Parse((string) context.Request.RouteValues["id"]);
                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    var feedItem = await feed.GetItemAsync(id);
                    await context.Response.WriteAsync(feedItem);
                });
                //.RequireHost("localhost:5002");

                // first priority - "/feed/1" more strict than "/feed/{id:int}"
                endpoints.MapGet("/feed/1", async context =>
                {
                    var id = 1;
                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    var feedItem = await feed.GetItemAsync(id);
                    await context.Response.WriteAsync($"strict [1] = {feedItem}");
                });

                endpoints.MapPost("/feed", async context =>
                {
                    await using var ms = new MemoryStream();
                    await context.Request.Body.CopyToAsync(ms);
                    var item = Encoding.UTF8.GetString(ms.ToArray());

                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    var id = await feed.AddItemAsync(item);
                    
                    context.Response.StatusCode = (int)HttpStatusCode.Created;
                    context.Response.Headers["Location"] = $"feed/{id}";
                    await context.Response.WriteAsync(id.ToString());
                });

                endpoints.MapPut("/feed/{id:int}", async context =>
                {
                    await using var ms = new MemoryStream();
                    await context.Request.Body.CopyToAsync(ms);
                    var item = Encoding.UTF8.GetString(ms.ToArray());
                    var id = int.Parse((string)context.Request.RouteValues["id"]);

                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    await feed.AddOrUpdateItemAsync(id, item);

                    context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                });

                endpoints.MapDelete("/feed/{id:int}", async context =>
                {
                    var id = int.Parse((string)context.Request.RouteValues["id"]);
                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    var deleted = await feed.DeleteItemAsync(id);
                    
                    context.Response.StatusCode = deleted 
                        ? (int)HttpStatusCode.NoContent 
                        : (int)HttpStatusCode.NotFound;
                });

                // RPC-style verb
                endpoints.MapDelete("/feed/clear", async context =>
                {
                    var feed = context.RequestServices.GetRequiredService<IFeedService>();
                    await feed.ClearAsync();
                    context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                });
            });

            // Endpoint wasn't found. It will be HTTP 404 status code.
            app.Use(next => context =>
            {
                var endpoint = context.GetEndpoint();
                Console.WriteLine($"3. Endpoint: {endpoint?.DisplayName ?? "(null)"}. Meta : {GetMetadata(endpoint)}");
                return next(context);
            });
        }

        private static string GetMetadata(Endpoint endpoint)
        {
            return endpoint == null ? "(null)" : string.Join('\n', endpoint.Metadata);
        }
    }
}
