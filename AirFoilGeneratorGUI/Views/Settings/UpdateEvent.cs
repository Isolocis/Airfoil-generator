using NACAAirFoilGenerator.Data;
using Prism.Events;

namespace AirfoilGeneratorGUI.Views.Settings
{
    public class UpdateEvent : PubSubEvent<AirfoilOutputData>
    {
    }
}