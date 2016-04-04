using System;
using System.IO.Compression;
using System.IO;
using Stimulsoft.Base.Zip;

namespace Stimulsoft.Base.Excel
{
	public class ZipWorker : IDisposable
	{
		#region Members and Properties

		private bool disposed;
		private bool _isCleaned;

		private const string TMP = "TMP_Z";
		private const string FOLDER_xl = "xl";
		private const string FOLDER_worksheets = "worksheets";
		private const string FILE_sharedStrings = "sharedStrings.{0}";
		private const string FILE_styles = "styles.{0}";
		private const string FILE_workbook = "workbook.{0}";
		private const string FILE_sheet = "sheet{0}.{1}";
		private const string FOLDER_rels = "_rels";
		private const string FILE_rels = "workbook.{0}.rels";

		private string _tempPath;
		private string _tempEnv;
		private string _exceptionMessage;
		private string _xlPath;
		private string _format = "xml";

		private bool _isValid;
		//private bool _isBinary12Format;

		/// <summary>
		/// Gets a value indicating whether this instance is valid.
		/// </summary>
		/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		public bool IsValid
		{
			get { return _isValid; }
		}

		/// <summary>
		/// Gets the temp path for extracted files.
		/// </summary>
		/// <value>The temp path for extracted files.</value>
		public string TempPath
		{
			get { return _tempPath; }
		}

		/// <summary>
		/// Gets the exception message.
		/// </summary>
		/// <value>The exception message.</value>
		public string ExceptionMessage
		{
			get { return _exceptionMessage; }
		}

		#endregion

		public ZipWorker()
		{
			_tempEnv = Path.GetTempPath();
		}

		/// <summary>
		/// Extracts the specified zip file stream.
		/// </summary>
		/// <param name="fileStream">The zip file stream.</param>
		/// <returns></returns>
		public bool Extract(Stream fileStream)
		{
			if (null == fileStream) return false;

			CleanFromTemp(false);

			NewTempPath();

			_isValid = true;

			try
			{
                #if NET45
                using (var zipArchive = new ZipArchive(fileStream))
                #else
                using (var zipArchive = new StiZipReader20(fileStream))
                #endif
                {
                    foreach (var zipEntry in zipArchive.Entries)
                    {
                        string tPath = Path.Combine(_tempPath, zipEntry.FullName.Replace('/', '\\'));
                        var dir = Path.GetDirectoryName(tPath);

                        if (dir != null && !Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        zipEntry.ExtractToFile(tPath, true);
                    }
                }
			}
			catch (Exception ex)
			{
				_isValid = false;
				_exceptionMessage = ex.Message;

				CleanFromTemp(true); //true tells CleanFromTemp not to raise an IO Exception if this operation fails. If it did then the real error here would be masked
				
			}
			finally
			{
				fileStream.Close();

				//if (null != zipFile) zipFile.Close();
			}

			return _isValid && CheckFolderTree();
		}

		/// <summary>
		/// Gets the shared strings stream.
		/// </summary>
		/// <returns></returns>
		public Stream GetSharedStringsStream()
		{
			return GetStream(Path.Combine(_xlPath, string.Format(FILE_sharedStrings, _format)));
		}

		/// <summary>
		/// Gets the styles stream.
		/// </summary>
		/// <returns></returns>
		public Stream GetStylesStream()
		{
			return GetStream(Path.Combine(_xlPath, string.Format(FILE_styles, _format)));
		}

		/// <summary>
		/// Gets the workbook stream.
		/// </summary>
		/// <returns></returns>
		public Stream GetWorkbookStream()
		{
			return GetStream(Path.Combine(_xlPath, string.Format(FILE_workbook, _format)));
		}

		/// <summary>
		/// Gets the worksheet stream.
		/// </summary>
		/// <param name="sheetId">The sheet id.</param>
		/// <returns></returns>
		public Stream GetWorksheetStream(int sheetId)
		{
			return GetStream(Path.Combine(
				Path.Combine(_xlPath, FOLDER_worksheets),
				string.Format(FILE_sheet, sheetId, _format)));
		}

        public Stream GetWorksheetStream(string sheetPath)
        {
            return GetStream(Path.Combine(_xlPath, sheetPath));
        }


		/// <summary>
		/// Gets the workbook rels stream.
		/// </summary>
		/// <returns></returns>
		public Stream GetWorkbookRelsStream()
		{
			return GetStream(Path.Combine(_xlPath, Path.Combine(FOLDER_rels, string.Format(FILE_rels, _format))));
		}

		private void CleanFromTemp(bool catchIoError)
		{
			if (string.IsNullOrEmpty(_tempPath)) return;

			_isCleaned = true;

			try
			{
				if (Directory.Exists(_tempPath))
				{
					Directory.Delete(_tempPath, true);
				}
			}
			catch (IOException)
			{
				if (!catchIoError)
					throw;
			}
			
		}

		private void NewTempPath()
		{
			_tempPath = Path.Combine(_tempEnv, TMP + DateTime.Now.ToFileTimeUtc().ToString());

			_isCleaned = false;

			Directory.CreateDirectory(_tempPath);
		}

		private bool CheckFolderTree()
		{
			_xlPath = Path.Combine(_tempPath, FOLDER_xl);

			return Directory.Exists(_xlPath) &&
				Directory.Exists(Path.Combine(_xlPath, FOLDER_worksheets)) &&
				File.Exists(Path.Combine(_xlPath, FILE_workbook)) &&
				File.Exists(Path.Combine(_xlPath, FILE_styles));
		}

		private static Stream GetStream(string filePath)
		{
			if (File.Exists(filePath))
			{
				return File.Open(filePath, FileMode.Open, FileAccess.Read);
			}
			else
			{
				return null;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				if (disposing)
				{
					if (!_isCleaned)
						CleanFromTemp(false);
				}

				disposed = true;
			}
		}

		~ZipWorker()
		{
			Dispose(false);
		}

		#endregion
	}
}