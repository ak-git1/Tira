using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Tira.App.Logic.Events;
using Tira.Logic.Models;

namespace Tira.App.UserControls
{
    /// <summary>
    /// Interaction logic for Gallery.xaml
    /// </summary>
    public partial class Gallery : UserControl
    {
        #region Properties

        /// <summary>
        /// Images
        /// </summary>
        [Bindable(true)]
        public BindingList<GalleryImage> Images
        {
            get => (BindingList<GalleryImage>)GetValue(ImagesProperty);
            set => SetValue(ImagesProperty, value);
        }

        #endregion     

        #region Dependency properties

        /// <summary>
        /// Images dependency property
        /// </summary>
        public static readonly DependencyProperty ImagesProperty = DependencyProperty.Register("Images", typeof(BindingList<GalleryImage>), typeof(Gallery), new UIPropertyMetadata(null));

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for gallery image selected event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GalleryImageEventArgs"/> instance containing the event data.</param>
        public delegate void GalleryImageSelectedHandler(object sender, GalleryImageEventArgs e);

        /// <summary>
        ///  Delegate for gallery images uids selected event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GalleryImagesUidsSelectionEventArgs"/> instance containing the event data.</param>
        public delegate void GalleryImagesIdsSelectedHandler(object sender, GalleryImagesUidsSelectionEventArgs e);

        #endregion

        #region Events

        /// <summary>
        /// Gallery image selected event
        /// </summary>
        public event GalleryImageSelectedHandler GalleryImageSelected;

        /// <summary>
        /// Gallery images uids selected event
        /// </summary>
        public event GalleryImagesIdsSelectedHandler GalleryImagesUidsSelected;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Gallery"/> class.
        /// </summary>
        public Gallery()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                GalleryImageSelected?.Invoke(this, new GalleryImageEventArgs((GalleryImage) e.AddedItems[0]));
                GalleryImagesUidsSelected?.Invoke(this, new GalleryImagesUidsSelectionEventArgs(GalleryListBox.SelectedItems.Cast<GalleryImage>().ToList()));
            }
        }

        #endregion
    }
}