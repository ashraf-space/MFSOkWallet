
import { Component, OnInit } from '@angular/core';
import { AreaService } from '../../../../services/environment';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../../shared/_services/authentication.service';
import { Message } from 'primeng/components/common/api';
import { MessageService } from 'primeng/api';


@Component({
    selector: 'app-area-addoredit',
    templateUrl: './area-addoredit.component.html',
    styleUrls: ['./area-addoredit.component.css']
})
export class AreaAddoreditComponent implements OnInit {
    regionCategoryList: any;
    areaModel: any = {};
    isEditMode: boolean = false;
    currentUserModel: any = {};
    entityId: string;
    msgs: Message[] = [];
    activeIndex: number = 0;
    isRegPermit = false;
    error: boolean = false;
    constructor(private areaService: AreaService,
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService,
        private messageService: MessageService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getRegionsDDL();
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getAreaById();
            this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }


    getAreaById(): any {
        this.areaService.getAreaById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.areaModel = data;
                },
                error => {
                    console.log(error);
                });

    };


    async getRegionsDDL() {
        this.areaService.getRegionsDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.regionCategoryList = data;
                },
                error => {
                    console.log(error);
                });
    }

    validation(): any {

        if (!this.areaModel.parent || !this.areaModel.name) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
            return false;
        } else {
            return true;
        }

    }
    onSaveArea(event) {

        this.areaModel.createdBy = this.currentUserModel.user.username;
        if (this.isEditMode) {
            this.areaModel.isEdit = true;
        }
        if (this.validation()) {
            this.areaService.save(this.areaModel).pipe(first())
                .subscribe(
                    data => {
                        window.history.back();
                        if (this.isEditMode && !this.isRegPermit) {
                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Area Updated' });
                        }
                        else if
                        (this.isRegPermit && this.isEditMode) {
                            this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Area Registered' });
                        }
                        else
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Area added' });
                    },
                    error => {
                        console.log(error);
                    });
        }




    }
}
