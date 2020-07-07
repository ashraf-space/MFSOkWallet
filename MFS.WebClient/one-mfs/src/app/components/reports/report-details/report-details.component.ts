import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/_services';
import { first } from 'rxjs/operators';
import { ReportUtilityService } from 'src/app/services/report/report-utility.service';
import { ReportInfoService } from 'src/app/services/report/report-info.service';

@Component({
  selector: 'app-report-details',
  templateUrl: './report-details.component.html',
  styleUrls: ['./report-details.component.css']
})
export class ReportDetailsComponent implements OnInit {

    @ViewChild('mfsPdfViewer') pdfViewer;
    @ViewChild('form') childForm;
    isLoading: boolean = false;
    pdf: any;
    currentUserModel: any = {};
    entityId: any;
    reportObject: any;
    fileOptionList: any;
    model: any;

    constructor(private authenticationService: AuthenticationService,
        private reportUtilityService: ReportUtilityService,
        private router: Router,
        private reportInfoService: ReportInfoService,
        private route: ActivatedRoute) {
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });

        this.pdf = {};
        this.reportObject = {};
        this.model = {};
        this.reportObject.fileType = 'PDF';
        this.fileOptionList = reportUtilityService.getFileExtensionList();
    }

    ngOnInit() {
        this.entityId = this.route.snapshot.paramMap.get('id');
        this.getReportConfigById();
    }

    getReportConfigById(): any {
        this.isLoading = true;
        this.reportInfoService.getReportConfigById(this.entityId)
            .pipe(first())
            .subscribe(
                data => {
                    if (data) {
                        this.isLoading = false;    
                        this.model = data;
                    }
                },
                error => {
                    console.log(error);
                }
            )
    }

    generateReport() {
        if (this.buildReportItereny()) {
            this.isLoading = true;
            console.log(this.reportObject);
            this.reportUtilityService.generateReport(this.model.CallingApi, this.reportObject).pipe(first())
                .subscribe(
                    data => {
                        if (data) {
                            this.pdf.source = data;
                            this.pdf.ext = this.reportObject.fileType;
                            this.pdf.fileName = this.model.ReportName;
                            this.isLoading = false;
                            this.pdfViewer.refreshReport();
                        }
                        else {
                            this.isLoading = false;
                        }
                       
                    },
                    error => {
                        this.isLoading = false;
                    });
        }        
       
    }

    buildReportItereny() {
        if (this.childForm) {
            var reportParam = this.childForm.getReportParam();
            if (reportParam.isNotValidated) {
                return false;
            }
            else {
                this.reportObject.reportOption = JSON.stringify(reportParam);
                this.reportObject.reportDetails = this.model;
                return true;
            }
        }
    }

    cancel() {
        window.history.back();
    }
}
