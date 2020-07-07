import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
    name: 'statusCheck'
})
export class StatusCheckPipe implements PipeTransform {
    constructor(private _sanitizer: DomSanitizer) { }

    transform(value: any, args: any): any {
        switch (args) {
            case 'status':
                switch (value) {
                    case 'A':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Active </span>");
                        break;
                    case 'I':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Inactive </span>");
                        break;
                    case 'D':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#96961e;color:white;padding:0.5em'> Dormant </span>");
                        break;
                    case 'C':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Close </span>");
                        break;
                    default:
                        return value;
                }
                break;
            case 'regStatus':
                switch (value) {
                    case 'L':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#96961e;color:white;padding:0.5em'> Logical </span>");
                        break;
                    case 'P':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Physical  </span>");
                        break;
                    case 'R':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Reject </span>");
                        break;
                    default:
                        return value;
                }
                break;
            case 'pinStatus':
                switch (value) {
                    case 'Y':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Verified </span>");;
                        break;
                    case 'N':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#96961e;color:white;padding:0.5em'> Not verified </span>");;
                        break;
                    case 'L':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Locked </span>");;
                        break;
                    default:
                        return value;
                }
                break;
            case 'accTypes':
                switch (value) {
                    case 'A':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Asset </span>");
                        break;
                    case 'L':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#ff936b;color:white;padding:0.5em'> Liability </span>");
                        break;
                    case 'I':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Income </span>");
                        break;
                    case 'E':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Expense </span>");
                        break;
                    case 'Total':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#78a0ca;color:white;padding:0.8em'> Total : </span>");
                        break;
                    default:
                        return value;
                }
                break;
            case 'financialStatus':
                switch (value) {
                    case null:
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#96961e;color:white;padding:0.5em'> Waiting For Approval </span>");
                        break;
                    case 'M':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Returned For Clarification </span>");
                        break;
                    case 'P':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Approved </span>");
                        break;
                    default:
                }
            case 'outboxStatus':
                switch (value) {
                    case 'Failed':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> " + value + " </span>");
                        break;
                    case 'Sent Successfully':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> " + value + " </span>");
                        break;
                    default:
                }
            case 'requestStatusCheck':
                switch (value) {
                    case 'Y':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Resolved </span>");
                        break;
                    case 'O':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#96961e;color:white;padding:0.5em'> Open </span>");
                        break;
                    case 'C':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Closed </span>");
                        break;
                    case 'P':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:#0e6bf7;color:white;padding:0.5em'> On Process </span>");
                        break;
                    default:
                        return value;
                }
            case 'checkStatus':
                switch (value) {
                    case 'Y':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> Checked </span>");
                        break;
                    case 'P':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:orange;color:white;padding:0.5em'> Pending </span>");
                        break;
                    case 'N':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Reject </span>");
                        break;
                    default:
                        return value;
                }
                break;
            case 'regSource':
                switch (value) {
                    case 'A':
                        return 'Agent';
                        break;
                    case 'O':
                        return 'Online';
                        break;
                    case 'B':
                        return 'Bulk';
                        break;
                    case 'E':
                        return 'E-KYC';
                        break;
                    case 'P':
                        return 'Bank User';
                        break;
                    case 'Q':
                        return 'Agent Online';
                        break;
                    default:
                        return value;
                }
                break;
            case 'blackStatus':
                switch (value) {
                    case 'N':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:green;color:white;padding:0.5em'> No </span>");
                        break;                   
                    case 'Y':
                        return this._sanitizer.bypassSecurityTrustHtml("<span style='background-color:red;color:white;padding:0.5em'> Yes </span>");
                        break;
                    default:
                        return value;
                }
                break;
            default:
        }
    }

}
