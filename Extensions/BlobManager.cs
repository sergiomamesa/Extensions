using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;

namespace Extensions
{
    public class BlobManager
    {
        private readonly CloudBlobContainer _blobContainer;

        /// <summary>
        /// Crea una instancia de un cloud blob client para un container especifico
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="container"></param>
        public BlobManager(ConnectionStringSettings connectionString, string container)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            _blobContainer = blobClient.GetContainerReference(container);
        }

        public BlobManager(string connectionString, string container)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            _blobContainer = blobClient.GetContainerReference(container);
        }

        /// <summary>
        /// Devuelve todos los archivos contenidos en un directorio de manera recursiva o no
        /// </summary>
        /// <param name="blobDirectory"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public static IEnumerable<CloudBlockBlob> GetFilesFrom(CloudBlobDirectory blobDirectory, bool isRecursive)
        {
            foreach (var blob in blobDirectory.ListBlobs().OfType<CloudBlockBlob>())
            {
                yield return blob;
            }

            if (isRecursive == false)
                yield break;

            foreach (var blob in blobDirectory.ListBlobs().OfType<CloudBlobDirectory>())
            {
                foreach (var innerBlob in GetFilesFrom(blob, true))
                {
                    yield return innerBlob;
                }
            }

        }

        /// <summary>
        /// Devuelve todos los archivos contenidos en un listado directorio de manera recursiva o no
        /// Se recomienda no utilizar para largas enumeraciones
        /// </summary>
        /// <param name="listBlobDirectory"></param>
        /// <param name="isRecursive"></param>
        /// <returns></returns>
        public static IEnumerable<CloudBlockBlob> GetFilesFrom(IEnumerable<CloudBlobDirectory> listBlobDirectory, bool isRecursive)
        {
            return listBlobDirectory.SelectMany(blobDirectory => GetFilesFrom(blobDirectory, isRecursive));
        }

        /// <summary>
        /// Devuelve todas las carpetas contenidas en la raiz del container
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CloudBlobDirectory> GetContainerFolders()
        {
            return _blobContainer.ListBlobs().OfType<CloudBlobDirectory>();
        }

        /// <summary>
        /// Devuelve un CloudBlockBlob a partir de una uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public CloudBlockBlob GetBlobReference(string uri)
        {
            return _blobContainer.GetBlockBlobReference(uri);
        }
    }
}
