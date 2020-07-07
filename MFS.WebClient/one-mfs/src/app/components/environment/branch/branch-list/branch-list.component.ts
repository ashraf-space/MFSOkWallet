import { Component, OnInit } from '@angular/core';
import { BankBranchService } from '../../../../services/environment';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';

@Component({
  selector: 'app-branch-list',
  templateUrl: './branch-list.component.html',
  styleUrls: ['./branch-list.component.css']
})
export class BranchListComponent implements OnInit {
    gridConfig: any;
    ProgressSpinnerDlg: boolean = false;

    constructor(private bankBranchService: BankBranchService, private gridSettingService: GridSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        //this.ProgressSpinnerDlg = true;
        this.initialiseGridConfig();
        //setTimeout(() => {
        //    this.ProgressSpinnerDlg = false;
        //}, 5000);
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Bank Branch List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/bank-branch/addoredit/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';

        this.gridConfig.columnList = [
            { field: 'branchcode', header: 'Branch Code', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'branchname', header: 'Branch Name', width: '60%', filter: this.gridSettingService.getDefaultFilterable() },
            //{ field: 'orderNo', header: 'Order Number', width: '60%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 10) },
            { field: 'branchcode', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        this.bankBranchService.getBankBranchList().pipe()
            .subscribe(data => {
                this.gridConfig.dataSource = data;
            })
    };

}
