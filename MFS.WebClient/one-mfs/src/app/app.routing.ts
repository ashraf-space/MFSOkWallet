import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home';
import { LoginComponent } from './components/security/login';
import { RegisterComponent } from './components/security/register';
import { AuthGuard } from './shared/_guards';

import { CustomerBaseComponent } from './components/setup/customer-components/customer-base/customer-base.component';
import { CustomerWorklistComponent } from './components/setup/customer-components/customer-worklist/customer-worklist.component';
import { CustomerCreateComponent } from './components/setup/customer-components/customer-create/customer-create.component';
import { CustomerDetailsComponent } from './components/setup/customer-components/customer-details/customer-details.component';
import { FeatureCreateComponent } from './components/security/feature/feature-create/feature-create.component';
import { FeatureListComponent } from './components/security/feature/feature-list/feature-list.component';
import { FeatureCategoryCreateComponent } from './components/security/feature-category/feature-category-create/feature-category-create.component';
import { FeatureCategoryListComponent } from './components/security/feature-category/feature-category-list/feature-category-list.component';
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
import { AccessPermissionComponent } from './components/security/access-permission/access-permission.component';
import { TerritoryAddoreditComponent } from './components/environment/territory/territory-addoredit/territory-addoredit.component';
import { TerritoryListComponent } from './components/environment/territory/territory-list/territory-list.component';
import { ClusterListComponent } from './components/environment/cluster/cluster-list/cluster-list.component';
import { ClusterAddoreditComponent } from './components/environment/cluster/cluster-addoredit/cluster-addoredit.component';
import { DsrListComponent } from './components/distribution/dsr/dsr-list/dsr-list.component';
import { DsrAddoreditComponent } from './components/distribution/dsr/dsr-addoredit/dsr-addoredit.component';
import { MerchantListComponent } from './components/distribution/merchant/merchant-list/merchant-list.component';
import { MerchantAddoreditComponent } from './components/distribution/merchant/merchant-addoredit/merchant-addoredit.component';
import { CustomerListComponent } from './components/distribution/customer/customer-list/customer-list.component';
import { CustomerAddoreditComponent } from './components/distribution/customer/customer-addoredit/customer-addoredit.component';
import { HotkeyListComponent } from './components/client/hotkey/hotkey-list/hotkey-list.component';
import { OutboxComponent } from './components/client/customer-care/outbox/outbox.component';
import { MessageResendComponent } from './components/client/customer-care/message-resend/message-resend.component';
import { AgentLocationComponent } from './components/client/agent-location/agent-location.component';
import { CustomerCarePortalComponent } from './components/client/customer-care/customer-care-portal/customer-care-portal.component';
import { ClientProfileComponent } from './components/client/client-profile/client-profile.component';
import { DistributorDepositListComponent } from './components/transaction/distributor-deposit/distributor-deposit-list/distributor-deposit-list.component';
import { DistributorDepositAddoreditComponent } from './components/transaction/distributor-deposit/distributor-deposit-addoredit/distributor-deposit-addoredit.component';
import { TransactionMasterComponent } from './components/transaction/transaction-master/transaction-master.component';
import { TransactionDetailComponent } from './components/transaction/transaction-detail/transaction-detail.component';
import { CustomerAccountsMappingListComponent } from './components/tools/customer-accounts-mapping-list/customer-accounts-mapping-list.component';
import { CustomerAccountsMappingComponent } from './components/tools/customer-accounts-mapping/customer-accounts-mapping.component';
import { FundEntryComponent } from './components/transaction/fund-transfer/fund-entry/fund-entry.component';
import { TransferComponent } from './components/transaction/fund-transfer/transfer/transfer.component';
import { CbsAccRemapComponent } from './components/tools/cbs-acc-remap/cbs-acc-remap.component';
import { CbsMappingCheckComponent } from './components/tools/cbs-mapping-check/cbs-mapping-check.component';
import { FundEntryActoacComponent } from './components/transaction/fund-transfer/fund-entry-actoac/fund-entry-actoac.component';
import { FundEntryActoglComponent } from './components/transaction/fund-transfer/fund-entry-actogl/fund-entry-actogl.component';
import { FundEntryGltoacComponent } from './components/transaction/fund-transfer/fund-entry-gltoac/fund-entry-gltoac.component';
import { ErrorListComponent } from './components/client/customer-care/error-list/error-list.component';
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
import { ReportDetailsComponent } from './components/reports/report-details/report-details.component';
import { ReportInfoComponent } from './components/reports/report-info/report-info.component';
import { ReportListComponent } from './components/reports/report-list/report-list.component';
import { SearchClientProfileComponent } from './components/client/search-client-profile/search-client-profile.component';
import { DistributorReplaceComponent } from './components/distribution/distributor-replace/distributor-replace.component';
import { MerchantUserComponent } from './components/distribution/merchant/merchant-user/merchant-user.component';
import { MerchantUserListComponent } from './components/distribution/merchant/merchant-user-list/merchant-user-list.component';
import { DisburseProcessComponent } from './components/transaction/disbursement/disburse-process/disburse-process.component';
import { CompanyListComponent } from './components/transaction/disbursement/company-list/company-list.component';
import { CompanyAddoreditComponent } from './components/transaction/disbursement/company-addoredit/company-addoredit.component';
import { CompanyDisbursementLimitComponent } from './components/transaction/disbursement/company-disbursement-limit/company-disbursement-limit.component';
import { DisburseAmountPostingComponent } from './components/transaction/disbursement/disburse-amount-posting/disburse-amount-posting.component';
import { DisbursePostingComponent } from './components/transaction/disbursement/disburse-posting/disburse-posting.component';
import { EnterpriseListComponent } from './components/distribution/enterprise/enterprise-list/enterprise-list.component';
import { EnterpriseAddoreditComponent } from './components/distribution/enterprise/enterprise-addoredit/enterprise-addoredit.component';
import { EodComponent } from './components/process/eod/eod.component';
import { ClientWorkTabComponent } from './components/client/client-work-tab/client-work-tab.component';
import { BulkUploadComponent } from './components/distribution/bulk-upload/bulk-upload.component';
import { AuditTrailComponent } from './components/security/audit/audit-trail/audit-trail.component';
import { AuditTrailDtlComponent } from './components/security/audit/audit-trail-dtl/audit-trail-dtl.component';
import { AgentReplaceComponent } from './components/distribution/agent-replace/agent-replace.component';
import { BlinkTopupStockEntryComponent } from './components/transaction/blink-topup-stock-entry/blink-topup-stock-entry.component';
import { CommissionEntryComponent } from './components/transaction/fund-transfer/commission-entry/commission-entry.component';
import { AirtelTopupStockEntryComponent } from './components/transaction/airtel-topup-stock-entry/airtel-topup-stock-entry.component';
import { CommissionApprovalComponent } from './components/transaction/fund-transfer/commission-approval/commission-approval.component';
import { NidPaymentCollectionComponent } from './components/transaction/nid-payment-collection/nid-payment-collection.component';
import { BillCollectionCommonComponent } from './components/transaction/bill-collection-common/bill-collection-common.component';
import { DisbursementUserListComponent } from './components/security/disbursement-user/disbursement-user-list/disbursement-user-list.component';
import { DisbursementUserAddEditComponent } from './components/security/disbursement-user/disbursement-user-add-edit/disbursement-user-add-edit.component';
import { ChngStatusComponent } from './components/tools/chng-status/chng-status.component';
import { CustRetailComponent } from './components/distribution/customer/retail/cust-retail/cust-retail.component';
import { CustRetailListComponent } from './components/distribution/customer/retail/cust-retail-list/cust-retail-list.component';
import { StatusListComponent } from './components/tools/status-list/status-list.component';
import { B2bAddoreditComponent } from './components/distribution/distributor/b2b-addoredit/b2b-addoredit.component';
import { B2bListComponent } from './components/distribution/distributor/b2b-list/b2b-list.component';
import { DashboardComponent } from './components/client/dashboard/dashboard.component';
import { B2bDsrListComponent } from './components/distribution/dsr/b2b-dsr-list/b2b-dsr-list.component';
import { B2bDsrAddoreditComponent } from './components/distribution/dsr/b2b-dsr-addoredit/b2b-dsr-addoredit.component';
import { TtalkTopupStockEntryComponent } from './components/transaction/ttalk-topup-stock-entry/ttalk-topup-stock-entry.component';
import { B2bRetailListComponent } from './components/distribution/distributor/b2b-retail-list/b2b-retail-list.component';
import { B2bRetailAddoreditComponent } from './components/distribution/distributor/b2b-retail-addoredit/b2b-retail-addoredit.component';
import { DownloadReceiptComponent } from './components/transaction/download-receipt/download-receipt.component';
import { CommissionConvertionListComponent } from './components/transaction/commission-convertion/commission-convertion-list/commission-convertion-list.component';
import { CommissionConvertionAddoreditComponent } from './components/transaction/commission-convertion/commission-convertion-addoredit/commission-convertion-addoredit.component';

const appRoutes: Routes = [
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent, canActivate: [AuthGuard] },
    { path: 'customer/create', component: CustomerCreateComponent, canActivate: [AuthGuard] },
    { path: 'customer/worklist', component: CustomerWorklistComponent, canActivate: [AuthGuard] },
    { path: 'feature/create/:id', component: FeatureCreateComponent, canActivate: [AuthGuard] },
    { path: 'feature/worklist', component: FeatureListComponent, canActivate: [AuthGuard] },
    { path: 'feature-category/create/:id', component: FeatureCategoryCreateComponent, canActivate: [AuthGuard] },
    { path: 'feature-category/worklist', component: FeatureCategoryListComponent, canActivate: [AuthGuard] },
    { path: 'bank-branch/list', component: BranchListComponent, canActivate: [AuthGuard] },
    { path: 'bank-branch/addoredit/:id', component: BranchAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'distribution-channel/region/list', component: RegionListComponent, canActivate: [AuthGuard] },
    { path: 'distribution-channel/region/addoredit/:id', component: RegionAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'distributor/list', component: DistributorListComponent, canActivate: [AuthGuard] },
    { path: 'distributor/addoredit/:id', component: DistributorAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'agent/addoredit/:id', component: AgentAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'agent/list', component: AgentListComponent, canActivate: [AuthGuard] },
    { path: 'role/list', component: RoleListComponent, canActivate: [AuthGuard] },
    { path: 'role/add-edit/:id', component: RoleAddEditComponent, canActivate: [AuthGuard] },
    { path: 'application-user/list', component: ApplicationUserListComponent, canActivate: [AuthGuard] },
    { path: 'application-user/add-edit/:id', component: ApplicationUserAddEditComponent },
    { path: 'area/list', component: AreaListComponent, canActivate: [AuthGuard] },
    { path: 'area/addoredit/:id', component: AreaAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'territory/list', component: TerritoryListComponent, canActivate: [AuthGuard] },
    { path: 'territory/addoredit/:id', component: TerritoryAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'cluster/list', component: ClusterListComponent, canActivate: [AuthGuard] },
    { path: 'cluster/addoredit/:id', component: ClusterAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'access-permission/permissions', component: AccessPermissionComponent, canActivate: [AuthGuard] },
    { path: 'dsr/list', component: DsrListComponent, canActivate: [AuthGuard] },
    { path: 'dsr/addoredit/:id', component: DsrAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'merchant/list', component: MerchantListComponent, canActivate: [AuthGuard] },
    { path: 'merchant/addoredit/:id', component: MerchantAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'customer/list', component: CustomerListComponent, canActivate: [AuthGuard] },
    { path: 'customer/addoredit/:id', component: CustomerAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'hotkey/list', component: HotkeyListComponent, canActivate: [AuthGuard] },
    { path: 'outbox/list', component: OutboxComponent, canActivate: [AuthGuard] },
    { path: 'message-resend/list', component: MessageResendComponent, canActivate: [AuthGuard] },
    { path: 'agent-location/list', component: AgentLocationComponent, canActivate: [AuthGuard] },
    { path: 'customer-care/portal', component: CustomerCarePortalComponent, canActivate: [AuthGuard] },
    { path: 'customer/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'distributor/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'agent/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'dsr/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'merchant/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'chain-merchant/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'distributor-deposit/list', component: DistributorDepositListComponent, canActivate: [AuthGuard] },
    { path: 'distributor-deposit/addoredit/:id', component: DistributorDepositAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'customer-accounts-mapping/list', component: CustomerAccountsMappingListComponent, canActivate: [AuthGuard] },
    { path: 'customer-accounts-mapping/addoredit', component: CustomerAccountsMappingComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-entry', component: FundEntryComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/transfer', component: TransferComponent, canActivate: [AuthGuard] },
    { path: 'transaction/list', component: TransactionMasterComponent, canActivate: [AuthGuard] },
    { path: 'cbs-acc-remap/addoredit', component: CbsAccRemapComponent, canActivate: [AuthGuard] },
    { path: 'cbs-mapping-check/list', component: CbsMappingCheckComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-entry-actoac', component: FundEntryActoacComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-entry-actogl', component: FundEntryActoglComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-entry-gltoac', component: FundEntryGltoacComponent, canActivate: [AuthGuard] },
    { path: 'error/list', component: ErrorListComponent, canActivate: [AuthGuard] },
    { path: 'general-ledger/chart-of-accounts', component: ChartOfAccountsListComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-entry-commissiontomain', component: FundEntryCommissiontomainComponent, canActivate: [AuthGuard] },
    { path: 'rate-config/list', component: RateConfigListComponent, canActivate: [AuthGuard] },
    { path: 'rate-config/addoredit/:id', component: RateConfigAddOrEditComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-transfer-gltogl', component: FundTransferGltoglComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-transfer-actoac', component: FundTransferActoacComponent, canActivate: [AuthGuard] },
    { path: 'transfer/branch-cash-in', component: BranchCashInComponent, canActivate: [AuthGuard] },
    { path: 'transfer/branch-cash-out', component: BranchCashOutComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-transfer-actogl', component: FundTransferActoglComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/fund-transfer-gltoac', component: FundTransferGltoacComponent, canActivate: [AuthGuard] },
    { path: 'customer-request/list', component: CustomerRequestComponent, canActivate: [AuthGuard] },
    { path: 'environment/rate-setup', component: InterestTaxVatRateSetupComponent, canActivate: [AuthGuard] },
    { path: 'merchant-config/edit', component: MerchantConfigAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'transaction/robi-topup-stock-entry', component: RobiTopupStockEntryComponent, canActivate: [AuthGuard] },
    { path: 'transaction/bank-deposit-status', component: BankDepositStatusComponent, canActivate: [AuthGuard] },
    { path: 'badge-generator/generate', component: BadgeGeneratorComponent, canActivate: [AuthGuard] },
    { path: 'chain-merchant/list', component: ChainMerchantListComponent, canActivate: [AuthGuard] },
    { path: 'chain-merchant/addoredit/:id', component: ChainMerchantAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'error', component: ErrorComponent },
    { path: 'merchant-config/list', component: MerchantConfigListComponent, canActivate: [AuthGuard] },
    { path: 'merchant-config/addoredit/:id', component: MerchantConfigComponent, canActivate: [AuthGuard] },
    { path: 'report/config', component: ReportListComponent, canActivate: [AuthGuard] },
    { path: 'report/config/:id', component: ReportInfoComponent, canActivate: [AuthGuard] },
    { path: 'report/details/:id', component: ReportDetailsComponent },
    { path: 'report/list', component: ReportListComponent },
    { path: 'client-profile/search', component: SearchClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'distribution/distributor-replace', component: DistributorReplaceComponent, canActivate: [AuthGuard] },
    { path: 'merchant-user/addoredit/:id', component: MerchantUserComponent, canActivate: [AuthGuard] },
    { path: 'merchant-user/list', component: MerchantUserListComponent, canActivate: [AuthGuard] },
    { path: 'disbursement/disburse-process', component: DisburseProcessComponent, canActivate: [AuthGuard] },
    { path: 'disbursement-company/list', component: CompanyListComponent, canActivate: [AuthGuard] },
    { path: 'disbursement-company/addoredit/:id', component: CompanyAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'disbursement/company-disbursement-limit', component: CompanyDisbursementLimitComponent, canActivate: [AuthGuard] },
    { path: 'disbursement/disburse-amount-posting', component: DisburseAmountPostingComponent, canActivate: [AuthGuard] },
    { path: 'disbursement/disburse-posting', component: DisbursePostingComponent, canActivate: [AuthGuard] },
    { path: 'enterprise/list', component: EnterpriseListComponent, canActivate: [AuthGuard] },    
    { path: 'enterprise/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'enterprise/addoredit/:id', component: EnterpriseAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'process/eod', component: EodComponent, canActivate: [AuthGuard] },
    { path: 'client-work/search', component: ClientWorkTabComponent, canActivate: [AuthGuard] },
    { path: 'distribution/bulk-upload', component: BulkUploadComponent, canActivate: [AuthGuard] },
    { path: 'audit/audit-trail/list', component: AuditTrailComponent, canActivate: [AuthGuard] },
    { path: 'audit/audit-trail-dtl/:id', component: AuditTrailDtlComponent, canActivate: [AuthGuard] },
    { path: 'distribution/agent-replace', component: AgentReplaceComponent, canActivate: [AuthGuard] },
    { path: 'transaction/blink-topup-stock-entry', component: BlinkTopupStockEntryComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/commission-entry', component: CommissionEntryComponent, canActivate: [AuthGuard] },
    { path: 'transaction/airtel-topup-stock-entry', component: AirtelTopupStockEntryComponent, canActivate: [AuthGuard] },
    { path: 'fund-transfer/commission-approval', component: CommissionApprovalComponent, canActivate: [AuthGuard] },
    { path: 'transaction/nid-payment-collection', component: NidPaymentCollectionComponent, canActivate: [AuthGuard] },
    { path: 'transaction/bill-collection-common/:id', component: BillCollectionCommonComponent, canActivate: [AuthGuard] },
    { path: 'disbursement-user/list', component: DisbursementUserListComponent, canActivate: [AuthGuard] },
    { path: 'disbursement-user/add-edit/:id', component: DisbursementUserAddEditComponent, canActivate: [AuthGuard] },
    { path: 'chck-status', component: ChngStatusComponent, canActivate: [AuthGuard] },
    { path: 'retail/list', component: CustRetailListComponent, canActivate: [AuthGuard] },
    { path: 'retail/addoredit/:id', component: CustRetailComponent, canActivate: [AuthGuard] },
    { path: 'retail/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'status/list', component: StatusListComponent, canActivate: [AuthGuard] },
    { path: 'status/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'b2b/list', component: B2bListComponent, canActivate: [AuthGuard] },
    { path: 'b2b/addoredit/:id', component: B2bAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'b2b/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'client/dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
    { path: 'b2bdsr/list', component: B2bDsrListComponent, canActivate: [AuthGuard] },
    { path: 'b2bdsr/addoredit/:id', component: B2bDsrAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'b2bdsr/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'transaction/ttalk-topup-stock-entry', component: TtalkTopupStockEntryComponent, canActivate: [AuthGuard] },
    { path: 'b2bretail/list', component: B2bRetailListComponent, canActivate: [AuthGuard] },
    { path: 'b2bretail/addoredit/:id', component: B2bRetailAddoreditComponent, canActivate: [AuthGuard] },
    { path: 'b2bretail/details/:id', component: ClientProfileComponent, canActivate: [AuthGuard] },
    { path: 'transaction/download-receipt', component: DownloadReceiptComponent, canActivate: [AuthGuard] },
    { path: 'commission-convertion/list', component: CommissionConvertionListComponent, canActivate: [AuthGuard] },
    { path: 'commission-convertion/addoredit/:id', component: CommissionConvertionAddoreditComponent, canActivate: [AuthGuard] },

    {
        path: "",
        pathMatch: "full",
        redirectTo: "home"
    },
    { path: '**', redirectTo: 'home' }
];

export const routing = RouterModule.forRoot(appRoutes, {
    useHash: true,
    enableTracing: false,
    scrollPositionRestoration: 'enabled'
});