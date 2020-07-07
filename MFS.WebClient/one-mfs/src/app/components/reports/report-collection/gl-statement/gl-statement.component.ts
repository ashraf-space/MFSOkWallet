import { Component, OnInit } from '@angular/core';
import { TransactionReportService } from 'src/app/services/report/transaction-report.service';
import { first } from 'rxjs/operators';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
  selector: 'app-gl-statement',
  templateUrl: './gl-statement.component.html',
  styleUrls: ['./gl-statement.component.css']
})
export class GlStatementComponent implements OnInit {
    assetTypeList:any;
    glCoaCodenalelevelDDL: any;
    model: any;
    isLoading: boolean = false;
    error: boolean = false;
    constructor(private transactionReportService: TransactionReportService, private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
        this.assetTypeList = [           
            { label: 'Asset', value: 'A' },
            { label: 'Expense', value: 'E' },
            { label: 'Income', value: 'I' },
            { label: 'Liability', value: 'L' }
        ];
    }

    loadGLCodeNameLevel() {
        this.isLoading = true;
        this.transactionReportService.getGlCoaCodeNameLevelDDL(this.model.assetType)
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.glCoaCodenalelevelDDL = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.fromDate = this.mfsUtilityService.renderDate(this.model.fromDate, true);
            obj.toDate = this.mfsUtilityService.renderDate(this.model.toDate, true);
            obj.assetType = this.model.assetType;
            obj.sysCoaCode = this.model.sysCoaCode;

            if (this.model.sysCoaCode) {
                obj.coaDes = this.glCoaCodenalelevelDDL.find(it => {
                    return it.value.toLowerCase().includes(this.model.sysCoaCode.toLowerCase());
                }).label;
            }

            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (!this.model.fromDate || !this.model.toDate
            || !this.model.assetType || this.model.assetType == '0'
            || !this.model.sysCoaCode || this.model.sysCoaCode == '0') {
            return false;
        }
        else {
            return true;
        }
    }


}
