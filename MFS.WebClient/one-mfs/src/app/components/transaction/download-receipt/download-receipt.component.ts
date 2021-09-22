import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message, MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { BillCollectionCommonService } from 'src/app/services/transaction';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { AuthenticationService, ReceiptapiService } from 'src/app/shared/_services';

@Component({
    selector: 'app-download-receipt',
    templateUrl: './download-receipt.component.html',
    styleUrls: ['./download-receipt.component.css']
})
export class DownloadReceiptComponent implements OnInit {
    gridConfig: any;
    receiptUrl: any;
    currentUserModel: any = {};
    billNo: string;
    selectedCategory: string;
    error: boolean = false;
    msgs: Message[] = [];
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    categoryList: any;
    billTitle: any;
    subMenuTitle: any;

    obj: any = {};

    constructor(private messageService: MessageService, private billCollectionCommonService: BillCollectionCommonService
        , private authService: AuthenticationService
        , private route: ActivatedRoute, private router: Router, private gridSettingService: GridSettingService
        , private recapiService: ReceiptapiService) {

        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });


        this.receiptUrl = recapiService.receiptUrl;


    }

    ngOnInit() {
        this.billTitle = "Bill No";
        this.GetBillPayCategoriesDDL();
        //this.selectedCategory = null;
        this.loadCommonInitialiseGrid();
    }

    GetBillPayCategoriesDDL(): any {
        this.billCollectionCommonService.GetBillPayCategoriesDDL(this.currentUserModel.user.id)
            .pipe(first())
            .subscribe(
                data => {
                    this.categoryList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    ChangeTitle(): any {
        if ( this.selectedCategory == '0' || this.selectedCategory == '' || this.selectedCategory == null) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            //for Title, SubmenuTitle by methodName
            this.billCollectionCommonService.GetTitleSubmenuTitleByMethod(this.selectedCategory)
                .pipe(first())
                .subscribe(
                    data2 => {
                        this.billTitle = data2.BILLTITLE;
                        this.subMenuTitle = data2.SUBMENUTITLE;
                        this.loadCommonInitialiseGrid();                       
                    },
                    error => {
                        console.log(error);
                    }
                );







        }
    }

    loadCommonInitialiseGrid(): any {
        this.gridConfig.dataSource = [];

        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;
        this.gridConfig.showUniversalFilter = false;




        if (this.subMenuTitle) {
            this.gridConfig.columnList = [
                { field: 'trans_No', header: 'Trans No', width: '15%' },
                { field: 'trans_Date', header: 'Trans Date', width: '15%' },
                { field: 'msg_Amt', header: 'Amount', width: '15%' },
                { field: 'billno', header: this.billTitle ? this.billTitle : 'Bill No', width: '15%' },
                { field: 'ref_Phone', header: 'Beneficiary Mobile Number', width: '20%' },
                { field: 'subname', header: this.subMenuTitle, width: '15%' },
                { field: 'Id', header: 'Generate Receipt', width: '20%', isCustomAction: true, customActionIcon: 'fas fa-file-download' }
            ];
        }
        else {
            this.gridConfig.columnList = [
                { field: 'trans_No', header: 'Trans No', width: '15%' },
                { field: 'trans_Date', header: 'Trans Date', width: '15%' },
                { field: 'msg_Amt', header: 'Amount', width: '15%' },
                { field: 'billno', header: this.billTitle ? this.billTitle : 'Bill No', width: '15%' },
                { field: 'ref_Phone', header: 'Beneficiary Mobile Number', width: '20%' },
                //{ field: 'subname', header: 'Sub Name', width: '15%' },
                { field: 'Id', header: 'Generate Receipt', width: '20%', isCustomAction: true, customActionIcon: 'fas fa-file-download' }
            ];
        }
    }

    getReceiptListBySearch(): any {
        if (!this.billNo || this.billNo == '' ||
            this.selectedCategory == '0' || this.selectedCategory == '' || this.selectedCategory == null) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {


            //for Title, SubmenuTitle by methodName
            this.billCollectionCommonService.GetTitleSubmenuTitleByMethod(this.selectedCategory)
                .pipe(first())
                .subscribe(
                    data2 => {
                        this.billTitle = data2.BILLTITLE;
                        this.subMenuTitle = data2.SUBMENUTITLE;
                        this.loadCommonInitialiseGrid();

                        this.billCollectionCommonService.GetDataForCommonGrid(this.currentUserModel.user.branchCode, this.selectedCategory, '', this.billNo)
                            .pipe(first())
                            .subscribe(
                                data => {
                                    this.gridConfig.dataSource = data;

                                },
                                error => {
                                    console.log(error);
                                }
                            );
                    },
                    error => {
                        console.log(error);
                    }
                );







        }
    }


    onGenerate(event) {
        //window.open(this.receiptUrl + event.ref_Phone + "&Trans_ID=" + event.trans_No, "_blank");

        this.obj.mphone = event.ref_Phone;
        this.obj.Trans_ID = event.trans_No;

        const mapForm = document.createElement('form');
        mapForm.target = '_blank';
        mapForm.method = 'POST'; // or "post" if appropriate
        mapForm.action = this.receiptUrl;
        Object.keys(this.obj).forEach((param) => {
            const mapInput = document.createElement('input');
            mapInput.type = 'hidden';
            mapInput.name = param;
            mapInput.setAttribute('value', this.obj[param]);
            mapForm.appendChild(mapInput);
        });
        document.body.appendChild(mapForm);
        mapForm.submit();
    }


}
