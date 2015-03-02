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
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
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
            File body = new File();
            body.Title = addParams.ServerFileName;
            body.MimeType = addParams.ContentType;


            var serverFolderId = FindFolderIdByName(addParams.ServerFolderName);
            if (String.IsNullOrEmpty(serverFolderId))
            {
                serverFolderId = createFolder(addParams.ServerFolderName);
            }

            body.Parents = new List<ParentReference>() { new ParentReference() { Id = serverFolderId } };

            byte[] byteArray = System.IO.File.ReadAllBytes(addParams.LocalFileName);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
            try
            {
                FilesResource.InsertMediaUpload request = this.driveService.Files.Insert(body, stream, addParams.ContentType);
                request.Upload();

                File file = request.ResponseBody;

                Console.WriteLine("New File Created: {0} with ID {1} ", file.Title, file.Id);

                return file.WebContentLink;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }

        public string Get(string serverFolderName, string ServerFileName)
        {
            throw new NotImplementedException();
        }

        private string FindFolderIdByName(string folderName)
        {
            IList<File> results = new List<File>();
            FilesResource.ListRequest listRequest = this.driveService.Files.List();
            StringBuilder queryBuilder = new StringBuilder();

            queryBuilder.Append("title = '");
            queryBuilder.Append(folderName);
            queryBuilder.Append("' ");
            queryBuilder.Append("and trashed = false");

            listRequest.Q = queryBuilder.ToString();
            results = listRequest.Execute().Items;

            if (results.Count() == 0)
            {
                return null;
            }
            else
            {
                return results[0].Id;
            }
        }

        private string createFolder(string folderName)
        {
            Permission folderPermissions = new Permission();
            folderPermissions.Type = "anyone";
            folderPermissions.Role = "reader";
            folderPermissions.Value = "";
            folderPermissions.WithLink = true;

            File folder = new File();
            folder.Title = folderName;
            folder.MimeType = "application/vnd.google-apps.folder";
            
            FilesResource.InsertRequest insertRequest = this.driveService.Files.Insert(folder);
            folder = insertRequest.Execute();

            this.driveService.Permissions.Insert(folderPermissions, folder.Id).Execute();

            Console.WriteLine("New Folder Created: {0} with id {1}", folderName, folder.Id);
            return folder.Id;
        }
    }
}
