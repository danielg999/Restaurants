using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;

namespace Restaurants.Infrastructure.Storage;

internal class BlobStorageService(IOptions<BlobStorageSettings> blobSorageSettingsOptions) : IBlobStorageService
{
    private readonly BlobStorageSettings _blobStorageSettings = blobSorageSettingsOptions.Value;
    public async Task<string> UploadToBlobAsync(Stream data, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(data);

        var blobUrl = blobClient.Uri.ToString();
        return blobUrl;
    }

    public string GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null) return null;
        var sasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = _blobStorageSettings.LogosContainerName,
            Resource = "b", // "b" is sharing blob, "c" is sharing container
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
            BlobName = GetBlobnameFromUrl(blobUrl)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);

        var sasToken = sasBuilder
            .ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, 
                _blobStorageSettings.AccountKey))
            .ToString();

        return $"{blobUrl}?{sasToken}";
        // blob: https://sarestaurantsdgdev.blob.core.windows.net/logos/Amazon_Web_Services_Logo.svg.png
        // sas: sp=r&st=2025-04-19T07:10:44Z&se=2025-04-19T15:10:44Z&spr=https&sv=2024-11-04&sr=b&sig=ckCxJAxW5L5xUfebaM21XkdadCy9SbRKfsxOx17R0jU%3D
    }

    private string GetBlobnameFromUrl(string blobUrl)
    {
        var uri = new Uri(blobUrl);
        return uri.Segments.Last();
    }
}
