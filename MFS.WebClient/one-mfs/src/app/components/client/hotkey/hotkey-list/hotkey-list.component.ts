import { Component, OnInit, Input } from '@angular/core';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { GridSettingService } from '../../../../services/grid-setting.service';
import { MfsUtilityService } from '../../../../services/mfs-utility.service';
import { MfsSettingService } from '../../../../services/mfs-setting.service';

@Component({
  selector: 'app-hotkey-list',
  templateUrl: './hotkey-list.component.html',
  styleUrls: ['./hotkey-list.component.css']
})
export class HotkeyListComponent implements OnInit {

    @Input() showCaption: boolean;
    gridConfig: any;

    constructor(private gridSettingService: GridSettingService, private mfsSettingService: MfsSettingService) {
        this.gridConfig = {};
    }

    ngOnInit() {
        this.initialiseGridConfig();
    }

    initialiseGridConfig(): any {
        this.gridConfig.dataSource = [];
        this.gridConfig.gridName = ' Hot Key';
        this.gridConfig.gridIconClass = 'fab fa-keycdn';

        this.gridConfig.dataSourcePath = this.mfsSettingService.clientApiServer + '/Hotkey/GetHotkeyList'; //-- FOR DATA SOURCE PATH 

        this.gridConfig.autoUpdateDataSource = true; // --- FOR AUTO BINDING THE DATA AFTER DATA RETRIVAL FROM THE API SERVER        

        if (this.showCaption != null) {
            this.gridConfig.showCaption = this.showCaption;
        }

        this.gridConfig.columnList = [            
            { field: 'hotkey', header: 'Hot Key', width: '10%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'hotkeyDesc', header: 'Description', width: '40%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'sampleMsg', header: 'Sample Message', width: '45%', filter: this.gridSettingService.getDefaultFilterable() },
            { field: 'transBlockTimeStatus', header: 'Transaction Block Time Status', width: '15%', filter: this.gridSettingService.getFilterableNone() },
            { field: 'example', header: 'Example', width: '45%', filter: this.gridSettingService.getDefaultFilterable() }            
        ];

    };
}
