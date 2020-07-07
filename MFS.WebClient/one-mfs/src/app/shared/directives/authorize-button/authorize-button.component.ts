import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MfsUtilityService } from '../../../services/mfs-utility.service';
import { AuthenticationService } from '../../_services';


@Component({
  selector: 'app-authorize-button',
  templateUrl: './authorize-button.component.html',
  styleUrls: ['./authorize-button.component.css']
})
export class AuthorizeButtonComponent implements OnInit {

    @Input() type: string;
    isAuthorized: boolean = false;
    currentUser: any = {};
    @Input() routeStateUrl: string;
    @Input() forcedDisable: boolean;
    @Input() text: string;

    isAdd: boolean = false;
    isEdit: boolean = false;
    isRegister: boolean = false;
    isDelete: boolean = false;
    isdetailEdit: boolean = false;
    isDetailRegister: boolean = false;
    isPassToMaker: boolean = false;
    isRejection: boolean = false;

    isSaveEntity: boolean = false;
    isRegisterEntity: boolean = false;

    entityId: any;
    isEditMode: boolean = false;

    @Output() onButtonClick: EventEmitter<string> = new EventEmitter<string>();

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private route: ActivatedRoute
    ) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    ngOnInit() {
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
        }

        if (this.type) {
            this.generateSecuredButton();
        }
    }

    async generateSecuredButton() {
        var obj = this.authenticationService.getCurrentStatePermission(this.route.snapshot.routeConfig.path);

        switch (this.type) {
            case 'add':
                this.isAdd = true;
                if (obj.ISADDPERMITTED == 1) {
                    this.isAuthorized = true;
                }
                break;
            case 'edit':
                this.isEdit = true;
                if (obj.ISREGISTRATIONPERMITTED == 1) {
                    this.isAuthorized = true;
                    this.isEdit = false;
                    this.isRegister = true;
                    break;
                }
                else if (obj.ISEDITPERMITTED == 1) {
                    this.isAuthorized = true;
                }
                break;
            case 'pass-to-maker':                
                if (obj.ISREGISTRATIONPERMITTED == 1) {
                    this.isAuthorized = true;
                    this.isPassToMaker = true;
                }
                break;
            case 'reject':                
                if (obj.ISREGISTRATIONPERMITTED == 1) {
                    this.isRejection = true;
                    this.isAuthorized = true;
                }
                break;
            case 'detail-edit':
                this.isdetailEdit = true;
                //if (obj.ISREGISTRATIONPERMITTED == 1) {
                //    this.isAuthorized = true;
                //    this.isdetailEdit = false;
                //    this.isDetailRegister = true;
                //    break;
                //}
                //else if (obj.ISEDITPERMITTED == 1) {
                //    this.isAuthorized = true;
                //}
                if (obj.ISEDITPERMITTED == 1) {
                    this.isAuthorized = true;
                }
                break;
            case 'delete':
                this.isDelete = true;
                if (obj.ISDELETEPERMITTED == 1) {
                    this.isAuthorized = true;
                }
                break;
            case 'save':
                this.isSaveEntity = true;
                if (obj.ISREGISTRATIONPERMITTED == 1) {
                    this.isAuthorized = true;
                    this.isSaveEntity = false;
                    this.isRegisterEntity = true;
                    break;
                }
                else if (obj.ISEDITPERMITTED == 1 && this.isEditMode) {
                    this.isAuthorized = true;
                    break;
                }
                else if (obj.ISADDPERMITTED == 1 && !this.isEditMode) {
                    this.isAuthorized = true;
                    break;
                }
                break;
            default:
        }
    }

    onButtonClickEvent(event): any {
        this.onButtonClick.emit(event);
    }

}
