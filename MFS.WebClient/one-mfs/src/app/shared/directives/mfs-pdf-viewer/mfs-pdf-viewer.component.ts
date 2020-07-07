import { Component, OnInit, Input, ViewChild } from '@angular/core';

@Component({
  selector: 'mfs-pdf-viewer',
  templateUrl: './mfs-pdf-viewer.component.html',
  styleUrls: ['./mfs-pdf-viewer.component.css']
})
export class MfsPdfViewerComponent implements OnInit {
    @Input() src: any;
    @ViewChild('pdfViewer') pdfViewer;
    showReport: boolean = false;

    constructor() { }

    ngOnInit() {
    }

    refreshReport() {
        if (this.src.ext == 'PDF') {
            this.showReport = true;
            this.pdfViewer.pdfSrc = this.base64ToArrayBuffer(this.src.source);  // pdfSrc can be Blob or Uint8Array
            this.pdfViewer.downloadFileName = this.src.fileName;
            this.pdfViewer.refresh();
        }
        else {
            this.createAndDownloadBlobFile(this.base64ToArrayBuffer(this.src.source), this.src.fileName , this.src.ext);
        }
    }

    base64ToArrayBuffer(base64: string) {
        const binaryString = window.atob(base64); // Comment this if not using base64
        const bytes = new Uint8Array(binaryString.length);
        return bytes.map((byte, i) => binaryString.charCodeAt(i));
    }

    createAndDownloadBlobFile(body, filename, extension = 'PDF') {
        extension = this.configureExtension(extension);
        const blob = new Blob([body]);
        const fileName = `${filename}.${extension}`;
        if (navigator.msSaveBlob) {
            navigator.msSaveBlob(blob, fileName);
        } else {
            const link = document.createElement('a');
            if (link.download !== undefined) {
                const url = URL.createObjectURL(blob);
                link.setAttribute('href', url);
                link.setAttribute('download', fileName);
                link.style.visibility = 'hidden';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        }
    }

    configureExtension(ext) {
        var val;
        switch (ext) {
            case 'PDF':
                val = ext;
                break;
            case 'EXCEL':
                val = 'xls';
                break;
            case 'WORDOPENXML':
                val = 'doc';
                break;
            default:
        }
        return val;
    }
}
