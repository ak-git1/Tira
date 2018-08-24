using System.Windows;
using System.Windows.Controls;

namespace Tira.App.UserControls
{
    /// <summary>
    /// Scroll ruler control
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ScrollViewer" />
    public class ScrollRuler : ScrollViewer
    {
        #region Variables

        /// <summary>
        /// The horizontal ruler scroll viewer
        /// </summary>
        private ScrollViewer _horizontalRulerScrollViewer;

        /// <summary>
        /// The vertical ruler scroll viewer
        /// </summary>
        private ScrollViewer _verticalRulerScrollViewer;

        #endregion

        #region Properties

        /// <summary>
        /// Ruler X length
        /// </summary>
        public double RulerXLength
        {
            get => (double)GetValue(RulerXLengthProperty);
            set => SetValue(RulerXLengthProperty, value);
        }

        /// <summary>
        /// Ruler X scale value
        /// </summary>
        public double RulerXScale
        {
            get => (double)GetValue(RulerXScaleProperty);
            set => SetValue(RulerXScaleProperty, value);
        }

        /// <summary>
        /// Ruler Y length
        /// </summary>
        public double RulerYLength
        {
            get => (double)GetValue(RulerYLengthProperty);
            set => SetValue(RulerYLengthProperty, value);
        }

        /// <summary>
        /// Ruler Y scale value
        /// </summary>
        public double RulerYScale
        {
            get => (double)GetValue(RulerYScaleProperty);
            set => SetValue(RulerYScaleProperty, value);
        }

        /// <summary>
        /// Show rulers
        /// </summary>
        public bool ShowRulers
        {
            get => (bool)GetValue(ShowRulersProperty);
            set => SetValue(ShowRulersProperty, value);
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// X length for ruler
        /// </summary>
        public static readonly DependencyProperty RulerXLengthProperty = DependencyProperty.Register("RulerXLength", typeof(double), typeof(ScrollRuler), new PropertyMetadata(0D));

        /// <summary>
        /// Y length for ruler
        /// </summary>
        public static readonly DependencyProperty RulerYLengthProperty = DependencyProperty.Register("RulerYLength", typeof(double), typeof(ScrollRuler), new PropertyMetadata(0D));

        /// <summary>
        /// X scale for ruler
        /// </summary>
        public static readonly DependencyProperty RulerXScaleProperty = DependencyProperty.Register("RulerXScale", typeof(double), typeof(ScrollRuler), new PropertyMetadata(1D));

        /// <summary>
        /// Y scale for ruler
        /// </summary>
        public static readonly DependencyProperty RulerYScaleProperty = DependencyProperty.Register("RulerYScale", typeof(double), typeof(ScrollRuler), new PropertyMetadata(1D));

        /// <summary>
        /// Property for show rulers
        /// </summary>
        public static readonly DependencyProperty ShowRulersProperty = DependencyProperty.Register("ShowRulers", typeof(bool), typeof(ScrollRuler), new PropertyMetadata(false));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ScrollRuler"/> class.
        /// </summary>
        static ScrollRuler()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollRuler), new FrameworkPropertyMetadata(typeof(ScrollRuler)));
        }

        #endregion

        #region Events handlers

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ScrollChanged += ScrollRulerScrollChanged;
            _horizontalRulerScrollViewer = GetTemplateChild("PART_HorizontalRulerScrollViewer") as ScrollViewer;
            _verticalRulerScrollViewer = GetTemplateChild("PART_VerticalRulerScrollViewer") as ScrollViewer;
        }

        private void ScrollRulerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _horizontalRulerScrollViewer?.ScrollToHorizontalOffset(HorizontalOffset);
            _verticalRulerScrollViewer?.ScrollToHorizontalOffset(VerticalOffset);
        }

        #endregion
    }
}
