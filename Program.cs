using System;
using System.IO;
using Ionic.Zip;

namespace DotNetZip
{
    class MainClass
    {
        public static void Main(string[] args)
		{
            String imageZipPassword = "12345";
			String dataDirectory = "/Users/tmt/Projects/sample/DotNetZip/data/";

            String[] imageFileNames = 
            {
                "image1.JPG"
                , "image2.JPG"
            };

            String[] imagePaths = 
            {
                dataDirectory + imageFileNames[0]
                , dataDirectory + imageFileNames[1]
            };

			String[] imageZipPaths =
			{
				dataDirectory + imageFileNames[0] + ".zip"
				, dataDirectory + imageFileNames[1] + ".zip"
			};

			Console.WriteLine("圧縮 START!");
            for (int i = 0; i < imagePaths.Length; i++) 
            {
                String imagePath = imagePaths[i];
                using (var inputStream = new FileStream(imagePath, FileMode.Open))
				{
					using (ZipFile zip = new ZipFile())
					{
                        zip.Password = imageZipPassword;
						zip.UpdateEntry(imageFileNames[i], inputStream);
                        zip.Save(imageZipPaths[i]);
					}
                }
			}
			Console.WriteLine("圧縮 END!");

			Console.WriteLine("解凍圧縮 START!");

            try
            {
				using (ZipOutputStream output = new ZipOutputStream(dataDirectory + "ftp.zip"))
				{
                    output.Password = "00000";

					for (int i = 0; i < imageZipPaths.Length; i++)
					{
						using (ZipFile zip = ZipFile.Read(new FileStream(imageZipPaths[i], FileMode.Open)))
						{
							zip.Password = imageZipPassword;
							foreach (ZipEntry inputEntry in zip)
							{
								ZipEntry outputEntry = output.PutNextEntry(inputEntry.FileName);
								inputEntry.Extract(output);
							}
						}
					}
				}

                // 画像ZIPファイル削除
				for (int i = 0; i < imageZipPaths.Length; i++)
				{
					File.Delete(imageZipPaths[i]);
                }
            }
            catch (BadPasswordException)
            {
                File.Delete(dataDirectory + "ftp.zip");

                Console.WriteLine("解凍圧縮 ERROR (BadPassword)!");
                return;
            }

			Console.WriteLine("解凍圧縮 END!");
        }
    }
}
