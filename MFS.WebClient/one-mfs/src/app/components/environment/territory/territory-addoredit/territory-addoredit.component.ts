
import { Component, OnInit } from '@angular/core';
import { AreaService } from '../../../../services/environment';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../../shared/_services/authentication.service';
import { TerritoryService } from '../../../../services/environment/territory.service';
import { DistributorService } from 'src/app/services/distribution';
import { Message } from 'primeng/components/common/api';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-territory-addoredit',
  templateUrl: './territory-addoredit.component.html',
  styleUrls: ['./territory-addoredit.component.css']
})
export class TerritoryAddoreditComponent implements OnInit {

    territoryModel: any = {};
    regionCategoryList: any;
    currentUserModel: any = {};
    entityId: string;
    areaList: any; 
    msgs: Message[] = [];
    activeIndex: number = 0;
    isRegPermit = false;
    error: boolean = false;
    isEditMode: boolean = false;
    constructor(private areaService: AreaService,
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService,
        private distributionService: DistributorService,
        private territoryService: TerritoryService,
        private messageService: MessageService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getRegionsDDL();
        this.getAreaDDL();
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getTerritoryById();
            this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }
    
    getTerritoryById(): any {
        this.territoryService.getTerritoryById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.territoryModel = data;                                    
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

    async getAreaDDL() {
        this.territoryService.GetAreasDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.areaList = data;
                },
                error => {
                    console.log(error);
                });
    }
    SelectAreasByRegion(value: string) {
        this.distributionService.getAreaListByRegion(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.areaList = data;
                },
                error => {
                    console.log(error);
                }
        );
    }
    GetTerritoryCode(value: string) {
        this.territoryService.GenerateTerritotyCode(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.territoryModel.code = data.code;
                },
                error => {
                    console.log(error);
                }
            );
    }
    validation(): any {

        if (!this.territoryModel.parent ||
            !this.territoryModel.name ||
            !this.territoryModel.regionCode 
            ) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
            return false;
        } else {
            return true;
        }

    }
    onSaveTerritory(event) {
        this.territoryModel.createdBy = this.currentUserModel.user.username;
        if (this.isEditMode) {
            this.territoryModel.isEditMode = true;
        }
        if (this.validation()) {
            this.territoryService.save(this.territoryModel).pipe(first())
                .subscribe(
                    data => {
                        window.history.back();
                        if (this.isEditMode && !this.isRegPermit) {
                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Territory Updated' });
                        }
                        else if
                            (this.isRegPermit && this.isEditMode) {
                            this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Territory Registered' });
                        }
                        else
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Territory added' });
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', detail: error, closable: false });
                    });   
        }
        
    }

}
