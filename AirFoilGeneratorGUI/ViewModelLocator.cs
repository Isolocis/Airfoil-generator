using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace AirfoilGeneratorGUI
{
    /// <summary>
    /// Class that provides methods to identify views and viewmodels.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Gets the name of the specified view.
        /// </summary>
        /// <param name="viewType">The type of view.</param>
        /// <returns>The name of the view as by convention.</returns>
        public static string GetViewName(Type viewType)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));

            return viewType.Name;
        }
        /// <summary>
        /// Gets the name of the specified view.
        /// </summary>
        /// <typeparam name="TView">The type of view.</typeparam>
        /// <returns>The name of the view as by convention.</returns>
        public static string GetViewName<TView>() where TView : class
        {
            return GetViewName(typeof(TView));
        }

        /// <summary>
        /// Locates the view model's type based on that of the view using the conventions of this class.
        /// </summary>
        /// <param name="viewType">The type of the view.</param>
        /// <returns>The type of the </returns>
        public static Type LocateViewModel(Type viewType)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));

            var viewName = viewType.FullName;
            var viewAssemblyName = viewType.Assembly.FullName;

            if (viewName.EndsWith("View"))
            {
                viewName = viewName.Substring(0, viewName.Length - 4);
            }

            var viewModelTypeName = string.Format(
                CultureInfo.InvariantCulture,
                $"{viewName}ViewModel, {viewAssemblyName}");
            var viewModelType = Type.GetType(viewModelTypeName);

            if (viewModelType == null)
                throw new ViewModelNotFoundException();

            return viewModelType;
        }


        /// <summary>
        /// Exception that is thrown when the viewmodel of a view could not be located. Code by Niels Hutman.
        /// </summary>
        [Serializable]
        public class ViewModelNotFoundException : Exception
        {
            private readonly Type viewType;

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewModelNotFoundException"/> class.
            /// </summary>
            public ViewModelNotFoundException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewModelNotFoundException"/> class.
            /// </summary>
            /// <param name="viewType">The type of the view for which the viewmodel could not be located.</param>
            public ViewModelNotFoundException(Type viewType)
                : this($"Unable to locate view model for view type: {viewType}")
            {
                this.viewType = viewType;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewModelNotFoundException"/> class.
            /// </summary>
            /// <param name="message">The error message.</param>
            public ViewModelNotFoundException(string message) : base(message)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewModelNotFoundException"/> class.
            /// </summary>
            /// <param name="message">The error message.</param>
            /// <param name="inner">The exception that caused this exception.</param>
            public ViewModelNotFoundException(string message, Exception inner) : base(message, inner)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewModelNotFoundException"/> class.
            /// </summary>
            /// <param name="info">The serialization info.</param>
            /// <param name="context">The streaming context.</param>
            protected ViewModelNotFoundException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }

            /// <summary>
            /// Gets the type of the view for which the viewmodel could not be located.
            /// </summary>
            public Type ViewType => this.viewType;
        }
    }


}