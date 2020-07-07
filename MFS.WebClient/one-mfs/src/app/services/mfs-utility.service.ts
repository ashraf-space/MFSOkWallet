import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MfsUtilityService {
    
    searchModelOptionList: any;
    searchModelCriteriaList: any;
    mTypeList: any;
    dateList: any;
    actionList: any;
    parentMenuList: any;
    constructor() {
        this.searchModelOptionList = [
            { label: 'Any A/C', value: 'RegInfo' },
            { label: 'Customer', value: 'C' },
            { label: 'Distributor', value: 'D' },
            { label: 'Agent', value: 'A' },
            { label: 'Merchant', value: 'M' },
            { label: 'DSR', value: 'R' },
            { label: 'Transaction', value: 'Transaction' },
            { label: 'Message', value: 'Message' }
        ];

        this.searchModelCriteriaList = [
            { label: 'Phone', value: 'Mphone' },
            { label: 'Name', value: 'Name' },
            { label: 'Photo Id', value: 'PhotoId' },
            { label: 'Parent Phone', value: 'Pmphone' },
            { label: 'Transaction Number', value: 'TransNo' },
            { label: 'Message Body', value: 'OutMsg' }
        ];

        this.mTypeList = [
            { label: 'Aggregator or Super Merchant', value: 'Aggregator or Super Merchant' },
            { label: 'Airlines or Bus Ticket', value: 'Airlines or Bus Ticket' },
            { label: 'Baby, Kids, Toys', value: 'Baby, Kids, Toys' },
            { label: 'Bakery, Pastry', value: 'Bakery, Pastry' },
            { label: 'Beauty Care', value: 'Beauty Care' },
            { label: 'Books Gifts', value: 'Books Gifts' },
            { label: 'Charity, Donation', value: 'Charity, Donation' },
            { label: 'Clothes, Boutique house', value: 'Clothes, Boutique house' },
            { label: 'Computing, Gaming', value: 'Computing, Gaming' },
            { label: 'eCommerce Store', value: 'eCommerce Store' },
            { label: 'Educational Institute', value: 'Educational Institute' },
            { label: 'Electronics', value: 'Electronics' },
            { label: 'Entertainment (Movie, Music)', value: 'Entertainment (Movie, Music)' },
            { label: 'Food, Restaurants', value: 'Food, Restaurants' },
            { label: 'Foot Ware', value: 'Foot Ware' },
            { label: 'General Store', value: 'General Store' },
            { label: 'Grocery shop', value: 'Grocery shop' },
            { label: 'Health, Fitness', value: 'Health, Fitness' },
            { label: 'Home Appliance', value: 'Home Appliance' },
            { label: 'Home Decoration', value: 'Home Decoration' },
            { label: 'Hospital, Diagnostics', value: 'Hospital, Diagnostics' },
            { label: 'Hotel, Resort', value: 'Hotel, Resort' },
            { label: 'Lotary Ticket', value: 'Lotary Ticket' },
            { label: 'Medicine, Pharmacy', value: 'Medicine, Pharmacy' },
            { label: 'Others', value: 'Others' },
            { label: 'Ride Sharing, Rent-a Car', value: 'Ride Sharing, Rent-a Car' },
            { label: 'Small Shop (Verities)', value: 'Small Shop (Verities)' },
            { label: 'Sports, Gym', value: 'Sports, Gym' },
            { label: 'Super Store', value: 'Super Store' },
            { label: 'Tours, Travels', value: 'Tours, Travels' },
            { label: 'Utility, Service', value: 'Utility, Service' },
            { label: 'Vehicles, Auto Mobiles', value: 'Vehicles, Auto Mobiles' }
        ];

        this.dateList = [
            { label: 'Saturday', value: 'sat' },
            { label: 'SunDay', value: 'sun' },
            { label: 'Monday', value: 'mon' },
            { label: 'Tuesday', value: 'tue' },
            { label: 'Wednesday', value: 'wed' },
            { label: 'Thursday', value: 'thu' },         
            { label: 'Friday', value: 'fri' }            
        ];
        this.actionList = [
            { label: 'ADD', value: 'ADD' },
            { label: 'VISIT', value: 'VISIT' },
            { label: 'SEARCH', value: 'SEARCH' },
            { label: 'CHANGE', value: 'CHANGE' }
        ];
        this.parentMenuList = [
            { label: 'Dashboard', value: 'Dashboard' },
            { label: 'Client', value: 'Client' },
            { label: 'Distribution', value: 'Distribution' },
            { label: 'Enviornment', value: 'Enviornment' },
            { label: 'Reports', value: 'Reports' },
            { label: 'Merchant', value: 'Merchant' },
            { label: 'Settings', value: 'Settings' },
            { label: 'Tools', value: 'Tools' },
            { label: 'Transaction', value: 'Transaction' },
            { label: 'Process', value: 'Process' }
            
        ];
    }

    addIndexing(list) {
        let index = 1;
        list.forEach(obj => {
            obj.index = index;
            index++;
        });

        return list; 
    }

    getCustomerTypeList() {
        return ['Distributor', 'DSR', 'Agent', 'Customer', 'Merchant', 'GP - Mobicash', 'GP - Mobicash Agent'];
    }
    //save mode
    renderDate(date: any, formatDate: boolean = false): any {
        if (!formatDate) {
            var res = new Date(Date.UTC(date.year, date.month - 1, date.day));
            return res;
        }
        else {
            return this.GetFormattedDate(date);
        }
    }
    //update mode
    renderDateObject(date: any): any {
        var splitDate = date.split('T')[0];
        var dateSplit = [];
        var dateObject: any = {};
        dateSplit = splitDate.split('-');
        dateObject.year = +dateSplit[0];
        dateObject.month = +dateSplit[1];
        dateObject.day = +dateSplit[2];
        return dateObject;
    }

    GetFormattedDate(date: any) {        
        //if (date.month < 12) {
        //    date.month = date.month + 1;
        //}

        return date.year + "/" +  date.month + "/" + date.day;
    };

    getFullDateByMonthParam(dateParam: any = 0, monthParam: any = 1): any {
        //param value set to 1 for getting the current month
        var x = new Date();
        if (dateParam != 0) { x.setDate(dateParam); }
        //x.setMonth(x.getMonth() + monthParam);        
        var date = x.getFullYear() + '-' + (x.getMonth() + monthParam) + '-' + x.getDate();
        return this.renderDateObject(date);
    };

    getPositiveWholeNumberRegExp(): RegExp {
        return /[0-9]$/;
    }

    getMobileNoRegExp(): RegExp {
        return /^(?:\+88|01)?(?:\d{11}|\d{13})$/;
    }
    checkPhotoIdLength(photoId: any, photoIdTypeCode: any): any {
        if (photoId.length && photoId.length != 0) {
            switch (+photoIdTypeCode) {
                case 1:
                    if (photoId.length == 10 || photoId.length == 13) {
                        return false;
                    }
                    else {
                        return true;
                    }
                    break;
                default:
            }
        }
    }

    //global search iterenies
    getSearchModelOptionParams() {
        return this.searchModelOptionList;
    }

    getSearchModelCriteriaParams(option) {
        var list = [];
        switch (option) {
            case 'Transaction':
                list.push(this.searchModelCriteriaList[0]);
                list.push(this.searchModelCriteriaList[4]);
                return list;
                break;
            case 'Message':
                list.push(this.searchModelCriteriaList[0]);
                list.push(this.searchModelCriteriaList[5]);
                return list;
                break;
            default:
                for (var i = 0; i < 4; i++) {
                    list.push(this.searchModelCriteriaList[i]);
                }
                return list;
                break;
        }
    }

    getMTypelIst(): any {
        return this.mTypeList;
    }

    getDateList(): any {
        return this.dateList;
    }
    getActionList(): any {
        return this.actionList;
    }
    getParentMenuList(): any {
        return this.parentMenuList;
    }
}
