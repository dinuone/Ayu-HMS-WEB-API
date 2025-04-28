namespace Harendra_Ayurveda_HMS_WEB_API.Helpers.Utils
{
    public class FileUploadHandler
    {
        private static readonly string[] _uploadFoldervalues = { "Uploads/QrCode", "Uploads/PatientReports", };

        public static async Task<FileUploadResponse> UploadFileAsync(string base64String, string fileName, string identifier, int uploadFolder = 0)
        {
            string parentFolderName = SetParentFolderName(uploadFolder);

            if (string.IsNullOrEmpty(base64String))
            {
                throw new ArgumentNullException("File content cannot be null or empty.", nameof(base64String));
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            try
            {
                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(base64String);
                }
                catch (FormatException ex)
                {
                    throw new ArgumentException("Invalid base64 string format.", nameof(base64String), ex);
                }

                long fileSizeInByte = GetFileSizeFromBase64(base64String);

                string uploadsDirectory;
                string fullPath;

                // ⚡ Here we check if the upload folder is QR Code (0 based on your _uploadFoldervalues)
                if (uploadFolder == 0) // 0 -> Uploads/QrCode
                {
                    uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), parentFolderName);
                    fullPath = Path.Combine(parentFolderName, fileName);
                }
                else
                {
                    uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), parentFolderName, identifier);
                    fullPath = Path.Combine(parentFolderName, identifier, fileName);
                }

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                await File.WriteAllBytesAsync(fullPath, fileBytes);

                return new FileUploadResponse
                {
                    FilePath = fullPath.Replace("\\", "/"),
                    FileSize = ConvertFileSize(fileSizeInByte),
                    OriginalFileName = fileName
                };

            }
            catch (DirectoryNotFoundException ex)
            {
                throw new IOException("The directory could not be created or accessed.", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("Permission denied to create or write to the directory.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("An error occurred while writing the file.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred during file upload.", ex);
            }
        }


        private static string ConvertFileSize(long fileSizeInBytes)
        {
            if (fileSizeInBytes >= 1048576) // 1 MB in bytes
                return $"{fileSizeInBytes / 1048576.0:F2} MB";
            else
                return $"{fileSizeInBytes / 1024.0:F2} KB";
        }

        private static long GetFileSizeFromBase64(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new ArgumentException("Base64 string cannot be null or empty.", nameof(base64String));

            // Calculate the file size in bytes from the length of the Base64 string
            int padding = base64String.EndsWith("==") ? 2 : base64String.EndsWith("=") ? 1 : 0;
            return (base64String.Length * 3 / 4) - padding;
        }

        private static string SetParentFolderName(int uploadFolder)
        {
            string parentFolder = "Uploads/New";
            if (uploadFolder < _uploadFoldervalues.Length)
            {
                parentFolder = _uploadFoldervalues[uploadFolder];
            }
            return parentFolder;
        }

        public class FileUploadResponse
        {
            public string FilePath { get; set; }
            public string FileSize { get; set; }
            public string OriginalFileName { get; set; }
        }
    }
}
