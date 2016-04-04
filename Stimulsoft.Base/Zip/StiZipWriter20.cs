using System;
using System.Collections;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Stimulsoft.Base.Zip
{
    public class StiZipWriter20
    {
        #region Constants
        private static class Constants
        {
            public const int Version = 20;

            public const int LocalHeaderBaseSize = 30;      //excluding variable length fields at end
            public const int CentralHeaderBaseSize = 46;    //excluding variable length fields at end
            public const int DataDescriptorSize = 16;

            public const int CentralHeaderSignature         = 0x02014B50;
            public const int LocalHeaderSignature           = 0x04034B50;
            public const int EndOfCentralDirectorySignature = 0x06054B50;
            public const int DataDescriptorSignature        = 0x08074B50;

            public static Encoding DefaultEncoding = Encoding.GetEncoding(System.Globalization.CultureInfo.InstalledUICulture.TextInfo.OEMCodePage);
        }
        #endregion

        #region Static methods
        private static byte[] ConvertToArray(bool useUnicode, string str)
        {
            if (str == null)
            {
                return new byte[0];
            }
            if (useUnicode)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            else
            {
                return Constants.DefaultEncoding.GetBytes(str);
            }
        }

        private static uint GetDosTime(DateTime dt)
        {
            return ((uint)dt.Year - 1980 & 0x7f) << 25 |
                    ((uint)dt.Month) << 21 |
                    ((uint)dt.Day) << 16 |
                    ((uint)dt.Hour) << 11 |
                    ((uint)dt.Minute) << 5 |
                    ((uint)dt.Second) >> 1;
        }
        #endregion

        #region Enumerations
        private enum CompressionMethod
        {
            /// <summary>
            /// Direct copy of the file contents
            /// </summary>
            Stored = 0,

            /// <summary>
            /// Compression method LZ/Huffman
            /// </summary>
            Deflated = 8
        }

        /// <summary>
        /// Defines the contents of the general bit flags field for an archive entry.
        /// </summary>
        [Flags]
        private enum GeneralBitFlags
        {
            None = 0,
            /// <summary>
            /// Bit 3 if set indicates a trailing data desciptor is appended to the entry data
            /// </summary>
            Descriptor = 0x0008,
            /// <summary>
            /// Bit 11 if set indicates the filename and comment fields for this file must be encoded using UTF-8.
            /// </summary>
            UnicodeText = 0x0800,
        }
        #endregion

        #region Class ZipException
        public class ZipException : ApplicationException
        {
            public ZipException(string message)
                : base(message)
            {
            }
        }
        #endregion

        #region Class Crc32
        public sealed class Crc32
        {
            readonly static uint CrcSeed = 0xFFFFFFFF;

            readonly static uint[] CrcTable = new uint[]
            {
                0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419, 0x706AF48F, 0xE963A535, 0x9E6495A3,
                0x0EDB8832, 0x79DCB8A4, 0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 0x90BF1D91,
                0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE, 0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7,
                0x136C9856, 0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9, 0xFA0F3D63, 0x8D080DF5,
                0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172, 0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
                0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 0x45DF5C75, 0xDCD60DCF, 0xABD13D59,
                0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116, 0x21B4F4B5, 0x56B3C423, 0xCFBA9599, 0xB8BDA50F,
                0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924, 0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D,
                0x76DC4190, 0x01DB7106, 0x98D220BC, 0xEFD5102A, 0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
                0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818, 0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
                0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E, 0x6C0695ED, 0x1B01A57B, 0x8208F4C1, 0xF50FC457,
                0x65B0D9C6, 0x12B7E950, 0x8BBEB8EA, 0xFCB9887C, 0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65,
                0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB,
                0x4369E96A, 0x346ED9FC, 0xAD678846, 0xDA60B8D0, 0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9,
                0x5005713C, 0x270241AA, 0xBE0B1010, 0xC90C2086, 0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
                0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD,
                0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A, 0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683,
                0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8, 0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1,
                0xF00F9344, 0x8708A3D2, 0x1E01F268, 0x6906C2FE, 0xF762575D, 0x806567CB, 0x196C3671, 0x6E6B06E7,
                0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC, 0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
                0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 0xA6BC5767, 0x3FB506DD, 0x48B2364B,
                0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60, 0xDF60EFC3, 0xA867DF55, 0x316E8EEF, 0x4669BE79,
                0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236, 0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F,
                0xC5BA3BBE, 0xB2BD0B28, 0x2BB45A92, 0x5CB36A04, 0xC2D7FFA7, 0xB5D0CF31, 0x2CD99E8B, 0x5BDEAE1D,
                0x9B64C2B0, 0xEC63F226, 0x756AA39C, 0x026D930A, 0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
                0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38, 0x92D28E9B, 0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21,
                0x86D3D2D4, 0xF1D4E242, 0x68DDB3F8, 0x1FDA836E, 0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 0x18B74777,
                0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C, 0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45,
                0xA00AE278, 0xD70DD2EE, 0x4E048354, 0x3903B3C2, 0xA7672661, 0xD06016F7, 0x4969474D, 0x3E6E77DB,
                0xAED16A4A, 0xD9D65ADC, 0x40DF0B66, 0x37D83BF0, 0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
                0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 0xCDD70693, 0x54DE5729, 0x23D967BF,
                0xB3667A2E, 0xC4614AB8, 0x5D681B02, 0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D
            };

            /// <summary>
            /// The crc data checksum so far.
            /// </summary>
            private uint crc;

            /// <summary>
            /// Returns the CRC32 data checksum computed so far.
            /// </summary>
            public long Value
            {
                get
                {
                    return (long)crc;
                }
                set
                {
                    crc = (uint)value;
                }
            }

            /// <summary>
            /// Resets the CRC32 data checksum as if no update was ever called.
            /// </summary>
            public void Reset()
            {
                crc = 0;
            }

            /// <summary>
            /// Adds the byte array to the data checksum.
            /// </summary>
            /// <param name = "buffer">
            /// The buffer which contains the data
            /// </param>
            /// <param name = "offset">
            /// The offset in the buffer where the data starts
            /// </param>
            /// <param name = "count">
            /// The number of data bytes to update the CRC with.
            /// </param>
            public void Update(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                {
                    throw new ArgumentNullException("Buffer is null");
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("Count cannot be less than zero");
                }
                if (offset < 0 || offset + count > buffer.Length)
                {
                    throw new ArgumentOutOfRangeException("Incorrect offset");
                }

                crc ^= CrcSeed;

                while (--count >= 0)
                {
                    crc = CrcTable[(crc ^ buffer[offset++]) & 0xFF] ^ (crc >> 8);
                }

                crc ^= CrcSeed;
            }
        }
        #endregion

        #region Class ZipEntry
        private class ZipEntry
        {
            #region Variables
            private string name;
            private ulong size;
            private ulong compressedSize;
            private uint crc;
            private DateTime dateTime;
            private CompressionMethod method = CompressionMethod.Deflated;
            private int flags;          //general purpose bit flags
            private long offset;        //offset in ZipOutputStream
            private bool isUnknownSize = true;
            #endregion

            #region Properties
            public bool IsUnknownSize
            {
                get
                {
                    return isUnknownSize;
                }
            }
 
            public int Flags
            {
                get
                {
                    return flags;
                }
                set
                {
                    flags = value;
                }
            }

            /// <summary>
            /// Get/set a flag indicating wether entry name are encoded in Unicode UTF8
            /// </summary>
            public bool UseUnicodeText
            {
                get
                {
                    return (flags & (int)GeneralBitFlags.UnicodeText) != 0;
                }
                set
                {
                    if (value)
                    {
                        flags |= (int)GeneralBitFlags.UnicodeText;
                    }
                    else
                    {
                        flags &= ~(int)GeneralBitFlags.UnicodeText;
                    }
                }
            }

            /// <summary>
            /// Get/set offset for use in central header
            /// </summary>
            public long Offset
            {
                get
                {
                    return offset;
                }
                set
                {
                    offset = value;
                }
            }

            /// <summary>
            /// Gets/Sets the time of last modification of the entry.
            /// </summary>
            public DateTime DateTime
            {
                get
                {
                    return dateTime;
                }
                set
                {
                    dateTime = value;
                }
            }

            /// <summary>
            /// Returns the entry name
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Gets/Sets the size of the uncompressed data.
            /// </summary>
            public long Size
            {
                get
                {
                    return (long)size;
                }
                set
                {
                    this.size = (ulong)value;
                    isUnknownSize = false;
                }
            }

            /// <summary>
            /// Gets/Sets the size of the compressed data.
            /// </summary>
            public long CompressedSize
            {
                get
                {
                    return (long)compressedSize;
                }
                set
                {
                    this.compressedSize = (ulong)value;
                }
            }

            /// <summary>
            /// Gets/Sets the crc of the uncompressed data.
            /// </summary>
            public long Crc
            {
                get
                {
                    return (long)crc;
                }
                set
                {
                    this.crc = (uint)value;
                }
            }

            public CompressionMethod CompressionMethod
            {
                get
                {
                    return method;
                }

                set
                {
                    this.method = value;
                }
            }

            /// <summary>
            /// Gets a value indicating if the entry is a directory.
            /// </summary>
            public bool IsDirectory
            {
                get
                {
                    int nameLength = name.Length;
                    return ((nameLength > 0) && ((name[nameLength - 1] == '/') || (name[nameLength - 1] == '\\')));
                }
            }
            #endregion

            #region Constructors
            public ZipEntry(string name) : this(name, CompressionMethod.Deflated)
            {
            }

            public ZipEntry(string name, CompressionMethod method)
            {
                if (name == null)
                {
                    throw new ZipException("ZipEntry name is null");
                }
                if (name.Length > 0xFFFF)
                {
                    throw new ZipException("ZipEntry name is too long");
                }

                this.name = name;
                this.DateTime = System.DateTime.Now;
                this.method = method;
                this.flags = (int)GeneralBitFlags.None;
                this.isUnknownSize = true;
            }
            #endregion
        }
        #endregion

        #region Class DeflaterOutputStream
        private class DeflaterOutputStream : Stream
        {
            #region Variables
            protected int TotalOut = 0;
            protected Stream baseOutputStream;
            private DeflateStream def = null;
            private bool isClosed = false;
            private bool isFinished = false;
            private bool isStreamOwner = false;
            #endregion

            #region Properties
            public bool IsStreamOwner
            {
                get
                {
                    return isStreamOwner;
                }
                set
                {
                    isStreamOwner = value;
                }
            }

            ///	<summary>
            /// Allows client to determine if an entry can be patched after its added
            /// </summary>
            public bool CanPatchEntries
            {
                get
                {
                    return baseOutputStream.CanSeek;
                }
            }
            #endregion

            #region Stream Overrides
            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return baseOutputStream.CanWrite;
                }
            }

            public override long Length
            {
                get
                {
                    return baseOutputStream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    return baseOutputStream.Position;
                }
                set
                {
                    throw new NotSupportedException("Position property not supported");
                }
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException("DeflaterOutputStream Seek not supported");
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
            }

            public override int ReadByte()
            {
                throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException("DeflaterOutputStream Read not supported");
            }

            public override void WriteByte(byte value)
            {
                byte[] b = new byte[1];
                b[0] = value;
                Write(b, 0, 1);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (isFinished)
                {
                    throw new ZipException("DeflateStream is finished");
                }
                long positionOld = baseOutputStream.Position;
                def.Write(buffer, offset, count);
                TotalOut += (int)(baseOutputStream.Position - positionOld);
            }

            public void Reset()
            {
                isFinished = false;
                def = new DeflateStream(baseOutputStream, CompressionMode.Compress, true);
                TotalOut = 0;
            }

            public virtual void Finish()
            {
                if (!isFinished)
                {
                    isFinished = true;
                    long positionOld = baseOutputStream.Position;
                    def.Close();
                    def = null;
                    TotalOut += (int)(baseOutputStream.Position - positionOld);
                    baseOutputStream.Flush();
                }
            }

            public override void Flush()
            {
                def.Flush();
                baseOutputStream.Flush();
            }

            public override void Close()
            {
                if (!isClosed)
                {
                    isClosed = true;
                    Finish();
                    if (isStreamOwner)
                    {
                        baseOutputStream.Close();
                    }
                    baseOutputStream = null;
                }
            }
            #endregion

            #region Constructors
            public DeflaterOutputStream(Stream baseOutputStream)
            {
                if (baseOutputStream == null)
                {
                    throw new ZipException("OutputStream cannot be null");
                }
                if (baseOutputStream.CanWrite == false)
                {
                    throw new ZipException("OutputStream must support writing");
                }

                this.baseOutputStream = baseOutputStream;
            }
            #endregion
        }
        #endregion

        #region Class ZipOutputStream
        private class ZipOutputStream : DeflaterOutputStream
        {
            #region Variables
            private ArrayList entriesList = new ArrayList();
            private Crc32 crc = new Crc32();
            private ZipEntry currentEntry;
            private long size;  //Used to track the size of data for an entry during writing.
            private long offset;
            bool patchEntryHeader;
            long crcPatchPos = 0;
            private const int extraLength = 0;
            private const int commentLength = 0;
            #endregion

            #region Utils.WriteData
            private void WriteDataShort(int value)
            {
                baseOutputStream.WriteByte((byte)(value & 0xff));
                baseOutputStream.WriteByte((byte)((value >> 8) & 0xff));
            }
            private void WriteDataInt(int value)
            {
                WriteDataShort(value);
                WriteDataShort(value >> 16);
            }
            private void WriteDataLong(long value)
            {
                WriteDataInt((int)value);
                WriteDataInt((int)(value >> 32));
            }
            #endregion

            #region Methods.PutNextEntry
            /// <summary>
            /// Put next entry into Zip file
            /// </summary>
            /// <param name="entry">Entry to put into zip file</param>
            public void PutNextEntry(ZipEntry entry)
            {
                if (entry == null)
                {
                    throw new ZipException("ZipEntry is null");
                }
                if (entriesList == null)
                {
                    throw new ZipException("ZipOutputStream was finished");
                }
                if (entriesList.Count == int.MaxValue)
                {
                    throw new ZipException("Too many entries for Zip file");
                }

                if (currentEntry != null)
                {
                    CloseEntry();
                }

                if (entry.CompressionMethod == CompressionMethod.Stored)
                {
                    if (CanPatchEntries != true)
                    {
                        // Can't patch entries so storing is not possible.
                        entry.CompressionMethod = CompressionMethod.Deflated;
                    }
                }

                patchEntryHeader = true;
                if (CanPatchEntries == false)
                {
                    // Only way to record size and compressed size is to append a data descriptor after compressed data.
                    entry.Flags |= (int)GeneralBitFlags.Descriptor;
                    patchEntryHeader = false;
                }
                entry.Offset = offset;

                #region Write the local file header
                WriteDataInt(Constants.LocalHeaderSignature);
                WriteDataShort(Constants.Version);
                WriteDataShort(entry.Flags);
                WriteDataShort((byte)entry.CompressionMethod);
                WriteDataInt((int)GetDosTime(entry.DateTime));

                if (patchEntryHeader)
                {
                    crcPatchPos = baseOutputStream.Position;
                }
                WriteDataInt(0);	// Crc
                WriteDataInt(0);	// Compressed size
                WriteDataInt(0);	// Uncompressed size

                byte[] name = ConvertToArray(entry.UseUnicodeText, entry.Name);
                if (name.Length > 0xFFFF)
                {
                    throw new ZipException("Entry name too long");
                }
                WriteDataShort(name.Length);
                WriteDataShort(extraLength);
                if (name.Length > 0)
                {
                    baseOutputStream.Write(name, 0, name.Length);
                }
                //no extra

                offset += Constants.LocalHeaderBaseSize + name.Length + extraLength;
                #endregion

                currentEntry = entry;
                size = 0;
                crc.Reset();
                if (entry.CompressionMethod == CompressionMethod.Deflated)
                {
                    Reset();
                }
            }
            #endregion

            #region Methods.Write
            /// <summary>
            /// Write data to the current entry
            /// </summary>
            public override void Write(byte[] buffer, int offset, int count)
            {
                if (currentEntry == null)
                {
                    throw new ZipException("No open entry");
                }
                if (buffer == null)
                {
                    throw new ZipException("Buffer is null");
                }
                if (offset < 0)
                {
                    throw new ZipException("Offset cannot be negative");
                }
                if (count < 0)
                {
                    throw new ZipException("Count cannot be negative");
                }
                if ((buffer.Length - offset) < count)
                {
                    throw new ZipException("Invalid offset/count combination");
                }

                crc.Update(buffer, offset, count);
                size += count;

                switch (currentEntry.CompressionMethod)
                {
                    case CompressionMethod.Deflated:
                        base.Write(buffer, offset, count);
                        break;

                    case CompressionMethod.Stored:
                        baseOutputStream.Write(buffer, offset, count);
                        break;
                }
            }
            #endregion

            #region Methods.CloseEntry
            /// <summary>
            /// Close current entry
            /// </summary>
            public void CloseEntry()
            {
                if (currentEntry == null)
                {
                    throw new ZipException("No open entry");
                }

                if (currentEntry.CompressionMethod == CompressionMethod.Deflated)
                {
                    base.Finish();
                }

                long csize = (currentEntry.CompressionMethod == CompressionMethod.Deflated ? TotalOut : size);

                currentEntry.Size = size;
                currentEntry.CompressedSize = csize;
                currentEntry.Crc = crc.Value;

                offset += csize;

                // Patch the header if possible
                if (patchEntryHeader == true)
                {
                    patchEntryHeader = false;

                    long curPos = baseOutputStream.Position;
                    baseOutputStream.Seek(crcPatchPos, SeekOrigin.Begin);
                    WriteDataInt((int)currentEntry.Crc);
                    WriteDataInt((int)currentEntry.CompressedSize);
                    WriteDataInt((int)currentEntry.Size);
                    baseOutputStream.Seek(curPos, SeekOrigin.Begin);
                }

                // Add data descriptor if flagged as required
                if ((currentEntry.Flags & (int)GeneralBitFlags.Descriptor) != 0)
                {
                    WriteDataInt(Constants.DataDescriptorSignature);
                    WriteDataInt((int)currentEntry.Crc);
                    WriteDataInt((int)currentEntry.CompressedSize);
                    WriteDataInt((int)currentEntry.Size);
                    offset += Constants.DataDescriptorSize;
                }

                entriesList.Add(currentEntry);
                currentEntry = null;
            }
            #endregion

            #region Methods.Finish
            public override void Finish()
            {
                if (entriesList == null)
                {
                    return;
                }
                if (currentEntry != null)
                {
                    CloseEntry();
                }

                long numEntries = entriesList.Count;
                long sizeEntries = 0;

                foreach (ZipEntry entry in entriesList)
                {
                    WriteDataInt(Constants.CentralHeaderSignature);
                    WriteDataShort(Constants.Version);
                    WriteDataShort(Constants.Version);
                    WriteDataShort(entry.Flags);
                    WriteDataShort((short)entry.CompressionMethod);
                    WriteDataInt((int)GetDosTime(entry.DateTime));
                    WriteDataInt((int)entry.Crc);
                    WriteDataInt((int)entry.CompressedSize);
                    WriteDataInt((int)entry.Size);

                    byte[] name = ConvertToArray(entry.UseUnicodeText, entry.Name);
                    if (name.Length > 0xFFFF)
                    {
                        throw new ZipException("Entry name too long.");
                    }

                    WriteDataShort(name.Length);
                    WriteDataShort(extraLength);
                    WriteDataShort(commentLength);
                    WriteDataShort(0);	// disk number
                    WriteDataShort(0);	// internal file attributes
                    // no external file attributes

                    if (entry.IsDirectory)
                    {                         // mark entry as directory
                        WriteDataInt(16);
                    }
                    else
                    {
                        WriteDataInt(0);
                    }
                    WriteDataInt((int)entry.Offset);

                    if (name.Length > 0)
                    {
                        baseOutputStream.Write(name, 0, name.Length);
                    }

                    //no extra
                    //no comment

                    sizeEntries += Constants.CentralHeaderBaseSize + name.Length + extraLength + commentLength;
                }

                long startOfCentralDirectory = offset;

                #region WriteEndOfCentralDirectory
                WriteDataInt(Constants.EndOfCentralDirectorySignature);
                WriteDataShort(0);                    // number of this disk
                WriteDataShort(0);                    // no of disk with start of central dir
                WriteDataShort((short)numEntries);    // entries in central dir for this disk
                WriteDataShort((short)numEntries);    // total entries in central directory
                WriteDataInt((int)sizeEntries);       // size of the central directory
                WriteDataInt((int)startOfCentralDirectory);   // offset of start of central dir
                WriteDataShort(0);    //commentLength
                #endregion

                entriesList = null;
            }
            #endregion

            #region Methods.Close
            public override void Close()
            {
                entriesList = null;
                crc = null;
                currentEntry = null;
                base.Close();
            }
            #endregion

            #region Constructors
            /// <summary>
            /// Creates a new Zip output stream, writing a zip archive.
            /// </summary>
            /// <param name="baseOutputStream">
            /// The output stream to which the archive contents are written.
            /// </param>
            public ZipOutputStream(Stream baseOutputStream)
                : base(baseOutputStream)
            {
            }
            #endregion
        }
        #endregion

        #region ZipHelper
        private Stream mainStream = null;
        private ZipOutputStream zipStream = null;

        public void Begin(Stream stream, bool leaveOpen)
        {
            mainStream = stream;
            if (mainStream == null)
            {
                throw new ZipException("Output stream is null");
            }
            zipStream = new ZipOutputStream(mainStream);
            zipStream.IsStreamOwner = !leaveOpen;
        }

        public void AddFile(string fileName, MemoryStream dataStream)
        {
            AddFile(fileName, dataStream, false);
        }

        public void AddFile(string fileName, MemoryStream dataStream, bool closeDataStream)
        {
            ZipEntry entry = new ZipEntry(fileName);
            //entry.UseUnicodeText = true;
            zipStream.PutNextEntry(entry);
            dataStream.WriteTo(zipStream);
            zipStream.CloseEntry();
            if (closeDataStream) dataStream.Close();
        }

        public void End()
        {
            zipStream.Finish();
            zipStream.Close();
            mainStream = null;
            zipStream = null;
        }

        public StiZipWriter20()
        {
            mainStream = null;
            zipStream = null;
        }
        #endregion
    }

}
