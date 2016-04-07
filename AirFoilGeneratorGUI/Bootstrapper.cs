using System.Windows;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Ninject;

namespace AirfoilGeneratorGUI
{
    /// <summary>
    /// The application bootstrapper.
    /// </summary>
    public class Bootstrapper : NinjectBootstrapper
    {
        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        /// The shell of the application.
        /// </returns>
        /// <remarks>
        /// If the returned instance is a <see cref="T:System.Windows.DependencyObject"/>, the
        ///             <see cref="T:Prism.Bootstrapper"/> will attach the default <see cref="T:Prism.Regions.IRegionManager"/> of
        ///             the application in its <see cref="F:Prism.Regions.RegionManager.RegionManagerProperty"/> attached property
        ///             in order to be able to add regions by using the <see cref="F:Prism.Regions.RegionManager.RegionNameProperty"/>
        ///             attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            var window = this.Shell as Window;
            window?.Show();
        }

        /// <summary>
        /// Configures the <see cref="T:IModuleCatalog"/> used by Prism.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            
            var catalog = this.ModuleCatalog as ModuleCatalog;
            catalog?.AddModule(typeof(ConfigurationModule));
        }

        /// <summary>
        /// Configures the <see cref="T:ViewModelLocator"/> used by Prism.
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(ViewModelLocator.LocateViewModel);
        }
    }
}