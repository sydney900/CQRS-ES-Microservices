using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class MyExtension
    {
        public static void RemoveAllEventHandlers<T>(this T t)
        {
            foreach (var ei in t.GetType().GetEvents(BindingFlags.Default))
            {
                var declaringType = ei.DeclaringType;
                var field = declaringType.GetField(ei.Name, BindingFlags.Default);
                if (field != null)
                {
                    var del = field.GetValue(t) as Delegate;
                    if (del != null)
                    {
                        foreach (var sub in del.GetInvocationList())
                        {
                            ei.RemoveEventHandler(t, sub);
                        }
                    }
                }
            }
        }

        public static bool IsGZipped(this byte[] buffer)
        {
            return buffer.Length > 2 && buffer[0] == 31 && buffer[1] == 139;
        }

        public static string ToUTF8String(this byte[] buffer)
        {
            if (buffer.IsGZipped())
            {
                using (var compressedStream = new MemoryStream(buffer))
                using (var compression = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var decompressedStream = new MemoryStream())
                {
                    compression.CopyTo(decompressedStream);
                    return Encoding.UTF8.GetString(decompressedStream.GetBuffer()).Trim('\0');
                }
            }
            else
            {
                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
