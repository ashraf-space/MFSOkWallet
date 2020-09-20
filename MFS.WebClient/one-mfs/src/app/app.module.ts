import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule }    from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent }  from './app.component';
import { routing }        from './app.routing';

import { AlertComponent } from './shared/_components';
import { JwtInterceptor, ErrorInterceptor } from './shared/_helpers';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/security/login';
import { RegisterComponent } from './components/security/register';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import {  ConfirmationService } from 'primeng/api';

import { SlideMenuModule } from 'primeng/slidemenu';
import { PanelMenuModule } from 'primeng/panelmenu';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { ChartModule } from 'primeng/chart';
import { TreeModule } from 'primeng/tree';
import { MenubarModule } from 'primeng/menubar';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { SplitButtonModule } from 'primeng/splitbutton';
import { TableModule } from 'primeng/table';
import { MegaMenuModule } from 'primeng/megamenu';
import { StepsModule } from 'primeng/steps';
import { MenuModule } from 'primeng/menu';
import { PanelModule } from 'primeng/panel';
import { FieldsetModule } from 'primeng/fieldset';
import { CalendarModule } from 'primeng/calendar';
import { SelectButtonModule } from 'primeng/selectbutton';
import { DropdownModule } from 'primeng/dropdown';
import { InputMaskModule } from 'primeng/inputmask';
import { KeyFilterModule } from 'primeng/keyfilter';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { PasswordModule } from 'primeng/password';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { InputSwitchModule } from 'primeng/inputswitch';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { TabViewModule } from 'primeng/tabview';
import { TabMenuModule } from 'primeng/tabmenu';
import { AccordionModule } from 'primeng/accordion';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { TreeTableModule } from 'primeng/treetable';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { CarouselModule } from 'primeng/carousel';
import { SidebarModule } from 'primeng/sidebar';
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { CheckboxModule } from 'primeng/checkbox';

import { UserIdleModule } from 'angular-user-idle';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxQRCodeModule } from 'ngx-qrcode2';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';

import { CustomerBaseComponent } from './components/setup/customer-components/customer-base/customer-base.component';
import { CustomerWorklistComponent } from './components/setup/customer-components/customer-worklist/customer-worklist.component';
import { CustomerCreateComponent } from './components/setup/customer-components/customer-create/customer-create.component';
import { CustomerDetailsComponent } from './components/setup/customer-components/customer-details/customer-details.component';
import { GenericGridComponent } from './shared/directives/generic-grid/generic-grid.component';
import { FeatureCreateComponent } from './components/security/feature/feature-create/feature-create.component';
import { FeatureListComponent } from './components/security/feature/feature-list/feature-list.component';
import { FeatureCategoryCreateComponent } from './components/security/feature-category/feature-category-create/feature-category-create.component';
import { FeatureCategoryListComponent } from './components/security/feature-category/feature-category-list/feature-category-list.component';
import { LoaderComponent } from './shared/directives/loader/loader.component';
import { GenericFormActionComponent } from './shared/directives/generic-form-action/generic-form-action.component';
import { BranchAddoreditComponent } from './components/environment/branch/branch-addoredit/branch-addoredit.component';
import { BranchListComponent } from './components/environment/branch/branch-list/branch-list.component';
import { RegionAddoreditComponent } from './components/environment/distribution-channel/region/region-addoredit/region-addoredit.component';
import { RegionListComponent } from './components/environment/distribution-channel/region/region-list/region-list.component';
import { DistributorListComponent } from './components/distribution/distributor/distributor-list/distributor-list.component';
import { DistributorAddoreditComponent } from './components/distribution/distributor/distributor-addoredit/distributor-addoredit.component';
import { AgentAddoreditComponent } from './components/distribution/agent/agent-addoredit/agent-addoredit.component';
import { AgentListComponent } from './components/distribution/agent/agent-list/agent-list.component';
import { RoleListComponent } from './components/security/role/role-list/role-list.component';
import { RoleAddEditComponent } from './components/security/role/role-add-edit/role-add-edit.component';
import { ApplicationUserListComponent } from './components/security/application-user/application-user-list/application-user-list.component';
import { ApplicationUserAddEditComponent } from './components/security/application-user/application-user-add-edit/application-user-add-edit.component';
import { AreaListComponent } from './components/environment/area/area-list/area-list.component';
import { AreaAddoreditComponent } from './components/environment/area/area-addoredit/area-addoredit.component';
import { GenericModalComponent } from './shared/directives/generic-modal/generic-modal.component';
import { AccessPermissionComponent } from './components/security/access-permission/access-permission.component';
import { TerritoryAddoreditComponent } from './components/environment/territory/territory-addoredit/territory-addoredit.component';
import { TerritoryListComponent } from './components/environment/territory/territory-list/territory-list.component';
import { ClusterListComponent } from './components/environment/cluster/cluster-list/cluster-list.component';
import { ClusterAddoreditComponent } from './components/environment/cluster/cluster-addoredit/cluster-addoredit.component';
import { DsrListComponent } from './components/distribution/dsr/dsr-list/dsr-list.component';
import { DsrAddoreditComponent } from './components/distribution/dsr/dsr-addoredit/dsr-addoredit.component';
import { AuthorizeButtonComponent } from './shared/directives/authorize-button/authorize-button.component';
import { GenericStepperFormActionComponent } from './shared/directives/generic-stepper-form-action/generic-stepper-form-action.component';
import { MerchantListComponent } from './components/distribution/merchant/merchant-list/merchant-list.component';
import { MerchantAddoreditComponent } from './components/distribution/merchant/merchant-addoredit/merchant-addoredit.component';
import { CustomerListComponent } from './components/distribution/customer/customer-list/customer-list.component';
import { CustomerAddoreditComponent } from './components/distribution/customer/customer-addoredit/customer-addoredit.component';
import { HotkeyListComponent } from './components/client/hotkey/hotkey-list/hotkey-list.component';
import { OutboxComponent } from './components/client/customer-care/outbox/outbox.component';
import { MfsDatepickerComponent } from './shared/directives/mfs-datepicker/mfs-datepicker.component';
import { MessageResendComponent } from './components/client/customer-care/message-resend/message-resend.component';
import { AgentLocationComponent } from './components/client/agent-location/agent-location.component';
import { CustomerCarePortalComponent } from './components/client/customer-care/customer-care-portal/customer-care-portal.component';
import { ClientProfileComponent } from './components/client/client-profile/client-profile.component';
import { ClientCategoryPipe } from './shared/filter-pipes/client-category.pipe';
import { YesNoPipe } from './shared/filter-pipes/yes-no.pipe';
import { GenderCheckPipe } from './shared/filter-pipes/gender-check.pipe';
import { ClientWorkTabComponent } from './components/client/client-work-tab/client-work-tab.component';
import { PhotoIdPipe } from './shared/filter-pipes/photo-id.pipe';
import { StatusCheckPipe } from './shared/filter-pipes/status-check.pipe';
import { DistributorDepositListComponent } from './components/transaction/distributor-deposit/distributor-deposit-list/distributor-deposit-list.component';
import { DistributorDepositAddoreditComponent } from './components/transaction/distributor-deposit/distributor-deposit-addoredit/distributor-deposit-addoredit.component';
import { SecuredDataPipe } from './shared/filter-pipes/secured-data.pipe';
import { TransactionMasterComponent } from './components/transaction/transaction-master/transaction-master.component';
import { TransactionDetailComponent } from './components/transaction/transaction-detail/transaction-detail.component';
import { CustomerAccountsMappingComponent } from './components/tools/customer-accounts-mapping/customer-accounts-mapping.component';
import { CustomerAccountsMappingListComponent } from './components/tools/customer-accounts-mapping-list/customer-accounts-mapping-list.component';
import { FundEntryComponent } from './components/transaction/fund-transfer/fund-entry/fund-entry.component';
import { TransferComponent } from './components/transaction/fund-transfer/transfer/transfer.component';
import { BdtCurrencyPipe } from './shared/filter-pipes/bdt-currency.pipe';
import { CbsAccRemapComponent } from './components/tools/cbs-acc-remap/cbs-acc-remap.component';
import { CbsMappingCheckComponent } from './components/tools/cbs-mapping-check/cbs-mapping-check.component';
import { FundEntryActoacComponent } from './components/transaction/fund-transfer/fund-entry-actoac/fund-entry-actoac.component';
import { ErrorListComponent } from './components/client/customer-care/error-list/error-list.component';
import { FundEntryActoglComponent } from './components/transaction/fund-transfer/fund-entry-actogl/fund-entry-actogl.component';
import { FundEntryGltoacComponent } from './components/transaction/fund-transfer/fund-entry-gltoac/fund-entry-gltoac.component';
import { ChartOfAccountsListComponent } from './components/transaction/chart-of-accounts/chart-of-accounts-list/chart-of-accounts-list.component';
import { FundEntryCommissiontomainComponent } from './components/transaction/fund-transfer/fund-entry-commissiontomain/fund-entry-commissiontomain.component';
import { RateConfigListComponent } from './components/transaction/rate-config/rate-config-list/rate-config-list.component';
import { RateConfigAddOrEditComponent } from './components/transaction/rate-config/rate-config-add-or-edit/rate-config-add-or-edit.component';
import { FundTransferGltoglComponent } from './components/transaction/fund-transfer/fund-transfer-gltogl/fund-transfer-gltogl.component';
import { FundTransferActoacComponent } from './components/transaction/fund-transfer/fund-transfer-actoac/fund-transfer-actoac.component';
import { BranchCashInComponent } from './components/transaction/branch-cash-in/branch-cash-in.component';
import { BranchCashOutComponent } from './components/transaction/branch-cash-out/branch-cash-out.component';
import { FundTransferActoglComponent } from './components/transaction/fund-transfer/fund-transfer-actogl/fund-transfer-actogl.component';
import { FundTransferGltoacComponent } from './components/transaction/fund-transfer/fund-transfer-gltoac/fund-transfer-gltoac.component';
import { CustomerRequestComponent } from './components/client/customer-care/customer-request/customer-request.component';
import { InterestTaxVatRateSetupComponent } from './components/environment/interest-tax-vat-rate-setup/interest-tax-vat-rate-setup.component';
import { MerchantConfigAddoreditComponent } from './components/environment/merchant-config/merchant-config-addoredit/merchant-config-addoredit.component';
import { RobiTopupStockEntryComponent } from './components/transaction/robi-topup-stock-entry/robi-topup-stock-entry.component';
import { BankDepositStatusComponent } from './components/transaction/bank-deposit-status/bank-deposit-status.component';
import { BadgeGeneratorComponent } from './components/tools/badge-generator/badge-generator.component';
import { ChainMerchantAddoreditComponent } from './components/distribution/merchant/chain-merchant-addoredit/chain-merchant-addoredit.component';
import { ChainMerchantListComponent } from './components/distribution/merchant/chain-merchant-list/chain-merchant-list.component';
import { ErrorComponent } from './shared/_components/error/error.component';
import { MerchantConfigComponent } from './components/distribution/merchant/merchant-config/merchant-config.component';
import { MerchantConfigListComponent } from './components/distribution/merchant/merchant-config-list/merchant-config-list.component';
import { LocalErrorLogComponent } from './components/tools/local-error-log/local-error-log.component';
import { QrCodeComponent } from './components/tools/qr-code/qr-code.component';
import { ReportListComponent } from './components/reports/report-list/report-list.component';
import { ReportDetailsComponent } from './components/reports/report-details/report-details.component';
import { ReportInfoComponent } from './components/reports/report-info/report-info.component';
import { MfsPdfViewerComponent } from './shared/directives/mfs-pdf-viewer/mfs-pdf-viewer.component';
import { TransactionHistoryReportComponent } from './components/reports/report-collection/transaction-history-report/transaction-history-report.component';
import { AccountStatementReportComponent } from './components/reports/report-collection/account-statement-report/account-statement-report.component';
import { ClientUpdateComponent } from './components/client/client-update/client-update.component';
import { UppercaseDirective } from './shared/directives/uppercase.directive';
import { SearchClientProfileComponent } from './components/client/search-client-profile/search-client-profile.component';
import { ReportTypePipe } from './shared/filter-pipes/report-type.pipe';
import { DistributorReplaceComponent } from './components/distribution/distributor-replace/distributor-replace.component';
import { CurrentAffairsStatementComponent } from './components/reports/report-collection/current-affairs-statement/current-affairs-statement.component';
import { MerchantUserComponent } from './components/distribution/merchant/merchant-user/merchant-user.component';
import { MerchantUserListComponent } from './components/distribution/merchant/merchant-user-list/merchant-user-list.component';
import { ChartOfAccountsComponent } from './components/reports/report-collection/chart-of-accounts/chart-of-accounts.component';
import { EodAffairsStatementComponent } from './components/reports/report-collection/eod-affairs-statement/eod-affairs-statement.component';
import { RegistrationReportComponent } from './components/reports/report-collection/registration-report/registration-report.component';
import { DisburseProcessComponent } from './components/transaction/disbursement/disburse-process/disburse-process.component';
import { CompanyListComponent } from './components/transaction/disbursement/company-list/company-list.component';
import { CompanyAddoreditComponent } from './components/transaction/disbursement/company-addoredit/company-addoredit.component';
import { CompanyDisbursementLimitComponent } from './components/transaction/disbursement/company-disbursement-limit/company-disbursement-limit.component';
import { DisburseAmountPostingComponent } from './components/transaction/disbursement/disburse-amount-posting/disburse-amount-posting.component';
import { DisbursePostingComponent } from './components/transaction/disbursement/disburse-posting/disburse-posting.component';
import { RegistrationSummaryComponent } from './components/reports/report-collection/registration-summary/registration-summary.component';
import { AgentInformationComponent } from './components/reports/report-collection/agent-information/agent-information.component';
import { EnterpriseAddoreditComponent } from './components/distribution/enterprise/enterprise-addoredit/enterprise-addoredit.component';
import { EnterpriseListComponent } from './components/distribution/enterprise/enterprise-list/enterprise-list.component';
import { GlStatementComponent } from './components/reports/report-collection/gl-statement/gl-statement.component';
import { EodComponent } from './components/process/eod/eod.component';
import { KycBalanceComponent } from './components/reports/report-collection/kyc-balance/kyc-balance.component';
import { BulkUploadComponent } from './components/distribution/bulk-upload/bulk-upload.component';
import { TransactionComponent } from './components/reports/report-collection/transaction-type/transaction/transaction.component';
import { AuditTrailComponent } from './components/security/audit/audit-trail/audit-trail.component';
import { AuditTrailDtlComponent } from './components/security/audit/audit-trail-dtl/audit-trail-dtl.component';
import { DpdcDescoComponent } from './components/reports/report-collection/dpdc-desco/dpdc-desco.component';
import { FundTransferComponent } from './components/reports/report-collection/transaction-type/fund-transfer/fund-transfer.component';
import { AgentReplaceComponent } from './components/distribution/agent-replace/agent-replace.component';
import { BranchCashinCashoutComponent } from './components/reports/report-collection/transaction-type/branch-cashin-cashout/branch-cashin-cashout.component';
import { CreditCardReportComponent } from './components/reports/report-collection/credit-card-report/credit-card-report.component';
import { BlinkTopupStockEntryComponent } from './components/transaction/blink-topup-stock-entry/blink-topup-stock-entry.component';
import { CreditBeftnComponent } from './components/reports/report-collection/credit-beftn/credit-beftn.component';

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        ReactiveFormsModule,
        HttpClientModule,
        routing,
        FormsModule,
        SlideMenuModule,
        PanelMenuModule,
        ScrollPanelModule,
        ChartModule,
        TreeModule,
        MenubarModule,
        CardModule,
        InputTextModule,
        SplitButtonModule,
        TableModule,
        MegaMenuModule,
        MenuModule,
        StepsModule,
        PanelModule,
        FieldsetModule,
        CalendarModule,
        SelectButtonModule,
        DropdownModule,
        InputMaskModule,
        KeyFilterModule,
        OverlayPanelModule,
        SliderModule,
        MultiSelectModule,
        ConfirmDialogModule,
        ToastModule,
        DialogModule,
        PasswordModule,
        InputTextareaModule,
        InputSwitchModule,
        MessageModule,
        MessagesModule,
        NgbModule,
        NgxQRCodeModule,
        TabViewModule,
        TabMenuModule,
        AccordionModule,
        ProgressSpinnerModule,
        OrganizationChartModule,
        TreeTableModule,
        TieredMenuModule,
        CarouselModule,
        SidebarModule,
        PdfJsViewerModule,
        TriStateCheckboxModule,
        ToggleButtonModule,
        CheckboxModule,
        UserIdleModule.forRoot({ idle: 900, timeout: 300, ping: 120 })
    ],
    declarations: [
        AppComponent,
        AlertComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
        CustomerBaseComponent,
        CustomerWorklistComponent,
        CustomerCreateComponent,
        CustomerDetailsComponent,
        GenericGridComponent,
        FeatureCreateComponent,
        FeatureListComponent,        
        FeatureCategoryCreateComponent,
        FeatureCategoryListComponent,
        LoaderComponent,
        GenericFormActionComponent,
        BranchAddoreditComponent,
        BranchListComponent,
        RegionAddoreditComponent,
        RegionListComponent,
        DistributorListComponent,
        DistributorAddoreditComponent,
        AgentAddoreditComponent,
        AgentListComponent,
        RoleListComponent,
        RoleAddEditComponent,
        ApplicationUserListComponent,
        ApplicationUserAddEditComponent,
        AreaListComponent,
        AreaAddoreditComponent,
        GenericModalComponent,
        AccessPermissionComponent,
        TerritoryAddoreditComponent,
        TerritoryListComponent,
        ClusterListComponent,
        ClusterAddoreditComponent,
        DsrListComponent,
        DsrAddoreditComponent,
        AuthorizeButtonComponent,
        GenericStepperFormActionComponent,
        MerchantListComponent,
        MerchantAddoreditComponent,
        CustomerListComponent,
        CustomerAddoreditComponent,
        HotkeyListComponent,
        OutboxComponent,
        MfsDatepickerComponent,
        MessageResendComponent,
        AgentLocationComponent,
        CustomerCarePortalComponent,
        ClientProfileComponent,
        ClientCategoryPipe,
        YesNoPipe,
        GenderCheckPipe,
        ClientWorkTabComponent,
        PhotoIdPipe,
        StatusCheckPipe,
        DistributorDepositListComponent,
        DistributorDepositAddoreditComponent,
        SecuredDataPipe,
        TransactionMasterComponent,
        TransactionDetailComponent,
        CustomerAccountsMappingComponent,
        CustomerAccountsMappingListComponent,
        FundEntryComponent,
        TransferComponent,
        BdtCurrencyPipe,
        CbsAccRemapComponent,
        CbsMappingCheckComponent,
        FundEntryActoacComponent,
        ErrorListComponent,
        FundEntryActoglComponent,
        FundEntryGltoacComponent,
        ChartOfAccountsListComponent,
        FundEntryCommissiontomainComponent,
        RateConfigListComponent,
        RateConfigAddOrEditComponent,
        FundTransferGltoglComponent,
        FundTransferActoacComponent,
        BranchCashInComponent,
        BranchCashOutComponent,
        FundTransferActoglComponent,
        FundTransferGltoacComponent,
        CustomerRequestComponent,
        InterestTaxVatRateSetupComponent,
        MerchantConfigAddoreditComponent,
        RobiTopupStockEntryComponent,
        BankDepositStatusComponent,
        BadgeGeneratorComponent,
        ChainMerchantAddoreditComponent,
        ChainMerchantListComponent,
        ErrorComponent,
        MerchantConfigComponent,
        MerchantConfigListComponent,
        LocalErrorLogComponent,
        QrCodeComponent,
        ReportListComponent,
        ReportDetailsComponent,
        ReportInfoComponent,        
        MfsPdfViewerComponent,
        TransactionHistoryReportComponent,
        AccountStatementReportComponent,
        UppercaseDirective,
        ClientUpdateComponent,
        SearchClientProfileComponent,
        ReportTypePipe,
        DistributorReplaceComponent,
        CurrentAffairsStatementComponent,
        MerchantUserComponent,
        MerchantUserListComponent,
        ChartOfAccountsComponent,
        EodAffairsStatementComponent,
        RegistrationReportComponent,
        DisburseProcessComponent,
        CompanyListComponent,
        CompanyAddoreditComponent,
        CompanyDisbursementLimitComponent,
        DisburseAmountPostingComponent,
        DisbursePostingComponent,
        RegistrationSummaryComponent,
        AgentInformationComponent,
        EnterpriseAddoreditComponent,
        EnterpriseListComponent,
        GlStatementComponent,
        EodComponent,
        KycBalanceComponent,
        BulkUploadComponent,
        TransactionComponent,
        AuditTrailComponent,
        AuditTrailDtlComponent,
        DpdcDescoComponent,
        FundTransferComponent,
        AgentReplaceComponent,
        BranchCashinCashoutComponent,
        CreditCardReportComponent,
        BlinkTopupStockEntryComponent,
        CreditBeftnComponent
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        //{
        //    provide: HTTP_INTERCEPTORS,
        //    useClass: LoaderInterceptorService,
        //    multi: true
        //},
        MessageService, ConfirmationService
        //TitleService              
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }