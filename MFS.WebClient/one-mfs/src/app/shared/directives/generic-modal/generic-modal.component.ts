import { Component, OnInit, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-generic-modal',
  templateUrl: './generic-modal.component.html',
  styleUrls: ['./generic-modal.component.css']
})
export class GenericModalComponent implements OnInit {

    @Input() initiateModal: boolean;
    @Input() header: string;
    @Input() disableConfirm: boolean;
    @Input() hideConfirm: boolean;
    @Input() hideCancel: boolean;

    @Output() onConfirm: EventEmitter<string> = new EventEmitter<string>();
    @Output() onDestroy: EventEmitter<string> = new EventEmitter<string>();

    constructor() { }

    ngOnInit() {
        
    }

    onConfirmClick() {
        this.onConfirm.emit('');
    }

    onDestroyClick() {
        this.onDestroy.emit('');
    }

}
