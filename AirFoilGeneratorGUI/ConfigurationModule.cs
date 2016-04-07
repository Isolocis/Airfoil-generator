using AirfoilGeneratorGUI.Views.Overview;
using Ninject;
using Prism.Modularity;
using Prism.Regions;

namespace AirfoilGeneratorGUI
{
    public class ConfigurationModule : IModule
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationModule"/> class.
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/> implementation to use.</param>
        public ConfigurationModule(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Initialize()
        {
            this.kernel.Bind<object>().To<OverviewView>().Named(ViewModelLocator.GetViewName<OverviewView>());
            var regionManager = this.kernel.Get<IRegionManager>();
            regionManager.RequestNavigate(Shell.RegionNames.MainRegion, ViewModelLocator.GetViewName<OverviewView>());
        }
    }
}