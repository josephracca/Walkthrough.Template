using Application.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Infrastructure
{
    public interface IStorageService
    {
        Task<Storage> UploadFile(string container, IFormFile formFile);
        Task<string> GetBlobSasToken(string container);
        Task<Stream> GetFileAsync(string ContainerName, string fileName);
    }
}
