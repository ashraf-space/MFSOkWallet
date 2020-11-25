import { Component, OnInit } from '@angular/core';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-report-list',
  templateUrl: './report-list.component.html',
  styleUrls: ['./report-list.component.css']
})
export class ReportListComponent implements OnInit {

    gridConfig: any;
    currentUserModel: any = {};
    constructor(private mfsSettingService: MfsSettingService, private gridSettingService: GridSettingService, private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {        
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];        
        this.gridConfig.entityField = 'Id';

        this.gridConfig.columnList = [
            { field: 'ReportType', header: 'Type', width: '20%', filter: this.gridSettingService.getDefaultFilterable(), template: this.gridSettingService.getReportTypeTemplateForRowData() },
            { field: 'ReportName', header: 'Report Name', width: '30%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'ReportDescription', header: 'Description', width: '30%', filter: this.gridSettingService.getDefaultFilterable() }
        ];

        this.toggleListingProperty();
    };

    toggleListingProperty() {
        var endPoint = this.route.snapshot.routeConfig.path.split('/')[1];
        if (endPoint == 'list') {
            this.gridConfig.gridName = "Report list";
            this.gridConfig.gridIconClass = 'fas fa-list';

            this.gridConfig.columnList.push({ field: 'Id', header: 'Generate Report', width: '10%', isCustomAction: true, customActionIcon: 'fas fa-directions', filter: this.gridSettingService.getFilterableNone() });

            this.gridConfig.dataSourcePath = this.mfsSettingService.reportingApiServer + '/ReportInfo/GetReportInfoList';
            this.gridConfig.autoUpdateDataSource = true;
        }
        else if (endPoint == 'config') {
            this.gridConfig.gridName = "Report Config";
            this.gridConfig.gridIconClass = 'fas fa-tools';
            this.gridConfig.createStateUrl = '/report/config/';
            this.gridConfig.hasEditState = true;

            this.gridConfig.columnList.push({ field: 'Id', header: 'Action', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() });

            this.gridConfig.dataSourcePath = this.mfsSettingService.reportingApiServer + '/ReportInfo/GetReportConfigList';
            this.gridConfig.autoUpdateDataSource = true;
        }
    }

    onGenerate(event) {
        //console.log(event.Id);
        this.router.navigateByUrl('/report/details/' + event.Id);
    }

}
