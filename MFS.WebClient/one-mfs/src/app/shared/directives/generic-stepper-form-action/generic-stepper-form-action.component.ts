import { Component, OnInit, EventEmitter, Output, Input  } from '@angular/core';

@Component({
    selector: 'app-generic-stepper-form-action',
    templateUrl: './generic-stepper-form-action.component.html',
    styleUrls: ['./generic-stepper-form-action.component.css']
})
export class GenericStepperFormActionComponent implements OnInit {

    @Input() activeIndex: number;
    @Input() maxIndex: number;
    @Input() disableAction: boolean;
    @Output() onStepAhead: EventEmitter<string> = new EventEmitter<string>();
    @Output() onStepBack: EventEmitter<string> = new EventEmitter<string>();
    @Input() isRejectAllowed: boolean;
    constructor() { }

    ngOnInit() {
    }

    onBack() {
        this.onStepBack.emit();
    }

    onNext(event) {
        this.onStepAhead.emit(event);
    }

    cancel() {
        window.history.back();
    }

}
