using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MFS.DistributionService.Service;
using MFS.SecurityService.Service;
using MFS.TransactionService.Models;
using MFS.TransactionService.Models.ViewModels;
using MFS.TransactionService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneMFS.SharedResources.CommonService;
using OneMFS.TransactionApiServer.Filters;

namespace OneMFS.TransactionApiServer.Controllers
{
    [Authorize]
    //[ApiGuardAuth]
    [Produces("application/json")]
    [Route("api/FundTransfer")]
    public class FundTransferController : Controller
    {
        private readonly IFundTransferService _fundTransferService;
        private readonly IDistributorDepositService _distributorDepositService;
        private readonly IDistributorService _distributorService;
        private readonly IAuditTrailService _auditTrailService;
        private readonly IErrorLogService errorLogService;
        public FundTransferController(IFundTransferService objFundTransferService, IDistributorDepositService objDistributorDepositService
            , IDistributorService objDistributorService, IAuditTrailService objAuditTrailService, IErrorLogService objerrorLogService)
        {
            this._fundTransferService = objFundTransferService;
            this._distributorDepositService = objDistributorDepositService;
            this._distributorService = objDistributorService;
            this._auditTrailService = objAuditTrailService;
            this.errorLogService = objerrorLogService;
        }
        [HttpGet]
        [Route("GetGlList")]
        public object GetGlList()
        {
            try
            {
                return _fundTransferService.GetGlList();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("getGlDetailsForRobi")]
        public object getGlDetailsForRobi()
        {
            try
            {
                return _fundTransferService.getGlDetailsForRobi();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("getGlDetailsForAirtel")]
        public object getGlDetailsForAirtel()
        {
            try
            {
                return _fundTransferService.getGlDetailsForAirtel();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("getGlDetailsForBlink")]
        public object getGlDetailsForBlink()
        {
            try
            {
                return _fundTransferService.getGlDetailsForBlink();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("getGlDetailsForTtalk")]
        public object getGlDetailsForTtalk()
        {
            try
            {
                return _fundTransferService.getGlDetailsForTtalk();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetACList")]
        public object GetACList()
        {
            try
            {
                return _fundTransferService.GetACList();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetAmountByGL")]
        public object GetAmountByGL(string sysCode)
        {
            try
            {
                return _fundTransferService.GetAmountByGL(sysCode);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetAmountByAC")]
        public object GetAmountByAC(string mPhone)
        {
            try
            {
                return _fundTransferService.GetAmountByAC(mPhone);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpPost]
        [Route("GetTransactionDetailsByPayAmount")]
        public object GetTransactionDetailsByPayAmount(string from, string to, [FromBody]FundTransfer fundTransferModel)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (fundTransferModel.PayAmt == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int pFrom = 0;
                        int pTo = 0;
                        String result = null;
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            if (fundTransferModel.Hotkey.Substring(0, fundTransferModel.Hotkey.IndexOf(" ")) == "AC")
                            {
                                VMACandGLDetails objVMACandGLDetails = new VMACandGLDetails();
                                objVMACandGLDetails = _fundTransferService.GetACandGLDetailsByMphone(fundTransferModel.TransFrom);
                                objVMTransactionDetails.ACNo = objVMACandGLDetails.ACNo;
                                objVMTransactionDetails.GLCode = objVMACandGLDetails.GLCode;
                                objVMTransactionDetails.GLSysCoaCode = objVMACandGLDetails.GLSysCoaCode;
                                objVMTransactionDetails.GLName = objVMACandGLDetails.GLName;
                                objVMTransactionDetails.CoaDesc = objVMACandGLDetails.CoaDesc;

                            }
                            else
                            {
                                pFrom = from.IndexOf(" (") + " (".Length;
                                pTo = from.LastIndexOf(")");
                                result = from.Substring(pFrom, pTo - pFrom);
                                objVMTransactionDetails.ACNo = "";
                                objVMTransactionDetails.GLCode = result;
                                objVMTransactionDetails.GLName = from.Substring(0, from.IndexOf("("));
                            }

                            objVMTransactionDetails.DebitAmount = fundTransferModel.PayAmt;
                            objVMTransactionDetails.CreditAmount = 0;

                        }
                        else if (i == 1)
                        {
                            if (fundTransferModel.Hotkey.Substring(fundTransferModel.Hotkey.LastIndexOf(' ') + 1) == "AC")
                            {
                                VMACandGLDetails objVMACandGLDetails = new VMACandGLDetails();
                                objVMACandGLDetails = _fundTransferService.GetACandGLDetailsByMphone(fundTransferModel.TransTo);
                                objVMTransactionDetails.ACNo = objVMACandGLDetails.ACNo;
                                objVMTransactionDetails.GLCode = objVMACandGLDetails.GLCode;
                                objVMTransactionDetails.GLSysCoaCode = objVMACandGLDetails.GLSysCoaCode;
                                objVMTransactionDetails.GLName = objVMACandGLDetails.GLName;
                                objVMTransactionDetails.CoaDesc = objVMACandGLDetails.CoaDesc;

                            }
                            else
                            {
                                pFrom = to.IndexOf(" (") + " (".Length;
                                pTo = to.LastIndexOf(")");
                                result = to.Substring(pFrom, pTo - pFrom);
                                objVMTransactionDetails.ACNo = "";
                                objVMTransactionDetails.GLCode = result;
                                objVMTransactionDetails.GLName = to.Substring(0, to.IndexOf("("));
                            }


                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = fundTransferModel.PayAmt;
                        }
                        else
                        {
                            objVMTransactionDetails.ACNo = "";
                            objVMTransactionDetails.GLCode = "";
                            objVMTransactionDetails.GLName = "";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = 0;
                        }

                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpPost]
        [Route("GetTransDtlForRobiByPayAmount")]
        public object GetTransDtlForRobiByPayAmount([FromBody]RobiTopupStockEntry robiTopupStockEntryModel)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (robiTopupStockEntryModel.TransactionAmt == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        //String result = null;
                        double robiBalance = 0;
                        robiBalance = robiTopupStockEntryModel.TransactionAmt / robiTopupStockEntryModel.DiscountRatio;
                        double rowThreeFour = 0, rowFiveSix = 0;
                        //rowFiveSix = Math.Round(((robiBalance * .016) * .1), 2);
                        //rowThreeFour = Math.Round(((robiBalance * .016) - rowFiveSix), 2);
                        rowFiveSix = (robiBalance * .016) * .1;
                        rowThreeFour = (robiBalance * .016) - rowFiveSix;

                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            objVMTransactionDetails.GLCode = robiTopupStockEntryModel.GlCode;
                            objVMTransactionDetails.GLName = robiTopupStockEntryModel.GlName;
                            objVMTransactionDetails.DebitAmount = robiTopupStockEntryModel.TransactionAmt;
                            objVMTransactionDetails.CreditAmount = 0;

                        }
                        else if (i == 1)
                        {
                            objVMTransactionDetails.GLCode = "1020401";
                            objVMTransactionDetails.GLName = "ROBI AIRTIME RECEIVEABLE";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = robiTopupStockEntryModel.TransactionAmt;
                        }
                        else if (i == 2)
                        {
                            objVMTransactionDetails.GLCode = "1010301";
                            objVMTransactionDetails.GLName = "ROBI AIRTIME STOCK";
                            objVMTransactionDetails.DebitAmount = rowThreeFour;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else if (i == 3)
                        {
                            objVMTransactionDetails.GLCode = "2030501";
                            objVMTransactionDetails.GLName = "PRE PAID INCOME FROM ROBI";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowThreeFour;
                        }
                        else if (i == 4)
                        {
                            objVMTransactionDetails.GLCode = "1020501";
                            objVMTransactionDetails.GLName = "ADVANCE TAX ON AIRTIME COMMISSION";
                            objVMTransactionDetails.DebitAmount = rowFiveSix;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else
                        {
                            objVMTransactionDetails.GLCode = "2030501";
                            objVMTransactionDetails.GLName = "PRE PAID INCOME FROM ROBI";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowFiveSix;
                        }
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpPost]
        [Route("GetTransDtlForAirtelByPayAmount")]
        public object GetTransDtlForAirtelByPayAmount([FromBody]RobiTopupStockEntry robiTopupStockEntryModel)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (robiTopupStockEntryModel.TransactionAmt == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        //String result = null;
                        double robiBalance = 0;
                        robiBalance = robiTopupStockEntryModel.TransactionAmt / robiTopupStockEntryModel.DiscountRatio;
                        double rowThreeFour = 0, rowFiveSix = 0;
                        //rowFiveSix = Math.Round(((robiBalance * .016) * .1), 2);
                        //rowThreeFour = Math.Round(((robiBalance * .016) - rowFiveSix), 2);
                        rowFiveSix = (robiBalance * .016) * .1;
                        rowThreeFour = (robiBalance * .016) - rowFiveSix;

                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            objVMTransactionDetails.GLCode = robiTopupStockEntryModel.GlCode;
                            objVMTransactionDetails.GLName = robiTopupStockEntryModel.GlName;
                            objVMTransactionDetails.DebitAmount = robiTopupStockEntryModel.TransactionAmt;
                            objVMTransactionDetails.CreditAmount = 0;

                        }
                        else if (i == 1)
                        {
                            objVMTransactionDetails.GLCode = "1020402";
                            objVMTransactionDetails.GLName = "AIRTEL AIRTIME RECEIVEABLE";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = robiTopupStockEntryModel.TransactionAmt;
                        }
                        else if (i == 2)
                        {
                            objVMTransactionDetails.GLCode = "1010302";
                            objVMTransactionDetails.GLName = "AIRTEL AIRTIME STOCK";
                            objVMTransactionDetails.DebitAmount = rowThreeFour;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else if (i == 3)
                        {
                            objVMTransactionDetails.GLCode = "2030502";
                            objVMTransactionDetails.GLName = "PRE PAID INCOME FROM AIRTEL";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowThreeFour;
                        }
                        else if (i == 4)
                        {
                            objVMTransactionDetails.GLCode = "1020501";
                            objVMTransactionDetails.GLName = "ADVANCE TAX ON AIRTIME COMMISSION";
                            objVMTransactionDetails.DebitAmount = rowFiveSix;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else
                        {
                            objVMTransactionDetails.GLCode = "2030502";
                            objVMTransactionDetails.GLName = "PRE PAID INCOME FROM AIRTEL";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowFiveSix;
                        }
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpPost]
        [Route("GetTransDtlForBlinkByPayAmount")]
        public object GetTransDtlForBlinkByPayAmount([FromBody]RobiTopupStockEntry robiTopupStockEntryModel)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (robiTopupStockEntryModel.TransactionAmt == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        //String result = null;
                        double blBalance = 0;
                        blBalance = robiTopupStockEntryModel.TransactionAmt / robiTopupStockEntryModel.DiscountRatio;
                        double rowThreeFour = 0, rowFiveSix = 0;
                        rowFiveSix = (blBalance * 0.0099108027750248) * .1;
                        rowThreeFour = (blBalance * 0.0099108027750248) - rowFiveSix;

                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            objVMTransactionDetails.GLCode = robiTopupStockEntryModel.GlCode;
                            objVMTransactionDetails.GLName = robiTopupStockEntryModel.GlName;
                            objVMTransactionDetails.DebitAmount = robiTopupStockEntryModel.TransactionAmt;
                            objVMTransactionDetails.CreditAmount = 0;

                        }
                        else if (i == 1)
                        {
                            objVMTransactionDetails.GLCode = "1020403";
                            objVMTransactionDetails.GLName = "BANALALINK AIRTIME RECEIVABLE";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = robiTopupStockEntryModel.TransactionAmt;
                        }
                        else if (i == 2)
                        {
                            objVMTransactionDetails.GLCode = "1010303";
                            objVMTransactionDetails.GLName = "BANGLALINK  AIRTIME STOCK";
                            objVMTransactionDetails.DebitAmount = rowThreeFour;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else if (i == 3)
                        {
                            objVMTransactionDetails.GLCode = "2030503";
                            objVMTransactionDetails.GLName = "PREPAID INCOME FROM BANGLALINK";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowThreeFour;
                        }
                        else if (i == 4)
                        {
                            objVMTransactionDetails.GLCode = "1020501";
                            objVMTransactionDetails.GLName = "ADVANCE TAX ON AIRTIME COMMISSION";
                            objVMTransactionDetails.DebitAmount = rowFiveSix;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else
                        {
                            objVMTransactionDetails.GLCode = "2030503";
                            objVMTransactionDetails.GLName = "PREPAID INCOME FROM BANGLALINK";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowFiveSix;
                        }
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpPost]
        [Route("GetTransDtlForTtalkByPayAmount")]
        public object GetTransDtlForTtalkByPayAmount([FromBody]RobiTopupStockEntry ttalkTopupStockEntryModel)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (ttalkTopupStockEntryModel.TransactionAmt == 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        //String result = null;
                        double blBalance = 0;
                        blBalance = ttalkTopupStockEntryModel.TransactionAmt / ttalkTopupStockEntryModel.DiscountRatio;

                        double rowThreeFour = 0, rowFiveSix = 0;
                        //rowFiveSix = (blBalance * 0.0244498777506112) * .1;
                        //rowThreeFour = (blBalance * 0.0244498777506112) - rowFiveSix;
                        rowFiveSix = (blBalance * 0.025) * .1;//since 2.5%
                        rowThreeFour = (blBalance * 0.025) - rowFiveSix;

                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            objVMTransactionDetails.GLCode = ttalkTopupStockEntryModel.GlCode;
                            objVMTransactionDetails.GLName = ttalkTopupStockEntryModel.GlName;
                            objVMTransactionDetails.DebitAmount = ttalkTopupStockEntryModel.TransactionAmt;
                            objVMTransactionDetails.CreditAmount = 0;

                        }
                        else if (i == 1)
                        {
                            objVMTransactionDetails.GLCode = "1020404";
                            objVMTransactionDetails.GLName = "TELETALK AIRTIME RECEIVABLE";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = ttalkTopupStockEntryModel.TransactionAmt;
                        }
                        else if (i == 2)
                        {
                            objVMTransactionDetails.GLCode = "1010304";
                            objVMTransactionDetails.GLName = "TELETALK  AIRTIME STOCK";
                            objVMTransactionDetails.DebitAmount = rowThreeFour;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else if (i == 3)
                        {
                            objVMTransactionDetails.GLCode = "2030504";
                            objVMTransactionDetails.GLName = "PREPAID INCOME FROM TELETALK";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowThreeFour;
                        }
                        else if (i == 4)
                        {
                            objVMTransactionDetails.GLCode = "1020501";
                            objVMTransactionDetails.GLName = "ADVANCE TAX ON AIRTIME COMMISSION";
                            objVMTransactionDetails.DebitAmount = rowFiveSix;
                            objVMTransactionDetails.CreditAmount = 0;
                        }
                        else
                        {
                            objVMTransactionDetails.GLCode = "2030504";
                            objVMTransactionDetails.GLName = "PREPAID INCOME FROM TELETALK";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = rowFiveSix;
                        }
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetTransactionDetailsByTransactionNo")]
        public object GetTransactionDetailsByTransactionNo(string transNo)
        {
            try
            {
                List<VMTransactionDetails> VMTransactionDetaillist = new List<VMTransactionDetails>();
                if (string.IsNullOrEmpty(transNo))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        objVMTransactionDetails.ACNo = "";
                        objVMTransactionDetails.GLCode = "";
                        objVMTransactionDetails.GLName = "";
                        objVMTransactionDetails.DebitAmount = 0;
                        objVMTransactionDetails.CreditAmount = 0;
                        objVMTransactionDetails.Remarks = "";
                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                else
                {
                    FundTransfer objFundTransfer = new FundTransfer();
                    objFundTransfer = _fundTransferService.SingleOrDefaultByCustomField(transNo, "TransNo", new FundTransfer());

                    for (int i = 0; i < 6; i++)
                    {

                        VMTransactionDetails objVMTransactionDetails = new VMTransactionDetails();
                        if (i == 0)
                        {
                            objVMTransactionDetails.ACNo = objFundTransfer.TransFrom;
                            //objVMTransactionDetails.GLCode = objFundTransfer.FromSysCoaCode;
                            objVMTransactionDetails.GLCode = _fundTransferService.GetCoaCodeBySysCoaCode(objFundTransfer.FromSysCoaCode);
                            if (objFundTransfer.Hotkey == "AC TO GL")
                            {
                                objVMTransactionDetails.ACHolderName = _distributorService.GetCompanyAndHolderName(objFundTransfer.TransFrom).name;
                                objVMTransactionDetails.GLName = objFundTransfer.Particular.Substring(0, objFundTransfer.Particular.IndexOf("="));
                            }
                            else
                            {
                                objVMTransactionDetails.GLName = objFundTransfer.Particular.Substring(0, objFundTransfer.Particular.IndexOf("="));
                            }

                            objVMTransactionDetails.DebitAmount = objFundTransfer.PayAmt;
                            objVMTransactionDetails.CreditAmount = 0;
                            objVMTransactionDetails.Remarks = objFundTransfer.Remarks;

                        }
                        else if (i == 1)
                        {
                            objVMTransactionDetails.ACNo = objFundTransfer.TransTo;
                            //objVMTransactionDetails.GLCode = objFundTransfer.ToSysCoaCode;
                            objVMTransactionDetails.GLCode = _fundTransferService.GetCoaCodeBySysCoaCode(objFundTransfer.ToSysCoaCode);
                            objVMTransactionDetails.GLName = objFundTransfer.Particular.Substring(objFundTransfer.Particular.LastIndexOf('>') + 1);
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = objFundTransfer.PayAmt;
                        }
                        else
                        {
                            objVMTransactionDetails.ACNo = "";
                            objVMTransactionDetails.GLCode = "";
                            objVMTransactionDetails.GLName = "";
                            objVMTransactionDetails.DebitAmount = 0;
                            objVMTransactionDetails.CreditAmount = 0;
                        }

                        VMTransactionDetaillist.Add(objVMTransactionDetails);
                    }
                }
                double totalDebitAmt = 0;
                double totalCreditAmt = 0;
                foreach (var item in VMTransactionDetaillist)
                {
                    totalDebitAmt += item.DebitAmount;
                    totalCreditAmt += item.CreditAmount;
                }

                VMTransactionDetails obj = new VMTransactionDetails();
                obj.GLCode = "";
                obj.GLName = "Total :";
                obj.DebitAmount = totalDebitAmt;
                obj.CreditAmount = totalCreditAmt;
                string totalAmt = totalDebitAmt.ToString("N2");

                NumericWordConversion numericWordConversion = new NumericWordConversion();
                obj.InWords = numericWordConversion.InWords(Convert.ToDecimal(totalAmt));

                VMTransactionDetaillist.Add(obj);

                return VMTransactionDetaillist;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveFundTransferEntry")]
        public object saveFundTransferEntry(bool isEditMode, string evnt, string from, string to, [FromBody]FundTransfer fundTransferModel)
        {
            try
            {
                if (isEditMode != true)
                {
                    try
                    {
                        //FundTransfer objFundTransfer = new FundTransfer();
                        fundTransferModel.TransNo = _distributorDepositService.GetTransactionNo();
                        fundTransferModel.TransDate = System.DateTime.Now;
                        //fundTransferModel.FromCatId = "S";
                        //fundTransferModel.ToCatId = "S";
                        fundTransferModel.Status = "M";
                        fundTransferModel.BalanceType = "M";
                        fundTransferModel.ToBalanceType = "M";
                        fundTransferModel.MsgAmt = fundTransferModel.PayAmt;
                        //fundTransferModel.Hotkey = "GL TO GL";
                        string fromGlorAC = null, toGlorAC = null;
                        if (fundTransferModel.Hotkey.Substring(0, fundTransferModel.Hotkey.IndexOf(" ")) == "AC")
                        {
                            fromGlorAC = from;
                        }
                        else
                        {
                            fromGlorAC = from.Substring(0, from.IndexOf("("));
                        }
                        if (fundTransferModel.Hotkey.Substring(fundTransferModel.Hotkey.LastIndexOf(' ') + 1) == "AC")
                        {
                            toGlorAC = to;
                        }
                        else
                        {
                            toGlorAC = to.Substring(0, to.IndexOf("("));
                        }




                        fundTransferModel.Particular = fromGlorAC + " => " + toGlorAC;
                        fundTransferModel.EntryDate = System.DateTime.Now;
                        _fundTransferService.Add(fundTransferModel);

                        //Insert into audit trial audit and detail
                        _auditTrailService.InsertModelToAuditTrail(fundTransferModel, fundTransferModel.EntryUser, 9, 3, "Fund Entry (" + fundTransferModel.Hotkey + ")", fundTransferModel.TransNo, "Saved Successfully!");

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    return true;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveBranchCashIn")]
        public object saveBranchCashIn(bool isEditMode, [FromBody]BranchCashIn branchCashIn)
        {
            try
            {
                if (isEditMode != true)
                {
                    try
                    {
                        //FundTransfer objFundTransfer = new FundTransfer();
                        branchCashIn.TransNo = _distributorDepositService.GetTransactionNo();
                        string successOrErrorMsg = _fundTransferService.saveBranchCashIn(branchCashIn).ToString();

                        string response = null;
                        if (successOrErrorMsg == "1")
                        {
                            response = "Save Successfully!";
                        }
                        else
                        {
                            response = successOrErrorMsg;
                        }

                        //Insert into audit trial audit and detail                      
                        _auditTrailService.InsertModelToAuditTrail(branchCashIn, branchCashIn.CheckedUser, 9, 3, "Brach Cash In (Deposit)", branchCashIn.Mphone, response);


                        return successOrErrorMsg;

                    }
                    catch (Exception ex)
                    {

                        return "Something Error";
                    }



                }
                else
                {
                    return "Something Error";
                }
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveCommiConvert")]
        public object saveCommiConvert([FromBody]BranchCashIn branchCashIn)
        {
            try
            {
                //FundTransfer objFundTransfer = new FundTransfer();
                branchCashIn.TransNo = _distributorDepositService.GetTransactionNo();
                //string successOrErrorMsg = _fundTransferService.saveBranchCashIn(branchCashIn).ToString();
                string successOrErrorMsg = null;

                string response = null;
                if (successOrErrorMsg == "1")
                {
                    response = "Save Successfully!";
                }
                else
                {
                    response = successOrErrorMsg;
                }

                //Insert into audit trial audit and detail                      
                _auditTrailService.InsertModelToAuditTrail(branchCashIn, branchCashIn.CheckedUser, 9, 3, "Commission Convertion", branchCashIn.Mphone, response);


                return successOrErrorMsg;

            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        private string GetGLName(string from)
        {
            int pFrom = 0, pTo = 0;
            pFrom = from.IndexOf(" (") + " (".Length;
            pTo = from.LastIndexOf(")");
            return from.Substring(pFrom, pTo - pFrom);
        }

        [HttpGet]
        [Route("GetTransactionList")]
        public object GetTransactionList(string hotkey, string branchCode, double transAmtLimt)
        {
            try
            {
                return _fundTransferService.GetTransactionList(hotkey, branchCode, transAmtLimt);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("AproveOrRejectFundTransfer")]
        public object AproveOrRejectFundTransfer(string evnt, string transType, [FromBody]FundTransfer fundTransferModel)
        {
            string successOrErrorMsg = null;
            string checkedUser = fundTransferModel.CheckUser;
            fundTransferModel = _fundTransferService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "TransNo", new FundTransfer());
            fundTransferModel.CheckUser = checkedUser;
            try
            {
                //string status = _fundTransferService.getStatusByTransNo(fundTransferModel.TransNo)
                if (fundTransferModel.Status == "M")
                {
                    if (evnt == "register")
                    {
                        fundTransferModel.Status = "A";
                        //fundTransferModel.Status = "P";
                        fundTransferModel.CheckDate = System.DateTime.Now;

                        successOrErrorMsg = _fundTransferService.DataInsertToTransMSTandDTL(fundTransferModel, transType);
                        if (successOrErrorMsg.ToString() == "1")
                        {
                            _fundTransferService.UpdateByStringField(fundTransferModel, "TransNo");
                        }

                        //Insert into audit trial audit and detail
                        string response = successOrErrorMsg.ToString() == "1" ? "Approved Successfully!" : successOrErrorMsg.ToString();
                        FundTransfer prevModel = _fundTransferService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "TransNo", new FundTransfer());
                        prevModel.Status = "M";//insert for only audit trail
                        _auditTrailService.InsertUpdatedModelToAuditTrail(fundTransferModel, prevModel, fundTransferModel.CheckUser, 9, 4, "Fund Transfer (" + fundTransferModel.Hotkey + ")", fundTransferModel.TransNo, response);


                        return successOrErrorMsg;

                    }
                    else if (evnt == "reject")
                    {
                        fundTransferModel.Status = "R";
                        fundTransferModel.CheckDate = System.DateTime.Now;
                        _fundTransferService.UpdateByStringField(fundTransferModel, "TransNo");

                        //Insert into audit trial audit and detail
                        FundTransfer prevModel = _fundTransferService.SingleOrDefaultByCustomField(fundTransferModel.TransNo, "TransNo", new FundTransfer());
                        prevModel.Status = "M";//insert for only audit trail
                        _auditTrailService.InsertUpdatedModelToAuditTrail(fundTransferModel, prevModel, fundTransferModel.CheckUser, 9, 4, "Fund Transfer (" + fundTransferModel.Hotkey + ")", fundTransferModel.TransNo, "Rejected Successfully!");

                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return successOrErrorMsg = "Failed";
                }


            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveBranchCashOut")]
        public object saveBranchCashOut(bool isEditMode, [FromBody]TblPortalCashout branchCashOut)
        {
            try
            {
                if (isEditMode != true)
                {
                    try
                    {
                        //FundTransfer objFundTransfer = new FundTransfer();
                        //branchCashIn.TransNo = _distributorDepositService.GetTransactionNo();
                        //_fundTransferService.saveBranchCashOut(branchCashOut);

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    return true;

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("AproveOrRejectBranchCashout")]
        public object AproveOrRejectBranchCashout(string evnt, [FromBody]TblPortalCashout tblPortalCashout)
        {
            try
            {
                string successOrErrorMsg = _fundTransferService.AproveOrRejectBranchCashout(tblPortalCashout, evnt).ToString();

                //Insert into audit trial audit and detail
                string response = null;
                if (successOrErrorMsg == "1")
                {
                    if (evnt == "register")
                        response = "Cashout approved successfully";
                    else
                        response = "Reject successfully";
                }
                else if (successOrErrorMsg == "Failed")
                {
                    response = "Failed";
                }
                else
                {
                    //response = "Not rejected";
                    response = successOrErrorMsg;
                }
                _auditTrailService.InsertModelToAuditTrail(tblPortalCashout, tblPortalCashout.CheckBy, 9, 3, "Brach Cash Out (Withdrawal)", tblPortalCashout.Mphone, response);
                return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
            }
        }


        [HttpGet]
        [Route("CheckData")]
        public string CheckData(string transNo, string mphone, double amount)
        {
            try
            {
                return _fundTransferService.CheckData(transNo, mphone, amount);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveRobiTopupStockEntry")]
        public object saveRobiTopupStockEntry([FromBody]RobiTopupStockEntry robiTopupStockEntryModel)
        {
            try
            {
                string successOrErrorMsg = _fundTransferService.saveRobiTopupStockEntry(robiTopupStockEntryModel).ToString();

                //Insert into audit trial audit and detail
                string response = null;
                string whichId = null;
                if (successOrErrorMsg == "Failed")
                {
                    response = "Failed";
                    whichId = robiTopupStockEntryModel.GlName;
                }
                else
                {
                    response = "Added Successfully";
                    whichId = successOrErrorMsg;
                }

                _auditTrailService.InsertModelToAuditTrail(robiTopupStockEntryModel, robiTopupStockEntryModel.EntryUser, 9, 3, "Robi Topup Stock Entry", whichId, response);
                return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }


        }


        [ApiGuardAuth]
        [HttpPost]
        [Route("saveAirtelTopupStockEntry")]
        public object saveAirtelTopupStockEntry([FromBody]RobiTopupStockEntry robiTopupStockEntryModel)
        {
            try
            {
                string successOrErrorMsg = _fundTransferService.saveAirtelTopupStockEntry(robiTopupStockEntryModel).ToString();

                //Insert into audit trial audit and detail
                string response = null;
                string whichId = null;
                if (successOrErrorMsg == "Failed")
                {
                    response = "Failed";
                    whichId = robiTopupStockEntryModel.GlName;
                }
                else
                {
                    response = "Added Successfully";
                    whichId = successOrErrorMsg;
                }

                _auditTrailService.InsertModelToAuditTrail(robiTopupStockEntryModel, robiTopupStockEntryModel.EntryUser, 9, 3, "Airtel Topup Stock Entry", whichId, response);
                return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }


        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveBlinkTopupStockEntry")]
        public object saveBlinkTopupStockEntry([FromBody]RobiTopupStockEntry robiTopupStockEntryModel)
        {
            try
            {
                string successOrErrorMsg = _fundTransferService.saveBlinkTopupStockEntry(robiTopupStockEntryModel).ToString();

                //Insert into audit trial audit and detail
                string response = null;
                string whichId = null;
                if (successOrErrorMsg == "Failed")
                {
                    response = "Failed";
                    whichId = robiTopupStockEntryModel.GlName;
                }
                else
                {
                    response = "Added Successfully";
                    whichId = successOrErrorMsg;
                }

                _auditTrailService.InsertModelToAuditTrail(robiTopupStockEntryModel, robiTopupStockEntryModel.EntryUser, 9, 3, "Banglalink Topup Stock Entry", whichId, response);
                return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }


        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("saveTtalkTopupStockEntry")]
        public object saveTtalkTopupStockEntry([FromBody]RobiTopupStockEntry ttalkTopupStockEntryModel)
        {
            try
            {
                string successOrErrorMsg = _fundTransferService.saveTtalkTopupStockEntry(ttalkTopupStockEntryModel).ToString();

                //Insert into audit trial audit and detail
                string response = null;
                string whichId = null;
                if (successOrErrorMsg == "Failed")
                {
                    response = "Failed";
                    whichId = ttalkTopupStockEntryModel.GlName;
                }
                else
                {
                    response = "Added Successfully";
                    whichId = successOrErrorMsg;
                }

                _auditTrailService.InsertModelToAuditTrail(ttalkTopupStockEntryModel, ttalkTopupStockEntryModel.EntryUser, 9, 3, "Teletalk Topup Stock Entry", whichId, response);
                return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }


        }

        [HttpGet]
        [Route("getAmountByTransNo")]
        public object getAmountByTransNo(string transNo, string mobile)
        {
            try
            {
                return _fundTransferService.getAmountByTransNo(transNo, mobile);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

            //return true;
        }

        [HttpGet]
        [Route("GetGLBalanceByGLSysCoaCode")]
        public object GetGLBalanceByGLSysCoaCode(string fromSysCoaCode)
        {
            try
            {
                return _fundTransferService.GetGLBalanceByGLSysCoaCode(fromSysCoaCode);
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetCommissionGlListForDDL")]
        public object GetCommissionGlListForDDL()
        {
            try
            {
                return _fundTransferService.GetCommissionGlListForDDL();
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }

        }

        [HttpGet]
        [Route("GetCommssionMobileList")]
        public object GetCommssionMobileList(string sysCoaCode,string fromCatId, string entryOrApproval)
        {
            try
            {
                return _fundTransferService.GetCommssionMobileList(sysCoaCode, fromCatId, entryOrApproval);
            }
            catch (Exception ex)
            {

                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [ApiGuardAuth]
        [HttpPost]
        [Route("SaveCommissionEntry")]
        public object SaveCommissionEntry(string entryBy, string toCatId,string fromCatId, string entrybrCode, [FromBody]List<CommissionMobile> commissionMobileList)
        {
            try
            {
                foreach (var item in commissionMobileList)
                {
                    string transNo = _distributorDepositService.GetTransactionNo();
                    _fundTransferService.SaveCommissionEntry(item, entryBy, toCatId, fromCatId, entrybrCode, transNo);
                    //Insert into audit trial audit and detail
                    item.Status = "M";
                    _auditTrailService.InsertModelToAuditTrail(item, entryBy, 9, 3, "Commission Entry", item.Mphone, "Saved Successfully!");
                }
                return true;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }

        [HttpGet]
        [Route("CheckPendingApproval")]
        public string CheckPendingApproval()
        {
            string status = null;
            try
            {
                status = _fundTransferService.CheckPendingApproval();
                return status;
            }
            catch (Exception ex)
            {

                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString()).ToString();
            }
        }


        [ApiGuardAuth]
        [HttpPost]
        [Route("AproveOrRejectCommissionEntry")]
        public object AproveOrRejectCommissionEntry(string evnt, string entryBy, [FromBody]List<CommissionMobile> commissionMobileList)
        {
            try
            {
                string successOrErrorMsg = null;
                foreach (var item in commissionMobileList)
                {
                    if (evnt == "register")
                    {
                        successOrErrorMsg = _fundTransferService.AproveOrRejectCommissionEntry(item, entryBy);

                        //Insert into audit trial audit and detail
                        item.Status = "A";
                        string response = successOrErrorMsg.ToString() == "1" ? "Approved Successfully!" : successOrErrorMsg.ToString();
                        CommissionMobile prevModel = new CommissionMobile();
                        prevModel.Status = "M";//insert for only audit trail
                        _auditTrailService.InsertUpdatedModelToAuditTrail(item, prevModel, entryBy, 9, 4, "Commission Approval", item.TransNo, response);
                    }
                    else //if (evnt == "reject")
                    {

                        _fundTransferService.UpdateCommissionEntry(item, entryBy);

                        item.Status = "R";
                        //Insert into audit trial audit and detail
                        CommissionMobile prevModel = new CommissionMobile();
                        prevModel.Status = "M";//insert for only audit trail
                        _auditTrailService.InsertUpdatedModelToAuditTrail(item, prevModel, entryBy, 9, 4, "Commission Approval", item.TransNo, "Rejected Successfully!");
                    }

                }

                return successOrErrorMsg;
            }
            catch (Exception ex)
            {
                return errorLogService.InsertToErrorLog(ex, MethodBase.GetCurrentMethod().Name, Request.Headers["UserInfo"].ToString());
            }
        }
    }
}