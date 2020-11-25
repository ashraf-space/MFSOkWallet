import { Directive, EventEmitter, ElementRef, HostListener, Output } from '@angular/core';

@Directive({
    selector: '[ngModel][appPascalcase]'
})
export class PascalcaseDirective {

    constructor() { }
    @Output() ngModelChange: EventEmitter<any> = new EventEmitter();
    value: any;
    word: any;
    @HostListener('input', ['$event']) onInputChange($event) {
        //this.word = $event.target.value;
        //this.value = this.word[0].toUpperCase() + this.word.substr(1).toLowerCase();
        this.value = $event.target.value;
        const arrStr = this.value.toLowerCase().split(' ');
        const word = arrStr.map((str) => (str.charAt(0).toUpperCase() + str.slice(1))).join(' ');

        this.ngModelChange.emit(word);
    }
}
