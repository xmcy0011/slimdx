﻿// Copyright (c) 2007-2010 SlimDX Group
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;

namespace SlimDX2.DirectWrite
{
    public partial interface PixelSnapping
    {
        /// <summary>
        /// Determines whether pixel snapping is disabled. The recommended default is FALSE,
        /// unless doing animation that requires subpixel vertical placement.
        /// </summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <returns>Receives TRUE if pixel snapping is disabled or FALSE if it not. </returns>
        /// <unmanaged>HRESULT IsPixelSnappingDisabled([None] void* clientDrawingContext,[Out] BOOL* isDisabled)</unmanaged>
        bool IsPixelSnappingDisabled(IntPtr clientDrawingContext);

        /// <summary>	
        ///  Gets a transform that maps abstract coordinates to DIPs. 	
        /// </summary>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SlimDX2.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>a structure which has transform information for  pixel snapping.</returns>
        /// <unmanaged>HRESULT GetCurrentTransform([None] void* clientDrawingContext,[Out] DWRITE_MATRIX* transform)</unmanaged>
        SlimDX2.DirectWrite.Matrix GetCurrentTransform(IntPtr clientDrawingContext);


        /// <summary>	
        ///  Gets the number of physical pixels per DIP. 	
        /// </summary>	
        /// <remarks>	
        ///  Because a DIP (device-independent pixel) is 1/96 inch,  the pixelsPerDip value is the number of logical pixels per inch divided by 96.	
        /// </remarks>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SlimDX2.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>the number of physical pixels per DIP</returns>
        /// <unmanaged>HRESULT GetPixelsPerDip([None] void* clientDrawingContext,[Out] FLOAT* pixelsPerDip)</unmanaged>
        float GetPixelsPerDip(IntPtr clientDrawingContext);
    }

    /// <summary>
    /// Internal TessellationSink Callback
    /// </summary>
    internal class PixelSnappingCallback<T> : SlimDX2.ComObjectCallback<T> where T : PixelSnapping
    {
        public PixelSnappingCallback(T callback, int nbMethods) : base(callback, nbMethods + 3)
        {
            AddMethod(new IsPixelSnappingDisabledDelegate(IsPixelSnappingDisabledImpl));
            AddMethod(new GetCurrentTransformDelegate(GetCurrentTransformImpl));
            AddMethod(new GetPixelsPerDipDelegate(GetPixelsPerDipImpl));
        }

        /// <summary>
        /// Determines whether pixel snapping is disabled. The recommended default is FALSE,
        /// unless doing animation that requires subpixel vertical placement.
        /// </summary>
        /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
        /// <returns>Receives TRUE if pixel snapping is disabled or FALSE if it not. </returns>
        /// <unmanaged>HRESULT IsPixelSnappingDisabled([None] void* clientDrawingContext,[Out] BOOL* isDisabled)</unmanaged>
        private delegate int IsPixelSnappingDisabledDelegate(IntPtr clientDrawingContext, out int isDisabled);
        private int IsPixelSnappingDisabledImpl(IntPtr clientDrawingContext, out int isDisabled)
        {
            isDisabled = 0;
            try
            {
                isDisabled = Callback.IsPixelSnappingDisabled(clientDrawingContext) ? 1 : 0;
            }
            catch (SlimDX2Exception exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception ex)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }

        /// <summary>	
        ///  Gets a transform that maps abstract coordinates to DIPs. 	
        /// </summary>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SlimDX2.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>a structure which has transform information for  pixel snapping.</returns>
        /// <unmanaged>HRESULT GetCurrentTransform([None] void* clientDrawingContext,[Out] DWRITE_MATRIX* transform)</unmanaged>
        private delegate int GetCurrentTransformDelegate(IntPtr clientDrawingContext, IntPtr transform);
        private int GetCurrentTransformImpl(IntPtr clientDrawingContext, IntPtr transform)
        {
            unsafe
            {
                SlimDX2.DirectWrite.Matrix matrix;
                try
                {
                    matrix = Callback.GetCurrentTransform(clientDrawingContext);
                    SlimDX2.Interop.Write((void*) transform, ref matrix);
                }
                catch (SlimDX2Exception exception)
                {
                    return exception.ResultCode.Code;
                }
                catch (Exception ex)
                {
                    return Result.Fail.Code;
                }
                return Result.Ok.Code;
            }
        }


        /// <summary>	
        ///  Gets the number of physical pixels per DIP. 	
        /// </summary>	
        /// <remarks>	
        ///  Because a DIP (device-independent pixel) is 1/96 inch,  the pixelsPerDip value is the number of logical pixels per inch divided by 96.	
        /// </remarks>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SlimDX2.DirectWrite.TextLayout.Draw_"/>.</param>
        /// <returns>the number of physical pixels per DIP</returns>
        /// <unmanaged>HRESULT GetPixelsPerDip([None] void* clientDrawingContext,[Out] FLOAT* pixelsPerDip)</unmanaged>
        private delegate int GetPixelsPerDipDelegate(IntPtr clientDrawingContext, out float pixelPerDip);
        private int GetPixelsPerDipImpl(IntPtr clientDrawingContext, out float pixelPerDip)
        {
            pixelPerDip = 0;
            try
            {
                pixelPerDip = Callback.GetPixelsPerDip(clientDrawingContext);
            }
            catch (SlimDX2Exception exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception ex)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }
    }

}
