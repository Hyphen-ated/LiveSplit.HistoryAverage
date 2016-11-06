namespace LiveSplit.UI.Components
{
    public class InternalHistoryAverageComponent : InfoTextComponent
    {
        public HistoryAverageComponentSettings Settings { get; set; }

        public InternalHistoryAverageComponent(HistoryAverageComponentSettings settings)
            : base("", "")
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
