using Elephant.Services;
using Elephant.Services.ExportService;
using Elephant.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Elephant_wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public sealed partial class App : Application
    {
        public IServiceProvider Services { get; }
        public new static App Current => (App)Application.Current;

        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IExportService, ExportService>(_ => ExportService.Instance);
            services.AddSingleton<IJsonTdcTagService, JsonFileTdcTagService>(_ => JsonFileTdcTagService.Instance);

            services.AddTransient<TdcTagViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
