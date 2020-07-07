import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuditTrailService } from '../../../../shared/_services/audit-trail.service';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { MfsSettingService } from 'src/app/services/mfs-setting.service';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { GenericGridComponent } from 'src/app/shared/directives/generic-grid/generic-grid.component';
import { first } from 'rxjs/operators';
@Component({
    selector: 'app-audit-trail-dtl',
    templateUrl: './audit-trail-dtl.component.html',
    styleUrls: ['./audit-trail-dtl.component.css']
})
export class AuditTrailDtlComponent implements OnInit {
    gridConfig: any;
    currentUserModel: any;
    @Input() entityId: any;
    @ViewChild(GenericGridComponent) child: GenericGridComponent;
    constructor(private auditTrailService: AuditTrailService, private router: Router,
        private route: ActivatedRoute, private gridSettingService: GridSettingService, private authService: AuthenticationService
        , private mfsSettingService: MfsSettingService, private mfsUtilityService: MfsUtilityService) {
        this.gridConfig = {};
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.initialiseGridConfig();        
    }
   
    cancel() {
        window.history.back();
    }
    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.dataSourcePath = this.mfsSettingService.securityApiServer + '/AuditTrail/getTrailDtlById?id=' + this.entityId.id;
        this.gridConfig.autoUpdateDataSource = true;
        this.gridConfig.autoIndexing = true;
        this.gridConfig.gridName = "Trail Detail";
        this.gridConfig.gridIconClass = 'fas fa-envelope-open-text';
        this.gridConfig.showUniversalFilter = false;
        this.gridConfig.hideCreateState = true;
        this.gridConfig.columnList = [
            { field: 'fieldName', header: 'Feild Name', width: '10%' },
            { field: 'preValue', header: 'Previous Value', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'curValue', header: 'Current Value', width: '10%', filter: this.gridSettingService.getDefaultDateFilterable() },
        ];
    }
}
