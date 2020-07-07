import { Component, OnInit } from '@angular/core';
import { RateConfigService } from 'src/app/services/transaction/rate-config.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-rate-config-add-or-edit',
  templateUrl: './rate-config-add-or-edit.component.html',
  styleUrls: ['./rate-config-add-or-edit.component.css']
})
export class RateConfigAddOrEditComponent implements OnInit {

    entityId: number;
    isEditMode: boolean = false;

    model: any = {};
    yesNoOptions = [];
    statusList = [];

    constructor(private rateConfigService: RateConfigService, private router: Router, private route: ActivatedRoute, private messageService: MessageService) {
        this.yesNoOptions = [{ label: "Active", value: "Y" }, { label: "Pending", value: "P" }, { label: "Inactive", value: "N" }];
        this.statusList = [{ label: 'Active', value: 'A', icon: 'pi pi-check' }, { label: 'Inactive', value: 'I', icon: 'pi pi-times' }]
    }

    ngOnInit() {
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getRateConfigDetailsById();
        }
    }

    getRateConfigDetailsById(): any {
        this.rateConfigService.getRateConfigByConfigId(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.model = data;
                    this.generatePercentage();
                },
                error => {
                    console.log(error);
                });

    }

    generatePercentage(): any {
        this.model.schargePer = this.model.schargePer * 100;  
        this.model.spDistPer = this.model.spDistPer * 100;
        this.model.distPer = this.model.distPer * 100;
        this.model.agentPer = this.model.agentPer * 100;
    };

    onSave() {
        this.generateFraction();
        this.rateConfigService.save(this.model).pipe(first())
            .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.model.rateconfig_for + ' saved Successfully' });
                    window.history.back();
                },
                error => {
                    console.log(error);
                });
    }

    generateFraction(): any {
        this.model.schargePer = this.model.schargePer / 100;
        this.model.spDistPer = this.model.spDistPer / 100;
        this.model.distPer = this.model.distPer / 100;
        this.model.agentPer = this.model.agentPer / 100;
    }

    onDelete(event) {
        //if (this.featureCategoryModel && this.featureCategoryModel.id) {
        //    this.featureCategoryService.delete(this.featureCategoryModel).pipe(first())
        //        .subscribe(
        //            data => {
        //                console.log(data);
        //                this.router.navigateByUrl('../feature-category/worklist');
        //            },
        //            error => {
        //                console.log(error);
        //            });
        //}
    }

}
