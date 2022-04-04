using Elephant.ViewModel;
using Elephant_Services.ApplicationConfiguration;
using Elephant_Services.Export;
using Elephant_Services.TagDataFile;
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
            services.AddSingleton<ITagDataFileService, TagDataFileService>();
            services.AddSingleton<IConfigFileService, ConfigFileService>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TdcTagViewModel>();
            services.AddSingleton<ParameterViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
