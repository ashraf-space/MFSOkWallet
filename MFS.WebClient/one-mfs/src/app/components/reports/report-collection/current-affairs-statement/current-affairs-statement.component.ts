import { Component, OnInit } from '@angular/core';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';

@Component({
    selector: 'current-affairs-statement',
    templateUrl: './current-affairs-statement.component.html',
    styleUrls: ['./current-affairs-statement.component.css']
})
export class CurrentAffairsStatementComponent implements OnInit {
    model: any;
    constructor(private mfsUtilityService: MfsUtilityService) {
        this.model = {};
    }

    ngOnInit() {
    }
    getReportParam() {
        if (this.validate()) {
            var obj: any = {};
            obj.date = this.mfsUtilityService.renderDate(this.model.date, true);
            return obj;
        }
        else {
            var obj: any = {};
            obj.isNotValidated = true;
        }
    }

    validate(): any {
        if (!this.model.date) {
            return false;
        }
        else {
            return true;
        }
    }
}
