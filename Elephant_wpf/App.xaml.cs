using Elephant.Services.ApplicationConfiguration;
using Elephant.Services.Export;
using Elephant.Services.TagDataFile;
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
            services.AddSingleton<ITagDataFileService, TagDataFileService>();
            services.AddSingleton<IConfigFileService, ConfigFileService>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TdcTagViewModel>();
            services.AddSingleton<ParameterViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
