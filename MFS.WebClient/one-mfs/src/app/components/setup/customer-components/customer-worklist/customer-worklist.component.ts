import { Component, OnInit } from '@angular/core';
import { FeatureCategoryService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';

@Component({
  selector: 'app-customer-worklist',
  templateUrl: './customer-worklist.component.html',
  styleUrls: ['./customer-worklist.component.css']
})
export class CustomerWorklistComponent implements OnInit {

    gridConfig: any;
    dataList: any;
    dataNode: any;    

    constructor(private gridSettingService: GridSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        var i;
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Customer List';
        this.gridConfig.gridIconClass = 'fas fa-users';

        this.gridConfig.columnList = [
            { field: 'Serial', header: 'Serial' },
            { field: 'Text', header: 'Text' },
            { field: 'DateCreated', header: 'Date Created', template: this.gridSettingService.getDateTemplateForRowData() }
        ];

        for (i = 0; i < 3500; i++) {
            this.dataNode = {};
            this.dataNode.Serial = i;
            this.dataNode.Text = Math.floor(100000 + Math.random() * 900000);
            this.dataNode.DateCreated = Date.now();
            this.gridConfig.dataSource.push(this.dataNode);
        }
    }

}
