import { Component, OnInit } from '@angular/core';
import { FeatureCategoryService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';

@Component({
  selector: 'app-feature-category-list',
  templateUrl: './feature-category-list.component.html',
  styleUrls: ['./feature-category-list.component.css']
})
export class FeatureCategoryListComponent implements OnInit {
    gridConfig: any;      

    constructor(private featureCategoryService: FeatureCategoryService, private gridSettingService: GridSettingService) {        
        this.gridConfig = {};
    }

      ngOnInit() {
        this.initialiseGridConfig();
      }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = 'Feature Category List';
        this.gridConfig.gridIconClass = 'fas fa-list';
        this.gridConfig.createStateUrl = '/feature-category/create/';
        this.gridConfig.hasEditState = true;
        this.gridConfig.entityField = 'id';

        this.gridConfig.columnList = [
            { field: 'name', header: 'Category Name', width: '42%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'icon', header: 'Icon', width: '98%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'orderNo', header: 'Order Number', width: '60%', filter: this.gridSettingService.getDefaultNumberFilterable(0, 10) },
            { field: 'id', header: 'Edit', width: '10%', isEditColumn: true, filter: this.gridSettingService.getFilterableNone() }
        ];

        this.featureCategoryService.getFeatureCategoryWorklist().pipe()
            .subscribe(data => {                
                this.gridConfig.dataSource = data;                    
            })
    };


}
