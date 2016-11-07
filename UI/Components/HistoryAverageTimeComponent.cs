using LiveSplit.TimeFormatters;
using System;

namespace LiveSplit.UI.Components
{
    public class HistoryAverageTimeComponent : InfoTimeComponent
    {
        public HistoryAverageComponentSettings Settings { get; set; }

        public HistoryAverageTimeComponent(HistoryAverageComponentSettings settings, RegularTimeFormatter formatter )
            : base(settings.Text1, TimeSpan.Zero, formatter)
        {
            Settings = settings;
        }

        public override void PrepareDraw(Model.LiveSplitState state, LayoutMode mode)
        {
            NameMeasureLabel.Font = Settings.OverrideFont1 ? Settings.Font1 : state.LayoutSettings.TextFont;
            ValueLabel.Font = Settings.OverrideFont2 ? Settings.Font2 : state.LayoutSettings.TextFont;
            NameLabel.Font = Settings.OverrideFont1 ? Settings.Font1 : state.LayoutSettings.TextFont;
        }
    }
}
