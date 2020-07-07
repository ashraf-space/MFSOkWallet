import { Component, OnInit } from '@angular/core';
import { AreaService } from '../../../../services/environment';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../../shared/_services/authentication.service';
import { TerritoryService } from '../../../../services/environment/territory.service';
import { ClusterService } from '../../../../services/environment/cluster.service';
import { DistributorService } from 'src/app/services/distribution';
import { MessageService } from 'primeng/api';
import { Message } from 'primeng/components/common/api';

@Component({
  selector: 'app-cluster-addoredit',
  templateUrl: './cluster-addoredit.component.html',
  styleUrls: ['./cluster-addoredit.component.css']
})
export class ClusterAddoreditComponent implements OnInit {
    clusterModel: any = {};
    regionCategoryList: any;
    isEditMode: boolean = false;
    currentUserModel: any = {};
    entityId: string;
    areaList: any;
    territoryList: any;
    msgs: Message[] = [];
    activeIndex: number = 0;
    isRegPermit = false;
    error: boolean = false;
    constructor(private areaService: AreaService,
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService,
        private distributionService: DistributorService,
        private territoryService: TerritoryService,
        private clusterService: ClusterService,
        private messageService: MessageService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getRegionsDDL();
        this.getAreaDDL();
        this.getTerritoryDDL();
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getClusterById();
            this.isRegPermit = this.authService.checkRegisterPermissionAccess(this.route.snapshot.routeConfig.path);
        }
    }
    
    getClusterById(): any {
        this.clusterService.GetClusterById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.clusterModel = data;
                },
                error => {
                    console.log(error);
                });
    };

    validation(): any {

        if (!this.clusterModel.parent ||
            !this.clusterModel.name ||
            !this.clusterModel.regionCode ||
            !this.clusterModel.areaCode
        ) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
            return false;
        } else {
            return true;
        }

    }

    onSaveCluster(event) {
        this.clusterModel.createdBy = this.currentUserModel.user.username;
        if (this.isEditMode) {
            this.clusterModel.isEditMode = true;
        }
        if (this.validation()) {
            this.clusterService.save(this.clusterModel).pipe(first())
                .subscribe(
                    data => {
                        window.history.back();
                        if (this.isEditMode && !this.isRegPermit) {
                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Cluster Updated' });
                        }
                        else if
                        (this.isRegPermit && this.isEditMode) {
                            this.messageService.add({ severity: 'success', summary: 'Register successfully', detail: 'Cluster Registered' });
                        }
                        else
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Cluster added' });
                    },
                    error => {
                        console.log(error);
                        this.messageService.add({ severity: 'error', summary: 'Erros Occured', detail: error, closable: false });
                    });
        }
        
    }
    GetClusterCode(value: string) {
        this.clusterService.GenerateClusterCode(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.clusterModel.code = data.code;
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
    SelectTerritoriesByArea(value: string) {
        this.distributionService.getTerritoryListByArea(value)
            .pipe(first())
            .subscribe(
                data => {
                    this.territoryList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }
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
    async getTerritoryDDL() {
        this.clusterService.getTerritoryDDL()
            .subscribe(
                data => {
                    this.territoryList = data;
                },
                error => {
                    console.log(error);
                });
    }

}
