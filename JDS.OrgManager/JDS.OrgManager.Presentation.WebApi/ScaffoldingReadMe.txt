ASP.NET Core MVC dependencies have been added to the project.
(These dependencies include packages required to enable scaffolding)

However you may still need to do make changes to your project.

1. Suggested changes to Startup class:

    1.1 Add a constructor:
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

    1.2 Add MVC services:
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

    1.3 Configure web app to use MVC routing:

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

2. Replace the project name and namespace in the newly scaffolded files:

    2.1 The file _Layout.cshtml contains the string "AppName" in a few places. It can be globally replaced with the name of the app.

    2.2 The file _ViewImports.cshtml contains the string "Root_Namespace". It should be replaced with the root namespace of the project.
