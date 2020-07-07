import { Component, OnInit } from '@angular/core';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { FeatureService } from '../../../../services/security';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-feature-list',
  templateUrl: './feature-list.component.html',
  styleUrls: ['./feature-list.component.css']
})
export class FeatureListComponent implements OnInit {
    gridConfig: any;      
    //optionList: any;

    constructor(private router: Router, private featureService: FeatureService, private gridSettingService: GridSettingService
        , private authService: AuthenticationService) {
        this.gridConfig = {};
       
        //this.optionList = [
        //    { label: '', value: 'Y', icon: 'fas fa-check' },
        //    { label: '', value: 'N', icon: 'fas fa-times' },
        //    { label: '', value: 'P', icon: 'far fa-clock' }        ];
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Feature List';
        this.gridConfig.gridIconClass = 'fab fa-phabricator';
        this.gridConfig.createStateUrl = '/feature/create/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';

        //this.gridConfig.isBatchSwitchBoxEdit = true;
        //this.gridConfig.showCaption = false;

        this.gridConfig.columnList = [
            { field: 'alias', header: 'Feature Name', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'icon', header: 'Icon', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'orderNo', header: 'Order Number', width: '10%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 10) },
            { field: 'url', header: 'link', width: '50%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'id', header: 'Action', width: '15%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
            //,
            //{ field: 'id', header: 'Action', width: '20%', isSelectButtonColumn: true, filter: this.gridSettingService.getFilterableNone(), optionList: this.optionList }
        ];

        this.featureService.getFeatureWorklist().pipe()
            .subscribe(data => {
                this.gridConfig.dataSource = data;
            })
    };

}
