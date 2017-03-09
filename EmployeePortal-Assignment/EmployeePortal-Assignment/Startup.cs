using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeePortal_Assignment.Startup))]
namespace EmployeePortal_Assignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
