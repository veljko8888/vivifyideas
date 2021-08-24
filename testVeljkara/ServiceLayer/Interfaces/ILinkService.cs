using ResponseWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.Dtos;

namespace testVeljkara.ServiceLayer.Interfaces
{
    public interface ILinkService
    {
        Task<ResponseWrapper<SaveLinkDto>> Insert(SaveLinkDto saveLinkDto);
        Task<ResponseWrapper<SaveLinkDto>> Delete(SaveLinkDto saveLinkDto);
        Task<ResponseWrapper<int>> CountUsers(SaveLinkDto saveLinkDto);
        string RedirectFromShort(string shortUrl, Guid userId);
    }
}
