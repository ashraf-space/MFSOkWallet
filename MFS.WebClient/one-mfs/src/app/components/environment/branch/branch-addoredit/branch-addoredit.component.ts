import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BankBranchService } from '../../../../services/environment';
import { first } from 'rxjs/operators';
import { MessageService, Message } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
    selector: 'app-branch-addoredit',
    templateUrl: './branch-addoredit.component.html',
    styleUrls: ['./branch-addoredit.component.css']
})
export class BranchAddoreditComponent implements OnInit {
    currentUserModel: any = {};
    entityId: string;
    isEditMode: boolean = false;

    bankBranchModel: any = {};

    error: boolean = false;

    constructor(private messageService: MessageService, private bankBranchService: BankBranchService, private router: Router, private route: ActivatedRoute
        , private authService: AuthenticationService) {
        this.authService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
    }

    msgs: Message[] = [];

    ngOnInit() {
        this.entityId = this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getBankBranchById();
        }
    }
    getBankBranchById(): any {
        this.bankBranchService.getBankBranchById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.bankBranchModel = data;
                },
                error => {
                    console.log(error);
                });

    };


    onBankBranchSave() {
      
        if (this.isEditMode) {
            this.bankBranchModel.updateBy = this.currentUserModel.user.username;
        }
        else {
            this.bankBranchModel.entryBy = this.currentUserModel.user.username;
        }

        if (!this.bankBranchModel.branchcode || !this.bankBranchModel.branchname) {
            this.msgs = [];
            this.msgs.push({ severity: 'error', summary: 'Warning! ', detail: 'Cannot be left blank' });
            this.error = true;
        }
        else {

            this.bankBranchService.save(this.bankBranchModel, this.isEditMode).pipe(first())
                .subscribe(
                    data => {
                        
                        if (this.isEditMode)
                            this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Bank branch updated' });
                        else
                            this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Bank branch added' });

                        window.history.back();
                    },
                    error => {
                        console.log(error);
                    });


        }
        //if (this.bankBranchModel.branchcode != "" || this.bankBranchModel.branchname != "") {
        //    this.bankBranchService.save(this.bankBranchModel).pipe(first())
        //        .subscribe(
        //            data => {
        //                window.history.back();
        //                if (this.isEditMode)
        //                    this.messageService.add({ severity: 'success', summary: 'Update successfully', detail: 'Bank branch updated' });
        //                else
        //                    this.messageService.add({ severity: 'success', summary: 'Save successfully', detail: 'Bank branch added' });
        //            },
        //            error => {
        //                console.log(error);
        //            });

        //}
    }

    cancel() {
        window.history.back();
    }

    onBranchDelete(event) {

    }


}
