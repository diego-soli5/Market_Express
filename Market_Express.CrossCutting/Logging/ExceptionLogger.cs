using Market_Express.CrossCutting.Utility;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Market_Express.CrossCutting.Logging
{
    public static class ExceptionLogger
    {
        public static void LogException(Exception exception)
        {
            try
            {
                string sEnvPath = Directory.GetCurrentDirectory();
                string sErrLogDirectoryPath = @$"{sEnvPath}\\log";
                string sErrLogFilePath = @$"{sErrLogDirectoryPath}\\errLog.txt";

                StringBuilder sbText = new($"\n\n----------------------------------------------{DateTimeUtility.NowCostaRica}----------------------------------------------\n");
                sbText.Append($"Message: {exception?.Message}\n");
                sbText.Append("*\n");
                sbText.Append($"StackTrace: {exception?.StackTrace}\n");
                sbText.Append("*\n");
                sbText.Append($"InnerExeption Message: {exception?.InnerException?.Message}\n");
                sbText.Append("*\n");
                sbText.Append($"InnerExeption StackTrace: {exception?.InnerException?.StackTrace}\n");

                if (!Directory.Exists(sErrLogDirectoryPath))
                    Directory.CreateDirectory(sErrLogDirectoryPath);

                using (StreamWriter sw = File.AppendText(sErrLogFilePath))
                {
                    sw.WriteLine(sbText);
                }

                Debug.Fail(exception.Message, sbText.ToString());
            }
            catch (Exception ex)
            {
            }
        }
    }
}
