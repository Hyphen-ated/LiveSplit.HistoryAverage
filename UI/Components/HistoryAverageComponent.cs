using LiveSplit.Model;
using LiveSplit.TimeFormatters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class HistoryAverageComponent : IComponent
    {
        protected InfoTimeComponent InternalComponent { get; set; }
        public HistoryAverageComponentSettings Settings { get; set; }
        protected LiveSplitState CurrentState { get; set; }

        protected int PreviousHistorySize { get; set; }
        protected TimingMethod PreviousTimingMethod { get; set; }

        public float PaddingTop => InternalComponent.PaddingTop;
        public float PaddingLeft => InternalComponent.PaddingLeft;
        public float PaddingBottom => InternalComponent.PaddingBottom;
        public float PaddingRight => InternalComponent.PaddingRight;

        public TimeSpan? HistoryAverageValue { get; set; }

        public IDictionary<string, Action> ContextMenuControls => null;

        private RegularTimeFormatter Formatter { get; set; }

        public HistoryAverageComponent(LiveSplitState state)
        {
            Settings = new HistoryAverageComponentSettings()
            {
                CurrentState = state
            };
            Formatter = new RegularTimeFormatter(Settings.Accuracy);
            InternalComponent = new InfoTimeComponent(Settings.Text1, TimeSpan.Zero, Formatter);
            state.OnSplit += state_OnSplit;
            state.OnUndoSplit += state_OnUndoSplit;
            state.OnReset += state_OnReset;
            CurrentState = state;
            CurrentState.RunManuallyModified += CurrentState_RunModified;
            UpdateHistoryValue(state);
        }
    
        void CurrentState_RunModified(object sender, EventArgs e)
        {
            UpdateHistoryValue(CurrentState);
        }

        void state_OnReset(object sender, TimerPhase e)
        {
            UpdateHistoryValue((LiveSplitState)sender);
        }

        void state_OnUndoSplit(object sender, EventArgs e)
        {
            UpdateHistoryValue((LiveSplitState)sender);
        }

        void state_OnSplit(object sender, EventArgs e)
        {
            UpdateHistoryValue((LiveSplitState)sender);
        }

        void UpdateHistoryValue(LiveSplitState state)
        {
            HistoryAverageValue = new TimeSpan(0, 1, 23);
            PreviousTimingMethod = state.CurrentTimingMethod;
        }

        private bool CheckIfRunChanged(LiveSplitState state)
        {
            if (PreviousTimingMethod != state.CurrentTimingMethod)
                return true;

            return false;
        }

        private void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if (Settings.BackgroundColor.ToArgb() != Color.Transparent.ToArgb()
                || Settings.BackgroundGradient != GradientType.Plain
                && Settings.BackgroundColor2.ToArgb() != Color.Transparent.ToArgb())
            {
                var gradientBrush = new LinearGradientBrush(
                            new PointF(0, 0),
                            Settings.BackgroundGradient == GradientType.Horizontal
                            ? new PointF(width, 0)
                            : new PointF(0, height),
                            Settings.BackgroundColor,
                            Settings.BackgroundGradient == GradientType.Plain
                            ? Settings.BackgroundColor
                            : Settings.BackgroundColor2);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
            }
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            DrawBackground(g, state, width, VerticalHeight);

            InternalComponent.DisplayTwoRows = Settings.Display2Rows;

            InternalComponent.NameLabel.HasShadow
                = InternalComponent.ValueLabel.HasShadow
                = state.LayoutSettings.DropShadows;

            Formatter.Accuracy = Settings.Accuracy;

            InternalComponent.NameLabel.ForeColor = Settings.OverrideTextColor ? Settings.TextColor : state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = Settings.OverrideTimeColor ? Settings.TimeColor : state.LayoutSettings.TextColor;

            InternalComponent.DrawVertical(g, state, width, clipRegion);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            DrawBackground(g, state, HorizontalWidth, height);

            InternalComponent.NameLabel.HasShadow
                = InternalComponent.ValueLabel.HasShadow
                = state.LayoutSettings.DropShadows;

            Formatter.Accuracy = Settings.Accuracy;

            InternalComponent.NameLabel.ForeColor = Settings.OverrideTextColor ? Settings.TextColor : state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = Settings.OverrideTimeColor ? Settings.TimeColor : state.LayoutSettings.TextColor;

            InternalComponent.DrawHorizontal(g, state, height, clipRegion);
        }

        public float VerticalHeight => InternalComponent.VerticalHeight;

        public float MinimumWidth => InternalComponent.MinimumWidth; 

        public float HorizontalWidth => InternalComponent.HorizontalWidth; 

        public float MinimumHeight => InternalComponent.MinimumHeight;

        public string ComponentName => "History Average";

        public Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (CheckIfRunChanged(state))
                UpdateHistoryValue(state);

            InternalComponent.TimeValue = HistoryAverageValue;

            InternalComponent.Update(invalidator, state, width, height, mode);
        }

        public void Dispose()
        {
        }

        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();
    }
}
