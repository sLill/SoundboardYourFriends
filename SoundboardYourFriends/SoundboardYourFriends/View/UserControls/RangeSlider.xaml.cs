using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace SoundboardYourFriends.View.UserControls
{
    /// <summary>
    /// Interaction logic for RangeSlider.xaml
    /// </summary>
    public partial class RangeSlider : UserControl, INotifyPropertyChanged
    {
        #region Member Variables..
        private Timer _playbackTimer;
        #endregion Member Variables..

        #region Properties..

        #region PlaybackCursor
        private int _PlaybackCursor;
        public int PlaybackCursor
        {
            get { return _PlaybackCursor; }
            set 
            {
                _PlaybackCursor = value;
                RaisePropertyChanged();
            }
        }
        #endregion PlaybackCursor

        #region Minimum
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));
        #endregion Minimum

        #region LowerValue
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public static DependencyProperty LowerValueProperty = DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion LowerValue

        #region UpperValue
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        public static DependencyProperty UpperValueProperty = DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion UpperValue

        #region Maximum
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));
        #endregion Maximum
        #endregion Properties..

        #region Event Handlers..
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Event Handlers..

        #region Constructors..
        #region RangeSlider
        public RangeSlider()
        {
            InitializeComponent();
            this.Loaded += Slider_Loaded;
        }
        #endregion RangeSlider
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region LowerSlider_ValueChanged
        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpperSlider.Value = Math.Max(UpperSlider.Value, LowerSlider.Value);
        }
        #endregion LowerSlider_ValueChanged

        #region Slider_Loaded
        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
            _playbackTimer = new Timer(100);
            _playbackTimer.Elapsed += OnTimerElapsed;

            LowerSlider.ValueChanged += LowerSlider_ValueChanged;
            UpperSlider.ValueChanged += UpperSlider_ValueChanged;
        }
        #endregion Slider_Loaded

        #region OnTimerElapsed
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            PlaybackCursor++;
        }
        #endregion OnTimerElapsed

        #region UpperSlider_ValueChanged
        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSlider.Value = Math.Min(UpperSlider.Value, LowerSlider.Value);
        }
        #endregion UpperSlider_ValueChanged
        #endregion Events..

        #region RaisePropertyChanged
        protected void RaisePropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion RaisePropertyChanged
        #endregion Methods..
    }
}
