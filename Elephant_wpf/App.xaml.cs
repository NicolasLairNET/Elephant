using Elephant.Services;
using Elephant.Services.ConfigFileManagerService;
using Elephant.Services.ExportService;
using Elephant.Services.JsonFileTDCTag;
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
            InitializeComponent();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IExportService, ExportService>();
            services.AddSingleton<IJsonTdcTagService, JsonFileTdcTagService>();
            services.AddSingleton<IConfigFileManagerService, ConfigFileManager>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TdcTagViewModel>();
            services.AddSingleton<ParameterViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
