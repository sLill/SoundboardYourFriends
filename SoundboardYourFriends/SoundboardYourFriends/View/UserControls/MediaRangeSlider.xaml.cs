using System;
using System.Windows;
using System.Windows.Controls;

namespace SoundboardYourFriends.View.UserControls
{
    public partial class MediaRangeSlider : UserControl
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region PlaybackCursorValue
        public double PlaybackCursorValue
        {
            get { return (double)GetValue(PlaybackCursorValueProperty); }
            set { SetValue(PlaybackCursorValueProperty, value); }
        }

        public static DependencyProperty PlaybackCursorValueProperty = DependencyProperty.Register("PlaybackCursorValue", typeof(double), typeof(MediaRangeSlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion PlaybackCursorValue

        #region Minimum
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(MediaRangeSlider), new UIPropertyMetadata(0d));
        #endregion Minimum

        #region LowerValue
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static DependencyProperty LowerValueProperty = DependencyProperty.Register("LowerValue", typeof(double), typeof(MediaRangeSlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion LowerValue

        #region UpperValue
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public static DependencyProperty UpperValueProperty = DependencyProperty.Register("UpperValue", typeof(double), typeof(MediaRangeSlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion UpperValue

        #region Maximum
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(MediaRangeSlider), new UIPropertyMetadata(0d));
        #endregion Maximum
        #endregion Properties..

        #region Event Handlers/Delegates..
        public delegate void OnPlaybackTimerElapsed();
        #endregion Event Handlers..

        #region Constructors..
        #region MediaRangeSlider
        public MediaRangeSlider()
        {
            InitializeComponent();
            this.Loaded += Slider_Loaded;
        }
        #endregion MediaRangeSlider
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region LowerSlider_ValueChanged
        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpperSlider.Value = Math.Max(UpperSlider.Value, LowerSlider.Value);
            PlaybackCursorSlider.Value = Math.Max(PlaybackCursorSlider.Value, LowerSlider.Value);
        }
        #endregion LowerSlider_ValueChanged

        #region Slider_Loaded
        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            LowerSlider.ValueChanged += LowerSlider_ValueChanged;
            UpperSlider.ValueChanged += UpperSlider_ValueChanged;
        }
        #endregion Slider_Loaded

        #region UpperSlider_ValueChanged
        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSlider.Value = Math.Min(UpperSlider.Value, LowerSlider.Value);
            PlaybackCursorSlider.Value = Math.Min(UpperSlider.Value, PlaybackCursorSlider.Value);
        }
        #endregion UpperSlider_ValueChanged
        #endregion Events..
        #endregion Methods..
    }
}
