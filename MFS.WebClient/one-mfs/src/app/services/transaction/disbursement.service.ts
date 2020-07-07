import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { HttpClient } from '@angular/common/http';
import { map, toArray } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class disbursementService {

    constructor(private http: HttpClient, private transactionService: MfsSettingService) { }

    save(tblDisburseCompanyInfoModel: any) {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/Disbursement/Save', tblDisburseCompanyInfoModel)
            .pipe(map(model => {
                return model;
            }))
    }

    getDisburseCompanyList(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/getDisburseCompanyList')
            .pipe(map(model => {
                return model;
            }))
    }

    getCompanyAndBatchNoList(forPosting: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/getCompanyAndBatchNoList?forPosting=' + forPosting)
            .pipe(map(model => {
                return model;
            }))
    }
    getDisburseTypeList(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/GetDisburseTypeList')
            .pipe(map(model => {
                return model;
            }))
    }

    getDisburseNameCodeList(): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/getDisburseNameCodeList')
            .pipe(map(model => {
                return model;
            }))
    }

    saveCompanyLimit(tblDisburseAmtDtlMakeModel: any) {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/Disbursement/saveCompanyLimit', tblDisburseAmtDtlMakeModel)
            .pipe(map(model => {
                return model;
            }))
    }

    getTransactionList(transAmtLimt: number): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/GetTransactionList?transAmtLimt=' + transAmtLimt)
            .pipe(map(TransactionList => {
                return TransactionList;
            }));
    }

    GetTransactionDetailsByTransactionNo(transNo: any): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/GetTransactionDetailsByTransactionNo?transNo=' + transNo)
            .pipe(map(transactionDetails => {
                return transactionDetails;
            }));
    }

    AproveOrRejectDisburseAmountPosting(fundTransferModel: any, event: any): any {
        return this.http.post<any>(this.transactionService.transactionApiServer + '/Disbursement/AproveOrRejectDisburseAmountPosting?evnt=' + event, fundTransferModel)
            .pipe(map(model => {
                return model;
            }))
    }

    getBatchNo(OrganizationId: number, DisburseType: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/getBatchNo?id=' + OrganizationId + '&tp=' + DisburseType)
            .pipe(map(batchNo => {
                return batchNo;
            }))
    }



    UploadExcel(formData: FormData, organizationId: number, batchno: string, makerId: string, amount: number) {
        let headers = new HttpHeaders();

        headers.append('Content-Type', 'multipart/form-data');
        headers.append('Accept', 'application/json');

        const httpOptions = { headers: headers };

        //return this.http.post(this.url + '/UploadExcel', formData, httpOptions)
        return this.http.post<string>(this.transactionService.reportingApiServer + '/ExcelUpload/UploadExcel?organizationId=' + organizationId + '&batchno=' + batchno + '&makerId=' + makerId + '&amount=' + amount, formData, httpOptions)
            .pipe(map(data => {
                return data;
            }))
    }

    Process(batchno: string, companyName: string): any {
        return this.http.get<string>(this.transactionService.transactionApiServer + '/Disbursement/Process?batchno=' + batchno + '&companyName=' + companyName)
            .pipe(map(message => {
                return message;
            }))
    }

    checkProcess(batchno: string): any {
        return this.http.get<boolean>(this.transactionService.transactionApiServer + '/Disbursement/checkProcess?batchno=' + batchno)
            .pipe(map(data => {
                return data;
            }))
    }

    getValidOrInvalidData(processBatchNo: string, validOrInvalid: string, forPosting: string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/getValidOrInvalidData?processBatchNo=' + processBatchNo + '&validOrInvalid=' + validOrInvalid + '&forPosting=' + forPosting)
            .pipe(map(data => {
                return data;
            }))
    }

    SendToPostingLevel(processBatchNo: string, totalSum: number): any {
        return this.http.get<boolean>(this.transactionService.transactionApiServer + '/Disbursement/SendToPostingLevel?processBatchNo=' + processBatchNo + '&totalSum=' + totalSum)
            .pipe(map(message => {
                return message;
            }))
    }

    AllSend(processBatchNo: string, brCode: string, checkerId: string, totalSum: number): any {
        return this.http.get<boolean>(this.transactionService.transactionApiServer + '/Disbursement/AllSend?processBatchNo=' + processBatchNo + '&brCode=' + brCode + '&checkerId=' + checkerId + '&totalSum=' + totalSum)
            .pipe(map(message => {
                return message;
            }))
    }

    BatchDelete(processBatchNo: string, brCode: string, checkerId: string, totalSum: number): any {
        return this.http.get<string>(this.transactionService.transactionApiServer + '/Disbursement/BatchDelete?processBatchNo=' + processBatchNo + '&brCode=' + brCode + '&checkerId=' + checkerId + '&totalSum=' + totalSum)
            .pipe(map(message => {
                return message;
            }))
    }

    GetAccountDetails(accountNo:  string): any {
        return this.http.get<any>(this.transactionService.transactionApiServer + '/Disbursement/GetAccountDetails?accountNo=' + accountNo)
            .pipe(map(data => {
                return data;
            }))
    }


}
