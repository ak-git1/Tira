using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Tira.App.Logic.Enums;

namespace Tira.App.UserControls
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        #region Variables

        /// <summary>
        /// Cursor hand pressed
        /// </summary>
        private Cursor _cursorHandPressed;

        /// <summary>
        /// Cursor hand released
        /// </summary>
        private Cursor _cursorHandReleased;

        /// <summary>
        /// Adorner layer
        /// </summary>
        private AdornerLayer _adornerLayer;

        /// <summary>
        /// The last center position on target
        /// </summary>
        private Point? _lastCenterPositionOnTarget;

        /// <summary>
        /// The last mouse position on target
        /// </summary>
        private Point? _lastMousePositionOnTarget;

        /// <summary>
        /// Previous position
        /// </summary>
        private Point _previousPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Adorner
        /// </summary>
        public object Adorner
        {
            get => GetValue(AdornerProperty);
            set => SetValue(AdornerProperty, value);
        }

        /// <summary>
        /// Adorner template
        /// </summary>
        public DataTemplate AdornerTemplate
        {
            get => (DataTemplate)GetValue(AdornerTemplateProperty);
            set => SetValue(AdornerTemplateProperty, value);
        }

        /// <summary>
        /// Adorner template selector
        /// </summary>
        public DataTemplateSelector AdornerTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(AdornerTemplateSelectorProperty);
            set => SetValue(AdornerTemplateSelectorProperty, value);
        }

        /// <summary>
        /// Image
        /// </summary>
        public BitmapSource BitmapSource
        {
            get => (BitmapSource)GetValue(BitmapSourceProperty);
            set => SetValue(BitmapSourceProperty, value);
        }

        public Effect EffectImageOnly
        {
            get => (Effect)GetValue(EffectImageOnlyProperty);
            set => SetValue(EffectImageOnlyProperty, value);
        }

        public FitOption FitMode
        {
            get => (FitOption)GetValue(FitModeProperty);
            set => SetValue(FitModeProperty, value);
        }

        /// <summary>
        /// Fitted scale
        /// </summary>
        public double FittedScale => (double)GetValue(FittedScaleProperty);

        /// <summary>
        /// Enable mouse wheel handling
        /// </summary>
        public bool HandleMouseWheel
        {
            get => (bool)GetValue(HandleMouseWheelProperty);
            set => SetValue(HandleMouseWheelProperty, value);
        }

        /// <summary>
        /// Iimage border brush
        /// </summary>
        public Brush ImageBorderBrush
        {
            get => (Brush)GetValue(ImageBorderBrushProperty);
            set => SetValue(ImageBorderBrushProperty, value);
        }

        /// <summary>
        /// Image border thickness
        /// </summary>
        public Thickness ImageBorderThickness
        {
            get => (Thickness)GetValue(ImageBorderThicknessProperty);
            set => SetValue(ImageBorderThicknessProperty, value);
        }

        /// <summary>
        /// Image height
        /// </summary>
        public double ImageHeight => (double)GetValue(ImageHeightProperty);

        /// <summary>
        /// Image width
        /// </summary>
        public double ImageWidth => (double)GetValue(ImageWidthProperty);

        /// <summary>
        /// Enable reset to fit size when bitmap source changed
        /// </summary>
        public bool ResetToFitSizeWhenBitmapSourceChanged
        {
            get => (bool)GetValue(ResetToFitSizeWhenBitmapSourceChangedProperty);
            set => SetValue(ResetToFitSizeWhenBitmapSourceChangedProperty, value);
        }

        /// <summary>
        /// Scale
        /// </summary>
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        /// <summary>
        /// Scroll viewer
        /// </summary>
        public ScrollViewer ScrollViewer => ScrollViewerElement;

        /// <summary>
        /// Show rulers
        /// </summary>
        public bool ShowRulers
        {
            get => (bool)GetValue(ShowRulersProperty);
            set => SetValue(ShowRulersProperty, value);
        }

        /// <summary>
        /// Cursor hand pressed
        /// </summary>
        private Cursor CursorHandPressed => _cursorHandPressed ?? (_cursorHandPressed = ((FrameworkElement)Resources["CursorHandPressed"]).Cursor);

        /// <summary>
        /// Cursor hand released
        /// </summary>
        private Cursor CursorHandReleased => _cursorHandReleased ?? (_cursorHandReleased = ((FrameworkElement)Resources["CursorHandReleased"]).Cursor);

        private AdornerLayer AdornerLayer
        {
            get
            {
                AdornerLayer adornerLayer = _adornerLayer;
                if (adornerLayer == null)
                {
                    AdornerLayer adornerLayer1 = AdornerLayer.GetAdornerLayer(ImageElement);
                    AdornerLayer adornerLayer2 = adornerLayer1;
                    _adornerLayer = adornerLayer1;
                    adornerLayer = adornerLayer2;
                }
                return adornerLayer;
            }
        }

        #endregion

        #region Dependency properties

        /// <summary>
        /// Adorner property
        /// </summary>
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.Register("Adorner", typeof(object), typeof(ImageViewer), new UIPropertyMetadata(null, (source, args) =>
        {
            ImageViewer imageViewer = source as ImageViewer;
            if (imageViewer == null)
                return;

            imageViewer.EraseAdorners();
            object newValue = args.NewValue;
            if (newValue == null)
                return;

            imageViewer.DrawAdorner(newValue);
        }));

        /// <summary>
        /// Adorner template property
        /// </summary>
        public static readonly DependencyProperty AdornerTemplateProperty = DependencyProperty.Register("AdornerTemplate", typeof(DataTemplate), typeof(ImageViewer), new UIPropertyMetadata(null));

        /// <summary>
        /// Adorner template selector property
        /// </summary>
        public static readonly DependencyProperty AdornerTemplateSelectorProperty = DependencyProperty.Register("AdornerTemplateSelector", typeof(DataTemplateSelector), typeof(ImageViewer), new UIPropertyMetadata(null));

        /// <summary>
        /// BitmapSource property
        /// </summary>
        public static readonly DependencyProperty BitmapSourceProperty = DependencyProperty.Register("BitmapSource", typeof(BitmapSource), typeof(ImageViewer), new UIPropertyMetadata(null, (source, args) =>
        {
            ImageViewer imageViewer = source as ImageViewer;
            if (imageViewer == null)
                return;
            
            BitmapSource bitmapSource = args.NewValue as BitmapSource;
            if (bitmapSource == null)
                return;

            imageViewer.SetValue(ImageHeightPropertyKey, (double)bitmapSource.PixelHeight);
            imageViewer.SetValue(ImageWidthPropertyKey, (double)bitmapSource.PixelWidth);
            imageViewer.SetValue(FittedScalePropertyKey, imageViewer.GetFittedScale());
            if (!imageViewer.ResetToFitSizeWhenBitmapSourceChanged)
                return;

            imageViewer.FitMode = FitOption.FitSize;
        }));

        /// <summary>
        /// Scroll changed event
        /// </summary>
        public static readonly RoutedEvent ScrollChangedEvent = EventManager.RegisterRoutedEvent("ScrollChanged", RoutingStrategy.Bubble, typeof(ScrollChangedEventHandler), typeof(ImageViewer));

        /// <summary>
        /// Effect image only property
        /// </summary>
        public static readonly DependencyProperty EffectImageOnlyProperty = DependencyProperty.Register("EffectImageOnly", typeof(Effect), typeof(ImageViewer), new UIPropertyMetadata());

        /// <summary>
        /// Reset to fit size when bitmap source changed property
        /// </summary>
        public static readonly DependencyProperty ResetToFitSizeWhenBitmapSourceChangedProperty = DependencyProperty.Register("ResetToFitSizeWhenBitmapSourceChanged", typeof(bool), typeof(ImageViewer), new PropertyMetadata(false));

        /// <summary>
        /// Handle mouse wheel property
        /// </summary>
        public static readonly DependencyProperty HandleMouseWheelProperty = DependencyProperty.Register("HandleMouseWheel", typeof(bool), typeof(ImageViewer), new PropertyMetadata(true));

        /// <summary>
        /// Image width property key
        /// </summary>
        public static readonly DependencyPropertyKey ImageWidthPropertyKey = DependencyProperty.RegisterReadOnly("ImageWidth", typeof(double), typeof(ImageViewer), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Image width property
        /// </summary>
        public static readonly DependencyProperty ImageWidthProperty = ImageWidthPropertyKey.DependencyProperty;

        /// <summary>
        /// Image height property key
        /// </summary>
        public static readonly DependencyPropertyKey ImageHeightPropertyKey = DependencyProperty.RegisterReadOnly("ImageHeight", typeof(double), typeof(ImageViewer), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Image height property
        /// </summary>
        public static readonly DependencyProperty ImageHeightProperty = ImageHeightPropertyKey.DependencyProperty;

        /// <summary>
        /// Scale property
        /// </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(ImageViewer), new UIPropertyMetadata(1.0, new PropertyChangedCallback(ScalePropertyChangedCallBack)));

        /// <summary>
        /// Fit mode property
        /// </summary>
        public static readonly DependencyProperty FitModeProperty = DependencyProperty.Register("FitMode", typeof(FitOption), typeof(ImageViewer), new UIPropertyMetadata(FitOption.FitSize, (source, args) =>
        {
            ImageViewer imageViewer = source as ImageViewer;
            if (imageViewer == null)
                return;

            imageViewer.SetValue(FittedScalePropertyKey, imageViewer.GetFittedScale());
        }));

        /// <summary>
        /// Fitted scale property key
        /// </summary>
        public static readonly DependencyPropertyKey FittedScalePropertyKey = DependencyProperty.RegisterReadOnly("FittedScale", typeof(double), typeof(ImageViewer), new UIPropertyMetadata(1.0, (source, args) =>
        {
            ImageViewer imageViewer = source as ImageViewer;
            if (imageViewer == null || imageViewer.FitMode == FitOption.None)
                return;
            imageViewer.Scale = (double)args.NewValue;
        }));

        /// <summary>
        /// Fitted scale property
        /// </summary>
        public static readonly DependencyProperty FittedScaleProperty = FittedScalePropertyKey.DependencyProperty;

        /// <summary>
        /// Image border thickness property
        /// </summary>
        public static readonly DependencyProperty ImageBorderThicknessProperty = DependencyProperty.Register("ImageBorderThickness", typeof(Thickness), typeof(ImageViewer), new PropertyMetadata(new Thickness()));

        /// <summary>
        /// Image border brush property
        /// </summary>
        public static readonly DependencyProperty ImageBorderBrushProperty = DependencyProperty.Register("ImageBorderBrush", typeof(Brush), typeof(ImageViewer), new PropertyMetadata((object)null));

        /// <summary>
        /// Show rulers property
        /// </summary>
        public static readonly DependencyProperty ShowRulersProperty = DependencyProperty.Register("ShowRulers", typeof(bool), typeof(ImageViewer), new PropertyMetadata(false));

        #endregion

        #region Events

        /// <summary>
        /// Event for scroll changed
        /// </summary>
        public event RoutedEventHandler ScrollChanged
        {
            add => AddHandler(ScrollChangedEvent, value);
            remove => RemoveHandler(ScrollChangedEvent, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageViewer"/> class.
        /// </summary>
        public ImageViewer()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets mouse position on image
        /// </summary>
        /// <returns></returns>
        public Point GetMousePositionOnImage()
        {
            return Mouse.GetPosition(ImageElement);
        }

        #endregion

        #region Private methods

        private void DrawAdorner(object adorner)
        {
            ContentControl contentControl = new ContentControl();
            DrawAdorner(adorner, contentControl, AdornerTemplateSelector != null ? AdornerTemplateSelector.SelectTemplate(adorner, contentControl) : AdornerTemplate);
        }

        private void DrawAdorner(object model, ContentControl view, DataTemplate template)
        {
            if (template != null && model != null)
            {
                TemplatedAdorner templatedAdorner = new TemplatedAdorner(ImageElement, view);
                AdornerLayer.Add(templatedAdorner);
                view.Content = model;
                view.ContentTemplate = template;
                view.Visibility = Visibility.Visible;
                view.Margin = new Thickness(0);
            }
        }

        private void EraseAdorner(Adorner adorner)
        {
            BindingOperations.ClearAllBindings(adorner);
            _adornerLayer.Remove(adorner);
        }

        private void EraseAdorners()
        {
            Adorner[] adorners = AdornerLayer.GetAdorners(ImageElement);
            if (adorners != null)
            {
                Adorner[] adornerArray = adorners;
                for (int i = 0; i < adornerArray.Length; i++)
                    EraseAdorner(adornerArray[i]);
            }
        }

        /// <summary>
        /// Scrolls to zoom position
        /// </summary>
        private void ScrollToZoomPosition()
        {
            Point? lastCenterPositionOnTarget = null;
            Point? position = null;
            if (_lastMousePositionOnTarget.HasValue)
            {
                lastCenterPositionOnTarget = _lastMousePositionOnTarget;
                position = Mouse.GetPosition(ImageElement);
                _lastMousePositionOnTarget = null;
            }
            else if (_lastCenterPositionOnTarget.HasValue)
            {
                Point point = new Point(ScrollViewerElement.ViewportWidth / 2, ScrollViewerElement.ViewportHeight / 2);
                Point point1 = ScrollViewerElement.TranslatePoint(point, ImageElement);
                lastCenterPositionOnTarget = _lastCenterPositionOnTarget;
                position = point1;
            }
            if (lastCenterPositionOnTarget.HasValue)
            {
                Point value = position.Value;
                double x = value.X;
                value = lastCenterPositionOnTarget.Value;
                double num = x - value.X;
                value = position.Value;
                double y = value.Y;
                value = lastCenterPositionOnTarget.Value;
                double y1 = y - value.Y;
                double extentWidth = ScrollViewerElement.ExtentWidth / ImageElement.ActualWidth;
                double extentHeight = ScrollViewerElement.ExtentHeight / ImageElement.ActualHeight;
                double horizontalOffset = ScrollViewerElement.HorizontalOffset - num * extentWidth;
                double verticalOffset = ScrollViewerElement.VerticalOffset - y1 * extentHeight;
                if (!double.IsNaN(horizontalOffset) && !double.IsNaN(verticalOffset))
                {
                    ScrollViewerElement.ScrollToHorizontalOffset(horizontalOffset);
                    ScrollViewerElement.ScrollToVerticalOffset(verticalOffset);
                }
            }
        }

        /// <summary>
        /// Zooms the in or zoom out image.
        /// </summary>
        /// <param name="delta">Delta</param>
        private void ZoomInOrZoomOutImage(int delta)
        {
            Scale = Scale * (delta > 0 ? 1.1 : 0.9);
        }

        private double GetFittedHeightScale()
        {
            double viewportHeight = ScrollViewerElement.ViewportHeight;
            double imageHeight = ImageHeight;
            Thickness imageBorderThickness = ImageBorderThickness;
            double top = imageHeight + imageBorderThickness.Top;
            imageBorderThickness = ImageBorderThickness;
            return viewportHeight / (top + imageBorderThickness.Bottom);
        }

        /// <summary>
        /// Gets the fitted scale
        /// </summary>
        /// <returns></returns>
        private double GetFittedScale()
        {
            double scale;
            if (!((int)ImageWidth != 0 && (int)ImageHeight != 0))
            {
                scale = 1;
            }
            else if (!ActualHeight.Equals(double.NaN) && !ActualWidth.Equals(double.NaN))
            {
                switch (FitMode)
                {
                    case FitOption.None:
                        scale = Scale;
                        return scale;

                    case FitOption.FitSize:
                        scale = Math.Min(GetFittedWidthScale(), GetFittedHeightScale());
                        return scale;

                    case FitOption.FitWidth:
                        scale = GetFittedWidthScale();
                        return scale;

                    case FitOption.FitHeight:
                        scale = GetFittedHeightScale();
                        return scale;
                }
                throw new NotSupportedException("Unsupported fit mode");
            }
            else
            {
                scale = 1;
            }
            return scale;
        }

        /// <summary>
        /// Gets the fitted width scale
        /// </summary>
        /// <returns></returns>
        private double GetFittedWidthScale()
        {
            double viewportWidth = ScrollViewerElement.ViewportWidth;
            double imageWidth = ImageWidth;
            Thickness imageBorderThickness = ImageBorderThickness;
            double left = imageWidth + imageBorderThickness.Left;
            imageBorderThickness = ImageBorderThickness;
            return viewportWidth / (left + imageBorderThickness.Right);
        }

        /// <summary>
        /// Redraws adorners
        /// </summary>
        private void RedrawAdorners()
        {
            _adornerLayer = _adornerLayer ?? AdornerLayer.GetAdornerLayer(ImageElement);
        }

        #endregion

        #region Events handlers

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!ImageElement.IsMouseCaptured)
                Cursor = ScrollViewerElement.ScrollableHeight > 0 || ScrollViewerElement.ScrollableWidth > 0 ? CursorHandReleased : Cursors.Arrow;
            
            SetValue(FittedScalePropertyKey, GetFittedScale());
            if (!(Math.Abs(e.ExtentHeightChange) <= 0) && Math.Abs(e.ExtentWidthChange) > 0)
                ScrollToZoomPosition();
            RaiseEvent(new RoutedEventArgs(ScrollChangedEvent, ScrollViewerElement));
        }

        private void AdornerDecorator_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageElement.CaptureMouse();
            _previousPosition = e.GetPosition(this);
        }

        private void AdornerDecorator_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Cursor = Cursor == CursorHandPressed ? CursorHandReleased : Cursors.Arrow;
            ImageElement.ReleaseMouseCapture();
        }

        private void AdornerDecorator_OnMouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ScrollViewerElement.ScrollToVerticalOffset(ScrollViewerElement.ContentVerticalOffset - (position.Y - _previousPosition.Y) * 1);
                ScrollViewerElement.ScrollToHorizontalOffset(ScrollViewerElement.ContentHorizontalOffset - (position.X - _previousPosition.X) * 1);
            }
            _previousPosition = position;
        }

        private void ImageViewerControl_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.Add:
                        ZoomInOrZoomOutImage(1);
                        break;

                    case Key.Subtract:
                        ZoomInOrZoomOutImage(-1);
                        break;

                    case Key.H:
                        FitMode = FitOption.FitSize;
                        break;
                }
            }
        }

        private void ImageViewerControl_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _lastMousePositionOnTarget = e.GetPosition(ImageElement);
            if (HandleMouseWheel)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                    ZoomInOrZoomOutImage(e.Delta);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _adornerLayer = AdornerLayer.GetAdornerLayer(ImageElement);
        }

        private static void ScalePropertyChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ImageViewer imageViewer = sender as ImageViewer;
            if (imageViewer != null)
            {
                imageViewer.RedrawAdorners();
                if (imageViewer.FitMode == FitOption.None ? false : imageViewer.Scale != imageViewer.FittedScale)
                    imageViewer.FitMode = FitOption.None;
                
                imageViewer.RedrawAdorners();
                ScrollRuler scrollRuler = imageViewer.ScrollViewerElement;
                Point point = new Point(scrollRuler.ViewportWidth / 2, scrollRuler.ViewportHeight / 2);
                Point lastCenterPositionOnTarget = scrollRuler.TranslatePoint(point, imageViewer.ImageElement);
                imageViewer._lastCenterPositionOnTarget = lastCenterPositionOnTarget;
            }
        }

        #endregion
    }
}