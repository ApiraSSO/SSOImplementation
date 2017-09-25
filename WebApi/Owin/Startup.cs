using Owin;

namespace WebApi
{
    public partial class Startup
	{
		/// <summary>
		/// Configurations the OWIN application.
		/// </summary>
		/// <param name="app">The application.</param>
		public void Configuration(IAppBuilder app)
		{
			this.ConfigureAuth(app);
		}
	}
}