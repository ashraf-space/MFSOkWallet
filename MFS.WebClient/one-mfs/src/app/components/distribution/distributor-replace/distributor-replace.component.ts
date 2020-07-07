import { Component, OnInit } from '@angular/core';
import { DistributorService } from 'src/app/services/distribution';
import { AuthenticationService } from 'src/app/shared/_services';
import { MessageService, Message } from 'primeng/api';
import { first } from 'rxjs/operators';

@Component({
    selector: 'app-distributor-replace',
    templateUrl: './distributor-replace.component.html',
    styleUrls: ['./distributor-replace.component.css']
})
export class DistributorReplaceComponent implements OnInit {
    distributorReplaceModel: any = {};
    currentUserModel: any = {};
    distributorList: any = {};
    isLoading: boolean = false;
    totalAgent: number = 0;
    msgs: Message[] = [];
    error: boolean = false;

    constructor(private distributorService: DistributorService, private authenticationService: AuthenticationService, private messageService: MessageService) {
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    ngOnInit() {
        this.getDistributorList();
    }
    getDistributorList(): any {
        this.isLoading = true;
        this.distributorService.getDistributorListForDDL()
            .pipe(first())
            .subscribe(
                data => {
                    this.isLoading = false;
                    this.distributorList = data;
                },
                error => {
                    console.log(error);
                }
            );
    }

    getTotalAgentByMobileNo(selectedExDistributor) {
        if (this.distributorReplaceModel.exMobileNo) {
            this.isLoading = true;
            this.distributorService.getTotalAgentByMobileNo(this.distributorReplaceModel.exMobileNo)
                .pipe(first())
                .subscribe(
                    data => {
                        this.isLoading = false;
                        if (data == null) {
                            this.totalAgent = 0;
                        }
                        else {
                            this.totalAgent = data.TOTALAGENT;
                        }


                    });
        }
        else {
            this.messageService.add({ severity: 'warn', summary: 'Can not be null', detail: 'Please select a distributor.' });
            this.distributorReplaceModel.exMobileNo = null;
            this.totalAgent = 0;
        }
    }

    ExecuteReplace(): any {
        if (!this.distributorReplaceModel.exMobileNo || this.distributorReplaceModel.exMobileNo == '' || this.distributorReplaceModel.exMobileNo == '0' ||
            !this.distributorReplaceModel.newMobileNo || this.distributorReplaceModel.newMobileNo == '' || this.distributorReplaceModel.newMobileNo == '0') {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {
            if (this.distributorReplaceModel.exMobileNo != "" || this.distributorReplaceModel.newMobileNo != "") {
                this.isLoading = true;
                //this.distributorReplaceModel.isRegistrationAllowed = this.distributorReplaceModel.isRegistrationAllowed == true ? 'Y' : 'N';
                this.distributorReplaceModel.isWithDSR = this.distributorReplaceModel.isWithDSR == true ? 'Y' : 'N';
                this.distributorService.ExecuteReplace(this.distributorReplaceModel).pipe(first())
                    .subscribe(
                        data => {
                            this.isLoading = false;
                            if (data == 1) {
                                this.messageService.add({ severity: 'success', summary: 'Replace successfully', detail: 'Distributor replacement done' });
                            }
                            else {
                                this.messageService.add({ severity: 'error', summary: 'Not replacement', detail: data });
                            }
                            
                            //console.log(data);
                            //setTimeout(() => {
                                this.isLoading = false;
                                location.reload();
                            //}, 100);
                        },
                        error => {
                            console.log(error);
                        });

            }
        }

    }

}
