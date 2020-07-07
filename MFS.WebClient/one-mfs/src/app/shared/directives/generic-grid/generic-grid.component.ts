import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from 'src/app/services/grid-setting.service';
import { MfsUtilityService } from '../../../services/mfs-utility.service';
import { MessageService } from 'primeng/api';
import { Footer } from 'primeng/components/common/shared';
declare let jsPDF: any;

@Component({
  selector: 'generic-grid',
  templateUrl: './generic-grid.component.html',
    styleUrls: ['./generic-grid.component.css'],
    encapsulation: ViewEncapsulation.None
})

export class GenericGridComponent implements OnInit {
    @Input() gridConfig: any;
    filterConfig: any;
    numberFilter: number = 0;
    dataSourcePath: string;
    isLoading: boolean = false;   
    itemsToEdit: any = [];

    @Output() customAction: EventEmitter<any> = new EventEmitter<any>();
    @Output() batchUpdateCallback: EventEmitter<any> = new EventEmitter<any>();
    
    constructor(private router: Router, private gridSettingService: GridSettingService, private mfsUtilityService: MfsUtilityService, private messageService: MessageService) {        
        this.filterConfig = {};
    }

    ngOnInit() {
        this.initializeGridConfig();
    }
    //ngOnChanges() {
    //    if (this.gridConfig.callApifromChild && this.gridConfig.dataSourcePath != null) {
    //        this.updateDataSource()
    //    }
    //}
    initializeGridConfig() {
        this.gridConfig.paginator = this.gridConfig.paginator != null ? this.gridConfig.paginator : true;
        this.gridConfig.row = this.gridConfig.row != null ? this.gridConfig.row : 50;
        this.gridConfig.showExport = this.gridConfig.showExport != null ? this.gridConfig.showExport : true;       
        this.gridConfig.scrollable = this.gridConfig.scrollable != null ? this.gridConfig.scrollable : true;  
        this.gridConfig.scrollHeight = this.gridConfig.scrollHeight != null ? this.gridConfig.scrollHeight : 'calc(61vh - (89px + 1.25em))';
        this.gridConfig.resizableColumns = this.gridConfig.resizableColumns != null ? this.gridConfig.resizableColumns : true;
        this.gridConfig.showUniversalFilter = this.gridConfig.showUniversalFilter != null ? this.gridConfig.showUniversalFilter : true;
        this.gridConfig.showCaption = this.gridConfig.showCaption != null ? this.gridConfig.showCaption : true;
        this.gridConfig.showPaginator = this.gridConfig.showPaginator != null ? this.gridConfig.showPaginator : true;
        this.gridConfig.reorderableColumns = this.gridConfig.reorderableColumns != null ? this.gridConfig.reorderableColumns : true;
        this.gridConfig.columnLength = this.gridConfig.columnList.length;
        

        if (this.gridConfig.autoUpdateDataSource && this.gridConfig.dataSourcePath != null) {
            this.updateDataSource();
        }

        if (this.gridConfig.customFilterOptionPath != null) {
            this.getCustomFilterOptionList();
            this.dataSourcePath = this.gridConfig.dataSourcePath;
            this.gridConfig.dataSourcePath = this.dataSourcePath + this.gridConfig.selectedCustomFilter;
        }

        if (this.gridConfig.customFilterOptionList && this.gridConfig.customFilterOptionList.length != 0) {
            this.dataSourcePath = this.gridConfig.dataSourcePath;
            this.gridConfig.dataSourcePath = this.dataSourcePath + this.gridConfig.selectedCustomFilter;
        }
        
    }

    async getCustomFilterOptionList(){
        this.gridSettingService.getCustomFilterDDL(this.gridConfig.customFilterOptionPath).pipe()
            .subscribe(data => {
                this.gridConfig.customFilterOptionList = data;
            });
    }

    onCustomFilterChange() {
        this.gridConfig.dataSourcePath = this.dataSourcePath + this.gridConfig.selectedCustomFilter;
        if (this.gridConfig.dataSourcePath != null) {
            this.updateDataSource();
        }
    }

    getDatafromDataSource() {
        if (this.gridConfig.dataSource) {
            return this.gridConfig.dataSource;
        }
    }

    pushDataInDataSource(item) {
        if (this.gridConfig.dataSource) {
            if (this.gridConfig.autoIndexing) {
                item.index = this.gridConfig.dataSource.length + 1;
            }

            this.gridConfig.dataSource.push(item);
        }
    }
    
    updateDataSource(): any {
        this.isLoading = true;
        try {
            this.gridSettingService.getWorklistForGridDataSource(this.gridConfig.dataSourcePath).pipe()
                .subscribe(data => {
                    if (data) {
                        if (this.gridConfig.autoIndexing) {
                            this.gridConfig.dataSource = this.mfsUtilityService.addIndexing(data);
                        }
                        else {
                            this.gridConfig.dataSource = data;
                        }
                        if (this.gridConfig.footerTemplate) {
                            this.setFooterTemplate();
                        }
                        this.isLoading = false;
                    }
                    this.isLoading = false;
                });
        }
        catch (error) {
            this.isLoading = false;
            console.log(error);
        }
        
    }

    clearDataSource() {
        this.isLoading = true;
        if (this.gridConfig.dataSource) {
            this.gridConfig.dataSource = [];
        }
        this.isLoading = false;
    }

    setFooterTemplate(): any {        
        Object.keys(this.gridConfig.footerTemplate.footerRow).forEach(footerObj => {         
            this.gridConfig.footerTemplate.footerRow[footerObj] = 0;
            this.gridConfig.dataSource.forEach(dataObj => {
                Object.keys(dataObj).forEach(colObj => {
                    if (colObj == footerObj) {
                        this.gridConfig.footerTemplate.footerRow[footerObj] = this.gridConfig.footerTemplate.footerRow[footerObj] + dataObj[colObj];
                    }
                })
            });            
        });
        this.gridConfig.footerTemplate.footerRow[this.gridConfig.columnList[this.gridConfig.footerTemplate.captionIndex].field] = "Total";
        this.pushDataInDataSource(this.gridConfig.footerTemplate.footerRow);
    };

    saveBatchUpdate(obj: any): any {
        this.gridSettingService.saveBatchUpdate(this.gridConfig.batchUpdatePath, obj).pipe()
            .subscribe(data => {
                this.updateDataSource();
                this.itemsToEdit = [];
                this.messageService.add({ severity: 'success', summary: 'Batch Update Successful', detail: data.toString() });
            });
    }

    onFilterClick() {
        console.log();
    }

    create(event) {        
        this.router.navigateByUrl(this.gridConfig.createStateUrl);
    }

    onEdit(editParam) {
        this.router.navigateByUrl(this.gridConfig.createStateUrl + editParam);
    }

    onDetails(detailsParam) {
        this.router.navigateByUrl(this.gridConfig.detailsStateUrl + detailsParam);
    }

    onCustomAction(row) {
        this.customAction.emit(row);
    }

    toggleAll(column, event) {
        this.gridConfig.dataSource.forEach(x => {
            this.handleChange(x, column, event);
        });

        this.itemsToEdit.forEach(obj => {
            obj = this.convertBooleanToYN(obj);
        });
    }

    handleChange(data, column, event) {
        let editObject: any = {
            data: {}, column: [], event: []
        };

        editObject.data = data;        
        editObject.column[0] = column;
        editObject.event[0] = event;

        if (this.gridConfig.isBatchSwitchBoxEdit) {
            this.updateSelectedItemList(editObject);
        }
        else if (this.gridConfig.switchBoxToggleSavePath != null) {
            editObject = this.convertBooleanToYN(editObject);
        }
    }

    async updateSelectedItemList(editObject: any) {
        let existingItem: boolean = false;
        if (this.itemsToEdit.length != 0 && this.itemsToEdit[0].data.index) {
            this.itemsToEdit.forEach(obj => {
                if (obj.data.index == editObject.data.index) {
                    existingItem = true;
                    obj.data = editObject.data;
                    obj.column.push(editObject.column[0]);
                    obj.event.push(editObject.event[0])
                }                
            });
            if (!existingItem) { this.itemsToEdit.push(editObject); }
        }
        else {
            this.itemsToEdit.push(editObject);
        }
    }

    onBatchSwitchBoxEditSave() {
        let listItems: any = [];
        this.itemsToEdit.forEach(e => {
            listItems.push(this.convertBooleanToYN(e));
        });

        let obj: any = {};

        obj.list = listItems;

        if (this.gridConfig.paramForBatchUpdate) {
            obj.param = this.gridConfig.paramForBatchUpdate;
        }

        if (this.gridConfig.batchUpdatePath) {
            this.saveBatchUpdate(obj);
        }
        else {
            this.itemsToEdit = [];
            this.batchUpdateCallback.emit(obj);
        }
    }

    convertBooleanToYN(obj: any) {
        if (this.gridConfig.isBatchSwitchBoxEdit) {
            Object.keys(obj.data).forEach(e => {
                obj.column.forEach(function (data, index) {
                    if (e == data.field) {
                        if (obj.event[index].checked) {
                            obj.data[e] = 1;
                        }
                        else {
                            obj.data[e] = 0;
                        }
                        if (obj.event[index].value) {
                            obj.data[e] = obj.event[index].value;
                        }                        
                    }
                })

            });
        }
        else {
            Object.keys(obj.data).forEach(e => {
                if (e == obj.column.field) {
                    if (obj.event.checked) {
                        obj.data[e] = 1;
                    }
                    else {
                        obj.data[e] = 0;
                    }
                }
            });
        }

        return obj.data;
    }

    exportToPDF() {
        // this will work if JSPDF module works. Just Comment out the html portion 
        const doc = new jsPDF();
        doc.autoTable(this.gridConfig.columnList, this.gridConfig.dataSource);
        doc.save(this.gridConfig.gridName + '.pdf');
    }

}
