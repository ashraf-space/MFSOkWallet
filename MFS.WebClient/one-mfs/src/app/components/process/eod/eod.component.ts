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
    lastEodDateTime: any;
    constructor(private messageService: MessageService, private mfsUtilityService: MfsUtilityService,
        private authService: AuthenticationService, private processService: ProcessService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.todayDate = this.mfsUtilityService.getFullDateByMonthParam(0, 1);

        this.processService.GetLastEodDateTime().pipe(first())
            .subscribe(
                data => {
                    this.lastEodDateTime = data
                },
                error => {
                    console.log(error);
                }
            );
    }

    executeEOD(): any {
        this.isLoading = true;
        if (this.todayDate) {
            this.userName = this.currentUserModel.user.username;
            this.processService.executeEOD(this.mfsUtilityService.renderDate(this.todayDate, true), this.userName).pipe(first())
                .subscribe(
                    data => {
                        if (data.substring(0, 36) == 'EOD Process Was Successfully Done By')
                            this.messageService.add({ severity: 'success', summary: 'EOD successfully done', detail: data.toString() });
                        else if (data == 'EOD Process Was Already Successfully Done Till Today.')
                            this.messageService.add({ severity: 'success', summary: 'EOD already done', detail: data.toString() });
                        else
                            this.messageService.add({ severity: 'error', summary: 'Something wrong', detail: 'EOD is not successful' });

                        setTimeout(() => {
                            this.isLoading = false;
                            location.reload();
                        }, 20000);
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
