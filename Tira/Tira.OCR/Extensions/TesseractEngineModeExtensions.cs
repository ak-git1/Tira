using System;
using Tesseract;
using Tira.OCR.Enums;

namespace Tira.OCR.Extensions
{
    /// <summary>
    /// Extensions for working with Tesseract
    /// </summary>
    internal static class TesseractExtensions
    {
        /// <summary>
        /// Convert engine modes
        /// </summary>
        /// <param name="mode">Mode.</param>
        /// <returns></returns>
        internal static EngineMode ToEngineMode(this TesseractEngineMode mode)
        {
            switch (mode)
            {
                case TesseractEngineMode.Default:
                    return EngineMode.Default;

                case TesseractEngineMode.Tesseract:
                    return EngineMode.TesseractOnly;

                case TesseractEngineMode.Cube:
                    return EngineMode.CubeOnly;

                case TesseractEngineMode.Both:
                    return EngineMode.TesseractAndCube;

                default:
                    throw new InvalidCastException("Undefined OCR Engine mode");
            }
        }
    }
}
