using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tira.App.Logic.Enums;
using Tira.App.Logic.Events;
using Tira.Logic.Models.Markup;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Tira.App.UserControls
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        #region Variables and constants

        #region Styles

        /// <summary>
        /// The fixed rectangle area style
        /// </summary>
        private const string FixedRectangleAreaStyle = "FixedRectangleAreaStyle";

        /// <summary>
        /// The drawing rectangle area style
        /// </summary>
        private const string DrawingRectangleAreaStyle = "DrawingRectangleAreaStyle";

        /// <summary>
        /// The fixed line style
        /// </summary>
        private const string FixedLineStyle = "FixedLineStyle";

        /// <summary>
        /// The drawing line style
        /// </summary>
        private const string DrawingLineStyle = "DrawingLineStyle";

        /// <summary>
        /// The crop rectangle area style
        /// </summary>
        private const string CropRectangleAreaStyle = "CropRectangleAreaStyle";

        #endregion

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

        /// <summary>
        /// The drawing is in progress
        /// </summary>
        private bool _isDrawingInProgress;

        /// <summary>
        /// The drawing start point
        /// </summary>
        private Point _drawingStartPoint;

        /// <summary>
        /// The drawing end point
        /// </summary>
        private Point _drawingEndPoint;

        /// <summary>
        /// The line click search area
        /// </summary>
        private const int LineClickSearchArea = 5;

        /// <summary>
        /// Selected Vertical line index
        /// </summary>
        private int? _selectedVerticalLineIndex;

        /// <summary>
        /// Selected horizontal line index
        /// </summary>
        private int? _selectedHorizontalLineIndex;

        /// <summary>
        /// Temporary rectangle
        /// </summary>
        private System.Windows.Shapes.Rectangle _tempRectangle;

        /// <summary>
        /// Temporary line
        /// </summary>
        private Line _tempLine;

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
        /// Adorner layer
        /// </summary>
        private AdornerLayer AdornerLayer
        {
            get
            {
                AdornerLayer adornerLayer = _adornerLayer;
                if (adornerLayer == null)
                {
                    AdornerLayer adornerLayer1 = AdornerLayer.GetAdornerLayer(ImageCanvas);
                    AdornerLayer adornerLayer2 = adornerLayer1;
                    _adornerLayer = adornerLayer1;
                    adornerLayer = adornerLayer2;
                }
                return adornerLayer;
            }
        }

        /// <summary>
        /// Current image viewer mode
        /// </summary>
        public ImageViewerMode CurrentImageViewerMode
        {
            get => (ImageViewerMode)GetValue(CurrentImageViewerModeProperty);
            set => SetValue(CurrentImageViewerModeProperty, value);
        }

        /// <summary>
        /// Current drawing object type
        /// </summary>
        public MarkupObjectType CurrentMarkupObjectType
        {
            get => (MarkupObjectType)GetValue(CurrentMarkupObjectTypeProperty);
            set => SetValue(CurrentMarkupObjectTypeProperty, value);
        }

        /// <summary>
        /// Markup drawing objects
        /// </summary>
        public MarkupObjects CurrentMarkupObjects
        {
            get => (MarkupObjects)GetValue(CurrentMarkupObjectsProperty);
            set => SetValue(CurrentMarkupObjectsProperty, value);
        }

        /// <summary>
        /// Index of the highlighted row
        /// </summary>
        public int? HighlightedRowIndex
        {
            get => (int?)GetValue(HighlightedRowIndexProperty);
            set => SetValue(HighlightedRowIndexProperty, value);
        }

        #region Cursors

        /// <summary>
        /// Cursor hand pressed
        /// </summary>
        private Cursor CursorHandPressed => ((FrameworkElement)Resources["CursorHandPressed"]).Cursor;

        /// <summary>
        /// Cursor hand released
        /// </summary>
        private Cursor CursorHandReleased => ((FrameworkElement)Resources["CursorHandReleased"]).Cursor;

        #endregion

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

        /// <summary>
        /// Current image viewer mode
        /// </summary>
        public static readonly DependencyProperty CurrentImageViewerModeProperty = DependencyProperty.Register("CurrentImageViewerMode", typeof(ImageViewerMode), typeof(ImageViewer), new UIPropertyMetadata(ImageViewerMode.None, (source, args) =>
        {
            ImageViewer imageViewer = source as ImageViewer;
            if (imageViewer == null)
                return;

            imageViewer.DrawMarkupObjects();
        }));

        /// <summary>
        /// Current drawing object type
        /// </summary>
        public static readonly DependencyProperty CurrentMarkupObjectTypeProperty = DependencyProperty.Register("CurrentMarkupObjectType", typeof(MarkupObjectType), typeof(ImageViewer), new UIPropertyMetadata(MarkupObjectType.None));

        /// <summary>
        /// Markup drawing objects
        /// </summary>
        public static readonly DependencyProperty CurrentMarkupObjectsProperty = DependencyProperty.Register("CurrentMarkupObjects", typeof(MarkupObjects), typeof(ImageViewer), new UIPropertyMetadata(null, (source, args) =>
        {
            ImageViewer imageViewer = source as ImageViewer;
            if (imageViewer == null)
                return;

            imageViewer.DrawMarkupObjects();
        }));

        /// <summary>
        /// Index of the highlighted row
        /// </summary>
        public static readonly DependencyProperty HighlightedRowIndexProperty = DependencyProperty.Register("HighlightedRowIndex", typeof(int), typeof(ImageViewer), new UIPropertyMetadata(null));

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for markup objects changed event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MarkupObjectsEventArgs"/> instance containing the event data.</param>
        public delegate void MarkupObjectsChangedHandler(object sender, MarkupObjectsEventArgs e);

        /// <summary>
        /// Delegate for crop area selected
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MarkupObjectsEventArgs"/> instance containing the event data.</param>
        public delegate void CropAreaSelectedHandler(object sender, RectangleAreaEventArgs e);

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

        /// <summary>
        /// Markup drawing objects changed event
        /// </summary>
        public event MarkupObjectsChangedHandler MarkupObjectsChanged;

        /// <summary>
        /// Crop area selected event
        /// </summary>
        public event CropAreaSelectedHandler CropAreaSelected;

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

        #region Private methods

        #region Adorners methods

        /// <summary>
        /// Draws adorner
        /// </summary>
        /// <param name="adorner">Adorner</param>
        private void DrawAdorner(object adorner)
        {
            ContentControl contentControl = new ContentControl();
            DrawAdorner(adorner, contentControl, AdornerTemplateSelector != null ? AdornerTemplateSelector.SelectTemplate(adorner, contentControl) : AdornerTemplate);
        }

        /// <summary>
        /// Draws adorner
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="view">View</param>
        /// <param name="template">Template</param>
        private void DrawAdorner(object model, ContentControl view, DataTemplate template)
        {
            if (template != null && model != null)
            {
                TemplatedAdorner templatedAdorner = new TemplatedAdorner(ImageCanvas, view);
                AdornerLayer.Add(templatedAdorner);
                view.Content = model;
                view.ContentTemplate = template;
                view.Visibility = Visibility.Visible;
                view.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// Erases the adorner
        /// </summary>
        /// <param name="adorner">Adorner.</param>
        private void EraseAdorner(Adorner adorner)
        {
            BindingOperations.ClearAllBindings(adorner);
            _adornerLayer.Remove(adorner);
        }

        /// <summary>
        /// Erases the adorners
        /// </summary>
        private void EraseAdorners()
        {
            Adorner[] adorners = AdornerLayer.GetAdorners(ImageCanvas);
            if (adorners != null)
            {
                Adorner[] adornerArray = adorners;
                for (int i = 0; i < adornerArray.Length; i++)
                    EraseAdorner(adornerArray[i]);
            }
        }

        /// <summary>
        /// Redraws adorners
        /// </summary>
        private void RedrawAdorners()
        {
            _adornerLayer = _adornerLayer ?? AdornerLayer.GetAdornerLayer(ImageCanvas);
        }

        #endregion

        #region Scroll ans zoom methods

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
                position = Mouse.GetPosition(ImageCanvas);
                _lastMousePositionOnTarget = null;
            }
            else if (_lastCenterPositionOnTarget.HasValue)
            {
                Point point = new Point(ScrollViewerElement.ViewportWidth / 2, ScrollViewerElement.ViewportHeight / 2);
                Point point1 = ScrollViewerElement.TranslatePoint(point, ImageCanvas);
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
                double extentWidth = ScrollViewerElement.ExtentWidth / ImageCanvas.ActualWidth;
                double extentHeight = ScrollViewerElement.ExtentHeight / ImageCanvas.ActualHeight;
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

        #endregion

        #region Markup methods

        /// <summary>
        /// Clears drawing area
        /// </summary>
        public void ClearDrawingArea()
        {
            ImageCanvas.Children.Clear();
        }

        /// <summary>
        /// Determines whether value in delta area.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="center">Center.</param>
        /// <param name="delta">Delta</param>
        /// <returns></returns>
        private bool IsValueInDeltaArea(double value, int center, int delta)
        {
            return IsValueInDeltaArea((int)value, center, delta);
        }

        /// <summary>
        /// Determines whether value in delta area.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="center">Center.</param>
        /// <param name="delta">Delta</param>
        /// <returns></returns>
        private bool IsValueInDeltaArea(int value, int center, int delta)
        {
            return center - delta <= value && value <= center + delta;
        }

        /// <summary>
        /// Determines whether point is in vertical line area
        /// </summary>
        /// <param name="p">Point</param>
        /// <param name="lineIndex">Index of the line.</param>
        /// <returns></returns>
        private bool IsPointInVerticalLineArea(Point p, out int? lineIndex)
        {
            lineIndex = null;

            if (CurrentMarkupObjects.RectangleArea != Rectangle.Empty && CurrentMarkupObjects.RectangleArea.Contains((int)p.X, (int)p.Y))
            {
                for (int i = 0; i < CurrentMarkupObjects.VerticalLinesCoordinates.Count; i++)
                {
                    if (IsValueInDeltaArea(p.X, CurrentMarkupObjects.VerticalLinesCoordinates[i], LineClickSearchArea))
                    {
                        lineIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether point is in horizontal line area
        /// </summary>
        /// <param name="p">Point</param>
        /// <param name="lineIndex">Index of the line.</param>
        /// <returns></returns>
        private bool IsPointInHorizontalLineArea(Point p, out int? lineIndex)
        {
            lineIndex = null;

            if (CurrentMarkupObjects.RectangleArea != Rectangle.Empty && CurrentMarkupObjects.RectangleArea.Contains((int)p.X, (int)p.Y))
            {
                for (int i = 0; i < CurrentMarkupObjects.HorizontalLinesCoordinates.Count; i++)
                {
                    if (IsValueInDeltaArea(p.Y, CurrentMarkupObjects.HorizontalLinesCoordinates[i], LineClickSearchArea))
                    {
                        lineIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Clears the drawing temp objects like temp points, line indexes etc
        /// </summary>
        private void ClearDrawingTempObjects()
        {
            _isDrawingInProgress = false;
            _drawingStartPoint = new Point(0, 0);
            _drawingEndPoint = new Point(0, 0);
            _selectedVerticalLineIndex = null;
            _selectedHorizontalLineIndex = null;
            _tempRectangle = null;
            _tempLine = null;
        }

        /// <summary>
        /// Creates the rectangle by two points.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <returns></returns>
        private Rectangle CreateRectangleByTwoPoints(Point startPoint, Point endPoint)
        {
            return new Rectangle
            {
                Location = new System.Drawing.Point((int)Math.Min(startPoint.X, endPoint.X), (int)Math.Min(startPoint.Y, endPoint.Y)),
                Size = new System.Drawing.Size((int)Math.Abs(startPoint.X - endPoint.X), (int)Math.Abs(startPoint.Y - endPoint.Y))
            };
        }

        /// <summary>
        /// Checks the point is in image area.
        /// </summary>
        /// <param name="p">Point</param>
        /// <returns></returns>
        private bool CheckPointIsInImageArea(Point p)
        {            
            if (new Rectangle(0, 0, (int)ImageCanvas.Width, (int)ImageCanvas.Height).Contains((int)p.X, (int)p.Y))
                return true;
            else
            {
                ClearDrawingTempObjects();
                return false;
            }
        }

        /// <summary>
        /// Draws markup objects.
        /// </summary>
        private void DrawMarkupObjects()
        {
            ClearDrawingArea();

            if (CurrentMarkupObjects != null && CurrentImageViewerMode == ImageViewerMode.Markup)
            {
                DrawMarkupRectangle(CurrentMarkupObjects.RectangleArea, FixedRectangleAreaStyle);
                foreach (int x in CurrentMarkupObjects.VerticalLinesCoordinates)
                    DrawMarkupVerticalLine(x, FixedLineStyle);
                foreach (int y in CurrentMarkupObjects.HorizontalLinesCoordinates)
                    DrawMarkupHorizontalLine(y, FixedLineStyle);
            }
        }
        
        /// <summary>
        /// Draws markup rectangle
        /// </summary>
        /// <param name="r">Rectangle area</param>
        /// <param name="style">Style</param>
        /// <returns></returns>
        private System.Windows.Shapes.Rectangle DrawMarkupRectangle(Rectangle r, string style)
        {
            System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle
            {
                Style = FindResource(style) as Style,
                Width = r.Width,
                Height = r.Height
            };
            Canvas.SetLeft(rectangle, r.Left);
            Canvas.SetTop(rectangle, r.Top);
            ImageCanvas.Children.Add(rectangle);

            return rectangle;
        }

        /// <summary>
        /// Draws the markup vertical line.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="style">Style</param>
        /// <returns></returns>
        private Line DrawMarkupVerticalLine(int x, string style)
        {
            Line line = new Line
            {
                Style = FindResource(style) as Style,
                X1 = x,
                X2 = x,
                Y1 = CurrentMarkupObjects.RectangleArea.Top,
                Y2 = CurrentMarkupObjects.RectangleArea.Bottom
            };
            ImageCanvas.Children.Add(line);

            return line;
        }

        /// <summary>
        /// Draws the markup horizontal line.
        /// </summary>
        /// <param name="y">Y</param>
        /// <param name="style">Style</param>
        /// <returns></returns>
        private Line DrawMarkupHorizontalLine(int y, string style)
        {
            Line line = new Line
            {
                Style = FindResource(style) as Style,
                X1 = CurrentMarkupObjects.RectangleArea.Left,
                X2 = CurrentMarkupObjects.RectangleArea.Right,
                Y1 = y,
                Y2 = y
            };
            ImageCanvas.Children.Add(line);

            return line;
        }

        #endregion

        #endregion

        #region Events handlers

        #region Adorners events

        private void AdornerDecorator_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentImageViewerMode == ImageViewerMode.None)
            {
                Cursor = CursorHandPressed;
                ImageCanvas.CaptureMouse();
                _previousPosition = e.GetPosition(this);
            }
        }

        private void AdornerDecorator_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CurrentImageViewerMode == ImageViewerMode.None)
            {
                Cursor = Cursors.Arrow;
                ImageCanvas.ReleaseMouseCapture();
            }
        }

        private void AdornerDecorator_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentImageViewerMode == ImageViewerMode.None)
            {
                Point position = e.GetPosition(this);
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    ScrollViewerElement.ScrollToVerticalOffset(ScrollViewerElement.ContentVerticalOffset - (position.Y - _previousPosition.Y) * 1);
                    ScrollViewerElement.ScrollToHorizontalOffset(ScrollViewerElement.ContentHorizontalOffset - (position.X - _previousPosition.X) * 1);
                }
                _previousPosition = position;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _adornerLayer = AdornerLayer.GetAdornerLayer(ImageCanvas);
        }

        #endregion

        #region Scroll and zoom methods

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (CurrentImageViewerMode == ImageViewerMode.None)
            {
                SetValue(FittedScalePropertyKey, GetFittedScale());
                if (!(Math.Abs(e.ExtentHeightChange) <= 0) && Math.Abs(e.ExtentWidthChange) > 0)
                    ScrollToZoomPosition();
                RaiseEvent(new RoutedEventArgs(ScrollChangedEvent, ScrollViewerElement));
            }
        }
       
        private void ImageViewerControl_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentImageViewerMode == ImageViewerMode.None)
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
        }

        private void ImageViewerControl_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _lastMousePositionOnTarget = e.GetPosition(ImageCanvas);
            if (HandleMouseWheel)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                    ZoomInOrZoomOutImage(e.Delta);
            }
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
                Point lastCenterPositionOnTarget = scrollRuler.TranslatePoint(point, imageViewer.ImageCanvas);
                imageViewer._lastCenterPositionOnTarget = lastCenterPositionOnTarget;
            }
        }

        #endregion        

        #region Markups events

        private void ImageCanvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearDrawingTempObjects();
            _drawingStartPoint = _drawingEndPoint = e.GetPosition(ImageCanvas);
            if (!CheckPointIsInImageArea(_drawingStartPoint))
                return;

            switch (CurrentImageViewerMode)
            {
                #region ImageViewerMode.None

                case ImageViewerMode.None:
                    return;

                #endregion

                #region ImageViewerMode.Markup

                case ImageViewerMode.Markup:
                    switch (CurrentMarkupObjectType)
                    {
                        case MarkupObjectType.Rectangle:
                            CurrentMarkupObjects.Clear();
                            ClearDrawingArea();
                            _isDrawingInProgress = true;
                            break;

                        case MarkupObjectType.VerticalLine:
                            if (IsPointInVerticalLineArea(_drawingStartPoint, out _selectedVerticalLineIndex))
                            {
                                _isDrawingInProgress = true;
                            }
                            else if (CurrentMarkupObjects.VerticalLinesCoordinates.Count < CurrentMarkupObjects.MaxNumberOfVerticalLines)
                            {
                                _isDrawingInProgress = true;
                            }
                            break;

                        case MarkupObjectType.HorizontalLine:
                            if (IsPointInHorizontalLineArea(_drawingStartPoint, out _selectedHorizontalLineIndex))
                            {
                                _isDrawingInProgress = true;
                            }
                            else
                            {
                                _isDrawingInProgress = true;
                            }
                            break;
                    }
                    break;

                #endregion

                #region ImageViewerMode.Crop

                case ImageViewerMode.Crop:
                    CurrentMarkupObjects.Clear();
                    ClearDrawingArea();
                    _isDrawingInProgress = true;
                    break;

                #endregion
            }                        
        }

        private void ImageCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (BitmapSource == null || !_isDrawingInProgress || e.LeftButton != MouseButtonState.Pressed)
                return;

            _drawingEndPoint = e.GetPosition(ImageCanvas);

            switch (CurrentImageViewerMode)
            {
                #region ImageViewerMode.Markup

                case ImageViewerMode.Markup:
                    switch (CurrentMarkupObjectType)
                    {
                        case MarkupObjectType.Rectangle:
                            Rectangle rectangleArea = CreateRectangleByTwoPoints(_drawingStartPoint, _drawingEndPoint);
                            if (_tempRectangle != null)
                                ImageCanvas.Children.Remove(_tempRectangle);

                            _tempRectangle = DrawMarkupRectangle(rectangleArea, DrawingRectangleAreaStyle);
                            break;

                        case MarkupObjectType.VerticalLine:
                            if (_tempLine == null)
                                _tempLine = DrawMarkupVerticalLine((int) _drawingEndPoint.X, DrawingLineStyle);
                            else
                            {
                                _tempLine.X1 = _drawingEndPoint.X;
                                _tempLine.X2 = _drawingEndPoint.X;
                            }
                            break;

                        case MarkupObjectType.HorizontalLine:
                            if (_tempLine == null)
                                _tempLine = DrawMarkupHorizontalLine((int)_drawingEndPoint.Y, DrawingLineStyle);
                            else    
                            {
                                _tempLine.Y1 = _drawingEndPoint.Y;
                                _tempLine.Y2 = _drawingEndPoint.Y;
                            }
                            break;
                    }
                    break;

                #endregion

                #region ImageViewerMode.Crop

                case ImageViewerMode.Crop:
                    Rectangle cropArea = CreateRectangleByTwoPoints(_drawingStartPoint, _drawingEndPoint);
                    if (_tempRectangle != null)
                        ImageCanvas.Children.Remove(_tempRectangle);

                    _tempRectangle = DrawMarkupRectangle(cropArea, CropRectangleAreaStyle);
                    break;

                #endregion
            }
        }

        private void ImageCanvas_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (BitmapSource == null || !_isDrawingInProgress || e.LeftButton != MouseButtonState.Released)
                return;

            if (!CheckPointIsInImageArea(_drawingEndPoint))
                return;

            switch (CurrentImageViewerMode)
            {
                #region ImageViewerMode.Markup

                case ImageViewerMode.Markup:
                    switch (CurrentMarkupObjectType)
                    {
                        case MarkupObjectType.Rectangle:
                            ImageCanvas.Children.Remove(_tempRectangle);
                            CurrentMarkupObjects.RectangleArea = CreateRectangleByTwoPoints(_drawingStartPoint, _drawingEndPoint);
                            DrawMarkupRectangle(CurrentMarkupObjects.RectangleArea, FixedRectangleAreaStyle);
                            MarkupObjectsChanged?.Invoke(this, new MarkupObjectsEventArgs(CurrentMarkupObjects));
                            break;

                        case MarkupObjectType.VerticalLine:
                            ImageCanvas.Children.Remove(_tempLine);

                            if (CurrentMarkupObjects.RectangleArea.Contains((int)_drawingEndPoint.X, CurrentMarkupObjects.RectangleArea.Top))
                            { 
                                if (_selectedVerticalLineIndex.HasValue)
                                    CurrentMarkupObjects.VerticalLinesCoordinates[_selectedVerticalLineIndex.Value] = (int)_drawingEndPoint.X;
                                else if (CurrentMarkupObjects.VerticalLinesCoordinates.Count < CurrentMarkupObjects.MaxNumberOfVerticalLines)
                                    CurrentMarkupObjects.VerticalLinesCoordinates.Add((int)_drawingEndPoint.X);
                                _selectedVerticalLineIndex = null;

                                DrawMarkupObjects();                           
                                MarkupObjectsChanged?.Invoke(this, new MarkupObjectsEventArgs(CurrentMarkupObjects));
                            }
                            break;

                        case MarkupObjectType.HorizontalLine:
                            ImageCanvas.Children.Remove(_tempLine);

                            if (CurrentMarkupObjects.RectangleArea.Contains(CurrentMarkupObjects.RectangleArea.Left, (int)_drawingEndPoint.Y))
                            {
                                if (_selectedHorizontalLineIndex.HasValue)
                                    CurrentMarkupObjects.HorizontalLinesCoordinates[_selectedHorizontalLineIndex.Value] = (int)_drawingEndPoint.Y;
                                else
                                    CurrentMarkupObjects.HorizontalLinesCoordinates.Add((int)_drawingEndPoint.Y);
                                _selectedHorizontalLineIndex = null;

                                DrawMarkupObjects();
                                MarkupObjectsChanged?.Invoke(this, new MarkupObjectsEventArgs(CurrentMarkupObjects));
                            }
                            break;
                    }

                    CurrentMarkupObjects.RemoveWrongLines();
                    break;

                #endregion

                #region ImageViewerMode.Crop

                case ImageViewerMode.Crop:
                    CropAreaSelected?.Invoke(this, new RectangleAreaEventArgs(CreateRectangleByTwoPoints(_drawingStartPoint, _drawingEndPoint)));
                    break;

                #endregion
            }            
        }

        private void ImageCanvas_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && _isDrawingInProgress)
            {
                if (_selectedVerticalLineIndex.HasValue)
                {
                    CurrentMarkupObjects.VerticalLinesCoordinates.RemoveAt(_selectedVerticalLineIndex.Value);
                    _selectedVerticalLineIndex = null;
                    ClearDrawingTempObjects();
                }

                if (_selectedHorizontalLineIndex.HasValue)
                {
                    CurrentMarkupObjects.HorizontalLinesCoordinates.RemoveAt(_selectedHorizontalLineIndex.Value);
                    _selectedHorizontalLineIndex = null;
                    ClearDrawingTempObjects();
                }
            }

            if (e.Key == Key.Escape && _isDrawingInProgress)
            {
                ClearDrawingTempObjects();
            }
        }

        #endregion

        #endregion
    }
}