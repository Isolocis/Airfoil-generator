using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AirfoilGeneratorGUI.Views.Settings;
using NACAAirFoilGenerator;
using NACAAirFoilGenerator.Data;
using NACAAirFoilGenerator.Exceptions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace AirfoilGeneratorGUI.Views.Export
{
    public class ExportViewModel : BindableBase
    {
        private string error = "";
        private bool includeThirdCoordinate = true;
        private string outputPath = "";
        private AirfoilOutputData results;
        private string success = "";

        /// <summary>
        /// Initialises a new instance of the <see cref="ExportViewModel"/>
        /// </summary>
        /// <param name="eventAggregator">The event aggregator implementation to use.</param>
        public ExportViewModel(IEventAggregator eventAggregator)
        {
            if(eventAggregator == null)
                throw new ArgumentNullException(nameof(eventAggregator));
            
            eventAggregator.GetEvent<UpdateEvent>().Subscribe(this.OnNewResultsAvailable);

            this.ExportCommand = new DelegateCommand(this.ExportCommand_Execute, this.ExportCommand_CanExecute);
        }

        /// <summary>
        /// Contains the error message.
        /// </summary>
        public string Error
        {
            get { return this.error; }
            set { this.SetProperty(ref this.error, value); }
        }

        /// <summary>
        /// Indicates if a third, zero, coordinate has to be included in the exported file.
        /// </summary>
        public bool IncludeThirdCoordinate
        {
            get { return this.includeThirdCoordinate; }
            set { this.SetProperty(ref this.includeThirdCoordinate, value); }
        }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        public string OutputPath
        {
            get { return this.outputPath; }
            set { this.SetProperty(ref this.outputPath, value); }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        public AirfoilOutputData Results
        {
            get { return this.results; }
            private set
            {
                if (!this.SetProperty(ref this.results, value)) return;

                this.ExportCommand.RaiseCanExecuteChanged();
                this.OnPropertyChanged(() => this.ResultsAvailable);
            }
        }

        /// <summary>
        /// Gets a bool indication if results are available.
        /// </summary>
        public bool ResultsAvailable => this.Results != null;

        /// <summary>
        /// Gets or sets the success message.
        /// </summary>
        public string Success
        {
            get { return this.success; }
            private set
            {
                if (this.SetProperty(ref this.success, value))
                {
                    this.ResetSuccessMessage();
                }
            }
        }

        private async void ResetSuccessMessage()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            this.Success = "";
        }

        private void OnNewResultsAvailable(AirfoilOutputData data)
        {
            this.Results = data;
            this.OutputPath = Path.Combine(Environment.CurrentDirectory, $"{this.Results.FullDesignation}.dat");
            this.Error = "";
            this.Success = "";
        }

        /// <summary>
        /// Gets the command that exports the airfoil data.
        /// </summary>
        public DelegateCommand ExportCommand { get; }

        private bool ExportCommand_CanExecute()
        {
            return this.ResultsAvailable;
        }

        private void ExportCommand_Execute()
        {
            try
            {
                AirfoilGenerator.WriteOutputFile(this.Results, this.OutputPath, this.IncludeThirdCoordinate);
                this.Success = $"File successfully exported to {this.OutputPath.Split('\\').Last()}";
            }
            catch (SaveAirfoilDataException ex)
            {
                this.Error = ex.Message;
            }
        }
    }
}