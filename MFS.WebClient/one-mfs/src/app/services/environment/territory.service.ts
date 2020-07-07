import { Injectable } from '@angular/core';
import { MfsSettingService } from '../mfs-setting.service';
import { map, first } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class TerritoryService {

    constructor(private http: HttpClient, private settings: MfsSettingService) { }
    GenerateTerritotyCode(value :string) {
        return this.http.get<any>(this.settings.environmentApiServer + '/Territory/GetTerritoryCode?code=' + value)
            .pipe(map(territoryCode => {
                return territoryCode;
            }));
    }
    save(territorryModel: any) {
        return this.http.post<any>(this.settings.environmentApiServer + '/Territory/SaveTerritory', territorryModel)
            .pipe(map(model => {
                return model;
            }));
    }

    
    
    getTerrirories() {
        return this.http.get<any>(this.settings.environmentApiServer + '/Territory/GetTerritories')
            .pipe(map(territoryList => {
                return territoryList;
            }));
    }
  
    getTerritoryById(territoryCode: string) {
        return this.http
            .get<any>(this.settings.environmentApiServer + '/Territory/GetTerritorieById?code=' + territoryCode)
            .pipe(map(model => {
                return model;
            }));
    }
    getAreaByAreaCode(code: string) {
        return this.http
            .get<any>(this.settings.environmentApiServer + '/Territory/getAreaByAreaCode?code=' + code)
            .pipe(map(model => {
                return model;
            }));
    }
    
    GetAreasDDL() {
        return this.http.get<any>(this.settings.environmentApiServer + '/Territory/GetAreasDDL')
            .pipe(map(areasList => {
                return areasList;
            }));
    }
}
