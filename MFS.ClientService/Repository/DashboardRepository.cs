using Dapper;
using MFS.ClientService.Models.Views;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ClientService.Repository
{
    public interface IDashboardRepository : IBaseRepository<DashboardViewModel>
    {
        Task<object> GetDataForDashboard();
        object GetGlobalSearchResult(string option, string criteria, string filter);
        object GetBillCollectionMenus(int userId);
    }

    public class DashboardRepository : BaseRepository<DashboardViewModel>, IDashboardRepository
    {

        private static string dbUser;
        public DashboardRepository(MainDbUser objMainDbUser)
        {
            dbUser = objMainDbUser.DbUser;
        }
        public async Task<object> GetDataForDashboard()
        {
            try
            {

                using (var connection = this.GetConnection())
                {
                    DashboardViewModel model = new DashboardViewModel();

                    string totalCountSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                count(merchant.mphone) as Merchant,count(merOnline.mphone) as MerchantOnline,count(merOffline.mphone) as MerchantOffline,  count(dsr.mphone) as DSR,COUNT(MMS.MPHONE) AS TotalMMS, COUNT(EMS.MPHONE) AS TotalEMS,
                count(r.mphone) as Total, count(mon.mphone) as clientThisMonth,
                count(year.mphone) as clientThisYear from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D' and dist.Status='A' LEFT JOIN " + dbUser + "RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C' and cust.Status in('A','D')  LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A' and agent.Status in('A','D')  LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M' and merchant.Status in ('A','D')   LEFT JOIN (select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category = 'E') merOnline on merOnline.Mphone = r.mphone LEFT JOIN (select r.mphone from " + dbUser + "reginfo r inner join one.merchant_config mc on r.mphone = mc.mphone where mc.category in ('C','M') and r.Status in('A','D')) merOffline on merOffline.Mphone = r.mphone  LEFT JOIN " + dbUser + "RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R'  LEFT JOIN " + dbUser + "REGINFO MMS ON MMS.Mphone = r.mphone and MMS.Cat_ID IN ('MMSC','MMSM') LEFT JOIN " + dbUser + "REGINFO EMS ON EMS.Mphone = r.mphone and EMS.Cat_ID IN('EMSM','EMSC') LEFT JOIN " + dbUser + "REGInFO mon on mon.mphone = r.mphone and to_char(mon.Autho_DATE,'MM') = to_CHAR(SYSDATE,'MM') and to_char(mon.Autho_DATE,'YYYY') = to_CHAR(SYSDATE,'YYYY') LEFT JOIN " + dbUser + "REGInFO year on year.mphone = r.mphone and to_char(year.Autho_date,'YYYY') = to_CHAR(SYSDATE,'YYYY') where r.reg_Status = 'P'";

                    string clientCountByYearSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                    count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D' LEFT JOIN " + dbUser + "RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C' LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A' LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M' LEFT JOIN " + dbUser + "RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R' where to_char(r.Autho_DATE,'YYYY') = to_CHAR(SYSDATE,'YYYY')";

                    string clientCountByMonthSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                    count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR, count(r.mphone) from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D' LEFT JOIN " + dbUser + "RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C' LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A' LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M' LEFT JOIN " + dbUser + "RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R' where to_char(r.Autho_DATE,'MM') = to_CHAR(SYSDATE,'MM') and to_char(r.Autho_DATE,'YYYY') = to_CHAR(SYSDATE,'YYYY')";

                    string pendingClientCountSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR, count(r.mphone) as Total from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D' LEFT JOIN " + dbUser + "RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C' LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A' LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M' LEFT JOIN " + dbUser + "RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R' where r.reg_status = 'L'";

                    string transactionByMonth = @"select to_char(a.Trans_Date, 'MON') as Month, ROUND(sum(a.Pay_Amt)/100000,2) as transaction
                from " + dbUser + "GL_TRANS_MST a where to_char(a.Trans_Date, 'YYYY') = to_char(sysdate, 'YYYY') group by to_char(a.Trans_Date, 'MM') ,to_char(a.Trans_Date, 'MON') order by to_char(a.Trans_Date, 'MM')";

                    string transactionByYear = @"select to_char(Trans_Date, 'YYYY') as Year, ROUND(sum(Msg_Amt)/100000,2) as transaction
                    from " + dbUser + "GL_TRANS_MST WHERE (From_Cat_Id='C' OR To_Cat_Id='C' OR (From_Cat_Id in ('A', 'BD', 'BR', 'BA', 'R', 'GPAY', 'GP', 'PR', 'BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM one.REGINFO WHERE MPHONE IN(SELECT MPHONE FROM ONE.MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and From_Cat_Id<>'S' AND To_Cat_Id<>'S' and trans_ref_no is null  group by to_char(Trans_Date, 'YYYY') order by 1";

                    string totalAssetSQL = "select Round(sum(DR_Amt-CR_amt),2) as asset from " + dbUser + "gl_trans_dtl where acc_TYPE = 'A'";

                    string totalTransactionNumber = "Select Count(*) as TotalTransactionNumber from " + dbUser + "Gl_Trans_Mst where  Trans_no not in ('0','''','null')";



                    //string transTredQuery = "select ('Up to Dec ' || (EXTRACT(YEAR  FROM  SYSDATE)-1)) as Caption, ROUND(sum(Pay_Amt)/1000000,2) as TransactionAmt from one.GL_TRANS_MST WHERE EXTRACT(YEAR FROM  Trans_Date) < EXTRACT(YEAR FROM  SYSDATE)";
                    string transTredQuery = "select ('Up to Dec ' || (EXTRACT(YEAR  FROM  SYSDATE)-1)) as Caption, ROUND(sum(Msg_Amt)/1000000,2) as TransactionAmt from " + dbUser + "GL_TRANS_MST WHERE (From_Cat_Id='C' OR To_Cat_Id='C' OR (From_Cat_Id in ('A','BD','BR','BA','R','GPAY','GP','PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM " + dbUser + "REGINFO WHERE MPHONE IN(SELECT MPHONE FROM ONE.MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and From_Cat_Id<>'S' AND To_Cat_Id<>'S' and trans_ref_no is null and EXTRACT(YEAR FROM  Trans_Date) < EXTRACT(YEAR FROM  SYSDATE)";

                    string barQuery = "Select ('Up to Dec ' || (EXTRACT(YEAR  FROM  SYSDATE)-1)) as Caption,count(agent.mphone) as Agent,count(cus.mphone) as Customer , count(dist.mphone) as distributor,count(merchant.mphone) as Merchant,count(merOnline.mphone) as MerchantOnline,count(merOffline.mphone) as MerchantOffline from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo cus on cus.Mphone = r.mphone and cus.Cat_ID = 'C'  and cus.Status in('A','D')  LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID = 'A'  and agent.Status in('A','D')  LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID = 'D' and dist.Status='A'   LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID = 'M' and merchant.Status in ('A','D')      LEFT JOIN (select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category = 'E') merOnline on merOnline.Mphone = r.mphone LEFT JOIN(select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category in ('C','M')  and r.Status in('A','D') ) merOffline on merOffline.Mphone = r.mphone  WHERE EXTRACT(YEAR FROM  r.reg_date) < EXTRACT(YEAR FROM  SYSDATE) and r.reg_Status = 'P'";

                    DateTime now = DateTime.Now;
                    int year = now.Year;
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime();
                    string captionName = null;
                    for (int month = 1; month <= now.Month; month++)
                    {
                        startDate = new DateTime(year, month, 1);
                        endDate = startDate.AddMonths(1).AddDays(-1);
                        captionName = startDate.ToString("MMM") + "-" + (year % 100);
                        if (endDate > now)
                        {
                            endDate = now;
                            captionName = "1-" + endDate.Day.ToString() + startDate.ToString("MMM") + "-" + (year % 100);
                        }
                        transTredQuery = transTredQuery + " Union all select '" + captionName + "' as Caption, ROUND(sum(Msg_Amt)/1000000,2) as TransactionAmt from " + dbUser + "GL_TRANS_MST WHERE (From_Cat_Id='C' OR To_Cat_Id='C' OR (From_Cat_Id in ('A','BD','BR','BA','R','GPAY','GP','PR','BP') and (TO_CAT_ID IN(SELECT DISTINCT CAT_ID FROM ONE.REGINFO WHERE MPHONE IN(SELECT MPHONE FROM " + dbUser + "MERCHANT_CONFIG)) OR To_Cat_Id in ('BD', 'BR', 'BA', 'ATM')))) and From_Cat_Id<>'S' AND To_Cat_Id<>'S'  and trans_ref_no is null and trunc(Trans_Date) between To_Date('" + startDate.ToString("dd-MM-yyyy") + "','DD-MM-RRRR') and To_Date('" + endDate.ToString("dd-MM-yyyy") + "','DD-MM-RRRR')" + "";

                        //barQuery = barQuery + " Union all select '" + captionName + "' as Caption, count(agent.mphone) as Agent,count(r.mphone) as Customer , count(dist.mphone) as distributor,count(merchant.mphone) as Merchant,count(merOnline.mphone) as MerchantOnline,count(merOffline.mphone) as MerchantOffline  from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID = 'A' LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID = 'D' LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID = 'M' LEFT JOIN (select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category = 'E') merOnline on merOnline.Mphone = r.mphone LEFT JOIN(select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category in ('C','M')) merOffline on merOffline.Mphone = r.mphone WHERE r.reg_date between To_Date('" + startDate.ToShortDateString() + "','MM-DD-RRRR') and To_Date('" + endDate.ToShortDateString() + "','MM-DD-RRRR') and r.reg_Status = 'P'" + "";
                        barQuery = barQuery + " Union all select '" + captionName + "' as Caption, count(agent.mphone) as Agent,count(cus.mphone) as Customer , count(dist.mphone) as distributor,count(merchant.mphone) as Merchant,count(merOnline.mphone) as MerchantOnline,count(merOffline.mphone) as MerchantOffline  from " + dbUser + "reginfo r LEFT JOIN " + dbUser + "RegInfo cus on cus.Mphone = r.mphone and cus.Cat_ID = 'C'  and cus.Status in('A','D') LEFT JOIN " + dbUser + "RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID = 'A'  and agent.Status in('A','D') LEFT JOIN " + dbUser + "RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID = 'D'  and dist.Status='A'   LEFT JOIN " + dbUser + "RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID = 'M'  and merchant.Status in ('A','D')   LEFT JOIN (select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category = 'E') merOnline on merOnline.Mphone = r.mphone LEFT JOIN (select r.mphone from " + dbUser + "reginfo r inner join " + dbUser + "merchant_config mc on r.mphone = mc.mphone where mc.category in ('C','M')  and r.Status in('A','D') ) merOffline on merOffline.Mphone = r.mphone WHERE trunc(r.reg_date) <= To_Date('" + endDate.ToString("dd-MM-yyyy") + "','DD-MM-RRRR') and r.reg_Status = 'P'" + "";
                    }





                    model.TotalClientCount = await connection.QueryFirstOrDefaultAsync(totalCountSQL);
                    model.TransactionByMonth = await connection.QueryAsync(transactionByMonth);
                    model.TransactionByYear = await connection.QueryAsync(transactionByYear);
                    model.TotalAsset = await connection.QueryFirstOrDefaultAsync(totalAssetSQL);
                    model.ClientCountByMonth = await connection.QueryFirstOrDefaultAsync(clientCountByMonthSQL);
                    model.ClientCountByYear = await connection.QueryFirstOrDefaultAsync(clientCountByYearSQL);
                    model.PendingClientCount = await connection.QueryFirstOrDefaultAsync(pendingClientCountSQL);
                    model.TotalTransactionNumber = await connection.QueryFirstOrDefaultAsync(totalTransactionNumber);


                    model.ListTransactionTrend = await connection.QueryAsync(transTredQuery);
                    model.DynamicClientCount = await connection.QueryAsync(barQuery);

                    this.CloseConnection(connection);

                    return model;
                }


            }
            catch (Exception e)
            {

                throw;
            }
        }

        public object GetGlobalSearchResult(string option, string criteria, string filter)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    TextCaseConversion convert = new TextCaseConversion();
                    string query;
                    switch (option)
                    {
                        case "Transaction":
                            if (criteria == "Mphone")
                                query = "Select Trans_no, Trans_date, Trans_From, trans_To,ROUND(pay_amt,3) as pay_amount from " + dbUser + "gl_trans_mst where Trans_From like '%" + filter + "%' or trans_To like '%" + filter + "%' order by trans_date desc ";
                            else
                                query = "Select Trans_no, Trans_date, Trans_From, trans_To,ROUND(pay_amt,3) as pay_amount from " + dbUser + "gl_trans_mst where Trans_no like '%" + filter + "%' order by trans_date desc ";
                            break;
                        case "Message":
                            query = "Select mphone,out_msg,out_time from " + dbUser + "outbox where " + convert.ToPascalCase(criteria) + " like '%" + filter + "%' and out_time is not null order by out_time desc";
                            break;
                        case "RegInfo":
                            query = "select mphone, Reg_date, ROUND(Balance_M,3) as Balance_M, Name, mphone as Details from " + dbUser + "regInfo where " + convert.ToPascalCase(criteria) + " like '%" + filter + "%' order by reg_date desc";
                            break;
                        default:
                            query = "select mphone, Reg_date, ROUND(Balance_M,3) as Balance_M, Name, mphone as Details from " + dbUser + "regInfo where " + convert.ToPascalCase(criteria) + " like '%" + filter + "%'  and cat_Id='" + option + "' order by reg_date desc";
                            break;
                    }

                    var result = connection.Query(query);
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public object GetBillCollectionMenus(int userId)
        {
            try
            {
                using (var connection = this.GetConnection())
                {
                    string query;

                    //query = "Select category_id as CategoryId,alias as FeatureName,url from " + dbUser + "feature  where Category_id in (31,32,33,34) or Alias in ('Branch Cash In ( Deposit )', 'Branch Cash Out ( Withdrawal )') order by category_id , order_no";
                    query = "Select f.category_id as CategoryId,f.alias as FeatureName,f.url from " + dbUser + "feature f INNER JOIN " + dbUser + "Permission p on p.feature_id = f.id INNER JOIN " + dbUser + "feature_category fc on fc.id = f.category_id where (f.Category_id in (31, 32, 33, 34) or f.Alias in ('Branch Cash In ( Deposit )', 'Branch Cash Out ( Withdrawal )')) and p.role_id = (select role_id from " + dbUser + "application_user where id = " + userId + ") and p.is_view_permitted = 'y' order by f.category_id , f.order_no";


                    var result = connection.Query(query);
                    this.CloseConnection(connection);
                    return result;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
