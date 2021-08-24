using AutoMapper;
using ResponseWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testVeljkara.ApplicationConstants;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.ServiceLayer.Services
{
    public class LinkService : ILinkService
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static readonly IDictionary<char, int> AlphabetIndex;
        private static readonly int Base = Alphabet.Length;
        public LinkService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseWrapper<SaveLinkDto>> Insert(SaveLinkDto saveLinkDto)
        {
            try
            {
                bool linkExists = _context.Links.Any(x => x.UserId == saveLinkDto.UserId && x.LongLink == saveLinkDto.LongLink);
                if (linkExists)
                {
                    return ResponseWrapper<SaveLinkDto>.Error(AppConstants.LinkAlreadyExists);
                }

                Uri uriResult;
                bool result = Uri.TryCreate(saveLinkDto.LongLink, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (!result)
                {
                    return ResponseWrapper<SaveLinkDto>.Error(AppConstants.LinkNotValid);
                }

                char[] charArray = saveLinkDto.LongLink.ToCharArray();
                Array.Reverse(charArray);
                saveLinkDto.ShortLink = new string(charArray);

                Link linkDB = _mapper.Map<Link>(saveLinkDto);
                linkDB.Id = Guid.NewGuid();
                _context.Links.Add(linkDB);
                await _context.SaveChangesAsync();
                saveLinkDto = _mapper.Map<SaveLinkDto>(linkDB);
                return ResponseWrapper<SaveLinkDto>.Success(saveLinkDto);
            }
            catch (Exception)
            {
                return ResponseWrapper<SaveLinkDto>.Error(AppConstants.ErrorSavingLink);
            }
        }

        public async Task<ResponseWrapper<SaveLinkDto>> Delete(SaveLinkDto saveLinkDto)
        {
            try
            {
                var linkDB = _context.Links.FirstOrDefault(x => x.Id == saveLinkDto.Id && x.UserId == saveLinkDto.UserId);
                if(linkDB != null)
                {
                    _context.Links.Remove(linkDB);
                    await _context.SaveChangesAsync();
                    return ResponseWrapper<SaveLinkDto>.Success(saveLinkDto);
                }
                else
                {
                    return ResponseWrapper<SaveLinkDto>.Error(AppConstants.LinkDoesNotExist);
                }
            }
            catch (Exception)
            {
                return ResponseWrapper<SaveLinkDto>.Error(AppConstants.ErrorRemovingLink);
            }
        }

        public async Task<ResponseWrapper<int>> CountUsers(SaveLinkDto saveLinkDto)
        {
            try
            {
                int numOfUsersWithThisLink = _context.Links.Where(x => x.LongLink == saveLinkDto.LongLink).Count();
                
                return ResponseWrapper<int>.Success(numOfUsersWithThisLink);
            }
            catch (Exception)
            {
                return ResponseWrapper<int>.Error(AppConstants.ErrorCountingUsers);
            }
        }

        public string RedirectFromShort(string shortUrl, Guid userId)
        {
            var link = _context.Links.FirstOrDefault(x => x.UserId == userId && x.ShortLink == shortUrl);
            if (link != null)
            {
                return link.LongLink;
            }
            else return string.Empty;
        }
        

        public string GenerateShortString(int seed)
        {
            if (seed < Base)
            {
                return Alphabet[0].ToString();
            }

            var str = new StringBuilder();
            var i = seed;

            while (i > 0)
            {
                str.Insert(0, Alphabet[i % Base]);
                i /= Base;
            }

            return str.ToString();
        }

        public int RestoreSeedFromString(string str)
        {
            return str.Aggregate(0, (current, c) => current * Base + AlphabetIndex[c]);
        }
    }
}
