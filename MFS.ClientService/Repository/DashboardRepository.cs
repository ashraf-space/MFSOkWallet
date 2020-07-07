using Dapper;
using MFS.ClientService.Models.Views;
using OneMFS.SharedResources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MFS.ClientService.Repository
{
    public interface IDashboardRepository : IBaseRepository<DashboardViewModel>
    {
        Task<object> GetDataForDashboard();
        object GetGlobalSearchResult(string option, string criteria, string filter);
    }

    public class DashboardRepository : BaseRepository<DashboardViewModel>, IDashboardRepository
    {
        public async Task<object> GetDataForDashboard()
        {
            try
            {

                using (var connection = this.GetConnection())
                {
                    DashboardViewModel model = new DashboardViewModel();

                    string totalCountSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR,
                count(r.mphone) as Total, count(mon.mphone) as clientThisMonth,
                count(year.mphone) as clientThisYear from reginfo r
                LEFT JOIN RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D'
                LEFT JOIN RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C'
                LEFT JOIN RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A'
                LEFT JOIN RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M'
                LEFT JOIN RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R'
                LEFT JOIN REGInFO mon on mon.mphone = r.mphone and to_char(mon.Autho_DATE,'MM') = to_CHAR(SYSDATE,'MM') and to_char(mon.Autho_DATE,'YYYY') = to_CHAR(SYSDATE,'YYYY')
                LEFT JOIN REGInFO year on year.mphone = r.mphone and to_char(year.Autho_date,'YYYY') = to_CHAR(SYSDATE,'YYYY')
                where r.reg_Status = 'P'";

                    string clientCountByYearSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR from reginfo r
                LEFT JOIN RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D'
                LEFT JOIN RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C'
                LEFT JOIN RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A'
                LEFT JOIN RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M'
                LEFT JOIN RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R'
                where to_char(r.Autho_DATE,'YYYY') = to_CHAR(SYSDATE,'YYYY')";

                    string clientCountByMonthSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR, count(r.mphone) from reginfo r
                LEFT JOIN RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D'
                LEFT JOIN RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C'
                LEFT JOIN RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A'
                LEFT JOIN RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M'
                LEFT JOIN RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R'
                where to_char(r.Autho_DATE,'MM') = to_CHAR(SYSDATE,'MM') and to_char(r.Autho_DATE,'YYYY') = to_CHAR(SYSDATE,'YYYY')";

                    string pendingClientCountSQL = @"select count(dist.mphone) as distributor, count(agent.mphone) as Agent, count(cust.mphone) as Customer,
                count(merchant.mphone) as Merchant, count(dsr.mphone) as DSR, count(r.mphone) as Total from reginfo r
                LEFT JOIN RegInfo dist on dist.Mphone = r.mphone and dist.Cat_ID='D'
                LEFT JOIN RegInfo cust on cust.Mphone = r.mphone and cust.Cat_ID='C'
                LEFT JOIN RegInfo agent on agent.Mphone = r.mphone and agent.Cat_ID='A'
                LEFT JOIN RegInfo merchant on merchant.Mphone = r.mphone and merchant.Cat_ID='M'
                LEFT JOIN RegInfo dsr on dsr.Mphone = r.mphone and dsr.Cat_ID='R'
                where r.reg_status = 'L'";

                    string transactionByMonth = @"select to_char(a.Trans_Date, 'MON') as Month, ROUND(sum(a.Pay_Amt)/100000,2) as transaction
                from GL_TRANS_MST a
                where to_char(a.Trans_Date, 'YYYY') = to_char(sysdate, 'YYYY')
                group by to_char(a.Trans_Date, 'MM') ,to_char(a.Trans_Date, 'MON') 
                order by to_char(a.Trans_Date, 'MM')";

                    string transactionByYear = @"select to_char(Trans_Date, 'YYYY') as Year, ROUND(sum(Pay_Amt)/100000,2) as transaction
                from GL_TRANS_MST
                group by to_char(Trans_Date, 'YYYY') 
                order by 1";

                    string totalAssetSQL = "select Round(sum(DR_Amt-CR_amt),2) as asset from gl_trans_dtl where acc_TYPE = 'A'";

                    model.TotalClientCount = await connection.QueryFirstOrDefaultAsync(totalCountSQL);
                    model.TransactionByMonth = await connection.QueryAsync(transactionByMonth);
                    model.TransactionByYear = await connection.QueryAsync(transactionByYear);
                    model.TotalAsset = await connection.QueryFirstOrDefaultAsync(totalAssetSQL);
                    model.ClientCountByMonth = await connection.QueryFirstOrDefaultAsync(clientCountByMonthSQL);
                    model.ClientCountByYear = await connection.QueryFirstOrDefaultAsync(clientCountByYearSQL);
                    model.PendingClientCount = await connection.QueryFirstOrDefaultAsync(pendingClientCountSQL);

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
                                query = "Select Trans_no, Trans_date, Trans_From, trans_To,ROUND(pay_amt,3) as pay_amount from gl_trans_mst where Trans_From like '%" + filter + "%' or trans_To like '%" + filter + "%' order by trans_date desc ";
                            else
                                query = "Select Trans_no, Trans_date, Trans_From, trans_To,ROUND(pay_amt,3) as pay_amount from gl_trans_mst where Trans_no like '%" + filter + "%' order by trans_date desc ";
                            break;
                        case "Message":
                            query = "Select mphone,out_msg,out_time from outbox where " + convert.ToPascalCase(criteria) + " like '%" + filter + "%' and out_time is not null order by out_time desc";
                            break;
                        case "RegInfo":
                            query = "select mphone, Reg_date, ROUND(Balance_M,3) as Balance_M, Name, mphone as Details from regInfo where " + convert.ToPascalCase(criteria) + " like '%" + filter + "%' order by reg_date desc";
                            break;
                        default:
                            query = "select mphone, Reg_date, ROUND(Balance_M,3) as Balance_M, Name, mphone as Details from regInfo where " + convert.ToPascalCase(criteria) + " like '%" + filter + "%'  and cat_Id='" + option + "' order by reg_date desc";
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
    }

}
