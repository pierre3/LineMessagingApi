using System;
using System.IO;
using System.Net.Http.Headers;

namespace Line.Messaging
{
    /// <summary>
    /// Stream object for content such as image, file, etc.
    /// </summary>
    public class ContentStream : Stream
    {
        protected Stream _baseStream;

        protected Stream BaseStream
        {
            get
            {
                if (_baseStream == null) { throw new ObjectDisposedException(nameof(BaseStream)); }
                return _baseStream;
            }
        }

        public HttpContentHeaders ContentHeaders { get; }

        public ContentStream(Stream baseStream, HttpContentHeaders contentHeaders)
        {
            _baseStream = baseStream;
            ContentHeaders = contentHeaders;
        }

        public override bool CanRead => _baseStream?.CanRead ?? false;

        public override bool CanSeek => _baseStream?.CanSeek ?? false;

        public override bool CanWrite => _baseStream?.CanWrite ?? false;

        public override long Length => BaseStream.Length;

        public override long Position { get => BaseStream.Position; set => BaseStream.Position = value; }

        public override void Flush() => BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

        public override void SetLength(long value) => BaseStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _baseStream?.Dispose();
                _baseStream = null;
            }
        }
    }
}
