import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { ConfirmationService, Message } from 'primeng/api';

@Component({
    selector: 'app-generic-form-action',
    templateUrl: './generic-form-action.component.html',
    styleUrls: ['./generic-form-action.component.css'],
    providers: [ConfirmationService]
})
export class GenericFormActionComponent implements OnInit {
    @Input() isEditMode: boolean;
    @Input() isPassToMakerAllowed: boolean;    
    @Input() isRejectAllowed: boolean;
    @Input() isDeleteAllowed: boolean;
    @Input() disableAction: boolean;
    @Output() onSave: EventEmitter<string> = new EventEmitter<string>();
    @Output() onDelete: EventEmitter<string> = new EventEmitter<string>();

    msgs: Message[] = [];

    constructor(private confirmationService: ConfirmationService) { }

    ngOnInit() {        
    }

    onSaveAction(event) {
        //this.onSave.emit(event);

        this.confirmationService.confirm({
            message: 'Are you sure that you want to proceed?',
            header: 'Confirmation',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.onSave.emit(event);
            },
            reject: () => {
                this.msgs = [{ severity: 'info', summary: 'Rejected', detail: 'You have rejected' }];
            }
        });

    }

    onDeleteAction() {
        this.confirmationService.confirm({
            message: 'Warning! Are you sure that you want to Delete this item?',
            accept: () => {
                this.onDelete.emit('delete');
            }
        });
    }

    cancel() {
        window.history.back();
    }
    

}
