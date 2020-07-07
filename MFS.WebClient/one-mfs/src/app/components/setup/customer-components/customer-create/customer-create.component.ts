import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-customer-create',
  templateUrl: './customer-create.component.html',
    styleUrls: ['./customer-create.component.css'],
    encapsulation: ViewEncapsulation.None
})

export class CustomerCreateComponent implements OnInit {

    items: MenuItem[];
    genderTypes: any;
    religeonList: any;
    photoIdTypes: any;

    activeIndex: number = 0;

    constructor(private messageService: MessageService) { }

    ngOnInit() {
        this.items = [
        {
            label: 'Primary',
            command: (event: any) => {
                this.activeIndex = 0;
                this.messageService.add({ severity: 'success', summary: 'Service Message', detail: 'Via MessageService' });
            }
        },
        {
            label: 'Personal',
            command: (event: any) => {
                this.activeIndex = 1;
            }
        },
        {
            label: 'Nominee & Introducer',
            command: (event: any) => {
                this.activeIndex = 2;
                //this.messageService.add({ severity: 'info', summary: 'Pay with CC', detail: event.item.label });
            }
        },
        {
            label: 'Others',
            command: (event: any) => {
                this.activeIndex = 3;
                //this.messageService.add({ severity: 'info', summary: 'Last Step', detail: event.item.label });
            }
        }
        ];
        this.genderTypes = [
            { label: 'Male', value: 'Male', icon: 'fas fa-male' },
            { label: 'Female', value: 'Female', icon: 'fas fa-female' },
            { label: 'Others', value: 'Others', icon: 'fas fa-transgender-alt' }
        ];
        this.photoIdTypes = [
            { label: 'NID', value: 'NID', icon: 'far fa-id-card' },
            { label: 'Passport', value: 'Passport', icon: 'fas fa-passport' },
            { label: 'Birth Registration', value: 'Birth Registration', icon: 'fas fa-id-card-alt' }
        ];
        this.religeonList = [
            { label: 'Islam', value: 'Islam' },
            { label: 'Hinduism', value: 'Hinduism' },
            { label: 'Chritianity', value: 'Chritianity' }
        ]
    }

    saveCustomer() {
        console.log('data saved')
        this.activeIndex =0;
    }

    onStepAhead() {
        if (this.activeIndex < 3) {
            this.activeIndex++;            
        }
        else {
            this.saveCustomer();
        }
    }

    onStepBack() {
        if (this.activeIndex > 0) {
            this.activeIndex--;
        }
    }

    cancel() {
        window.history.back();
    }

}
