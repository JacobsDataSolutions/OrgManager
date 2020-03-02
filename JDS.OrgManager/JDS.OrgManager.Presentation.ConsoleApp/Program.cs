// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.HumanResources.Employees.Commands.RegisterOrUpdateEmployee;
using JDS.OrgManager.Application.HumanResources.Employees.Queries.GetEmployeeOrgChart;
using JDS.OrgManager.Common.Text;
using JDS.OrgManager.Domain;
using JDS.OrgManager.Domain.HumanResources.Advanced;
using JDS.OrgManager.Domain.HumanResources.Employees;
using JDS.OrgManager.Infrastructure;
using JDS.OrgManager.Persistence;
using JDS.OrgManager.Presentation.ConsoleApp.PaidTimeOffPolicies;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Presentation.ConsoleApp
{
    internal static class Program
    {
        private const int advancedOrgDepth = 3;

        private const int advancedOrgNumTrees = 4;

        private const int basicOrgDepth = 3;

        private const bool runAdvancedScenario = false;

        public static async Task Main(string[] args)
        {
            var config = LoadConfiguration();
            var provider = ConfigureServices(config);

            var dbUpdater = provider.GetRequiredService<DatabaseUpdater>();
            await dbUpdater.UpdateDatabaseAsync();

            if (runAdvancedScenario)
            {
                await BuildAdvancedOrganization(provider);
            }
            else
            {
                await BuildBasicOrganization(provider);
            }

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static async Task BuildAdvancedOrganization(IServiceProvider provider)
        {
            var ptoPolicyController = provider.GetRequiredService<PaidTimeOffPolicyController>();
            var policies = (await ptoPolicyController.GetAllPtoPoliciesAsync()).Payload;

            var employeeController = provider.GetRequiredService<EmployeeController>();
            var mapper = provider.GetRequiredService<IViewModelToDomainEntityMapper<RegisterOrUpdateEmployeeCommand, Employee>>();

            Console.WriteLine("Simulating HR personnel creating employees in the UI...");

            // Generate organization N levels deep, with multiple top-level employees.
            for (int n = 0; n < advancedOrgNumTrees; n++)
            {
                var boss = NewEmployeeGenerator.GenerateEmployee(advancedOrgDepth);
                foreach (var employee in FlattenOrganization(boss))
                {
                    Console.WriteLine($"Registering new employee: {employee.FirstName} {employee.LastName}.");
                    var employeeVm = mapper.MapToViewModel(employee);
                    var selectedPtoPolicy = policies.Where(p => p.EmployeeLevel == employee.EmployeeLevel).RandomSubset(1).First();
                    employeeVm.PaidTimeOffPolicyId = selectedPtoPolicy.Id;
                    if (!selectedPtoPolicy.AllowsUnlimitedPto)
                    {
                        employeeVm.PtoHoursRemaining = 0.0m;
                    }

                    var registerEmployeeResult = await employeeController.RegisterOrUpdateEmployeeAsync(employeeVm);
                    if (registerEmployeeResult.Succeeded)
                    {
                        Console.WriteLine("Got success result from controller.");
                    }
                    else
                    {
                        Console.WriteLine($@"Failed with message: ""{string.Join(" ", registerEmployeeResult.Errors)}""");
                        break;
                    }
                    employee.Id = registerEmployeeResult.Payload.Id;
                }
            }
            Console.WriteLine("Done registering employees. Verifying entire organization and getting stats...");
            var verifyOrgResult = await employeeController.VerifyOrganizationAsync();
            if (verifyOrgResult.Succeeded)
            {
                Console.WriteLine("Got success result from controller.");
                var stats = verifyOrgResult.Payload.Stats;
                var sb = new StringBuilder().AppendLine($"{stats.Count} organizational structures analyzed.");
                foreach (var stat in stats)
                {
                    sb.AppendLine($"Num Employees: {stat.NumEmployees}, Org Complexity: {stat.OrgComplexity}");
                }
                sb.AppendLine($"Total Employees: {stats.Sum(s => s.NumEmployees)}, Total Complexity: {stats.Sum(s => s.OrgComplexity)}");
                Console.WriteLine(sb.ToString());
            }
            else
            {
                Console.WriteLine($@"Failed with message: ""{string.Join(" ", verifyOrgResult.Errors)}""");
            }

            Console.WriteLine("Querying the API to get complete org structure...");
            var orgChartResult = await employeeController.GetEmployeeOrgChartAsync();
            if (orgChartResult.Succeeded)
            {
                Console.WriteLine("Got success result from controller. Here is the org chart:");
                Console.WriteLine(string.Join("\r\n\r\n", from e in orgChartResult.Payload select GetPrintableOrgTree(e).ToString()));
            }
            else
            {
                Console.WriteLine($@"Failed with message: ""{string.Join(" ", orgChartResult.Errors)}""");
                return;
            }
        }

        private static async Task BuildBasicOrganization(IServiceProvider provider)
        {
            // *** Simulate data hitting the web API from the UI. ***

            // Pretend we are ASP.NET Core resolving the controller.
            var ptoPolicyController = provider.GetRequiredService<PaidTimeOffPolicyController>();
            Console.WriteLine("Querying PTO policies. This should cause them to be added to cache.");
            _ = await ptoPolicyController.GetAllPtoPoliciesAsync();
            Console.WriteLine("Querying PTO policies again to test the cache. Here they are:");
            var policies = (await ptoPolicyController.GetAllPtoPoliciesAsync()).Payload;
            foreach (var policy in policies)
            {
                Console.WriteLine(policy);
            }

            var employeeController = provider.GetRequiredService<EmployeeController>();
            var mapper = provider.GetRequiredService<IViewModelToDomainEntityMapper<RegisterOrUpdateEmployeeCommand, Employee>>();

            Console.WriteLine("Simulating HR personnel creating employees in the UI...");

            // Generate a fake organization N levels deep.
            var boss = NewEmployeeGenerator.GenerateEmployee(basicOrgDepth);
            foreach (var employee in FlattenOrganization(boss))
            {
                Console.WriteLine($"Registering new employee: {employee.FirstName} {employee.LastName}.");
                var employeeVm = mapper.MapToViewModel(employee);
                var selectedPtoPolicy = policies.Where(p => p.EmployeeLevel == employee.EmployeeLevel).RandomSubset(1).First();
                employeeVm.PaidTimeOffPolicyId = selectedPtoPolicy.Id;
                if (!selectedPtoPolicy.AllowsUnlimitedPto)
                {
                    employeeVm.PtoHoursRemaining = 0.0m;
                }

                var registerEmployeeResult = await employeeController.RegisterOrUpdateEmployeeAsync(employeeVm);
                if (registerEmployeeResult.Succeeded)
                {
                    Console.WriteLine("Got success result from controller.");
                }
                else
                {
                    Console.WriteLine($@"Failed with message: ""{string.Join(" ", registerEmployeeResult.Errors)}""");
                    break;
                }
                employee.Id = registerEmployeeResult.Payload.Id;
            }

            Console.WriteLine("Done registering employees. Getting full employee list...");
            var employeeListResult = await employeeController.GetEmployeeListAsync();
            if (employeeListResult.Succeeded)
            {
                Console.WriteLine("Got success result from controller. Here they are:");
                Console.WriteLine("Id   Name                            Gender  Level  Num.Subordinates  Manager Name");
                Console.WriteLine("------------------------------------------------------------------------------------------------");
                foreach (var emp in employeeListResult.Payload)
                {
                    Console.WriteLine($"{emp.Id,-3}  {(emp.LastName + ", " + emp.FirstName + " " + emp.MiddleName).Trim(',', ' '),-30}  {emp.Gender,-6}    {emp.EmployeeLevel,-2}          {emp.NumSubordinates,-7}    {(emp.ManagerLastName + ", " + emp.ManagerFirstName + " " + emp.ManagerMiddleName).Trim(',', ' '),-30}");
                }
            }
            else
            {
                Console.WriteLine($@"Failed with message: ""{string.Join(" ", employeeListResult.Errors)}""");
                return;
            }

            Console.WriteLine("Querying the API again to get details of a random employee...");
            var empId = employeeListResult.Payload.RandomSubset(1).First().Id;
            var employeeDetailResult = await employeeController.GetEmployeeDetailsAsync(empId);
            if (employeeDetailResult.Succeeded)
            {
                Console.WriteLine("Got success result from controller. Here are the employee details:");
                Console.WriteLine(employeeDetailResult.Payload);
            }
            else
            {
                Console.WriteLine($@"Failed with message: ""{string.Join(" ", employeeDetailResult.Errors)}""");
                return;
            }

            Console.WriteLine("Querying the API to get complete org structure...");
            var orgChartResult = await employeeController.GetEmployeeOrgChartAsync();
            if (orgChartResult.Succeeded)
            {
                Console.WriteLine("Got success result from controller. Here is the org chart:");
                Console.WriteLine(string.Join("\r\n\r\n", from e in orgChartResult.Payload select GetPrintableOrgTree(e).ToString()));
            }
            else
            {
                Console.WriteLine($@"Failed with message: ""{string.Join(" ", orgChartResult.Errors)}""");
                return;
            }
        }

        private static IServiceCollection ConfigureSerilog(this IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .WriteTo.File(@"Logs\orgManager.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(logger, false));
            return services;
        }

        private static IServiceProvider ConfigureServices(IConfiguration configuration)
        {
            var services = new ServiceCollection()

            // Add classes from this assembly.
            .AddSingleton<DatabaseUpdater>()
            .AddSingleton(configuration)
            .AddScoped<EmployeeController>()
            .AddScoped<PaidTimeOffPolicyController>()

            // Add caching.
            .AddSingleton<IDistributedCache, MemoryDistributedCache>()

            // Configure logging.
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            })
            .ConfigureSerilog()

            // Add mediatR and associated types.
            .AddMediatR((from t in new[] { typeof(DomainLayer), typeof(ApplicationLayer) } select t.Assembly).ToArray())

            // Add layers.
            .AddDomainLayer()
            .AddDomainLayerAdvanced()
            .AddApplicationLayer(addRequestLogging: true, useReadThroughCachingForQueries: true)
            .AddPersistenceLayer(configuration)
            .AddInfrastructureLayer()

            // Other
            ;

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.WireUpDomainEventHandlers();
            return serviceProvider;
        }

        private static List<Employee> FlattenOrganization(Employee employee)
        {
            var list = new List<Employee>();
            void RecurseSubordinates(Employee e)
            {
                foreach (var subordinate in e.Subordinates)
                {
                    RecurseSubordinates(subordinate);
                }
                list.Add(e);
            };
            RecurseSubordinates(employee);
            return list;
        }

        private static string GetPrintableOrgTree(GetEmployeeOrgChartViewModel employee)
        {
            AsciiTreeNode<string> ConvertToTreeNodes(GetEmployeeOrgChartViewModel e)
            {
                var childNodes = (from s in e.Subordinates select ConvertToTreeNodes(s)).ToArray();
                return new AsciiTreeNode<string>($"({ e.Id }) {e.LastName}, {e.FirstName} {e.MiddleName}".Trim(), childNodes);
            }
            return ConvertToTreeNodes(employee).ToString();
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false,
                             reloadOnChange: true);
            return builder.Build();
        }
    }
}