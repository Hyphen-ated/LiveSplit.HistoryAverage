using LiveSplit.Model;
using System;

namespace LiveSplit.UI.Components
{
    public class HistoryAverageComponentFactory : IComponentFactory
    {
        public string ComponentName => "History Average";

        public string Description => "Calculates average run times from your history.";

        public ComponentCategory Category => ComponentCategory.Information;

        public IComponent Create(LiveSplitState state) => new HistoryAverageComponent(state);

        public string UpdateName => ComponentName;

        public string XMLURL => "http://livesplit.org/update/Components/update.LiveSplit.HistoryAverage.xml";

        public string UpdateURL => "http://livesplit.org/update/";

        public Version Version => Version.Parse("0.0.1");
    }
}
