import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReceiptapiService {
    receiptUrl: any;
    thermalReceiptUrl: any;
    constructor() {
        //for local
        //this.receiptUrl = "http://10.20.34.35/NEW/ok_api/receipt/view.php";
        //this.thermalReceiptUrl = "http://10.20.34.35/NEW/ok_api/receipt/indexT.php";

        //for Live
        this.receiptUrl = "http://10.156.4.68:8080/receipt/";
        this.thermalReceiptUrl = "http://10.156.4.68:8080/receipt/indexT.php";
    }
}
