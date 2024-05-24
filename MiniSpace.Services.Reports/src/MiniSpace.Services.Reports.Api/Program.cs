using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Convey.Logging;
using Convey.Types;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniSpace.Services.Reports.Application;
using MiniSpace.Services.Reports.Application.Commands;
using MiniSpace.Services.Reports.Application.DTO;
using MiniSpace.Services.Reports.Application.Services;
//using MiniSpace.Services.Reports.Application.Queries;
using MiniSpace.Services.Reports.Core.Entities;
using MiniSpace.Services.Reports.Infrastructure;

namespace MiniSpace.Services.Reports.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Post<SearchReports>("reports/search", async (cmd, ctx) =>
                        {
                            var pagedResult = await ctx.RequestServices.GetService<IReportsService>().BrowseReportsAsync(cmd);
                            await ctx.Response.WriteJsonAsync(pagedResult);
                        })
                    )
                    .UseDispatcherEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Post<CreateReport>("reports", afterDispatch: (cmd, ctx) 
                            => ctx.Response.Created($"reports/{cmd.ReportId}"))
                        .Post<CancelReport>("reports/{reportId}/cancel")
                        .Post<ResolveReport>("reports/{reportId}/resolve")
                        .Post<RejectReport>("reports/{reportId}/reject")
                        ))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}
