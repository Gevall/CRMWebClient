using CRMWebClient.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CRMWebClient
{
    public class Program
    {
        public static readonly ILoggerFactory logFactory = LoggerFactory.Create(conf => conf.AddConsole());

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ��������, ����� �� �������������� �������� ��� ��������� ������
                        ValidateIssuer = true,

                        // ������, �������������� ��������
                        ValidIssuer = AuthOptions.ISSUER,

                        // ����� �� �������������� ����������� ������
                        ValidateAudience = true,

                        // ��������� ����������� ������
                        ValidAudience = AuthOptions.AUDIENCE,

                        // ����� �� �������������� ����� �������������
                        ValidateLifetime = true,

                        // ��������� ����� ������������
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

                        // ��������� ����� ������������
                        ValidateIssuerSigningKey = true,
                    };
                });
            //builder.Services.AddControllersWithViews();
            builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

            builder.Services.AddHttpClient();
            builder.Services.AddTransient<HomeController>();

            var app = builder.Build();

            
            //app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            //app.UseDefaultFiles();

            ////app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(r =>
            {
                r.MapRoute(
                    name: default,
                    template: "{controller=Home}/{action=Index}"
                    );
            }
            );
            
            app.Run();
        }
    }
}