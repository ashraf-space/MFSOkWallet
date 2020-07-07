import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FeatureCategoryService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-feature-category-create',
  templateUrl: './feature-category-create.component.html',
    styleUrls: ['./feature-category-create.component.css']
})
export class FeatureCategoryCreateComponent implements OnInit {

    entityId: number;
    isEditMode: boolean = false;
    
    featureCategoryModel: any = {};

    constructor(private featureCategoryService: FeatureCategoryService, private router: Router, private route: ActivatedRoute, private messageService: MessageService) { }

    ngOnInit() {    
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getFeatureCategoryById();
        }
    }
    getFeatureCategoryById(): any {
        this.featureCategoryService.getFeatureCategoryById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.featureCategoryModel = data;
                },
                error => {
                    console.log(error);
                });

    };
    

    onSave() {
        if (this.featureCategoryModel.name != "") {
            this.featureCategoryService.save(this.featureCategoryModel).pipe(first())
                .subscribe(
                data => {
                    this.messageService.add({ severity: 'success', summary: 'Success', detail: this.featureCategoryModel.name + ' category saved Successfully' });
                    console.log(data);
                    window.history.back();
                    },
                    error => {
                        console.log(error);
                    });
        }
    }

    onDeleteCategory(event) {
        if (this.featureCategoryModel && this.featureCategoryModel.id) {
            this.featureCategoryService.delete(this.featureCategoryModel).pipe(first())
                .subscribe(
                    data => {
                        console.log(data);
                        this.router.navigateByUrl('../feature-category/worklist');
                    },
                    error => {
                        console.log(error);
                    });
        }
    }
}
