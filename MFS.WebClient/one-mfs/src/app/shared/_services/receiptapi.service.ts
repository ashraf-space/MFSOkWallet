import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ReceiptapiService {
    receiptUrl: any;
    constructor() {
        //for local
        //this.receiptUrl = "http://10.20.32.118/NEW/ok_api/receipt/view.php";

        //for Live
        this.receiptUrl = "http://10.156.4.68:8080/receipt/";
    }
}
