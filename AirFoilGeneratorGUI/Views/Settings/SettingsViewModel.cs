using System;
using NACAAirFoilGenerator;
using NACAAirFoilGenerator.Data;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace AirfoilGeneratorGUI.Views.Settings
{
    /// <summary>
    /// View model of <see cref="SettingsView"/>
    /// </summary>
    public class SettingsViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private bool closeTrailingEdge;
        private string designation;
        private string error = "";
        private int nodes;
        private bool useHalfCosineSpacing;

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator implementation to use.</param>
        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            if(eventAggregator == null)
                throw new ArgumentNullException(nameof(eventAggregator));

            this.eventAggregator = eventAggregator;
            this.UpdateCommand = new DelegateCommand(this.UpdateCommand_Execute, this.UpdateCommand_CanExecute);
        }

        /// <summary>
        /// Indicates if the trailing edge has to be closed.
        /// </summary>
        public bool CloseTrailingEdge
        {
            get { return this.closeTrailingEdge; }
            set { this.SetProperty(ref this.closeTrailingEdge, value); }
        }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        public string Designation
        {
            get { return this.designation; }
            set
            {
                if (this.SetProperty(ref this.designation, value))
                {
                    this.UpdateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Error
        {
            get { return this.error; }
            set { this.SetProperty(ref this.error, value); }
        }

        /// <summary>
        /// Gets or sets the number of nodes.
        /// </summary>
        public int Nodes
        {
            get { return this.nodes; }
            set { this.SetProperty(ref this.nodes, value); }
        }

        /// <summary>
        /// Indicates if half cosine spacing should be used for the generated nodes.
        /// </summary>
        public bool UseHalfCosineSpacing
        {
            get { return this.useHalfCosineSpacing; }
            set { this.SetProperty(ref this.useHalfCosineSpacing, value); }
        }
        
        public DelegateCommand UpdateCommand { get; }

        private bool UpdateCommand_CanExecute()
        {
            if (string.IsNullOrWhiteSpace(this.Designation))
                return false;

            var input = new AirfoilInputData
            {
                Designation = this.Designation
            };

            string errorMessage;
            var valid = input.IsValid(out errorMessage);
            this.Error = errorMessage;

            return valid;
        }

        private void UpdateCommand_Execute()
        {
            var output = AirfoilGenerator.GenerateAirfoilData(new AirfoilInputData
            {
                Designation = this.Designation,
                NodesPerSide = this.Nodes,
                CloseTrailingEdge = this.CloseTrailingEdge,
                UseHalfCosineSpacing = this.useHalfCosineSpacing
            });

            this.eventAggregator.GetEvent<UpdateEvent>().Publish(output);
        }
    }
}