using AutoMapper;
using NUnit.Framework;
using System.Threading.Tasks;
using testVeljkara.AutoMapper;
using testVeljkara.DbLayer;
using testVeljkara.Dtos;
using testVeljkara.Helpers;
using testVeljkara.ServiceLayer.Interfaces;
using testVeljkara.ServiceLayer.Services;

namespace Tests
{
    public class DataServiceTests
    {
        private IDataService _dataService;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            ApplicationDbContext dbContext = new DbContextCreator().GetDatabaseContext();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperDefinition());
            });

            _mapper = mappingConfig.CreateMapper();

            _dataService = new DataService(dbContext, _mapper);
        }

        [Test]
        public async Task EmptyStringForBase64Data()
        {
            var data = new DataDto();
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(ResultStatus.Failure, result.Status);
        }

        [Test]
        public async Task InvalidBase64Data()
        {
            var data = new DataDto();
            data.Data = "aaa";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(ResultStatus.Failure, result.Status);
        }

        [Test]
        public async Task NullBase64Data()
        {
            var data = new DataDto();
            data.Data = null;
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(ResultStatus.Failure, result.Status);
        }

        [Test]
        public async Task SuccessInsertData()
        {
            var data = new DataDto();
            data.Data = "AAAAAA==";
            data.Id = 1;
            data.Side = "left";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
        }

        [Test]
        public async Task SuccessUpdateData()
        {
            var data = new DataDto();
            data.Data = "AQABAQ==";
            data.Id = 1;
            data.Side = "left";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
        }

        [Test]
        public async Task DiffWhenNoRecord()
        {
            Result<DiffDto> result = await _dataService.CheckDiff(1);
            Assert.AreEqual(ResultStatus.NotFound, result.Status);
        }

        [Test]
        public async Task DiffWhenOneRecord()
        {
            var data = new DataDto();
            data.Data = "AQABAQ==";
            data.Id = 1;
            data.Side = "left";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            Result<DiffDto> diffResult = await _dataService.CheckDiff(1);
            Assert.AreEqual(ResultStatus.NotFound, diffResult.Status);
        }

        [Test]
        public async Task DiffWhenTwoValidRecordsDifferentSize()
        {
            var data = new DataDto();
            data.Data = "cXdlcXdlcXdlcXdl";
            data.Id = 1;
            data.Side = "left";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            data.Data = "AAAAAA==";
            data.Id = 1;
            data.Side = "right";
            result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            Result<DiffDto> diffResult = await _dataService.CheckDiff(1);
            Assert.AreEqual(ResultStatus.Success, diffResult.Status);
            Assert.AreEqual(nameof(DiffResultType.SizeDoNotMatch), diffResult.ResultObject.DiffResultType);
        }

        public async Task DiffWhenTwoValidRecordsAreEqual()
        {
            var data = new DataDto();
            data.Data = "AQABAQ==";
            data.Id = 1;
            data.Side = "left";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            data.Data = "AAAAAA==";
            data.Id = 1;
            data.Side = "right";
            result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            Result<DiffDto> diffResult = await _dataService.CheckDiff(1);
            Assert.AreEqual(ResultStatus.Success, diffResult.Status);
            Assert.AreEqual(nameof(DiffResultType.ContentDoNotMatch), diffResult.ResultObject.DiffResultType);
            Assert.AreEqual(2, diffResult.ResultObject.Differences.Count);
            Assert.AreEqual(0, diffResult.ResultObject.Differences[0].Offset);
            Assert.AreEqual(1, diffResult.ResultObject.Differences[0].Length);
            Assert.AreEqual(2, diffResult.ResultObject.Differences[1].Offset);
            Assert.AreEqual(2, diffResult.ResultObject.Differences[1].Length);
        }

        public async Task DiffWhenTwoValidRecordsWithDifferences()
        {
            var data = new DataDto();
            data.Data = "AAAAAA==";
            data.Id = 1;
            data.Side = "left";
            Result<DataDto> result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            data.Data = "AAAAAA==";
            data.Id = 1;
            data.Side = "right";
            result = await _dataService.Insert(data);
            Assert.AreEqual(data.Data, result.ResultObject.Data);
            Assert.AreEqual(data.Id, result.ResultObject.Id);
            Assert.AreEqual(data.Side, result.ResultObject.Side);
            Assert.AreEqual(ResultStatus.Created, result.Status);
            Result<DiffDto> diffResult = await _dataService.CheckDiff(1);
            Assert.AreEqual(ResultStatus.Success, diffResult.Status);
            Assert.AreEqual(nameof(DiffResultType.Equals), diffResult.ResultObject.DiffResultType);
        }
    }
}