﻿using AlbumPhoto.Models;
using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
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
        // this is for getting link
        private BlobContainerPermissions _blobContainterPermissions;
        public static string SAS = string.Empty;

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

			_tableClient = _account.CreateCloudTableClient();
			_filesTable = _tableClient.GetTableReference("files");
			_filesTable.CreateIfNotExists();
			_commentsTable = _tableClient.GetTableReference("comments");
			_commentsTable.CreateIfNotExists();
            _ctx = _tableClient.GetTableServiceContext();

            // this is for link with 2h access
            _blobContainterPermissions = new BlobContainerPermissions();
            _blobContainterPermissions.SharedAccessPolicies.Add("twohourspolicy", new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                Permissions = SharedAccessBlobPermissions.Read
            });
            _blobContainterPermissions.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(_blobContainterPermissions);
            SAS = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "twohourspolicy");

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

        public void AddComment(string userName, string comment, string fileName)
        {
            string description = fileName;
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, description)
            {
                Comment = fileName+comment,
                User = userName,
                Picture = fileName
            });

            _ctx.SaveChangesWithRetries();
        }

        public static string GetSasBlobUrl(string fileName)
        {
            var uri = new Uri(fileName);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient sasBlobClient = new CloudBlobClient(storageAccount.BlobEndpoint, new StorageCredentials(SAS));

            CloudBlob blob = (CloudBlob)sasBlobClient.GetBlobReferenceFromServer(uri);
            return blob.Uri.AbsoluteUri + SAS;
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
                    Url = file.Url,
                    PictureName = file.RowKey
                });
            }
            return poze;
        }
        
        public string GetLink(string pictureName)
        {
            try
            {
                var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                             where file.RowKey == pictureName
                             select file).AsTableServiceQuery<FileEntity>(_ctx);

                foreach (var item in query)
                {
                    return GetSasBlobUrl(item.Url);
                }
            }
            catch { }
            return "Error";
        }

        public List<Comments> GetComments(string pictureName)
        {
            var commentList = new List<Comments>();
            if (!string.IsNullOrEmpty(pictureName))
            {
                var request = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)select file).AsTableServiceQuery<CommentEntity>(_ctx);

                foreach (var comment in request)
                {
                    if (!string.IsNullOrEmpty(comment.Comment))
                        if (comment.Comment.StartsWith(pictureName))
                            commentList.Add(new Comments()
                            {
                                User = comment.User,
                                Comment = comment.Comment.Substring(pictureName.Length),
                                Picture =  comment.Picture
                            });
                }
            }
            return commentList;
        }
        




    }
}