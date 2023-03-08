using Delta.Api.IDal;
using Delta.Api.Wrapper;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Delta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly IMdmInfoDal _mdmInfoDal;
        public readonly IPropertyInfoDal _propertyInfoDal;
        public readonly IClientInfoDal _clientInfoDal;
        public readonly IRelationInfoDal _relationInfoDal;
        private readonly IConfiguration _configuration;
        public TestController(
             IMdmInfoDal mdmInfoDal,
             IPropertyInfoDal propertyInfoDal,
             IClientInfoDal clientInfoDal,
             IRelationInfoDal relationInfoDal,
             IConfiguration configuration
            )
        {
            _mdmInfoDal = mdmInfoDal;
            _propertyInfoDal = propertyInfoDal;
            _clientInfoDal = clientInfoDal;
            _relationInfoDal = relationInfoDal;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllMdm")]
        public async Task<IActionResult> GetAllMdm()
        {
            var result = await _mdmInfoDal.GetMdmInfoDocAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetMdm")]
        public async Task<IActionResult> GetMdm()
        {
            var result = await _mdmInfoDal.GetByKeyAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetProp")]
        public async Task<IActionResult> GetProp()
        {
            var result = await _propertyInfoDal.GetByListAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetAllClient")]
        public async Task<IActionResult> GetAllClient()
        {
            var result = await _clientInfoDal.GetAllAsync("clientInfo");
            return Ok();
        }
        [HttpDelete]
        [Route("DeleteClient")]
        public async Task<IActionResult> DeleteClient(string clientKey)
        {
            var result = await _clientInfoDal.DeleteAsync("clientInfo", clientKey);
            return Ok();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string relationshipCollection, string collectionName, string columnName, List<string> keysToDelete)
        {
            if (keysToDelete == null)
            {
                return BadRequest("Keys not provided");
            }
            if (keysToDelete.Count == 0)
            {
                return BadRequest("Keys not provided");
            }
            if (collectionName == string.Empty)
            {
                return BadRequest("Collection Name not provided");
            }
            int batchSize = _configuration.GetValue<int>("Batch:Size");
            int keysLength = keysToDelete.Count;
            List<List<string>> deleteBatches = new List<List<string>>();
            List<string> failedList = new List<string>();
            List<string> deleteBatch = new List<string>();
            var relations = await _relationInfoDal.GetRelationsAsync(collectionName, relationshipCollection);

            for (int i = 0; i < keysLength; i++)
            {
                //check if relational data exists
                bool relationDataExists = false;
                foreach (var relation in relations)
                {
                    var data = await _relationInfoDal.GetFirstAsync(relation.destinationTable, relation.destinationId, keysToDelete[i]);
                    if (data != null && data._id != string.Empty) { relationDataExists = true; break; }
                }
                if (relationDataExists)
                    failedList.Add(keysToDelete[i]);//if exists in relational data then add to failed list
                else
                {
                    //get the data for getting key
                    var data = await _relationInfoDal.GetFirstAsync(collectionName, columnName, keysToDelete[i]);
                    if (data != null && data._id != string.Empty)
                        deleteBatch.Add(data._id);//else add to deleteBatch list
                    else
                        failedList.Add(keysToDelete[i]);//data not found in database
                }

                // if deleteBatch.Length ==batchSize or i==keysLength-1
                // then add deleteBatch to deleteBatches and create new instance of deleteBatch
                if (deleteBatch.Count == batchSize || i == (keysLength - 1))
                {
                    if (deleteBatch.Count > 0)
                        deleteBatches.Add(deleteBatch);
                    deleteBatch = new List<string>();
                }
            }
            foreach (var batchKeysToDelete in deleteBatches)
            {
                var result = await _relationInfoDal.DeleteMatchedAsync(collectionName, batchKeysToDelete);
                if (!result)
                    failedList.AddRange(batchKeysToDelete);
            }
            string message = failedList.Count > 0 ? "Success with failed data" : "Success";
            ApiResponse response = new ApiResponse(failedList, 200, message, keysLength);
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteByExcelNew")]
        public async Task<IActionResult> DeleteByExcelNew(IFormFile file, string collectionName, string relationshipCollection)
        {
            if (file == null)
            {
                return BadRequest("File not found");
            }
            if (file.Length == 0)
            {
                return BadRequest("File not found");
            }
            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest("File format not supported");
            }
            if (!file.FileName.EndsWith(".xlsx"))
            {
                return BadRequest("File not supported");
            }
            List<string> keysToDelete = new List<string>();
            //string collectionName;
            string collectionname;
            string columnName;
            keysToDelete = ReadExcel(file, out collectionname, out columnName);

            int batchSize = _configuration.GetValue<int>("Batch:Size");
            int keysLength = keysToDelete.Count;
            List<List<string>> deleteBatches = new List<List<string>>();
            List<string> failedList = new List<string>();
            List<string> deleteBatch = new List<string>();
            var relations = await _relationInfoDal.GetRelationsAsync(collectionName, relationshipCollection);

            for (int i = 0; i < keysLength; i++)
            {
                //check if relational data exists
                bool relationDataExists = false;
                foreach (var relation in relations)
                {
                    var data = await _relationInfoDal.GetFirstAsync(relation.destinationTable, relation.destinationId, keysToDelete[i]);
                    if (data != null && data._id != string.Empty) { relationDataExists = true; break; }
                }
                if (relationDataExists)
                    failedList.Add(keysToDelete[i]);//if exists in relational data then add to failed list
                else
                {
                    //get the data for getting key
                    var data = await _relationInfoDal.GetFirstAsync(collectionName, columnName, keysToDelete[i]);
                    if (data != null && data._id != string.Empty)
                        deleteBatch.Add(keysToDelete[i]);//else add to deleteBatch list
                    else
                        failedList.Add(keysToDelete[i]);//data not found in database
                }

                // if deleteBatch.Length ==batchSize or i==keysLength-1
                // then add deleteBatch to deleteBatches and create new instance of deleteBatch
                if (deleteBatch.Count == batchSize || i == (keysLength - 1))
                {
                    if (deleteBatch.Count > 0)
                        deleteBatches.Add(deleteBatch);
                    deleteBatch = new List<string>();
                }
            }
            foreach (var batchKeysToDelete in deleteBatches)
            {
                var result = await _relationInfoDal.DeleteByQueryAsync(collectionName, columnName, batchKeysToDelete);
                if (!result)
                    failedList.AddRange(batchKeysToDelete);
            }


            string message = failedList.Count > 0 ? "Success with failed data" : "Success";
            ApiResponse response = new ApiResponse(failedList, 200, message, keysLength);
            return Ok(response);
        }

        [HttpDelete]
        [Route("DeleteByExcel")]
        public async Task<IActionResult> DeleteByExcel(IFormFile file, string relationshipCollection)
        {
            if (file == null)
            {
                return BadRequest("File not found");
            }
            if (file.Length == 0)
            {
                return BadRequest("File not found");
            }
            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest("File format not supported");
            }
            if (!file.FileName.EndsWith(".xlsx"))
            {
                return BadRequest("File not supported");
            }
            List<string> keysToDelete = new List<string>();
            string collectionName;
            string columnName;
            keysToDelete = ReadExcel(file, out collectionName, out columnName);

            int batchSize = 2;
            int keysLength = keysToDelete.Count;
            List<List<string>> deleteBatches = new List<List<string>>();
            List<string> failedList = new List<string>();
            List<string> deleteBatch = new List<string>();
            var relations = await _relationInfoDal.GetRelationsAsync(collectionName, relationshipCollection);

            for (int i = 0; i < keysLength; i++)
            {
                //check if relational data exists
                bool relationDataExists = false;
                foreach (var relation in relations)
                {
                    var data = await _relationInfoDal.GetFirstAsync(relation.destinationTable, relation.destinationId, keysToDelete[i]);
                    if (data != null && data._id != string.Empty) { relationDataExists = true; break; }
                }
                if (relationDataExists)
                    failedList.Add(keysToDelete[i]);//if exists in relational data then add to failed list
                else
                {
                    //get the data for getting key
                    var data = await _relationInfoDal.GetFirstAsync(collectionName, columnName, keysToDelete[i]);
                    if (data != null && data._id != string.Empty)
                        deleteBatch.Add(data._id);//else add to deleteBatch list
                    else
                        failedList.Add(keysToDelete[i]);//data not found in database
                }

                // if deleteBatch.Length ==batchSize or i==keysLength-1
                // then add deleteBatch to deleteBatches and create new instance of deleteBatch
                if (deleteBatch.Count == batchSize || i == (keysLength - 1))
                {
                    if (deleteBatch.Count > 0)
                        deleteBatches.Add(deleteBatch);
                    deleteBatch = new List<string>();
                }
            }
            foreach (var batchKeysToDelete in deleteBatches)
            {
                var result = await _relationInfoDal.DeleteMatchedAsync(collectionName, batchKeysToDelete);
                if (!result)
                    failedList.AddRange(batchKeysToDelete);
            }


            string message = failedList.Count > 0 ? "Success with failed data" : "Success";
            ApiResponse response = new ApiResponse(failedList, 200, message, keysLength);
            return Ok(response);
        }

        private List<string> ReadExcel(IFormFile file, out string collectionName, out string columnName)
        {
            List<string> data = new List<string>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream =new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dt = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    }).Tables[0];//https://github.com/ExcelDataReader/ExcelDataReader
                    collectionName = dt.TableName;
                    columnName = dt.Columns[0].ColumnName;
                    foreach (DataRow row in dt.Rows)
                    {
                        data.Add(row[columnName].ToString());
                    }
                }
            }

            return data;
        }

        

    }
}
