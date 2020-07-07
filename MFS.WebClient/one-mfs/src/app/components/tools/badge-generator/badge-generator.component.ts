import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import html2canvas from 'html2canvas';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgxQRCodeModule } from 'ngx-qrcode2';
import { FormsModule } from '@angular/forms';
declare let jsPDF: any;
import { QrCodeService } from '../../../services/tools/qr-code.service'
import { first } from 'rxjs/operators';
import { DomSanitizer } from '@angular/platform-browser';
import { KycService } from '../../../services/distribution/kyc.service';
import { MessageService, MenuItem } from 'primeng/api';

@Component({
    selector: 'app-badge-generator',
    templateUrl: './badge-generator.component.html',
    styleUrls: ['./badge-generator.component.css']
})
export class BadgeGeneratorComponent implements OnInit {
    model: any;
    window: any;
    imageToShow: any;
    title = 'generate-qrcode';
    elementType: 'url' | 'canvas' | 'img' = 'url';
    value: string;
    display: boolean = false;
    href: string;
    isImageLoading: boolean = false;
    clientList: any;
    sanitizedImageData: any;
    isLoading: boolean = false;
    constructor(private qrCodeService: QrCodeService,
        private sanitizer: DomSanitizer,
        private kycService: KycService,
        private messageService: MessageService) {
        this.clientList = [{ label: 'Merchant', value: 'M' }, { label: 'Agent', value: 'A' }];
        this.model = {};
        this.model.qrCodeNumber = '';
        this.window = window;
    }

    ngOnInit() {
    }

    downloadImage() {
        html2canvas(document.querySelector("#main-badge")).then(canvas => {

            var pdf = new jsPDF('p', 'pt', [canvas.width, canvas.height]);

            var imgData = canvas.toDataURL("image/jpeg", 1.0);
            pdf.addImage(imgData, 0, 0, canvas.width, canvas.height);
            pdf.save('converteddoc.pdf');
            window.open(imgData);
            
        });       
    }
    generateQRCode(): any {
        this.isLoading = true;       
        this.qrCodeService.getQrImage(this.model.qrCodeNumber)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.imageToShow = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + data);                      
                        this.isImageLoading = true;
                        this.isLoading = false;
                    }
                },
                error => {
                    console.log(error);
                });
    }
 
    checkIfExist() {
        this.isLoading = true;
        this.kycService.getReginfoByMphone(this.model.qrCodeNumber)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.isImageLoading = false;
                        if (this.model.clientType === data.catId) {
                            this.model.clientName = data.companyName;
                            this.isLoading = false;
                        }
                        else {
                            this.messageService.add({severity: 'error', summary: 'Type mismatched', detail: 'Please select proper client type', closable: true});
                            this.isLoading = false;
                        }

                    }
                    else {
                        this.isLoading = false;
                    }
                },
                error => {
                    console.log(error);
                });
    }
}
