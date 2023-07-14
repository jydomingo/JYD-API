using Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using JYD.Controllers;
using JYD.DataContext.DbModel;
using System;

namespace JYD.DataContext.Repository
{
    public class ProcedureRepository : IProcedureRepository
    {

        private readonly AppDbContext _DbContext;
        public ProcedureRepository(AppDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<UserDB?> GetDBs(string user_id, string password_hash, string temp_password)
        {
            List<UserDB>? user = await _DbContext.User.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getuser {user_id},{password_hash},{temp_password}").ToListAsync();

            if (user == null)
                return null;

            return user.FirstOrDefault();
        }

        public async Task<Agency_Ref?> GetAgency_Ref(string agency_code)
        {
            List<Agency_Ref>? agencyref = await _DbContext.Agencyref.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getagencytype {agency_code}").ToListAsync();

            if (agencyref == null)
                return null;

            return agencyref.FirstOrDefault();
        }

        public async Task<UserAuditDB?> GetUserAudit(string User_id)
        {
            List<UserAuditDB>? userauditdb = await _DbContext.UserAudit.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getuserhistoryperid {User_id}").ToListAsync();

            if (userauditdb == null)
                return null;

            return userauditdb.FirstOrDefault();
        }

        public async Task<int> UpdateUserPassword(string user_id, string password_hash)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_updateuserpassword {user_id},{password_hash}");
            return affectedRows;
        }

        public async Task<Retrieve_UserdtlDB?> GetUserDetail(string User_id)
        {
            List<Retrieve_UserdtlDB>? userdetail = await _DbContext.UserdtlDBs.FromSqlInterpolated($"EXEC dbo.sproc_pmf_retrieve_userdtl {User_id}").ToListAsync();

            if (userdetail == null)
                return null;

            return userdetail.FirstOrDefault();
        }

        public async Task<int> ForgotUserPassword(string user_id, string temp_password, string email_address)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_forgotuserpassword {user_id},{temp_password},{email_address}");
            return affectedRows;
        }

        public async Task<int> UpdateSession(string user_id, string session_flag)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_updatesessionflag {user_id},{session_flag}");
            return affectedRows;
        }

        public async Task<List<DealerDB>?> GetListOfDealer(string site_code)
        {
            List<DealerDB>? dealers = await _DbContext.Dealer.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getdealerperdo {site_code}").ToListAsync();

            if (dealers == null)
                return null;

            return dealers;
        }
        public async Task<List<ItemDB>?> GetListOfItem(string dealer_code)
        {
            List<ItemDB>? items = await _DbContext.Item.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getitemperdealer {dealer_code}").ToListAsync();

            if (items == null)
                return null;

            return items;
        }

        public async Task<List<AllocationDtlDB>?> GetAllocationDtlperNo(string control_no)
        {
            var AllocationDtl = await _DbContext.AllocationDtls.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getallocdtlperno {control_no}").ToListAsync();
            return AllocationDtl;
        }

        public async Task<Validate?> GetValidate(string item_code, string range_fr, string range_to)
        {
            var valid = await _DbContext.Validates.FromSqlInterpolated($"EXEC dbo.sproc_pmf_ValidatePlate {item_code},{range_fr},{range_to}").ToListAsync();
            if (valid == null)
                return null;
            return valid.FirstOrDefault();
        }

        public async Task<List<CheckDuplicate>?> GetCheckDuplicate(string item_code, string range_fr, string range_to)
        {
            var cd = await _DbContext.Duplicates.FromSqlInterpolated($"EXEC dbo.sproc_pmf_checkduplicate {item_code},{range_fr},{range_to}").ToListAsync();
            if (cd == null)
                return null;
            return cd;
        }

        public async Task<Exploded?> GetExploded(string control_no, string ref_no, string site_code, string agency_code, string item_code, string range_fr, string range_to, string status_code, string user_id)
        {
            var ex = await _DbContext.Explodeds.FromSqlInterpolated($"EXEC dbo.sproc_pmf_createallocationhdr {control_no},{ref_no},{site_code},{agency_code},{item_code},{range_fr},{range_to},{status_code},{user_id}").ToListAsync();
            if (ex == null)
                return null;
            return ex.FirstOrDefault();
        }

        public async Task<int> DeleteAllocationDtl(string control_no, int seq_no, string user_id)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_deleteallocdtl {control_no},{seq_no}, {user_id}");
            return affectedRows;
        }

        public async Task<NewAllocationDB?> ProcessNewAllocation(string control_no, string user_id)
        {
            var AllocationDtl = await _DbContext.NewAllocation.FromSqlInterpolated($"EXEC dbo.sproc_pmf_processnewallocation {control_no},{user_id}").ToListAsync();
            if (AllocationDtl == null)
                return null;
            return AllocationDtl.FirstOrDefault();
        }

        public async Task<int> DelAllocHDRDetails(string control_no, int seq_no)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_del_allocation_hrd_dtls {control_no},{seq_no}");
            return affectedRows;
        }

        public async Task<int> CancellAllocation(string control_no)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_CancelAllocation {control_no}");
            return affectedRows;
        }

        public async Task<int> RemoveTMAllocation(string user_id)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_RemoveAllTMAllocByUserID {user_id}");
            return affectedRows;
        }

        public async Task<int> SaveNotification(string id, string userid, string agency_code, string refno, string messasge)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_createnotification {id}, {userid}, {agency_code}, {refno}, {messasge}");
            return affectedRows;
        }

        public async Task<List<Notification>?> GetNotification(string userid)
        {
            List<Notification>? notification = await _DbContext.Notifications.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getnotification {userid}").ToListAsync();
            return notification;
        }

        public async Task<Notification?> GetNotificationById(string Id)
        {
            List<Notification>? notification = await _DbContext.Notifications.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getnotificationbyid {Id}").ToListAsync();
            return notification.FirstOrDefault();
        }

        public async Task<int> ReadNotification(string id, string userid)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_readnotification {id}, {userid}");
            return affectedRows;
        }

        public async Task<List<Status_Ref>?> Status_Ref(string table_name)
        {
            List<Status_Ref>? statusref = await _DbContext.Status_Refs.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getstatuslistpertable {table_name}").ToListAsync();

            if (statusref == null)
                return null;

            return statusref.ToList();
        }

        public async Task<List<ReportbyRefNo>?> ReportbyRefNo(string dealer, string refno)
        {
            List<ReportbyRefNo>? refNos = await _DbContext.RepRefno.FromSqlInterpolated($"EXEC dbo.sproc_pmf_reportbyreferenceno {dealer},{refno}").ToListAsync();
            if (refNos == null)
                return null;
            return refNos;
        }

        public async Task<List<Allocation_Inquiry>?> Allocation_Inquiries(string site_code, string agency_code, string ref_no, string item_code, string status, DateTime date_from, DateTime date_to)
        {
            List<Allocation_Inquiry>? alloc_detail = await _DbContext.Alloc_Inquiries.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getallocationinquiry {site_code},{agency_code},{ref_no},{item_code},{status},{date_from},{date_to}").ToListAsync();
            if (alloc_detail == null)
                return null;
            return alloc_detail;

        }

        public async Task<int> ProcessAcceptance(string ref_no, string user_id)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_proccessacceptance {ref_no},{user_id}");
            return affectedRows;
        }

        public async Task<List<ReportbyPlateNo>?> ReportbyPlateNos(string plate_no, string site_code, string agency_code)
        {
            List<ReportbyPlateNo>? repbyplate = await _DbContext.RepPlateno.FromSqlInterpolated($"EXEC dbo.sproc_pmf_reportbyplateno {plate_no},{site_code},{agency_code}").ToListAsync();
            if (repbyplate == null) return null;
            return repbyplate;
        }

        public async Task<allochdr?> allochdr(string ref_no)
        {
            var ah = await _DbContext.GetAllochdrs.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getallocationhdr {ref_no}").ToListAsync();
            if (ah == null)
                return null;
            return ah.FirstOrDefault();
        }

        public async Task<int> UpdateAllocationDtl(string plate_no, string status_code, string engine_no, string chassis_no, DateTime date_reserved, string user_id)
        {
            int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_updateallocationdtl {plate_no}, {status_code}, {engine_no}, {chassis_no}, {date_reserved}, {user_id}");
            return affectedRows;
        }

        //public async Task<int> allex(string control_no, int seq_no, string plate_no, string user_id)
        //{
        //    int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_ins_allocation_except {control_no}, {seq_no}, {plate_no}, {user_id}");
        //    return affectedRows;
        //}

        public async Task<List<AllocationException>?> allex(string control_no, int seq_no, string plate_no, string user_id)
        {
            List<AllocationException>? plate_except = await _DbContext.allocEx.FromSqlInterpolated($"EXEC dbo.sproc_pmf_ins_plate_except {control_no}, {seq_no}, {plate_no}, {user_id}").ToListAsync();
            if (plate_except == null) return null;
            return plate_except;
        }

        public async Task<List<AllocationException>?> delallex(string control_no, int seq_no, string plate_no, string user_id)
        {
            List<AllocationException>? allocexp = await _DbContext.delallocEx.FromSqlInterpolated($"EXEC dbo.sproc_pmf_del_allocation_except {control_no}, {seq_no}, {plate_no}, {user_id}").ToListAsync();
            if (allocexp == null) return null;
            return allocexp;
        }

        public async Task<List<AllocationException>?> GetAlloctionException(string control_no, int seq_no)
        {
            List<AllocationException>? alloc_excp = await _DbContext.AllocationExceptions.FromSqlInterpolated($"EXEC dbo.sproc_pmf_getallocationexception {control_no}, {seq_no}").ToListAsync();

            if (alloc_excp == null)
                return null;

            return alloc_excp.ToList();
        }

        public async Task<List<Validate_PlateEngChass>?> validate_PEC(string agency_code, string plate_no, string status_code, string engine_no, string chassis_no, DateTime date_reserved, string item_code,string old_values, string user_id )
        {
            List<Validate_PlateEngChass>? vpec = await _DbContext.validatePEC.FromSqlInterpolated($"EXEC dbo.sproc_pmf_validateupdateMV {agency_code}, {plate_no}, {status_code},{engine_no}, {chassis_no},{date_reserved}, {item_code},{old_values},{user_id}").ToListAsync();
            if (vpec == null)
                return null;
            return vpec.ToList();
        }

        public async Task<List<GetPlateFIFO>?> plateFIFOs(string agency_code, string item_code)
        {
            List<GetPlateFIFO>? fifo = await _DbContext.fifo.FromSqlInterpolated($"EXEC dbo.sproc_pmf_RetrievePlateFIFO {agency_code}, {item_code}").ToListAsync();

            if (fifo == null)
                return null;

            return fifo.ToList();
        }

        public async Task<List<PlateAllocationReport>?> PlateAllocationRep(string site_code, string agency_code, string ref_no, string item_code, string status, DateTime date_from, DateTime date_to)
         {
             List<PlateAllocationReport>? plate_report = await _DbContext.PlateAllocationReport.FromSqlInterpolated($"EXEC dbo.sproc_pmf_rep_allocationlist {site_code}, {agency_code}, {ref_no}, {item_code}, {status}, {date_from}, {date_to}").ToListAsync();

             if (plate_report == null)
                 return null;

             return plate_report;
         }

        public async Task<List<PlateAllocationReportDtl>?> PlateAllocationRepDtl(string ref_no, string status)
        {
            List<PlateAllocationReportDtl>? plate_reportdtl = await _DbContext.PlateAllocationReportDtl.FromSqlInterpolated($"EXEC dbo.sproc_pmf_rep_allocationlistdtl {ref_no}, {status}").ToListAsync();
            
            if (plate_reportdtl == null)
                return null;

            return plate_reportdtl;
        }

        public async Task<List<RevertPlateAllocation>?> RevertPlateAlloc(string ref_no, string user_id)
        {
            List<RevertPlateAllocation>? revert_alloc = await _DbContext.RevertPlateAllocation.FromSqlInterpolated($"EXEC sproc_pmf_processrevertalloc {ref_no}, {user_id}").ToListAsync();

            if (revert_alloc == null)
                return null;

            return revert_alloc;
        }

        public async Task<int> DeletePlateAssignment(string plateno, string user_id)
        {
           int affectedRows = await _DbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC dbo.sproc_pmf_del_assignment {plateno},{user_id}");
           return affectedRows;
        }

     
    }
}
