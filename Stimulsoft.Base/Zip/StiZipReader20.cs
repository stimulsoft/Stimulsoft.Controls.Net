using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading;

namespace Stimulsoft.Base.Zip
{
    public class StiZipReader20 : IDisposable
    {
        #region Methods
        public Stream GetEntryStream(StiZipEntry entry)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("ZipFile");
            }
            if (entry.IsCrypted)
            {
                throw new StiZipException("Encryption is not supported");
            }

            long start = LocateEntry(entry);
            CompressionMethod method = entry.CompressionMethod;
            Stream result = new StiPartialInputStream(this, start, entry.CompressedSize);

            switch (method)
            {
                case CompressionMethod.Stored:
                    break;

                case CompressionMethod.Deflated:
                    byte[] buf = new byte[entry.CompressedSize];
                    _baseStream.Seek(start, SeekOrigin.Begin);
                    _baseStream.Read(buf, 0, buf.Length);
                    MemoryStream ms = new MemoryStream(buf);
                    result = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
                    break;

                default:
                    throw new StiZipException("Unsupported compression method " + method);
            }

            return result;
        }

        #region Read data
        private ushort ReadLEUshort()
        {
            int data1 = _baseStream.ReadByte();
            if (data1 < 0)
            {
                throw new EndOfStreamException("End of stream");
            }
            int data2 = _baseStream.ReadByte();
            if (data2 < 0)
            {
                throw new EndOfStreamException("End of stream");
            }
            return unchecked((ushort) ((ushort) data1 | (ushort) (data2 << 8)));
        }

        private uint ReadLEUint()
        {
            return (uint) (ReadLEUshort() | (ReadLEUshort() << 16));
        }

        private ulong ReadLEUlong()
        {
            return ReadLEUint() | ((ulong) ReadLEUint() << 32);
        }
        #endregion

        private long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize,
            int maximumVariableData)
        {
            using (StiZipHelperStream les = new StiZipHelperStream(_baseStream))
            {
                return les.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
            }
        }

        private void ReadEntries()
        {
            long locatedEndOfCentralDir = LocateBlockWithSignature(
                StiZipConstants.EndOfCentralDirectorySignature,
                _baseStream.Length,
                StiZipConstants.EndOfCentralRecordBaseSize,
                0xffff);

            if (locatedEndOfCentralDir < 0)
            {
                throw new StiZipException("Cannot find central directory");
            }

            ushort thisDiskNumber = ReadLEUshort();
            ushort startCentralDirDisk = ReadLEUshort();
            ulong entriesForThisDisk = ReadLEUshort();
            ulong entriesForWholeCentralDir = ReadLEUshort();
            ulong centralDirSize = ReadLEUint();
            long offsetOfCentralDir = ReadLEUint();

            bool isZip64 = false;

            #region Check if zip64 header information is required.

            if ((thisDiskNumber == 0xffff) ||
                (startCentralDirDisk == 0xffff) ||
                (entriesForThisDisk == 0xffff) ||
                (entriesForWholeCentralDir == 0xffff) ||
                (centralDirSize == 0xffffffff) ||
                (offsetOfCentralDir == 0xffffffff))
            {
                isZip64 = true;

                long offset = LocateBlockWithSignature(StiZipConstants.Zip64CentralDirLocatorSignature,
                    locatedEndOfCentralDir, 0, 0x1000);
                if (offset < 0)
                {
                    throw new StiZipException("Cannot find Zip64 locator");
                }

                ReadLEUint();
                ulong offset64 = ReadLEUlong();
                uint totalDisks = ReadLEUint();

                _baseStream.Position = (long) offset64;
                long sig64 = ReadLEUint();

                if (sig64 != StiZipConstants.Zip64CentralFileHeaderSignature)
                {
                    throw new StiZipException(string.Format("Invalid Zip64 Central directory signature at {0:X}", offset64));
                }

                ulong recordSize = ReadLEUlong();
                int versionMadeBy = ReadLEUshort();
                int versionToExtract = ReadLEUshort();
                uint thisDisk = ReadLEUint();
                uint centralDirDisk = ReadLEUint();
                entriesForThisDisk = ReadLEUlong();
                entriesForWholeCentralDir = ReadLEUlong();
                centralDirSize = ReadLEUlong();
                offsetOfCentralDir = (long) ReadLEUlong();
            }

            #endregion

            _entries = new StiZipEntry[entriesForThisDisk];

            if (!isZip64 && (offsetOfCentralDir < locatedEndOfCentralDir - (4 + (long) centralDirSize)))
            {
                offsetOfFirstEntry = locatedEndOfCentralDir - (4 + (long) centralDirSize + offsetOfCentralDir);
                if (offsetOfFirstEntry <= 0)
                {
                    throw new StiZipException("Invalid embedded zip archive");
                }
            }

            _baseStream.Seek(offsetOfFirstEntry + offsetOfCentralDir, SeekOrigin.Begin);

            for (ulong i = 0; i < entriesForThisDisk; i++)
            {
                if (ReadLEUint() != StiZipConstants.CentralHeaderSignature)
                {
                    throw new StiZipException("Wrong Central Directory signature");
                }

                int versionMadeBy = ReadLEUshort();
                int versionToExtract = ReadLEUshort();
                int bitFlags = ReadLEUshort();
                int method = ReadLEUshort();
                uint dostime = ReadLEUint();
                uint crc = ReadLEUint();
                long csize = (long) ReadLEUint();
                long size = (long) ReadLEUint();
                int nameLen = ReadLEUshort();
                int extraLen = ReadLEUshort();
                int commentLen = ReadLEUshort();

                int diskStartNo = ReadLEUshort();
                int internalAttributes = ReadLEUshort();

                uint externalAttributes = ReadLEUint();
                long offset = ReadLEUint();

                byte[] buffer = new byte[Math.Max(nameLen, commentLen)];

                Utils.ReadFully(_baseStream, buffer, 0, nameLen);
                string name = StiZipConstants.ConvertToStringExt(bitFlags, buffer, nameLen);

                StiZipEntry entry = new StiZipEntry(name, versionToExtract, versionMadeBy, (CompressionMethod) method);
                entry.zipFile = this;
                entry.Crc = crc & 0xffffffffL;
                entry.Size = size & 0xffffffffL;
                entry.CompressedSize = csize & 0xffffffffL;
                entry.Flags = bitFlags;
                entry.DosTime = (uint) dostime;
                entry.ZipFileIndex = (long) i;
                entry.Offset = offset;
                entry.ExternalFileAttributes = (int) externalAttributes;

                if (extraLen > 0)
                {
                    byte[] extra = new byte[extraLen];
                    Utils.ReadFully(_baseStream, extra);
                    entry.ExtraData = extra;
                }

                entry.ProcessExtraData(false);

                if (commentLen > 0)
                {
                    Utils.ReadFully(_baseStream, buffer, 0, commentLen);
                    entry.Comment = StiZipConstants.ConvertToStringExt(bitFlags, buffer, commentLen);
                }

                _entries[i] = entry;
            }
        }

        private long LocateEntry(StiZipEntry entry)
        {
            _baseStream.Seek(offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
            if ((int) ReadLEUint() != StiZipConstants.LocalHeaderSignature)
            {
                throw new StiZipException(string.Format("Wrong local header signature @{0:X}",
                    offsetOfFirstEntry + entry.Offset));
            }

            short extractVersion = (short) ReadLEUshort();
            short localFlags = (short) ReadLEUshort();
            short compressionMethod = (short) ReadLEUshort();
            short fileTime = (short) ReadLEUshort();
            short fileDate = (short) ReadLEUshort();
            uint crcValue = ReadLEUint();
            long compressedSize = ReadLEUint();
            long size = ReadLEUint();
            int storedNameLength = ReadLEUshort();
            int extraDataLength = ReadLEUshort();

            byte[] nameData = new byte[storedNameLength];
            Utils.ReadFully(_baseStream, nameData);

            byte[] extraData = new byte[extraDataLength];
            Utils.ReadFully(_baseStream, extraData);

            StiZipExtraData localExtraData = new StiZipExtraData(extraData);

            if (entry.IsFile)
            {
                if (!entry.IsCompressionMethodSupported())
                {
                    throw new StiZipException("Compression method not supported");
                }

                if ((extractVersion > StiZipConstants.VersionMadeBy) ||
                    ((extractVersion > 20) && (extractVersion < StiZipConstants.VersionZip64)) ||
                    ((localFlags &
                      (int)
                          (GeneralBitFlags.Patched | GeneralBitFlags.StrongEncryption | GeneralBitFlags.EnhancedCompress |
                           GeneralBitFlags.HeaderMasked)) != 0))
                {
                    throw new StiZipException("The library does not support the zip version required to extract this entry");
                }
            }

            int extraLength = storedNameLength + extraDataLength;
            return offsetOfFirstEntry + entry.Offset + StiZipConstants.LocalHeaderBaseSize + extraLength;
        }
        #endregion

        #region Properties
        public StiZipEntry[] Entries
        {
            get { return _entries; }
        }
        #endregion

        #region Fields
        private bool _isDisposed;
        private Stream _baseStream;
        private bool isStreamOwner;
        private long offsetOfFirstEntry;

        private StiZipEntry[] _entries;
        #endregion

        #region Enumerations
        /// <summary>
        /// The kind of compression used for an entry in an archive
        /// </summary>
        internal enum CompressionMethod
        {
            /// <summary>
            /// A direct copy of the file contents is held in the archive
            /// </summary>
            Stored = 0,

            /// <summary>
            /// Common Zip compression method using a sliding dictionary 
            /// of up to 32KB and secondary compression from Huffman/Shannon-Fano trees
            /// </summary>
            Deflated = 8,

            /// <summary>
            /// An extension to deflate with a 64KB window. Not supported
            /// </summary>
            Deflate64 = 9,

            /// <summary>
            /// BZip2 compression. Not supported
            /// </summary>
            BZip2 = 11,

            /// <summary>
            /// WinZip special for AES encryption, Not supported
            /// </summary>
            WinZipAES = 99
        }

        /// <summary>
        /// Defines the contents of the general bit flags field for an archive entry.
        /// </summary>
        [Flags]
        private enum GeneralBitFlags
        {
            /// <summary>
            /// Bit 5 if set indicates the file contains Pkzip compressed patched data.
            /// Requires version 2.7 or greater.
            /// </summary>
            Patched = 0x0020,

            /// <summary>
            /// Bit 6 if set indicates strong encryption has been used for this entry.
            /// </summary>
            StrongEncryption = 0x0040,

            /// <summary>
            /// Bit 11 if set indicates the filename and 
            /// comment fields for this file must be encoded using UTF-8.
            /// </summary>
            UnicodeText = 0x0800,

            /// <summary>
            /// Bit 12 is documented as being reserved by PKware for enhanced compression.
            /// </summary>
            EnhancedCompress = 0x1000,

            /// <summary>
            /// Bit 13 if set indicates that values in the local header are masked to hide
            /// their actual values, and the central directory is encrypted.
            /// </summary>
            /// <remarks>
            /// Used when encrypting the central directory contents.
            /// </remarks>
            HeaderMasked = 0x2000
        }

        /// <summary>
        /// Defines known values for the <see cref="HostSystemID"/> property.
        /// </summary>
        private enum HostSystemID
        {
            /// <summary>
            /// Host system = MSDOS
            /// </summary>
            Msdos = 0,

            /// <summary>
            /// Host system = Amiga
            /// </summary>
            Amiga = 1,

            /// <summary>
            /// Host system = Open VMS
            /// </summary>
            OpenVms = 2,

            /// <summary>
            /// Host system = Unix
            /// </summary>
            Unix = 3,

            /// <summary>
            /// Host system = VMCms
            /// </summary>
            VMCms = 4,

            /// <summary>
            /// Host system = Atari ST
            /// </summary>
            AtariST = 5,

            /// <summary>
            /// Host system = OS2
            /// </summary>
            OS2 = 6,

            /// <summary>
            /// Host system = Macintosh
            /// </summary>
            Macintosh = 7,

            /// <summary>
            /// Host system = ZSystem
            /// </summary>
            ZSystem = 8,

            /// <summary>
            /// Host system = Cpm
            /// </summary>
            Cpm = 9,

            /// <summary>
            /// Host system = Windows NT
            /// </summary>
            WindowsNT = 10,

            /// <summary>
            /// Host system = MVS
            /// </summary>
            MVS = 11,

            /// <summary>
            /// Host system = VSE
            /// </summary>
            Vse = 12,

            /// <summary>
            /// Host system = Acorn RISC
            /// </summary>
            AcornRisc = 13,

            /// <summary>
            /// Host system = VFAT
            /// </summary>
            Vfat = 14,

            /// <summary>
            /// Host system = Alternate MVS
            /// </summary>
            AlternateMvs = 15,

            /// <summary>
            /// Host system = BEOS
            /// </summary>
            BeOS = 16,

            /// <summary>
            /// Host system = Tandem
            /// </summary>
            Tandem = 17,

            /// <summary>
            /// Host system = OS400
            /// </summary>
            OS400 = 18,

            /// <summary>
            /// Host system = OSX
            /// </summary>
            OSX = 19,

            /// <summary>
            /// Host system = WinZIP AES
            /// </summary>
            WinZipAES = 99,
        }
        #endregion

        #region Class StiZipEntry
        public class StiZipEntry
        {
            #region Enums
            [Flags]
            private enum Known : byte
            {
                None = 0,
                Size = 0x01,
                CompressedSize = 0x02,
                Crc = 0x04,
                Time = 0x08,
                ExternalAttributes = 0x10,
            }
            #endregion

            #region Constructors
            internal StiZipEntry(string name, int versionRequiredToExtract, int madeByInfo, CompressionMethod method)
            {
                this.DateTime = System.DateTime.Now;
                this.name = name;
                this.versionMadeBy = (ushort) madeByInfo;
                this.versionToExtract = (ushort) versionRequiredToExtract;
                this.method = method;
            }
            #endregion

            #region Methods
            public void ExtractToFile(string path, bool overwrite)
            {
                var dir = Path.GetDirectoryName(path);
                if (dir != null && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (IsFile)
                {
                    Stream stream = zipFile.GetEntryStream(this);

                    FileStream fs = new FileStream(path, FileMode.Create);

                    const int bufSize = 8192;

                    byte[] buf = new byte[bufSize];
                    int length = (int) Size;
                    while (length > 0)
                    {
                        int count = stream.Read(buf, 0, length > bufSize ? bufSize : length);
                        if (count > 0)
                        {
                            fs.Write(buf, 0, count);
                            length -= count;
                        }
                        else
                        {
                            length = 0;
                        }
                    }

                    fs.Flush();
                    fs.Close();
                }

            }

            private bool HasDosAttributes(int attributes)
            {
                bool result = false;
                if ((known & Known.ExternalAttributes) != 0)
                {
                    if (((HostSystem == (int) HostSystemID.Msdos) || (HostSystem == (int) HostSystemID.WindowsNT)) &&
                        (ExternalFileAttributes & attributes) == attributes)
                    {
                        result = true;
                    }
                }
                return result;
            }

            internal void ProcessExtraData(bool localHeader)
            {
                StiZipExtraData extraData = new StiZipExtraData(this.extra);

                if (extraData.Find(0x0001))
                {
                    //forceZip64_ = true;

                    if (extraData.ValueLength < 4)
                    {
                        throw new StiZipException("Extra data extended Zip64 information length is invalid");
                    }

                    if (localHeader || (size == uint.MaxValue))
                    {
                        size = (ulong) extraData.ReadLong();
                    }
                    if (localHeader || (compressedSize == uint.MaxValue))
                    {
                        compressedSize = (ulong) extraData.ReadLong();
                    }
                    if (!localHeader && (offset == uint.MaxValue))
                    {
                        offset = extraData.ReadLong();
                    }
                }

                if (method == CompressionMethod.WinZipAES)
                {
                    throw new StiZipException("Encryption is not supported");
                }
            }
            #endregion

            #region Properties
            public bool IsCrypted
            {
                get { return (flags & 1) != 0; }
            }

            internal int Flags
            {
                set { flags = value; }
            }

            internal long ZipFileIndex
            {
                get { return zipFileIndex; }
                set { zipFileIndex = value; }
            }

            internal long Offset
            {
                get { return offset; }
                set { offset = value; }
            }

            internal int ExternalFileAttributes
            {
                get
                {
                    if ((known & Known.ExternalAttributes) == 0)
                    {
                        return -1;
                    }
                    return externalFileAttributes;
                }
                set
                {
                    externalFileAttributes = value;
                    known |= Known.ExternalAttributes;
                }
            }

            private int HostSystem
            {
                get { return (versionMadeBy >> 8) & 0xff; }
            }

            internal long DosTime
            {
                set
                {
                    unchecked
                    {
                        dosTime = (uint) value;
                    }
                    known |= Known.Time;
                }
            }

            internal DateTime DateTime
            {
                set
                {
                    uint year = (uint) value.Year;
                    uint month = (uint) value.Month;
                    uint day = (uint) value.Day;
                    uint hour = (uint) value.Hour;
                    uint minute = (uint) value.Minute;
                    uint second = (uint) value.Second;

                    if (year < 1980)
                    {
                        year = 1980;
                        month = 1;
                        day = 1;
                        hour = 0;
                        minute = 0;
                        second = 0;
                    }
                    else if (year > 2107)
                    {
                        year = 2107;
                        month = 12;
                        day = 31;
                        hour = 23;
                        minute = 59;
                        second = 59;
                    }

                    DosTime = ((year - 1980) & 0x7f) << 25 |
                              (month << 21) |
                              (day << 16) |
                              (hour << 11) |
                              (minute << 5) |
                              (second >> 1);
                }
            }

            public string FullName
            {
                get { return name; }
            }

            public string Name
            {
                get
                {
                    if (IsFile)
                    {
                        return Path.GetFileName(name);
                    }
                    return name;
                }
            }

            public long Size
            {
                get { return (known & Known.Size) != 0 ? (long) size : -1L; }
                set
                {
                    this.size = (ulong) value;
                    this.known |= Known.Size;
                }
            }

            internal long CompressedSize
            {
                get { return (known & Known.CompressedSize) != 0 ? (long) compressedSize : -1L; }
                set
                {
                    this.compressedSize = (ulong) value;
                    this.known |= Known.CompressedSize;
                }
            }

            internal long Crc
            {
                set
                {
                    if (((ulong) crc & 0xffffffff00000000L) != 0)
                    {
                        throw new ArgumentOutOfRangeException("value");
                    }
                    this.crc = (uint) value;
                    this.known |= Known.Crc;
                }
            }

            internal CompressionMethod CompressionMethod
            {
                get { return method; }
            }

            internal byte[] ExtraData
            {
                set { extra = value; }
            }

            public string Comment
            {
                set { comment = value; }
            }

            public bool IsDirectory
            {
                get
                {
                    int nameLength = name.Length;
                    bool result =
                        ((nameLength > 0) &&
                         ((name[nameLength - 1] == '/') || (name[nameLength - 1] == '\\'))) ||
                        HasDosAttributes(16);
                    return result;
                }
            }

            public bool IsFile
            {
                get { return !IsDirectory && !HasDosAttributes(8); }
            }

            public bool IsCompressionMethodSupported()
            {
                return
                    (this.CompressionMethod == CompressionMethod.Deflated) ||
                    (this.CompressionMethod == CompressionMethod.Stored);
            }
            #endregion

            #region Fields
            private Known known;
            private int externalFileAttributes = -1;

            private ushort versionMadeBy;
            private ushort versionToExtract;

            private string name;
            private ulong size;
            private ulong compressedSize;
            private uint crc;
            private uint dosTime;
            private CompressionMethod method = CompressionMethod.Deflated;
            private byte[] extra;
            private string comment;
            private int flags;

            private long zipFileIndex = -1;
            private long offset;

            //private bool forceZip64_;

            internal StiZipReader20 zipFile = null;
            #endregion
        }
        #endregion

        #region Class StiPartialInputStream
        private class StiPartialInputStream : Stream
        {
            #region Constructors
            public StiPartialInputStream(StiZipReader20 zipFile, long start, long length)
            {
                _start = start;
                _length = length;
                _baseStream = zipFile._baseStream;
                _readPos = start;
                _end = start + length;
            }
            #endregion

            #region Methods

            public override int ReadByte()
            {
                if (_readPos >= _end)
                {
                    return -1;
                }
                lock (_baseStream)
                {
                    _baseStream.Seek(_readPos++, SeekOrigin.Begin);
                    return _baseStream.ReadByte();
                }
            }

            public override void Close()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                lock (_baseStream)
                {
                    if (count > _end - _readPos)
                    {
                        count = (int) (_end - _readPos);
                        if (count == 0)
                        {
                            return 0;
                        }
                    }

                    _baseStream.Seek(_readPos, SeekOrigin.Begin);
                    int readCount = _baseStream.Read(buffer, offset, count);
                    if (readCount > 0)
                    {
                        _readPos += readCount;
                    }
                    return readCount;
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                long newPos = _readPos;

                switch (origin)
                {
                    case SeekOrigin.Begin:
                        newPos = _start + offset;
                        break;

                    case SeekOrigin.Current:
                        newPos = _readPos + offset;
                        break;

                    case SeekOrigin.End:
                        newPos = _end + offset;
                        break;
                }

                if (newPos < _start)
                {
                    throw new ArgumentException("Negative position is invalid");
                }

                if (newPos >= _end)
                {
                    throw new IOException("Cannot seek past end");
                }
                _readPos = newPos;
                return _readPos;
            }

            public override void Flush()
            {
            }

            public override long Position
            {
                get { return _readPos - _start; }
                set
                {
                    long newPos = _start + value;
                    if (newPos < _start)
                    {
                        throw new ArgumentException("Negative position is invalid");
                    }
                    if (newPos >= _end)
                    {
                        throw new InvalidOperationException("Cannot seek past end");
                    }
                    _readPos = newPos;
                }
            }

            public override long Length
            {
                get { return _length; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override bool CanSeek
            {
                get { return true; }
            }

            public override bool CanRead
            {
                get { return true; }
            }

            #endregion

            #region Fields

            private Stream _baseStream;
            private long _start;
            private long _length;
            private long _readPos;
            private long _end;

            #endregion
        }

        #endregion

        #region Class StiZipConstants
        private sealed class StiZipConstants
        {
            public const int VersionMadeBy = 51;
            public const int VersionZip64 = 45;
            public const int LocalHeaderBaseSize = 30;
            public const int EndOfCentralRecordBaseSize = 22;

            public const int LocalHeaderSignature = 'P' | ('K' << 8) | (3 << 16) | (4 << 24);
            public const int CentralHeaderSignature = 'P' | ('K' << 8) | (1 << 16) | (2 << 24);
            public const int Zip64CentralFileHeaderSignature = 'P' | ('K' << 8) | (6 << 16) | (6 << 24);
            public const int Zip64CentralDirLocatorSignature = 'P' | ('K' << 8) | (6 << 16) | (7 << 24);
            public const int EndOfCentralDirectorySignature = 'P' | ('K' << 8) | (5 << 16) | (6 << 24);

            private static int defaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;

            public static string ConvertToStringExt(int flags, byte[] data, int count)
            {
                if (data == null)
                {
                    return string.Empty;
                }

                if ((flags & (int) GeneralBitFlags.UnicodeText) != 0)
                {
                    return Encoding.UTF8.GetString(data, 0, count);
                }
                return Encoding.GetEncoding(defaultCodePage).GetString(data, 0, count);
            }

        }

        #endregion

        #region Class StiZipException

        public class StiZipException : ApplicationException
        {
            public StiZipException()
                : base(string.Empty)
            {
            }

            public StiZipException(string message)
                : base(message)
            {
            }
        }

        #endregion

        #region Class StiZipHelperStream
        internal class StiZipHelperStream : Stream
        {
            #region Constructors
            public StiZipHelperStream(string name)
            {
                stream_ = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
                isOwner_ = true;
            }

            public StiZipHelperStream(Stream stream)
            {
                stream_ = stream;
            }
            #endregion

            #region Base Stream Methods

            public override bool CanRead
            {
                get { return stream_.CanRead; }
            }

            public override bool CanSeek
            {
                get { return stream_.CanSeek; }
            }


            public override long Length
            {
                get { return stream_.Length; }
            }

            public override long Position
            {
                get { return stream_.Position; }
                set { stream_.Position = value; }
            }

            public override bool CanWrite
            {
                get { return stream_.CanWrite; }
            }

            public override void Flush()
            {
                stream_.Flush();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return stream_.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                stream_.SetLength(value);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return stream_.Read(buffer, offset, count);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                stream_.Write(buffer, offset, count);
            }

            public override void Close()
            {
                Stream toClose = stream_;
                stream_ = null;
                if (isOwner_ && (toClose != null))
                {
                    isOwner_ = false;
                    toClose.Close();
                }
            }
            #endregion

            #region Methods
            public long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize,
                int maximumVariableData)
            {
                long pos = endLocation - minimumBlockSize;
                if (pos < 0)
                {
                    return -1;
                }

                long giveUpMarker = Math.Max(pos - maximumVariableData, 0);

                do
                {
                    if (pos < giveUpMarker)
                    {
                        return -1;
                    }
                    Seek(pos--, SeekOrigin.Begin);
                } while (ReadLEInt() != signature);

                return Position;
            }

            public int ReadLEShort()
            {
                int byteValue1 = stream_.ReadByte();

                if (byteValue1 < 0)
                {
                    throw new EndOfStreamException();
                }

                int byteValue2 = stream_.ReadByte();
                if (byteValue2 < 0)
                {
                    throw new EndOfStreamException();
                }

                return byteValue1 | (byteValue2 << 8);
            }

            public int ReadLEInt()
            {
                return ReadLEShort() | (ReadLEShort() << 16);
            }

            public long ReadLELong()
            {
                return (uint) ReadLEInt() | ((long) ReadLEInt() << 32);
            }
            #endregion

            #region Fields
            private bool isOwner_;
            private Stream stream_;
            #endregion
        }
        #endregion

        #region Class StiZipExtraData
        private sealed class StiZipExtraData
        {
            #region Constructors
            public StiZipExtraData()
            {
                Clear();
            }

            public StiZipExtraData(byte[] data)
            {
                if (data == null)
                {
                    _data = new byte[0];
                }
                else
                {
                    _data = data;
                }
            }
            #endregion

            #region Methods
            public void Clear()
            {
                if ((_data == null) || (_data.Length != 0))
                {
                    _data = new byte[0];
                }
            }

            public bool Find(int headerID)
            {
                _readValueStart = _data.Length;
                _readValueLength = 0;
                _index = 0;

                int localLength = _readValueStart;
                int localTag = headerID - 1;

                while ((localTag != headerID) && (_index < _data.Length - 3))
                {
                    localTag = ReadShortInternal();
                    localLength = ReadShortInternal();
                    if (localTag != headerID)
                    {
                        _index += localLength;
                    }
                }

                bool result = (localTag == headerID) && ((_index + localLength) <= _data.Length);

                if (result)
                {
                    _readValueStart = _index;
                    _readValueLength = localLength;
                }

                return result;
            }
            #endregion

            #region Methods.Reading
            public long ReadLong()
            {
                ReadCheck(8);
                return (ReadInt() & 0xffffffff) | (((long) ReadInt()) << 32);
            }

            public int ReadInt()
            {
                ReadCheck(4);

                int result = _data[_index] + (_data[_index + 1] << 8) +
                             (_data[_index + 2] << 16) + (_data[_index + 3] << 24);
                _index += 4;
                return result;
            }

            private void ReadCheck(int length)
            {
                if ((_readValueStart > _data.Length) ||
                    (_readValueStart < 4))
                {
                    throw new StiZipException("Find must be called before calling a Read method");
                }

                if (_index > _readValueStart + _readValueLength - length)
                {
                    throw new StiZipException("End of extra data");
                }

                if (_index + length < 4)
                {
                    throw new StiZipException("Cannot read before start of tag");
                }
            }

            private int ReadShortInternal()
            {
                if (_index > _data.Length - 2)
                {
                    throw new StiZipException("End of extra data");
                }

                int result = _data[_index] + (_data[_index + 1] << 8);
                _index += 2;
                return result;
            }
            #endregion

            #region Properties
            public int ValueLength
            {
                get { return _readValueLength; }
            }
            #endregion

            #region Fields
            private int _index;
            private int _readValueStart;
            private int _readValueLength;

            private byte[] _data;
            #endregion
        }
        #endregion

        #region Utils
        private class Utils
        {
            public static void ReadFully(Stream stream, byte[] buffer)
            {
                ReadFully(stream, buffer, 0, buffer.Length);
            }

            public static void ReadFully(Stream stream, byte[] buffer, int offset, int count)
            {
                if (stream == null)
                {
                    throw new ArgumentNullException("stream");
                }
                if (buffer == null)
                {
                    throw new ArgumentNullException("buffer");
                }
                if ((offset < 0) || (offset > buffer.Length))
                {
                    throw new ArgumentOutOfRangeException("offset");
                }
                if ((count < 0) || (offset + count > buffer.Length))
                {
                    throw new ArgumentOutOfRangeException("count");
                }

                while (count > 0)
                {
                    int readCount = stream.Read(buffer, offset, count);
                    if (readCount <= 0)
                    {
                        throw new EndOfStreamException();
                    }
                    offset += readCount;
                    count -= readCount;
                }
            }
        }
        #endregion

        #region Dispose
        void IDisposable.Dispose()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }

        ~StiZipReader20()
        {
            DisposeInternal(false);
        }

        public void Dispose(bool disposing)
        {
            DisposeInternal(disposing);
        }

        private void DisposeInternal(bool disposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _entries = new StiZipEntry[0];

                if (isStreamOwner && (_baseStream != null))
                {
                    _baseStream.Close();
                }
            }
        }
        #endregion

        #region this
        private void OpenZipFile(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (stream.Length == 0)
            {
                throw new ArgumentException("Stream is empty", "stream");
            }
            if (!stream.CanSeek)
            {
                throw new ArgumentException("Stream is not seekable", "stream");
            }

            _baseStream = stream;
            isStreamOwner = true;

            try
            {
                ReadEntries();
            }
            catch
            {
                Dispose(true);
                throw;
            }
        }

        public StiZipReader20(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            OpenZipFile(fs);
        }

        public StiZipReader20(Stream stream)
        {
            OpenZipFile(stream);
        }
        #endregion
    }
}
