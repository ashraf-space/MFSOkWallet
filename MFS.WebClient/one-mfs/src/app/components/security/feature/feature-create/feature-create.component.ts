import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FeatureCategoryService, FeatureService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Message } from 'primeng/components/common/api';

@Component({
  selector: 'app-feature-create',
  templateUrl: './feature-create.component.html',
  styleUrls: ['./feature-create.component.css']
})
export class FeatureCreateComponent implements OnInit {

    featureCategoryList: any;
    featureModel: any = {};
    isEditMode: boolean = false;
    entityId: number;
    error: boolean = false;

    constructor(private featureCategoryService: FeatureCategoryService, private featureService: FeatureService, private router: Router, private route: ActivatedRoute, private messageService: MessageService) { }

    msgs: Message[] = [];

    ngOnInit() {
        this.getFeatureCategoryDDL();
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getFeatureById();
        }
    }
    getFeatureById(): any {
        this.featureService.getFeatureById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.featureModel = data;
                    this.featureModel.isRegistrationAllowed = this.featureModel.isRegistrationAllowed == 'Y' ? 1 : 0;
                },
                error => {
                    console.log(error);
                });
    }

    async getFeatureCategoryDDL() {
        this.featureCategoryService.getFeatureCategoryListDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.featureCategoryList = data;
                },
                error => {
                    console.log(error);
                });
    }

    onSaveFeature(event) {
        if (!this.featureModel.categoryId || !this.featureModel.alias || !this.featureModel.url) {            
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            this.error = false;
            this.msgs = [];
            this.featureModel.isRegistrationAllowed = this.featureModel.isRegistrationAllowed == true ? 'Y' : 'N';
            this.featureService.save(this.featureModel).pipe(first())
                .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.featureModel.alias + ' feature saved Successfully' });
                        console.log(data);
                        window.history.back();
                    },
                error => {
                    this.messageService.add({ severity: 'error', summary: 'Failed', detail: this.featureModel.alias + ' feature could not be added' });
                        console.log(error);
                    });
        }
    }

    onDeleteFeature(event) {
        if (this.featureModel && this.featureModel.id) {
            this.featureService.delete(this.featureModel).pipe(first())
                .subscribe(
                    data => {
                        console.log(data);
                        this.router.navigateByUrl('../feature/worklist');
                    },
                    error => {
                        console.log(error);
                    });
        }
    }

}
