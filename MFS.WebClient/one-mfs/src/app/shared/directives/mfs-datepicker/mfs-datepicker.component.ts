import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { MfsUtilityService } from '../../../services/mfs-utility.service';

@Component({
  selector: 'app-mfs-datepicker',
  templateUrl: './mfs-datepicker.component.html',
  styleUrls: ['./mfs-datepicker.component.css']
})
export class MfsDatepickerComponent implements OnInit {
    @Input() dateModel: any;
    @Output() onSelect: EventEmitter<any> = new EventEmitter<any>();

    constructor(private mfsUtilityService: MfsUtilityService) { }

    ngOnInit() {
        if (this.dateModel) {
            console.log(this.dateModel);
            this.dateModel = this.mfsUtilityService.renderDateObject(this.dateModel);
            
        }
      }

    onChange(event) {
        this.onSelect.emit(this.mfsUtilityService.renderDate(event));
        console.log(event);
    }

}
