using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherDataCollector.StorageProvider
{
    public class GoogleDriveStorageProvider : IPermanentStorageProvider
    {
        private DriveService driveService;

        public GoogleDriveStorageProvider(String clientId, String clientSecret, String applicationName)
        {
            //First initialization on new client will open browser to allow service to use client
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                new[] { DriveService.Scope.Drive },
                "user",
                CancellationToken.None).Result;

            this.driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
        }

        public string Add(StorageProviderAddParams addParams)
        {
            var body = new File {Title = addParams.ServerFileName, MimeType = addParams.ContentType};

            var serverFolderId = FindFolderIdByName(addParams.ServerFolderName);
            if (String.IsNullOrEmpty(serverFolderId))
            {
                serverFolderId = CreateFolder(addParams.ServerFolderName);
            }

            body.Parents = new List<ParentReference>() { new ParentReference() { Id = serverFolderId } };

            byte[] byteArray = System.IO.File.ReadAllBytes(addParams.LocalFileName);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
            try
            {
                var request = this.driveService.Files.Insert(body, stream, addParams.ContentType);
                request.Upload();

                var file = request.ResponseBody;

                Console.WriteLine("Google Drive File {0} Created ", file.Title);

                return file.WebContentLink;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }

        private string FindFolderIdByName(string folderName)
        {
            var listRequest = this.driveService.Files.List();
            var queryBuilder = new StringBuilder();

            queryBuilder.Append("title = '");
            queryBuilder.Append(folderName);
            queryBuilder.Append("' ");
            queryBuilder.Append("and trashed = false");

            listRequest.Q = queryBuilder.ToString();
            var results = listRequest.Execute().Items;

            if (!results.Any())
            {
                return null;
            }

            return results[0].Id;
        }

        private string CreateFolder(string folderName)
        {
            var folderPermissions = new Permission
            {
                Type = "anyone",
                Role = "reader",
                Value = "",
                WithLink = true
            };

            var folder = new File {Title = folderName, MimeType = "application/vnd.google-apps.folder"};

            var insertRequest = this.driveService.Files.Insert(folder);
            folder = insertRequest.Execute();

            this.driveService.Permissions.Insert(folderPermissions, folder.Id).Execute();

            Console.WriteLine("Google Drive Folder {0} Created",folderName);

            return folder.Id;
        }
    }
}
