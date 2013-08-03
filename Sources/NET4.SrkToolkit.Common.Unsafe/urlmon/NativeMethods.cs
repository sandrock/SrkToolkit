
namespace SrkToolkit.Common.urlmon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.InteropServices;
    using System.IO;

    /// <summary>
    /// Contains P/Invoke code.
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// Finds the MIME from data.
        /// </summary>
        /// <param name="pBC">A pointer to the IBindCtx interface. Can be set to NULL.</param>
        /// <param name="pwzUrl">A pointer to a string value that contains the URL of the data. Can be set to NULL if pBuffer contains the data to be sniffed.</param>
        /// <param name="pBuffer">A pointer to the buffer that contains the data to be sniffed. Can be set to NULL if pwzUrl contains a valid URL.</param>
        /// <param name="cbSize">An unsigned long integer value that contains the size of the buffer.</param>
        /// <param name="pwzMimeProposed">A pointer to a string value that contains the proposed MIME type. This value is authoritative if type cannot be determined from the data. If the proposed type contains a semi-colon (;) it is removed. This parameter can be set to NULL.</param>
        /// <param name="dwMimeFlags">
        ///   One of the following required values:
        ///     FMFD_DEFAULT
        ///         No flags specified. Use default behavior for the function. 
        ///     FMFD_URLASFILENAME
        ///         Treat the specified pwzUrl as a file name. 
        ///     FMFD_ENABLEMIMESNIFFING
        ///         Microsoft Internet Explorer 6 for Windows XP Service Pack 2 (SP2) and later. Use MIME-type detection even if FEATURE_MIME_SNIFFING is detected. Usually, this feature control key would disable MIME-type detection. 
        ///     FMFD_IGNOREMIMETEXTPLAIN
        ///         Internet Explorer 6 for Windows XP SP2 and later. Perform MIME-type detection if "text/plain" is proposed, even if data sniffing is otherwise disabled. Plain text may be converted to text/html if HTML tags are detected. 
        ///     FMFD_SERVERMIME
        ///         Windows Internet Explorer 8. Use the authoritative MIME type specified in pwzMimeProposed. Unless FMFD_IGNOREMIMETEXTPLAIN is specified, no data sniffing is performed. 
        ///     FMFD_RESPECTTEXTPLAIN New for Internet Explorer 9 
        ///         Internet Explorer 9. Do not perform detection if "text/plain" is specified in pwzMimeProposed. 
        ///     FMFD_RETURNUPDATEDIMGMIMES New for Internet Explorer 9 
        ///         Internet Explorer 9. Returns image/png and image/jpeg instead of image/x-png and image/pjpeg. 
        /// </param>
        /// <param name="ppwzMimeOut">The address of a string value that receives the suggested MIME type.</param>
        /// <param name="dwReserved">Reserved. Must be set to 0.</param>
        /// <returns>
        ///   Returns one of the following values.
        ///     S_OK 	The operation completed successfully.
        ///     E_FAIL 	The operation failed.
        ///     E_INVALIDARG 	One or more arguments are invalid.
        ///     E_OUTOFMEMORY 	There is insufficient memory to complete the operation.
        /// </returns>
        [DllImport(@"urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        private extern static int FindMimeFromData(
            IntPtr pBC,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] byte[] pBuffer,
            int cbSize,
            [MarshalAs(UnmanagedType.LPWStr)]  string pwzMimeProposed,
            int dwMimeFlags,
            out IntPtr ppwzMimeOut,
            int dwReserved
        );

        /// <summary>
        /// Wraps a call to urlmon.dll/GetMimeFromFile.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static string GetMimeFromFile(byte[] buffer, string filename)
        {
            try
            {
                IntPtr mimetype;
                int result = FindMimeFromData(IntPtr.Zero, filename, buffer, 256, string.Empty, 0, out mimetype, 0);
                if (result != 0)
                {
                    throw Marshal.GetExceptionForHR(result);
                }

                string mime = Marshal.PtrToStringUni(mimetype);
                Marshal.FreeCoTaskMem(mimetype);

                // let's fix windows
                if (mime == "image/x-png")
                    mime = "image/png";

                return string.IsNullOrWhiteSpace(mime) ? null : mime;
            }
            catch (AccessViolationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
