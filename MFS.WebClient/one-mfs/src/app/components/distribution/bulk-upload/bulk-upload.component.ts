import { Component, OnInit, ViewChild } from '@angular/core';
import { MessageService } from 'primeng/api';
import { DistributorService } from 'src/app/services/distribution';
import { first } from 'rxjs/operators';
declare let jsPDF: any;

@Component({
    selector: 'app-bulk-upload',
    templateUrl: './bulk-upload.component.html',
    styleUrls: ['./bulk-upload.component.css']
})
export class BulkUploadComponent implements OnInit {
    bulkModel: any = {};
    bulkUploadTypeList: any;
    isDistributorShow: boolean = false;
    isAgentShow: boolean = false;
    isCustomerShow: boolean = false;
    isUploadDisabled: boolean = true;
    isLoading: boolean = false;
    displayMaximizable: boolean = false;
    @ViewChild('fileInput') fileInput;
    content: string;
    constructor(private messageService: MessageService, private distributionService: DistributorService) { }

    ngOnInit() {
        this.bulkUploadTypeList = [
            { label: "Distributor", value: "Distributor" },
            { label: "Agent", value: "Agent" },
            { label: "Customer", value: "Customer" }
        ];
    }

    showHideDiv() {
        if (this.bulkModel.bulkUploadType) {
            if (this.bulkModel.bulkUploadType == 'Distributor') {
                this.isDistributorShow = true;
                this.isAgentShow = false;
                this.isCustomerShow = false;
                this.disableUpload();
            }
            else if (this.bulkModel.bulkUploadType == 'Agent') {
                this.isDistributorShow = false;
                this.isAgentShow = true;
                this.isCustomerShow = false;
                this.disableUpload();
            }
            else {
                this.isDistributorShow = false;
                this.isAgentShow = false;
                this.isCustomerShow = true;
                this.disableUpload();
            }
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Bulk Upload Type Empty', detail: 'Select Bulk Upload Type First!' });
            this.bulkModel.bulkUploadType = '';
        }

    }
    disableUpload() {
        if (this.bulkModel.bulkUploadType == 'Distributor') {
            this.isUploadDisabled = true;
        }
        else if (this.bulkModel.bulkUploadType == 'Agent') {
            if (this.bulkModel.distributorAC) {
                this.distributionService.GetDistcodeAndNameByMphone(this.bulkModel.distributorAC)
                    .pipe(first())
                    .subscribe(
                        data => {
                            //this.isLoading = false;

                            if (data == null) {
                                this.bulkModel.distributorCode = null;
                                this.bulkModel.distributorName = null;
                                this.isUploadDisabled = true;
                            }
                            else {
                                this.bulkModel.distributorCode = data["DIST_CODE"];
                                this.bulkModel.distributorName = data["NAME"];
                                this.isUploadDisabled = false;                                
                            }
                        },
                        error => {
                            console.log(error);
                        }
                    );
            }
            //if (this.bulkModel.distributorAC && this.bulkModel.distributorCode && this.bulkModel.distributorName) {
            //    this.isUploadDisabled = false;
            //}
            //else {
            //    this.isUploadDisabled = true;
            //}
        }
        else {
            this.isUploadDisabled = false;
        }
    }

    bulkUploadFile() {
        if (this.bulkModel.bulkUploadType) {
            //this.tblDisburseModel.makerId = this.currentUserModel.user.username;
            //this.isLoading = true;
            let formData = new FormData();
            formData.append('upload', this.fileInput.nativeElement.files[0])
            this.distributionService.bulkUploadExcel(formData, this.bulkModel.bulkUploadType, this.bulkModel.distributorAC, this.bulkModel.distributorCode, this.bulkModel.distributorName).subscribe(result => {
                //this.message = result.toString();
                //this.loadAllUser();
                if (result.toString() == 'Excel file has been successfully uploaded')
                    this.messageService.add({ severity: 'success', summary: 'uploaded successfully', detail: 'Excel file has been successfully uploaded' });
                else {
                    //this.messageService.add({ severity: 'warn', summary: 'Failed', detail: result.toString() });
                    this.displayMaximizable = true;
                    this.content = result.toString();
                }
                    

                //setTimeout(() => {
                //    this.isLoading = false;
                //    location.reload();
                //}, 50);
            });
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Bulk Upload Type Empty', detail: 'Select Bulk Upload Type First!' });
        }

    }

  
}
