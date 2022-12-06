using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommadHandler : IRequestHandler<UploadProductImageCommadRequest, UploadProductImageCommadResponse>
    {
        readonly private IStorageService _storageService;
        readonly private IProductImageFilesWriteRepository _productImageFilesWriteRepository;
        readonly private IProductReadRepository _productReadRepository;

        public UploadProductImageCommadHandler(IStorageService storageService, IProductImageFilesWriteRepository productImageFilesWriteRepository, IProductReadRepository productReadRepository)
        {
            _storageService = storageService;
            _productImageFilesWriteRepository = productImageFilesWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<UploadProductImageCommadResponse> Handle(UploadProductImageCommadRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> results = await _storageService.UploadAsync("photo-images", request.Files);

            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);
            await _productImageFilesWriteRepository.AddRangeAsync(results.Select(d => new Domain.Entities.ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Domain.Entities.Product>() { product }

            }).ToList());

            await _productImageFilesWriteRepository.SaveAsync();
            return null;
        }
    }
}
