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

        public List<Comentarii> GetComments(string numePoza)
        {
            var comments = new List<Comentarii>();
            var query = (from comment in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
						 select comment).AsTableServiceQuery<CommentEntity>(_ctx);


            foreach (var comment in query)
            {

                if (comment.Text != null)
                {
                    string[] sir = comment.Text.Split(new string[] { "+" }, StringSplitOptions.None);

                    if (sir[0] != null && sir.Length > 1)
                    {
                        string comentariu = sir[0];
                        string poza = sir[1];

                        if (poza.Equals(numePoza))
                        {
                            comments.Add(new Comentarii()
                            {
                                Text = comentariu,
                                MadeBy = comment.MadeBy

                            });
                        }
                    }
                }
            }
            return comments;
        }
        public void AdaugaComentariu(string userName, string comentariu)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, comentariu)
            {
                MadeBy = userName,
                Text = comentariu
            });

            _ctx.SaveChangesWithRetries();
        }

        public string GetBlobSasUri(string photoName)
        {
            string sasBlobToken;
            var blob = _photoContainer.GetBlockBlobReference(photoName);

            SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write
            };

            // Generate the shared access signature on the blob, setting the constraints directly on the signature.
            sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);
            return sasBlobToken;
            
        }
        

	}
}