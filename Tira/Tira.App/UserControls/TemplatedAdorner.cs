using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Tira.App.UserControls
{
    /// <summary>
    /// Templated adorender control
    /// </summary>
    /// <seealso cref="System.Windows.Documents.Adorner" />
    public class TemplatedAdorner : Adorner
    {
        #region Variables

        /// <summary>
        /// Framework element
        /// </summary>
        private readonly FrameworkElement _frameworkElement;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of visual child elements within this element
        /// </summary>
        protected override int VisualChildrenCount => 1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatedAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">Adorned element</param>
        /// <param name="frameworkElement">Framework element</param>
        public TemplatedAdorner(UIElement adornedElement, FrameworkElement frameworkElement) : base(adornedElement)
        {
            _frameworkElement = frameworkElement;
            AddVisualChild(_frameworkElement);
            AddLogicalChild(_frameworkElement);
        }

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement" /> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _frameworkElement.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)" />, and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return _frameworkElement;
        }

        /// <summary>
        /// Implements any custom measuring behavior for the adorner.
        /// </summary>
        /// <param name="constraint">A size to constrain the adorner to.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Size" /> object representing the amount of layout space needed by the adorner.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            _frameworkElement.Measure(constraint);
            return _frameworkElement.DesiredSize;
        }

        #endregion
    }
}
