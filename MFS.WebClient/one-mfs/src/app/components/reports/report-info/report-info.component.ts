import { Component, OnInit } from '@angular/core';
import { SelectItem } from 'primeng/api';
import { RoleService } from '../../../services/security';
import { ReportInfoService } from '../../../services/report/report-info.service';
import { first } from 'rxjs/operators';
import { MessageService, MenuItem, Message } from 'primeng/api';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-report-info',
    templateUrl: './report-info.component.html',
    styleUrls: ['./report-info.component.css']
})
export class ReportInfoComponent implements OnInit {

    constructor(private roleService: RoleService,
        private reportInfoService: ReportInfoService,
        private messageService: MessageService,
        private router: Router,
        private authService: AuthenticationService,
        private route: ActivatedRoute) { }
    reportInfoModel: any = {};
    error: boolean = false;
    reportList: any;
    roleList: SelectItem[];
    entityId: any;
    isRegistrationPermitted: any;
    isEditMode: boolean = false;
    selectRoles: SelectItem[];
    ngOnInit() {
        this.reportList = [
            { label: 'Kyc', value: 'K' },
            { label: 'Transaction', value: 'T' }
        ]
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.getReportConfigById();
            this.isEditMode = true;
            this.isRegistrationPermitted = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
        this.getRoleDdl();
    }

    getRoleDdl(): any {
        this.roleService.getRoleListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.roleList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    onReportInfoSave(event) {
        this.reportInfoModel._Roles = this.selectRoles;
        this.reportInfoService.onRoleInfoSave(this.reportInfoModel, this.isEditMode, event).pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        window.history.back();
                        this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Report Save' });
                    }
                },
                error => {
                    console.log(error);
                });
    }

    getReportConfigById(): any {
        this.reportInfoService.getReportConfigById(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.reportInfoModel = data; 
                        if (data._Roles) {
                            this.selectRoles = this.reportInfoModel._Roles;
                        }
                                               
                    }

                },
                error => {
                    console.log(error);
                }
            )
    }
}
