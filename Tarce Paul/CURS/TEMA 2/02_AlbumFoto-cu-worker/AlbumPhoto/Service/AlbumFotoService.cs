﻿using AlbumPhoto.Models;
using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Auth;


namespace AlbumPhoto.Service
{
    public class AlbumFotoService 
    {
        private CloudStorageAccount _account;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _photoContainer;
        private CloudTableClient _tableClient;
        private CloudTable _filesTable;
        private CloudTable _commentsTable;
        private TableServiceContext _ctx;

        private BlobContainerPermissions contPermision;
        static private string sasToken;
        // static 
        public AlbumFotoService()
        {
            //_account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("PhotoStorage"));
            _account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            _blobClient = _account.CreateCloudBlobClient();
            _photoContainer = _blobClient.GetContainerReference("poze");
            if (_photoContainer.CreateIfNotExists())
            {
                _photoContainer.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }


            contPermision = new BlobContainerPermissions();
            contPermision.SharedAccessPolicies.Add(
                "policy", new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow.AddSeconds(-1),
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                    Permissions = SharedAccessBlobPermissions.Read
                });

            contPermision.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(contPermision);
            sasToken = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "policy");
            

            _tableClient = _account.CreateCloudTableClient();
            _filesTable = _tableClient.GetTableReference("files");
            _filesTable.CreateIfNotExists();
            _commentsTable = _tableClient.GetTableReference("comments");
            _commentsTable.CreateIfNotExists();
            _ctx = _tableClient.GetTableServiceContext();
        }

        public static string GetSasBlobUrl(string fileName)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient sasBlobClient = new CloudBlobClient(storageAccount.BlobEndpoint, new StorageCredentials(sasToken));
            CloudBlob blob = (CloudBlob)sasBlobClient.GetBlobReferenceFromServer(new Uri(fileName));
            return blob.Uri.AbsoluteUri + sasToken;
        }

        public Link LinkPoza(string poza)
        {
            Link link = new Link();
            var pictures = new List<Poza>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach(var file in query)
            {
                if (file.RowKey.Equals(poza))
                    link.LinkPoza = GetSasBlobUrl(file.Url);
                link.Poza = poza;
            }
            return link;
        }
        public List<Poza> GetPoze()
        {
            var poze = new List<Poza>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                poze.Add(new Poza()
                {
                    Description = file.RowKey,
                    ThumbnailUrl = file.ThumbnailUrl,
                    Url = file.Url
                });
            }

            return poze;
        }

        public void IncarcaPoza(string userName, string description, Stream continut)
        {
            var blob = _photoContainer.GetBlockBlobReference(description);
            blob.UploadFromStream(continut);

            _ctx.AddObject(_filesTable.Name, new FileEntity(userName, description)
            {
                PublishDate = DateTime.UtcNow,
                Size = continut.Length,
                Url = blob.Uri.ToString(),
            });

            _ctx.SaveChangesWithRetries();
        }


        public void AddComment(string userName, string comment, string file,Stream com)
        {

            var blob = _photoContainer.GetBlockBlobReference(userName);
            blob.UploadFromStream(com);

            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, file)
            {
                Text = comment,
                MadeBy = userName,
            });

            _ctx.SaveChangesWithRetries();
        }

        public List<Comentariu> AfisareComentariu(string picture)
        {
            var comentarii = new List<Comentariu>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach (var comment in query)
            {
                if (comment.Text != null)
                {
                    if (comment.RowKey.StartsWith(picture))
                        comentarii.Add(new Comentariu()
                        {
                            Autor = comment.MadeBy,
                            Text = comment.Text,
                        });
                }
                
            }
            return comentarii;
        }

    }
}