using ExcelDataReader;
using MFS.DistributionService.Models;
using MFS.DistributionService.Service;
using MFS.ReportingService.Models;
using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Service;
using Newtonsoft.Json;
using OneMFS.ReportingApiServer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OneMFS.ReportingApiServer.Controllers
{
    public class ExcelUploadController : ApiController
    {
        private readonly ITblDisburseTmpService _TblDisburseTmpService;
        private readonly IDistributorService _distributorService;
        private readonly IAgentService _agentService;
        private readonly IAuditTrailService _auditTrailService;

        //public ExcelUploadController(ITblDisburseTmpService objTblDisburseTmpService, IDistributorService objDstributorService) : base()
        public ExcelUploadController(ITblDisburseTmpService objTblDisburseTmpService,
            IDistributorService objDstributorService, IAgentService objAgentService, IAuditTrailService objAuditTrailService) : base()
        {
            this._TblDisburseTmpService = objTblDisburseTmpService;
            this._distributorService = objDstributorService;
            this._agentService = objAgentService;
            _auditTrailService = objAuditTrailService;
        }
        //[Route("UploadExcel")]

        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/ExcelUpload/UploadExcel")]
        public string UploadExcel(int organizationId, string batchno, string makerId, double amount)
        {
            string message = "";
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile file = httpRequest.Files[0];
                Stream stream = file.InputStream;

                IExcelDataReader reader = null;

                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    message = "This file format is not supported";
                }

                DataSet excelRecords = reader.AsDataSet();
                reader.Close();

                var finalRecords = excelRecords.Tables[0];

                bool isUploaded = false;
                int lastRow = finalRecords.Rows.Count - 1;
                double disburseTotal = Convert.ToDouble(finalRecords.Rows[lastRow][2]);
                if (disburseTotal > 0)
                {
                    if (disburseTotal > amount)
                    {
                        isUploaded = false;
                        message = "Disbursed total amount is greater than company total.";
                    }
                    else
                    {

                        for (int i = 1; i < finalRecords.Rows.Count - 1; i++)
                        {
                            TblDisburseTmp objTblDisburseTmp = new TblDisburseTmp();
                            objTblDisburseTmp.AcNo = finalRecords.Rows[i][1].ToString();
                            objTblDisburseTmp.Amount = Convert.ToDouble(finalRecords.Rows[i][2]);
                            objTblDisburseTmp.MakerId = makerId.ToString();
                            objTblDisburseTmp.Batchno = batchno.ToString();
                            objTblDisburseTmp.Sl = Convert.ToInt16(finalRecords.Rows[i][0]);
                            objTblDisburseTmp.OrganizationId = organizationId;

                            _TblDisburseTmpService.Add(objTblDisburseTmp);
                        }
                        isUploaded = true;
                        if (isUploaded)
                        {
                            message = "Excel file has been successfully uploaded";
                            //Insert into audit trial audit and detail
                            CompanyDisbursementUpload objCompanyDisbursementUpload = new CompanyDisbursementUpload();
                            objCompanyDisbursementUpload.CompanyId = organizationId;
                            objCompanyDisbursementUpload.BatchNo = batchno;
                            objCompanyDisbursementUpload.MakerId = makerId;
                            _auditTrailService.InsertModelToAuditTrail(objCompanyDisbursementUpload, objCompanyDisbursementUpload.MakerId, 10, 3, "Disbursement Process", objCompanyDisbursementUpload.CompanyId.ToString(), "Uploaded Successfully!");
                        }
                        else
                        {
                            message = "Excel file uploaded has fiald";
                        }
                    }
                }
                else
                {
                    message = "Disbursed total amount must be greater than 0.";
                }
               



            }

            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return message;
        }

        [HttpPost]
        [AcceptVerbs("GET", "POST")]
        [Route("api/ExcelUpload/BulkUploadExcel")]
        public string BulkUploadExcel(string bulkUploadType, string distributorAC, string distributorCode, string distributorName)
        {
            string message = "";
            string duplicateMsg = "";
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile file = httpRequest.Files[0];
                Stream stream = file.InputStream;

                IExcelDataReader reader = null;

                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    message = "This file format is not supported";
                }

                DataSet excelRecords = reader.AsDataSet();
                reader.Close();

                var finalRecords = excelRecords.Tables[0];


                string agentCode = null;
                if (bulkUploadType == "Agent")
                {
                    string firstClusterCode = _agentService.GetClusterCodeByTerritoryCode(distributorCode.Substring(0, 6));
                    agentCode = Convert.ToString(_agentService.GenerateAgentCodeAsString(firstClusterCode));
                }


                for (int i = 1; i < finalRecords.Rows.Count; i++)
                {
                    if (_distributorService.IsExistsByMpohne(finalRecords.Rows[i][0].ToString()) == false)
                    {
                        Reginfo objReginfo = new Reginfo();
                        bool isDuplicateFound = false;
                        objReginfo.Mphone = finalRecords.Rows[i][0].ToString();
                        objReginfo.BankAcNo = finalRecords.Rows[i][1].ToString();
                        if (bulkUploadType == "Distributor")
                        {
                            objReginfo.CatId = "D";
                        }
                        else if (bulkUploadType == "Agent")
                        {
                            objReginfo.CatId = "A";
                            objReginfo.DistCode = agentCode;
                        }
                        else
                        {
                            objReginfo.CatId = "C";
                        }

                        if (_distributorService.IsExistsByCatidPhotoId(objReginfo.CatId, finalRecords.Rows[i][13].ToString()) == true)
                        {
                            if (!string.IsNullOrEmpty(duplicateMsg))
                            {
                                duplicateMsg = duplicateMsg + " , Photo duplicate for " + finalRecords.Rows[i][0].ToString();
                            }
                            else
                            {
                                duplicateMsg = "Photo duplicate for " + finalRecords.Rows[i][0].ToString();
                            }

                            isDuplicateFound = true;
                        }


                        if (isDuplicateFound == false)
                        {
                            objReginfo.Pmphone = finalRecords.Rows[i][3].ToString();
                            objReginfo.BranchCode = finalRecords.Rows[i][4].ToString();
                            objReginfo.DateOfBirth = Convert.ToDateTime(finalRecords.Rows[i][5]);
                            objReginfo.Name = finalRecords.Rows[i][6].ToString();
                            objReginfo.FatherName = finalRecords.Rows[i][7].ToString();
                            objReginfo.MotherName = finalRecords.Rows[i][8].ToString();
                            objReginfo.SpouseName = finalRecords.Rows[i][9].ToString();
                            objReginfo.Gender = finalRecords.Rows[i][10].ToString();
                            objReginfo.PhotoIdTypeCode = Convert.ToInt32(finalRecords.Rows[i][12]);
                            objReginfo.PhotoId = finalRecords.Rows[i][13].ToString();
                            objReginfo.TinNo = finalRecords.Rows[i][14].ToString();
                            objReginfo.Religion = finalRecords.Rows[i][15].ToString();
                            objReginfo.Occupation = finalRecords.Rows[i][16].ToString();
                            objReginfo.OffMailAddr = finalRecords.Rows[i][17].ToString();
                            objReginfo.LocationCode = finalRecords.Rows[i][18].ToString();

                            //objReginfo.DistCode = agentCode;
                            objReginfo.PreAddr = finalRecords.Rows[i][22].ToString();
                            objReginfo.PerAddr = finalRecords.Rows[i][23].ToString();

                            _distributorService.Add(objReginfo);
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(duplicateMsg))
                        {
                            duplicateMsg = duplicateMsg + " , " + finalRecords.Rows[i][0].ToString();
                        }
                        else
                        {
                            duplicateMsg = finalRecords.Rows[i][0].ToString();
                        }

                    }

                }
                //isUploaded = true;
                if (!string.IsNullOrEmpty(duplicateMsg))
                {
                    //message = "Excel file has been successfully uploaded";
                    message = "Uploaded but following are duplicate : " + duplicateMsg;
                }
                else
                {
                    //message = "Excel file uploaded has fiald";
                    message = "Excel file has been successfully uploaded";
                }



            }

            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return message;
        }
    }

}
