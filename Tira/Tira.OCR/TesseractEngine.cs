using System;
using System.Drawing;
using Tesseract;
using Tira.OCR.Enums;
using Tira.OCR.Extensions;

namespace Tira.OCR
{
    /// <summary>
    /// Engine for working with Tesseract OCR
    /// </summary>
    public class TesseractEngine
    {
        #region Properties

        /// <summary>
        /// Default segmentation mode.
        /// </summary>
        public TesseractPageSegmentationMode DefaultSegmentationMode { get; set; }

        /// <summary>
        /// Engine mode
        /// </summary>
        public TesseractEngineMode EngineMode { get; set; }

        /// <summary>
        /// Recognition language 1.
        /// </summary>
        public RecognitionLanguage RecognitionLanguage1 { get; set; }

        /// <summary>
        /// Recognition language 2.
        /// </summary>
        public RecognitionLanguage RecognitionLanguage2 { get; set; }

        /// <summary>
        /// Search for digits only
        /// </summary>
        public bool SearchForDigitsOnly { get; set; } = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractEngine" /> class.
        /// </summary>
        /// <param name="language1">Language 1</param>
        /// <param name="language2">Language 2</param>
        public TesseractEngine(RecognitionLanguage language1, RecognitionLanguage language2) 
            : this(language1, language2, TesseractEngineMode.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractEngine"/> class.
        /// </summary>
        /// <param name="language1">Language 1</param>
        /// <param name="language2">Language 2</param>
        /// <param name="engineMode">Engine mode.</param>
        public TesseractEngine(RecognitionLanguage language1, RecognitionLanguage language2, TesseractEngineMode engineMode)
            : this(language1, language2, engineMode, TesseractPageSegmentationMode.Auto)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractEngine"/> class.
        /// </summary>
        /// <param name="language1">Language 1</param>
        /// <param name="language2">Language 2</param>
        /// <param name="engineMode">Engine mode.</param>
        /// <param name="defaultSegmentationMode">Default segmentation mode.</param>
        public TesseractEngine(RecognitionLanguage language1, RecognitionLanguage language2, TesseractEngineMode engineMode, TesseractPageSegmentationMode defaultSegmentationMode)
        {
            RecognitionLanguage1 = language1;
            RecognitionLanguage2 = language2;
            EngineMode = engineMode;
            DefaultSegmentationMode = defaultSegmentationMode;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Processes the specified image path.
        /// </summary>
        /// <param name="bitmap">Bitmap.</param>
        /// <returns></returns>
        public string Process(Bitmap bitmap)
        {
            using (Tesseract.TesseractEngine engine = CreateEngine())
            {
                engine.DefaultPageSegMode = (PageSegMode)DefaultSegmentationMode;
                using (Page page = engine.Process(bitmap))
                    return page.GetText();
            }
        }

        /// <summary>
        /// Processes the specified image path.
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        /// <returns></returns>
        public string Process(string imagePath)
        {
            using (Tesseract.TesseractEngine engine = CreateEngine())
            {
                engine.DefaultPageSegMode = (PageSegMode)DefaultSegmentationMode;
                using (Bitmap bitmap = (Bitmap)Image.FromFile(imagePath))
                using (Page page = engine.Process(bitmap))
                    return page.GetText();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates OCR engine
        /// </summary>
        /// <returns></returns>
        private Tesseract.TesseractEngine CreateEngine()
        {
            Tesseract.TesseractEngine tesseractEngine = new Tesseract.TesseractEngine("tessdata", GetLanguageNamesString(RecognitionLanguage1, RecognitionLanguage2), EngineMode.ToEngineMode());
            if (SearchForDigitsOnly)
                tesseractEngine.SetVariable("tessedit_char_whitelist", "0123456789");
            return tesseractEngine;
        }

        /// <summary>
        /// Gets the language names string.
        /// </summary>
        /// <param name="language1">Language1.</param>
        /// <param name="language2">Language2.</param>
        /// <returns></returns>
        private string GetLanguageNamesString(RecognitionLanguage language1, RecognitionLanguage language2)
        {
            if (language1 == RecognitionLanguage.None && language2 == RecognitionLanguage.None)
                return "eng";

            if (language1 == RecognitionLanguage.None)
                return GetLanguageName(language2);

            if (language2 == RecognitionLanguage.None)
                return GetLanguageName(language1);

            return $"{GetLanguageName(language1)}+{GetLanguageName(language2)}";
        }

        /// <summary>
        /// Gets the tesseract name of the language.
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns></returns>
        private string GetLanguageName(RecognitionLanguage language)
        {
            switch (language)
            {
                case RecognitionLanguage.English:
                    return "eng";

                case RecognitionLanguage.Russian:
                    return "rus";

                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }

        #endregion
    }
}
