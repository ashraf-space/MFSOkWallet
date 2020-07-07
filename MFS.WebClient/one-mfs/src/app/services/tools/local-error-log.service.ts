import { Injectable } from '@angular/core';
import * as data from 'src/error-log.json';

@Injectable({
  providedIn: 'root'
})
export class LocalErrorLogService {

    errorList: any = (data as any).default;

    constructor() {
    }

    getErrorList() {
        return this.errorList;
    }
}
