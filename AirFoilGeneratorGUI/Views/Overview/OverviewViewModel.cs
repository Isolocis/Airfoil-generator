using System;
using AirfoilGeneratorGUI.Views.Settings;
using Prism.Mvvm;
using Prism.Regions;

namespace AirfoilGeneratorGUI.Views.Overview
{
    public class OverviewViewModel : BindableBase, INavigationAware
    {
        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        /// <summary>
        /// Called to determine if this instance can handle the navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>True if this instance accepts the navigation request. Otherwise, false.</returns>
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// Calles when navigation leaves the implementer.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}