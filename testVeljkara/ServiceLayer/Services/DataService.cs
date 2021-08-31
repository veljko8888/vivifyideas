using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.Helpers;
using testVeljkara.ServiceLayer.Interfaces;

namespace testVeljkara.ServiceLayer.Services
{
    public class DataService : IDataService
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        public DataService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Inserts base64 data into database
        /// </summary>
        /// <param name="dataBase64Dto">base64 data info details</param>
        /// <returns>data model object wrapped with result info wrapper object</returns>
        public async Task<Result<DataDto>> Insert(DataDto dataBase64Dto)
        {
            Result<DataDto> result = new Result<DataDto>();
            try
            {
                if (string.IsNullOrEmpty(dataBase64Dto.Data) || !CheckIfValidBase64String(dataBase64Dto.Data))
                {
                    return HelperManager<DataDto>.GenerateErrorServiceResult(
                        ResultStatus.Failure,
                        "Provided value is not valid Base64 string.",
                        result);
                }

                DataBase64 sameIDData = await _context.DataBase64s.FirstOrDefaultAsync(x => x.Id == dataBase64Dto.Id);
                if (sameIDData != null && sameIDData.Side == dataBase64Dto.Side)
                {
                    sameIDData.Data = dataBase64Dto.Data;
                    _context.DataBase64s.Update(sameIDData);
                    await _context.SaveChangesAsync();
                    dataBase64Dto = _mapper.Map<DataDto>(sameIDData);
                    return HelperManager<DataDto>.GenerateServiceSuccessResult(ResultStatus.Created, dataBase64Dto, result);
                }
                else
                {
                    DataBase64 dataDb = _mapper.Map<DataBase64>(dataBase64Dto);
                    _context.DataBase64s.Add(dataDb);
                    await _context.SaveChangesAsync();
                    dataBase64Dto = _mapper.Map<DataDto>(dataDb);
                    return HelperManager<DataDto>.GenerateServiceSuccessResult(ResultStatus.Created, dataBase64Dto, result);
                }
            }
            catch (Exception ex)
            {
                return HelperManager<DataDto>.GenerateErrorServiceResult(
                        ResultStatus.Failure,
                        ex.Message,
                        result);
            }
        }

        /// <summary>
        /// Checks the difference between two base64 data records
        /// </summary>
        /// <param name="id">id of the base64 records for which the 
        /// difference is requested</param>
        /// <returns>difference model object wrapped with result info wrapper object</returns>
        public async Task<Result<DiffDto>> CheckDiff(int id)
        {
            Result<DiffDto> result = new Result<DiffDto>();
            try
            {
                List<DataBase64> dataRecords = await _context.DataBase64s.Where(x => x.Id == id).ToListAsync();
                if(dataRecords == null || dataRecords.Count < 2)
                {
                    return HelperManager<DiffDto>.GenerateErrorServiceResult(
                        ResultStatus.NotFound,
                        "There are no 2 data records with the provided id.",
                        result);
                }

                DiffDto diff = new DiffDto();
                byte[] bytes1 = Convert.FromBase64String(dataRecords[0].Data);
                byte[] bytes2 = Convert.FromBase64String(dataRecords[1].Data);
                if (dataRecords[0].Data == dataRecords[1].Data)
                {
                    diff.DiffResultType = nameof(DiffResultType.Equals);
                }
                else if(bytes1.Length != bytes2.Length)
                {
                    diff.DiffResultType = nameof(DiffResultType.SizeDoNotMatch);
                }
                else
                {
                    var differences = new List<Difference>();
                    int i = 0;
                    while (i < bytes1.Length)
                    {
                        if (bytes1[i] != bytes2[i])
                        {
                            int indexDiff = i;
                            int length = 0;
                            while (i < bytes1.Length && bytes1[i] != bytes2[i])
                            {
                                i++;
                                length++;
                            }

                            differences.Add(new Difference(indexDiff, length));
                        }
                        else
                        {
                            i++;
                        }
                    }

                    diff.Differences = differences;
                    diff.DiffResultType = nameof(DiffResultType.ContentDoNotMatch);
                }

                result.ResultObject = diff;

                return result;
            }
            catch (Exception ex)
            {
                return HelperManager<DiffDto>.GenerateErrorServiceResult(
                        ResultStatus.Failure,
                        ex.Message,
                        result);
            }
        }

        private bool CheckIfValidBase64String(string base64string)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64string.Length]);
            return Convert.TryFromBase64String(base64string, buffer, out int bytesParsed);
        }
    }
}
