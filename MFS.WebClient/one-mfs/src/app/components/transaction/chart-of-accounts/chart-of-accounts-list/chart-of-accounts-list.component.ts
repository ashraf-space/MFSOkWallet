import { Component, OnInit } from '@angular/core';
import { ChartOfAccountsService } from 'src/app/services/transaction';
import { first } from 'rxjs/operators';
import { TreeNode } from 'primeng/api';
import { AuthenticationService } from 'src/app/shared/_services';

@Component({
  selector: 'app-chart-of-accounts-list',
  templateUrl: './chart-of-accounts-list.component.html',
  styleUrls: ['./chart-of-accounts-list.component.css']
})
export class ChartOfAccountsListComponent implements OnInit {
    masterLog: any;
    graphicalChartOfAccounts: any;
    tabularChartOfAccounts: any;
    chartOfAccountsDropdownList: any;
    accountTypeList: any;
    levelTypeList: any;
    currentUserModel: any;
    statusList: any;

    showData: boolean = false;
    showCreateNewModal: boolean = false;

    views: any;
    viewData: number = 1;
    model: any;

    constructor(private chartOfAccountService: ChartOfAccountsService, private authenticationService: AuthenticationService,) {
        this.authenticationService.currentUser.subscribe(x => {
            this.currentUserModel = x;
        });
        this.graphicalChartOfAccounts = [];
        this.tabularChartOfAccounts = [];
        this.model = {};
        this.chartOfAccountsDropdownList = [];
        this.views = [{ label: 'Details', value: 1, icon: 'fas fa-list-ul' }, { label: 'Graphical', value: 2, icon: 'fas fa-chart-bar' }];
        this.accountTypeList = [{ label: 'Asset', value: 'A' }, { label: 'Liabilities', value: 'L' }, { label: 'Income', value: 'I' }, { label: 'Expense', value: 'E' }];
        this.levelTypeList = [{ label: 'Root', value: 'R' }, { label: 'Leaf', value: 'L' }, { label: 'Root-leaf', value: 'RL' }];
        this.statusList = [{ label: 'Active', value: 'A', icon: 'pi pi-check' }, { label: 'Inactive', value: 'I', icon: 'pi pi-times' }];
    }

    ngOnInit() {
        this.getChartOfAccountsList();
    }

    async getChartOfAccountsList() {
        this.chartOfAccountService.getChartOfAccountsList().pipe(first())
            .subscribe(
                data => {
                    this.masterLog = data;
                    this.generateHierarchyData();
                    this.generateDropdownList();
                },
                error => {
                    console.log(error);
                });
    }

    generateDropdownList(): any {        
        this.masterLog.forEach(obj => {
            if (obj.coaLevel != 4) {
                this.chartOfAccountsDropdownList.push({ label: obj.coaDesc + ' | ' + obj.coaCode + ' | ' + obj.coaLevel, value: obj });
            }
        });
    };

    generateHierarchyData() {
        this.masterLog.forEach(obj => {
            if (obj.coaLevel == 1) {
                this.graphicalChartOfAccounts.push([{
                    label: obj.coaDesc, children: [], expandedIcon: "fa fa-folder-open", collapsedIcon: "fa fa-folder"
                }]);
                this.tabularChartOfAccounts.push({
                    label: obj.sysCoaCode, children: [], data: { code: obj.coaCode, description: obj.coaDesc, level: obj.coaLevel, date: obj.createDate }
                });
                this.mapHierarchyData(this.graphicalChartOfAccounts.find(function (e) {
                    return e[0].label.trim() == obj.coaDesc.trim();
                })[0], this.tabularChartOfAccounts.find(function (e) {
                        return e.label.trim() == obj.sysCoaCode.trim();
                }), obj);
            }
        })
        
        this.graphicalChartOfAccounts.reverse();        
        console.log(this.graphicalChartOfAccounts[0]);

        this.showData = true;
    }

    mapHierarchyData(graphicalNode, tabularNode, parentDetails) {
        if (graphicalNode && tabularNode) {
            var childrenList = this.masterLog.filter(function (e) { return e.parentCode == parentDetails.sysCoaCode; });

            if (childrenList && childrenList.length) {                
                childrenList.forEach(obj => {
                    graphicalNode.children.push({ label: obj.coaDesc, children: [], expandedIcon: "fa fa-folder-open", collapsedIcon: "fa fa-folder" });
                    tabularNode.children.push({ label: obj.sysCoaCode, children: [], data: { code: obj.coaCode, description: obj.coaDesc, level: obj.coaLevel, date: obj.createDate } });
                    this.mapHierarchyData(graphicalNode.children.find(function (e) { return e.label.trim() == obj.coaDesc.trim() }),
                        tabularNode.children.find(function (e) { return e.label.trim() == obj.sysCoaCode.trim() }), obj);
                });
            }
        }
    }

    createNewCoa() {
        this.showCreateNewModal = true;
        this.model = {};
    }

    confirmAddNewGL() {
        if (this.generateSysCoaCode()) {
            this.chartOfAccountService.save(this.model).pipe(first())
                .subscribe(
                    data => {
                        this.showCreateNewModal = false;
                        this.graphicalChartOfAccounts = [];
                        this.tabularChartOfAccounts = [];
                        this.showData = false;
                        this.getChartOfAccountsList();
                        this.model = {};
                    },
                    error => {
                        console.log(error);
                    });
        }
    }

    generateSysCoaCode(): any {
        var dump = [];
        this.masterLog.forEach(obj => {            
            dump.push(obj.sysCoaCode.slice(2, obj.sysCoaCode.length));
        });
        var e = ("000000000" + (+dump.sort()[dump.length - 1] + 1)).slice(-10);
        this.model.sysCoaCode = this.model.accType + '' + this.model.coaLevel + '' + e;
        this.model.shortName = this.model.coaDesc;
        return true;
    }

    prepareCoaModel(event) {
        this.model = {};
        this.model.parentCode = event.value.sysCoaCode;
        this.model.pCoaCode = event.value.coaCode;
        this.model.pCoaDesc = event.value.coaDesc;
        this.model.pCoaLevel = event.value.coaLevel;
        this.model.coaLevel = +event.value.coaLevel + 1;
        this.model.accType = event.value.accType;
        this.model.levelType = 'L';
        this.model.createBy = this.currentUserModel.user.username;
        this.model.status = 'A';
        this.model.p_LevelType = event.value.levelType;
        this.getMaxCoaCode();
    }

    getMaxCoaCode() {        
        var dump = [];
        this.masterLog.forEach(obj => {
            if (obj.coaLevel == this.model.coaLevel && obj.accType == this.model.accType && obj.parentCode == this.model.parentCode) {
                dump.push(+obj.coaCode);
            }
        });
        dump.sort();
        this.model.coaCode = dump.length == 0 ? this.model.pCoaCode : dump[dump.length - 1] + '';     
        var fPart = this.model.coaCode.slice(0, (this.model.coaLevel * 2) - 1);
        var sPart = this.model.coaCode.slice((this.model.coaLevel * 2) - 1, this.model.coaCode.length);
        this.model.coaCode = (+fPart + 1) + '' + sPart;        
    }    
}
