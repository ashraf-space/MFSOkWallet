import { Component, OnInit } from '@angular/core';
import { MessageService, Message } from 'primeng/api';
import { MfsUtilityService } from 'src/app/services/mfs-utility.service';
import { AuthenticationService } from 'src/app/shared/_services';
import { TransactionMasterComponent } from '../../transaction/transaction-master/transaction-master.component';
import { ProcessService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-eod',
    templateUrl: './eod.component.html',
    styleUrls: ['./eod.component.css']
})
export class EodComponent implements OnInit {
    isLoading: boolean = false;
    msgs: Message[] = [];
    todayDate: any = {};
    userName: string = null;
    currentUserModel: any;
    error: any;
    constructor(private messageService: MessageService, private mfsUtilityService: MfsUtilityService,
        private authService: AuthenticationService, private processService: ProcessService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.todayDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);

    }

    executeEOD(): any {
        this.isLoading = true;
        if (this.todayDate) {
            this.userName = this.currentUserModel.user.username;
            this.processService.executeEOD(this.mfsUtilityService.renderDate(this.todayDate, true), this.userName).pipe(first())
                .subscribe(
                    data => {
                        if (data == 'SUCCESS')
                            this.messageService.add({ severity: 'success', summary: 'EOD successfully', detail: 'EOD is done successfully' });
                        else
                            this.messageService.add({ severity: 'error', summary: 'Something wrong', detail: 'EOD is not successful' });

                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 100);
                    },
                    error => {
                        console.log(error);
                    });
        }
        else {
            this.messageService.add({ severity: 'error', summary: 'Date is empty' });
        }

    }

}
