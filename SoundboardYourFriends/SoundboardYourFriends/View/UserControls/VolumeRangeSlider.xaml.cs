using System;
using System.Windows;
using System.Windows.Controls;

namespace SoundboardYourFriends.View.UserControls
{
    public partial class VolumeRangeSlider : UserControl
    {
        #region Member Variables..
        #endregion Member Variables..

        #region Properties..
        #region VolumeValue
        public double VolumeValue
        {
            get { return (double)GetValue(VolumeValueProperty); }
            set { SetValue(VolumeValueProperty, value); }
        }

        public static DependencyProperty VolumeValueProperty = DependencyProperty.Register("VolumeValue", typeof(double), typeof(VolumeRangeSlider),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion VolumeValue
        #endregion Properties..

        #region Event Handlers/Delegates..
        #endregion Event Handlers/Delegates..

        #region Constructors..
        #region VolumeRangeSlider
        public VolumeRangeSlider()
        {
            InitializeComponent();
            this.Loaded += Slider_Loaded;
        }
        #endregion VolumeRangeSlider
        #endregion Constructors..

        #region Methods..
        #region Events..
        #region Slider_Loaded
        private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion Slider_Loaded
        #endregion Events..
        #endregion Methods..
    }
}
