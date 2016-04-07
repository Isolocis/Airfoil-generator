using System;
using AirfoilGeneratorGUI.Views.Settings;
using NACAAirFoilGenerator.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Prism.Events;
using Prism.Mvvm;

namespace AirfoilGeneratorGUI.Views.Plot
{
    public class PlotViewModel : BindableBase
    {
        private PlotModel plotModel;

        public PlotViewModel(IEventAggregator eventAggregator)
        {
            if(eventAggregator == null)
                throw new ArgumentNullException(nameof(eventAggregator));
            
            eventAggregator.GetEvent<UpdateEvent>().Subscribe(this.OnNewResultsAvailable);
        }

        /// <summary>
        /// Gets or sets the plot model.
        /// </summary>
        public PlotModel PlotModel
        {
            get { return this.plotModel; }
            private set { this.SetProperty(ref this.plotModel, value); }
        }

        private void OnNewResultsAvailable(AirfoilOutputData data)
        {
            var model = new PlotModel {Title = data.FullDesignation};

            var series1 = new LineSeries {Color = OxyColors.Blue};
            var series2 = new LineSeries {Color = OxyColors.Blue};

            for (int i = 0; i < data.XUpper.Length; i++)
            {
                series1.Points.Add(new DataPoint(data.XUpper[i], data.YUpper[i]));
                series2.Points.Add(new DataPoint(data.XLower[i], data.YLower[i]));
            }

            model.Series.Add(series1);
            model.Series.Add(series2);

            var axis = new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.Green
            };

            model.Axes.Add(axis);

            var axis2 = new LinearAxis
            {
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.Green,
                Position = AxisPosition.Bottom
            };
            model.Axes.Add(axis2);

            this.PlotModel = model;
        }
    }
}